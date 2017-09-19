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
    /// <summary>
    /// Base class for a custom Fluidity repository
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <seealso cref="Fluidity.Data.IFluidityRepository" />
    public abstract class FluidityRepository<TEntity, TId> : IFluidityRepository
    {
        private TId GetId(TEntity entity)
        {
            return GetIdImpl(entity);
        }
        protected abstract TId GetIdImpl(TEntity entity);


        /// <summary>
        /// Gets entity by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="fireEvents">if set to <c>true</c> fire events.</param>
        /// <returns>The entity.</returns>
        public TEntity Get(TId id, bool fireEvents = true)
        {
            return GetImpl(id);
        }
        protected abstract TEntity GetImpl(TId id);


        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="fireEvents">if set to <c>true</c> fire events.</param>
        /// <returns>A collection of entities.</returns>
        public IEnumerable<TEntity> GetAll(bool fireEvents = true)
        {
            return GetAllImpl();
        }
        protected abstract IEnumerable<TEntity> GetAllImpl();

        
        /// <summary>
        /// Gets a paged collection of entities.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="orderDirection">The order direction.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="fireEvents">if set to <c>true</c> fire events.</param>
        /// <returns>A collection of entities.</returns>
        public PagedResult<TEntity> GetPaged(int pageNumber = 1, int pageSize = 10, Expression<Func<TEntity, object>> orderBy = null, Direction orderDirection = Direction.Ascending, Expression<Func<TEntity, bool>> whereClause = null, bool fireEvents = true)
        {
            return GetPagedImpl(pageNumber, pageSize, orderBy, orderDirection, whereClause);
        }
        protected abstract PagedResult<TEntity> GetPagedImpl(int pageNumber, int pageSize, Expression<Func<TEntity, object>> orderBy, Direction orderDirection, Expression<Func<TEntity, bool>> whereClause);

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="fireEvents">if set to <c>true</c> fire events.</param>
        /// <returns>The entity.</returns>
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
        protected abstract TEntity SaveImpl(TEntity entity);


        /// <summary>
        /// Deletes the entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="fireEvents">if set to <c>true</c> fire events.</param>
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
        protected abstract void DeleteImpl(TId id);

        /// <summary>
        /// Gets the total record count.
        /// </summary>
        /// <param name="fireEvents">if set to <c>true</c> fire events.</param>
        /// <returns>The total number of records.</returns>
        public long GetTotalRecordCount(bool fireEvents = true)
        {
            return GetTotalRecordCountImpl();
        }
        protected abstract long GetTotalRecordCountImpl();

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
