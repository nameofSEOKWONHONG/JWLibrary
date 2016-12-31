using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.Cryption.Str
{
	public class CryptorEngineAES256 : ICrypto
    {
		#region low speed and random value, so return value is not match. single string only
		public string Encrypt(string plainText, string password) {
			if (plainText == null) {
				return null;
			}

			if (password == null) {
				password = String.Empty;
			}

			System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();

			if (string.IsNullOrEmpty(password))
				password = (string)settingsReader.GetValue("SecurityKey", typeof(String));

			// Get the bytes of the string
			var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
			var passwordBytes = Encoding.UTF8.GetBytes(password);

			// Hash the password with SHA256
			passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

			var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

			return Convert.ToBase64String(bytesEncrypted);
		}

		public string Decrypt(string encryptedText, string password) {
			if (encryptedText == null) {
				return null;
			}

			if (password == null) {
				password = String.Empty;
			}

			System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();

			if (string.IsNullOrEmpty(password))
				password = (string)settingsReader.GetValue("SecurityKey", typeof(String));

			// Get the bytes of the string
			var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
			var passwordBytes = Encoding.UTF8.GetBytes(password);

			passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

			var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

			return Encoding.UTF8.GetString(bytesDecrypted);
		}

		private byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes) {
			byte[] encryptedBytes = null;

			// Set your salt here, change it to meet your flavor:
			// The salt bytes must be at least 8 bytes.
			var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

			using (MemoryStream ms = new MemoryStream()) {
				using (RijndaelManaged AES = new RijndaelManaged()) {
					var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

					AES.KeySize = 256;
					AES.BlockSize = 128;
					AES.Key = key.GetBytes(AES.KeySize / 8);
					AES.IV = key.GetBytes(AES.BlockSize / 8);

					AES.Mode = CipherMode.CBC;

					using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write)) {
						cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
						cs.Close();
					}

					encryptedBytes = ms.ToArray();
				}
			}

			return encryptedBytes;
		}

		private byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes) {
			byte[] decryptedBytes = null;

			// Set your salt here, change it to meet your flavor:
			// The salt bytes must be at least 8 bytes.
			var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

			using (MemoryStream ms = new MemoryStream()) {
				using (RijndaelManaged AES = new RijndaelManaged()) {
					var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

					AES.KeySize = 256;
					AES.BlockSize = 128;
					AES.Key = key.GetBytes(AES.KeySize / 8);
					AES.IV = key.GetBytes(AES.BlockSize / 8);
					AES.Mode = CipherMode.CBC;

					using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write)) {
						cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
						cs.Close();
					}

					decryptedBytes = ms.ToArray();
				}
			}

			return decryptedBytes;
		}
		#endregion

		public string Encrypt(string encryptText, string encryptKey = null, bool useHashing = false) {
			RijndaelManaged RijndaelCipher = new RijndaelManaged();

			// 입력받은 문자열을 바이트 배열로 변환  
			byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(encryptText);

			// 딕셔너리 공격을 대비해서 키를 더 풀기 어렵게 만들기 위해서   
			// Salt를 사용한다.  
			byte[] Salt = Encoding.ASCII.GetBytes(encryptKey.Length.ToString());

			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(encryptKey, Salt);

			// Create a encryptor from the existing SecretKey bytes.  
			// encryptor 객체를 SecretKey로부터 만든다.  
			// Secret Key에는 32바이트  
			// Initialization Vector로 16바이트를 사용  
			ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

			MemoryStream memoryStream = new MemoryStream();

			// CryptoStream객체를 암호화된 데이터를 쓰기 위한 용도로 선언  
			CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

			cryptoStream.Write(PlainText, 0, PlainText.Length);

			cryptoStream.FlushFinalBlock();

			byte[] CipherBytes = memoryStream.ToArray();

			memoryStream.Close();
			cryptoStream.Close();

			string EncryptedData = Convert.ToBase64String(CipherBytes);

			return EncryptedData;
		}

		public string Decrypt(string decryptText, string decryptKey = null, bool useHashing = false) {
			RijndaelManaged RijndaelCipher = new RijndaelManaged();

			byte[] EncryptedData = Convert.FromBase64String(decryptText);
			byte[] Salt = Encoding.ASCII.GetBytes(decryptKey.Length.ToString());

			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(decryptKey, Salt);

			// Decryptor 객체를 만든다.  
			ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

			MemoryStream memoryStream = new MemoryStream(EncryptedData);

			// 데이터 읽기 용도의 cryptoStream객체  
			CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

			// 복호화된 데이터를 담을 바이트 배열을 선언한다.  
			byte[] PlainText = new byte[EncryptedData.Length];

			int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

			memoryStream.Close();
			cryptoStream.Close();

			string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

			return DecryptedData;
		}
	}
}
