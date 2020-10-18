using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO;

namespace JWLibrary.Web {
    public class SwaggerConfigSetting {
        public SwaggerConfigSettingDoc SettingDoc { get; set; }
        public SwaggerConfigSettingSecurityDef SettingSecurityDef { get; set; }
        public OpenApiSecurityRequirement SettingSecurityReq { get; set; }
        public string XmlName { get; set; }
    }

    public class SwaggerConfigSettingDoc {
        public string Version { get; set; }
        public Microsoft.OpenApi.Models.OpenApiInfo OpenApiInfo { get; set; }
    }

    public class SwaggerConfigSettingSecurityDef {
        public string Name { get; set; }
        public OpenApiSecurityScheme OpenApiSecurityScheme { get; set; }
    }

    /// <summary>
    /// ref : https://docs.microsoft.com/ko-kr/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio
    /// </summary>
    public static class SwaggerConfig {
        /// <summary>
        /// configure services on use startup
        /// </summary>
        /// <param name="services"></param>
        public static void SwaggerConfigureServices(this IServiceCollection services,
            SwaggerConfigSetting swaggerConfigSetting) {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                c.SwaggerDoc(swaggerConfigSetting.SettingDoc.Version, swaggerConfigSetting.SettingDoc.OpenApiInfo);
                c.AddSecurityDefinition(swaggerConfigSetting.SettingSecurityDef.Name, swaggerConfigSetting.SettingSecurityDef.OpenApiSecurityScheme);
                c.AddSecurityRequirement(swaggerConfigSetting.SettingSecurityReq);
                c.IncludeXmlComments(swaggerConfigSetting.XmlName);
            });
        }

        /// <summary>
        /// configure on use startup
        /// </summary>
        /// <param name="app"></param>
        public static void SwaggerConfigure(this IApplicationBuilder app, string url, string apiName) {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint(url, apiName);
                c.RoutePrefix = string.Empty;
            });
        }
    }
}