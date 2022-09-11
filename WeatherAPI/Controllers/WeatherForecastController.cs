using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherAPI.Client;
using WeatherAPI.Models.Response;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("v1/weather")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherClient _client;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherClient client)
        {
            _logger = logger;
            _client = client;
        }

        public float ConvertToCelcius(float temp)
        {
            float result = (temp - 32) * 5 / 9; ;
            return result;
        }

        [HttpGet("city={city}&country={country}")]
        public async Task<ActionResult<WeatherForecast>> Get(string city, string country)
        {
            try
            {
                _logger.LogInformation("Request received {city}{country}", city, country);
                var forecast = await _client.GetCurrentWeatherAsync(city, country);
                if (forecast.error ==null)
                {
                    WeatherForecast weatherForecast = new WeatherForecast
                    {
                        Weather = forecast.current.condition.text,
                        TemperatureC = (int)ConvertToCelcius(forecast.current.temp_f),
                        LocalTime = DateTime.Parse(forecast.location.localtime).ToString("HH:mm")
                    };
                    return Ok(weatherForecast);
                }
                else
                {
                    WeatherForecast weatherForecast =  new WeatherForecast
                    {
                        ErrorMessage = forecast.error.message
                    };

                    return BadRequest(weatherForecast.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Calling {city}{country} at {date}", city, country, DateTime.Now);
                WeatherForecast weatherForecast = new WeatherForecast
                {
                    ErrorMessage = ex.Message
                };

                return BadRequest(weatherForecast.ErrorMessage);
            }
        }
    }
}
