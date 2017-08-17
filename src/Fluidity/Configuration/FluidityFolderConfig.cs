using System;

namespace Fluidity.Configuration
{
    public class FluidityFolderConfig : FluidityContainerTreeItemConfig
    {
        protected FluidityViewMode _viewMode;
        internal FluidityViewMode ViewMode => _viewMode;

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

        public FluidityFolderConfig SetViewMode(FluidityViewMode viewMode)
        {
            _viewMode = viewMode;
            return this;
        }
    }
}