using System;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Core.Repositories;
using Vivarni.DDD.Infrastructure.Caching;
using Vivarni.DDD.Infrastructure.DomainEvents;

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
            @this.AddScoped(typeof(IDomainEventBrokerService), typeof(DomainEventBrokerService));

            var options = new VivarniInfrastructureOptionsBuilder();
            if (optionsBuilder != null)
                optionsBuilder.Invoke(options);
            @this.Add(new ServiceDescriptor(typeof(ICachingProvider), options.CachingProviderType, options.CachingProviderServiceLifetime));

            var key = typeof(IGenericRepository<>);
            if (!options.GenericRepositories.ContainsKey(key))
                options.GenericRepositories.Add(key, typeof(GenericRepository<>));

            foreach (var genericRepository in options.GenericRepositories)
            {
                @this.AddScoped(genericRepository.Key, genericRepository.Value);
            }

            return @this;
        }
    }
}
