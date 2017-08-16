using System.Collections.Generic;
using Fluidity.Configuration;
using Fluidity.Extensions;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Fluidity.Data
{
    public class DefaultFluidityRepository : IFluidityRepository
    {
        protected FluidityCollectionConfig _config;

        protected Database Db => ApplicationContext.Current.DatabaseContext.Database;

        public DefaultFluidityRepository(FluidityCollectionConfig config)
        {
            _config = config;
        }

        public object Get(object id)
        {
            return Db.SingleOrDefault(_config.EntityType, id);
        }

        public IEnumerable<object> GetAll()
        {
            var query = new Sql($"SELECT * FROM {_config.EntityType.GetTableName()}");

            if (_config.DeletedProperty != null)
            {
                query.Append($"WHERE {_config.DeletedProperty.GetColumnName()} = 0");
            }

            if (_config.SortProperty != null)
            {
                query.OrderBy($"{_config.SortProperty.GetColumnName()} {_config.SortOrder}");
            }

            return Db.Fetch(_config.EntityType, query);
        }

        public object Save(object entity)
        {
            Db.Save(entity);

            return entity;
        }

        public void Delete(object[] ids)
        {
            var query = new Sql(_config.DeletedProperty != null
                ? $"UPDATE {_config.EntityType.GetTableName()} SET {_config.DeletedProperty.GetColumnName()} = 1 WHERE {_config.IdProperty.GetColumnName()} IN (@ids)"
                : $"DELETE FROM {_config.EntityType.GetTableName()} WHERE {_config.IdProperty.GetColumnName()} IN (@ids)",
                new { ids });

            Db.Execute(query);
        }
    }
}
