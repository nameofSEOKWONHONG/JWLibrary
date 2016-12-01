using System.IO;

namespace JWLibrary.FileIdentify
{
    public class FileUniqueId
    {
        public ulong ApproachB(string fileName)
        {
            WIN32.Interop.Win32Apis.BY_HANDLE_FILE_INFORMATION objectFileInfo = new WIN32.Interop.Win32Apis.BY_HANDLE_FILE_INFORMATION();

            FileInfo fi = new FileInfo(fileName);

            ulong fileIndex = 0;

            try
            {
                if (fi.Exists)
                {
                    using (FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        WIN32.Interop.Win32Apis.GetFileInformationByHandle(fs.Handle, out objectFileInfo);

                        fs.Close();

                        fileIndex = ((ulong)objectFileInfo.FileIndexHigh << 32) + (ulong)objectFileInfo.FileIndexLow;
                    }
                }
            }
            catch
            {
                throw;
            }

            fi = null;

            return fileIndex;
        }
    }
}
