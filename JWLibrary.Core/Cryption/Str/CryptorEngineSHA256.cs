using System;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Core.Cryption.Str
{
    public class CryptorEngineSHA256 : ICrypto
    {
        /// <summary>
        /// SHA256 Encrypt (Decrypt is not support.)
        /// </summary>
        /// <param name="encryptText"></param>
        /// <param name="encryptKey"></param>
        /// <param name="useHashing">not use</param>
        /// <returns></returns>
		public string Encrypt(string encryptText, string encryptKey = null, bool useHashing = false) {
			SHA256 sha = new SHA256Managed();
			byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(encryptText));

			StringBuilder stringBuilder = new StringBuilder();

			foreach (byte b in hash) {
				stringBuilder.AppendFormat("{0:x2}", b);
			}

			return stringBuilder.ToString();
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
