using System.ComponentModel.DataAnnotations;
using Vivarni.DDD.Core;
using Vivarni.Example.Shared.Shared.Models;

namespace Vivarni.Example.Domain.Entities;

public class GuestMessage: BaseEntity, IAggregateRoot
{
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;
    public GuestMessage()
    {

    }
    public GuestMessage(GuestMessageCreateDTO record)
    {
        Message = record.GuestMessage;
        CreatedBy = record.CreatedByUser;
        LastModifiedBy = record.CreatedByUser;
    }
}
