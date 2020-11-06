using System;
using System.Linq;

namespace JWLibrary.Core {

    public static class JReflection {

        public static TValue jGetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null) return valueSelector(att);
            return default;
        }
    }
}