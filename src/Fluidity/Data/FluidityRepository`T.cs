// <copyright file="FluidityRepository`T.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Fluidity.Events;
using Fluidity.Models;
using Umbraco.Core.Models;

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
        public virtual Type EntityType => typeof(TEntity);
        public virtual Type IdType => typeof(TId);

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
        public virtual TEntity Get(TId id, bool fireEvents = true)
        {
            return GetImpl(id);
        }
        protected abstract TEntity GetImpl(TId id);


        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="fireEvents">if set to <c>true</c> fire events.</param>
        /// <returns>A collection of entities.</returns>
        public virtual IEnumerable<TEntity> GetAll(bool fireEvents = true)
        {
            return GetAllImpl();
        }
        protected abstract IEnumerable<TEntity> GetAllImpl();


        /// <summary>
        /// Gets a paged collection of entities.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="orderDirection">The order direction.</param>
        /// <param name="fireEvents">if set to <c>true</c> fire events.</param>
        /// <returns>A collection of entities.</returns>
        public virtual PagedResult<TEntity> GetPaged(int pageNumber = 1, int pageSize = 10, Expression<Func<TEntity, bool>> whereClause = null, Expression<Func<TEntity, object>> orderBy = null, SortDirection orderDirection = SortDirection.Ascending, bool fireEvents = true)
        {
            return GetPagedImpl(pageNumber, pageSize, whereClause, orderBy, orderDirection);
        }
        protected abstract PagedResult<TEntity> GetPagedImpl(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> whereClause, Expression<Func<TEntity, object>> orderBy, SortDirection orderDirection);

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="fireEvents">if set to <c>true</c> fire events.</param>
        /// <returns>The entity.</returns>
        public virtual TEntity Save(TEntity entity, bool fireEvents = true)
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
                args.Entity.After = entity;

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
        public virtual void Delete(TId id, bool fireEvents = true)
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
        public virtual long GetTotalRecordCount(bool fireEvents = true)
        {
            return GetTotalRecordCountImpl();
        }
        protected abstract long GetTotalRecordCountImpl();

        #region IFluidityRepository

        object IFluidityRepository.Get(object id, bool fireEvents)
        {
            // Check if the specified Identifier type is the same type as the "id" arguement.
            // If it is, then we don't need to convert this value.
            // Otherwise, we will need to convert the "id" to the specified type.
            if(typeof(TId) == id.GetType())
            {
                return Get((TId)id, fireEvents);
            }
            else
            {
                return Get((TId)TypeDescriptor.GetConverter(typeof(TId)).ConvertFrom(id), fireEvents);
            }
        }

        IEnumerable<object> IFluidityRepository.GetAll(bool fireEvents)
        {
            return GetAll(fireEvents).Select(x => (object)x).ToList();
        }

        PagedResult<object> IFluidityRepository.GetPaged(int pageNumber, int pageSize, LambdaExpression whereClause, LambdaExpression orderBy, SortDirection orderDirection, bool fireEvents)
        {
            var result = GetPaged(pageNumber, pageSize, (Expression<Func<TEntity, bool>>)whereClause, (Expression<Func<TEntity, object>>)orderBy, orderDirection, fireEvents);

            return new PagedResult<object>(result.TotalItems, result.PageNumber, result.PageSize)
            {
                Items = result.Items.Select(x => (object)x).ToList()
            };
        }

        object IFluidityRepository.Save(object entity, bool fireEvents)
        {
            return Save((TEntity)entity, fireEvents);
        }

        void IFluidityRepository.Delete(object id, bool fireEvents)
        {
            Delete((TId)TypeDescriptor.GetConverter(typeof(TId)).ConvertFrom(id), fireEvents);
        }

        long IFluidityRepository.GetTotalRecordCount(bool fireEvents)
        {
            return GetTotalRecordCount(fireEvents);
        }

        #endregion

        public virtual void Dispose()
        {
            //No resources to dispose of by default
        }
    }
}
