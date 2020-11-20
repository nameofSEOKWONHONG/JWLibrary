using FluentValidation;
using JWLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.ServiceExecutor {
    public class ServiceExecutorManager<TIService> : IDisposable
        where TIService : IServiceBase {
        private TIService service;
        private JList<Func<TIService, bool>> filters = new JList<Func<TIService, bool>>();
        private bool disposed;

        public ServiceExecutorManager(TIService service) {
            this.service = service;
        }

        public ServiceExecutorManager<TIService> SetRequest(Action<TIService> action) {
            action(service);
            return this;
        }

        public ServiceExecutorManager<TIService> AddFilter(Func<TIService, bool> func) {
            filters.Add(func);
            return this;
        }

        public void OnExecuted(Action<TIService> action) {
            foreach(var filter in filters) {
                var pass = filter(this.service);
                if (pass.jIsFalse()) return;
            }

            this.service.Validate();
            var preExecuted = this.service.PreExecute();
            if(preExecuted) {
                this.service.Execute();
            }
            this.service.PostExecute();

            action(this.service);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) return;
            if (disposing) {
                this.service.Dispose();
            }

            disposed = true;
        }
    }
}
