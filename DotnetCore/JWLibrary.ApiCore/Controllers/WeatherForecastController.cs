using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using JWLibrary.ApiCore.Config;
using JWLibrary.Core;
using JWLibrary.Pattern.TaskService;
using JWLibrary.Web;
using JWService.Data;
using JWService.WeatherForecast;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWLibrary.ApiCore.Controllers
{
    /// <summary>
    ///     WeatherForecastController
    ///     **no more use biner**
    ///     ref : http://www.binaryintellect.net/articles/03f580c4-84ad-4d78-847f-43103b4e4691.aspx
    /// </summary>
    [ApiVersion("0.0")]
    public class WeatherForecastController : JControllerBase<WeatherForecastController>
    {
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
            : base(logger)
        {
        }

        /// <summary>
        ///     get
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] {"idx"})]
        [Transaction(TransactionScopeOption.Suppress)]
        public async Task<WEATHER_FORECAST> GetWeather(int idx = 1)
        {
            WEATHER_FORECAST result = null;
            result = await CreateAction<IGetWeatherForecastSvc,
                    GetWeatherForecastSvc,
                    WeatherForecastRequestDto,
                    WEATHER_FORECAST,
                    GetWeatherForecastSvc.Validator>()
                .SetRequest(new WeatherForecastRequestDto
                {
                    ID = idx
                }).ExecuteAsync();
            return result;
        }

        /// <summary>
        ///     get all
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        [Transaction(TransactionScopeOption.Suppress)]
        public async Task<IEnumerable<WEATHER_FORECAST>> GetWeathers()
        {
            IEnumerable<WEATHER_FORECAST> result = null;
            result = await CreateAction<IGetAllWeatherForecastSvc,
                    GetAllWeatherForecastSvc,
                    WeatherForecastRequestDto,
                    IEnumerable<WEATHER_FORECAST>>()
                .SetRequest(new WeatherForecastRequestDto())
                .ExecuteAsync();
            return result;
        }

        /// <summary>
        ///     Post메서드
        /// </summary>
        /// <param name="request">요청:RequestDto<TestRequestDto></param>
        /// <returns></returns>
        [HttpPost]
        [Transaction(TransactionScopeOption.Required)]
        public async Task<int>
            SaveWeather( /*[FromBody][ModelBinder(typeof(JPostModelBinder<RequestDto<WEATHER_FORECAST>>))]*/
                RequestDto<WEATHER_FORECAST> request)
        {
            using (var action = ServiceFactory.CreateService<ISaveWeatherForecastSvc,
                SaveWeatherForecastSvc,
                WEATHER_FORECAST,
                int,
                SaveWeatherForecastSvc.Validator>())
            {
                return await action.SetLogger(Logger)
                    .SetRequest(request.RequestDto)
                    .ExecuteAsync();
            }
        }

        /// <summary>
        ///     remove
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Transaction(TransactionScopeOption.Required)]
        public async Task<bool>
            RemoveWeather( /*[FromBody][ModelBinder(typeof(JPostModelBinder<RequestDto<WeatherForecastRequestDto>>))]*/
                RequestDto<WeatherForecastRequestDto> request)
        {
            using (var action = ServiceFactory.CreateService<IDeleteWeatherForecastSvc,
                DeleteWeatherForecastSvc,
                WeatherForecastRequestDto,
                bool>())
            {
                return await action.SetLogger(Logger)
                    .SetRequest(request.RequestDto)
                    .ExecuteAsync();
            }
        }
    }
}