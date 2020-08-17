using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace JWLibrary.StaticMethod
{
    public class JMemoryCache
    {
        private static Dictionary<string, object> _caches = new Dictionary<string, object>();

        public static bool Add(string key, object value)
        {
            var exist = _caches.Where(m => m.Key == key).FirstOrDefault();
            if(exist.jIsNotNull()) {
                return false;
            }
            else {
                _caches.Add(key, value);
            }
            return true;
        }

        public static bool Remove(string key) {
            var exist = _caches.Where(m => m.Key == key).FirstOrDefault();
            if(exist.jIsNotNull()) _caches.Remove(key);
            else return false;
            return true;
        }

        public static T Get<T>(string key) {
            var exist = _caches.Where(m => m.Key == key).FirstOrDefault();
            if(exist.jIsNotNull())
            {
                if (exist.Value is T) {
                    return (T)exist.Value;
                } 
                try {
                    return (T)Convert.ChangeType(exist.Value, typeof(T));
                } 
                catch (InvalidCastException) {
                    return default(T);
                }
            }

            return default(T);
        }

        public override string ToString() {
            var ret = new StringBuilder();
            foreach (var keyValue in _caches)
            {
                ret.Append(keyValue.Key);
                ret.Append(":");
                ret.Append(JsonConvert.SerializeObject(keyValue.Value));
                ret.Append(",");
            }
            return ret.ToString();
        }
    }
}