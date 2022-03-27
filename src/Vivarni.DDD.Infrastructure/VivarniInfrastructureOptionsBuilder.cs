using System;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Infrastructure.Caching;

namespace Vivarni.DDD.Infrastructure
{
    /// <summary>
    /// Configures the Vivarni infrastructure options.
    /// </summary>
    public class VivarniInfrastructureOptionsBuilder
    {
        /// <summary>
        /// The <see cref="ICachingProvider"/> type used by <see cref="GenericRepository{T}"/>.
        /// </summary>
        public Type CachingProviderType { internal get; set; } = typeof(CachingProviderStub);

        /// <summary>
        /// Service lifetime for the <see cref="ICachingProvider"/> used by <see cref="GenericRepository{T}"/>.
        /// </summary>
        internal ServiceLifetime CachingProviderServiceLifetime { get; set; }

        /// <summary>
        /// Configures the <see cref="ICachingProvider"/> type to be used by <see cref="GenericRepository{T}"/> with the provided service <paramref name="lifeTime"/>.
        /// </summary>
        public void WithCachingProvider<T>(ServiceLifetime lifeTime = ServiceLifetime.Scoped)
            where T : ICachingProvider
        {
            CachingProviderType = typeof(T);
            CachingProviderServiceLifetime = lifeTime;
        }
    }
}
