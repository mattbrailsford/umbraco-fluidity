using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Fluidity.Configuration;
using Fluidity.Data;
using Fluidity.Extensions;
using Fluidity.Web.Models;
using Fluidity.Web.Models.Mappers;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
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

        public FluiditySectionDisplayModel GetSectionDisplayModel(FluiditySectionConfig section)
        {
            var sectionMapper = new FluiditySectionMapper();

            return sectionMapper.ToDisplayModel(section);
        }

        public IEnumerable<FluidityCollectionDisplayModel> GetDashboardCollectionDisplayModels(FluiditySectionConfig section)
        {
            var collectionMapper = new FluidityCollectionMapper();

            return section.Tree.FalttenedTreeItems.Values
                .Where(x => x is FluidityCollectionConfig && ((FluidityCollectionConfig)x).IsVisibleOnDashboard)
                .Select(x => collectionMapper.ToDisplayModel(section, (FluidityCollectionConfig)x, false));
        }

        public FluidityCollectionDisplayModel GetCollectionDisplayModel(FluiditySectionConfig section, FluidityCollectionConfig collection, bool includeListView)
        {
            var collectionMapper = new FluidityCollectionMapper();
            return collectionMapper.ToDisplayModel(section, collection, includeListView);
        }

        public PagedResult<FluidityEntityDisplayModel> GetEntityDisplayModels(FluiditySectionConfig section, FluidityCollectionConfig collection, int pageNumber = 1, int pageSize = 10, string orderBy = null, string orderDirection = null, string query = null, string dataView = null)
        {
            var repo = _repoFactory.GetRepository(collection);
            
            // Construct where clause
            LambdaExpression whereClauseExp = null;

            // Determine the data view where clause
            if (!dataView.IsNullOrWhiteSpace())
            {
                var hasDataViews = collection.ListView != null && collection.ListView.DataViews.Any();
                if (hasDataViews)
                {
                    var dv = collection.ListView.DataViews.FirstOrDefault(x => x.Alias.InvariantEquals(dataView));
                    if (dv != null)
                    {
                        whereClauseExp = dv.WhereClauseExpression;
                    }
                }
            }

            // Construct a query where clause (and combind with the data view where clause if one exists)
            if (!query.IsNullOrWhiteSpace() && collection.ListView != null && collection.ListView.SearchFields.Any())
            {
                LambdaExpression queryExpression = null;

                // Create shared expressions
                var parameter = whereClauseExp != null 
                    ? whereClauseExp.Parameters.First() 
                    : Expression.Parameter(collection.EntityType);
                var queryConstantExpression = Expression.Constant(query, typeof(string));

                // Loop through searchable fields
                foreach (var searchField in collection.ListView.SearchFields)
                {
                    // Create field starts with expression
                    var property = Expression.Property(parameter, searchField.Property);
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
                ? orderDirection.InvariantEquals("asc") ? Direction.Ascending : Direction.Descending
                : collection.SortDirection;

            // Perform the query
            var result = repo?.GetPaged(pageNumber, pageSize, orderByExp, orderDir, whereClauseExp); 

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
            var repo = _repoFactory.GetRepository(collection);

            // Construct where clause
            LambdaExpression whereClauseExp = null;

            // Create shared expressions
            var parameter = Expression.Parameter(collection.EntityType);

            // Loop through ids
            foreach (var id in ids)
            {
                // Create id comparrisons
                var property = Expression.Property(parameter, collection.IdProperty);
                var idsConst = Expression.Constant(TypeDescriptor.GetConverter(collection.IdProperty.PropertyType).ConvertFrom(id), collection.IdProperty.PropertyType);
                var compare = Expression.Equal(property, idsConst);
                var lambda = Expression.Lambda(compare, parameter);

                // Combine clauses
                whereClauseExp = whereClauseExp == null
                    ? lambda
                    : Expression.Lambda(Expression.OrElse(whereClauseExp.Body, lambda.Body), parameter);
            }

            // Perform the query
            var result = repo?.GetPaged(1, ids.Length, null, Direction.Ascending, whereClauseExp);

            // If we've got no results, return null
            if (result == null)
                return null;

            // Map the results to the view display
            var mapper = new FluidityEntityMapper();
            return result.Items.Select(x => mapper.ToDisplayModel(section, collection, x));
        }

        public FluidityEntityEditModel GetEntityEditModel(FluiditySectionConfig section, FluidityCollectionConfig collection, object entityOrId = null)
        {
            var repo = _repoFactory.GetRepository(collection);
             
            object entity = null;
            if (entityOrId != null)
            {
                entity = (entityOrId.GetType() == collection.EntityType ? entityOrId : null) ?? repo.Get(entityOrId);
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

            if (isNew && collection.DateCreatedProperty != null)
            {
                entity.SetPropertyValue(collection.DateCreatedProperty, DateTime.Now);
            }

            if (collection.DateModifiedProperty != null)
            {
                entity.SetPropertyValue(collection.DateModifiedProperty, DateTime.Now);
            }

            repo?.Save(entity);

            return entity;
        }

        public void DeleteEntity(FluidityCollectionConfig collection, object id)
        {
            var repo = _repoFactory.GetRepository(collection);
            repo?.Delete(id);
        }

        public long GetsEntityTotalRecordCount(FluidityCollectionConfig collection)
        {
            var repo = _repoFactory.GetRepository(collection);
            return repo?.GetTotalRecordCount() ?? 0;
        }
    }
}
