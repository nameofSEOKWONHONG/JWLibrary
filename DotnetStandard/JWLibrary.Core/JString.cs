namespace JWLibrary.Core {

    public static class JString {

        public static int jToCount(this string str) {
            return str.jIsNull() ? 0 : str.Length;
        }

        public static bool jIsNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }

        public static string jReplace(this string text, string oldValue, string newValue) {
            return text.jIfNullOrEmpty(x => string.Empty).Replace(oldValue, newValue);
        }
    }
}