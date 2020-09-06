using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace JWLibrary.Core.NetFramework.GZip {

    public class GZipStringCompress : ICompress, IDisposable {

        public string Compress(string param) {
            if (param.Length <= 0) return string.Empty;

            var rowData = Encoding.UTF8.GetBytes(param);
            byte[] compressed = null;
            using (var outStream = new MemoryStream()) {
                using (var hgs = new GZipStream(outStream, CompressionMode.Compress)) {
                    hgs.Write(rowData, 0, rowData.Length);
                }
                compressed = outStream.ToArray();
            }

            return Convert.ToBase64String(compressed);
        }

        public string Decompress(string param) {
            if (param.Length <= 0) return string.Empty;

            byte[] cmpData = Convert.FromBase64String(param);
            string output = null;
            using (var decomStream = new MemoryStream(cmpData)) {
                using (var hgs = new GZipStream(decomStream, CompressionMode.Decompress)) {
                    //decomStream에 압축 헤제된 데이타를 저장한다.
                    using (var reader = new StreamReader(hgs)) {
                        output = reader.ReadToEnd();
                    }
                }
            }

            return output;
        }

        #region IDisposable Members

        /// <summary>
        /// Internal variable which checks if Dispose has already been called
        /// </summary>
        private Boolean disposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(Boolean disposing) {
            if (disposed) {
                return;
            }

            if (disposing) {
                //TODO: Managed cleanup code here, while managed refs still valid
            }
            //TODO: Unmanaged cleanup code here

            disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            // Call the private Dispose(bool) helper and indicate
            // that we are explicitly disposing
            this.Dispose(true);

            // Tell the garbage collector that the object doesn't require any
            // cleanup when collected since Dispose was called explicitly.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The destructor for the class.
        /// </summary>
        ~GZipStringCompress() {
            this.Dispose(false);
        }

        #endregion IDisposable Members
    }
}