using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace JWLibrary.DB.MYSQL.Module {
	public class MySqlParameterHelper : IParameterHelper {
		#region IParameterHelper 멤버

		public DbParameter CreateParameter(string parameterName, object value) {
			return CreateParameter(parameterName, value, ParameterDirection.Input);
		}

		public DbParameter CreateParameter(string parameterName, object value, ParameterDirection direction) {
			MySqlParameter param = new MySqlParameter(parameterName, value);
			param.Direction = direction;
			return param;
		}

		public DbParameter CreateParameter(string parameterName, object value, object dbType) {
			return CreateParameter(parameterName, value, dbType, ParameterDirection.Input);
		}

		public DbParameter CreateParameter(string parameterName, object value, object dbType, ParameterDirection direction) {
			MySqlParameter param = new MySqlParameter(parameterName, value);

			if (dbType is MySqlDbType)
				param.MySqlDbType = (MySqlDbType)dbType;
			else
				param.DbType = (DbType)dbType;

			param.Direction = direction;

			return param;
		}

		public DbParameter CreateParameter(string parameterName, object value, object dbType, int size) {
			return CreateParameter(parameterName, value, dbType, size, ParameterDirection.Input);
		}

		public DbParameter CreateParameter(string parameterName, object value, object dbType, int size, ParameterDirection direction) {
			MySqlParameter param = new MySqlParameter(parameterName, value);

			if (dbType is MySqlDbType)
				param.MySqlDbType = (MySqlDbType)dbType;
			else
				param.DbType = (DbType)dbType;

			param.Size = size;
			param.Direction = direction;

			return param;
		}

		#endregion
	}
}
