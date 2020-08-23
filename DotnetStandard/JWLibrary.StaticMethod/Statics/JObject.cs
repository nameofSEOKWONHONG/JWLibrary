using Org.BouncyCastle.Crypto.Engines;
using System;

namespace JWLibrary.StaticMethod
{
    public static class JObject
    {
        public static bool jIsTrue(this bool obj) {
            return obj.Equals(true);
        }

        public static bool jIsFalse(this bool obj) {
            return obj.Equals(false);
        }

        public static bool jIsNull(this object obj) {
            if (obj == null) return true;
            if (obj.GetType() == typeof(string))
            {
                return StringIsNullOrEmpty(obj);
            }
            return false;
        }

        public static bool jIsNotNull(this object obj) {
            if (obj == null) return false;
            if(obj.GetType() == typeof(string))
            {
                return StringIsNotNullOrEmpty(obj);
            }
            return true;
        }

        private static bool StringIsNullOrEmpty(object obj)
        {
            return string.IsNullOrEmpty((string)obj);
        }

        private static bool StringIsNotNullOrEmpty(object obj)
        {
            return !string.IsNullOrEmpty((string)obj);
        }
    }
}