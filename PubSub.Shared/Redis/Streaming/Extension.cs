using Microsoft.Extensions.DependencyInjection;
using PubSub.Shared.Streaming;

namespace PubSub.Shared.Redis.Streaming;

public static class Extension
{
    public static IServiceCollection AddRedisStreaming(this IServiceCollection services)
        => services
            .AddSingleton<IStreamSubscriber, ReadisStreamSubscriber>()
            .AddSingleton<IStreamPublisher, ReadisStreamPublisher>();
}