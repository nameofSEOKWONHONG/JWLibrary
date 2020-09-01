using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.NetFramework.GZip
{
	public class GZipFileCompress : ICompress, IDisposable {		
		public string Compress (string param) {
			var fileToCompress = new FileInfo(param);
			if (!fileToCompress.Exists) return string.Empty;

			using (FileStream originalFileStream = fileToCompress.OpenRead()) {
				using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz")) {
					using (GZipStream compressionStream = new GZipStream(compressedFileStream,
					   CompressionMode.Compress)) {
						originalFileStream.CopyTo(compressionStream);
					}
				}
			}
			return fileToCompress.FullName + ".gz";
		}

		public string Decompress(string param) {
			var fileToDecompress = new FileInfo(param);
			if (!fileToDecompress.Exists) return string.Empty;

			using (FileStream originalFileStream = fileToDecompress.OpenRead()) {
				string currentFileName = fileToDecompress.FullName;
				string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

				using (FileStream decompressedFileStream = File.Create(newFileName)) {
					using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress)) {
						decompressionStream.CopyTo(decompressedFileStream);
					}
				}
			}
			return fileToDecompress.Name;
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
		protected virtual void Dispose (Boolean disposing) {
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
		public void Dispose () {
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
		~GZipFileCompress () {
			this.Dispose(false);
		}


		#endregion
	}
}
