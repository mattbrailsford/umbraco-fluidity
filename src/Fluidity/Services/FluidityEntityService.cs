// <copyright file="FluidityEntityService.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Fluidity.Configuration;
using Fluidity.Data;
using Fluidity.Extensions;
using Fluidity.Models;
using Fluidity.Web.Models;
using Fluidity.Web.Models.Mappers;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Fluidity.Services
{
    internal class FluidityEntityService
    {
        protected FluidityRepositoryFactory RepositoryFactory => FluidityContext.Current.Data.RepositoryFactory;

        public ServiceContext Services => ApplicationContext.Current.Services;

        public FluiditySectionDisplayModel GetSectionDisplayModel(FluiditySectionConfig section)
        {
            var sectionMapper = new FluiditySectionMapper();

            return sectionMapper.ToDisplayModel(section);
        }

        public IEnumerable<FluidityCollectionDisplayModel> GetDashboardCollectionDisplayModels(FluiditySectionConfig section)
        {
            var collectionMapper = new FluidityCollectionMapper();

            return section.Tree.FlattenedTreeItems.Values
                .Where(x => x is FluidityCollectionConfig && ((FluidityCollectionConfig)x).IsVisibleOnDashboard)
                .Select(x => collectionMapper.ToDisplayModel(section, (FluidityCollectionConfig)x, false));
        }

        public FluidityCollectionDisplayModel GetCollectionDisplayModel(FluiditySectionConfig section, FluidityCollectionConfig collection, bool includeListView)
        {
            var collectionMapper = new FluidityCollectionMapper();
            return collectionMapper.ToDisplayModel(section, collection, includeListView);
        }

        public PagedResult<FluidityEntityDisplayModel> GetEntityDisplayModels(FluiditySectionConfig section, FluidityCollectionConfig collection, int pageNumber = 1, int pageSize = 10, string query = null, string orderBy = null, string orderDirection = null, string dataView = null)
        {            
            // Construct where clause
            LambdaExpression whereClauseExp = null;

            // Determine the data view where clause
            if (!dataView.IsNullOrWhiteSpace())
            {
                var wc = collection.ListView?.DataViewsBuilder?.GetDataViewWhereClause(dataView);
                if (wc != null)
                {
                    whereClauseExp = wc;
                }
            }

            // Construct a query where clause (and combind with the data view where clause if one exists)
            if (!query.IsNullOrWhiteSpace() && collection.ListView != null && collection.SearchableProperties.Any())
            {
                LambdaExpression queryExpression = null;

                // Create shared expressions
                var parameter = whereClauseExp != null 
                    ? whereClauseExp.Parameters.First() 
                    : Expression.Parameter(collection.EntityType);
                var queryConstantExpression = Expression.Constant(query, typeof(string));

                // Loop through searchable fields
                foreach (var searchProp in collection.SearchableProperties)
                {
                    // Create field starts with expression
                    var property = Expression.Property(parameter, searchProp.PropertyInfo);
                    var startsWithCall = Expression.Call(property, "StartsWith", null, queryConstantExpression);
                    var lambda = Expression.Lambda(startsWithCall, parameter);

                    // Combine query
                    queryExpression = queryExpression == null
                        ? lambda
                        : Expression.Lambda(Expression.OrElse(queryExpression.Body, lambda.Body), parameter);
                }

                // Combine query with any existing where clause
                if (queryExpression != null)
                {
                    whereClauseExp = whereClauseExp == null 
                        ? queryExpression 
                        : Expression.Lambda(Expression.AndAlso(whereClauseExp.Body, queryExpression.Body), parameter);
                }
            }

            // Parse the order by
            LambdaExpression orderByExp = null;
            if (!orderBy.IsNullOrWhiteSpace() && !orderBy.InvariantEquals("name"))
            {
                // Convert string into an Expression<Func<TEntityType, object>>
                var prop = collection.EntityType.GetProperty(orderBy);
                if (prop != null)
                {
                    var parameter = Expression.Parameter(collection.EntityType);
                    var property = Expression.Property(parameter, prop);
                    var castToObject = Expression.Convert(property, typeof(object));
                    orderByExp = Expression.Lambda(castToObject, parameter);
                }
            }

            var orderDir = !orderDirection.IsNullOrWhiteSpace()
                ? orderDirection.InvariantEquals("asc") ? SortDirection.Ascending : SortDirection.Descending
                : collection.SortDirection;

            PagedResult<object> result;
            using (var repo = RepositoryFactory.GetRepository(collection))
            {
                // Perform the query
                result = repo?.GetPaged(pageNumber, pageSize, whereClauseExp, orderByExp, orderDir);
            }

            // If we've got no results, return an empty result set
            if (result == null)
                return new PagedResult<FluidityEntityDisplayModel>(0, pageNumber, pageSize);

            // Map the results to the view display
            var mapper = new FluidityEntityMapper();
            return new PagedResult<FluidityEntityDisplayModel>(result.TotalItems, pageNumber, pageSize)
            {
                Items = result.Items.Select(x => mapper.ToDisplayModel(section, collection, x))
            };
        }

        public IEnumerable<FluidityEntityDisplayModel> GetEntityDisplayModelsByIds(FluiditySectionConfig section, FluidityCollectionConfig collection, object[] ids)
        {
            // Get the entities
            var entities = GetEntitiesByIds(section, collection, ids);
            if (entities == null)
                return  null;

            // Map the results to the view display
            var mapper = new FluidityEntityMapper();
            return entities.Select(x => mapper.ToDisplayModel(section, collection, x));
        }

        public IEnumerable<object> GetEntitiesByIds(FluiditySectionConfig section, FluidityCollectionConfig collection, object[] ids)
        {
            // Construct where clause
            LambdaExpression whereClauseExp = null;

            // Create shared expressions
            var parameter = Expression.Parameter(collection.EntityType);
            var idPropInfo = collection.IdProperty.PropertyInfo;

            // Loop through ids
            foreach (var id in ids)
            {
                // Create id comparrisons
                var property = Expression.Property(parameter, idPropInfo);
                var idsConst = Expression.Constant(TypeDescriptor.GetConverter(idPropInfo.PropertyType).ConvertFrom(id), idPropInfo.PropertyType);
                var compare = Expression.Equal(property, idsConst);
                var lambda = Expression.Lambda(compare, parameter);

                // Combine clauses
                whereClauseExp = whereClauseExp == null
                    ? lambda
                    : Expression.Lambda(Expression.OrElse(whereClauseExp.Body, lambda.Body), parameter);
            }

            // Perform the query
            PagedResult<object> result;
            using (var repo = RepositoryFactory.GetRepository(collection))
            {
                result = repo?.GetPaged(1, ids.Length, whereClauseExp, null, SortDirection.Ascending);
            }

            // Return the results
            return result?.Items;
        }

        public FluidityEntityEditModel GetEntityEditModel(FluiditySectionConfig section, FluidityCollectionConfig collection, object entityOrId = null)
        {             
            object entity = null;
            if (entityOrId != null)
            {
                using (var repo = RepositoryFactory.GetRepository(collection))
                {
                    entity = (entityOrId.GetType() == collection.EntityType ? entityOrId : null) ?? repo.Get(entityOrId);
                }
            }

            var mapper = new FluidityEntityMapper();
            var scaffold = mapper.ToEditModel(section, collection, entity);

            return scaffold;
        }

        public object NewEntity(FluidityCollectionConfig collection)
        {
            return Activator.CreateInstance(collection.EntityType);
        }

        public object GetEntity(FluidityCollectionConfig collection, object id)
        {
            using (var repo = RepositoryFactory.GetRepository(collection))
            {
                return repo?.Get(id);
            }
        }

        public IEnumerable<object> GetAllEntities(FluidityCollectionConfig collection)
        {
            using (var repo = RepositoryFactory.GetRepository(collection))
            {
                return repo?.GetAll();
            }
        }

        public object SaveEntity(FluidityCollectionConfig collection, object entity)
        {
            var isNew = Equals(entity.GetPropertyValue(collection.IdProperty), collection.IdProperty.Type.GetDefaultValue());

            if (isNew && collection.DateCreatedProperty != null)
            {
                entity.SetPropertyValue(collection.DateCreatedProperty, DateTime.Now);
            }

            if (collection.DateModifiedProperty != null)
            {
                entity.SetPropertyValue(collection.DateModifiedProperty, DateTime.Now);
            }

            using (var repo = RepositoryFactory.GetRepository(collection))
            {
                var savedEntity = repo?.Save(entity);

                return savedEntity;
            }
        }

        public void DeleteEntity(FluidityCollectionConfig collection, object id)
        {
            using (var repo = RepositoryFactory.GetRepository(collection))
            {
                repo?.Delete(id);
            }
        }

        public long GetsEntityTotalRecordCount(FluidityCollectionConfig collection)
        {
            using (var repo = RepositoryFactory.GetRepository(collection))
            {
                return repo?.GetTotalRecordCount() ?? 0;
            }
        }
    }
}
