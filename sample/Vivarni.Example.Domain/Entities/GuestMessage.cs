using System.ComponentModel.DataAnnotations;
using Ardalis.Specification;
using Vivarni.DDD.Core;
using Vivarni.DDD.Core.Repositories;

namespace Vivarni.Example.Domain.Entities;

public class GuestMessage : BaseEntity, IAggregateRoot
{
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;
    public GuestMessage()
    {

    }

    public GuestMessage(string message, string author)
    {
        Message = message;
        CreatedBy = author;
        LastModifiedBy = author;
    }

}
public class GuestMessageCreatedEvent : IDomainEvent
{
    public GuestMessageCreatedEvent(GuestMessage guestMessage)
    {
        GuestMessage = guestMessage;
    }

    public GuestMessage GuestMessage { get; }
}
public class GuestMessageCreatedEventHandler : IDomainEventHandler<GuestMessageCreatedEvent>
{
    private readonly IGenericRepository<GuestMessagesCounter> _guestMessagesCounterrepository;

    public GuestMessageCreatedEventHandler(IGenericRepository<GuestMessagesCounter> guestMessagesCounterrepository)
    {
        _guestMessagesCounterrepository = guestMessagesCounterrepository;
    }
    public async Task HandleAsync(GuestMessageCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        var counts = await _guestMessagesCounterrepository.ListAsync(cancellationToken);
        int count = counts.Max(x => x.Count) + 1;
        await _guestMessagesCounterrepository.AddAsync(new GuestMessagesCounter(count, domainEvent.GuestMessage.CreatedBy));
    }
}
