using System;
using System.IO;
using System.Runtime.InteropServices;

namespace JWLibrary.Core.NetFramework.FileIdentify {

    public class FileUniqueId {

        public ulong ApproachB(string fileName) {
            Win32.BY_HANDLE_FILE_INFORMATION objectFileInfo = new Win32.BY_HANDLE_FILE_INFORMATION();

            FileInfo fi = new FileInfo(fileName);

            ulong fileIndex = 0;

            try {
                if (fi.Exists) {
                    using (FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        Win32.GetFileInformationByHandle(fs.Handle, out objectFileInfo);

                        fs.Close();

                        fileIndex = ((ulong)objectFileInfo.FileIndexHigh << 32) + (ulong)objectFileInfo.FileIndexLow;
                    }
                }
            } catch {
                throw;
            }

            fi = null;

            return fileIndex;
        }
    }

    internal class Win32 {

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern IntPtr NtQueryInformationFile(IntPtr fileHandle, ref IO_STATUS_BLOCK IoStatusBlock, IntPtr pInfoBlock, uint length, FILE_INFORMATION_CLASS fileInformation);

        public struct IO_STATUS_BLOCK {
            private uint status;
            private ulong information;
        }

        public struct _FILE_INTERNAL_INFORMATION {
            public ulong IndexNumber;
        }

        // Abbreviated, there are more values than shown
        public enum FILE_INFORMATION_CLASS {
            FileDirectoryInformation = 1,     // 1
            FileFullDirectoryInformation,     // 2
            FileBothDirectoryInformation,     // 3
            FileBasicInformation,         // 4
            FileStandardInformation,      // 5
            FileInternalInformation      // 6
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileInformationByHandle(IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        public struct BY_HANDLE_FILE_INFORMATION {
            public uint FileAttributes;
            public FILETIME CreationTime;
            public FILETIME LastAccessTime;
            public FILETIME LastWriteTime;
            public uint VolumeSerialNumber;
            public uint FileSizeHigh;
            public uint FileSizeLow;
            public uint NumberOfLinks;
            public uint FileIndexHigh;
            public uint FileIndexLow;
        }
    }
}