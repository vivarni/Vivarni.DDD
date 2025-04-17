using System.ComponentModel.DataAnnotations;
using Vivarni.DDD.Core;

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
