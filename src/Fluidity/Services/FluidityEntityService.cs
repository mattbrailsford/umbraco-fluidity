using System;
using System.Collections.Generic;
using Fluidity.Configuration;
using Fluidity.Data;
using Fluidity.Extensions;
using Fluidity.Web.Models;
using Fluidity.Web.Models.Mappers;
using Umbraco.Core;
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

        public FluidityEntityDisplay Scaffold(FluiditySectionConfig section, FluidityCollectionConfig config, object entityOrId = null)
        {
            var repo = _repoFactory.GetRepository(config);
             
            object entity = null;
            if (entityOrId != null)
            {
                entity = (entityOrId.GetType() == config.EntityType ? entityOrId : null) ?? repo.Get(entityOrId);
            }

            var mapper = new FluidityEntityMapper();
            var scaffold = mapper.ToDisplay(section, config, entity);

            return scaffold;
        }

        public object New(FluidityCollectionConfig config)
        {
            return Activator.CreateInstance(config.EntityType);
        }

        public object Get(FluidityCollectionConfig config, object id)
        {
            var repo = _repoFactory.GetRepository(config);

            return repo?.Get(id);
        }

        public IEnumerable<object> GetAll(FluidityCollectionConfig config)
        {
            var repo = _repoFactory.GetRepository(config);

            return repo?.GetAll();
        }

        public object Save(FluidityCollectionConfig config, object entity)
        {
            var repo = _repoFactory.GetRepository(config);
            var isNew = entity.GetPropertyValue(config.IdProperty) == config.IdProperty.PropertyType.GetDefaultValue();

            if (isNew && config.DateCreated != null)
            {
                entity.SetPropertyValue(config.DateCreated, DateTime.Now);
            }

            if (config.DateModified != null)
            {
                entity.SetPropertyValue(config.DateModified, DateTime.Now);
            }

            repo?.Save(entity);

            return entity;
        }

        public void Delete(FluidityCollectionConfig config, object[] ids)
        {
            var repo = _repoFactory.GetRepository(config);
            repo?.Delete(ids);
        }
    }
}
