using FluentValidation;
using JWLibrary.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.TaskService {
    public class ValidateTaskService<TIAction, TAction, TRequest, TResult, TValidator> : TaskService<TIAction, TAction, TRequest, TResult>
        where TIAction : ISvcBase<TRequest, TResult>
        where TAction : SvcBase<TRequest, TResult>, new()
        where TRequest : class
        where TValidator : class, IValidator<TAction>, new() {
        protected TValidator _validator;

        public ValidateTaskService() {
            _validator = new TValidator();
        }

        public override Task<TResult> ExecuteAsync() {
            var validateResult = _validator.Validate(base._instance);
            if (validateResult.IsValid.jIsFalse()) throw new Exception(validateResult.Errors[0].ErrorMessage);
            return base.ExecuteAsync();
        }
    }
}
