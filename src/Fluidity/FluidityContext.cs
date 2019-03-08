// <copyright file="FluidityContext.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using Fluidity.Configuration;
using Fluidity.Services;
using Fluidity.Data;

namespace Fluidity
{
    internal class FluidityContext
    {
        internal static FluidityContext Current { get; set; }

        internal FluidityConfig Config { get; }

        internal FluidityDataContext Data { get; }

        internal FluidityServiceContext Services { get; }

        internal FluidityContext(FluidityConfig config,
            FluidityDataContext data,
            FluidityServiceContext services)
        {
            Config = config;
            Data = data;
            Services = services;
        }

        internal static FluidityContext EnsureContext(FluidityConfig config,
            FluidityDataContext data,
            FluidityServiceContext services,
            bool replaceContext = false)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (services == null) throw new ArgumentNullException(nameof(services));

            // if there's already a singleton, and we're not replacing then there's no need to ensure anything
            if (Current != null)
            {
                if (replaceContext == false)
                    return Current;
            }

            var fluidityContext = new FluidityContext(config, data, services);

            // assign the singleton
            Current = fluidityContext;

            return Current;
        }
    }

    internal class FluidityDataContext
    {
        public FluidityRepositoryFactory RepositoryFactory { get; }

        internal FluidityDataContext(FluidityRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory;
        }
    }

    internal class FluidityServiceContext
    {
        public FluidityEntityService EntityService { get; }

        internal FluidityServiceContext(FluidityEntityService entityService)
        {
            EntityService = entityService;
        }
    }
}
