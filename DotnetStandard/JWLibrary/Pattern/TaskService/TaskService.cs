using JWLibrary.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.TaskService {

    public class TaskService<TIAction, TRequest, TResult> : IDisposable
        where TIAction : ISvcBase<TRequest, TResult>
        where TRequest : class
    {
        private TIAction _action = default(TIAction);

        public TaskService()
        {
        }

        public void Dispose()
        {

        }
    }

    public class TaskService<TIAction, TAction, TRequest, TResult> : IDisposable
        where TIAction : ISvcBase<TRequest, TResult>
        where TAction : SvcBase<TRequest, TResult>, new()
        where TRequest : class {
        private readonly ISvcBase<TRequest, TResult> _action;
        protected readonly TAction Instance;

        public TaskService() {
            Instance = new TAction();
            _action = Instance;
        }

        public void Dispose() {
        }

        public TaskService<TIAction, TAction, TRequest, TResult> SetLogger(ILogger logger) {
            Instance.Logger = logger;
            return this;
        }

        public TaskService<TIAction, TAction, TRequest, TResult> SetRequest(TRequest request) {
            Instance.Request = request;
            return this;
        }

        public TaskService<TIAction, TAction, TRequest, TResult> SetFilter(Func<TRequest, bool> func) {
            Instance.Filters.Add(func);
            return this;
        }

        public virtual Task<TResult> ExecuteAsync() {
            TResult result = default;
            if (FilterValidate())
                return Task.Run(() => {
                    Instance.Logger?.LogInformation("init default result");

                    Instance.Logger?.LogInformation("run pre execute");
                    if (_action.PreExecute()) {
                        Instance.Logger?.LogInformation("run action executed");
                        result = _action.Executed();
                        _action.Result = result;

                        Instance.Logger?.LogInformation($"return result : {_action.Result.jToString()}");
                        _action.PostExecute();
                    } else {
                        Instance.Logger?.LogInformation("not passed pre execute");
                    }

                    return result;
                });

            return Task.FromResult(result);
        }

        private bool FilterValidate() {
            var valid = true;
            Instance.Filters.jForEach(item => {
                valid = item(Instance.Request);
                if (valid.jIsTrue()) return true;
                return false;
            });

            return valid;
        }
    }
}