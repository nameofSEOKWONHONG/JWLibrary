using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Pattern.Chainging {
    public interface IAddRquest<TIService, TRequest, TResult>
        where TIService : class, IServiceBase<TRequest, TResult>
        where TRequest : class
        where TResult : class {
        IAddFilter<TIService, TRequest, TResult> SetRequest(TRequest request);
    }
}
