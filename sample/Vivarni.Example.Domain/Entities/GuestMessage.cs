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

    public GuestMessage(string message, string author)
    {
        Message = message;
        CreatedBy = author;
        LastModifiedBy = author;
    }
}
