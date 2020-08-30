using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWLibrary.ApiCore.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using JWLibrary.StaticMethod;
using JWLibrary.ApiCore.Config;

namespace JWLibrary.ApiCore.Controllers
{

    public class WeatherForecastController : JControllerBase {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// Post메서드
        /// </summary>
        /// <param name="request">요청:RequestDto<TestRequestDto></param>
        /// <returns></returns>
        [HttpPost]
        public string Post(
            [FromBody]
            [ModelBinder(typeof(JPostModelBinder<RequestDto<TestRequestDto>>))]
            RequestDto<TestRequestDto> request) {
            var req = HttpContext.Request;
            var items = base.HttpContext.Items;

            request.WRITER_ID = "test";
            request.WRITE_DT = DateTime.Now;
            return request.Serialize();
        }
    }
}
