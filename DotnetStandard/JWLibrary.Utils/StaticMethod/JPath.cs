using System.IO;
using System.Text.RegularExpressions;

namespace JWLibrary.StaticMethod {

    public static class JPath {

        /// <summary>
        /// executable app root path
        /// c:\development\MyApp
        /// “TargetFile.cs”.ToApplicationPath()
        /// c:\development\MyApp\TargetFile.cs
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string jToAppPath(this string fileName, string addPath = null) {
            var exePath = Path.GetDirectoryName(System.Reflection
                                .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            appRoot = appRoot + "/" + addPath;
            return Path.Combine(appRoot, fileName);
        }
    }
}