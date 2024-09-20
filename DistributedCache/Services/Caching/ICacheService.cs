namespace DistributedCache.Caching;

public interface ICacheService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key">returns from distributed cache by the key</param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T?> GetAsync<T>(string key, CancellationToken token = default )
        where T : class;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key">returns from distributed cache by the key</param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T>> func, CancellationToken token = default )
        where T : class;
    
    /// <summary>
    /// Set data into cache
    /// </summary>
    /// <param name="data"></param>
    /// <param name="token"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<bool> SetAsync<T>(string key, T data, CancellationToken token = default)
        where T : class;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<bool> InvalidateAsync(string key, CancellationToken token = default);
}