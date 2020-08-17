using System.Reflection;

namespace JWLibrary.DB.SQLite
{
  public interface IDataBinderAttribute
  {
    IDataBinderAttribute CreateDataBinderAttribute(MemberInfo member);
  }
}
