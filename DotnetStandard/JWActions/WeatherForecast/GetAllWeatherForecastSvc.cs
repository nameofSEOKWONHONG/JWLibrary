namespace JWService.WeatherForecast {
    using FluentValidation;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.Pattern.TaskAction;
    using JWService.Data;
    using SqlKata;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class GetAllWeatherForecastSvc : SvcBase<WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>>, IGetAllWeatherForecastSvc {
        public override bool PreExecute() {
            return true;
        }
        public override IEnumerable<WEATHER_FORECAST> Executed() {
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
        public override bool PostExecute() {
            if(this.Result.jIsNotNull()) {
                //System.Diagnostics.Debug.Assert(false, "result is not null");
            }
            return true;
        }



    }
}