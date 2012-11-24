using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NotMissing.Db
{
    public class SqliteQueryCreator : IQueryBuilder
    {
        readonly IDbConnection database;
        public SqliteQueryCreator(IDbConnection conn)
        {
            database = conn;
        }

        public string CreateTableQuery(SqlTable table)
        {
            var columns = table.Columns.Select(c => "'{0}' {1} {2} {3} {4}".SFormat(c.Name, DbTypeToString(c.Type, c.Length), c.Primary ? "PRIMARY KEY" : "", c.AutoIncrement ? "AUTOINCREMENT" : "", c.NotNull ? "NOT NULL" : "", c.Unique ? "UNIQUE" : ""));
            return "CREATE TABLE '{0}' ({1})".SFormat(table.Name, string.Join(", ", columns.ToArray()));
        }
        static readonly Random rand = new Random();
        /// <summary>
        /// Alter a table from source to destination
        /// </summary>
        /// <param name="from">Must have name and column names. Column types are not required</param>
        /// <param name="to">Must have column names and column types.</param>
        /// <returns></returns>
        public string AlterTableQuery(SqlTable from, SqlTable to)
        {
            var rstr = rand.NextString(20);
            var alter = "ALTER TABLE '{0}' RENAME TO '{1}_{0}'".SFormat(from.Name, rstr);
            var create = CreateTableQuery(to);
            //combine all columns in the 'from' variable excluding ones that aren't in the 'to' variable.
            //exclude the ones that aren't in 'to' variable because if the column is deleted, why try to import the data?
            var insert = "INSERT INTO '{0}' ({1}) SELECT {1} FROM {2}_{0}".SFormat(from.Name, string.Join(", ", from.Columns.Where(c => to.Columns.Any(c2 => c2.Name == c.Name)).Select(c => c.Name).ToArray()), rstr);
            var drop = "DROP TABLE '{0}_{1}'".SFormat(rstr, from.Name);
            return "{0}; {1}; {2}; {3};".SFormat(alter, create, insert, drop);
            /*
                ALTER TABLE "main"."Bans" RENAME TO "oXHFcGcd04oXHFcGcd04_Bans"
                CREATE TABLE "main"."Bans" ("IP" TEXT PRIMARY KEY ,"Name" TEXT)
                INSERT INTO "main"."Bans" SELECT "IP","Name" FROM "main"."oXHFcGcd04oXHFcGcd04_Bans"
                DROP TABLE "main"."oXHFcGcd04oXHFcGcd04_Bans"
             * 
             */
        }

        static readonly Dictionary<DbType, string> TypesAsStrings = new Dictionary<DbType, string>
        {
            {DbType.String, "TEXT"},
            {DbType.Int32, "INTEGER"},
        };
        public string DbTypeToString(DbType type, int? length)
        {
            string ret;
            if (TypesAsStrings.TryGetValue(type, out ret))
                return ret;
            throw new NotImplementedException(Enum.GetName(typeof(DbType), type));
        }
        public DbType StringToDbType(string type)
        {
            var obj = TypesAsStrings.FirstOrDefault(kv => kv.Value == type);
            if (!string.IsNullOrEmpty(obj.Value))
                return obj.Key;
            throw new NotImplementedException(type);
        }

        public void CreateTable(SqlTable table)
        {
            var columns = GetColumns(table);
            if (columns.Count > 0)
            {
                if (!CompareColumns(table, columns))
                {
                    var from = new SqlTable(table.Name, columns);
                    database.Query(AlterTableQuery(from, table));
                }
            }
            else
            {
                database.Query(CreateTableQuery(table));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columns"></param>
        /// <returns>True if the columns match, otherwise false</returns>
        bool CompareColumns(SqlTable table, List<SqlColumn> columns)
        {
            if (table.Columns.Count != columns.Count)
                return false;

            foreach (var col in table.Columns)
            {
                var tcol = columns.FirstOrDefault(s => s.Name == col.Name);
                if (tcol == null)
                    return false;

                if (tcol.Type != col.Type || 
                    tcol.Primary != col.Primary || 
                    tcol.NotNull != col.NotNull)
                    return false;
            }
            return true;
        }

        public List<SqlColumn> GetColumns(SqlTable table)
        {
            var ret = new List<SqlColumn>();
            using (var reader = database.QueryReader("PRAGMA table_info({0})".SFormat(table.Name)))
            {
                while (reader.Read())
                    ret.Add(new SqlColumn(reader.Get<string>("name"), StringToDbType(reader.Get<string>("type"))) { NotNull = (reader.Get<int>("notnull") != 0), Primary = (reader.Get<int>("pk") != 0) });
            }
            return ret;
        }



    }
}