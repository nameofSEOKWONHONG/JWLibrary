using FluentValidation;
using JWLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.ServiceExecutor {
    public class ServiceExecutor<TOwner, TValidator, TRequest, TResult> : ServiceBase<TOwner>, IServiceExecutor<TRequest, TResult>
        where TOwner : ServiceExecutor<TOwner, TValidator, TRequest, TResult>
        where TValidator : AbstractValidator<TOwner>, new() {
        public TOwner Owner { get; set; }
        public TRequest Request { get; set; }
        public TResult Result { get; set; }

        public ServiceExecutor() {
            this.Owner = (TOwner)this;
        }

        public override void Execute() {

        }

        public override void PostExecute() {

        }

        public override bool PreExecute() {
            return true;
        }

        public override void Validate() {
            var validator = new ValidatorBase<TOwner, TValidator>();
            if (validator.TValidator.jIsNotNull()) {
                var result = validator.TValidator.Validate(Owner);
                if (!result.IsValid) {
                    throw new Exception(result.Errors.jFirst().ErrorMessage);
                }
            }
        }

        public override void Dispose() {

        }
    }


}
