﻿using System;

namespace JWLibrary.Core.Cryption.HMAC
{
    public class HMACCipherCommon
    {
        public static byte[] DeConvertCipherText(string cipherText, DeconvertCipherFormat outputFormat)
        {
            byte[] decodeText = null;
            switch (outputFormat)
            {
                case DeconvertCipherFormat.HEX:
                    decodeText = HexStringToByte(cipherText);
                    break;
                case DeconvertCipherFormat.Base64:
                    decodeText = Convert.FromBase64String(cipherText);
                    break;
                default:
                    throw new Exception("not implement exception.");
            }

            return decodeText;
        }

        public static byte[] HexStringToByte(string hexString)
        {
            try
            {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return bytes;
            }
            catch
            {
                return null;
            }
        }

        public static string ByteToString(byte[] hashMessage)
        {
            string sbinary = string.Empty;

            for (int i = 0; i < hashMessage.Length; i++)
            {
                sbinary += hashMessage[i].ToString("X2");
            }

            return sbinary;
        }
    }

    public enum DeconvertCipherFormat
    {
        Base64,
        HEX
    }
}
