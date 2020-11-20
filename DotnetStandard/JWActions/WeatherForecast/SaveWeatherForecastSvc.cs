namespace ServiceExample.WeatherForecast {
    using FluentValidation;
    using Dapper;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.Pattern.TaskService;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using ServiceExample.Data;
    using JWLibrary.Pattern.Chainging;

    public class SaveWeatherForecastSvc : BaseService<WEATHER_FORECAST, int>, ISaveWeatherForecastSvc {
        private WEATHER_FORECAST _exists = null;
        private IGetWeatherForecastSvc _getWeatherForecastSvc;
        public SaveWeatherForecastSvc(IGetWeatherForecastSvc getWeatherForecastSvc) {
            this._getWeatherForecastSvc = getWeatherForecastSvc;
        }

        public override bool PreExecute() {
            using var executor = new ServiceExecutorManager<WeatherForecastRequestDto, WEATHER_FORECAST>(_getWeatherForecastSvc);
            this._exists = executor.SetRequest(o => new WeatherForecastRequestDto() { ID = this.Request.ID })
                .OnExecuted(o => o);

            if (this._exists.jIsNotNull()) return true;
            return false;
        }

        public override int Execute() {
            return JDataBase.Resolve<SqlConnection>()
                        .jQuery<int>(db => {
                            if (this._exists.jIsNotNull()) {
                                return db.Update<WEATHER_FORECAST>(this.Request);
                            }

                            return db.Insert<WEATHER_FORECAST>(this.Request).Value;
                        });
        }

        public override void PostExecute() {
            base.PostExecute();
        }

        public class Validator : AbstractValidator<SaveWeatherForecastSvc> {
            public Validator() {
                RuleFor(o => o.Request).NotNull();
                RuleFor(o => o.Request.ID).GreaterThan(0);
            }
        }
    }
}