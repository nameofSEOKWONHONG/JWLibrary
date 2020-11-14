using aspnetcore_autofac.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_autofac.Controllers {
    public class WeatherForecastController : BaseController<WeatherForecastController> {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHelloService _service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IHelloService service) : base(logger) {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get() {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public string Hello(string message) {
            var result = string.Empty;
            using var executor = this.CreateServiceExecutor<string, string>(_service);
            executor.SetReqeust(o => o = message)
                    .AddFilter(o => true)
                    .OnExecuted(o => {
                        if (!string.IsNullOrEmpty(o)) {
                            result = o;
                        }
                    });

            return result;
        }
    }
}
