using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace JWLibrary.ApiCore {
    /// <summary>
    /// program
    /// </summary>
#pragma warning disable RCS1102 // Make class static.

    public class Program
#pragma warning restore RCS1102 // Make class static.
    {
        /// <summary>
        /// program main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// create host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}