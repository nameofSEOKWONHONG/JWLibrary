using System;
using System.Collections.Generic;
using System.Linq;

namespace JWLibrary.Core {

    public static class JLinq {

        public static List<T> jToList<T>(this IEnumerable<T> list) {
            return list.jIsNull() ? new List<T>() : list.ToList();
        }

        public static T[] jToArray<T>(this IEnumerable<T> list) {
            return list.jIsNull() ? new T[0] : list.ToArray();
        }

        public static T jFirst<T>(this IEnumerable<T> list, Func<T, bool> predicate = null) {
            if (predicate.jIsNotNull()) return list.FirstOrDefault(predicate);
            return list.FirstOrDefault();
        }

        public static T jLast<T>(this IEnumerable<T> list, Func<T, bool> predicate = null) {
            if (predicate.jIsNotNull()) return list.LastOrDefault(predicate);
            return list.LastOrDefault();
        }

        public static string jIfNullOrEmpty(this string str, Func<string, string> func) {
            var result = str;
            if (str.jIsNullOrEmpty().jIsTrue()) {
                result = func(str);
            }
            return result;
        }

        public static T jIfNull<T>(this T obj, Func<T, T> predicate) {
            if (obj.jIsNull()) {
                obj = predicate(obj);
            }

            return obj;
        }

        public static T jIfNotNull<T>(this T obj, Func<T, T> predicate, T defaultValue)
            where T : class {
            if (obj.jIsNotNull()) {
                return predicate(obj);
            }

            return defaultValue;
        }

        public static IEnumerable<T> jWhere<T>(this IEnumerable<T> obj, Func<T, bool> predicate)
            where T : class {
            List<T> list = new List<T>();
            if (obj.jToList().jCount() <= 0) obj = new List<T>();
            list = obj.Where(predicate).jToList();
            return list;
        }

        public static bool jEquals<T>(this T obj, object diffObj) {
            return obj.Equals(diffObj);
        }
    }
}