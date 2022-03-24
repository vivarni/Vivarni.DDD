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
        public static IServiceCollection AddVivarniInfrastructure(this IServiceCollection @this, Action<VivarniInfrastructureOptionsBuilder> optionsBuilder)
        {
            @this.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            @this.AddScoped(typeof(IDomainEventBrokerService), typeof(DomainEventBrokerService));
            @this.AddSingleton<ICachingProvider, CachingProviderStub>();

            var options = new VivarniInfrastructureOptionsBuilder();
            optionsBuilder.Invoke(options);
            @this.Add(new ServiceDescriptor(typeof(ICachingProvider), options.CachingProviderType, options.CachingProviderServiceLifetime));

            return @this;
        }
    }
}
