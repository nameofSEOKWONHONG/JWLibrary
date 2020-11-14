using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_autofac.Services {
    //public interface IExecutor{
    //    void Execute();
    //}

    public class Executor<TRequest, TResult> : IDisposable {
        private readonly IBaseService<TRequest, TResult> _service;
        private TRequest request;
        private readonly List<Func<TRequest, bool>> filters;
        private bool disposed;
        private Action<TResult> executedAction;


        public Executor(IBaseService<TRequest, TResult> servive) {
            this._service = servive;
            this.filters = new List<Func<TRequest, bool>>();
        }

        public Executor<TRequest, TResult> SetReqeust(Func<TRequest, TRequest> func) {
            this._service.Request = func(this.request);
            return this;
        }

        public Executor<TRequest, TResult> AddFilter(Func<TRequest, bool> func) {
            this.filters.Add(func);
            return this;
        }

        public void OnExecuted(Action<TResult> action) {
            var result = Execute();
            action(result);
        }

        private TResult Execute() {
            var result = default(TResult);

            foreach (var filter in filters) {
                var isPass = filter(this.request);
                if(!isPass) {
                    return result;
                }
            }

            var isValid = this._service.PreExecute();
            if (isValid) {
                result = this._service.Execute();
            }
            this._service.PostExecute();

            return result;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) {
                _service.Dispose();
            }

            disposed = true;
        }
    }

    public interface IBaseService<TRequest, TResult> : IDisposable {
        TRequest Request { get; set; }
        TResult Result { get; set; }
        bool PreExecute();
        TResult Execute();
        void PostExecute();
    }

    public class BaseService<TRequest, TResult> : IBaseService<TRequest, TResult> {
        public TRequest Request { get; set; }
        public TResult Result { get; set; }

        public virtual TResult Execute() {
            return default(TResult);
        }

        public virtual void PostExecute() {
            return;
        }

        public virtual bool PreExecute() {
            return true;
        }
        public virtual void Dispose() {
            
        }
    }

    public interface IHelloService : IBaseService<string, string>{ 

    }

    public class HelloSerivce : BaseService<string, string>, IHelloService {
        public override string Execute() {
            return this.Request;
        }

        public override void Dispose() {
            //your code....

            base.Dispose();
        }
    }
}
