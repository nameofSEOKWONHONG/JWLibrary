using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Utils {

    public static class CryptoSHA512 {

        /// <summary>
        /// SHA512 Encrypt (Decrypt is not support.)
        /// </summary>
        /// <param name="encryptText"></param>
        /// <param name="encryptKey"></param>
        /// <param name="useHashing">not use</param>
        /// <returns></returns>
        public static string ToSHA512(this string encryptText) {
            using (var sha512 = SHA512.Create()) {
                byte[] bytes = Encoding.UTF8.GetBytes(encryptText);
                byte[] hash = sha512.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++) {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}