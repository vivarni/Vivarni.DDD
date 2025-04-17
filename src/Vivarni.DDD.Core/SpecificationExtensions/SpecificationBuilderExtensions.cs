using System;
using Ardalis.Specification;

namespace Vivarni.DDD.Core.SpecificationExtensions;

/// <summary>
/// Provides extension methods on the "Ardalis.Specification" nuget package.
/// </summary>
public static class SpecificationBuilderExtensions
{
    /// <summary>
    /// Configures the cached specification to expire after a certain timespan (TTL = Time To Live).
    /// </summary>
    public static ICacheSpecificationBuilder<T> WithCachingTTL<T>(this ICacheSpecificationBuilder<T> @this, TimeSpan ttl)
        where T : class
    {
        @this.Specification.SetCacheTTL(ttl);
        return @this;
    }

    /// <summary>
    /// Configures the cached specification to forcefully refresh. In other words: the
    /// specification result associated with the cache key of this specification will
    /// be forcefully refreshed.
    /// </summary>
    public static ICacheSpecificationBuilder<T> WithForcedCacheRefresh<T>(this ICacheSpecificationBuilder<T> @this, bool forceCacheRefresh = true)
        where T : class
    {
        if (forceCacheRefresh)
            @this.Specification.SetForcedCacheRefreshFlag();

        return @this;
    }
}
