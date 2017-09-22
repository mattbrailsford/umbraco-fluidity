// <copyright file="FluidityApiController.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq;
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
            return Context.Services.EntityService.GetSectionDisplayModel(sectionConfig);
        }
        
        [HttpGet]
        public object GetCollectionByAlias(string section, string collectionAlias)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FlattenedTreeItems[collectionAlias] as FluidityCollectionConfig;

            return Context.Services.EntityService.GetCollectionDisplayModel(sectionConfig, collectionConfig, true);
        }

        [HttpGet]
        public object GetDashboardCollections(string section)
        {
            var sectionConfig = Context.Config.Sections[section];
            return Context.Services.EntityService.GetDashboardCollectionDisplayModels(sectionConfig);
        }

        [HttpGet]
        public object GetCollectionsLookup()
        {
            return Context.Config.Sections.Values.Select(s => new {
                    alias = s.Alias,
                    name = s.Name,
                    collections = s.Tree.FlattenedTreeItems.Values
                        .Where(c => c is FluidityCollectionConfig)
                        .Cast<FluidityCollectionConfig>()
                        .Select(c => new {
                            alias = c.Alias,
                            name = c.NamePlural,
                            dataViews = c.ListView?.DataViews.Select(dv => new
                            {
                                alias = dv.Alias,
                                name = dv.Name
                            })
                        })
                });
        }

        [HttpGet]
        public object GetEntityScaffold(string section, string collectionAlias)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FlattenedTreeItems[collectionAlias] as FluidityCollectionConfig;
            return Context.Services.EntityService.GetEntityEditModel(sectionConfig, collectionConfig);
        }

        [HttpGet]
        public object GetEntityById(string section, string collectionAlias, string id)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FlattenedTreeItems[collectionAlias] as FluidityCollectionConfig;
            return Context.Services.EntityService.GetEntityEditModel(sectionConfig, collectionConfig, id);
        }

        [HttpGet]
        public object GetEntities(string section, string collectionAlias, int pageNumber = 1, int pageSize = 10, string orderBy = "", string orderDirection = "", string query = "", string dataView = "")
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FlattenedTreeItems[collectionAlias] as FluidityCollectionConfig;
            return Context.Services.EntityService.GetEntityDisplayModels(sectionConfig, collectionConfig, pageNumber, pageSize, orderBy, orderDirection, query, dataView);
        }

        [HttpGet]
        public object GetEntitiesByIds(string section, string collectionAlias, string ids)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FlattenedTreeItems[collectionAlias] as FluidityCollectionConfig;
            return Context.Services.EntityService.GetEntityDisplayModelsByIds(sectionConfig, collectionConfig, ids.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries));
        }

        [HttpPost]
        [FileUploadCleanupFilter]
        public object SaveEntity([ModelBinder(typeof(FluidityEntityBinder))] FluidityEntityPostModel postModel)
        {
            FluidityEntityEditModel display;

            var sectionConfig = Context.Config.Sections[postModel.Section];
            var collectionConfig = sectionConfig.Tree.FlattenedTreeItems[postModel.Collection] as FluidityCollectionConfig;

            var entity = postModel.Id != null
                ? Context.Services.EntityService.GetEntity(collectionConfig, postModel.Id)
                : Context.Services.EntityService.NewEntity(collectionConfig);

            // Map property values
            var mapper = new FluidityEntityMapper();
            entity = mapper.FromPostModel(sectionConfig, collectionConfig, postModel, entity);

            // Validate the property values (review ContentItemValidationHelper)
            var validator = new FluidityEntityPostValidator();
            validator.Validate(ModelState, postModel, entity, collectionConfig);

            // Check to see if model is valid
            if (!ModelState.IsValid)
            {
                display = Context.Services.EntityService.GetEntityEditModel(sectionConfig, collectionConfig, entity);
                display.Errors = ModelState.ToErrorDictionary();
                throw new HttpResponseException(Request.CreateValidationErrorResponse(display));
            }

            // Do the save
            entity = Context.Services.EntityService.SaveEntity(collectionConfig, entity);
            display = Context.Services.EntityService.GetEntityEditModel(sectionConfig, collectionConfig, entity);

            // Return the updated model
            return display;
        }

        [HttpDelete]
        public void DeleteEntity(string section, string collectionAlias, string id)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FlattenedTreeItems[collectionAlias] as FluidityCollectionConfig;

            Context.Services.EntityService.DeleteEntity(collectionConfig, id);
        }

        [HttpGet]
        public object GetEntityTotalRecordCount(string section, string collectionAlias)
        {
            var sectionConfig = Context.Config.Sections[section];
            var collectionConfig = sectionConfig.Tree.FlattenedTreeItems[collectionAlias] as FluidityCollectionConfig;

            return Context.Services.EntityService.GetsEntityTotalRecordCount(collectionConfig);
        }
    }
}
