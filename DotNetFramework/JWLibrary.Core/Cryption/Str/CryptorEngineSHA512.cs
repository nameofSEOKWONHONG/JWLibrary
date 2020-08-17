using System;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.Cryption.Str
{
    public class CryptorEngineSHA512 : ICrypto
    {
        /// <summary>
        /// SHA512 Encrypt (Decrypt is not support.)
        /// </summary>
        /// <param name="encryptText"></param>
        /// <param name="encryptKey"></param>
        /// <param name="useHashing">not use</param>
        /// <returns></returns>
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

        /// <summary>
        /// do not implemented.
        /// </summary>
        /// <param name="decryptText"></param>
        /// <param name="decryptKey"></param>
        /// <param name="useHashing"></param>
        /// <returns></returns>
		public string Decrypt(string decryptText, string decryptKey = null, bool useHashing = false) {
			throw new NotImplementedException();
		}
	}
}
