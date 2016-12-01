/*
 * Orignal Writer : TeaHo-Meang.
 * Editor : SeokWon-Hong.
 * Date : 2012.02.01 
 */

using JWLibrary.DB.MSSQL.Module;
using System.Data.SqlClient;


namespace JWLibrary.DB.MSSQL
{
  public class DataHelperFactory
  {
    #region MS-SQL Data Helper

    public static MsSqlDacHelper CreateMsSqlDacHelper(string connectionStringName)
    {
      return new MsSqlDacHelper(connectionStringName);
    }

    public static MsSqlDacHelper CreateMsSqlDacHelper(int commandTimeout, string connectionStringName)
    {
      return new MsSqlDacHelper(commandTimeout, connectionStringName);
    }

    public static MsSqlDacHelper CreateMsSqlDacHelper(SqlConnection connection)
    {
      return new MsSqlDacHelper(connection);
    }

    #endregion       
  }
}