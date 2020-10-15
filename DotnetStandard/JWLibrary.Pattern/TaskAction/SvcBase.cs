using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.TaskAction {
    public abstract class SvcBase<TRequest, TResult> : ISvcBase<TRequest, TResult>, IDisposable
        where TRequest : class {

        public SvcBase() {

        }

        public TRequest Request { get; set; }

        public TResult Result { get; set; }

        public ILogger Logger { get; set; }

        public abstract Task<bool> PreExecute();

        public abstract TResult Executed();

        public abstract Task<bool> PostExecute();

        public void Dispose() {
        }
    }
}