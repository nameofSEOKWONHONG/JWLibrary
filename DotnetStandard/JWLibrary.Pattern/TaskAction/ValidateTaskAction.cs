using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using JWLibrary.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.TaskAction {

    public class TaskAction<TIAction, TAction, TRequest, TResult> : IDisposable
         where TIAction : IActionBase<TRequest, TResult>
         where TAction : ActionBase<TRequest, TResult>, new()
         where TRequest : class {
        protected IActionBase<TRequest, TResult> _action;
        protected TAction _instance;
        public IActionBase<TRequest, TResult> Action { get { return _action; } }

        public TRequest Request {
            set {
                _action.Request = value;
            }
        }

        public TaskAction() {
            _instance = new TAction();
            _action = _instance;
        }



        public virtual Task<TResult> ExecuteCore() {
            return Task.Run(() => {
                TResult result = default(TResult);
                if (_action.PreExecute()) {
                    result = _action.Executed();
                    _action.PostExecute();
                }
                return result;
            });
        }

        public void Dispose() {
        }
    }

    public class ValidateTaskAction<TIAction, TAction, TRequest, TResult, TValidator> : TaskAction<TIAction, TAction, TRequest, TResult>
        where TIAction : IActionBase<TRequest, TResult>
        where TAction : ActionBase<TRequest, TResult>, new()
        where TRequest : class
        where TValidator : class, IValidator<TAction>, new() {
        protected TValidator _validator;

        private readonly static Lazy<HashSet<TValidator>> _validations = new Lazy<HashSet<TValidator>>(() => {
            return new HashSet<TValidator>();
        });

        public ValidateTaskAction() {
            _validator = _validations.Value.jFirst(m => m.GetType().jEquals(typeof(TValidator)));
            _validator.jIfNull(_ => {
                _validator = new TValidator();
                _validations.Value.Add(_validator);
            });
        }

        public override Task<TResult> ExecuteCore() {
            var validateResult = _validator.Validate(this._instance);
            if (validateResult.IsValid.jIsFalse()) throw new Exception(validateResult.Errors[0].ErrorMessage);
            return base.ExecuteCore();
        }
    }
}
