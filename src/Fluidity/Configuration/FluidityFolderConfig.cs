using System;

namespace Fluidity.Configuration
{
    public class FluidityFolderConfig : FluidityContainerTreeItemConfig
    {
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
    }
}