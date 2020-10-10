namespace JWService.WeatherForecast {
    using JWLibrary.Pattern.TaskAction;
    using JWService.Data;
    using System.Collections.Generic;

    public interface IGetWeatherForecastSvc : ISvcBase<WeatherForecastRequestDto, WEATHER_FORECAST> { }

    public interface IGetAllWeatherForecastSvc : ISvcBase<WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>> { }

    public interface ISaveWeatherForecastSvc : ISvcBase<WEATHER_FORECAST, int> { }

    public interface IDeleteWeatherForecastSvc : ISvcBase<WeatherForecastRequestDto, bool> { }
}