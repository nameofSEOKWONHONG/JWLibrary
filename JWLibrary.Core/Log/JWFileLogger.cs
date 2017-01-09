using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.Log
{
    public class JWFileLogger : IJWLogger
    {
        private static string strLogDir = ExecuteLocation.PathInfo.GetExecutablePath();
        private static string strLogFile = "Log";
        private static object asyncObject = new object();
        private const string DateTimeFormet = "yyyyMMdd";

        public void WriteLog(string strLog, LogCode logCode)
        {
            WriteLog(strLog, logCode, strLogFile);
        }

        public void WriteLog(string strLog, LogCode logCode, string strFileName)
        {
            WriteLog(strLog, logCode, strFileName, strLogDir);
        }

        public void WriteLog(string strLog, LogCode logCode, string strFileName, string strPath)
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
    }

}
