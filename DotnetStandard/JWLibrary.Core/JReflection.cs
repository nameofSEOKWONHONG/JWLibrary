using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace JWLibrary.Core {
    public static class JReflection {
        public static TValue jGetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null) {
                return valueSelector(att);
            }
            return default(TValue);
        }
    }
}
