using FluentValidation;
using Microsoft.Extensions.Logging;

namespace JWLibrary.Pattern.TaskAction {

    public class ServiceFactory {

        public static TaskService<TIAction, TAction, TRequest, TResult> CreateService<TIAction, TAction, TRequest, TResult>()
            where TIAction : ISvcBase<TRequest, TResult>
            where TAction : SvcBase<TRequest, TResult>, new()
            where TRequest : class
            {
            return new TaskService<TIAction, TAction, TRequest, TResult>();
        }

        public static ValidateTaskService<TIAction, TAction, TRequest, TResult, TValidator> CreateService<TIAction, TAction, TRequest, TResult, TValidator>()
            where TIAction : ISvcBase<TRequest, TResult>
            where TAction : SvcBase<TRequest, TResult>, new()
            where TRequest : class
            where TValidator : class, IValidator<TAction>, new() {
            return new ValidateTaskService<TIAction, TAction, TRequest, TResult, TValidator>();
        }
    }
}