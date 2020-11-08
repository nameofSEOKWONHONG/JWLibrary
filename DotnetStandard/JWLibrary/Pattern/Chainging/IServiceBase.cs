using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Pattern.Chainging {
    public interface IServiceBase<TRequest, TResult> {
        bool PreExeute();
        TResult OnExecute();
        bool PostExecute();
    }
}
