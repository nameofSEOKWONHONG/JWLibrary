namespace JWLibrary.Core {
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
            return false;
        }

        public static bool jIsNotNull(this object obj) {
            if (obj == null) return false;
            return true;
        }
    }
}