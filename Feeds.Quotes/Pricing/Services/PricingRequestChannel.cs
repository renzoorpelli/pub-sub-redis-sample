using System.Threading.Channels;
using PubSub.Redis.Pricing.Request;

namespace PubSub.Redis.Pricing.Services;

internal sealed class PricingRequestChannel
{
    public readonly Channel<IPricingRequest> Requests = Channel.CreateUnbounded<IPricingRequest>();
}