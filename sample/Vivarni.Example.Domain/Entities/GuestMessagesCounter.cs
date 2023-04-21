﻿using System.ComponentModel.DataAnnotations;
using Vivarni.DDD.Core;

namespace Vivarni.Example.Domain.Entities;

public class GuestMessagesCounter : BaseEntity, IAggregateRoot
{
    public int Count { get; set; }
    [MaxLength(50)]
    public string LastEntryBy { get; set; }
       
}
