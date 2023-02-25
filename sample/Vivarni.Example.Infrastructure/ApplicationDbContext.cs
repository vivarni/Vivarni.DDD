using Microsoft.EntityFrameworkCore;
using Vivarni.DDD.Core;
using Vivarni.DDD.Infrastructure.DomainEvents;
using Vivarni.Example.Domain.Entities;

namespace Vivarni.Example.Infrastructure.SQLite;

public class ApplicationDbContext : DbContext
{
    public DbSet<GuestMessage> GuestMessages { get; set; }

    private readonly IDomainEventBrokerService _domainEventBroker;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventBrokerService domainEventBrokerService)
        : base(options)
    {
        _domainEventBroker = domainEventBrokerService;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreationDate = DateTime.Now;
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    break;
            }
        }
        var events = PopUnpublishedDomainEvents();

        // In order to keep the database consistent, we want any additional changes introduced
        // by our event handlers to be kept within the same transaction.
        var isTransactionOwned = false;
        var tx = Database.CurrentTransaction;
        if (events.Any() && tx == null)
        {
            tx = await Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
            isTransactionOwned = true;
        }

        // Save original changes to the database
        var result = await base.SaveChangesAsync(cancellationToken);

        // Publish events only if save was successful. This might trigger additional
        // database changes which are executed within the same transaction because as
        // the original database changes.
        await _domainEventBroker.PublishEventsAsync(events, cancellationToken);

        // The transaction might have been created by ourselves. If that's the case,
        // we need to commit the transaction ourself.
        if (isTransactionOwned)
        {
            await tx.CommitAsync(cancellationToken);
        }

        return result;
    }

    private IDomainEvent[] PopUnpublishedDomainEvents()
    {
        var entitiesWithEvents = ChangeTracker.Entries<IEntityWithDomainEvents>()
            .Select(e => e.Entity)
            .Where(e => e.Events.Any())
            .ToArray();

        var events = new List<IDomainEvent>();
        foreach (var entity in entitiesWithEvents)
        {
            events.AddRange(entity.Events.ToArray());
            entity.Events.Clear();
        }

        return events.ToArray();
    }
}
