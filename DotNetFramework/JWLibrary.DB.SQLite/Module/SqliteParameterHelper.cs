using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace JWLibrary.DB.SQLite.Module
{
    public class SqliteParameterHelper : IParameterHelper {
		#region IParameterHelper 멤버

		public DbParameter CreateParameter(string parameterName, object value) {
			return CreateParameter(parameterName, value, ParameterDirection.Input);
		}

		public DbParameter CreateParameter(string parameterName, object value, ParameterDirection direction) {
			SQLiteParameter param = new SQLiteParameter(parameterName, value);
			param.Direction = direction;
			return param;
		}

		public DbParameter CreateParameter(string parameterName, object value, object dbType) {
			return CreateParameter(parameterName, value, dbType, ParameterDirection.Input);
		}

		public DbParameter CreateParameter(string parameterName, object value, object dbType, ParameterDirection direction) {
			SQLiteParameter param = new SQLiteParameter(parameterName, value);

			param.DbType = (DbType)dbType;

			param.Direction = direction;

			return param;
		}

		public DbParameter CreateParameter(string parameterName, object value, object dbType, int size) {
			return CreateParameter(parameterName, value, dbType, size, ParameterDirection.Input);
		}

		public DbParameter CreateParameter(string parameterName, object value, object dbType, int size, ParameterDirection direction) {
			SQLiteParameter param = new SQLiteParameter(parameterName, value);

			param.DbType = (DbType)dbType;

			param.Size = size;
			param.Direction = direction;

			return param;
		}

		#endregion
	}
}
