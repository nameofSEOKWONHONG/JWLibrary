using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JWLibrary.test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            var encText = "test";
            //var encKey = "";
            var encKey = "";
            var encKeyIV = "";

            JWLibrary.Core.Cryption.HMAC.CryptoHMAC hmac = new Core.Cryption.HMAC.CryptoHMAC(Core.Cryption.HMAC.DeconvertCipherFormat.Base64);
            var signiture = hmac.GetHMACValue(encText, encKey);
            Assert.AreEqual(signiture, hmac.GetHMACValue(encText, encKey));

            JWLibrary.Core.Cryption.HMAC.AES256Cipher aES256Cipher = new Core.Cryption.HMAC.AES256Cipher();
            var enc = aES256Cipher.EncodeAES256(encText, encKey, encKeyIV, System.Security.Cryptography.CipherMode.ECB, System.Security.Cryptography.PaddingMode.PKCS7);
            var dec = aES256Cipher.DecodeAES256(enc, encKey, encKeyIV, System.Security.Cryptography.CipherMode.ECB, System.Security.Cryptography.PaddingMode.PKCS7, Core.Cryption.HMAC.DeconvertCipherFormat.HEX);

            Assert.AreEqual(encText, dec);
        }
    }
}
