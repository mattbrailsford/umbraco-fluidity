using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Fluidity.Data
{
    public interface IFluidityRepository
    {
        object Get(object id);

        IEnumerable<object> GetAll();

        PagedResult<object> GetPaged(int pageNumber, int pageSize, string orderBy, string orderDirection, string filter);

        object Save(object entity);

        void Delete(object[] ids);
    }
}