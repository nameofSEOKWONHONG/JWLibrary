namespace JWActions.WeatherForecast {

    using JWLibrary.Pattern.TaskAction;
    using System.Collections.Generic;

    public interface IGetWeatherForecastAction : IActionBase<WeatherForecastRequestDto, WEATHER_FORECAST> { }

    public interface IGetAllWeatherForecastAction : IActionBase<WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>> { }

    public interface ISaveWeatherForecastAction : IActionBase<WEATHER_FORECAST, int> { }

    public interface IDeleteWeatherForecastAction : IActionBase<WeatherForecastRequestDto, bool> { }
}