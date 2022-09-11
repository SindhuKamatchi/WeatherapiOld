using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using WeatherAPI.Client;
using WeatherAPI.Controllers;
using WeatherAPI.Models.Response;
using WeatherAPI.Test.Fixtures;
using Xunit;

namespace WeatherAPI.Test
{
    public class WeatherForecastControllerTest
    {
        private readonly Mock<IWeatherClient> _mock_weatherClient;
        private readonly Mock<ILogger<WeatherForecastController>> _mockLogger;
        private WeatherForecastController _underTest;
        string city = "London";
        string country = "United Kingdom";

        public WeatherForecastControllerTest()
        {
            _mockLogger = new Mock<ILogger<WeatherForecastController>>();
            _mock_weatherClient = new Mock<IWeatherClient>();
            _underTest = new WeatherForecastController(_mockLogger.Object, _mock_weatherClient.Object);
        }

        [Fact]
        public async Task GetWeatherDetails_With_city_country()
        {
            
            _mock_weatherClient.Setup(client =>
                    client.GetCurrentWeatherAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(RootResponseFixture.ValidRootResponse);

            var response = await _underTest.Get(city, country);
            var result = response.Result as OkObjectResult;
            var resultValue = result.Value as WeatherForecast;
            resultValue.LocalTime.Should().NotBeEmpty();
            resultValue.Weather.Should().NotBeEmpty();
            resultValue.TemperatureC.Should().Be(21);
        }

        [Fact]
        public async Task GetWeatherDetails_With_citynull_country()
        {
            _mock_weatherClient.Setup(client =>
                    client.GetCurrentWeatherAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(RootResponseFixture.ErrorRootResponse);

            var response = await _underTest.Get(String.Empty, country);
            var result = response.Result as BadRequestObjectResult;
            result.StatusCode.Should().NotBeNull();
        }

        [Fact]
        public async Task GetWeatherDetails_With_citynull_countrynull()
        {
            _mock_weatherClient.Setup(client =>
                    client.GetCurrentWeatherAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(RootResponseFixture.ErrorRootResponse);

            var response = await _underTest.Get(String.Empty, String.Empty);
            var result = response.Result as BadRequestObjectResult ;
            result.StatusCode.Should().NotBeNull();
        }
    }
}