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
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
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

        public object Get(object id)
        {
            return Db.SingleOrDefault(_collection.EntityType, id);
        }

        public IEnumerable<object> GetAll()
        {
            var query = new Sql($"SELECT * FROM {_collection.EntityType.GetTableName()}");

            if (_collection.DeletedProperty != null)
            {
                query.Append($"WHERE {_collection.DeletedProperty.GetColumnName()} = 0");
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

        public PagedResult<object> GetPaged(int pageNumber, int pageSize, LambdaExpression orderBy, SortDirection orderDirection, LambdaExpression whereClause)
        {
            var query = new Sql($"SELECT * FROM {_collection.EntityType.GetTableName()}");

            // Where
            if (whereClause != null)
            {
                query.Where(_collection.EntityType, whereClause, SyntaxProvider);
            }
            else
            {
                query.Where(" 1 = 1");
            }

            if (_collection.DeletedProperty != null)
            {
                query.Append($" AND {_collection.DeletedProperty.GetColumnName()} = 0");
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

            var result = Db.Page(_collection.EntityType, pageNumber, pageSize, query);

            return  new PagedResult<object>(result.TotalItems, pageNumber, pageSize)
            {
                Items = result.Items
            };
        }

        public object Save(object entity)
        {
            var existing = Get(entity.GetPropertyValue(_collection.IdProperty));
            var args = new SavingEntityEventArgs
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

            Db.Save(args.Entity.After);

            Fluidity.OnSavedEntity(args);

            return args.Entity.After;
        }

        public void Delete(object id)
        {
            var existing = Get(id);
            var args = new DeletingEntityEventArgs
            {
                Entity = existing
            };

            Fluidity.OnDeletingEntity(args);

            if (args.Cancel)
                return;

            var query = new Sql(_collection.DeletedProperty != null
                ? $"UPDATE {_collection.EntityType.GetTableName()} SET {_collection.DeletedProperty.GetColumnName()} = 1 WHERE {_collection.IdProperty.GetColumnName()} = @0"
                : $"DELETE FROM {_collection.EntityType.GetTableName()} WHERE {_collection.IdProperty.GetColumnName()} = @0",
                id);

            Db.Execute(query);

            Fluidity.OnDeletedEntity(args);
        }

        public long GetTotalRecordCount()
        {
            var sql = $"SELECT COUNT(1) FROM {_collection.EntityType.GetTableName()}";

            if (_collection.DeletedProperty != null)
            {
                sql += $" WHERE {_collection.DeletedProperty.GetColumnName()} = 0";
            }

            return Db.ExecuteScalar<long>(sql);
        }
    }
}
