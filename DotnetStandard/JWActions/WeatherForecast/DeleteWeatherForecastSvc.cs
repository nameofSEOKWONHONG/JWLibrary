namespace JWService.WeatherForecast {

    using Dapper;
    using JWService;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.Pattern.TaskAction;
    using System.Data.SqlClient;
    using JWService.Data;

    public class DeleteWeatherForecastSvc : SvcBase<WeatherForecastRequestDto, bool>, IDeleteWeatherForecastSvc {
        private WEATHER_FORECAST _removeObj = null;

        public override bool Executed() {
            using (var action = ServiceFactory.CreateService<IGetWeatherForecastSvc, GetWeatherForecastSvc, WeatherForecastRequestDto, WEATHER_FORECAST>()) {
                _removeObj = action.SetLogger(base.Logger)
                    .SetRequest(new WeatherForecastRequestDto() {
                        ID = this.Request.ID
                    })
                    .ExecuteCoreAsync().Result;
                return _removeObj.jIsNotNull();
            }
        }

        public override bool PostExecute() {
            return JDataBase.Resolve<SqlConnection>().jQuery<bool>(db => {
                return db.Delete<WEATHER_FORECAST>(_removeObj) > 0;
            });
        }

        public override bool PreExecute() {
            return true;
        }
    }
}