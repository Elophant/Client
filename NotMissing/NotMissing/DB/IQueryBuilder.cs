using System.Collections.Generic;
using System.Data;

namespace NotMissing.Db
{
    public interface IQueryBuilder
    {
        /// <summary>
        /// Construct sql query and return the string
        /// </summary>
        /// <param name="table">Table to constructor the query with</param>
        /// <returns>Sql query</returns>
        string CreateTableQuery(SqlTable table);
        string AlterTableQuery(SqlTable from, SqlTable to);
        /// <summary>
        /// Create the table or if it exists make sure the columns match. If they do not match it will alter the table.
        /// </summary>
        /// <param name="table">Table to create/alter</param>
        void CreateTable(SqlTable table);

        string DbTypeToString(DbType type, int? length);
        DbType StringToDbType(string type);

        /// <summary>
        /// Gets the columns from the table specified.
        /// </summary>
        /// <param name="table">Table to get the columns from. Only Table.Name is required.</param>
        /// <returns>List of columns</returns>
        List<SqlColumn> GetColumns(SqlTable table);
    }
}
