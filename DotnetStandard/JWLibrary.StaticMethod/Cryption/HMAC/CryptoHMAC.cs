using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.Cryption.HMAC
{
    public static class CryptoHMAC
    {
        public static string GetHMACValue(this string encData, string encKey, DeconvertCipherFormat deconvertCipherFormat)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] keyBuff = encoding.GetBytes(encKey);
            byte[] hashMessage = null;

            using(var hmacsha256 = new HMACSHA256(keyBuff))
            {
                byte[] dataBytes = encoding.GetBytes(encData);
                hashMessage = hmacsha256.ComputeHash(dataBytes);
            }

            return hashMessage.jToString();
        }
    }
}
