using System;

namespace Fluidity.Configuration
{
    public class FluidityFolderConfig : FluidityContainerTreeItemConfig
    {
        protected FluidityTreeMode _treeMode;
        internal FluidityTreeMode TreeMode => _treeMode;

        public FluidityFolderConfig(string name, string icon = null, Action<FluidityFolderConfig> config = null)
            : base(name, icon)
        {
            config?.Invoke(this);
        }

        public FluidityFolderConfig SetAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        public FluidityFolderConfig SetTreeMode(FluidityTreeMode treeMode)
        {
            _treeMode = treeMode;
            return this;
        }
    }
}