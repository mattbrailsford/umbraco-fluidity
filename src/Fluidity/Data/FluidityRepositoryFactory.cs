using System;
using Fluidity.Configuration;

namespace Fluidity.Data
{
    internal class FluidityRepositoryFactory
    {
        internal FluidityRepositoryFactory()
        { }

        public IFluidityRepository GetRepository(FluidityCollectionConfig collection)
        {
            var defaultRepoType = typeof(DefaultFluidityRepository);
            var repoType = collection.RepositoryType ?? defaultRepoType;

            return defaultRepoType.IsAssignableFrom(repoType)
               ? (IFluidityRepository)Activator.CreateInstance(repoType, collection)
               : (IFluidityRepository)Activator.CreateInstance(repoType);
        }
    }
}
