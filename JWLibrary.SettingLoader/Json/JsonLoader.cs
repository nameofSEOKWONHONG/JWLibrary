using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.SettingLoader.Json
{
    public class JsonLoader<T> where T : class
    {
        public static T LoadFromJson(string filename)
        {
            if (false == File.Exists(filename)) return null;

            try
            {
                // read JSON directly from a file
                using (StreamReader file = File.OpenText(filename))
                {
                    var json = file.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (System.Exception e)
            {
                Debug.Assert(false);
                e.ToString();
            }

            return null;
        }

        public static bool SaveToJson(string filename, T settings)
        {
            if (null == filename || string.Empty == Path.GetFileName(filename))
            {
                return false;
            }

            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            try
            {
                using (var sw = File.CreateText(filename))
                {
                    sw.Write(json);
                }
            }
            catch (System.Exception e)
            {
                Debug.Assert(false);
                e.ToString();
                return false;
            }

            return true;
        }
    }
}
