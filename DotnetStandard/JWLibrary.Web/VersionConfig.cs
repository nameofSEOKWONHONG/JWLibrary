using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace JWLibrary.Web {

    public static class VersionConfig {

        public static void AddVersionConfig(this IServiceCollection services) {
            services.AddApiVersioning(x => {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
                x.ApiVersionReader = new HeaderApiVersionReader("api-version"); //header versioning setting
            });
        }
    }
}