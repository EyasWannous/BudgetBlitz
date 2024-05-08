using BudgetBlitz.Application.IServices;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace BudgetBlitz.Infrastructure.Services;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> _cachekeys = new();

    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cacheItem = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (cacheItem is null)
            return null;

        var result = JsonConvert.DeserializeObject<T>(cacheItem);
        if (result is null)
            return null;

        return result;
    }

    public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? cacheItem = await GetAsync<T>(key, cancellationToken);
        if (cacheItem is not null)
            return cacheItem;

        cacheItem = await factory();

        await SetAsync(key, cacheItem, cancellationToken);

        return cacheItem;
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);

        _cachekeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        //foreach (var key in _cachekeys.Keys)
        //{
        //    if(!key.StartsWith(prefixKey)) 
        //        continue;

        //    await _distributedCache.RemoveAsync(key, cancellationToken);
        //}

        IEnumerable<Task> tasks = _cachekeys.Keys
            .Where(key => key.StartsWith(prefixKey))
            .Select(key => RemoveAsync(key, cancellationToken));

        await Task.WhenAll(tasks);
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cacheKey = JsonConvert.SerializeObject(value);

        await _distributedCache.SetStringAsync(key, cacheKey, cancellationToken);

        _cachekeys.TryAdd(key, false);
    }
}
