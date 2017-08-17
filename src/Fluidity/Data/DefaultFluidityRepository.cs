using System.Collections.Generic;
using Fluidity.Configuration;
using Fluidity.Extensions;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;

namespace Fluidity.Data
{
    public class DefaultFluidityRepository : IFluidityRepository
    {
        protected FluidityCollectionConfig _collection;

        protected Database Db => ApplicationContext.Current.DatabaseContext.Database;

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

            if (_collection.SortProperty != null)
            {
                query.OrderBy($"{_collection.SortProperty.GetColumnName()} {_collection.SortOrder}");
            }

            return Db.Fetch(_collection.EntityType, query);
        }

        public PagedResult<object> GetPaged(int pageNumber, int pageSize, string orderBy, string orderDirection, string filter)
        {
            var query = new Sql($"SELECT * FROM {_collection.EntityType.GetTableName()}");

            if (_collection.DeletedProperty != null)
            {
                query.Append($"WHERE {_collection.DeletedProperty.GetColumnName()} = 0");
            }

            if (!orderBy.IsNullOrWhiteSpace())
            {
                query.OrderBy($"{orderBy} {orderDirection ?? "ASC"}");
            }
            else if (_collection.SortProperty != null)
            {
                query.OrderBy($"{_collection.SortProperty.GetColumnName()} {_collection.SortOrder}");
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
