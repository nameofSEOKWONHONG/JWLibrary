using FluentValidation;
using JWLibrary.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.TaskAction {
    public class ValidateTaskService<TIAction, TAction, TRequest, TResult, TValidator> : TaskService<TIAction, TAction, TRequest, TResult>
        where TIAction : ISvcBase<TRequest, TResult>
        where TAction : SvcBase<TRequest, TResult>, new()
        where TRequest : class
        where TValidator : class, IValidator<TAction>, new() {
        protected TValidator _validator;

        public ValidateTaskService() {
        }

        public override Task<TResult> ExecuteCoreAsync() {
            var validateResult = _validator.Validate(this._instance);
            if (validateResult.IsValid.jIsFalse()) throw new Exception(validateResult.Errors[0].ErrorMessage);
            return base.ExecuteCoreAsync();
        }
    }
}
