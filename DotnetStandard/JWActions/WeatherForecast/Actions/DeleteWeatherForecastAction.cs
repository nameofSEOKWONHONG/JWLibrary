using JWLibrary.Core;
using JWLibrary.Database;
using JWLibrary.Pattern.TaskAction;
using System;
using System.Collections.Generic;
using System.Text;

using Dapper;

namespace JWActions {
    public class DeleteWeatherForecastAction : ActionBase<WeatherForecastRequestDto, bool>, IDeleteWeatherForecastAction {
        WEATHER_FORECAST _removeObj = null;
        public override bool Executed() {
            using (var action = ActionFactory.CreateAction<IGetWeatherForecastAction, GetWeatherForecastAction, WeatherForecastRequestDto, WEATHER_FORECAST>()) {
                action.Request = new WeatherForecastRequestDto() {
                    ID = this.Request.ID
                };
                _removeObj = action.ExecuteCore().Result;
                return _removeObj.jIsNotNull();
            }
        }

        public override bool PostExecute() {
            return DatabaseConfig.DB_CONNECTION.jQuery<bool>(db => {
                return db.Delete<WEATHER_FORECAST>(_removeObj) > 0;
            });
        }

        public override bool PreExecute() {
            return true;
        }
    }
}
