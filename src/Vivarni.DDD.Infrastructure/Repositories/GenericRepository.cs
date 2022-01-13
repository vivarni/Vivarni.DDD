using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vivarni.DDD.Core;
using Vivarni.DDD.Core.Repositories;
using Vivarni.DDD.Core.SpecificationExtensions;
using Vivarni.DDD.Infrastructure.Caching;

namespace Vivarni.DDD.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IAggregateRoot
    {
        internal readonly DbContext _ctx;

        private const string LOG_MSG_DFLT = "{GenericQueryTypeMethod} execution for {GenericQueryType} took {GenericQueryMilliseconds} ms";
        private const string LOG_MSG_SPEC = "{GenericQueryTypeMethod} execution for {GenericQueryType} took {GenericQueryMilliseconds} ms using specification {GenericQuerySpecification}";

        /// <summary>
        /// We use the standard Microsoft logger in order to be portable between solutions which
        /// may use other logging frameworks than Serilog. All logs are routed to serilog anyways,
        /// so no worries there ;-)
        /// </summary>
        private readonly ILogger<GenericRepository<T>> _logger;
        private readonly ICachingProvider _cacheProvider;

        public GenericRepository(DbContext ctx, ILogger<GenericRepository<T>> logger, ICachingProvider cacheProvider)
        {
            _ctx = ctx;
            _logger = logger;
            _cacheProvider = cacheProvider;
        }

        public virtual async Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var result = await _ctx.Set<T>().FindAsync(new object[] { id }, cancellationToken);
            _logger.LogInformation(LOG_MSG_DFLT + "Id={GenericQueryId}", nameof(GetByIdAsync), typeof(T), sw.ElapsedMilliseconds, id);

            return result;
        }

        public async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            var result = await _ctx.Set<T>().ToListAsync(cancellationToken);
            _logger.LogInformation(LOG_MSG_DFLT, nameof(ListAsync), typeof(T), sw.ElapsedMilliseconds);

            return result;
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            IReadOnlyList<T> result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                result = await _cacheProvider.GetAsync(spec.CacheKey, async () => await specificationResult.ToListAsync(cancellationToken), ttl);
            }
            else
            {
                result = await specificationResult.ToListAsync(cancellationToken);
            }

            _logger.LogInformation(LOG_MSG_SPEC, nameof(ListAsync), typeof(T), sw.ElapsedMilliseconds, SpecToString(spec));
            return result;
        }

        public IEnumerable<T> Enumerate(ISpecification<T> spec)
        {
            if (spec.CacheEnabled)
                throw new Exception($"GenericRepository method {nameof(Enumerate)} does not support cached specifications.");

            var specificationResult = ApplySpecification(spec);
            return specificationResult.AsEnumerable<T>();
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            int result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                result = await _cacheProvider.GetAsync(spec.CacheKey, async () => await specificationResult.CountAsync(cancellationToken), ttl);
            }
            else
            {
                result = await specificationResult.CountAsync(cancellationToken);
            }

            _logger.LogInformation(LOG_MSG_SPEC, nameof(CountAsync), typeof(T), sw.ElapsedMilliseconds, SpecToString(spec));
            return result;
        }

        public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _ctx.Set<T>().AddAsync(entity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task<T> FirstAsync<Spec>(Spec spec, CancellationToken cancellationToken = default)
            where Spec : ISpecification<T>, ISingleResultSpecification
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            T result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                result = await _cacheProvider.GetAsync(spec.CacheKey, async () => await specificationResult.FirstAsync(cancellationToken), ttl);
            }
            else
            {
                result = await specificationResult.FirstAsync(cancellationToken);
            }

            _logger.LogInformation(LOG_MSG_SPEC, nameof(FirstAsync), typeof(T), sw.ElapsedMilliseconds, SpecToString(spec));
            return result;
        }

        public async Task<T> FirstOrDefaultAsync<Spec>(Spec spec, CancellationToken cancellationToken = default)
            where Spec : ISpecification<T>, ISingleResultSpecification
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            T result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                result = await _cacheProvider.GetAsync(spec.CacheKey, async () => await specificationResult.FirstOrDefaultAsync(cancellationToken), ttl);
            }
            else
            {
                result = await specificationResult.FirstOrDefaultAsync(cancellationToken);
            }

            _logger.LogInformation(LOG_MSG_SPEC, nameof(FirstOrDefaultAsync), typeof(T), sw.ElapsedMilliseconds, SpecToString(spec));
            return result;
        }

        public async Task<T> SingleAsync<Spec>(Spec spec, CancellationToken cancellationToken = default)
            where Spec : ISpecification<T>, ISingleResultSpecification
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            T result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                result = await _cacheProvider.GetAsync(spec.CacheKey, async () => await specificationResult.SingleAsync(cancellationToken), ttl);
            }
            else
            {
                result = await specificationResult.SingleAsync(cancellationToken);
            }

            _logger.LogInformation(LOG_MSG_SPEC, nameof(SingleAsync), typeof(T), sw.ElapsedMilliseconds, SpecToString(spec));
            return result;
        }

        public virtual async Task<T> SingleOrDefaultAsync<Spec>(Spec spec, CancellationToken cancellationToken = default)
            where Spec : ISpecification<T>, ISingleResultSpecification
        {
            var specificationResult = ApplySpecification(spec);
            var sw = Stopwatch.StartNew();
            T result;

            if (spec.CacheEnabled)
            {
                var ttl = spec.GetCacheTTL();
                result = await _cacheProvider.GetAsync(spec.CacheKey, async () => await specificationResult.SingleOrDefaultAsync(cancellationToken), ttl);
            }
            else
            {
                result = await specificationResult.SingleOrDefaultAsync(cancellationToken);
            }

            _logger.LogInformation(LOG_MSG_SPEC, nameof(SingleOrDefaultAsync), typeof(T), sw.ElapsedMilliseconds, SpecToString(spec));
            return result;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var evaluator = SpecificationEvaluator.Default;
            return evaluator.GetQuery(_ctx.Set<T>().AsQueryable(), spec);
        }

        private string SpecToString(ISpecification<T> spec)
        {
            // TODO : Add more usefull information..
            return spec.GetType().ToString();
        }
    }
}
