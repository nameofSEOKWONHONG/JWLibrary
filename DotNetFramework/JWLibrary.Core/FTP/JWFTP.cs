using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JWLibrary.Core.NetFramework.FTP {

    public class FTPHanderEventArgs : EventArgs {
        public readonly long _sendByte;
        public readonly long _totalByte;
        public readonly string _fileName;

        public FTPHanderEventArgs(long sendByte, long totalByte, string fileName) {
            this._sendByte = sendByte;
            this._totalByte = totalByte;
            this._fileName = fileName;
        }
    }

    /// <summary>
    /// 프로그레스 진행을 넘겨주기 위한 이벤트 델리게이트 선언
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FTPEventHandler(object sender, FTPHanderEventArgs e);

    public class JWFTP {
        private string host = null;
        private string user = null;
        private string pass = null;
        private bool passiveMode = true;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        private int bufferSize = 2048;

        /// <summary>
        /// 델리게이트 이벤트를 선언
        /// </summary>
        public event FTPEventHandler SendChanged;

        /// <summary>
        /// 구현할 이벤트 함수 선언
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSendChanged(FTPHanderEventArgs e) {
            if (SendChanged != null) SendChanged(this, e);
        }

        /* Construct Object */

        public JWFTP(string hostIP, string userName, string password, bool passiveMode) {
            host = hostIP;
            user = userName;
            pass = password;
            this.passiveMode = passiveMode;
        }

        /* Download File */

        public void download(string remoteFile, string localFile) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                ftpStream = ftpResponse.GetResponseStream();
                /* Open a File Stream to Write the Downloaded File */
                FileStream localFileStream = new FileStream(localFile, FileMode.Create);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                try {
                    while (bytesRead > 0) {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                    }
                } catch (Exception ex) { throw ex; } finally {
                    /* Resource Cleanup */
                    localFileStream.Close();
                    ftpStream.Close();
                    ftpResponse.Close();
                    ftpRequest = null;
                }
            } catch (Exception ex) { throw ex; }
            return;
        }

        /* Upload File */

        public void upload(string remoteFile, string localFile) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                FileStream localFileStream = new FileStream(localFile, FileMode.Open, FileAccess.Read);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int totalReadBytesCount = 0;
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                totalReadBytesCount += bytesSent;
                OnSendChanged(new FTPHanderEventArgs(totalReadBytesCount, localFileStream.Length, localFile));
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try {
                    while (bytesSent != 0) {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                        totalReadBytesCount += bytesSent;
                        OnSendChanged(new FTPHanderEventArgs(totalReadBytesCount, localFileStream.Length, localFile));
                    }
                } catch (Exception ex) { throw ex; } finally {
                    /* Resource Cleanup */
                    localFileStream.Close();
                    ftpStream.Close();
                    ftpRequest = null;
                }
            } catch (Exception ex) { throw ex; }
            return;
        }

        /// <summary>
        /// upload custom function
        /// </summary>
        /// <param name="remoteFile"></param>
        /// <param name="localFile"></param>
        public void uploadByBinaryReader(string remoteFile, string localFile) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                FileStream localFileStream = new FileStream(localFile, FileMode.Open, FileAccess.Read);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int totalReadBytesCount = 0;
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                totalReadBytesCount += bytesSent;
                OnSendChanged(new FTPHanderEventArgs(totalReadBytesCount, localFileStream.Length, localFile));

                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try {
                    while (bytesSent != 0) {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                        totalReadBytesCount += bytesSent;
                        OnSendChanged(new FTPHanderEventArgs(totalReadBytesCount, localFileStream.Length, localFile));
                    }
                } catch (Exception ex) { throw ex; } finally {
                    /* Resource Cleanup */
                    localFileStream.Close();
                    ftpStream.Close();
                    ftpRequest = null;
                }
            } catch (Exception ex) { throw ex; }
            return;
        }

        /* Delete File */

        public void delete(string deleteFile) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            } catch (Exception ex) { throw ex; } finally {
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            return;
        }

        /* remove dir */

        public void removeDirectory(string dirPath) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + dirPath);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            } catch (Exception ex) { throw ex; } finally {
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            return;
        }

        /* Rename File */

        public void rename(string currentFileNameAndPath, string newFileName) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + currentFileNameAndPath);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                /* Rename the File */
                ftpRequest.RenameTo = newFileName;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            } catch (Exception ex) { throw ex; } finally {
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            return;
        }

        /* Create a New Directory on the FTP Server */

        public void createDirectory(string newDirectory) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + newDirectory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            } catch (Exception ex) { throw ex; } finally {
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            return;
        }

        /* Get the Date/Time a File was Created */

        public string getFileCreatedDateTime(string fileName) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { fileInfo = ftpReader.ReadToEnd(); } catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Created Date Time */
                return fileInfo;
            } catch (Exception ex) { throw ex; }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* Get the Size of a File */

        public string getFileSize(string fileName) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } } catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Size */
                return fileInfo;
            } catch (Exception ex) { throw ex; }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* List Directory Contents File/Folder Name Only */

        public string[] directoryListSimple(string directory) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } } catch (Exception ex) { throw ex; } finally {
                    /* Resource Cleanup */
                    ftpReader.Close();
                    ftpStream.Close();
                    ftpResponse.Close();
                    ftpRequest = null;
                }
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; } catch (Exception ex) { throw ex; ; }
            } catch (Exception ex) { throw ex; }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }

        /* List Directory Contents in Detail (Name, Size, Created, etc.) */

        public string[] directoryListDetailed(string directory) {
            try {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = this.passiveMode;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } } catch (Exception ex) { throw ex; } finally {
                    /* Resource Cleanup */
                    ftpReader.Close();
                    ftpStream.Close();
                    ftpResponse.Close();
                    ftpRequest = null;
                }
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; } catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            } catch (Exception ex) { throw ex; }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }

        /// <summary>
        /// FTP에 해당 디렉터리가 있는지 알아온다.
        /// </summary>
        /// <param name="currentDirectory">디렉터리 명</param>
        /// <returns>있으면 참</returns>
        public bool isExtistDirectory(string currentDirectory) {
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + getParentDirectory(currentDirectory));
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.Timeout = 10000;
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            string data = string.Empty;
            try {
                ftpResponse = ftpRequest.GetResponse() as FtpWebResponse;
                if (ftpResponse != null) {
                    using (StreamReader streamReader = new StreamReader(ftpResponse.GetResponseStream(), Encoding.Default)) {
                        data = streamReader.ReadToEnd();
                    }
                }
            } finally {
                if (ftpResponse != null) {
                    ftpResponse.Close();
                }
                ftpRequest = null;
            }

            string[] directorys = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return (from directory in directorys
                    select directory.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                        into directoryInfos
                    where directoryInfos[0][0] == 'd'
                    select directoryInfos[8]).Any(
                        name => name == (currentDirectory.Split('/')[currentDirectory.Split('/').Length - 1]).ToString());
        }

        /// <summary>
        /// 상위 디렉터리를 알아옵니다.
        /// </summary>
        /// <param name="currentDirectory"></param>
        /// <returns></returns>
        private string getParentDirectory(string currentDirectory) {
            string[] directorys = currentDirectory.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string parentDirectory = string.Empty;
            for (int i = 0; i < directorys.Length - 1; i++) {
                parentDirectory += "/" + directorys[i];
            }

            return parentDirectory;
        }

        /// <summary>
        /// FTP 경로의 디렉토리를 점검하고 없으면 생성
        /// </summary>
        /// <param name="directoryPath">디렉터리 경로 입니다.</param>
        public void ftpDirectioryCheck(string directoryPath) {
            string[] directoryPaths = directoryPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            string currentDirectory = string.Empty;
            foreach (string directory in directoryPaths) {
                currentDirectory += string.Format("/{0}", directory);
                if (!isExtistDirectory(currentDirectory)) {
                    createDirectory(currentDirectory);
                }
            }
        }

        /// <summary>
        /// 해당 경로에 파일이 존재하는지 여부를 가져옵니다.
        /// </summary>
        /// <param name="ftpFilePath">파일 경로</param>
        /// <returns>존재시 참 </returns>
        private bool isFTPFileExsit(string ftpFilePath) {
            string fileName = getFileName(ftpFilePath);

            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + getDirectoryPath(ftpFilePath));
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.Timeout = 10000;
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            string data = string.Empty;
            try {
                ftpResponse = ftpRequest.GetResponse() as FtpWebResponse;
                if (ftpResponse != null) {
                    using (StreamReader streamReader = new StreamReader(ftpResponse.GetResponseStream(), Encoding.Default)) {
                        data = streamReader.ReadToEnd();
                    }
                }
            } finally {
                if (ftpResponse != null) {
                    ftpResponse.Close();
                }

                ftpRequest = null;
            }

            string[] directorys = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return (from directory in directorys
                    select directory.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                        into directoryInfos
                    where directoryInfos[0][0] != 'd'
                    select directoryInfos[8]).Any(
                        name => name == fileName);
        }

        /// <summary>
        /// FTP 풀 경로에서 Directory 경로만 가져옵니다.
        /// </summary>
        /// <param name="ftpFilePath">FTP 풀 경로</param>
        /// <returns>디렉터리 경로입니다.</returns>
        private string getDirectoryPath(string ftpFilePath) {
            string[] datas = ftpFilePath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string directoryPath = string.Empty;

            for (int i = 0; i < datas.Length - 1; i++) {
                directoryPath += string.Format("/{0}", datas[i]);
            }
            return directoryPath;
        }

        /// <summary>
        /// FTP 풀 경로에서 파일이름만 가져옵니다.
        /// </summary>
        /// <param name="ftpFilePath">FTP 풀 경로</param>
        /// <returns>파일명입니다.</returns>
        private string getFileName(string ftpFilePath) {
            string[] datas = ftpFilePath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            return datas[datas.Length - 1];
        }
    }
}