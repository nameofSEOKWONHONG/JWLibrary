using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Pattern.Chainging {
    public class ValidateServiceBase<TRequest, TResult, TValidator> : ServiceBase<TRequest, TResult>
        where TValidator : class, IValidator, new() {
        protected TValidator ServiceValidator = null;
        public ValidateServiceBase() {
            ServiceValidator = new TValidator();
        }
    }
}
