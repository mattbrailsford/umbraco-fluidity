// <copyright file="FluidityConfigModule.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Umbraco.Core;

namespace Fluidity.Configuration
{
    public abstract class FluidityConfigModule : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            FluidityBootManager.FluidityStarting += (sender, args) => {
                Configure(args.Config);
            };
        }

        /// <summary>
        /// The entry point for a Fluidity configuration.
        /// </summary>
        /// <param name="config">The base Fluidity configuration.</param>
        public abstract void Configure(FluidityConfig config);
    }
}
