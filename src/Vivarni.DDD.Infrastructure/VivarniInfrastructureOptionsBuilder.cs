using System;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Infrastructure.Caching;

namespace Vivarni.DDD.Infrastructure
{
    public class VivarniInfrastructureOptionsBuilder
    {
        public Type CachingProviderType { internal get; set; } = typeof(CachingProviderStub);
        internal ServiceLifetime CachingProviderServiceLifetime { get; set; }

        public void AddCachingProvider<T>(ServiceLifetime lifeTime = ServiceLifetime.Scoped)
            where T : ICachingProvider
        {
            CachingProviderType = typeof(T);
            CachingProviderServiceLifetime = lifeTime;
        }
    }
}
