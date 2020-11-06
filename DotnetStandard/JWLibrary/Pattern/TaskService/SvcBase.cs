using JWLibrary.Core;
using Microsoft.Extensions.Logging;
using System;

namespace JWLibrary.Pattern.TaskService {

    public abstract class SvcBase<TRequest, TResult> : ISvcBase<TRequest, TResult>, IDisposable
        where TRequest : class {
        public ILogger Logger { get; set; }

        public void Dispose() {
        }

        public TRequest Request { get; set; }

        public TResult Result { get; set; }

        public JList<Func<TRequest, bool>> Filters { get; set; } = new JList<Func<TRequest, bool>>();

        public abstract bool PreExecute();

        public abstract TResult Executed();

        public abstract bool PostExecute();
    }
}