using JCoreSvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Pattern {


    

    public interface IActionBase<TResult> {
        bool PreExecute();
        TResult Executed();
        bool PostExecute();

    }
    public abstract class ActionBase<TRequst, TResult> : IActionBase<TResult>, IDisposable
        where TRequst : class {

        public abstract bool PreExecute();
        public abstract TResult Executed();
        public abstract bool PostExecute();

        public void Dispose() {

        }
    }

    public class ActionFactory {
        public static TaskAction<TIAction, TAction, TRequest, TResult> CreateAction<TIAction, TAction, TRequest, TResult>()
            where TIAction : IActionBase<TResult>
            where TAction : ActionBase<TRequest, TResult>, new()
            where TRequest : class {
            return new TaskAction<TIAction, TAction, TRequest, TResult>();
        }
    }

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
