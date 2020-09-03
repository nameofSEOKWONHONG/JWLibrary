using System;
using System.Collections.Generic;
using System.Text;

using JWLibrary.Pattern.TaskAction;
using JWLibrary.Database;

using SqlKata;
using JWLibrary.Core;

namespace JWActions {
    public class GetWeatherForecastAction : ActionBase<WeatherForecastRequestDto, WEATHER_FORECAST>, IGetWeatherForecastAction {
        public override bool PostExecute() {
            return true;
        }
        public override WEATHER_FORECAST Executed() {
            //use sqlkata
            var query = new Query("WEATHER_FORECAST").Where("ID", this.Request.ID).Select("*");
            return DatabaseConfig.DB_CONNECTION.jGet<WEATHER_FORECAST>(query, SQL_COMPILER_TYPE.MSSQL, item => {
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

        public override bool PreExecute() {
            return true;
        }
    }
}
