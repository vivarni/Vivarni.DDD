using System;
using System.Collections.Generic;
using System.Text;

namespace Vivarni.Domain.Core
{
    /// <summary>
    /// Preferred base class for entities that are mapped to the database.
    /// </summary>
    public abstract class BaseEntity : IEntity<long>
    {
        /// <inheritdoc/>
        public virtual long Id { get; set; }
    }
}
