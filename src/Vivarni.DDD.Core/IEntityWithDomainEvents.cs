﻿using System;
using System.Collections.Generic;

namespace Vivarni.DDD.Core;

/// <summary>
/// An entity which is capable of carrying domain events.
/// </summary>
public interface IEntityWithDomainEvents
{
    /// <summary>
    /// List of events carried by this entity.
    /// This property should be ignored by Entity Framework.
    /// </summary>
    public List<IDomainEvent> Events { get; }
}

/// <summary>
/// An entity which is capable of carrying domain events. The infrastructure layer should provide a
/// mechanism to trigger domain events after the changes have been saved to the database, preferably
/// within a database transaction because the domain event handlers may also apply changes to the
/// database. Such a mechanism is provided in <c>Vivarni.Domain.Infrastructure</c>.
/// </summary>
/// <seealso href="https://github.com/vivarni/vivarni.domain"/>
[Obsolete("Replace the usage of this generic interface with the non-generic variant. Will be removed in future releases.")]
public interface IEntityWithDomainEvents<TId> : IEntityWithDomainEvents { }
