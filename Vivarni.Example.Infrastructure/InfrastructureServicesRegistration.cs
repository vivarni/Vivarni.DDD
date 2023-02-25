using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Core;
using Vivarni.DDD.Core.Repositories;
using Vivarni.DDD.Infrastructure;
using Vivarni.DDD.Infrastructure.DomainEvents;

namespace Vivarni.Example.Infrastructure.SQLite;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        services.AddDbContext<DbContext, ApplicationDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        services.AddVivarniInfrastructure();
        services.AddScoped<IDomainEventBrokerService, DomainEventBrokerService>();

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
