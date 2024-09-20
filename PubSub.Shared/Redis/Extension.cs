using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace PubSub.Shared.Redis;

public static class Extension
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("redis");
        var options = new RedisOptions();
        section.Bind(options);
        services.Configure<RedisOptions>(section);
        //single instace, stackchange.Redis
        // if this is a required infrastructure and the redis server is down, the application wont start

        services.TryAddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options.ConnectionString!));

        return services;
    }
}