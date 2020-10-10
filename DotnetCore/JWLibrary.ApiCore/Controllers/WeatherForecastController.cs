using JWLibrary.ApiCore.Base;
using JWLibrary.Core;
using JWLibrary.Pattern.TaskAction;
using JWLibrary.Web;
using JWService.Data;
using JWService.WeatherForecast;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JWLibrary.ApiCore.Controllers {

    /// <summary>
    /// WeatherForecastController
    /// **no more use biner**
    /// ref : http://www.binaryintellect.net/articles/03f580c4-84ad-4d78-847f-43103b4e4691.aspx
    /// </summary>
    public class WeatherForecastController : JControllerBase<WeatherForecastController> {
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
            : base(logger) {
        }

        /// <summary>
        /// get
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "idx" })]
        [Transaction(System.Transactions.TransactionScopeOption.Suppress)]
        public async Task<WEATHER_FORECAST> Get(int idx = 1) {
            WEATHER_FORECAST result = null;
            result = await base.CreateAction<IGetWeatherForecastSvc,
                        GetWeatherForecastSvc,
                        WeatherForecastRequestDto,
                        WEATHER_FORECAST,
                        GetWeatherForecastSvc.Validator>().SetRequest(new WeatherForecastRequestDto() {
                            ID = idx
                        }).ExecuteCoreAsync();
            return result;
        }

        /// <summary>
        /// get all
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        [Transaction(System.Transactions.TransactionScopeOption.Suppress)]
        public async Task<IEnumerable<WEATHER_FORECAST>> GetAll() {
            IEnumerable<WEATHER_FORECAST> result = null;
            result = await base.CreateAction<IGetAllWeatherForecastSvc,
                                GetAllWeatherForecastSvc,
                                WeatherForecastRequestDto,
                                IEnumerable<WEATHER_FORECAST>>()
                                .SetRequest(new WeatherForecastRequestDto())
                                .ExecuteCoreAsync();
            return result;
        }

        /// <summary>
        /// Post메서드
        /// </summary>
        /// <param name="request">요청:RequestDto<TestRequestDto></param>
        /// <returns></returns>
        [HttpPost]
        [Transaction(System.Transactions.TransactionScopeOption.Required)]
        public async Task<int> Save(/*[FromBody][ModelBinder(typeof(JPostModelBinder<RequestDto<WEATHER_FORECAST>>))]*/
            RequestDto<WEATHER_FORECAST> request) {
            using (var action = ServiceFactory.CreateService<ISaveWeatherForecastSvc,
                                SaveWeatherForecastSvc,
                                WEATHER_FORECAST,
                                int,
                                SaveWeatherForecastSvc.Validator>()) {
                return await action.SetLogger(base.Logger)
                    .SetRequest(request.Dto)
                    .ExecuteCoreAsync();
            }
        }

        /// <summary>
        /// remove
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Transaction(System.Transactions.TransactionScopeOption.Required)]
        public async Task<bool> Remove(/*[FromBody][ModelBinder(typeof(JPostModelBinder<RequestDto<WeatherForecastRequestDto>>))]*/
            RequestDto<WeatherForecastRequestDto> request) {
            using (var action = ServiceFactory.CreateService<IDeleteWeatherForecastSvc,
                                DeleteWeatherForecastSvc,
                                WeatherForecastRequestDto,
                                bool>()) {
                return await action.SetLogger(base.Logger)
                    .SetRequest(request.Dto)
                    .ExecuteCoreAsync();
            }
        }
    }
}