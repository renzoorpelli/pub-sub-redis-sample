using System.Net;

namespace DistributedCache.Controllers.Response;

public record GetWeatherListResponse()
{
    public IReadOnlyList<WeatherForecast?> Data { get; set; }
    public bool? Error { get;  set; }
    public bool? Cached { get;  set; }
    public string? ErrorList{ get;  set; }
    public HttpStatusCode? StatusCode{ get;  set; }
};

public static class Extensions
{
    public static GetWeatherListResponse DataSourceResponse(this GetWeatherListResponse response, IReadOnlyList<WeatherForecast> result)
    {
        response.Data = result;
        response.Error = false;
        response.StatusCode = HttpStatusCode.OK;
        response.Cached = false;

        return response;
    }
    
    public static GetWeatherListResponse CacheResponse(this GetWeatherListResponse response, IReadOnlyList<WeatherForecast> result)
    {
        response.Data = result;
        response.Error = false;
        response.StatusCode = HttpStatusCode.OK;
        response.Cached = true;

        return response;
    }
}