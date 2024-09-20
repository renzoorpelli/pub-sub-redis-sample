using PubSub.Redis.Pricing.Request;
using PubSub.Shared.Streaming;

namespace PubSub.Redis.Pricing.Services;

internal class PricingBackgroundService : BackgroundService
{
    private int _runningStatus;
    private readonly ILogger<PricingBackgroundService> _logger;
    private readonly IPricingGenerator _pricingGenerator;
    private readonly PricingRequestChannel _pricingRequestChannel;
    private readonly IStreamPublisher _steamPublisher;

    public PricingBackgroundService(
        ILogger<PricingBackgroundService> logger,
        IPricingGenerator pricingGenerator, 
        PricingRequestChannel pricingRequestChannel,
        IStreamPublisher steamPublisher)
    {
        _logger = logger;
        _pricingGenerator = pricingGenerator;
        _pricingRequestChannel = pricingRequestChannel;
        _steamPublisher = steamPublisher;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var request in _pricingRequestChannel.Requests.Reader.ReadAllAsync(stoppingToken))
        {
            var _ = request switch
            {
                StartPricing => StartGeneratorAsync(),
                StopPricing => StopGeneratorAsync(),
                _ => Task.CompletedTask
            };
        }
    }


    private async Task StartGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
        {
            // is already running
            return;
        }

        await foreach (var cp in _pricingGenerator.StartAsync())
        {
            _logger.LogInformation("publishing the currency");
            await _steamPublisher.PublishAsync("pricing", cp);
        }
    }
    
    private async Task StopGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
        {
            // is not running
            return;
        }

        await _pricingGenerator.StopAsync();
    }
}