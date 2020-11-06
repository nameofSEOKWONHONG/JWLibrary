using JWLibrary.Core;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Utils.Files {

    public static class FileExtensions {

        public static string[] jReadLines(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return File.ReadAllLines(fileName);
        }

        public static async Task<string[]> jReadLinesAsync(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return await File.ReadAllLinesAsync(fileName);
        }

        public static byte[] jReadBytes(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return File.ReadAllBytes(fileName);
        }

        public static async Task<byte[]> jReadBytesAsync(this string fileName) {
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            return await File.ReadAllBytesAsync(fileName);
        }

        public static void jWriteLines(this string fileName, string[] lines, Encoding encoding = null) {
            if (encoding.jIsNotNull())
                File.WriteAllLines(fileName, lines, encoding);

            File.WriteAllLines(fileName, lines);
        }

        public static async Task jWriteLinesAsync(this string fileName, string[] lines, Encoding encoding = null) {
            if (encoding.jIsNotNull())
                await File.WriteAllLinesAsync(fileName, lines, encoding);

            await File.WriteAllLinesAsync(fileName, lines);
        }

        public static void jWriteBytes(this string fileName, byte[] bytes) {
            File.WriteAllBytes(fileName, bytes);
        }

        public static async Task jWriteBytesAsync(this string fileName, byte[] bytes) {
            await File.WriteAllBytesAsync(fileName, bytes);
        }

        public static bool jFileExists(this string fileName) {
            return File.Exists(fileName);
        }

        public static string jToFileUniqueId(this string fileName) {
            var ret = string.Empty;
            if (!File.Exists(fileName)) throw new Exception($"not exists {fileName}");
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(fileName)) {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }
    }
}