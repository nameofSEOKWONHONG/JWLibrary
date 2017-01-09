using JWLibrary.Core.DB.MSSQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
#if NETFX_40
using System.Dynamic;
#endif
using System.Linq;
using System.Reflection;
using System.Text;

namespace JWLibrary.Core.DB.MSSQL.Module {

#if NETFX_40
	public class DynamicDataReader : DynamicEnumerableDataObjectWrapper<DbDataReader> {
		public DynamicDataReader(DbDataReader reader)
			: base(reader) {
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result) {
			if (base.TryGetMember(binder, out result))
				return true;
			else {
				try {
					if (!Obj.IsDBNull(Obj.GetOrdinal(binder.Name)))
						result = Obj.GetValue(Obj.GetOrdinal(binder.Name));
					else
						result = null;
					return true;
				}
				catch (Exception) {
					result = null;
					return false;
				}
			}
		}

		public override bool TryGetIndex(GetIndexBinder binder,
			   object[] indexes, out object result) {
			try {
				object index = indexes[0];
				if (index is int) {
					int intIndex = (int)index;
					if (!Obj.IsDBNull(intIndex))
						result = Obj.GetValue(intIndex);
					else
						result = null;
					return true;
				}
				else if (index is string) {
					string strIndex = (string)index;
					if (!Obj.IsDBNull(Obj.GetOrdinal(strIndex)))
						result = Obj.GetValue(Obj.GetOrdinal(strIndex));
					else
						result = null;
					return true;
				}
				else {
					result = null;
					return false;
				}
			}
			catch (Exception) {
				result = null;
				return false;
			}
		}

		public static implicit operator DbDataReader(DynamicDataReader reader) {
			return reader.Obj;
		}

		public static explicit operator DynamicDataReader(DbDataReader reader) {
			return new DynamicDataReader(reader);
		}
	}

	public abstract class DynamicEnumerableDataObjectWrapper<T> :
		   DynamicDataObjectWrapper<T>, IEnumerable
		   where T : IEnumerable {
		public DynamicEnumerableDataObjectWrapper(T obj)
			: base(obj) {
		}

		public virtual IEnumerator GetEnumerator() {
			return Obj.GetEnumerator();
		}
	}

	public class DynamicDataObjectWrapper<T> : DynamicObject {
		protected T Obj { get; private set; }
		protected Type ObjType { get; private set; }

		public DynamicDataObjectWrapper(T obj) {
			this.Obj = obj;
			this.ObjType = obj.GetType();
		}

		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
			try {
				result = ObjType.InvokeMember(binder.Name,
					System.Reflection.BindingFlags.InvokeMethod |
					System.Reflection.BindingFlags.Instance |
					System.Reflection.BindingFlags.Public, null, Obj, args);

				return true;
			}
			catch (Exception) {
				result = null;
				return false;
			}
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result) {
			PropertyInfo property = ObjType.GetProperty(binder.Name, BindingFlags.Instance | BindingFlags.Public);

			if (property != null) {
				result = property.GetValue(Obj, null);
				return true;
			}
			else {
				result = null;
				return false;
			}
		}

		public override bool TrySetMember(SetMemberBinder binder, object value) {
			PropertyInfo property = ObjType.GetProperty(binder.Name,
				   BindingFlags.Instance | BindingFlags.Public);
			if (property != null) {
				property.SetValue(Obj, value, null);
				return true;
			}
			else
				return false;
		}
	}
#endif
}
