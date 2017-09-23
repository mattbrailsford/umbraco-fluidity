using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Core.Models;

namespace Fluidity.Data
{
    public class FluidityRepositoryProxy<TEntity, TId>
    {
        protected IFluidityRepository _repository;

        internal FluidityRepositoryProxy(IFluidityRepository repository)
        {
            if (repository.EntityType != typeof(TEntity))
                throw new ArgumentException($"The repository passed in to the proxy is not for the same entity type as that defined in the procies generic params. Proxies entity type is {typeof(TEntity)}, where as the passed in repositories entity type is {repository.EntityType}", nameof(repository));

            _repository = repository;
        }

        public virtual TEntity Get(TId id)
        {
            return (TEntity)_repository.Get(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _repository.GetAll().Cast<TEntity>().ToList();
        }

        public virtual PagedResult<TEntity> GetPaged(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> whereClause, Expression<Func<TEntity, object>> orderBy, SortDirection orderDirection)
        {
            var results = _repository.GetPaged(pageNumber, pageSize, whereClause, orderBy, orderDirection);

            return new PagedResult<TEntity>(results.TotalItems, results.PageNumber, results.PageSize)
            {
                Items = results.Items.Cast<TEntity>().ToList()
            };
        }

        public virtual TEntity Save(TEntity entity)
        {
            return (TEntity)_repository.Save(entity);
        }

        public virtual void Delete(TId id)
        {
            _repository.Delete(id);
        }

        public virtual long GetTotalRecordCount()
        {
            return _repository.GetTotalRecordCount();
        }
    }
}
