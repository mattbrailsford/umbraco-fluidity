// <copyright file="FluiditySectionConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using Umbraco.Core;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity section configuration
    /// </summary>
    public class FluiditySectionConfig
    {
        protected string _alias;
        internal string Alias => _alias;

        protected string _name;
        internal string Name => _name;

        protected string _icon;
        internal string Icon => _icon;

        protected FluidityTreeConfig _tree;
        internal FluidityTreeConfig Tree => _tree;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluiditySectionConfig"/> class.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="icon">The icon of the section.</param>
        /// <param name="config">A configuration delgate</param>
        public FluiditySectionConfig(string sectionName, string icon = null, Action<FluiditySectionConfig> config = null)
        {
            _alias = sectionName.ToSafeAlias(true);
            _name = sectionName;
            _icon = icon ?? "icon-server-alt";

            config?.Invoke(this);
        }

        /// <summary>
        /// Changes the alias of the section.
        /// </summary>
        /// <remarks>
        /// An alias will automatically be generated from the section name however you can use SetAlias to change this if required
        /// </remarks>
        /// <param name="alias">The alias.</param>
        /// <returns>The section configuration.</returns>
        public FluiditySectionConfig SetAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        /// <summary>
        /// Sets the tree for the section.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="treeConfig">The tree configuration.</param>
        /// <returns>The tree configuration.</returns>
        public FluidityTreeConfig SetTree(string name, Action<FluidityTreeConfig> treeConfig = null)
        {
            return SetTree(new FluidityTreeConfig(name, _icon, treeConfig));
        }

        /// <summary>
        /// Sets the tree for the section.
        /// </summary>
        /// <param name="treeConfig">The tree configuration.</param>
        /// <returns>The tree configuration.</returns>
        public FluidityTreeConfig SetTree(FluidityTreeConfig treeConfig)
        {
            _tree = treeConfig;
            return Tree;
        }

        /// <summary>
        /// Performs any post processing needed after initialization
        /// </summary>
        internal void PostProcess()
        {
            _tree?.PostProcess();
        }
    }
}