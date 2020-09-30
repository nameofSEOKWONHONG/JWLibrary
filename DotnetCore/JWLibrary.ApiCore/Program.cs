using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Debug;
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
                .ConfigureLogging((hostingContext, logging) => {
                    logging.ClearProviders();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}