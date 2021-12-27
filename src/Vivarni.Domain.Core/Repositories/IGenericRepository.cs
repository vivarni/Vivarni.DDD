using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace Vivarni.Domain.Core.Repositories
{
    /// <summary>
    /// <para>
    /// A <see cref="IGenericRepository{T}" /> can be used to query and save instances of <typeparamref name="T" />.
    /// An <see cref="ISpecification{T}"/> (or derived) is used to encapsulate the LINQ queries against the database.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of entity being operated on by this repository.</typeparam>
    public interface IGenericRepository<T> where T : IAggregateRoot
    {
        /// <summary>
        /// Finds an entity with the given primary key value.
        /// </summary>
        /// <typeparam name="TId">The type of primary key.</typeparam>
        /// <param name="id">The value of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="T" />, or <see langword="null"/>.
        /// </returns>
        Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds all entities of <typeparamref name="T" /> from the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> which allows to stream entities from the
        /// database. Only makes sence when the encapsulated <paramref name="specification"/>
        /// is configured not to track its entities.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        IEnumerable<T> Enumerate(ISpecification<T> specification);

        /// <summary>
        /// Returns a number that represents how many entities satisfy the encapsulated query logic
        /// of the <paramref name="specification"/>.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the
        /// number of elements in the input sequence.
        /// </returns>
        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns the first entity (of <typeparamref name="T" />) of a sequence that matches the
        /// encapsulated query logic of the <paramref name="specification"/>.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the first
        /// element that matches the encapsulated query logic.
        /// </returns>
        Task<T> FirstAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns the first entity (of <typeparamref name="T" />) of a sequence that matches the
        /// encapsulated query logic of the <paramref name="specification"/>, or a default value if the sequence
        /// contains no elements.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <c>default</c>(<typeparamref name="T"/>) if
        /// the encapsulated query yields no results; otherwise, the first element that matches the encapsulated query logic.
        /// </returns>
        Task<T> FirstOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns the only entity (of <typeparamref name="T" />) of a sequence that matches the
        /// encapsulated query logic of the <paramref name="specification"/>, and throws an exception if there
        /// is not exactly one element in the sequence.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the only
        /// element in source that matches the encapsulated query logic.
        /// </returns>
        /// <exception cref="InvalidOperationException">The encapsulated query logic of <paramref name="specification"/> yields more than one element.</exception>
        Task<T> SingleAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns the only entity (of <typeparamref name="T" />) of a sequence that matches the
        /// encapsulated query logic of the <paramref name="specification"/>, or a default value if the sequence
        /// is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains <c>default</c>(<typeparamref name="T"/>) if
        /// the encapsulated query yields no results; otherwise, the only element that matches the encapsulated query logic.
        /// </returns>
        /// <exception cref="InvalidOperationException">The encapsulated query logic of <paramref name="specification"/> yields more than one element.</exception>
        Task<T> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously adds the provided <paramref name="entity"/> to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the added entity.</returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously updates the provided <paramref name="entity"/> in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously removes the provided <paramref name="entity"/> from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
