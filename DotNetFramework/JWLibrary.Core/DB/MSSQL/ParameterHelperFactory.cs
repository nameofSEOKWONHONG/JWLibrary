using System.Data;

using System.Data.SqlClient;

using JWLibrary.Core.NetFramework.DB.MSSQL.Module;

namespace JWLibrary.Core.NetFramework.DB.MSSQL
{
  public class ParameterHelperFactory
  {
    #region MS-SQL Parameter 

    public static SqlParameter CreateMsSqlParameter(string parameterName, object value)
    {
      return (SqlParameter)new MsSqlParameterHelper().CreateParameter(parameterName, value);
    }

    public static SqlParameter CreateMsSqlParameter(string parameterName, object value, ParameterDirection direction)
    {
      return (SqlParameter)new MsSqlParameterHelper().CreateParameter(parameterName, value, direction);
    }

    public static SqlParameter CreateMsSqlParameter(string parameterName, object value, object dbType)
    {
      return (SqlParameter)new MsSqlParameterHelper().CreateParameter(parameterName, value, dbType);
    }

    public static SqlParameter CreateMsSqlParameter(string parameterName, object value, object dbType, ParameterDirection direction)
    {
      return (SqlParameter)new MsSqlParameterHelper().CreateParameter(parameterName, value, dbType, direction);
    }

    public static SqlParameter CreateMsSqlParameter(string parameterName, object value, object dbType, int size)
    {
      return (SqlParameter)new MsSqlParameterHelper().CreateParameter(parameterName, value, dbType, size);
    }

    public static SqlParameter CreateMsSqlParameter(string parameterName, object value, object dbType, int size, ParameterDirection direction)
    {
      return (SqlParameter)new MsSqlParameterHelper().CreateParameter(parameterName, value, dbType, size, direction);
    }

    #endregion 
  }
}
