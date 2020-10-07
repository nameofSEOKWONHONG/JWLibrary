using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Web {
    public class JErrorHandlingMiddleware {
        readonly RequestDelegate _next;
        readonly ILogger _logger;
        public JErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) {
            _next = next;
            _logger = loggerFactory.CreateLogger<JErrorHandlingMiddleware>();
        }
        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            }
            finally {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null && contextFeature.Error != null) {
                    // ...
                    // Add lines to your log file, or your
                    // Application insights instance here
                    // ...

                    _logger.LogError(contextFeature.Error, contextFeature.Error.Message);
                    //context.Response.StatusCode = (int)GetErrorCode(contextFeature.Error);
                    //context.Response.ContentType = "application/json";

                    //await context.Response.WriteAsync(JsonConvert.SerializeObject(new ProblemDetails() {
                    //    Status = context.Response.StatusCode,
                    //    Title = contextFeature.Error.Message
                    //}));
                }
            }
        }
    }
}
