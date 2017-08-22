using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Fluidity.Configuration;
using Fluidity.Web.Extensions;
using Fluidity.Web.Models;
using Fluidity.Web.Models.Mappers;
using Fluidity.Web.WebApi.Binders;
using Fluidity.Web.WebApi.Filters;
using Fluidity.Web.WebApi.Validation;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Fluidity.Web.Api
{
    [PluginController("fluidity")]
    [OutgoingDateTimeFormat]
    public class FluidityApiController : UmbracoAuthorizedJsonController
    {
        internal FluidityContext Context => FluidityContext.Current;

        public object Index()
        {
            // Utility action used just as a hook for ServerVariablesInjector
            return null;
        }

        [HttpGet]
        public object GetSectionByAlias(string section)
        {
            var sectionConfig = Context.Config.Sections[section];
            return Context.Services.EntityService.GetSectionDisplay(sectionConfig);
        }

        [HttpGet]
        public object GetEntityScaffold(string section, string collectionAlias)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FalttenedTreeItems[collectionAlias] as FluidityCollectionConfig;
            return Context.Services.EntityService.GetEntityDisplay(sectionConfig, collectionConfig);
        }

        [HttpGet]
        public object GetEntityById(string section, string collectionAlias, string id)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FalttenedTreeItems[collectionAlias] as FluidityCollectionConfig;
            return Context.Services.EntityService.GetEntityDisplay(sectionConfig, collectionConfig, id);
        }

        [HttpGet]
        public object GetListViewEntities(string section, string collectionAlias, int pageNumber = 1, string orderBy = "", string orderDirection = "", string query = "", string dataView = "")
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FalttenedTreeItems[collectionAlias] as FluidityCollectionConfig;
            return Context.Services.EntityService.GetListViewEntitiesDisplay(sectionConfig, collectionConfig, pageNumber, orderBy, orderDirection, query, dataView);
        }

        [HttpPost]
        [FileUploadCleanupFilter]
        public object SaveEntity([ModelBinder(typeof(FluidityEntityBinder))] FluidityEntityPost postModel)
        {
            FluidityEntityDisplay display;

            var sectionConfig = Context.Config.Sections[postModel.Section];
            var collectionConfig = sectionConfig.Tree.FalttenedTreeItems[postModel.Collection] as FluidityCollectionConfig;

            var entity = postModel.Id != null
                ? Context.Services.EntityService.GetEntity(collectionConfig, postModel.Id)
                : Context.Services.EntityService.NewEntity(collectionConfig);

            // Map property values
            var mapper = new FluidityEntityMapper();
            entity = mapper.FromPost(sectionConfig, collectionConfig, postModel, entity);

            // Validate the property values (review ContentItemValidationHelper)
            var validator = new FluidityEntityPostValidator();
            validator.Validate(ModelState, postModel, collectionConfig);

            // Check to see if model is valid
            if (!ModelState.IsValid)
            {
                display = Context.Services.EntityService.GetEntityDisplay(sectionConfig, collectionConfig, entity);
                display.Errors = ModelState.ToErrorDictionary();
                throw new HttpResponseException(Request.CreateValidationErrorResponse(display));
            }

            // Do the save
            entity = Context.Services.EntityService.SaveEntity(collectionConfig, entity);
            display = Context.Services.EntityService.GetEntityDisplay(sectionConfig, collectionConfig, entity);

            // Return the updated model
            return display;
        }

        [HttpDelete]
        public void DeleteEntity(string section, string collectionAlias, string id)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FalttenedTreeItems[collectionAlias] as FluidityCollectionConfig;

            Context.Services.EntityService.DeleteEntity(collectionConfig, id);
        }

        [HttpGet]
        public object GetCollectionByAlias(string section, string collectionAlias)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FalttenedTreeItems[collectionAlias] as FluidityCollectionConfig;

            return Context.Services.EntityService.GetCollectionDisplay(sectionConfig, collectionConfig);
        }

        [HttpGet]
        public object GetDashboardCollections(string section)
        {
            var sectionConfig = Context.Config.Sections[section];
            return Context.Services.EntityService.GetDashboardCollectionDisplays(sectionConfig);
        }

        [HttpGet]
        public object GetEntityTotalRecordCount(string section, string collectionAlias)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FalttenedTreeItems[collectionAlias] as FluidityCollectionConfig;

            return Context.Services.EntityService.GetsEntityTotalRecordCount(collectionConfig);
        }
    }
}
