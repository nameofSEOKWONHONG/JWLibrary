namespace JWLibrary.StaticMethod
{
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;

    public static class JSonNetEx
    {
        public static T Deserialize<T>(this string jsonString)
            where T :class
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }

        public static IEnumerable<T> DeserializeAll<T>(this string jsonString)
            where T : class
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }

        public static string Serialize<T>(this T entity, Formatting? formatting = null, JsonSerializerSettings serializerSettings = null)
            where T : class
            {
                if(formatting.jIsNotNull() && serializerSettings.jIsNotNull()) {
                    return JsonConvert.SerializeObject(entity, formatting.Value, serializerSettings);
                }
                else if(formatting.jIsNotNull() && serializerSettings.jIsNull()) {
                    return JsonConvert.SerializeObject(entity, formatting.Value);
                }
                else if(formatting.jIsNull() && serializerSettings.jIsNotNull()) {
                    return JsonConvert.SerializeObject(entity, serializerSettings);
                }
                else {
                    return JsonConvert.SerializeObject(entity);
                }
            }
    }
}