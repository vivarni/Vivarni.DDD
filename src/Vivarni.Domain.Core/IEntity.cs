namespace Vivarni.Domain.Core
{
    /// <summary>
    /// An entity which is mapped to the databasse via Entity Framework.
    /// </summary>
    public interface IEntity { }

    /// <summary>
    /// In support for database tables which don't use bigint as primary key data type. Direct usage
    /// of this interface should be avoided; <see cref="BaseEntity"/> should be favored in stead.
    /// We allow different primary key data types untill we have a more stable ground to refactor
    /// even further.
    /// </summary>
    /// <typeparam name="TId">The type used as primary key.</typeparam>
    public interface IEntity<TId> : IEntity
    {
        TId Id { get; }
    }
}
