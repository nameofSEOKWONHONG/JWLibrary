namespace ServiceExample.WeatherForecast {
    using Dapper;
    using FluentValidation;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.ServiceExecutor;
    using JWService.WeatherForecast;
    using ServiceExample.Data;
    using System.Data.SqlClient;

    public class SaveWeatherForecastSvc : ServiceExecutor<SaveWeatherForecastSvc, SaveWeatherForecastSvc.Validator, WEATHER_FORECAST, int>, ISaveWeatherForecastSvc {
        private WEATHER_FORECAST _exists = null;
        private IGetWeatherForecastSvc _getWeatherForecastSvc;
        public SaveWeatherForecastSvc(IGetWeatherForecastSvc getWeatherForecastSvc) {
            this._getWeatherForecastSvc = getWeatherForecastSvc;
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<IGetWeatherForecastSvc>(this._getWeatherForecastSvc);
            executor.SetRequest(o => o.Request = new WeatherForecastRequestDto() { ID = this.Request.ID })
                .OnExecuted(o => {
                    this._exists = o.Result;
                });

            if (this._exists.jIsNotNull()) return true;
            return false;
        }

        public override void Execute() {
            this.Result =
                JDataBase.Resolve<SqlConnection>()
                    .jQuery<int>(db => {
                        if (this._exists.jIsNotNull()) {
                            return db.Update<WEATHER_FORECAST>(this.Request);
                        }
                        return db.Insert<WEATHER_FORECAST>(this.Request).Value;
                    });
        }

        public class Validator : AbstractValidator<SaveWeatherForecastSvc> {
            public Validator() {
                RuleFor(o => o.Request).NotNull();
                RuleFor(o => o.Request.ID).GreaterThan(0);
            }
        }
    }
}