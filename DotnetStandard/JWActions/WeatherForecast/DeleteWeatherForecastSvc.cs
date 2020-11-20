namespace JWService.WeatherForecast {

    using Dapper;
    using FluentValidation;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.ServiceExecutor;
    using ServiceExample.Data;
    using System.Data.SqlClient;

    public class DeleteWeatherForecastSvc : ServiceExecutor<DeleteWeatherForecastSvc, DeleteWeatherForecastSvc.Validator, WeatherForecastRequestDto, bool>,
        IDeleteWeatherForecastSvc {
        IGetWeatherForecastSvc _getWeatherForecastSvc;
        private WEATHER_FORECAST _removeObj = null;

        public DeleteWeatherForecastSvc(IGetWeatherForecastSvc getWeatherForecastSvc) {
            this._getWeatherForecastSvc = getWeatherForecastSvc;
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetWeatherForecastSvc>(this._getWeatherForecastSvc);
            executor.SetRequest(o => o.Request = new WeatherForecastRequestDto() { ID = this.Request.ID })
                .OnExecuted(o => this._removeObj = o.Result);

            if (this._removeObj.jIsNotNull()) return true;
            return false;
        }

        public override void Execute() {
            this.Result = JDataBase.Resolve<SqlConnection>().jQuery<bool>(db => {
                return db.Delete<WEATHER_FORECAST>(_removeObj) > 0;
            });
        }

        public class Validator : AbstractValidator<DeleteWeatherForecastSvc> {
            public Validator() {

            }
        }
    }
}