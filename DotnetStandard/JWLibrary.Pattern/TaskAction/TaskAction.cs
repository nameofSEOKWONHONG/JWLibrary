using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using JWLibrary.Core;
using Microsoft.Extensions.Logging;
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

        public TaskAction() {
            _instance = new TAction();
            _action = _instance;
        }

        public TaskAction<TIAction, TAction, TRequest, TResult> SetLogger(ILogger logger) {
            this._instance.Logger = logger;
            return this;
        }

        public TaskAction<TIAction, TAction, TRequest, TResult> SetRequest(TRequest request) {
            this._instance.Request = request;
            return this;
        }

        public virtual Task<TResult> ExecuteCoreAsync() {
            return Task.Run(() => {
                TResult result = default(TResult);
                this._instance.Logger?.LogInformation("init default result");

                this._instance.Logger?.LogInformation("run pre execute");
                if (_action.PreExecute()) {
                    this._instance.Logger?.LogInformation("run action executed");
                    result = _action.Executed();
                    _action.Result = result;

                    this._instance.Logger?.LogInformation($"return result : {_action.Result.jToSerialize()}");
                    _action.PostExecute();
                }
                else {
                    this._instance.Logger?.LogInformation("not passed pre execute");
                }
                return result;
            });
        }

        public void Dispose() {
        }
    }
}
