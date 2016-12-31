using JWLibrary.Core.DB.MSSQL.Meta.Query;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static JWLibrary.Core.DB.MSSQL.Meta.DataAccesor.MetaDataAccesor;

namespace JWLibrary.Core.DB.MSSQL.Meta
{
    public class MetaInfoManager
    {
        public SqlConnection Connection { get; set; }

        public MetaInfoManager(SqlConnection con)
        {
            Connection = con;
        }

        public MetaInfoManager() : this (null)
        {

        }

        public List<DatabaseDAO> GetDatabaseList()
        {
            List<DatabaseDAO> list = null;

			try {
				if (Connection != null && Connection.State == System.Data.ConnectionState.Closed) {
					Connection.Open();

					list = (List<DatabaseDAO>)DataHelperFactory.CreateMsSqlDacHelper(Connection).SelectMultipleEntities<DatabaseDAO>(
						typeof(DatabaseDAO)
						, QueryConst.SELECT_DATABASES()
						);
				}
			}
			catch { }
			finally {
				Connection.Close();
			}


            return list;
        }

        public List<DatabaseTableDAO> GetDatabaseTableList(string databaseName)
        {
            List<DatabaseTableDAO> list = null;

			try {
				if (Connection != null && Connection.State == System.Data.ConnectionState.Closed) {
					Connection.Open();

					list = (List<DatabaseTableDAO>)DataHelperFactory.CreateMsSqlDacHelper(Connection).SelectMultipleEntities<DatabaseTableDAO>(
						typeof(DatabaseTableDAO)
						, QueryConst.SELECT_TABLES(databaseName)
						);
				}
			}
			catch { }
			finally {
				Connection.Close();
			}

            return list;
        }

        public List<DatabaseTableColumnDAO> GetDatabaseTableColumnList(string databaseName, string tableName)
        {
            List<DatabaseTableColumnDAO> list = null;

			try {
				if (Connection != null && Connection.State == System.Data.ConnectionState.Closed) {
					Connection.Open();

					list = (List<DatabaseTableColumnDAO>)DataHelperFactory.CreateMsSqlDacHelper(Connection).SelectMultipleEntities<DatabaseTableColumnDAO>(
						typeof(DatabaseTableColumnDAO)
						, QueryConst.SELECT_TABLE_COLUMS(databaseName, tableName)
						);
				}
			}
			catch { }
			finally {
				Connection.Close();
			}
			
            return list;
        }

        public int GetDatabaseTableExsist(string databaseName, string tableName)
        {
            int n = 0;

			try {
				if (Connection != null && Connection.State == System.Data.ConnectionState.Closed) {
					Connection.Open();

					object o = DataHelperFactory.CreateMsSqlDacHelper(Connection).SelectSingleEntity(
						 QueryConst.SELECT_EXSIST_TABLE(databaseName, tableName), null
						);

					if (o != null) {
						DataRow dr = (DataRow)o;

						n = int.Parse(dr["CNT"].ToString());
					}
				}
			}
			catch { }
			finally{
				Connection.Close();
			}

            return n;
        }


        public DatabaseTableColumnDAO GetDatabaseTableColumnInfo(string databaseName, string tableName, string columnName)
        {
            DatabaseTableColumnDAO item = null;

            if (Connection != null && Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();

				try {
					item = (DatabaseTableColumnDAO)DataHelperFactory.CreateMsSqlDacHelper(Connection).SelectSingleEntity<DatabaseTableColumnDAO>(
						typeof(DatabaseTableColumnDAO)
						, QueryConst.SELECT_TABLE_COLUM_INFO(databaseName, tableName, columnName)
						);
				}
				catch {

				}
				finally {
					Connection.Close();
				}
            }

            return item;
        }

        public DatabaseTableColumnPk GetDatabaseTableColumnPk(string databaseName, string tableName)
        {
            DatabaseTableColumnPk databaseTableColumnPk = null;

			try {
				if (Connection != null && Connection.State == System.Data.ConnectionState.Closed) {
					Connection.Open();

					databaseTableColumnPk = DataHelperFactory.CreateMsSqlDacHelper(Connection).SelectSingleEntity<DatabaseTableColumnPk>(
						typeof(DatabaseTableColumnPk)
						, QueryConst.SELECT_TABLE_COLUMN_PK(databaseName, tableName)
						);
				}
			}
			catch { }
			finally {
				Connection.Close();
			}

            return databaseTableColumnPk;
        }
    }
}
