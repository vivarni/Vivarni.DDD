using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Core.Repositories;
using Vivarni.DDD.Infrastructure.DomainEvents;

namespace Vivarni.DDD.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFVivarniInfrastructure(this IServiceCollection @this, IConfiguration configuration)
        {
            @this.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            @this.AddScoped(typeof(IDomainEventBrokerService), typeof(DomainEventBrokerService));

            return @this;
        }
    }
}
