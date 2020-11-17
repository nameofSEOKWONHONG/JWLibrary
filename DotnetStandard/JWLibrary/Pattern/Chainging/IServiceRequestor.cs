using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.Chainging {
    public interface IServiceRequestor<TRequest, TResult> : IServiceExecutor<TRequest, TResult> {
        IServiceFilter<TRequest, TResult> SetRequest(Func<TRequest, TRequest> func);
    }
}
