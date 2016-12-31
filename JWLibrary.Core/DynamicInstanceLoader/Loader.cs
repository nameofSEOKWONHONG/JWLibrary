using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.DynamicInstanceLoader
{
    public class Loader
    {
        public static T LoadAssembly<T>(string fileName, string instanceName)
        {
            if (!File.Exists(fileName)) return default(T);

            var dll = Assembly.LoadFile(fileName);
            return (T)dll.CreateInstance(instanceName);
        }
    }
}
