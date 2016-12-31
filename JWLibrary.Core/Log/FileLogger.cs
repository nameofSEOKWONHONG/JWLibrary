using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.Log
{
    public class FileLogger
    {
        private static string strLogDir = ExecuteLocation.PathInfo.GetExecutablePath();
        private static string strLogFile = "Log";
        private static object asyncObject = new object();
        private const string DateTimeFormet = "yyyyMMdd";

        public FileLogger()
        {

        }

        public static void SetLogPath(string strDir)
        {
            strLogDir = strDir;
        }

        public static void SetLogFile(string strFile)
        {
            strLogFile = strFile;
        }

        public static string GetLogPath()
        {
            return strLogDir;
        }

        public static void WriteLog(string strLog)
        {
            WriteLog(strLog, LogCode.Infomation, strLogFile);
        }

        public static void WriteLog(string strLog, string strFileName)
        {
            WriteLog(strLog, LogCode.Infomation, strFileName, strLogDir);
        }

        public static void WriteLog(string strLog, string strFileName, string strPath)
        {
            WriteLog(strLog, LogCode.Infomation, strFileName, strPath);
        }

        public static void WriteLog(string strLog, LogCode logCode)
        {
            WriteLog(strLog, logCode, strLogFile);
        }

        public static void WriteLog(string strLog, LogCode logCode, string strFileName)
        {
            WriteLog(strLog, logCode, strFileName, strLogDir);
        }

        public static void WriteLog(string strLog, LogCode logCode, string strFileName, string strPath)
        {
            string strFullName;

            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            if (strPath.EndsWith(@"\") == false || strPath.EndsWith("/") == false)
            {
                strPath = strPath + @"\";
            }

            strFullName = strPath + strFileName + "_" + DateTime.Now.ToString(DateTimeFormet) + ".txt";

            string strFullLog = DateTime.Now.ToString("HH:mm:ss") + " (" + logCode.ToString() + ")" + " : " + strLog;

            lock (asyncObject)
            {
                using (StreamWriter sw = new StreamWriter(strFullName, true, System.Text.Encoding.UTF8, 4096))
                {
                    sw.WriteLine(strFullLog);
                    sw.Close();
                }
            }
        }

        public static void WriteLog(string strLog, bool timeDisplay)
        {
            string strFullName;

            if (!Directory.Exists(strLogDir))
            {
                Directory.CreateDirectory(strLogDir);
            }

            if (strLogDir.EndsWith(@"\") == false || strLogDir.EndsWith("/") == false)
            {
                strLogDir = strLogDir + @"\";
            }
            strFullName = strLogDir + strLogFile + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            string strFullLog = string.Empty;

            if (timeDisplay)
                strFullLog = DateTime.Now.ToString("HH:mm:ss") + " " + strLog;
            else
                strFullLog = strLog;

            lock (asyncObject)
            {
                using (StreamWriter sw = new StreamWriter(strFullName, true, System.Text.Encoding.UTF8, 4096))
                {
                    sw.WriteLine(strFullLog);
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        public static void WriteLogForOnlyText(string strLog, string strLogFileName)
        {
            lock (asyncObject)
            {
                using (StreamWriter sw = new StreamWriter(strLogFileName, true, System.Text.Encoding.UTF8, 4096))
                {
                    sw.WriteLine(strLog);
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        public static void AppendPathSeparator(ref string dir)
        {
            if (!dir.EndsWith("\\") && !dir.EndsWith("/"))
                dir += "\\";
        }

        public enum LogCode
        {
            Infomation = 0,
            Success = 1,
            Error = -1,
            Failure = -2,
            Warning = -10,
            SystemError = -101,
            ApplicationError = -201
        }
    }
}
