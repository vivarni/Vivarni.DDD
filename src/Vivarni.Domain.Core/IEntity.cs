namespace Vivarni.Domain.Core
{
    /// <summary>
    /// An entity which is mapped to the database.
    /// </summary>
    public interface IEntity { }

    /// <summary>
    /// In support for database tables that don't use <see cref="long"/> as primary key data type.
    /// </summary>
    /// <typeparam name="TId">The type used as primary key.</typeparam>
    public interface IEntity<TId> : IEntity
    {
        /// <summary>
        /// Entity Identity. Most probably mapped to the primary key in the database.
        /// </summary>
        TId Id { get; }
    }
}
