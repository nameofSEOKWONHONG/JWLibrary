namespace JWService.WeatherForecast {
    using FluentValidation;
    using Dapper;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.Pattern.TaskService;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Logging;
    using JWService.Data;

    public class SaveWeatherForecastSvc : SvcBase<WEATHER_FORECAST, int>, ISaveWeatherForecastSvc {
        private WEATHER_FORECAST _exists = null;

        public override bool PreExecute() {
            using (var action = ServiceFactory.CreateService<IGetWeatherForecastSvc, GetWeatherForecastSvc, WeatherForecastRequestDto, WEATHER_FORECAST>()) {
                _exists = action.SetLogger(base.Logger)
                    .SetRequest(new WeatherForecastRequestDto() {
                        ID = this.Request.ID
                    })
                    .ExecuteAsync().Result;
            }
            return true;
            //return DatabaseConfig.DB_CONNECTION.jQuery<bool>(db => {
            //    var sql = "SELECT * FROM WEATHER_FORECAST WHERE ID = @ID";
            //    _exists = db.QueryFirstOrDefault<WEATHER_FORECAST>(sql, new { ID = this.Request.ID });
            //    return true;
            //});
        }

        public override int Executed() {
            return JDataBase.Resolve<SqlConnection>()
                        .jQuery<int>(db => {
                            if (this._exists.jIsNotNull()) {
                                return db.Update<WEATHER_FORECAST>(this.Request);
                            }

                            return db.Insert<WEATHER_FORECAST>(this.Request).Value;
                        });
        }

        public override bool PostExecute() {
            return true;
        }

        public class Validator : AbstractValidator<SaveWeatherForecastSvc> {
            public Validator() {
                RuleFor(o => o.Request).NotNull();
                RuleFor(o => o.Request.ID).GreaterThan(0);
            }
        }
    }
}