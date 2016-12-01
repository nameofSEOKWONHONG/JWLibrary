using System;
using System.Reflection;

namespace JWLibrary.DB.MYSQL.Module {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MySqlDataBinderAttribute : Attribute, IDataBinderAttribute {
		private string fieldName;

		public MySqlDataBinderAttribute(string fieldName) {
			this.fieldName = fieldName;
		}

		public string FieldName {
			get { return fieldName; }
			set { fieldName = value; }
		}

		#region IDataBinderAttribute 멤버

		public IDataBinderAttribute CreateDataBinderAttribute(MemberInfo member) {
			try {
				object[] custAttrs = member.GetCustomAttributes(typeof(MySqlDataBinderAttribute), false);

				if (custAttrs != null && custAttrs.Length > 0)
					return custAttrs[0] as IDataBinderAttribute;
			}
			catch {
			}
			return null;
		}

		#endregion
	}
}
