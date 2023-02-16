using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Infrastructure;
using Vivarni.DDD.Infrastructure.DomainEvents;
using Vivarni.DDD.Core.Repositories;
using Vivarni.DDD.Core;

namespace Vivarni.Example.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDomainEventBrokerService, DomainEventBrokerService>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        var executingAssembly = Assembly.GetExecutingAssembly(); services.AddDbContext<DbContext, ApplicationDbContext>();
        services.AddVivarniInfrastructure();
        services.Scan(scan => scan
        .FromAssemblies(executingAssembly)
        .AddClasses(classes => classes.AssignableTo(typeof(IGenericRepository<>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());
        services.Scan(scan => scan
        .FromAssemblies(Assembly.GetExecutingAssembly())
        .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());
        services.Scan(scan => scan
        .FromAssemblies(executingAssembly)
        .AddClasses(b => b.Where(type => type.Name.EndsWith("Repository")))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        return services;
    }
}
