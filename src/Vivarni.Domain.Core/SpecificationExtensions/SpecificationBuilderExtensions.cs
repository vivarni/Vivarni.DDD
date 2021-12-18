﻿using System;
using Ardalis.Specification;

namespace Vivarni.Domain.Core.SpecificationExtensions
{
    /// <summary>
    /// Provides extension methods on the "Ardalis.Specification" nuget package.
    /// </summary>
    public static class SpecificationBuilderExtensions
    {
        /// <summary>
        /// Configures the cached specification to expire after a certain timespan (TTL = Time To Live).
        /// </summary>
        public static ISpecificationBuilder<T> WithCachingTTL<T>(this ISpecificationBuilder<T> @this, TimeSpan ttl)
            where T : class
        {
            @this.Specification.SetCacheTTL(ttl);
            return @this;
        }
    }
}
