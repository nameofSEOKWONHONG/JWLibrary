using System;

namespace JWLibrary.StaticMethod
{
    public static class JObject
    {
        public static bool jIsNull(this object obj) {
            if (obj.GetType() == typeof(string))
            {
                return StringIsNullOrEmpty(obj);
            }
            return obj == null;
        }

        public static bool jIsNotNull(this object obj) {
            if(obj.GetType() == typeof(string))
            {
                return StringIsNotNullOrEmpty(obj);
            }
            return obj != null;
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