using System.Reflection;

namespace JWLibrary.Core.NetFramework.DB.MSSQL {

    public interface IDataBinderAttribute {

        IDataBinderAttribute CreateDataBinderAttribute(MemberInfo member);
    }
}