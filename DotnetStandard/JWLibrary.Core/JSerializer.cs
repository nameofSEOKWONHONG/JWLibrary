namespace JWLibrary.Core {
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public static class JSerializer {

        public static T jToDeserialize<T>(this string jsonString)
            where T : class {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static IEnumerable<T> jToDeserializeAll<T>(this string jsonString)
            where T : class {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
        }

        public static string jToSerialize<T>(this T entity, Formatting? formatting = null, JsonSerializerSettings serializerSettings = null)
            where T : class {
            if (formatting.jIsNotNull() && serializerSettings.jIsNotNull()) {
                return JsonConvert.SerializeObject(entity, formatting.Value, serializerSettings);
            } else if (formatting.jIsNotNull() && serializerSettings.jIsNull()) {
                return JsonConvert.SerializeObject(entity, formatting.Value);
            } else if (formatting.jIsNull() && serializerSettings.jIsNotNull()) {
                return JsonConvert.SerializeObject(entity, serializerSettings);
            } else {
                return JsonConvert.SerializeObject(entity);
            }
        }

        public static string jToSerialize(this object obj) {
            return JsonConvert.SerializeObject(obj);
        }
    }
}