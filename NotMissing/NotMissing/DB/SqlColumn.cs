
using System.Data;

namespace NotMissing.Db
{
    public class SqlColumn
    {
        //Required
        public string Name { get; set; }
        public DbType Type { get; set; }


        //Optional
        public bool Unique { get; set; }
        public bool Primary { get; set; }
        public bool AutoIncrement { get; set; }
        public bool NotNull { get; set; }
        public string DefaultValue { get; set; }

        /// <summary>
        /// Length of the data type, null = default
        /// </summary>
        public int? Length { get; set; }

        public SqlColumn(string name)
            : this(name, DbType.String, null)
        {
        }
        public SqlColumn(string name, DbType type)
            : this(name, type, null)
        {
        }
        public SqlColumn(string name, DbType type, int? length)
        {
            Name = name;
            Type = type;
            Length = length;
        }
    }
}
