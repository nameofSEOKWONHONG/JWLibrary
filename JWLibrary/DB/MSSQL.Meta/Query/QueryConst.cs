namespace JWLibrary.DB.MSSQL.Meta.Query
{
    public class QueryConst
    {
        public static string SELECT_DATABASES()
        {
            return
                @"
                SELECT NAME AS DATABASE_NAME
                FROM MASTER..SYSDATABASES
                ORDER BY NAME ASC
                ";
        }

        public static string SELECT_TABLES(string databaseName)
        {
            string query =
                @"
                SELECT TABLE_NAME FROM {0}.INFORMATION_SCHEMA.TABLES
                ORDER BY TABLE_NAME ASC
                ";

            return query = string.Format(query, databaseName);
        }

        public static string SELECT_TABLE_COLUMS(string databaseName, string tableName)
        {
            string query =
				@"
SELECT A.TABLE_CATALOG, A.TABLE_NAME, A.COLUMN_NAME, A.DATA_TYPE, CASE WHEN ISNULL(B.COLUMN_NAME, '') = '' THEN 'N' ELSE 'Y' END AS PK, A.IS_NULLABLE
FROM {0}.INFORMATION_SCHEMA.COLUMNS A LEFT OUTER JOIN {0}.INFORMATION_SCHEMA.KEY_COLUMN_USAGE B
				                                      ON A.TABLE_CATALOG = B.TABLE_CATALOG AND A.TABLE_NAME = B.TABLE_NAME AND A.COLUMN_NAME = B.COLUMN_NAME
WHERE A.TABLE_CATALOG = '{0}'
AND A.TABLE_NAME = '{1}'                              
                ";

            return query = string.Format(query, databaseName, tableName);
        }

        public static string SELECT_EXSIST_TABLE(string databaseName, string tableName)
        {
            string query =
                @"
                SELECT COUNT(*) AS CNT
                FROM {0}.INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_CATALOG = '{0}'
                AND TABLE_NAME = '{1}'                
                ";

            return query = string.Format(query, databaseName, tableName);
        }

        public static string SELECT_TABLE_COLUM_INFO(string databaseName, string tableName, string columnName)
        {
            string query =
                @"
                SELECT TABLE_CATALOG, TABLE_NAME, COLUMN_NAME, DATA_TYPE, IS_NULLABLE
                FROM {0}.INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_CATALOG = '{0}'
                AND TABLE_NAME = '{1}'                
                AND COLUMN_NAME = '{2}'
                ";

            return query = string.Format(query, databaseName, tableName, columnName);
        }

        public static string SELECT_TABLE_COLUMN_PK(string databaseName, string tableName)
        {
            string query =
                @"
                SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, CONSTRAINT_NAME
                FROM {0}.INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE
                WHERE TABLE_NAME LIKE '%{1}%' AND CONSTRAINT_NAME LIKE 'PK%'
                ";
            return query = string.Format(query, databaseName, tableName);
        }
    }
}
