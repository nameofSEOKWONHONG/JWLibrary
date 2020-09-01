using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Pattern.TaskAction {
    public abstract class ActionBase<TRequst, TResult> : IActionBase<TResult>, IDisposable
        where TRequst : class {

        public abstract bool PreExecute();
        public abstract TResult Executed();
        public abstract bool PostExecute();

        public void Dispose() {

        }
    }
}
