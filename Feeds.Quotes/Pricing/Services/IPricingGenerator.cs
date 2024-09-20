using PubSub.Redis.Models;

namespace PubSub.Redis.Pricing.Services;

internal interface IPricingGenerator
{
    /// <summary>
    /// This method will generate mock data and will be called in the backgorundService
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<CurrencyPair> StartAsync();

    Task StopAsync();
}