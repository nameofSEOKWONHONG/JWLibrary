
using JWLibrary.DB.SQLite.Module;
using System.Data.SQLite;


namespace JWLibrary.DB.SQLite
{
  public class DataHelperFactory
  {
    #region MS-SQL Data Helper

    public static SqliteDacHelper CreateMsSqlDacHelper(string connectionStringName)
    {
		return new SqliteDacHelper(connectionStringName);
    }

	public static SqliteDacHelper CreateMsSqlDacHelper(int commandTimeout, string connectionStringName)
    {
		return new SqliteDacHelper(commandTimeout, connectionStringName);
    }

	public static SqliteDacHelper CreateMsSqlDacHelper(SQLiteConnection connection)
    {
		return new SqliteDacHelper(connection);
    }

    #endregion
  }
}