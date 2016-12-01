using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace JWLibrary.DB.MSSQL.Module
{
    public class MsSqlDacHelper : IDacHelper
    {
        #region Fields

        private static int _defaultCommandTimeout = 30;
        //private static string _defaultConnectionStringName = "";

        private int commandTimeout = _defaultCommandTimeout;
        private string connectionStringName = null;

        private SqlConnection connection;

        #endregion


        #region Constructors

        //public MsSqlDacHelper() : this(_defaultConnectionStringName)
        //{
        //}

        public MsSqlDacHelper(string connectionStringName)
            : this(_defaultCommandTimeout, connectionStringName)
        {
        }

        public MsSqlDacHelper(int commandTimeout, string connectionStringName)
        {
            this.commandTimeout = commandTimeout;
            this.connectionStringName = connectionStringName;
            this.connection = new SqlConnection(connectionStringName);
            //this._connection.ConnectionTimeout = this._commandTimeout;
        }

        public MsSqlDacHelper(SqlConnection connection)
        {
            this.connection = connection;
        }

        #endregion


        #region Properties

        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        public string ConnectionStringName
        {
            get { return connectionStringName; }
            set { connectionStringName = value; }
        }

        #endregion


        #region IDacHelper 멤버

        #region SelectSingleEntity

        public object SelectSingleEntity(string query, params DbParameter[] dbParams)
        {
            return SelectSingleEntity(CommandType.Text, query, dbParams);
        }

        public object SelectSingleEntity(CommandType cmdType, string query, params DbParameter[] dbParams)
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = cmdType;
            command.CommandTimeout = commandTimeout;

            if (dbParams != null) command.Parameters.AddRange(dbParams);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet ret = new DataSet();
            adapter.Fill(ret);

            command.Parameters.Clear();

            if (ret.Tables.Count > 0 && ret.Tables[0].Rows.Count > 0)
                return ret.Tables[0].Rows[0];

            return null;
        }

        public T SelectSingleEntity<T>(Type type, string query, params DbParameter[] dbParams)
        {
            return SelectSingleEntity<T>(type, CommandType.Text, query, dbParams);
        }

        public T SelectSingleEntity<T>(Type type, CommandType cmdType, string query, params DbParameter[] dbParams)
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = cmdType;
            command.CommandTimeout = commandTimeout;

            if(dbParams != null) command.Parameters.AddRange(dbParams);

            SqlDataReader reader = command.ExecuteReader();

            command.Parameters.Clear();

            if (reader != null && reader.Read())
            {
                T t = new MsSqlDataBinder<T>(type, reader).DataBind();
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

        public object SelectMultipleEntities(string query, params DbParameter[] dbParams)
        {
            return SelectMultipleEntities(CommandType.Text, query, dbParams);
        }

        public object SelectMultipleEntities(CommandType cmdType, string query, params DbParameter[] dbParams)
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = cmdType;
            command.CommandTimeout = commandTimeout;

            if(dbParams != null) command.Parameters.AddRange(dbParams);

            SqlDataAdapter adpater = new SqlDataAdapter(command);
            DataSet ret = new DataSet();
            adpater.Fill(ret);

            command.Parameters.Clear();

            return ret.Tables[0].Rows;
        }

        public object SelectMultipleEntities(CommandType cmdType, string query, int startIndex, int length, params DbParameter[] dbParams)
        {
            DataRowCollection rows = (DataRowCollection)SelectMultipleEntities(cmdType, query, dbParams);

            for (int i = 0; i < rows.Count; i++)
            {
                if (i < startIndex || i >= startIndex + length)
                    rows.Remove(rows[i]);
            }
            return rows;
        }

        public List<T> SelectMultipleEntities<T>(Type type, string query, params DbParameter[] dbParams)
        {
            return SelectMultipleEntities<T>(type, CommandType.Text, query, dbParams);
        }

        public List<T> SelectMultipleEntities<T>(Type type, CommandType cmdType, string query, params DbParameter[] dbParams)
        {
            List<T> ret = new List<T>();

            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = cmdType;
            command.CommandTimeout = commandTimeout;
            if (dbParams != null) command.Parameters.AddRange(dbParams);

            SqlDataReader reader = command.ExecuteReader();

            if (reader != null)
                while (reader.Read())
                    ret.Add(new MsSqlDataBinder<T>(type, reader).DataBind());

            reader.Close();
			command.Parameters.Clear();

            return ret;
        }

        public List<T> SelectMultipleEntities<T>(Type type, CommandType cmdType, string query, int startIndex, int legnth, params DbParameter[] dbParams)
        {
            List<T> ret = SelectMultipleEntities<T>(type, cmdType, query, dbParams);

            for (int i = 0; i < ret.Count; i++)
            {
                if (i < startIndex || i >= startIndex + legnth)
                    ret.Remove(ret[i]);
            }
            return ret;
        }

        #endregion

        #region SelectScalar

        public object SelectScalar(string query, params DbParameter[] dbParams)
        {
            return SelectScalar(query, null, DBNull.Value, dbParams);
        }

        public object SelectScalar(string query, object dbNullDefault, object nullDefault, params DbParameter[] dbParams)
        {
            return SelectScalar(CommandType.Text, query, dbNullDefault, nullDefault, dbParams);
        }

        public object SelectScalar(CommandType cmdType, string query, params DbParameter[] dbParams)
        {
            return SelectScalar(cmdType, query, null, DBNull.Value, dbParams);
        }

        public object SelectScalar(CommandType cmdType, string query, object dbNullDefault, object nullDefault, params DbParameter[] dbParams)
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = cmdType;
            command.CommandTimeout = commandTimeout;

            if (dbParams != null) command.Parameters.AddRange(dbParams);

            return command.ExecuteScalar();
        }

        #endregion

        #region Execute

		/// <summary>
		/// TransactionScope 사용해야 함.
		/// </summary>
		/// <param name="query"></param>
		/// <param name="dbParams"></param>
		/// <returns></returns>
        public int Execute(string query, params DbParameter[] dbParams)
        {
            return Execute(CommandType.Text, query, dbParams);
        }

        public int Execute(CommandType cmdType, string query, params DbParameter[] dbParams)
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = cmdType;
            command.CommandTimeout = commandTimeout;
            if (dbParams != null) command.Parameters.AddRange(dbParams);

            int n = command.ExecuteNonQuery();

            command.Parameters.Clear();

            return n;
        }

        #endregion

		/// <summary>
		/// SqlTransaction 사용 (TransactionScope 사용 안함.)
		/// </summary>
		/// <param name="query"></param>
		/// <param name="dbParams"></param>
		/// <returns></returns>
        public int ExecuteWithTransaction(SqlTransaction tran, string query, params DbParameter[] dbParams)
        {
			return ExecuteWithTransaction(tran, CommandType.Text, query, dbParams);
        }

		public int ExecuteWithTransaction(SqlTransaction tran, CommandType cmdType, string query, params DbParameter[] dbParams)
        {
            int ret = 0;

            if (connection != null && connection.State == ConnectionState.Open)
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Transaction = tran;

                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = _defaultCommandTimeout;
					
                    if (dbParams != null) command.Parameters.AddRange(dbParams);

                    ret = command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {                    
                    throw ex;
                }
                finally
                {

                }
            }

            return ret;
        }

        #region DataTable
        public DataTable SelectDataTable(string query, params DbParameter[] dbParams)
        {
            return SelectDataTable(CommandType.Text, query, dbParams);
        }

        public DataTable SelectDataTable(CommandType cmdType, string query, params DbParameter[] dbParams)
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = cmdType;
            command.CommandTimeout = commandTimeout;

            if (dbParams != null) command.Parameters.AddRange(dbParams);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet ret = new DataSet();
            adapter.Fill(ret);

            command.Parameters.Clear();

            return ret.Tables[0];
        }
        #endregion

		#region DataSet
		public DataSet SelectDataSet(string qeury, params DbParameter[] dbParams) {
			return SelectDataSet(CommandType.Text, qeury, dbParams);
		}

		public DataSet SelectDataSet(CommandType cmdType, string query, params DbParameter[] dbParams) {
			SqlCommand command = new SqlCommand(query, connection);
			command.CommandType = cmdType;
			command.CommandTimeout = commandTimeout;

			if (dbParams != null) command.Parameters.AddRange(dbParams);

			SqlDataAdapter adapter = new SqlDataAdapter(command);
			DataSet ret = new DataSet();
			adapter.Fill(ret);

			command.Parameters.Clear();

			return ret;
		}
		#endregion
		#region Date

		public DateTime GetDate()
        {
            return (DateTime)SelectScalar("SELECT GETDATE()", null);
        }

        public DateTime GetUtcDate()
        {
            return (DateTime)SelectScalar("SELECT GETUTCDATE()", null);
        }

        #endregion

        #endregion
	}
}