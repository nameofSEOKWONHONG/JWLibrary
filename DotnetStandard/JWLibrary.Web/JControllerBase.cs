using JWLibrary.Pattern.ServiceExecutor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Web {
    /// <summary>
    /// base controller
    /// </summary>
    [ApiController]
    //[Route("api/[controller]/[action]")] //normal route
    [Route("api/{v:apiVersion}/[controller]/[action]")] //url version route
    public class JControllerBase<TController> : ControllerBase, IDisposable
        where TController : class {
        protected ILogger<TController> Logger;

        public JControllerBase(ILogger<TController> logger) {
            Logger = logger;
        }

        public ServiceExecutorManager<TRequest, TResult> CreateServiceExecutor<TRequest, TResult>(IServiceBaseExecutor<TRequest, TResult> service) {
            return new ServiceExecutorManager<TRequest, TResult>(service);
        }

        public void Dispose() {

        }
    }
}
