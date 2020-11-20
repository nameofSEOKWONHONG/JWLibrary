using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.ServiceExecutor {

    public interface IServiceExecutor<TRequest, TResult> : IServiceBase {
        TRequest Request { get; set; }
        TResult Result { get; set; }
    }
}
