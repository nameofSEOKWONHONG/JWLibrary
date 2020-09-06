using JWLibrary.DB.SQLite;

namespace GSCoreLibray.DB.MsSql.Module {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SqliteDataBinderAttribute : Attribute, IDataBinderAttribute {
        private string fieldName;

        public SqliteDataBinderAttribute(string fieldName) {
            this.fieldName = fieldName;
        }

        public string FieldName {
            get { return fieldName; }
            set { fieldName = value; }
        }

        #region IDataBinderAttribute 멤버

        public IDataBinderAttribute CreateDataBinderAttribute(MemberInfo member) {
            try {
                object[] custAttrs = member.GetCustomAttributes(typeof(SqliteDataBinderAttribute), false);

                if (custAttrs != null && custAttrs.Length > 0)
                    return custAttrs[0] as IDataBinderAttribute;
            } catch {
            }
            return null;
        }

        #endregion IDataBinderAttribute 멤버
    }
}