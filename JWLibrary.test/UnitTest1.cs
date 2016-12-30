using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var encrypto = JWLibrary.Cryption.Str.CryptoFactory<JWLibrary.Cryption.Str.CryptorEngineAES128>.Encrypt(text, key);
            var decrypto = JWLibrary.Cryption.Str.CryptoFactory<JWLibrary.Cryption.Str.CryptorEngineAES128>.Decrypt(encrypto, key);

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
            ffmpegCav.FFmpegCommandExcute(null, "ffmpeg.exe", FFmpeg.BuildCommand.BuildRecordingCommand(FFmpeg.RecordingTypes.Local, model), true);

            ffmpegCav.UnRegister();

        }
    }
}
