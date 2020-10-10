using FluentValidation;
using JWLibrary.Pattern.TaskAction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace JWLibrary.ApiCore.Base {

    /// <summary>
    /// base controller
    /// </summary>
    [ApiController]
    [Route("japi/[controller]/[action]")]
    public class JControllerBase<TController> : ControllerBase
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
    }
}