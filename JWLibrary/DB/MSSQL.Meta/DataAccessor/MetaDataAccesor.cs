using JWLibrary.DB.MSSQL.Module;

namespace JWLibrary.DB.MSSQL.Meta.DataAccesor {
	public class MetaDataAccesor
    {
        public class DatabaseDAO
        {
            [MsSqlDataBinder("DATABASE_NAME")]
            public string DatabaseName { get; set; }
        }

        public class DatabaseTableDAO
        {
            [MsSqlDataBinder("TABLE_NAME")]
            public string TableName { get; set; }
        }

        public class DatabaseTableColumnDAO
        {
            [MsSqlDataBinder("TABLE_CATALOG")]
            public string TableCatalog { get; set; }

            [MsSqlDataBinder("TABLE_NAME")]
            public string TableName { get; set; }

            [MsSqlDataBinder("COLUMN_NAME")]
            public string ColumnName { get; set; }

            [MsSqlDataBinder("DATA_TYPE")]
            public string DataType { get; set; }

			[MsSqlDataBinder("PK")]
			public string PK { get; set; }

			[MsSqlDataBinder("IS_NULLABLE")]
			public string ISNULLABLE { get; set; }
        }

        public class DatabaseTableColumnPk : DatabaseTableColumnDAO
        {
            [MsSqlDataBinder("CONSTRAINT_NAME")]
            public string ConstraintName { get; set; }
        }
    }
}
