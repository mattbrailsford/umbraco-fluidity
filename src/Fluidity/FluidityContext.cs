using System;
using Fluidity.Configuration;
using Fluidity.Services;

namespace Fluidity
{
    internal class FluidityContext
    {
        internal static FluidityContext Current { get; set; }

        internal FluidityConfig Config { get; }

        internal FluidityServiceContext Services { get; }

        internal FluidityContext(FluidityConfig config,
            FluidityServiceContext services)
        {
            Config = config;
            Services = services;
        }

        internal static FluidityContext EnsureContext(FluidityConfig config,
            FluidityServiceContext services,
            bool replaceContext = false)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (services == null) throw new ArgumentNullException(nameof(services));

            // if there's already a singleton, and we're not replacing then there's no need to ensure anything
            if (Current != null)
            {
                if (replaceContext == false)
                    return Current;
            }

            var fluidityContext = CreateContext(config, services);

            // assign the singleton
            Current = fluidityContext;

            return Current;
        }

        internal static FluidityContext CreateContext(FluidityConfig config, 
            FluidityServiceContext services)
        {
            return new FluidityContext(config, services);
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
