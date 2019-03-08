// <copyright file="FluidityRepositoryFactory.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using Fluidity.Configuration;

namespace Fluidity.Data
{
    internal class FluidityRepositoryFactory
    {
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
