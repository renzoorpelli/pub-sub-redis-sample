using Microsoft.Extensions.DependencyInjection;

namespace PubSub.Shared.Serialization;

public static class Extension
{
    public static IServiceCollection AddSerialization(this IServiceCollection services)
        => services.AddSingleton<ISerializer, DefaultSerializer>();
}