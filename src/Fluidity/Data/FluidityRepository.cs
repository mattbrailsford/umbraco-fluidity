using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;

namespace Fluidity.Data
{
    public abstract class FluidityRepository<TEntity, TId> : IFluidityRepository
    {
        public abstract TEntity Get(TId id);

        public abstract IEnumerable<TEntity> GetAll();

        public abstract PagedResult<TEntity> GetPaged(int pageNumber = 1, int pageSize = 10, Expression<Func<TEntity, object>> orderBy = null, Direction orderDirection = Direction.Ascending, Expression<Func<TEntity, bool>> whereClause = null);

        public abstract TEntity Save(TEntity entity);

        public abstract void Delete(TId id);

        public abstract long GetTotalRecordCount();

        #region IFluidityRepository

        object IFluidityRepository.Get(object id)
        {
            return Get((TId)TypeDescriptor.GetConverter(typeof(TId)).ConvertFrom(id));
        }

        IEnumerable<object> IFluidityRepository.GetAll()
        {
            return GetAll().Select(x => (object)x);
        }

        PagedResult<object> IFluidityRepository.GetPaged(int pageNumber, int pageSize, LambdaExpression orderBy, Direction orderDirection, LambdaExpression whereClause)
        {
            var result = GetPaged(pageNumber, pageSize, (Expression<Func<TEntity, object>>)orderBy, orderDirection, (Expression<Func<TEntity, bool>>)whereClause);

            return new PagedResult<object>(result.TotalItems, result.PageNumber, result.PageSize)
            {
                Items = result.Items.Select(x => (object)x)
            };
        }

        object IFluidityRepository.Save(object entity)
        {
            return Save((TEntity)entity);
        }

        void IFluidityRepository.Delete(object id)
        {
            Delete((TId)TypeDescriptor.GetConverter(typeof(TId)).ConvertFrom(id));
        }

        #endregion
    }
}
