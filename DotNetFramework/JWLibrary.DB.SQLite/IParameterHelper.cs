using System.Data;
using System.Data.Common;

namespace JWLibrary.DB.SQLite
{
  public interface IParameterHelper
  {
    DbParameter CreateParameter(string parameterName, object value);

    DbParameter CreateParameter(string parameterName, object value, ParameterDirection direction);

    DbParameter CreateParameter(string parameterName, object value, object dbType);

    DbParameter CreateParameter(string parameterName, object value, object dbType, ParameterDirection direction);

    DbParameter CreateParameter(string parameterName, object value, object dbType, int size);

    DbParameter CreateParameter(string parameterName, object value, object dbType, int size, ParameterDirection direction);
  }
}
