using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.Log
{
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
