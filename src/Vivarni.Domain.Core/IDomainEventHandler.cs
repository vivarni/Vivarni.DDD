using System.Threading;
using System.Threading.Tasks;

namespace Vivarni.Domain.Core
{
    /// <summary>
    /// Handler for executing domain events applied to an aggregate root or other domain entity instance.
    /// </summary>
    public interface IDomainEventHandler { }

    /// <inheritdoc cref="IDomainEventHandler"/>
    public interface IDomainEventHandler<in TEvent> : IDomainEventHandler
        where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
    }
}
