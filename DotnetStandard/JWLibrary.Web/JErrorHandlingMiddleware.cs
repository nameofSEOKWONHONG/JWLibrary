using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JWLibrary.Web {

    public class JErrorHandlingMiddleware {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public JErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory) {
            _next = next;
            _logger = loggerFactory.CreateLogger<JErrorHandlingMiddleware>();
        }

        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            } finally {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null && contextFeature.Error != null) // ...
                    // Add lines to your log file, or your
                    // Application insights Instance here
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