using PubSub.Redis.Models;

namespace PubSub.Redis.Pricing.Services;

internal class PricingGenerator : IPricingGenerator
{
    private readonly Random _random = new();
    private readonly ILogger<PricingGenerator> _logger;

    private readonly Dictionary<string, decimal> _currencyPairs = new()
    {
        ["EURUSD"] = 1.13M,
        ["EURGBP"] = 0.85M,
        ["EURCHF"] = 1.04M,
        ["EURPLN"] = 4.62M
    };
    
    private bool _isRunning;
    public event EventHandler<CurrencyPair>? PricingUpdated;

    public PricingGenerator(ILogger<PricingGenerator> logger)
    {
        _logger = logger;
    }
    public async IAsyncEnumerable<CurrencyPair> StartAsync()
    {
        _logger.LogInformation("Pricing generator is starting!");
        _isRunning = true;
        while (_isRunning)
        {
            foreach (var (symbol, pricing) in _currencyPairs)
            {
                if (!_isRunning)
                {
                    yield break;
                }

                var tick = NextTick();
                var newPricing = pricing + tick;
                _currencyPairs[symbol] = newPricing;

                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                var currencyPair = new CurrencyPair(symbol, newPricing, timestamp);
                _logger.LogInformation("Currency pair genearted in {Symbol}", currencyPair.Symbol);
                yield return currencyPair;
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }

    public Task StopAsync()
    {
        _logger.LogInformation("Pricing generator is stopped!");
        _isRunning = !_isRunning;
        return Task.CompletedTask;
    }
    
    private decimal NextTick()
    {
        var sign = _random.Next(0, 2) == 0 ? -1 : 1;
        var tick = _random.NextDouble() / 20;
        return (decimal) (sign * tick);
    }
}