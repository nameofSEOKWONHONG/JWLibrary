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
                    _action.Result = result;
                    _action.PostExecute();
                }
                return result;
            });
        }

        public void Dispose() {
        }
    }
}
