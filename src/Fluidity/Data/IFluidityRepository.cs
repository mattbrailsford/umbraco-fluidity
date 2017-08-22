using System.Collections.Generic;
using System.Linq.Expressions;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;

namespace Fluidity.Data
{
    public interface IFluidityRepository
    {
        object Get(object id);

        IEnumerable<object> GetAll();

        PagedResult<object> GetPaged(int pageNumber, int pageSize, LambdaExpression orderBy, Direction orderDirection, LambdaExpression whereClause);

        object Save(object entity);

        void Delete(object id);

        long GetTotalRecordCount();
    }
}