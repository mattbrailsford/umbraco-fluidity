// <copyright file="DefaultFluidityRepository.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;
using System.Linq.Expressions;
using Fluidity.Configuration;
using Fluidity.Events;
using Fluidity.Extensions;
using Fluidity.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;
using System;

namespace Fluidity.Data
{
    public class DefaultFluidityRepository : IFluidityRepository
    {
        protected FluidityCollectionConfig _collection;

        protected ISqlSyntaxProvider SyntaxProvider => ApplicationContext.Current.DatabaseContext.SqlSyntax;

        protected Database Db => !_collection.ConnectionString.IsNullOrWhiteSpace()
            ? new Database(_collection.ConnectionString)
            : ApplicationContext.Current.DatabaseContext.Database;

        public DefaultFluidityRepository(FluidityCollectionConfig collection)
        {
            _collection = collection;
        }

        public Type EntityType => _collection.EntityType;

        public Type IdType => _collection.IdProperty.Type;

        public object Get(object id, bool fireEvents = true)
        {
            return Db.SingleOrDefault(_collection.EntityType, id);
        }

        public IEnumerable<object> GetAll(bool fireEvents = true)
        {
            var query = new Sql($"SELECT * FROM [{_collection.EntityType.GetTableName()}]");

            bool hasWhere = false;
            if (_collection.FilterExpression != null)
            {
                query.Where(_collection.EntityType, _collection.FilterExpression, SyntaxProvider);
                hasWhere = true;
            }

            if (_collection.DeletedProperty != null)
            {
                var prefix = !hasWhere ? "WHERE" : "AND";
                query.Append($" {prefix} {_collection.DeletedProperty.GetColumnName()} = 0 ");
            }

            if (_collection.SortProperty != null)
            {
                if (_collection.SortDirection == SortDirection.Ascending)
                {
                    SqlExtensions.OrderBy(query, _collection.EntityType, _collection.SortProperty, SyntaxProvider);
                }
                else
                {
                    SqlExtensions.OrderByDescending(query, _collection.EntityType, _collection.SortProperty, SyntaxProvider);

                }
            }

            return Db.Fetch(_collection.EntityType, query);
        }

        public PagedResult<object> GetPaged(int pageNumber, int pageSize, LambdaExpression whereClause, LambdaExpression orderBy, SortDirection orderDirection, bool fireEvents = true)
        {
            var query = new Sql($"SELECT * FROM [{_collection.EntityType.GetTableName()}]");

            // Where
            if (_collection.FilterExpression != null && whereClause != null)
            {
                var body = Expression.AndAlso(whereClause.Body, _collection.FilterExpression.Body);
                whereClause = Expression.Lambda(body, whereClause.Parameters[0]);
            }
            else if (_collection.FilterExpression != null)
            {
                whereClause = _collection.FilterExpression;
            }

            if (whereClause != null)
            {
                query.Where(_collection.EntityType, whereClause, SyntaxProvider);
            }
            else
            {
                query.Where("1 = 1");
            }

            if (_collection.DeletedProperty != null)
            {
                query.Append($" AND ({_collection.DeletedProperty.GetColumnName()} = 0)");
            }

            // Order by
            LambdaExpression orderByExp = orderBy ?? _collection.SortProperty;
            if (orderByExp != null) 
            {
                if (orderDirection == SortDirection.Ascending)
                {
                    SqlExtensions.OrderBy(query, _collection.EntityType, orderByExp, SyntaxProvider);
                }
                else
                {
                    SqlExtensions.OrderByDescending(query, _collection.EntityType, orderByExp, SyntaxProvider);
                }
            }
            else
            {
                // There is a bug in the Db.Page code that effectively requires there
                // to be an order by clause no matter what, so if one isn't provided
                // we'lld just order by 1
                query.Append(" ORDER BY 1 ");
            }

            var result = Db.Page(_collection.EntityType, pageNumber, pageSize, query);

            return new PagedResult<object>(result.TotalItems, pageNumber, pageSize)
            {
                Items = result.Items
            };
        }

        public object Save(object entity, bool fireEvents = true)
        {
            SavingEntityEventArgs args = null;

            if (fireEvents)
            {
                var existing = Get(entity.GetPropertyValue(_collection.IdProperty));
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
                    return args.Entity.After;

                entity = args.Entity.After;
            }

            Db.Save(entity);

            if (fireEvents)
            {
                Fluidity.OnSavedEntity(args);

                entity = args.Entity.After;
            }

            return entity;
        }

        public void Delete(object id, bool fireEvents = true)
        {
            DeletingEntityEventArgs args = null;

            if (fireEvents)
            {
                var existing = Get(id);
                args = new DeletingEntityEventArgs
                {
                    Entity = existing
                };

                Fluidity.OnDeletingEntity(args);

                if (args.Cancel)
                    return;

            }

            var query = new Sql(_collection.DeletedProperty != null
                ? $"UPDATE [{_collection.EntityType.GetTableName()}] SET {_collection.DeletedProperty.GetColumnName()} = 1 WHERE {_collection.IdProperty.GetColumnName()} = @0"
                : $"DELETE FROM [{_collection.EntityType.GetTableName()}] WHERE {_collection.IdProperty.GetColumnName()} = @0",
                id);

            Db.Execute(query);

            if (fireEvents)
                Fluidity.OnDeletedEntity(args);
        }

        public long GetTotalRecordCount(bool fireEvents = true)
        {
            var query = new Sql($"SELECT COUNT(1) FROM [{_collection.EntityType.GetTableName()}]");

            bool hasWhere = false;
            if (_collection.FilterExpression != null)
            {
                query.Where(_collection.EntityType, _collection.FilterExpression, SyntaxProvider);
                hasWhere = true;
            }

            if (_collection.DeletedProperty != null)
            {
                var prefix = !hasWhere ? "WHERE" : "AND";
                query.Append($" {prefix} {_collection.DeletedProperty.GetColumnName()} = 0 ");
            }

            return Db.ExecuteScalar<long>(query);
        }

        public LambdaExpression CreateQueryExpression(FluidityCollectionConfig collection, ParameterExpression parameter, string query)
        {
            LambdaExpression queryExpression = null;

            // Create shared expressions
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

            return queryExpression;
        }

        public void Dispose()
        {
            //No disposable resources
        }
    }
}
