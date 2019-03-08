// <copyright file="FluidityConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using Fluidity.Collections;

namespace Fluidity.Configuration
{
    /// <summary>
    /// The base Fluidity configuration
    /// </summary>
    public class FluidityConfig
    {
        protected ConcurrentDictionary<string, FluiditySectionConfig> _sections;
        internal IReadOnlyDictionary<string, FluiditySectionConfig> Sections => _sections;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityConfig"/> class.
        /// </summary>
        public FluidityConfig()
        {
            _sections = new ConcurrentDictionary<string, FluiditySectionConfig>();
        }

        /// <summary>
        /// Adds a section to the configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sectionConfig">A section configuration delegate.</param>
        /// <returns>The section configuration.</returns>
        public FluiditySectionConfig AddSection(string name, Action<FluiditySectionConfig> sectionConfig = null)
        {
            return AddSection(new FluiditySectionConfig(name, config : sectionConfig));
        }

        /// <summary>
        /// Adds a section to the configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="sectionConfig">A section configuration delegate.</param>
        /// <returns></returns>
        public FluiditySectionConfig AddSection(string name, string icon, Action<FluiditySectionConfig> sectionConfig = null)
        {
            return AddSection(new FluiditySectionConfig(name, icon, sectionConfig));
        }

        /// <summary>
        /// Adds a section to the configuration.
        /// </summary>
        /// <param name="sectionConfig">The section configuration.</param>
        /// <returns>The section configuration.</returns>
        public FluiditySectionConfig AddSection(FluiditySectionConfig sectionConfig)
        {
            var section = sectionConfig;
            _sections.AddOrUpdate(section.Alias, section, (s, config) => config);
            return section;
        }

        /// <summary>
        /// Performs any post processing needed after initialization
        /// </summary>
        internal void PostProcess()
        {
            foreach (var section in _sections.Values)
            {
                section.PostProcess();
            }
        }
    }
}
