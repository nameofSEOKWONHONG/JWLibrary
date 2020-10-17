using JWLibrary.DB.MYSQL.Module;
using MySqlConnector;

namespace JWLibrary.DB.MYSQL {

    public class DataHelperFactory {

        #region MySql Data Helper

        public static MySqlDacHelper CreateMySqlDacHelper(string connectionStringName) {
            return new MySqlDacHelper(connectionStringName);
        }

        public static MySqlDacHelper CreateMySqlDacHelper(int commandTimeout, string connectionStringName) {
            return new MySqlDacHelper(commandTimeout, connectionStringName);
        }

        public static MySqlDacHelper CreateMySqlDacHelper(MySqlConnection connection) {
            return new MySqlDacHelper(connection);
        }

        #endregion MySql Data Helper
    }
}