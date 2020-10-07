using FluentValidation;
using Microsoft.Extensions.Logging;
using System;

namespace JWLibrary.Pattern.TaskAction {

    public abstract class ActionBase<TRequest, TResult> : IActionBase<TRequest, TResult>, IDisposable
        where TRequest : class {

        public ActionBase() {

        }

        public TRequest Request { get; set; }

        public TResult Result { get; set; }

        public ILogger Logger { get; set; }

        public abstract bool PreExecute();

        public abstract TResult Executed();

        public abstract bool PostExecute();

        public void Dispose() {
        }
    }
}