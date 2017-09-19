using System.Collections.Generic;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Un typed base class  for a <see cref="FluidityEditorConfig{TEntityType}"/>
    /// </summary>
    public abstract class FluidityEditorConfig
    {
        protected List<FluidityTabConfig> _tabs;
        internal IEnumerable<FluidityTabConfig> Tabs => _tabs;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityEditorConfig"/> class.
        /// </summary>
        protected FluidityEditorConfig()
        {
            _tabs = new List<FluidityTabConfig>();
        }
    }
}