using System;
using System.Collections.Generic;
using Fluidity.Collections;

namespace Fluidity.Configuration
{
    public class FluidityTreeConfig : FluidityContainerTreeItemConfig
    {
        protected bool _initialize;
        internal bool Initialize => _initialize;

        protected ConcurrentDictionary<string, FluidityTreeItemConfig> _flattenedTreeItems;
        internal IReadOnlyDictionary<string, FluidityTreeItemConfig> FalttenedTreeItems => _flattenedTreeItems;

        public FluidityTreeConfig(string name, string icon = null, Action<FluidityTreeConfig> config = null)
            : base(name, icon)
        {
            _path = "-1";
            _initialize = true;

            _flattenedTreeItems = new ConcurrentDictionary<string, FluidityTreeItemConfig>();

            config?.Invoke(this);
        }

        public FluidityTreeConfig SetAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        internal void PostProcess()
        {
            PostProcessTreeItemsRecursive(TreeItems.Values, this);
        }

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