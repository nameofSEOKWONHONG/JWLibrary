using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JWLibrary.Pattern.Chainging {
    public interface IAddFilter<TIService, TRequest, TResult>
        where TIService : class, IServiceBase<TRequest, TResult>
        where TRequest : class
        where TResult : class {
        IAddFilter<TIService, TRequest, TResult> SetFilter(Expression<Func<TRequest, bool>> filter);
        TResult OnExecute();
    }
}
