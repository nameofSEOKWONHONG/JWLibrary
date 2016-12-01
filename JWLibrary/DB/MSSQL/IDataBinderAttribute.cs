using System.Reflection;

namespace JWLibrary.DB.MSSQL
{
  public interface IDataBinderAttribute
  {
    IDataBinderAttribute CreateDataBinderAttribute(MemberInfo member);
  }
}
