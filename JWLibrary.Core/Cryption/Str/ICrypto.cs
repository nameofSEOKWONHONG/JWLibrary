using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWLibrary.Core.Cryption.Str
{
	public interface ICrypto {
		string Encrypt(string encryptText, string encryptKey = null, bool useHashing = false);
		string Decrypt(string decryptText, string decryptKey = null, bool useHashing = false);
	}
}
