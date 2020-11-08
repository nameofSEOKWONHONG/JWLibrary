using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using JWLibrary.ApiCore.Config;
using JWLibrary.ApiCore.Util;
using JWLibrary.Web;
using JWService.Accounts;
using JWService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace JWLibrary.ApiCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region [swagger parameter settings]

            var swaggerSetting = new SwaggerConfigSetting
            {
                SettingDoc = new SwaggerConfigSettingDoc
                {
                    Version = "v1",
                    OpenApiInfo = new OpenApiInfo
                    {
                        Title = "JWLibrary.ApiCore",
                        Version = "v1",
                        Description = "rest api base"
                    }
                },
                SettingSecurityDef = new SwaggerConfigSettingSecurityDef
                {
                    Name = "Bearer",
                    OpenApiSecurityScheme = new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
                    }
                },
                SettingSecurityReq = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                },
                XmlName = Path.Combine(AppContext.BaseDirectory, "JWLibrary.ApiCore.xml")
            };

            #endregion [swagger parameter settings]

            services.SwaggerConfigureServices(swaggerSetting);
            services.AddControllers()
                //.AddNewtonsoftJson(options => {
                //    options.SerializerSettings.ContractResolver = new LowercaseContractResolver();
                //})
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddVersionConfig();
            services.AddScoped<IGetAccountSvc, GetAccountSvc>();

            // ********************
            // USE CORS
            // ref : https://stackoverflow.com/questions/53675850/how-to-fix-the-cors-protocol-does-not-allow-specifying-a-wildcard-any-origin
            // ********************
            services.AddCors(options =>
            {
                //options.AddPolicy("AllowAll",
                //    builder => {
                //        builder
                //        .AllowAnyOrigin()
                //        .AllowAnyMethod()
                //        .AllowAnyHeader()
                //        .AllowCredentials();
                //    });
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // add service for allowing caching of responses
            // ref : https://github.com/Cingulara/dotnet-core-web-api-caching-examples
            services.AddResponseCaching();
            //.AddNewtonsoftJson(o => o.SerializerSettings.Converters.Insert(0, new CustomConverter()));
        }

        public void ConfigureContainer(ContainerBuilder builder) {
            builder.RegisterModule(new ServiceModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<JErrorHandlingMiddleware>();

            //set autofac container
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            // ********************
            // USE CORS
            // ********************
            app.UseCors("CorsPolicy");

            // allow response caching directives in the API Controllers
            app.UseResponseCaching();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //name: "default", pattern: "{controller=WeatherForecast}/{action=GetWeathers}/{id?}");
                endpoints.MapControllers();
            });

            app.SwaggerConfigure("../swagger/v1/swagger.json", "jwlibrary.apicore v1");
        }
    }

    //public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver {
    //    protected override string ResolvePropertyName(string propertyName) {
    //        return propertyName.ToLower();
    //    }
    //}
}