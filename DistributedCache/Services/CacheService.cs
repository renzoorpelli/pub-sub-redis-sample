using System.Text.Json;
using DistributedCache.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCache.Services;

internal sealed class CacheService(IDistributedCache distributedCache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class
    {
        var cachedValue = await distributedCache.GetStringAsync(key, token);

        if (cachedValue is null)
        {
            return null;
        }

        var result = JsonSerializer.Deserialize<T>(
            cachedValue);

        return result;
    }

    public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> func, CancellationToken token = default) where T : class
    {
        var result = await this.GetAsync<T>(key, token);

        if (result is not null)
        {
            return result;
        }
        
        // if cache is null use Func provided to set the data to the cache
        T cachedValue = await func();

        await this.SetAsync(key, cachedValue, token);

        return cachedValue;
    }

    public async Task<bool> SetAsync<T>(string key, T data, CancellationToken token = default) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(data);

        await distributedCache.SetStringAsync(key, cacheValue, token);

        return true;
    }

    public async Task<bool> InvalidateAsync(string key, CancellationToken token = default)
    {
        await distributedCache.RemoveAsync(key, token);

        return true;
    }
}