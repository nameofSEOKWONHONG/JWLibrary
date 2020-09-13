namespace JWActions.WeatherForecast {
    using FluentValidation;
    using JAction;
    using JAction.Data;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.Pattern.TaskAction;
    using SqlKata;
    using System.Data.SqlClient;

    public class GetWeatherForecastAction : ActionBase<WeatherForecastRequestDto, WEATHER_FORECAST>, IGetWeatherForecastAction {
        public GetWeatherForecastAction() {

        }

        public override bool PreExecute() {
            return true;
        }

        public override WEATHER_FORECAST Executed() {
            //use sqlkata
            var query = new Query("WEATHER_FORECAST").Where("ID", this.Request.ID).Select("*");
            return JDataBase.Resolve<SqlConnection>()
                        .jGet<WEATHER_FORECAST>(query, SQL_COMPILER_TYPE.MSSQL, item => {
                            if (item.jIsNull()) return null;
                            item.TEMPERATURE_F = 32 + (int)(item.TEMPERATURE_C / 0.5556);
                            return item;
                        });

            //use text query
            //return DatabaseConfig.DB_CONNECTION.Get<WEATHER_FORECAST>($"select * from dbo.WEATHER_FORECAST where ID = {this.Request.ID}", item => {
            //    item.TEMPERATURE_F = 32 + (int)(item.TEMPERATURE_C / 0.5556);
            //    return item;
            //});
        }

        public override bool PostExecute() {
            return true;
        }

        public class Validator : AbstractValidator<GetWeatherForecastAction> {
            public Validator() {
                RuleFor(o => o.Request).NotNull();
                RuleFor(o => o.Request.ID).GreaterThan(0);
            }
        }
    }
}