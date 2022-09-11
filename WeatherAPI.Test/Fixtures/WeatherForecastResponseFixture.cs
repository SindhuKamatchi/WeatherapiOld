using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAPI.Models.Response;

namespace WeatherAPI.Test.Fixtures
{
    public class WeatherForecastResponseFixture
    {
        public static WeatherForecast ValidResponse => new WeatherForecast()
        {
            LocalTime = "07:23",
            TemperatureC = 14,
            Weather = "Light rain shower",
            ErrorMessage = null
        };

        public static WeatherForecast ErrorResponse => new WeatherForecast()
        {
            ErrorMessage = "City or Country is empty"
        };
    }
}
