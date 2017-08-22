using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Fluidity.Events;
using Fluidity.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;

namespace Fluidity.Data
{
    public abstract class FluidityRepository<TEntity, TId> : IFluidityRepository
    {
        protected abstract TId GetIdImpl(TEntity entity);
        private TId GetId(TEntity entity)
        {
            return GetIdImpl(entity);
        }

        protected abstract TEntity GetImpl(TId id);
        public TEntity Get(TId id, bool fireEvents = true)
        {
            return GetImpl(id);
        }

        protected abstract IEnumerable<TEntity> GetAllImpl();
        public IEnumerable<TEntity> GetAll(bool fireEvents = true)
        {
            return GetAllImpl();
        }

        protected abstract PagedResult<TEntity> GetPagedImpl(int pageNumber, int pageSize, Expression<Func<TEntity, object>> orderBy, Direction orderDirection, Expression<Func<TEntity, bool>> whereClause);
        public PagedResult<TEntity> GetPaged(int pageNumber = 1, int pageSize = 10, Expression<Func<TEntity, object>> orderBy = null, Direction orderDirection = Direction.Ascending, Expression<Func<TEntity, bool>> whereClause = null, bool fireEvents = true)
        {
            return GetPagedImpl(pageNumber, pageSize, orderBy, orderDirection, whereClause);
        }

        protected abstract TEntity SaveImpl(TEntity entity);
        public TEntity Save(TEntity entity, bool fireEvents = true)
        {
            SavingEntityEventArgs args = null;

            if (fireEvents) { 

                var existing = Get(GetId(entity));
                args = new SavingEntityEventArgs
                {
                    Entity = new BeforeAndAfter<object>
                    {
                        Before = existing,
                        After = entity
                    }
                };
                
                Fluidity.OnSavingEntity(args);

                if (args.Cancel)
                    return (TEntity)args.Entity.After;

                entity = (TEntity)args.Entity.After;
            }

            entity = SaveImpl(entity);

            if (fireEvents)
            {
                Fluidity.OnSavedEntity(args);

                entity = (TEntity)args.Entity.After;
            }

            return entity;
        }

        protected abstract void DeleteImpl(TId id);
        public void Delete(TId id, bool fireEvents = true)
        {
            DeletingEntityEventArgs args = null;

            if (fireEvents) { 

                var existing = Get(id);
                args = new DeletingEntityEventArgs
                {
                    Entity = existing
                };

                Fluidity.OnDeletingEntity(args);

                if (args.Cancel)
                    return;
            }

            DeleteImpl(id);

            if (fireEvents)
                Fluidity.OnDeletedEntity(args);
        }

        protected abstract long GetTotalRecordCountImpl();
        public long GetTotalRecordCount(bool fireEvents = true)
        {
            return GetTotalRecordCountImpl();
        }

        #region IFluidityRepository

        object IFluidityRepository.Get(object id)
        {
            return Get((TId)TypeDescriptor.GetConverter(typeof(TId)).ConvertFrom(id), true);
        }

        IEnumerable<object> IFluidityRepository.GetAll()
        {
            return GetAll(true).Select(x => (object)x);
        }

        PagedResult<object> IFluidityRepository.GetPaged(int pageNumber, int pageSize, LambdaExpression orderBy, Direction orderDirection, LambdaExpression whereClause)
        {
            var result = GetPaged(pageNumber, pageSize, (Expression<Func<TEntity, object>>)orderBy, orderDirection, (Expression<Func<TEntity, bool>>)whereClause, true);

            return new PagedResult<object>(result.TotalItems, result.PageNumber, result.PageSize)
            {
                Items = result.Items.Select(x => (object)x)
            };
        }

        object IFluidityRepository.Save(object entity)
        {
            return Save((TEntity)entity, true);
        }

        void IFluidityRepository.Delete(object id)
        {
            Delete((TId)TypeDescriptor.GetConverter(typeof(TId)).ConvertFrom(id), true);
        }

        long IFluidityRepository.GetTotalRecordCount()
        {
            return GetTotalRecordCount(true);
        }

        #endregion
    }
}
