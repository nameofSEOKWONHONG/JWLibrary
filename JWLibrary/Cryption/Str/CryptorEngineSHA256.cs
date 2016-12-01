using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Cryption.Str
{
	public class CryptorEngineSHA256 : ICrypto
    {
		public string Encrypt(string encryptText, string encryptKey = null, bool useHashing = false) {
			SHA256 sha = new SHA256Managed();
			byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(encryptText));

			StringBuilder stringBuilder = new StringBuilder();

			foreach (byte b in hash) {
				stringBuilder.AppendFormat("{0:x2}", b);
			}

			return stringBuilder.ToString();
		}

		public string Decrypt(string decryptText, string decryptKey = null, bool useHashing = false) {
			throw new NotImplementedException();
		}
	}
}
