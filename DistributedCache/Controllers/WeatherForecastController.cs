using System.Collections.Immutable;
using System.Net;
using DistributedCache.Caching;
using DistributedCache.Controllers.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace DistributedCache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICacheService _cacheService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<GetWeatherListResponse> Get(CancellationToken token)
        {
            var response = new GetWeatherListResponse();

            var resultFromCache = await _cacheService
                .GetAsync<IReadOnlyList<WeatherForecast>>("weather-records", token);

            if (resultFromCache is not null)
            {
                response.CacheResponse(resultFromCache!);
                return response;
            }
            
            IReadOnlyList<WeatherForecast> result = GenerateWeatherForecasts();
            
            response.DataSourceResponse(result);

            await _cacheService.SetAsync("weather-records", result, token);
            
            return response;
        }

        

        private static IReadOnlyList<WeatherForecast> GenerateWeatherForecasts() =>
            Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToList();
    }
}