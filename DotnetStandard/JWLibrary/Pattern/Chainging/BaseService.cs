using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.Chainging {
    public class BaseService<TRequest, TResult> : IBaseService<TRequest, TResult> {
        public TRequest Request { get; set; }
        public TResult Result { get; set; }

        public virtual TResult Execute() {
            return default(TResult);
        }

        public virtual void PostExecute() {
            return;
        }

        public virtual bool PreExecute() {
            return true;
        }
        public virtual void Dispose() {

        }

        public class BaseServiceValidator : AbstractValidator<BaseService<TResult, TResult>> {
            public BaseServiceValidator() {

            }
        }
    }
}
