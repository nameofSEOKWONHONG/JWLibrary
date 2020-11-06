using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Utils {

    public static class CryptoSHA256 {

        /// <summary>
        ///     SHA256 Encrypt (Decrypt is not support.)
        /// </summary>
        /// <param name="encryptText"></param>
        /// <param name="encryptKey"></param>
        /// <param name="useHashing">not use</param>
        /// <returns></returns>
        public static string ToSHA256(this string encryptText) {
            using (var sha = SHA256.Create()) {
                var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(encryptText));

                var stringBuilder = new StringBuilder();

                foreach (var b in hash) stringBuilder.AppendFormat("{0:x2}", b);

                return stringBuilder.ToString();
            }
        }
    }
}