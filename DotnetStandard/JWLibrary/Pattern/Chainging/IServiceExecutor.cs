using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.Chainging {
    public interface IServiceExecutor<TRequest, TResult> {
        void OnExecuted(Action<TResult> action);
        Task OnExecutedAsync(Action<TResult> action);
    }
}
