using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace JWLibrary.StaticMethod
{
    public static class JString
    {
        public static int jToCount(this string str) {
            return str.jIsNull() ? 0 : str.Length;
        }

        public static bool jIsNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }

        private static void jCopyTo(Stream src, Stream dest) {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static byte[] jZip(this string str) {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
                    //msi.CopyTo(gs);
                    jCopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string jUnzip(this byte[] bytes) {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream()) {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                    //gs.CopyTo(mso);
                    jCopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
    }
}
