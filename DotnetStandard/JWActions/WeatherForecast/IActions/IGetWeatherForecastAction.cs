namespace JWActions.WeatherForecast {
    using System;
    using System.Collections.Generic;
    using System.Text;

    using JWLibrary.Pattern.TaskAction;

    public interface IGetWeatherForecastAction : IActionBase<WeatherForecastRequestDto, WEATHER_FORECAST> { }
    public interface IGetAllWeatherForecastAction : IActionBase<WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>> { }

    public interface ISaveWeatherForecastAction : IActionBase<WEATHER_FORECAST, int> { }

    public interface IDeleteWeatherForecastAction : IActionBase<WeatherForecastRequestDto, bool> { }

}
