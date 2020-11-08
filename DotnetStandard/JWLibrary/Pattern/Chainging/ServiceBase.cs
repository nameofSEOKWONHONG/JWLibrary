using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Pattern.Chainging {
    public abstract class ServiceBase<TRequest, TResult> : IServiceBase<TRequest, TResult> {
        public virtual TResult OnExecute() {
            return default(TResult);
        }

        public virtual bool PostExecute() {
            return true;
        }

        public virtual bool PreExeute() {
            return true;
        }
    }
}
