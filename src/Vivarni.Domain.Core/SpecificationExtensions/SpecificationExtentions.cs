using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Ardalis.Specification;

namespace Vivarni.Domain.Core.SpecificationExtensions
{
    /// <summary>
    /// Extension methods for <see cref="Ardalis.Specification"/> which allows to configure the
    /// expiration time of cached specifications using <see cref="TimeSpan"/>'s.
    /// </summary>
    public static class SpecificationExtentions
    {
        /// <summary>
        /// We use a <see cref="System.Runtime.CompilerServices.ConditionalWeakTable{TKey, TValue}"/>
        /// to attach caching option data to objects that implement <see cref="Specification{T}"/>.
        /// </summary>
        private static readonly ConditionalWeakTable<object, CacheOptions> SpecificationCacheOptions = new ConditionalWeakTable<object, CacheOptions>();

        /// <summary>
        /// Sets the caching expiration timespan of the specification,
        /// </summary>
        public static void SetCacheTTL<T>(this ISpecification<T> spec, TimeSpan ttl)
        {
            SpecificationCacheOptions.AddOrUpdate(spec, new CacheOptions() { TTL = ttl });
        }

        /// <summary>
        /// Returns the caching expiration timespan of the specification,
        /// or <see cref="TimeSpan.MaxValue" /> if not configured.
        /// </summary>
        public static TimeSpan GetCacheTTL<T>(this ISpecification<T> spec)
        {
            var opts = SpecificationCacheOptions.GetOrCreateValue(spec);
            return opts?.TTL ?? TimeSpan.MaxValue;
        }

        /// <summary>
        /// We need reference types in order to be able to work with ConditionalWeakTable's.
        /// This private class is a workaround because <see cref="TimeSpan"/> is a struct :-)
        /// </summary>
        private class CacheOptions
        {
            public TimeSpan TTL { get; set; }
        }
    }
}
