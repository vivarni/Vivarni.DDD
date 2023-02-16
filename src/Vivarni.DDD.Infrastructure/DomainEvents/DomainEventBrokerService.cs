using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Vivarni.DDD.Core;

namespace Vivarni.DDD.Infrastructure.DomainEvents
{
    /// <summary>
    /// A broker that publishes domain events to domain event handlers.
    /// </summary>
    public interface IDomainEventBrokerService
    {
        /// <summary>
        /// Publishes the given domain event asynchronously to all registered event handlers for the event type.
        /// </summary>
        Task PublishEventsAsync(IDomainEvent[] entitiesWithEvents, CancellationToken cancellationToken);
    }

    /// <inheritdoc cref="IDomainEventBrokerService"/>
    public class DomainEventBrokerService : IDomainEventBrokerService
    {
        private readonly Dictionary<Type, IReadOnlyCollection<IDomainEventHandler>> _handlers;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        public DomainEventBrokerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _handlers = new Dictionary<Type, IReadOnlyCollection<IDomainEventHandler>>();
        }

        /// <inheritdoc/>
        public async Task PublishEventsAsync(IDomainEvent[] events, CancellationToken cancellationToken)
        {
            foreach (var domainEvent in events)
            {
                await PublishAsync((dynamic)domainEvent, cancellationToken);
            }
        }

        /// <summary>
        /// Publishes the given domain event asynchronously to all registered event handlers for the event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the domain event.</typeparam>
        /// <param name="domainEvent">The domain event instance to execute.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        private async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken)
            where TEvent : class, IDomainEvent
        {
            var handlers = ResolveEventHandlers<IDomainEventHandler<TEvent>>();

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(domainEvent, cancellationToken);
            }
        }

        /// <summary>
        /// Resolves the set of event handlers registered for handling the given domain event.
        /// </summary>
        /// <typeparam name="TEventHandler">The type of the domain event handler.</typeparam>
        /// <returns>A collection of event handler instances.</returns>
        private IReadOnlyCollection<TEventHandler> ResolveEventHandlers<TEventHandler>()
        {
            if (!_handlers.ContainsKey(typeof(TEventHandler)))
            {
                var handlers = (IReadOnlyCollection<IDomainEventHandler>)_serviceProvider.GetServices<TEventHandler>();
                _handlers[typeof(TEventHandler)] = handlers;
            }

            return (IReadOnlyCollection<TEventHandler>)_handlers[typeof(TEventHandler)];
        }
    }
}
