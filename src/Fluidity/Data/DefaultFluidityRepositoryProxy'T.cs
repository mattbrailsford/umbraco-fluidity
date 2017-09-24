using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Core.Models;

namespace Fluidity.Data
{
    public class DefaultFluidityRepositoryProxy<TEntity, TId> : FluidityRepository<TEntity, TId>
    {
        protected DefaultFluidityRepository _repository;

        internal DefaultFluidityRepositoryProxy(DefaultFluidityRepository repository)
        {
            if (repository.EntityType != typeof(TEntity))
                throw new ArgumentException($"The repository passed in to the proxy is not for the same entity type as that defined in the procies generic params. Proxies entity type is {typeof(TEntity)}, where as the passed in repositories entity type is {repository.EntityType}", nameof(repository));

            _repository = repository;
        }

        // We are going to proxy the methods directly so just leave the impl methods
        // unimplemented as we'll override the wrapping public methods in a second instead

        protected override TId GetIdImpl(TEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override TEntity GetImpl(TId id)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<TEntity> GetAllImpl()
        {
            throw new NotImplementedException();
        }

        protected override PagedResult<TEntity> GetPagedImpl(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> whereClause, Expression<Func<TEntity, object>> orderBy, SortDirection orderDirection)
        {
            throw new NotImplementedException();
        }

        protected override TEntity SaveImpl(TEntity entity)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteImpl(TId id)
        {
            throw new NotImplementedException();
        }

        protected override long GetTotalRecordCountImpl()
        {
            throw new NotImplementedException();
        }

        // We override all public accessors to ensure we only
        // fire events on the proxies respository as we don't
        // want to fire two sets of events

        public override TEntity Get(TId id, bool fireEvents = true)
        {
            return (TEntity)_repository.Get(id, fireEvents);
        }

        public override IEnumerable<TEntity> GetAll(bool fireEvents = true)
        {
            return _repository.GetAll(fireEvents).Cast<TEntity>().ToList();
        }

        public override PagedResult<TEntity> GetPaged(int pageNumber = 1, int pageSize = 10, Expression<Func<TEntity, bool>> whereClause = null, Expression<Func<TEntity, object>> orderBy = null, SortDirection orderDirection = SortDirection.Ascending, bool fireEvents = true)
        {
            // Perform get
            var results = _repository.GetPaged(pageNumber, pageSize, whereClause, orderBy, orderDirection, fireEvents);

            // Re-cast results
            return new PagedResult<TEntity>(results.TotalItems, results.PageNumber, results.PageSize)
            {
                Items = results.Items.Cast<TEntity>().ToList()
            };
        }

        public override TEntity Save(TEntity entity, bool fireEvents = true)
        {
            return (TEntity)_repository.Save(entity, fireEvents);
        }

        public override void Delete(TId id, bool fireEvents = true)
        {
            _repository.Delete(id, fireEvents);
        }

        public override long GetTotalRecordCount(bool fireEvents = true)
        {
            return _repository.GetTotalRecordCount(fireEvents);
        }
    }
}
