using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Vivarni.DDD.Infrastructure.Caching
{
    internal class MemoryCacheCachingProvider : ICachingProvider
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheCachingProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan timeToLive, bool forceRefresh)
        {
            if (forceRefresh)
                _memoryCache.Remove(cacheKey);

            return _memoryCache.GetOrCreateAsync<T>(cacheKey, async e =>
            {
                e.AbsoluteExpirationRelativeToNow = timeToLive;
                return await dataRetriever();
            });
        }
    }
}
