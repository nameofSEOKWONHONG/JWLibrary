namespace JWActions {
    using System;
    using System.Collections.Generic;
    using System.Text;

    using JWLibrary.Pattern.TaskAction;
    using JWLibrary.Database;
    using JWLibrary.Core;

    using SqlKata;
    using SqlKata.Compilers;

    public class GetAllWeatherForecastAction : ActionBase<WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>>, IGetAllWeatherForecastAction {
        public override bool PostExecute() {
            return true;
        }
        public override IEnumerable<WEATHER_FORECAST> Executed() {
            var query = new Query("WEATHER_FORECAST").Select("*");
            return DatabaseConfig.DB_CONNECTION.jGetAll<WEATHER_FORECAST>(query, SQL_COMPILER_TYPE.MSSQL, items => {
                items.jForEach(item => {
                    item.TEMPERATURE_F = 32 + (int)(item.TEMPERATURE_C / 0.5556);
                    return true;
                });

                return items;
            });
        }

        public override bool PreExecute() {
            return true;
        }
    }
}
