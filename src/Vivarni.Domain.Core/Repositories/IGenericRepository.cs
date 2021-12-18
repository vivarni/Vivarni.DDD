﻿using System;
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

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}
