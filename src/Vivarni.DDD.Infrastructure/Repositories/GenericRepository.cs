using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vivarni.DDD.Core;
using Vivarni.DDD.Core.Repositories;
using Vivarni.DDD.Core.SpecificationExtensions;
using Vivarni.DDD.Infrastructure.Caching;

namespace Vivarni.DDD.Infrastructure.Repositories
{
    /// <inheritdoc cref="IGenericRepository{T}"/>
    /// <summary>
    /// We use the standard Microsoft logger in order to be portable between solutions which
    /// may use other logging frameworks than Serilog. All logs are routed to serilog anyways,
    /// so no worries there ;-)
    /// </summary>
    public class GenericRepository<T>(ILogger<GenericRepository<T>> logger, ICachingProvider cacheProvider, DbContext ctx) : IGenericRepository<T>
        where T : class, IAggregateRoot
    {
        private readonly DbContext ctx;

        private const string LOG_MSG_DFLT =
            "{GenericRepositoryMethod} execution for {GenericRepositoryType} took {GenericRepositoryMilliseconds} ms";

        private void Log(ISpecification<T> spec, string methodName, long milliseconds, bool? cacheHit)
        {
            var specName = spec.GetType().ToString();
            var messageTemplate =
                "{GenericRepositoryMethod} execution for {GenericRepositoryType} took {GenericRepositoryMilliseconds} ms using specification {GenericRepositorySpecification} " +
                "with CacheEnabled {GenericRepositoryCacheEnabled} and CacheHit {GenericRepositoryCacheHit}";

            logger.LogInformation(messageTemplate, methodName, typeof(T), milliseconds, specName, spec.CacheEnabled, cacheHit);
        }

        /// <inheritdoc/>
        public virtual async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
            where TId : notnull
        {
            var sw = Stopwatch.StartNew();
            var result = await ctx.Set<T>().FindAsync(new object[] { id }, cancellationToken);
            logger.LogInformation(LOG_MSG_DFLT + "Id={GenericQueryId}", nameof(GetByIdAsync), typeof(T), sw.ElapsedMilliseconds, id);

            return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var result = await ctx.Set<T>().ToListAsync(cancellationToken);
            logger.LogInformation(LOG_MSG_DFLT, nameof(ListAsync), typeof(T), sw.ElapsedMilliseconds);

            return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            bool? cacheHit = null;
            IReadOnlyList<T> result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                var forceRefresh = spec.HasForcedCacheRefreshFlag();
                cacheHit = true;
                result = await cacheProvider.GetAsync(spec.CacheKey!, async () =>
                {
                    cacheHit = false;
                    return await specificationResult.ToListAsync(cancellationToken);
                }, ttl, forceRefresh);
            }
            else
            {
                result = await specificationResult.ToListAsync(cancellationToken);
            }

            Log(spec, nameof(ListAsync), sw.ElapsedMilliseconds, cacheHit);
            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Enumerate(ISpecification<T> spec)
        {
            if (spec.CacheEnabled)
                throw new Exception($"GenericRepository method {nameof(Enumerate)} does not support cached specifications.");

            var specificationResult = ApplySpecification(spec);
            return specificationResult.AsEnumerable();
        }

        /// <inheritdoc/>
        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            bool? cacheHit = null;
            int result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                var forceRefresh = spec.HasForcedCacheRefreshFlag();
                cacheHit = true;
                result = await cacheProvider.GetAsync(spec.CacheKey!, async () =>
                {
                    cacheHit = false;
                    return await specificationResult.CountAsync(cancellationToken);
                }, ttl, forceRefresh);
            }
            else
            {
                result = await specificationResult.CountAsync(cancellationToken);
            }

            Log(spec, nameof(CountAsync), sw.ElapsedMilliseconds, cacheHit);
            return result;
        }

        /// <inheritdoc/>
        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            await ctx.Set<T>().AddAsync(entity, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
            logger.LogInformation(LOG_MSG_DFLT, nameof(AddAsync), typeof(T), sw.ElapsedMilliseconds);

            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            await ctx.SaveChangesAsync(cancellationToken);
            logger.LogInformation(LOG_MSG_DFLT, nameof(UpdateAsync), typeof(T), sw.ElapsedMilliseconds);
        }

        /// <inheritdoc/>
        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            ctx.Set<T>().Remove(entity);
            await ctx.SaveChangesAsync(cancellationToken);
            logger.LogInformation(LOG_MSG_DFLT, nameof(DeleteAsync), typeof(T), sw.ElapsedMilliseconds);
        }

        /// <inheritdoc/>
        public async Task<T> FirstAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            bool? cacheHit = null;
            T result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                var forceRefresh = spec.HasForcedCacheRefreshFlag();
                cacheHit = true;
                result = await cacheProvider.GetAsync(spec.CacheKey!, async () =>
                {
                    cacheHit = false;
                    return await specificationResult.FirstAsync(cancellationToken);
                }, ttl, forceRefresh);
            }
            else
            {
                result = await specificationResult.FirstAsync(cancellationToken);
            }

            Log(spec, nameof(FirstAsync), sw.ElapsedMilliseconds, cacheHit);
            return result;
        }

        /// <inheritdoc/>
        public async Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            bool? cacheHit = null;
            T? result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                var forceRefresh = spec.HasForcedCacheRefreshFlag();
                cacheHit = true;
                result = await cacheProvider.GetAsync(spec.CacheKey!, async () =>
                {
                    cacheHit |= false;
                    return await specificationResult.FirstOrDefaultAsync(cancellationToken);
                }, ttl, forceRefresh);
            }
            else
            {
                result = await specificationResult.FirstOrDefaultAsync(cancellationToken);
            }

            Log(spec, nameof(FirstOrDefaultAsync), sw.ElapsedMilliseconds, cacheHit);
            return result;
        }

        /// <inheritdoc/>
        public async Task<T> SingleAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            bool? cacheHit = null;
            T result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                var forceRefresh = spec.HasForcedCacheRefreshFlag();
                cacheHit = true;
                result = await cacheProvider.GetAsync(spec.CacheKey!, async () =>
                {
                    cacheHit = false;
                    return await specificationResult.SingleAsync(cancellationToken);
                }, ttl, forceRefresh);
            }
            else
            {
                result = await specificationResult.SingleAsync(cancellationToken);
            }

            Log(spec, nameof(SingleAsync), sw.ElapsedMilliseconds, cacheHit);
            return result;
        }

        /// <inheritdoc/>
        public virtual async Task<T?> SingleOrDefaultAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            bool? cacheHit = null;
            T? result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                var forceRefresh = spec.HasForcedCacheRefreshFlag();
                cacheHit = true;
                result = await cacheProvider.GetAsync(spec.CacheKey!, async () =>
                {
                    cacheHit = false;
                    return await specificationResult.SingleOrDefaultAsync(cancellationToken);
                }, ttl, forceRefresh);
            }
            else
            {
                result = await specificationResult.SingleOrDefaultAsync(cancellationToken);
            }

            Log(spec, nameof(SingleOrDefaultAsync), sw.ElapsedMilliseconds, cacheHit);
            return result;
        }

        /// <inheritdoc/>
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var evaluator = SpecificationEvaluator.Default;
            return evaluator.GetQuery(ctx.Set<T>().AsQueryable(), spec);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            await ctx.Set<T>().AddRangeAsync(entities, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
            logger.LogInformation(LOG_MSG_DFLT, nameof(AddRangeAsync), typeof(T), sw.ElapsedMilliseconds);

            return entities;
        }

        /// <inheritdoc/>
        public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            await ctx.SaveChangesAsync(cancellationToken);
            logger.LogInformation(LOG_MSG_DFLT, nameof(UpdateRangeAsync), typeof(T), sw.ElapsedMilliseconds);
        }

        /// <inheritdoc/>
        public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            ctx.Set<T>().RemoveRange(entities);
            await ctx.SaveChangesAsync(cancellationToken);
            logger.LogInformation(LOG_MSG_DFLT, nameof(DeleteRangeAsync), typeof(T), sw.ElapsedMilliseconds);
        }
    }
}
