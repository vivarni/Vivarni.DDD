using System.Threading;
using System.Threading.Tasks;

namespace Vivarni.DDD.Core;

/// <summary>
/// Handler for executing domain events applied to an aggregate root or other domain entity instance.
/// </summary>
public interface IDomainEventHandler { }

/// <inheritdoc cref="IDomainEventHandler"/>
public interface IDomainEventHandler<in TEvent> : IDomainEventHandler
    where TEvent : IDomainEvent
{
    /// <summary>
    /// Asynchronously handles the <paramref name="domainEvent"/>.
    /// </summary>
    /// <param name="domainEvent">Domain event to be handled.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
}
