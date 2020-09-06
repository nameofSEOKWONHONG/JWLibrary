﻿using System;

namespace JWLibrary.Core.NetFramework.GZip {

    public sealed class GZipCompressFactory<T> where T : ICompress {

        public static string Compress(string param) {
            string ret = string.Empty;

            var type = typeof(T);

            ICompress compress = (ICompress)Activator.CreateInstance(type);

            if (compress != null) {
                ret = compress.Compress(param);
                IDisposable dispose = compress as IDisposable;
                if (dispose != null) {
                    dispose.Dispose();
                }
            }

            return ret;
        }

        public static string DeCompress(string param) {
            string ret = string.Empty;

            var type = typeof(T);

            ICompress compress = (ICompress)Activator.CreateInstance(type);

            if (compress != null) {
                ret = compress.Decompress(param);
                IDisposable dispose = compress as IDisposable;
                if (dispose != null) {
                    dispose.Dispose();
                }
            }

            return ret;
        }
    }
}