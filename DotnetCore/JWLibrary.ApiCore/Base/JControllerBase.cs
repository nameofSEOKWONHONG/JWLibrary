using FluentValidation;
using JWLibrary.Pattern.TaskService;
using LiteDbFlex;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JWLibrary.ApiCore.Base {

    /// <summary>
    /// base controller
    /// </summary>
    [ApiController]
    //[Route("api/[controller]/[action]")] //normal route
    [Route("api/{v:apiVersion}/[controller]/[action]")] //url version route
    public class JControllerBase<TController> : ControllerBase, IDisposable
        where TController : class {
        protected ILogger<TController> Logger = null;

        public JControllerBase(ILogger<TController> logger) {
            this.Logger = logger;
        }

        protected TaskService<TIAction, TAction, TRequest, TResult> CreateAction<TIAction, TAction, TRequest, TResult>()
            where TIAction : ISvcBase<TRequest, TResult>
            where TAction : SvcBase<TRequest, TResult>, new()
            where TRequest : class {
            return ServiceFactory.CreateService<TIAction,
                        TAction,
                        TRequest,
                        TResult>().SetLogger(this.Logger);
        }

        protected TaskService<TIAction, TAction, TRequest, TResult> CreateAction<TIAction, TAction, TRequest, TResult, TValidator>()
            where TIAction : ISvcBase<TRequest, TResult>
            where TAction : SvcBase<TRequest, TResult>, new()
            where TRequest : class
            where TValidator : class, IValidator<TAction>, new() {
            return ServiceFactory.CreateService<TIAction,
                        TAction,
                        TRequest,
                        TResult,
                        TValidator>().SetLogger(this.Logger);
        }

        protected void SetCache<TEntity>(CacheInfo<TEntity> cache, string privateFileName = "")
            where TEntity : class {
            LiteDbCacheFlexer<TEntity>.Instance.Value.SetAdditionalDbFileName(privateFileName).Execute(cache.CacheName, o => {
                o.Insert(cache);
                return cache;
            });
        }

        protected CacheInfo<TEntity> GetCache<TEntity>(string cacheName, string privateFileName = "")
            where TEntity : class {
            var cache = LiteDbCacheFlexer<TEntity>.Instance.Value.SetAdditionalDbFileName(privateFileName).Execute(cacheName, o => {
                return o.Get(x => x.CacheName == cacheName).GetResult<CacheInfo<TEntity>>();
            });

            return cache;
        }

        public void Dispose() {
            LiteDbFlexerManager.Instance.Value.Dispose();
        }
    }
}