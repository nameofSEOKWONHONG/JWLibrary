using System;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Utils {
    public static class CryptoHMAC
    {
        public static string ToHMACString(this string encData, string encKey, DeconvertCipherFormat deconvertCipherFormat)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] keyBuff = encoding.GetBytes(encKey);
            byte[] hashMessage = null;

            using(var hmacsha256 = new HMACSHA256(keyBuff))
            {
                byte[] dataBytes = encoding.GetBytes(encData);
                hashMessage = hmacsha256.ComputeHash(dataBytes);
            }

            return hashMessage.jToHMACString();
        }

        public static byte[] jToHMACBytes(this string cipherText, DeconvertCipherFormat outputFormat) {
            byte[] decodeText = null;
            switch (outputFormat) {
                case DeconvertCipherFormat.HEX:
                    decodeText = jToByte(cipherText);
                    break;
                case DeconvertCipherFormat.Base64:
                    decodeText = Convert.FromBase64String(cipherText);
                    break;
                default:
                    throw new Exception("not implement exception.");
            }

            return decodeText;
        }

        public static byte[] jToByte(this string hexString) {
            try {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++) {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return bytes;
            } catch {
                return null;
            }
        }

        public static string jToHMACString(this byte[] hashMessage) {
            string sbinary = string.Empty;

            for (int i = 0; i < hashMessage.Length; i++) {
                sbinary += hashMessage[i].ToString("X2");
            }

            return sbinary;
        }
    }

    public enum DeconvertCipherFormat {
        Base64,
        HEX
    }
}
