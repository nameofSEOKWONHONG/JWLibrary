using System.Reflection;

namespace JWLibrary.Core.DB.MSSQL
{
  public interface IDataBinderAttribute
  {
    IDataBinderAttribute CreateDataBinderAttribute(MemberInfo member);
  }
}
