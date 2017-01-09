using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.Cryption.Str
{
	public class CryptorEngineMD5 : ICrypto
    {
		public string Encrypt(string encryptText, string encryptKey = null, bool useHashing = false) {
			byte[] keyArray;
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(encryptText);

			System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();

			if (string.IsNullOrEmpty(encryptKey))
				encryptKey = (string)settingsReader.GetValue("SecurityKey", typeof(string));

			if (useHashing) {
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptKey));
				hashmd5.Clear();
			}
			else
				keyArray = UTF8Encoding.UTF8.GetBytes(encryptKey);

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = tdes.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			tdes.Clear();
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}

		public string Decrypt(string decryptText, string decryptKey = null, bool useHashing = false) {
			byte[] keyArray;
			byte[] toEncryptArray = Convert.FromBase64String(decryptText);

			System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();

			if (string.IsNullOrEmpty(decryptKey)) {
				decryptKey = (string)settingsReader.GetValue("SecurityKey", typeof(string));
			}

			if (useHashing) {
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(decryptKey));
				hashmd5.Clear();
			}
			else
				keyArray = UTF8Encoding.UTF8.GetBytes(decryptKey);

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			tdes.Key = keyArray;
			tdes.Mode = CipherMode.ECB;
			tdes.Padding = PaddingMode.PKCS7;

			ICryptoTransform cTransform = tdes.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

			tdes.Clear();
			return UTF8Encoding.UTF8.GetString(resultArray);
		}
	}
}
