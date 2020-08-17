using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Dynamic;

namespace JWLibrary.Helpers
{
    public class ParameterHelper
    {
        private Dictionary<string, object> _map = new Dictionary<string, object>();
        public ParameterHelper(params KeyValuePair<string, object>[] kvps)
        {
            foreach(var item in kvps){
                _map.Add(item.Key, item.Value);

            }
        }

        public ParameterHelper(params dynamic[] kvps)
        {
            foreach(var kvp in kvps)
            {
                _map.Add(kvp.Key, kvp.Value);
            }
        }
        
        public void Add(KeyValuePair<string, object> kvp)
        {
            _map.Add(kvp.Key, kvp.Value);
        }        

        public JObject ToJObject()
        {
            return JObject.FromObject(_map);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_map);
        }
        
        public string ToGetParameter()
        {
            string ret = string.Empty;

            int idx = 0;
            foreach(var kvp in _map)
            {
                if (idx == 0) ret += "?" + kvp.Key + "=" + kvp.Value;
                else ret += "&" + kvp.Key + "=" + kvp.Value;
            }

            return ret;
        }
    }
}
