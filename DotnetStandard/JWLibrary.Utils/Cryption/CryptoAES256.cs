using System;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Utils {
    public static class CryptoAES256
    {
        public static string ToEncAES256(this string plainText, string cipherKey, string cipherIV, CipherMode cipherMode, PaddingMode paddingMode)
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
                    var result = aesCryptoProvider.CreateEncryptor(byteKey, byteIV).TransformFinalBlock(bytePlainText, 0, bytePlainText.Length);
                    return result.jToHMACString();
                }
                catch
                {
                    throw;
                }
                finally {
                    aesCryptoProvider.Dispose();
                }
            }
        }

        public static string ToDecAES256(this string cipherText, string cipherKey, string cipherIV, CipherMode cipherMode, PaddingMode paddingMode, DeconvertCipherFormat format)
        {
            byte[] byteKey = Encoding.UTF8.GetBytes(cipherKey);
            byte[] byteIV = Encoding.UTF8.GetBytes(cipherIV);
            byte[] byteBuff = cipherText.jToHMACBytes(format);
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
    }
}
