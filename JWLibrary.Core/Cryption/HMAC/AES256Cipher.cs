using System;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.Cryption.HMAC
{
    public class AES256Cipher
    {
        public string EncodeAES256(string plainText, string cipherKey, string cipherIV, CipherMode cipherMode, PaddingMode paddingMode)
        {
            byte[] byteKey = Encoding.UTF8.GetBytes(cipherKey);
            byte[] byteIV = Encoding.UTF8.GetBytes(cipherIV);

            string strEncode = string.Empty;
            byte[] bytePlainText = Encoding.UTF8.GetBytes(plainText);

            using(AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider())
            {
                try
                {
                    aesCryptoProvider.Mode = cipherMode;
                    aesCryptoProvider.Padding = paddingMode;
                    return HMACCipherCommon.ByteToString(aesCryptoProvider.CreateEncryptor(byteKey, byteIV).TransformFinalBlock(bytePlainText, 0, bytePlainText.Length));
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public string DecodeAES256(string cipherText, string cipherKey, string cipherIV, CipherMode cipherMode, PaddingMode paddingMode, DeconvertCipherFormat format)
        {
            byte[] byteKey = Encoding.UTF8.GetBytes(cipherKey);
            byte[] byteIV = Encoding.UTF8.GetBytes(cipherIV);
            byte[] byteBuff = HMACCipherCommon.DeConvertCipherText(cipherText, format);
            using(AesCryptoServiceProvider aesCrytoProvider = new AesCryptoServiceProvider())
            {
                try
                {
                    aesCrytoProvider.Mode = cipherMode;
                    aesCrytoProvider.Padding = paddingMode;
                    var dec = aesCrytoProvider.CreateDecryptor(byteKey, byteIV).TransformFinalBlock(byteBuff, 0, byteBuff.Length);
                    return Encoding.UTF8.GetString(dec);
                }
                catch(Exception e)
                {
                    return string.Empty;
                }
            }
        }
    }
}
