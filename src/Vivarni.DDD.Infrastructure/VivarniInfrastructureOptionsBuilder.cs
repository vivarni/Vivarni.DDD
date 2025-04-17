using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Core.Repositories;
using Vivarni.DDD.Infrastructure.Caching;
using Vivarni.DDD.Infrastructure.Repositories;

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
        internal Type CachingProviderType { get; set; } = typeof(MemoryCacheCachingProvider);

        /// <summary>
        /// Service lifetime for the <see cref="ICachingProvider"/> used by <see cref="GenericRepository{T}"/>.
        /// </summary>
        internal ServiceLifetime CachingProviderServiceLifetime { get; set; }

        /// <summary>
        /// Additional <see cref="IGenericRepository{T}"/> registrations
        /// </summary>
        internal Dictionary<Type, Type> GenericRepositories { get; set; } = new Dictionary<Type, Type>();

        /// <summary>
        /// Configures the <see cref="ICachingProvider"/> type to be used by <see cref="GenericRepository{T}"/> with the provided service <paramref name="lifeTime"/>.
        /// </summary>
        public VivarniInfrastructureOptionsBuilder WithCachingProvider<T>(ServiceLifetime lifeTime = ServiceLifetime.Scoped)
            where T : ICachingProvider
        {
            CachingProviderType = typeof(T);
            CachingProviderServiceLifetime = lifeTime;

            return this;
        }

        /// <summary>
        /// Configures your own implementation and interface for <see cref="IGenericRepository{T}"/>
        /// 
        /// This method may be used to register additional (multiple) sub interfaces of <see cref="IGenericRepository{T}"/>
        /// </summary>
        public VivarniInfrastructureOptionsBuilder WithGenericRepository(Type genericRepositoryInterface, Type genericRepositoryImplementation)
        {
            if (!GenericRepositories.ContainsKey(genericRepositoryInterface))
                GenericRepositories.Add(genericRepositoryInterface, genericRepositoryImplementation);

            return this;
        }
    }
}
