using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace JWLibrary.Helpers
{
    public static class DataTableHelper
    {
        public static List<TEntity> ToList<TEntity>(this DataTable datatable)
        {

        }

        public static DataTable ToDatatable(this string json)
        {
            return JsonConvert.DeserializeObject<DataTable>(json);
        }

        public static JObject ToJObject(this DataTable datatable)
        {
            return JObject.FromObject(datatable);
        }

        public static JArray ToJArray(this DataTable datatable)
        {
            return (JArray)JsonConvert.SerializeObject(datatable);
        } 
    }
}
