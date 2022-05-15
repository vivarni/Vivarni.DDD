using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vivarni.DDD.Core;
using Vivarni.DDD.Infrastructure.DomainEvents;

namespace Vivarni.DDD.Infrastructure
{
    public abstract class VivarniContext : DbContext
    {
        private readonly IDomainEventBrokerService _domainEventBroker;

        public VivarniContext(IDomainEventBrokerService domainEventBroker)
        {
            _domainEventBroker = domainEventBroker;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
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
}
