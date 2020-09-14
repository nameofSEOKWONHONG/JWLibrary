namespace JWActions.WeatherForecast {

    using Dapper;
    using JAction;
    using JAction.Data;
    using JWLibrary.Core;
    using JWLibrary.Database;
    using JWLibrary.Pattern.TaskAction;
    using System.Data.SqlClient;

    public class DeleteWeatherForecastAction : ActionBase<WeatherForecastRequestDto, bool>, IDeleteWeatherForecastAction {
        private WEATHER_FORECAST _removeObj = null;

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
            return JDataBase.Resolve<SqlConnection>().jQuery<bool>(db => {
                return db.Delete<WEATHER_FORECAST>(_removeObj) > 0;
            });
        }

        public override bool PreExecute() {
            return true;
        }
    }
}