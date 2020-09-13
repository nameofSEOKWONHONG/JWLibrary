namespace JWActions.WeatherForecast {
    using FluentValidation;
    using Dapper;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.Pattern.TaskAction;
    using JAction;
    using JAction.Data;
    using System.Data.SqlClient;

    public class SaveWeatherForecastAction : ActionBase<WEATHER_FORECAST, int>, ISaveWeatherForecastAction {
        private WEATHER_FORECAST _exists = null;

        public override bool PreExecute() {
            using (var action = ActionFactory.CreateAction<IGetWeatherForecastAction, GetWeatherForecastAction, WeatherForecastRequestDto, WEATHER_FORECAST>()) {
                action.Request = new WeatherForecastRequestDto() {
                    ID = this.Request.ID
                };
                _exists = action.ExecuteCore().Result;
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

        public class Validator : AbstractValidator<SaveWeatherForecastAction> {
            public Validator() {
                RuleFor(o => o.Request).NotNull();
                RuleFor(o => o.Request.ID).GreaterThan(0);
            }
        }
    }
}