using System;

namespace JWLibrary.Core.NetFramework.Cryption.Str {

    /// <summary>
    /// usage : you must use app.config or web.config file.
    /// appsettings node to write this, add key="SecurityKey" value="Syed Moshiur Murshed"
    /// (Must : SecurityKey must set 24bytes)
    /// If not use appsettings, you must write to encryptKey.
    /// </summary>
    public sealed class CryptoFactory<T> where T : ICrypto {

        public static string Encrypt(string encryptText, string encryptKey = null, bool useHashing = false) {
            var type = typeof(T);

            ICrypto crypto = (ICrypto)Activator.CreateInstance(type);

            if (crypto != null) {
                return crypto.Encrypt(encryptText, encryptKey, useHashing);
            }

            return null;
        }

        public static string Decrypt(string encryptedText, string encryptKey = null, bool useHashing = false) {
            var type = typeof(T);

            ICrypto crypto = (ICrypto)Activator.CreateInstance(type);

            if (crypto != null) {
                return crypto.Decrypt(encryptedText, encryptKey, useHashing);
            }

            return null;
        }
    }

    public enum SecurityType {

        /// <summary>
        /// SHA256
        /// </summary>
        PSWD,

        /// <summary>
        /// SHA512
        /// </summary>
        PSWD2,

        /// <summary>
        /// AES128
        /// </summary>
        SSN,

        /// <summary>
        /// AES256
        /// </summary>
        SSN2,

        /// <summary>
        /// MD5
        /// </summary>
        SSN3
    }
}