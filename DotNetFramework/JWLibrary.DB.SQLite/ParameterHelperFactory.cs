using JWLibrary.DB.SQLite.Module;

namespace GSCoreLibray.DB.MsSql {

    public class ParameterHelperFactory {

        #region MS-SQL Parameter

        public static SQLiteParameter CreateMsSqlParameter(string parameterName, object value) {
            return (SQLiteParameter)new SqliteParameterHelper().CreateParameter(parameterName, value);
        }

        public static SQLiteParameter CreateMsSqlParameter(string parameterName, object value, ParameterDirection direction) {
            return (SQLiteParameter)new SqliteParameterHelper().CreateParameter(parameterName, value, direction);
        }

        public static SQLiteParameter CreateMsSqlParameter(string parameterName, object value, object dbType) {
            return (SQLiteParameter)new SqliteParameterHelper().CreateParameter(parameterName, value, dbType);
        }

        public static SQLiteParameter CreateMsSqlParameter(string parameterName, object value, object dbType, ParameterDirection direction) {
            return (SQLiteParameter)new SqliteParameterHelper().CreateParameter(parameterName, value, dbType, direction);
        }

        public static SQLiteParameter CreateMsSqlParameter(string parameterName, object value, object dbType, int size) {
            return (SQLiteParameter)new SqliteParameterHelper().CreateParameter(parameterName, value, dbType, size);
        }

        public static SQLiteParameter CreateMsSqlParameter(string parameterName, object value, object dbType, int size, ParameterDirection direction) {
            return (SQLiteParameter)new SqliteParameterHelper().CreateParameter(parameterName, value, dbType, size, direction);
        }

        #endregion MS-SQL Parameter
    }
}