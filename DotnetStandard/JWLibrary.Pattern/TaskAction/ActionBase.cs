using FluentValidation;
using System;

namespace JWLibrary.Pattern.TaskAction {

    public abstract class ActionBase<TRequest, TResult> : IActionBase<TRequest, TResult>, IDisposable
        where TRequest : class {

        public ActionBase() {

        }

        public TRequest Request { get; set; }

        public abstract bool PreExecute();

        public abstract TResult Executed();

        public abstract bool PostExecute();

        public void Dispose() {
        }
    }
}