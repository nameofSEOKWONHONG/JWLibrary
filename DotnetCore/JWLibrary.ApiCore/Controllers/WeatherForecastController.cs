using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWLibrary.ApiCore.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using JWLibrary.Core;
using JWLibrary.ApiCore.Config;
using JWLibrary.Pattern;
using JWLibrary.Pattern.TaskAction;
using JWLibrary.Database;
using JWLibrary.StaticMethod;

using JWActions;

namespace JWLibrary.ApiCore.Controllers {

    public class WeatherForecastController : JControllerBase {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<WEATHER_FORECAST> Get(string name, int age) {
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WEATHER_FORECAST {
        //        DATE = DateTime.Now.AddDays(index),
        //        TEMPERATURE_C = rng.Next(-20, 55),
        //        SUMMARY = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}


        [HttpGet]
        public async Task<WEATHER_FORECAST> Get(int idx = 1) {
            WEATHER_FORECAST result = null;
            using (var action = ActionFactory.CreateAction<IGetWeatherForecastAction, GetWeatherForecastAction, WeatherForecastRequestDto, WEATHER_FORECAST>()) {
                action.Request = new WeatherForecastRequestDto() {
                    ID = idx
                };
                result = await action.ExecuteCore();
            }
            return result;
        }

        [HttpGet]
        public async Task<IEnumerable<WEATHER_FORECAST>> GetAll() {
            IEnumerable<WEATHER_FORECAST> result = null;
            using (var action = ActionFactory.CreateAction<IGetAllWeatherForecastAction, GetAllWeatherForecastAction, WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>>()) {
                result = await action.ExecuteCore();
            }
            return result;
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
