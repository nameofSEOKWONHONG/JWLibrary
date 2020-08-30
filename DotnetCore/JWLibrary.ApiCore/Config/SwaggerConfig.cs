using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Config {
    /// <summary>
    /// ref : https://docs.microsoft.com/ko-kr/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
    /// </summary>
    public class SwaggerConfig {
        public static void ConfigureServices(IServiceCollection services) {
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

        public static void Configure(IApplicationBuilder app) {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
