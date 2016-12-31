using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.Cryption.Str
{
	public class CryptorEngineSHA512 : ICrypto
    {
		public string Encrypt(string encryptText, string encryptKey = null, bool useHashing = false) {
			SHA512 sha512 = new SHA512Managed();
			byte[] bytes = Encoding.UTF8.GetBytes(encryptText);
			byte[] hash = sha512.ComputeHash(bytes);
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < hash.Length; i++) {
				sb.Append(hash[i].ToString("X2"));
			}

			return sb.ToString();
		}

		public string Decrypt(string decryptText, string decryptKey = null, bool useHashing = false) {
			throw new NotImplementedException();
		}
	}
}
