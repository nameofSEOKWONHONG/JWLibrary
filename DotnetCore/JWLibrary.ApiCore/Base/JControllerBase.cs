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

        protected TaskAction<TIAction, TAction, TRequest, TResult> CreateAction<TIAction, TAction, TRequest, TResult>()
            where TIAction : IActionBase<TRequest, TResult>
            where TAction : ActionBase<TRequest, TResult>, new()
            where TRequest : class {
            return ActionFactory.CreateAction<TIAction,
                        TAction,
                        TRequest,
                        TResult>().SetLogger(this.Logger);
        }

        protected TaskAction<TIAction, TAction, TRequest, TResult> CreateAction<TIAction, TAction, TRequest, TResult, TValidator>()
            where TIAction : IActionBase<TRequest, TResult>
            where TAction : ActionBase<TRequest, TResult>, new()
            where TRequest : class
            where TValidator : class, IValidator<TAction>, new() {
            return ActionFactory.CreateAction<TIAction,
                        TAction,
                        TRequest,
                        TResult,
                        TValidator>().SetLogger(this.Logger);
        }
    }
}