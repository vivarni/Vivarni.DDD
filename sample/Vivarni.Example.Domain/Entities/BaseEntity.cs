﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vivarni.DDD.Core;

namespace Vivarni.Example.Domain.Entities;

public abstract class BaseEntity : IEntityWithDomainEvents
{
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string CreatedBy { get; set; } = string.Empty;
    [MaxLength(50)]
    public string LastModifiedBy { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public DateTime LastModifiedDate { get; set; }

    [NotMapped]
    public List<IDomainEvent> Events { get; set; } = new();
}
