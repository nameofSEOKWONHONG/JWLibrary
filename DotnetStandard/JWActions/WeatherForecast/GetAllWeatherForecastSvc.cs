namespace JWService.WeatherForecast {
    using FluentValidation;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.ServiceExecutor;
    using ServiceExample.Data;
    using SqlKata;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class GetAllWeatherForecastSvc : ServiceExecutor<GetAllWeatherForecastSvc, GetAllWeatherForecastSvc.Validator, WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>>, IGetAllWeatherForecastSvc {
        public override void Execute() {
            var query = new Query("WEATHER_FORECAST").Select("*");
            this.Result = JDataBase.Resolve<SqlConnection>()
                        .jGetAll<WEATHER_FORECAST>(query, SQL_COMPILER_TYPE.MSSQL, items => {
                            items.jForEach(item => {
                                item.TEMPERATURE_F = 32 + (int)(item.TEMPERATURE_C / 0.5556);
                                return true;
                            });

                            return items;
                        });
        }

        public class Validator : AbstractValidator<GetAllWeatherForecastSvc> {
            public Validator() {

            }
        }
    }
}