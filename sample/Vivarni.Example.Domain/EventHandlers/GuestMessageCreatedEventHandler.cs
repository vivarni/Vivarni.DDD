using Vivarni.DDD.Core;
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.Domain.Entities;

namespace Vivarni.Example.Domain.EventHandlers;

/// <summary>
/// Domain event handler to update our guest message counter. The guest message counter keeps
/// track of the total number of guest messages in out application.
/// </summary>
public class GuestMessageCreatedEventHandler : IDomainEventHandler<GuestMessageCreatedEvent>
{
    private readonly IGenericRepository<GuestMessage> _messageRepository;
    private readonly IGenericRepository<GuestMessagesCounter> _counterRepository;

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    public GuestMessageCreatedEventHandler(IGenericRepository<GuestMessagesCounter> guestMessagesCounterrepository, IGenericRepository<GuestMessage> messageRepository)
    {
        _counterRepository = guestMessagesCounterrepository;
        _messageRepository = messageRepository;
    }

    /// <inheritdoc cref="GuestMessageCreatedEventHandler"/>
    public async Task HandleAsync(GuestMessageCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var counters = await _counterRepository.ListAsync(cancellationToken);
        var counter = counters.SingleOrDefault();

        if (counter == null)
        {
            // The counter functionality was added after the guest messages.
            // This if-clause is only executed on the very first time the
            // event is triggerd, and it takes care of setting the data right.

            var allMessages = await _messageRepository.ListAsync(cancellationToken);
            var lastMessage = allMessages.OrderBy(s => s.CreationDate).LastOrDefault();

            counter = new GuestMessagesCounter()
            {
                Count = allMessages.Count,
                LastEntryBy = lastMessage?.CreatedBy ?? "n/a"
            };

            await _counterRepository.AddAsync(counter, cancellationToken);
        }
        else
        {
            counter.Count++;
            counter.LastEntryBy = domainEvent.GuestMessage.CreatedBy;
            await _counterRepository.UpdateAsync(counter, cancellationToken);
        }
    }
}
