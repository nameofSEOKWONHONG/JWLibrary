//namespace JWService.WeatherForecast {

//    using Dapper;
//    using JWService;
//    using JWLibrary.Core;
//    using JWLibrary.Database;
//    using JWLibrary.Pattern.TaskService;
//    using System.Data.SqlClient;
//    using JWService.Data;
//    using System.Threading.Tasks;
//    using JWLibrary.Pattern.Chainging;

//    public class DeleteWeatherForecastSvc : BaseService<WeatherForecastRequestDto, bool>, IDeleteWeatherForecastSvc {
//        IGetWeatherForecastSvc _getWeatherForecastSvc;
//        private WEATHER_FORECAST _removeObj = null;

//        public DeleteWeatherForecastSvc(IGetWeatherForecastSvc getWeatherForecastSvc) {
//            this._getWeatherForecastSvc = getWeatherForecastSvc;
//        }

//        public override bool PreExecute() {
//            using var executor = new ServiceExecutorManager<WeatherForecastRequestDto, WEATHER_FORECAST>(this._getWeatherForecastSvc);
//            this._removeObj = executor.SetRequest(o => {
//                    return new WeatherForecastRequestDto() {
//                        ID = this.Request.ID
//                    };
//                })
//                .OnExecuted(o => o);

//            if (this._removeObj.jIsNotNull()) return true;
//            return false;
//        }

//        public override bool Execute() {
//            var result = JDataBase.Resolve<SqlConnection>().jQuery<bool>(db => {
//                return db.Delete<WEATHER_FORECAST>(_removeObj) > 0;
//            });

//            return result;
//        }
//    }
//}