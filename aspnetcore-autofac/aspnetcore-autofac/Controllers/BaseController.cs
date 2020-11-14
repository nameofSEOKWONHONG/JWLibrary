using aspnetcore_autofac.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_autofac.Controllers {
    [ApiController]
    [Route("[controller]/[action]")]
    public class BaseController<TController> : ControllerBase {
        private readonly ILogger<TController> logger;
        public BaseController(ILogger<TController> logger) {
            this.logger = logger;
        }

        public Executor<TRequest, TResult> CreateServiceExecutor<TRequest, TResult>(IBaseService<TRequest, TResult> service) {
            return new Executor<TRequest, TResult>(service);
        }
    }
}
