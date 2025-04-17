using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Core;

namespace Vivarni.Example.Domain;

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

