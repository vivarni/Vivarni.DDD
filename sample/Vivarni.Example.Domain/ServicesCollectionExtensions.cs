using System.Reflection;
using Vivarni.DDD.Core;
using Microsoft.Extensions.DependencyInjection;
namespace Vivarni.Example.Domain
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
             var executingAssembly = Assembly.GetExecutingAssembly();
    
            services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
        }
    }
}

