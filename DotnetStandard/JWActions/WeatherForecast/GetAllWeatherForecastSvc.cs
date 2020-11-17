namespace JWService.WeatherForecast {
    using FluentValidation;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.Pattern.Chainging;
    using JWLibrary.Pattern.TaskService;
    using JWService.Data;
    using SqlKata;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class GetAllWeatherForecastSvc : BaseService<WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>>, IGetAllWeatherForecastSvc {
        public override IEnumerable<WEATHER_FORECAST> Execute() {
            var query = new Query("WEATHER_FORECAST").Select("*");
            var result = JDataBase.Resolve<SqlConnection>()
                        .jGetAll<WEATHER_FORECAST>(query, SQL_COMPILER_TYPE.MSSQL, items => {
                            items.jForEach(item => {
                                item.TEMPERATURE_F = 32 + (int)(item.TEMPERATURE_C / 0.5556);
                                return true;
                            });

                            return items;
                        });

            return result;
        }
    }
}