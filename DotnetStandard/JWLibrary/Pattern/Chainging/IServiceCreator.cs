using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.Chainging {
    public interface IServiceCreator<TRequest, TResult> : IServiceExecutor<TRequest, TResult> {
        IServiceRequestor<TRequest, TResult> SetService(IBaseService<TRequest, TResult> service);
    }
}
