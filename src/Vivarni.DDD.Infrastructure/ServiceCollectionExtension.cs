using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Core.Repositories;
using Vivarni.DDD.Infrastructure.Caching;
using Vivarni.DDD.Infrastructure.DomainEvents;
using Vivarni.DDD.Infrastructure.Repositories;

namespace Vivarni.DDD.Infrastructure
{
    /// <summary>
    /// Extension methods for adding Vivarni Infrastructure services to the <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds scoped services for <see cref="IGenericRepository{T}"/> and <see cref="IDomainEventBrokerService"/>.
        /// </summary>
        public static IServiceCollection AddVivarniInfrastructure(this IServiceCollection @this, Action<VivarniInfrastructureOptionsBuilder>? optionsBuilder = null)
        {
            var options = new VivarniInfrastructureOptionsBuilder();
            optionsBuilder?.Invoke(options);

            @this.AddScoped(typeof(IDomainEventBrokerService), typeof(DomainEventBrokerService));
            @this.AddCachingProvider(options);

            var key = typeof(IGenericRepository<>);
            if (!options.GenericRepositories.ContainsKey(key))
                options.GenericRepositories.Add(key, typeof(GenericRepository<>));

            foreach (var genericRepository in options.GenericRepositories)
            {
                @this.AddScoped(genericRepository.Key, genericRepository.Value);
            }

            return @this;
        }

        /// <summary>
        /// Registers the configured Vivarni ICachingProvider. The default caching provider
        /// is <see cref="MemoryCacheCachingProvider"/>. When using the default provider, this
        /// method will also register <see cref="IMemoryCache"/> if nessecary.
        /// </summary>
        private static void AddCachingProvider(this IServiceCollection @this, VivarniInfrastructureOptionsBuilder options)
        {
            @this.Add(new ServiceDescriptor(typeof(ICachingProvider), options.CachingProviderType, options.CachingProviderServiceLifetime));

            if (options.CachingProviderType == typeof(MemoryCacheCachingProvider))
                @this.AddMemoryCache();
        }
    }
}
