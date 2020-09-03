using JWActions;
using JWLibrary.ApiCore.Dto;
using JWLibrary.Pattern.TaskAction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Controllers {
    /// <summary>
    /// CRUD TEST CLASS
    /// </summary>
    public class WeatherForecastController : JControllerBase {
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
        public async Task<int> Save([FromBody][ModelBinder(typeof(JPostModelBinder<RequestDto<WEATHER_FORECAST>>))]
            RequestDto<WEATHER_FORECAST> request) {

            using(var action = ActionFactory.CreateAction<ISaveWeatherForecastAction, SaveWeatherForecastAction, WEATHER_FORECAST, int>()) {
                action.Request = request.Dto;
                return await action.ExecuteCore();
            }
        }

        [HttpPost]
        public async Task<bool> Remove([FromBody][ModelBinder(typeof(JPostModelBinder<RequestDto<WeatherForecastRequestDto>>))]
            RequestDto<WeatherForecastRequestDto> request) {

            using (var action = ActionFactory.CreateAction<IDeleteWeatherForecastAction, DeleteWeatherForecastAction, WeatherForecastRequestDto, bool>()) {
                action.Request = request.Dto;
                return await action.ExecuteCore();
            }
        }
    }
}
