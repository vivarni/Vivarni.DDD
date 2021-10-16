using System;
using System.Threading.Tasks;

namespace Vivarni.Domain.Infrastructure.Caching
{
    public interface ICachingProvider
    {
        Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan expiration);
    }
}
