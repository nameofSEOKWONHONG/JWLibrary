using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.Log
{
    public interface IJWLogger
    {
        void WriteLog(string strLog, LogCode logCode);        
    }
}
