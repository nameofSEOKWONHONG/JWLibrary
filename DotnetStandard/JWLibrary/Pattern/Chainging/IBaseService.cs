using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.Chainging {
    public interface IBaseService<TRequest, TResult> : IDisposable {
        TRequest Request { get; set; }
        TResult Result { get; set; }
        bool PreExecute();
        TResult Execute();
        void PostExecute();
    }
}
