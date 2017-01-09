using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.Log
{
    public class LoggerFactory
    {
        public IJWLogger CreateInstance(LogType type, string connectionString = null)
        {
            if(type == LogType.FileLog)
            {
                return new JWFileLogger();
            }
            else
            {
                return new JWDbLogger(connectionString);
            }
        }
    }

    public enum LogType
    {
        FileLog,
        DbLog
    }
}
