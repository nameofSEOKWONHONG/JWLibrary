using JWLibrary.DB.MYSQL.Module;
using MySql.Data.MySqlClient;

namespace JWLibrary.DB.MYSQL {
	public class DataHelperFactory {
		#region MS-SQL Data Helper

		public static MySqlDacHelper CreateMySqlDacHelper(string connectionStringName) {
			return new MySqlDacHelper(connectionStringName);
		}

		public static MySqlDacHelper CreateMySqlDacHelper(int commandTimeout, string connectionStringName) {
			return new MySqlDacHelper(commandTimeout, connectionStringName);
		}

		public static MySqlDacHelper CreateMySqlDacHelper(MySqlConnection connection) {
			return new MySqlDacHelper(connection);
		}

		#endregion
	}
}
