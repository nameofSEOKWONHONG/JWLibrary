using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.Chainging {
    public interface IServiceExecutor<TRequest, TResult> {
        TResult OnExecuted(Func<TResult, TResult> func);
        Task<TResult> OnExecutedAsync(Func<TResult, TResult> func);
    }
}
