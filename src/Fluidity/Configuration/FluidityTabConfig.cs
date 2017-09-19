using System.Collections.Generic;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Un typed base class for a <see cref="FluidityTabConfig{TEntityType}"/>
    /// </summary>
    public abstract class FluidityTabConfig
    {
        protected string _name;
        internal string Name => _name;

        protected List<FluidityEditorFieldConfig> _fields;
        internal IEnumerable<FluidityEditorFieldConfig> Fields => _fields;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityTabConfig"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected FluidityTabConfig(string name)
        {
            _name = name;
            _fields = new List<FluidityEditorFieldConfig>();
        }
    }
}