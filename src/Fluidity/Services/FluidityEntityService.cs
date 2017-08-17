using System;
using System.Collections.Generic;
using System.Linq;
using Fluidity.Configuration;
using Fluidity.Data;
using Fluidity.Extensions;
using Fluidity.Web.Models;
using Fluidity.Web.Models.Mappers;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Fluidity.Services
{
    internal class FluidityEntityService
    {
        protected FluidityRepositoryFactory _repoFactory;

        public ServiceContext Services => ApplicationContext.Current.Services;

        public FluidityEntityService(FluidityRepositoryFactory repositoryFactory)
        {
            _repoFactory = repositoryFactory;
        }

        public FluidityCollectionDisplay GetsCollectionDisplay(FluiditySectionConfig section, FluidityCollectionConfig collection)
        {
            // TODO: Should this even be in the "EntityService"?
            var collectionMapper = new FluidityCollectionMapper();
            return collectionMapper.ToDisplay(section, collection);
        }

        public PagedResult<FluidityEntityListViewDisplay> GetListViewEntitiesDisplay(FluiditySectionConfig section, FluidityCollectionConfig collection, int pageNumber = 1, int pageSize = 10, string orderBy = null, string orderDirection = null, string filter = null)
        {
            var repo = _repoFactory.GetRepository(collection);
            var result = repo?.GetPaged(pageNumber, pageSize, orderBy, orderDirection, filter);

            // If we've got no results, return an empty result set
            if (result == null)
                return new PagedResult<FluidityEntityListViewDisplay>(0, pageNumber, pageSize);

            // Map the results to the view display
            var mapper = new FluidityEntityListViewMapper();
            return new PagedResult<FluidityEntityListViewDisplay>(result.TotalItems, result.PageNumber, result.PageSize)
            {
                Items = result.Items.Select(x => mapper.ToDisplay(section, collection, x))
            };
        }

        public FluidityEntityDisplay GetEntityDisplay(FluiditySectionConfig section, FluidityCollectionConfig collection, object entityOrId = null)
        {
            var repo = _repoFactory.GetRepository(collection);
             
            object entity = null;
            if (entityOrId != null)
            {
                entity = (entityOrId.GetType() == collection.EntityType ? entityOrId : null) ?? repo.Get(entityOrId);
            }

            var mapper = new FluidityEntityMapper();
            var scaffold = mapper.ToDisplay(section, collection, entity);

            return scaffold;
        }

        public object NewEntity(FluidityCollectionConfig collection)
        {
            return Activator.CreateInstance(collection.EntityType);
        }

        public object GetEntity(FluidityCollectionConfig collection, object id)
        {
            var repo = _repoFactory.GetRepository(collection);

            return repo?.Get(id);
        }

        public IEnumerable<object> GetAllEntities(FluidityCollectionConfig collection)
        {
            var repo = _repoFactory.GetRepository(collection);

            return repo?.GetAll();
        }

        public object SaveEntity(FluidityCollectionConfig collection, object entity)
        {
            var repo = _repoFactory.GetRepository(collection);
            var isNew = entity.GetPropertyValue(collection.IdProperty) == collection.IdProperty.PropertyType.GetDefaultValue();

            if (isNew && collection.DateCreated != null)
            {
                entity.SetPropertyValue(collection.DateCreated, DateTime.Now);
            }

            if (collection.DateModified != null)
            {
                entity.SetPropertyValue(collection.DateModified, DateTime.Now);
            }

            repo?.Save(entity);

            return entity;
        }

        public void DeleteEntity(FluidityCollectionConfig collection, object[] ids)
        {
            var repo = _repoFactory.GetRepository(collection);
            repo?.Delete(ids);
        }
    }
}
