using System.Reflection;

namespace JWLibrary.DB.MYSQL {

    public interface IDataBinderAttribute {

        IDataBinderAttribute CreateDataBinderAttribute(MemberInfo member);
    }
}