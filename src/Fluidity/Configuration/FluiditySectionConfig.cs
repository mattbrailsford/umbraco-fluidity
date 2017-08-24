using System;
using Umbraco.Core;

namespace Fluidity.Configuration
{
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

        public FluiditySectionConfig(string sectionName, string icon = null, Action<FluiditySectionConfig> config = null)
        {
            _alias = sectionName.ToSafeAlias(true);
            _name = sectionName;
            _icon = icon ?? "icon-server-alt";

            config?.Invoke(this);
        }

        public FluiditySectionConfig SetAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        public FluidityTreeConfig SetTree(string name, Action<FluidityTreeConfig> treeConfig = null)
        {
            return SetTree(new FluidityTreeConfig(name, config : treeConfig));
        }

        public FluidityTreeConfig SetTree(string name, string icon, Action<FluidityTreeConfig> treeConfig = null)
        {
            return SetTree(new FluidityTreeConfig(name, icon, treeConfig));
        }

        public FluidityTreeConfig SetTree(FluidityTreeConfig treeConfig)
        {
            _tree = treeConfig;
            return Tree;
        }

        internal void PostProcess()
        {
            _tree?.PostProcess();
        }
    }
}