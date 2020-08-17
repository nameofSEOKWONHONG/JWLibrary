using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace JWLibrary.DB.MYSQL.Module {
	public class MySqlDacHelper : IDacHelper {
		#region Fields

		private static int _defaultCommandTimeout = 30;
		//private static string _defaultConnectionStringName = "";

		private int _commandTimeout = _defaultCommandTimeout;
		private string _connectionStringName = null;

		private MySqlConnection _connection;

		#endregion


		#region Constructors

		//public MsSqlDacHelper() : this(_defaultConnectionStringName)
		//{
		//}

		public MySqlDacHelper(string connectionStringName)
			: this(_defaultCommandTimeout, connectionStringName) {
		}

		public MySqlDacHelper(int commandTimeout, string connectionStringName) {
			this._commandTimeout = commandTimeout;
			this._connectionStringName = connectionStringName;
			this._connection = new MySqlConnection(connectionStringName);
			//this._connection.ConnectionTimeout = this._commandTimeout;
		}

		public MySqlDacHelper(MySqlConnection connection) {
			this._connection = connection;
		}

		#endregion


		#region Properties

		public int CommandTimeout {
			get { return _commandTimeout; }
			set { _commandTimeout = value; }
		}

		public string ConnectionStringName {
			get { return _connectionStringName; }
			set { _connectionStringName = value; }
		}

		#endregion


		#region IDacHelper 멤버

		#region SelectSingleEntity

		public object SelectSingleEntity(string query, params DbParameter[] dbParams) {
			return SelectSingleEntity(CommandType.Text, query, dbParams);
		}

		public object SelectSingleEntity(CommandType cmdType, string query, params DbParameter[] dbParams) {
			MySqlCommand command = new MySqlCommand(query, _connection);
			command.CommandType = cmdType;
			command.CommandTimeout = _commandTimeout;

			if (dbParams != null) command.Parameters.AddRange(dbParams);

			MySqlDataAdapter adapter = new MySqlDataAdapter(command);
			DataSet ret = new DataSet();
			adapter.Fill(ret);

			command.Parameters.Clear();

			if (ret.Tables.Count > 0 && ret.Tables[0].Rows.Count > 0)
				return ret.Tables[0].Rows[0];

			return null;
		}

		public T SelectSingleEntity<T>(Type type, string query, params DbParameter[] dbParams) {
			return SelectSingleEntity<T>(type, CommandType.Text, query, dbParams);
		}

		public T SelectSingleEntity<T>(Type type, CommandType cmdType, string query, params DbParameter[] dbParams) {
			MySqlCommand command = new MySqlCommand(query, _connection);
			command.CommandType = cmdType;
			command.CommandTimeout = _commandTimeout;

			if (dbParams != null) command.Parameters.AddRange(dbParams);

			MySqlDataReader reader = command.ExecuteReader();

			command.Parameters.Clear();

			if (reader != null && reader.Read()) {
				T t = new MySqlDataBinder<T>(type, reader).DataBind();
				if (!reader.IsClosed) reader.Close();
				return t;
			}

			if (!reader.IsClosed) reader.Close();

			if (new List<T>().Count > 0)
				return new List<T>(1)[0];

			return default(T);
		}

		#endregion

		#region SelectMultipleEntities

		public object SelectMultipleEntities(string query, params DbParameter[] dbParams) {
			return SelectMultipleEntities(CommandType.Text, query, dbParams);
		}

		public object SelectMultipleEntities(CommandType cmdType, string query, params DbParameter[] dbParams) {
			MySqlCommand command = new MySqlCommand(query, _connection);
			command.CommandType = cmdType;
			command.CommandTimeout = _commandTimeout;

			if (dbParams != null) command.Parameters.AddRange(dbParams);

			MySqlDataAdapter adpater = new MySqlDataAdapter(command);
			DataSet ret = new DataSet();
			adpater.Fill(ret);

			command.Parameters.Clear();

			return ret.Tables[0].Rows;
		}

		public object SelectMultipleEntities(CommandType cmdType, string query, int startIndex, int length, params DbParameter[] dbParams) {
			DataRowCollection rows = (DataRowCollection)SelectMultipleEntities(cmdType, query, dbParams);

			for (int i = 0; i < rows.Count; i++) {
				if (i < startIndex || i >= startIndex + length)
					rows.Remove(rows[i]);
			}
			return rows;
		}

		public List<T> SelectMultipleEntities<T>(Type type, string query, params DbParameter[] dbParams) {
			return SelectMultipleEntities<T>(type, CommandType.Text, query, dbParams);
		}

		public List<T> SelectMultipleEntities<T>(Type type, CommandType cmdType, string query, params DbParameter[] dbParams) {
			List<T> ret = new List<T>();

			MySqlCommand command = new MySqlCommand(query, _connection);
			command.CommandType = cmdType;
			command.CommandTimeout = _commandTimeout;
			if (dbParams != null) command.Parameters.AddRange(dbParams);

			MySqlDataReader reader = command.ExecuteReader();

			command.Parameters.Clear();

			if (reader != null)
				while (reader.Read())
					ret.Add(new MySqlDataBinder<T>(type, reader).DataBind());

			command.Parameters.Clear();

			reader.Close();

			return ret;
		}

		public List<T> SelectMultipleEntities<T>(Type type, CommandType cmdType, string query, int startIndex, int legnth, params DbParameter[] dbParams) {
			List<T> ret = SelectMultipleEntities<T>(type, cmdType, query, dbParams);

			for (int i = 0; i < ret.Count; i++) {
				if (i < startIndex || i >= startIndex + legnth)
					ret.Remove(ret[i]);
			}
			return ret;
		}

		#endregion

		#region SelectScalar

		public object SelectScalar(string query, params DbParameter[] dbParams) {
			return SelectScalar(query, null, DBNull.Value, dbParams);
		}

		public object SelectScalar(string query, object dbNullDefault, object nullDefault, params DbParameter[] dbParams) {
			return SelectScalar(CommandType.Text, query, dbNullDefault, nullDefault, dbParams);
		}

		public object SelectScalar(CommandType cmdType, string query, params DbParameter[] dbParams) {
			return SelectScalar(cmdType, query, null, DBNull.Value, dbParams);
		}

		public object SelectScalar(CommandType cmdType, string query, object dbNullDefault, object nullDefault, params DbParameter[] dbParams) {
			MySqlCommand command = new MySqlCommand(query, _connection);
			command.CommandType = cmdType;
			command.CommandTimeout = _commandTimeout;

			if (dbParams != null) command.Parameters.AddRange(dbParams);

			return command.ExecuteScalar();
		}

		#endregion

		#region Execute

		public int Execute(string query, params DbParameter[] dbParams) {
			return Execute(CommandType.Text, query, dbParams);
		}

		public int Execute(CommandType cmdType, string query, params DbParameter[] dbParams) {
			MySqlCommand command = new MySqlCommand(query, _connection);
			command.CommandType = cmdType;
			command.CommandTimeout = _commandTimeout;
			if (dbParams != null) command.Parameters.AddRange(dbParams);

			int n = command.ExecuteNonQuery();

			command.Parameters.Clear();

			return n;
		}

		#endregion

		public int ExecuteNonQuery(string query, params DbParameter[] dbParams) {
			return ExecuteNonQuery(CommandType.Text, query, dbParams);
		}

		public int ExecuteNonQuery(CommandType cmdType, string query, params DbParameter[] dbParams) {
			int ret = 0;

			MySqlTransaction transaction = this._connection.BeginTransaction();

			if (_connection != null && _connection.State == ConnectionState.Open) {
				try {
					MySqlCommand command = new MySqlCommand(query, _connection);
					command.Transaction = transaction;

					command.CommandType = CommandType.Text;
					command.CommandTimeout = _defaultCommandTimeout;


					if (dbParams != null) command.Parameters.AddRange(dbParams);

					ret = command.ExecuteNonQuery();

					transaction.Commit();
				}
				catch (MySqlException ex) {
					transaction.Rollback();
					throw ex;
				}
				finally {

				}
			}

			return ret;
		}

		#region DataTable
		public DataTable SelectDataTable(string query, params DbParameter[] dbParams) {
			return SelectDataTable(CommandType.Text, query, dbParams);
		}

		public DataTable SelectDataTable(CommandType cmdType, string query, params DbParameter[] dbParams) {
			MySqlCommand command = new MySqlCommand(query, _connection);
			command.CommandType = cmdType;
			command.CommandTimeout = _commandTimeout;

			if (dbParams != null) command.Parameters.AddRange(dbParams);

			MySqlDataAdapter adapter = new MySqlDataAdapter(command);
			DataSet ret = new DataSet();
			adapter.Fill(ret);

			command.Parameters.Clear();

			return ret.Tables[0];
		}
		#endregion

		#region Date

		public DateTime GetDate() {
			return (DateTime)SelectScalar("SELECT GETDATE()", null);
		}

		public DateTime GetUtcDate() {
			return (DateTime)SelectScalar("SELECT GETUTCDATE()", null);
		}

		#endregion

		#endregion
	}
}
