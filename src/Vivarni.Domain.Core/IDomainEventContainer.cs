using System.Collections.Generic;
using Vivarni.Domain.Core;

namespace Vivarni.Domain.Core
{
    /// <summary>
    /// An entity tracked by entity framework which is capable of carrying domain events. Our
    /// architecture provides a mechanism to trigger domain events during the SaveChanges method
    /// of the database context, which is why this interface can only be applied to EF entities
    /// that are effectively saved to the database.
    /// </summary>
    public interface IDomainEventContainer : IEntity
    {
        /// <summary>
        /// List of events carried by this <see cref="IDomainEventContainer"/>. This property should
        /// be ignored by Entity Framework.
        /// </summary>
        public List<IDomainEvent> Events { get; }
    }
}
