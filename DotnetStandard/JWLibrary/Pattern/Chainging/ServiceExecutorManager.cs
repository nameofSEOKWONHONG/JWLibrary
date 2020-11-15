using FluentValidation;
using JWLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.Pattern.Chainging {

    public class ServiceExecutorManager<TRequest, TResult> : IServiceCreator<TRequest, TResult>,
        IServiceRequestor<TRequest, TResult>,
        IServiceFilter<TRequest, TResult>,
        IServiceExecutor<TRequest, TResult>,
        IDisposable {
        protected IBaseService<TRequest, TResult> _service;
        protected List<Func<TRequest, bool>> filters = new List<Func<TRequest, bool>>();
        private bool disposed;

        public ServiceExecutorManager() { }

        public ServiceExecutorManager(IBaseService<TRequest, TResult> service) {
            this._service = service;
        }

        public IServiceRequestor<TRequest, TResult> SetService(IBaseService<TRequest, TResult> service) {
            if (this._service.jIsNotNull()) return this;

            this._service = service;
            return this;
        }

        public IServiceFilter<TRequest, TResult> SetRequest(Func<TRequest, TRequest> func) {
            if(this._service.jIsNotNull()) {
                this._service.Request = func(this._service.Request);
            }
            return this;
        }

        public IServiceFilter<TRequest, TResult> AddFilter(Func<TRequest, bool> func) {
            this.filters.Add(func);
            return this;
        }

        public IServiceExecutor<TRequest, TResult> Validate<TOwner, TValidator>()
            where TOwner : BaseService<TRequest, TResult>
            where TValidator : IValidator<TOwner>, new() {
            TValidator validator = new TValidator();
            var validateResult = validator.Validate(this._service.jCast<TOwner>());
            if (!validateResult.IsValid) {
                throw new Exception(validateResult.Errors[0].ErrorMessage);
            }
            return this;
        }

        public void OnExecuted(Action<TResult> action) {
            var result = Execute();
            action(result);
        }

        public async Task OnExecutedAsync(Action<TResult> action) {
            await Task.Factory.StartNew(() => {
                var result = Execute();
                action(result);
            });
        }

        private TResult Execute() {
            var result = default(TResult);
            var pass = true;
            filters.jForEach(filter => {
                var filterResult = filter(this._service.Request);
                if (filterResult.jIsFalse()) {
                    pass = false;
                    return false;
                }

                return true;
            });

            if (pass.jIsFalse()) return result;

            var isValid = this._service.PreExecute();
            if (isValid.jIsTrue()) {
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

    #region [original version - no use]
    [Obsolete("no more use", true)]
    public class ServiceExecutor<TRequest, TResult> : IDisposable {
        private readonly IBaseService<TRequest, TResult> _service;

        private bool disposed;

        private readonly List<Func<TRequest, bool>> filters;

        public ServiceExecutor(IBaseService<TRequest, TResult> servive) {
            this._service = servive;
            this.filters = new List<Func<TRequest, bool>>();
        }

        public ServiceExecutor<TRequest, TResult> SetReqeust(Func<TRequest, TRequest> func) {
            this._service.Request = func(this._service.Request);
            return this;
        }

        public ServiceExecutor<TRequest, TResult> AddFilter(Func<TRequest, bool> func) {
            this.filters.Add(func);
            return this;
        }

        public ServiceExecutor<TRequest, TResult> Validate<TOwner, TValidator>()
            where TOwner : BaseService<TRequest, TResult>
            where TValidator : IValidator<TOwner>, new() {
            TValidator validator = new TValidator();
            var validateResult = validator.Validate((TOwner)this._service);
            if (!validateResult.IsValid) {
                throw new Exception(validateResult.Errors[0].ErrorMessage);
            }
            return this;
        }

        public void OnExecuted(Action<TResult> action) {
            var result = Execute();
            action(result);
        }

        public async Task OnExecutedAsync(Action<TResult> action) {
            await Task.Factory.StartNew(() => {
                var result = Execute();
                action(result);
            });
        }

        private TResult Execute() {
            var result = default(TResult);

            foreach (var filter in filters) {
                var isPass = filter(this._service.Request);
                if (!isPass) {
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
    #endregion
}
