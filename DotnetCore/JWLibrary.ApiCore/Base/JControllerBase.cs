using System;
using FluentValidation;
using JWLibrary.Pattern.TaskService;
using LiteDbFlex;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWLibrary.ApiCore.Base
{
    /// <summary>
    ///     base controller
    /// </summary>
    [ApiController]
    //[Route("api/[controller]/[action]")] //normal route
    [Route("api/{v:apiVersion}/[controller]/[action]")] //url version route
    public class JControllerBase<TController> : ControllerBase, IDisposable
        where TController : class
    {
        protected ILogger<TController> Logger;

        public JControllerBase(ILogger<TController> logger)
        {
            Logger = logger;
        }

        public void Dispose()
        {
            LiteDbFlexerManager.Instance.Value.Dispose();
        }

        protected TaskService<TIAction, TAction, TRequest, TResult> CreateAction<TIAction, TAction, TRequest, TResult>()
            where TIAction : ISvcBase<TRequest, TResult>
            where TAction : SvcBase<TRequest, TResult>, new()
            where TRequest : class
        {
            return ServiceFactory.CreateService<TIAction,
                TAction,
                TRequest,
                TResult>().SetLogger(Logger);
        }

        protected TaskService<TIAction, TAction, TRequest, TResult> CreateAction<TIAction, TAction, TRequest, TResult,
            TValidator>()
            where TIAction : ISvcBase<TRequest, TResult>
            where TAction : SvcBase<TRequest, TResult>, new()
            where TRequest : class
            where TValidator : class, IValidator<TAction>, new()
        {
            return ServiceFactory.CreateService<TIAction,
                TAction,
                TRequest,
                TResult,
                TValidator>().SetLogger(Logger);
        }

        protected void SetCache<TEntity>(CacheInfo<TEntity> cache, string privateFileName = "")
            where TEntity : class
        {
            LiteDbCacheFlexer.Instance.Value.SetAdditionalDbFileName(privateFileName).SetCache(() => cache);
        }

        protected CacheInfo<TEntity> GetCache<TEntity>(string cacheName, string privateFileName = "")
            where TEntity : class
        {
            var cache = LiteDbCacheFlexer.Instance.Value.SetAdditionalDbFileName(privateFileName)
                .GetCache<TEntity>(cacheName);
            return cache;
        }
    }
}