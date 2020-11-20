namespace ServiceExample.WeatherForecast {
    using FluentValidation;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.ServiceExecutor;
    using JWService.WeatherForecast;
    using ServiceExample.Data;
    using SqlKata;
    using System.Data.SqlClient;

    public class GetWeatherForecastSvc : ServiceExecutor<GetWeatherForecastSvc, GetWeatherForecastSvc.Validator, WeatherForecastRequestDto, WEATHER_FORECAST>, IGetWeatherForecastSvc {
        public override void Execute() {
            //use sqlkata
            var query = new Query("WEATHER_FORECAST").Where("ID", this.Request.ID).Select("*");
            this.Result = JDataBase.Resolve<SqlConnection>()
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
        public class Validator : AbstractValidator<GetWeatherForecastSvc> {
            public Validator() {
                RuleFor(o => o.Request).NotNull();
                RuleFor(o => o.Request.ID).GreaterThan(0);
            }
        }
    }
}