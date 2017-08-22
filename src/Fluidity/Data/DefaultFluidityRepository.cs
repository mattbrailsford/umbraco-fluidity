using System.Collections.Generic;
using System.Linq.Expressions;
using Fluidity.Configuration;
using Fluidity.Extensions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
using Umbraco.Core.Persistence.SqlSyntax;

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

            if (_collection.SortPropertyExp != null)
            {
                if (_collection.SortDirection == Direction.Ascending)
                {
                    SqlExtensions.OrderBy(query, _collection.EntityType, _collection.SortPropertyExp, SyntaxProvider);
                }
                else
                {
                    SqlExtensions.OrderByDescending(query, _collection.EntityType, _collection.SortPropertyExp, SyntaxProvider);

                }
            }

            return Db.Fetch(_collection.EntityType, query);
        }

        public PagedResult<object> GetPaged(int pageNumber, int pageSize, LambdaExpression orderBy, Direction orderDirection, LambdaExpression whereClause)
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
            var orderByExp = orderBy ?? _collection.SortPropertyExp;
            if (orderByExp != null)
            {
                if (orderDirection == Direction.Ascending)
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
            Db.Save(entity);

            return entity;
        }

        public void Delete(object[] ids)
        {
            var query = new Sql(_collection.DeletedProperty != null
                ? $"UPDATE {_collection.EntityType.GetTableName()} SET {_collection.DeletedProperty.GetColumnName()} = 1 WHERE {_collection.IdProperty.GetColumnName()} IN (@ids)"
                : $"DELETE FROM {_collection.EntityType.GetTableName()} WHERE {_collection.IdProperty.GetColumnName()} IN (@ids)",
                new { ids });

            Db.Execute(query);
        }
    }
}
