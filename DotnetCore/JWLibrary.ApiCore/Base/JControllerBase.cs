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
            LiteDbFlex.LiteDbFlexerManager.Instance.Value.Create<CacheInfo<TEntity>>(privateFileName)
                .Insert(cache);
        }

        protected CacheInfo<TEntity> GetCache<TEntity>(Expression<Func<CacheInfo<TEntity>, bool>> expression, string privateFileName = "")
            where TEntity : class {
            var cache = LiteDbFlex.LiteDbFlexerManager.Instance.Value.Create<CacheInfo<TEntity>>(privateFileName)
                .Get(expression)
                .GetResult<CacheInfo<TEntity>>();
            if (cache == null) return null;
            if((DateTime.Now - DateTime.Parse(cache.SetDateTime)).TotalSeconds > cache.Interval) {
                LiteDbFlex.LiteDbFlexerManager.Instance.Value.Create<CacheInfo<TEntity>>(privateFileName)
                    .Delete(cache.Id);
                return null;
            }

            return cache;
        }

        public void Dispose() {
            LiteDbFlexerManager.Instance.Value.Dispose();
        }

        [LiteDbTable("cache.db", "caches")]
        [LiteDbIndex(new[] { "CacheName" })]
        public class CacheInfo<TEntity> {
            public int Id { get; set; }
            public string CacheName { get; set; }
            public string SetDateTime { get; set; }
            public int Interval { get; set; }
            public TEntity Data { get; set; }
        }
    }
}