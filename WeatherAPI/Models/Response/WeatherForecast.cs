using System;

namespace WeatherAPI.Models.Response
{
    public class WeatherForecast
    {
        public string LocalTime { get; set; }

        public int TemperatureC { get; set; }

        public string Weather { get; set; }

        public string ErrorMessage { get; set; }
    }
}
