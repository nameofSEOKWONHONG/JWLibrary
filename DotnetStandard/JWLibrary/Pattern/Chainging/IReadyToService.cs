using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Pattern.Chainging {
    public interface IReadyToService<TIService, TRequest, TResult>
        where TIService : ServiceBase<TRequest, TResult>, IServiceBase<TRequest, TResult>
        where TRequest : class
        where TResult : class {
        ServiceManager<TIService, TRequest, TResult> Create();
    }
}
