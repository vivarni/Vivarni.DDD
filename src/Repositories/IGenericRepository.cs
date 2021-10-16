using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Vivarni.Domain.Core.Repositories
{
    public interface IGenericRepository<T> where T : IAggregateRoot
    {
        /// <summary>
        /// Finds an entity with the given primary key value.
        /// </summary>
        /// <typeparam name="TId">The type of primary key.</typeparam>
        /// <param name="id">The value of the primary key for the entity to be found.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="T" />, or <see langword="null"/>.
        /// </returns>
        Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        IEnumerable<T> Enumerate(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        Task<T> FirstAsync<Spec>(Spec spec, CancellationToken cancellationToken = default) where Spec : ISingleResultSpecification, ISpecification<T>;
        Task<T> FirstOrDefaultAsync<Spec>(Spec spec, CancellationToken cancellationToken = default) where Spec : ISingleResultSpecification, ISpecification<T>;

        Task<T> SingleAsync<Spec>(Spec spec, CancellationToken cancellationToken = default) where Spec : ISingleResultSpecification, ISpecification<T>;
        Task<T> SingleOrDefaultAsync<Spec>(Spec spec, CancellationToken cancellationToken = default) where Spec : ISingleResultSpecification, ISpecification<T>;

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
