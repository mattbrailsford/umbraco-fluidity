using System;
using Fluidity.Configuration;

namespace Fluidity.Data
{
    internal class FluidityRepositoryFactory
    {
        internal FluidityRepositoryFactory()
        { }

        public IFluidityRepository GetRepository(FluidityCollectionConfig config)
        {
            var defaultRepoType = typeof(DefaultFluidityRepository);
            var repoType = config.RepositoryType ?? defaultRepoType;

            return defaultRepoType.IsAssignableFrom(repoType)
               ? (IFluidityRepository)Activator.CreateInstance(repoType, config)
               : (IFluidityRepository)Activator.CreateInstance(repoType);
        }
    }
}
