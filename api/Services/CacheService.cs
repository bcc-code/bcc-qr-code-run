using api.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace api.Services
{
    public class CacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IDistributedCache distributedCache, IMemoryCache memoryCache, ILogger<CacheService> logger)
        {
            _distributedCache = distributedCache;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();

        public async Task<TItem?> GetOrCreateAsync<TItem>(string key, TimeSpan ttl, Func<Task<TItem>> factory)
        {
            // Attempt to retrieve from memory
            var itm = _memoryCache.Get<TItem>(key);
            if (itm != null) return itm;

            // Only allow one thread to retrieve value from remote sources (distributed cached or database)
            var semaphore =  _semaphores.GetOrAdd(key, new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            try
            {
                // Retrieve value and add to memory cache
                return (await _memoryCache.GetOrCreateAsync(key, async c =>
                {
                    TItem? item = default(TItem);
                    try
                    {
                        // Attempt to retrieve from distributed cache
                        var timeoutToken = new CancellationTokenSource(2000).Token;
                        var sValue = await _distributedCache.GetStringAsync(key, timeoutToken);
                        if (sValue != null)
                        {
                            item = JsonConvert.DeserializeObject<TItem>(sValue);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to retrieve item from distributed cache. Key: {key}");
                    }
                    if (item == null)
                    {
                        // Retrieve from source
                        try
                        {
                            item = await factory();
                            if (item != null)
                            {
                                try
                                {
                                    // Attempt to write to distributed cache
                                    var timeoutToken = new CancellationTokenSource(5000).Token;
                                    await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(item), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl }, timeoutToken);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, $"Failed to store item in distributed cache. Key: {key}");
                                }
                            }
                        } 
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to retrieve item from source. Key: {key}");
                        }
                    }
                    if (item != null)
                    {
                        c.AbsoluteExpirationRelativeToNow = ttl;
                        return item;
                    }
                    else
                    {
                        c.AbsoluteExpirationRelativeToNow = TimeSpan.Zero;
                        return default(TItem);
                    }
                }));
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
