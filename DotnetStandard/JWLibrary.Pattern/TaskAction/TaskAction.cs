using JCoreSvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.TaskAction {
    public class TaskAction<TIAction, TAction, TRequest, TResult> : IDisposable
        where TIAction : IActionBase<TResult>
        where TAction : ActionBase<TRequest, TResult>, new()
        where TRequest : class {
        private IActionBase<TResult> _action;
        public IActionBase<TResult> Action { get { return _action; } }
        public TRequest Request { get; set; }

        public TaskAction() {
            _action = new TAction();
        }

        public Task<TResult> ExecuteCore() {
            return Task.Run(() => {
                TResult result = default(TResult);
                if (_action.PostExecute()) {
                    result = _action.Executed();
                    _action.PostExecute();
                }
                return result;
            });
        }

        public void Dispose() {

        }
    }
}
