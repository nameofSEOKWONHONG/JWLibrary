using JWLibrary.Pattern.TaskService;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Pattern.Pipeline {
    public class SvcPipeline<TIAction, TAction, TRequest, TResult> : IDisposable
        where TAction : class, new() {
        TAction action = new TAction();
        public SvcPipeline<TIAction, TAction, TRequest, TResult> CreatePipeline() {
            return this;
        }

        public SvcPipeline<TIAction, TAction, TRequest, TResult> Registry() {
            return this;
        }

        public SvcPipeline<TIAction, TAction, TRequest, TResult> Mapping(Func<TAction, bool> func) {
            func(action);
            return this;
        }

        public SvcPipeline<TIAction, TAction, TRequest, TResult> Filter() {
            return this;
        }

        public void OnExecuted() {

        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}
