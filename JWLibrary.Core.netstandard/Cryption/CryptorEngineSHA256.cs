using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.NetStandard.Cryption
{
    public class CryptorEngineSHA256
    {
        private CryptorEngineSHA256() { }

        /// <summary>
        /// SHA256 Encrypt (Decrypt is not support.)
        /// </summary>
        /// <param name="encryptText"></param>
        /// <param name="encryptKey"></param>
        /// <param name="useHashing">not use</param>
        /// <returns></returns>
		public string Encrypt(string encryptText)
        {
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(encryptText));

                StringBuilder stringBuilder = new StringBuilder();

                foreach (byte b in hash)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }

                return stringBuilder.ToString();
            }
        }
    }
}
