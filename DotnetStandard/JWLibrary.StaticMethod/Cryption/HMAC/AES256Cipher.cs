using System;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.Cryption.HMAC
{
    public class GenericBaseClass<T>
    {

    }

    public static class AES256Instance
    {
        public static string Encode(this string plainText, string cipherKey, string cipherIV, CipherMode cipherMode, PaddingMode paddingMode)
        {
            using (var aes256 = new AES256Cipher())
            {
                return aes256.Encode(plainText, cipherKey, cipherIV, cipherMode, paddingMode);
            }
        }

        public static string Decode(this string cipherText, string cipherKey, string cipherIV, CipherMode cipherMode, PaddingMode paddingMode, DeconvertCipherFormat format)
        {
            using (var aes256 = new AES256Cipher())
            {
                return aes256.Decode(cipherText, cipherKey, cipherIV, cipherMode, paddingMode, format);
            }
        }
    }

    internal sealed class AES256Cipher : IDisposable
    {
        internal AES256Cipher() { }

        public string Encode(string plainText, string cipherKey, string cipherIV, CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] byteKey = Encoding.UTF8.GetBytes(cipherKey);
            byte[] byteIV = Encoding.UTF8.GetBytes(cipherIV);

            string strEncode = string.Empty;
            byte[] bytePlainText = Encoding.UTF8.GetBytes(plainText);

            using (AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider())
            {
                try
                {
                    aesCryptoProvider.Mode = cipherMode;
                    aesCryptoProvider.Padding = paddingMode;
                    return HMACCipher.jToString(aesCryptoProvider.CreateEncryptor(byteKey, byteIV).TransformFinalBlock(bytePlainText, 0, bytePlainText.Length));
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public string Decode(string cipherText, string cipherKey, string cipherIV, CipherMode cipherMode, PaddingMode paddingMode, DeconvertCipherFormat format)
        {
            byte[] byteKey = Encoding.UTF8.GetBytes(cipherKey);
            byte[] byteIV = Encoding.UTF8.GetBytes(cipherIV);
            byte[] byteBuff = HMACCipher.jToDecode(cipherText, format);
            using (AesCryptoServiceProvider aesCrytoProvider = new AesCryptoServiceProvider())
            {
                try
                {
                    aesCrytoProvider.Mode = cipherMode;
                    aesCrytoProvider.Padding = paddingMode;
                    var dec = aesCrytoProvider.CreateDecryptor(byteKey, byteIV).TransformFinalBlock(byteBuff, 0, byteBuff.Length);
                    return Encoding.UTF8.GetString(dec);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    aesCrytoProvider.Dispose();
                }
            }
        }

        public void Dispose()
        {
            //do nothing...
        }
    }


}
