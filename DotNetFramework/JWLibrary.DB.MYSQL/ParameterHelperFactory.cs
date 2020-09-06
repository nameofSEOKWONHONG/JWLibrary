using JWLibrary.DB.MYSQL.Module;
using System.Data;

namespace JWLibrary.DB.MYSQL {

    public class ParameterHelperFactory {

        #region MySql Parameter

        public static MySqlParameter CreateMySqlParameter(string parameterName, object value) {
            return (MySqlParameter)new MySqlParameterHelper().CreateParameter(parameterName, value);
        }

        public static MySqlParameter CreateMySqlParameter(string parameterName, object value, ParameterDirection direction) {
            return (MySqlParameter)new MySqlParameterHelper().CreateParameter(parameterName, value, direction);
        }

        public static MySqlParameter CreateMySqlParameter(string parameterName, object value, object dbType) {
            return (MySqlParameter)new MySqlParameterHelper().CreateParameter(parameterName, value, dbType);
        }

        public static MySqlParameter CreateMySqlParameter(string parameterName, object value, object dbType, ParameterDirection direction) {
            return (MySqlParameter)new MySqlParameterHelper().CreateParameter(parameterName, value, dbType, direction);
        }

        public static MySqlParameter CreateMySqlParameter(string parameterName, object value, object dbType, int size) {
            return (MySqlParameter)new MySqlParameterHelper().CreateParameter(parameterName, value, dbType, size);
        }

        public static MySqlParameter CreateMySqlParameter(string parameterName, object value, object dbType, int size, ParameterDirection direction) {
            return (MySqlParameter)new MySqlParameterHelper().CreateParameter(parameterName, value, dbType, size, direction);
        }

        #endregion MySql Parameter
    }
}