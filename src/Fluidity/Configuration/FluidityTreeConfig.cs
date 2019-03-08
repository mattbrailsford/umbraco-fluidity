// <copyright file="FluidityTreeConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using Fluidity.Collections;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity tree configuration
    /// </summary>
    /// <seealso cref="Fluidity.Configuration.FluidityContainerTreeItemConfig" />
    public class FluidityTreeConfig : FluidityContainerTreeItemConfig
    {
        protected bool _initialize;
        internal bool Initialize => _initialize;

        protected ConcurrentDictionary<string, FluidityTreeItemConfig> _flattenedTreeItems;
        internal IReadOnlyDictionary<string, FluidityTreeItemConfig> FlattenedTreeItems => _flattenedTreeItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityTreeConfig"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="config">A configuration delegate.</param>
        public FluidityTreeConfig(string name, string icon = null, Action<FluidityTreeConfig> config = null)
            : base(name, icon)
        {
            _path = "-1";
            _initialize = true;

            _flattenedTreeItems = new ConcurrentDictionary<string, FluidityTreeItemConfig>();

            config?.Invoke(this);
        }

        /// <summary>
        /// Changes the alias of the tree.
        /// </summary>
        /// <remarks>
        /// An alias will automatically be generated from the tree name however you can use SetAlias to change this if required
        /// </remarks>
        /// <param name="alias">The alias.</param>
        /// <returns>The tree configuration.</returns>
        public FluidityTreeConfig SetAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        /// <summary>
        /// Performs any post processing needed after initialization
        /// </summary>
        internal void PostProcess()
        {
            PostProcessTreeItemsRecursive(TreeItems.Values, this);
        }

        /// <summary>
        /// Post processes the tree items recursively.
        /// </summary>
        /// <param name="configs">The configs.</param>
        /// <param name="parentConfig">The parent configuration.</param>
        private void PostProcessTreeItemsRecursive(IEnumerable<FluidityTreeItemConfig> configs, FluidityTreeItemConfig parentConfig)
        {
            foreach (var config in configs)
            {
                // Update the paths
                config.UpdatePaths(parentConfig.Path);

                // Add item to flattened tree items collection
                _flattenedTreeItems.AddOrUpdate(config.Alias, config, (s, config2) => { throw new ApplicationException($"A tree item with the alias '{config2.Alias}' has already been added"); });

                // Recurse
                var folderConfig = config as FluidityContainerTreeItemConfig;
                if (folderConfig != null)
                {
                    PostProcessTreeItemsRecursive(folderConfig.TreeItems.Values, config);
                }
            }
        }

        // HideFromSideBar?
        // IsHidden?
    }
}