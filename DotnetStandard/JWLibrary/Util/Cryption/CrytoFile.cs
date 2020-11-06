/*
 * refer from : http://h5bak.tistory.com/148
 */

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JWLibrary.Utils {

    public static class CrytoFile {

        public static void EncryptFile(string inputFile, string outputFile, string sKey) {
            var UE = new UnicodeEncoding();
            var key = UE.GetBytes(sKey);

            var cryptFile = outputFile;

            using (var fsCrypt = new FileStream(cryptFile, FileMode.Create)) {
                var RMCrypto = new RijndaelManaged();

                using (var cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write)) {
                    using (var fsIn = new FileStream(inputFile, FileMode.Open)) {
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

        public static void DecryptFile(string inputFile, string outputFile, string sKey) {
            var UE = new UnicodeEncoding();
            var key = UE.GetBytes(sKey);

            using (var fsCrypt = new FileStream(inputFile, FileMode.Open)) {
                var RMCrypto = new RijndaelManaged();

                using (var cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read)) {
                    using (var fsOut = new FileStream(outputFile, FileMode.Create)) {
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