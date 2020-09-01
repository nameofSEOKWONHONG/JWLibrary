using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.NetFramework.Cryption.Str
{
    public class CryptorEngineAES128 : ICrypto {
        /// <summary>
        /// AES128 Encrypt
        /// </summary>
        /// <param name="encryptText"></param>
        /// <param name="encryptKey"></param>
        /// <param name="useHashing">not use</param>
        /// <returns></returns>
		public string Encrypt(string encryptText, string encryptKey = null, bool useHashing = false) {
			RijndaelManaged RijndaelCipher = new RijndaelManaged();

			byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(encryptText);
			byte[] Salt = Encoding.ASCII.GetBytes(encryptKey.Length.ToString());

			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(encryptKey, Salt);
			ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

			cryptoStream.Write(PlainText, 0, PlainText.Length);
			cryptoStream.FlushFinalBlock();

			byte[] CipherBytes = memoryStream.ToArray();

			memoryStream.Close();
			cryptoStream.Close();

			string EncryptedData = Convert.ToBase64String(CipherBytes);

			return EncryptedData;
		}

        /// <summary>
        /// AES128 Decrypt
        /// </summary>
        /// <param name="decryptText"></param>
        /// <param name="decryptKey"></param>
        /// <param name="useHashing">not use</param>
        /// <returns></returns>
		public string Decrypt(string decryptText, string decryptKey = null, bool useHashing = false) {
			RijndaelManaged RijndaelCipher = new RijndaelManaged();

			byte[] EncryptedData = Convert.FromBase64String(decryptText);
			byte[] Salt = Encoding.ASCII.GetBytes(decryptKey.Length.ToString());

			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(decryptKey, Salt);
			ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
			MemoryStream memoryStream = new MemoryStream(EncryptedData);
			CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

			byte[] PlainText = new byte[EncryptedData.Length];

			int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

			memoryStream.Close();
			cryptoStream.Close();

			string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

			return DecryptedData;
		}
	}
}
