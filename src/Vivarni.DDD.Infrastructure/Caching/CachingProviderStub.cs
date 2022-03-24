using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
