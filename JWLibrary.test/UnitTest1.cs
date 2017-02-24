using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JWLibrary.Core.Cryption.Str;
using System.Diagnostics;
using System.Text;

namespace JWLibrary.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var text = "hello world";
            var key = "(C)StudioStoneCircle.Inc.";
            var encrypto = CryptoFactory<CryptorEngineAES128>.Encrypt(text, key);
            var decrypto = CryptoFactory<CryptorEngineAES128>.Decrypt(encrypto, key);

            Assert.AreEqual(text, decrypto);
        }

        [TestMethod]
        public void TestMethod2()
        {
            JWLibrary.FFmpeg.FFMpegCaptureAV ffmpegCav = new FFmpeg.FFMpegCaptureAV();
            ffmpegCav.Initialize();
            ffmpegCav.Register();

            var model = new FFmpeg.FFmpegCommandModel()
            {

            };
            //ffmpegCav.FFmpegCommandExcute(null, "ffmpeg.exe", FFmpeg.BuildCommand.BuildRecordingCommand(FFmpeg.RecordingTypes.Local, model), true);

            ffmpegCav.UnRegister();

        }

        [TestMethod]
        public void TestMethod3()
        {
            byte[] pbData = Encoding.UTF8.GetBytes("HelloWorld");
            /*byte[] pbData = {(byte)0x00, (byte)0x01, (byte)0x02, (byte)0x03, (byte)0x04, (byte)0x05, (byte)0x06, (byte)0x07,
                            (byte)0x08, (byte)0x09, (byte)0x0A, (byte)0x0B, (byte)0x0C, (byte)0x0D, (byte)0x0E, (byte)0x0F,
                            (byte)0x08, (byte)0x09, (byte)0x0A, (byte)0x0B, (byte)0x0C, (byte)0x0D, (byte)0x0E, (byte)0x0F,
                            (byte)0x00, (byte)0x01, (byte)0x02, (byte)0x03, (byte)0x04, (byte)0x05, (byte)0x06, (byte)0x07,
                            (byte)0x08, (byte)0x09, (byte)0x0A, (byte)0x0B, (byte)0x0C, (byte)0x0D, (byte)0x0E, (byte)0x0F,
                            (byte)0x08, (byte)0x09, (byte)0x0A, (byte)0x0B, (byte)0x0C, (byte)0x0D, (byte)0x0E, (byte)0x0F};*/
            byte[] pbData1 = { (byte)0x61 };

            byte[] pbCipher = new byte[32];
            byte[] pbPlain = new byte[16];

            Debug.WriteLine("[ Test SHA256 reference code ]" + "\n");
            Debug.WriteLine("\n\n");
            Debug.WriteLine("[ Test HASH mode ]" + "\n");
            Debug.WriteLine("\n");

            int Plaintext_length = 1;

            for (int k = 0; k < pbData.Length; k++)
            {

                Debug.WriteLine("Plaintext\t: ");
                for (int i = 0; i < Plaintext_length; i++) Debug.WriteLine((0xff & pbData[i]).ToString("X4") + " ");
                Debug.WriteLine("\n");

                // Encryption	
                JWLibrary.Core.Cryption.KISA.CryptoEngineSHA256.SHA256_Encrpyt(pbData, Plaintext_length, pbCipher);

                Debug.WriteLine("Ciphertext\t: ");
                for (int i = 0; i < 32; i++) Debug.WriteLine((0xff & pbCipher[i]).ToString("X4") + " ");
                Debug.WriteLine("\n\n");

                Plaintext_length++;

            }

            Debug.WriteLine(Encoding.UTF8.GetString(pbData));

            Debug.WriteLine("Plaintext\t: ");
            for (int i = 0; i < 1; i++) Debug.WriteLine((0xff & pbData1[i]).ToString("X4") + " ");
            Debug.WriteLine("\n");
            // Encryption			    
            JWLibrary.Core.Cryption.KISA.CryptoEngineSHA256.SHA256_Encrpyt(pbData1, 1, pbCipher);
            Debug.WriteLine("Ciphertext\t: ");
            for (int i = 0; i < 32; i++) Debug.WriteLine((0xff & pbCipher[i]).ToString("X4") + " ");
            Debug.WriteLine("\n\n");
        }
    }
}
