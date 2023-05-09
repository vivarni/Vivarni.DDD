using Vivarni.DDD.Core;
using Vivarni.DDD.Core.Repositories;
using Vivarni.Example.Domain.Entities;

namespace Vivarni.Example.API.EventHandlers;

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
        if (counts.Count == 0)
        {
            wm = new GuestMessagesCounter() { Count = 1, LastEntryBy = domainEvent.GuestMessage.CreatedBy };
            await _guestMessagesCounterrepository.AddAsync(wm);
            return;
        }
        int count = counts.Max(x => x.Count) + 1;

        wm.Count = count;
        wm.LastEntryBy = domainEvent.GuestMessage.CreatedBy;

        await _guestMessagesCounterrepository.AddAsync(wm);
    }
}
