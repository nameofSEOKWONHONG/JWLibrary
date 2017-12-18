using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.Cryption.HMAC
{
    public class CryptoHMAC
    {
        private DeconvertCipherFormat _deconvertCipherFormat;

        public CryptoHMAC(DeconvertCipherFormat deconvertCipherFormat)
        {
            _deconvertCipherFormat = deconvertCipherFormat;
        }

        public string GetHMACValue(string encData, string encKey)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] keyBuff = encoding.GetBytes(encKey);
            byte[] hashMessage = null;

            using(var hmacsha256 = new HMACSHA256(keyBuff))
            {
                byte[] dataBytes = encoding.GetBytes(encData);
                hashMessage = hmacsha256.ComputeHash(dataBytes);
            }

            return HMACCipherCommon.ByteToString(hashMessage);
        }
    }
}
