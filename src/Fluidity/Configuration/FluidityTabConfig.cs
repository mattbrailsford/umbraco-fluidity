using System.Collections.Generic;

namespace Fluidity.Configuration
{
    public abstract class FluidityTabConfig
    {
        protected string _name;
        internal string Name => _name;

        protected List<FluidityEditorFieldConfig> _fields;
        internal IEnumerable<FluidityEditorFieldConfig> Fields => _fields;

        protected FluidityTabConfig(string name)
        {
            _name = name;
            _fields = new List<FluidityEditorFieldConfig>();
        }
    }
}