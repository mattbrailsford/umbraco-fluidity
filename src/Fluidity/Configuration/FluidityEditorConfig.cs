using System.Collections.Generic;

namespace Fluidity.Configuration
{
    public abstract class FluidityEditorConfig
    {
        protected List<FluidityTabConfig> _tabs;
        internal IEnumerable<FluidityTabConfig> Tabs => _tabs;

        protected FluidityEditorConfig()
        {
            _tabs = new List<FluidityTabConfig>();
        }
    }
}