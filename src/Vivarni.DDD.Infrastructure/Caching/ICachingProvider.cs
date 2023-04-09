using System;
using System.Threading.Tasks;

namespace Vivarni.DDD.Infrastructure.Caching
{
    /// <summary>
    /// Proxy for retrieving data from an abritary source.
    /// Allows to cache certain data obtained from some data retriever.
    /// 
    /// </summary>
    public interface ICachingProvider
    {
        /// <summary>
        /// Retrieves data from the <paramref name="dataRetriever"/> and keeps it cached
        /// respecting the provided <paramref name="timeToLive"/>. The cached value
        /// is associated with <paramref name="cacheKey"/>.
        /// </summary>
        /// <typeparam name="T">Entity type that needs to be returned by the <paramref name="dataRetriever"/>.</typeparam>
        /// <param name="cacheKey">Key to associate the cached object with.</param>
        /// <param name="dataRetriever">Function to actually retrieve data with, in case of missing <paramref name="cacheKey"/>'s.</param>
        /// <param name="timeToLive">Time to live</param>
        /// <param name="forceRefresh">Indicactes whether the case should be forcefully be cleared for the provided <paramref name="cacheKey"/>.</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> dataRetriever, TimeSpan timeToLive, bool forceRefresh);
    }
}
