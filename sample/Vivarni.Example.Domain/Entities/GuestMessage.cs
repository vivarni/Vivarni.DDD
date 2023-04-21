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

    public static GuestMessage GuestMessageCreate(string message, string author)
    {
        var result = new GuestMessage()
            { 
                Message = message, 
                CreatedBy = author, 
                LastModifiedBy = author 
            };
        result.Events.Add(new GuestMessageCreatedEvent(result));
        return result;
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
        GuestMessagesCounter wm = new GuestMessagesCounter();
        var counts = await _guestMessagesCounterrepository.ListAsync(cancellationToken);
        if(counts.Count == 0)
        {
            wm = new GuestMessagesCounter(){ Count = 1, LastEntryBy = domainEvent.GuestMessage.CreatedBy};
            await _guestMessagesCounterrepository.AddAsync(wm);
            return;
        }
        int count = counts.Max(x => x.Count) + 1;
        
        wm.Count = count;
        wm.LastEntryBy = domainEvent.GuestMessage.CreatedBy;
        
        await _guestMessagesCounterrepository.AddAsync(wm);
    }
}
