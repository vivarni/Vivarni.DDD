using System;
using System.Threading.Tasks;

namespace Vivarni.DDD.Infrastructure.Caching
{
    internal class CachingProviderStub : ICachingProvider
    {
        public Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan timeToLive)
        {
            return null;
        }
    }
}
