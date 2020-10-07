using FluentValidation;
using Microsoft.Extensions.Logging;

namespace JWLibrary.Pattern.TaskAction {

    public class ActionFactory {

        public static TaskAction<TIAction, TAction, TRequest, TResult> CreateAction<TIAction, TAction, TRequest, TResult>()
            where TIAction : IActionBase<TRequest, TResult>
            where TAction : ActionBase<TRequest, TResult>, new()
            where TRequest : class
            {
            return new TaskAction<TIAction, TAction, TRequest, TResult>();
        }

        public static ValidateTaskAction<TIAction, TAction, TRequest, TResult, TValidator> CreateAction<TIAction, TAction, TRequest, TResult, TValidator>()
            where TIAction : IActionBase<TRequest, TResult>
            where TAction : ActionBase<TRequest, TResult>, new()
            where TRequest : class
            where TValidator : class, IValidator<TAction>, new() {
            return new ValidateTaskAction<TIAction, TAction, TRequest, TResult, TValidator>();
        }
    }
}