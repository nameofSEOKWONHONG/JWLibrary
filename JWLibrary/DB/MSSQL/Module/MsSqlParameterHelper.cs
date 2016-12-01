using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace JWLibrary.DB.MSSQL.Module
{
  public class MsSqlParameterHelper : IParameterHelper
  {
    #region IParameterHelper 멤버

    public DbParameter CreateParameter(string parameterName, object value)
    {
      return CreateParameter(parameterName, value, ParameterDirection.Input);
    }

    public DbParameter CreateParameter(string parameterName, object value, ParameterDirection direction)
    {
      SqlParameter param = new SqlParameter(parameterName, value);
      param.Direction = direction;
      return param;
    }
    
    public DbParameter CreateParameter(string parameterName, object value, object dbType)
    {
      return CreateParameter(parameterName, value, dbType, ParameterDirection.Input);
    }

    public DbParameter CreateParameter(string parameterName, object value, object dbType, ParameterDirection direction)
    {
      SqlParameter param = new SqlParameter(parameterName, value);

      if (dbType is SqlDbType)
        param.SqlDbType = (SqlDbType)dbType;
      else
        param.DbType = (DbType)dbType;

      param.Direction = direction;

      return param;
    }

    public DbParameter CreateParameter(string parameterName, object value, object dbType, int size)
    {
      return CreateParameter(parameterName, value, dbType, size, ParameterDirection.Input);
    }

    public DbParameter CreateParameter(string parameterName, object value, object dbType, int size, ParameterDirection direction)
    {
      SqlParameter param = new SqlParameter(parameterName, value);

      if (dbType is SqlDbType)
        param.SqlDbType = (SqlDbType)dbType;
      else
        param.DbType = (DbType)dbType;

      param.Size = size;
      param.Direction = direction;

      return param;
    }

    #endregion 
  }
}
