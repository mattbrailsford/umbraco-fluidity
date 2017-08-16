using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Fluidity.Events;
using Fluidity.Models;

namespace Fluidity.Data
{
    public abstract class FluidityRepository<TEntity, TId> : IFluidityRepository
    {
        //public static event EventHandler<SingleEntityEventArgs<BeforeAndAfter<TEntity>>> UpdatingEntity;
        //public static event EventHandler<SingleEntityEventArgs<BeforeAndAfter<TEntity>>> UpdatedEntity;

        //public static event EventHandler<SingleEntityEventArgs<BeforeAndAfter<TEntity>>> CreatingEntity;
        //public static event EventHandler<SingleEntityEventArgs<BeforeAndAfter<TEntity>>> CreatedEntity;

        //public static event EventHandler<MultiEntityEventArgs<BeforeAndAfter<TEntity>>> DeletingEntities;
        //public static event EventHandler<MultiEntityEventArgs<BeforeAndAfter<TEntity>>> DeletedEntities;

        public abstract TEntity Get(TId id);

        public abstract IEnumerable<TEntity> GetAll();

        public abstract TEntity Save(TEntity entity);

        public abstract void Delete(TId[ ] ids);

        #region IFluidityRepository

        object IFluidityRepository.Get(object id)
        {
            return Get((TId)TypeDescriptor.GetConverter(typeof(TId)).ConvertFrom(id));
        }

        IEnumerable<object> IFluidityRepository.GetAll()
        {
            return GetAll().Select(x => (object)x);
        }

        object IFluidityRepository.Save(object entity)
        {
            return Save((TEntity)entity);
        }

        void IFluidityRepository.Delete(object[] ids)
        {
            Delete(ids.Select(x => (TId)TypeDescriptor.GetConverter(typeof(TId)).ConvertFrom(x)).ToArray());
        }

        #endregion
    }
}
