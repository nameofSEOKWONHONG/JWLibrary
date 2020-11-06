using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace JWLibrary.Core {

    public static class EmbeddedResource {

        public static string GetApiRequestFile(this string namespaceAndFileName) {
            try {
                using (var stream = typeof(EmbeddedResource).GetTypeInfo().Assembly
                    .GetManifestResourceStream(namespaceAndFileName))
                using (var reader = new StreamReader(stream, Encoding.UTF8)) {
                    return reader.ReadToEnd();
                }
            } catch {
                //ApplicationProvider.WriteToLog<EmbeddedResource>().Error(exception.Message);
                throw new Exception($"Failed to read Embedded Resource {namespaceAndFileName}");
            }
        }
    }
}