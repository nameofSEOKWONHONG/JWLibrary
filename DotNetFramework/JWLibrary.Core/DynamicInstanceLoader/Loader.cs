using System.IO;
using System.Reflection;

namespace JWLibrary.Core.NetFramework.DynamicInstanceLoader {

    public class Loader {

        public static T LoadAssembly<T>(string fileName, string instanceName) {
            if (!File.Exists(fileName)) return default(T);

            var dll = Assembly.LoadFile(fileName);
            return (T)dll.CreateInstance(instanceName);
        }
    }
}