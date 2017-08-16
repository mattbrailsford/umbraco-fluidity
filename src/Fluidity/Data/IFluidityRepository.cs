using System.Collections.Generic;

namespace Fluidity.Data
{
    public interface IFluidityRepository
    {
        object Get(object id);

        IEnumerable<object> GetAll();

        object Save(object entity);

        void Delete(object[] ids);
    }
}