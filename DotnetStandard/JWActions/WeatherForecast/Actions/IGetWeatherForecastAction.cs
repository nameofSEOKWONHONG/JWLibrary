using System;
using System.Collections.Generic;
using System.Text;

using JWLibrary.Pattern.TaskAction;

namespace JWActions {
    public interface IGetWeatherForecastAction : IActionBase<WeatherForecastRequestDto, WEATHER_FORECAST> { }
    public interface IGetAllWeatherForecastAction : IActionBase<WeatherForecastRequestDto, IEnumerable<WEATHER_FORECAST>> { }
}
