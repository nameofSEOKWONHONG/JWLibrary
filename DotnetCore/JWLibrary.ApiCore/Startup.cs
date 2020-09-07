namespace JWLibrary.ApiCore {

    using JWLibrary.ApiCore.Config;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup {

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.SwaggerConfigureServices();
            services.AddControllers()
                .AddNewtonsoftJson(options => {
                    options.SerializerSettings.ContractResolver = new LowercaseContractResolver();
                })
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            // ********************
            // USE CORS
            // ********************
            services.AddCors(options => {
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // ********************
            // USE CORS
            // ********************
            app.UseCors("CorsPolicy");

            // allow response caching directives in the API Controllers
            app.UseResponseCaching();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.SwaggerConfigure();
        }
    }

    public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver {

        protected override string ResolvePropertyName(string propertyName) {
            return propertyName.ToLower();
        }
    }
}