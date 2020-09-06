using System;
using System.Reflection;

namespace JWLibrary.Core.NetFramework.DB.MSSQL.Module {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MsSqlDataBinderAttribute : Attribute, IDataBinderAttribute {
        private string fieldName;

        public MsSqlDataBinderAttribute(string fieldName) {
            this.fieldName = fieldName;
        }

        public string FieldName {
            get { return fieldName; }
            set { fieldName = value; }
        }

        #region IDataBinderAttribute 멤버

        public IDataBinderAttribute CreateDataBinderAttribute(MemberInfo member) {
            try {
                object[] custAttrs = member.GetCustomAttributes(typeof(MsSqlDataBinderAttribute), false);

                if (custAttrs != null && custAttrs.Length > 0)
                    return custAttrs[0] as IDataBinderAttribute;
            } catch {
            }
            return null;
        }

        #endregion IDataBinderAttribute 멤버
    }
}