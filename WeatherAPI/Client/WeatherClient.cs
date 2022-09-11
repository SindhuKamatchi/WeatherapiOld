using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPI.Models.ConfigData;
using WeatherAPI.Models.Response;

namespace WeatherAPI.Client
{
    public class WeatherClient :IWeatherClient
    {
        private readonly HttpClient httpClient;
        private readonly ServiceSettings settings;
        private readonly ILogger<WeatherClient> _logger;

        public WeatherClient(HttpClient httpClient, IOptions<ServiceSettings> options, ILogger<WeatherClient> logger)
        {
            this.httpClient = httpClient;
            settings = options.Value;
            _logger = logger;
        }

        public async Task<RootResponse> GetCurrentWeatherAsync(string city, string country)
        {
            RootResponse rootResponse = new RootResponse();
            try
            {
                _logger.LogInformation("Calling weather API");
                var url = $"{settings.OpenWeatherHost}/v1/current.json?key={settings.ApiKey}&q={city}";

                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                rootResponse = JsonConvert.DeserializeObject<RootResponse>(content);
                if (response.IsSuccessStatusCode && rootResponse.error==null)
                {
                    _logger.LogInformation("weather API success"); 
                }
                else
                {
                    _logger.LogInformation("weather API failure");
                }
                return rootResponse;


            }
            catch (Exception e)
            {
                _logger.LogInformation("weather API exception {error}", e.Message);
                rootResponse = new RootResponse
                {
                    error =new Error
                    {
                        code = "API Exception",
                        message= "weather API exception"
                    }
                };
            }
            return rootResponse;

        }

    }
}