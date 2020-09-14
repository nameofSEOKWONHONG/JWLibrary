using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace JWLibrary.Web {

    /// <summary>
    /// ref : https://docs.microsoft.com/ko-kr/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
    /// </summary>
    public static class SwaggerConfig {

        /// <summary>
        /// configure services on use startup
        /// </summary>
        /// <param name="services"></param>
        public static void SwaggerConfigureServices(this IServiceCollection services) {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() {
                    Title = "JWLibrary.ApiCore",
                    Version = "v1",
                    Description = "rest api base"
                });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "JWLibrary.ApiCore.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        /// configure on use startup
        /// </summary>
        /// <param name="app"></param>
        public static void SwaggerConfigure(this IApplicationBuilder app) {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}