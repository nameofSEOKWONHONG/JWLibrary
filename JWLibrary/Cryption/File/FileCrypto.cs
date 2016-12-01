//http://h5bak.tistory.com/148

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Cryption.File
{
	public class Crypto {
		/// <summary>
		/// AES 256 암호화
		/// </summary>
		/// <param name="inputFile"></param>
		/// <param name="outputFile"></param>
		/// <param name="sKey"></param>
		public static void EncryptFile(string inputFile, string outputFile, string sKey) {
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] key = UE.GetBytes(sKey);

			string cryptFile = outputFile;

			using (FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create)) {
				RijndaelManaged RMCrypto = new RijndaelManaged();

				using (CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write)) {
					using (FileStream fsIn = new FileStream(inputFile, FileMode.Open)) {
						int data;
						while ((data = fsIn.ReadByte()) != -1)
							cs.WriteByte((byte)data);

						fsIn.Close();
					}
					cs.Close();
				}
				fsCrypt.Close();
			}
		}

		/// <summary>
		/// AES 256 복호화
		/// </summary>
		/// <param name="inputFile"></param>
		/// <param name="outputFile"></param>
		/// <param name="sKey"></param>
		public static void DecryptFile(string inputFile, string outputFile, string sKey) {
			UnicodeEncoding UE = new UnicodeEncoding();
			byte[] key = UE.GetBytes(sKey);

			using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open)) {
				RijndaelManaged RMCrypto = new RijndaelManaged();

				using (CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read)) {
					using (FileStream fsOut = new FileStream(outputFile, FileMode.Create)) {
						int data;
						while ((data = cs.ReadByte()) != -1)
							fsOut.WriteByte((byte)data);

						fsOut.Close();
					}

					cs.Close();
				}
				fsCrypt.Close();
			}
		}
	}
}
