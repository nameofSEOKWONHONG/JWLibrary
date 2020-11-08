using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using FluentValidation;
using JWLibrary.Core;

namespace JWLibrary.Pattern.Chainging {
    public class ServiceManager<TIService, TRequest, TResult> : IAddRquest<TIService, TRequest, TResult>,
        IAddFilter<TIService, TRequest, TResult>,
        IReadyToService<TIService, TRequest, TResult>
        where TIService : ServiceBase<TRequest, TResult>, IServiceBase<TRequest, TResult>
        where TRequest : class
        where TResult : class {
        private TIService _service;
        private TRequest _request;
        private List<Expression<Func<TRequest, bool>>> _filterExpressions = new List<Expression<Func<TRequest, bool>>>();

        private ServiceManager() {
        }


        public static IAddRquest<TIService, TRequest, TResult> AddIService(TIService service) {
            var serviceManager = new ServiceManager<TIService, TRequest, TResult>();
            serviceManager._service = service;
            return serviceManager;
        }

        public ServiceManager<TIService, TRequest, TResult> Create() {
            return this;
        }

        public IAddFilter<TIService, TRequest, TResult> SetFilter(Expression<Func<TRequest, bool>> filter) {
            _filterExpressions.Add(filter);
            return this;
        }

        public TResult OnExecute() {
            TResult result = default(TResult);
            foreach (var expression in this._filterExpressions) {
                var filter = expression.Compile();
                var vaild = filter(this._request);
                if (!vaild) {
                    Debug.WriteLine("not pass filter :" + expression.jToString());
                    return result;
                }
            }

            if(this._service.PreExeute()) {
                result = this._service.OnExecute();
                this._service.PostExecute();
            }

            return result;
        }

        public IAddFilter<TIService, TRequest, TResult> SetRequest(TRequest request) {
            this._request = request;
            return this;
        }
    }
}
