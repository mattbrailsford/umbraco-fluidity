using System.Collections.Generic;

namespace Fluidity.Configuration
{
    public abstract class FluidityListViewConfig
    {
        protected List<IFluidityMenuAction> _menuActions;
        internal IEnumerable<IFluidityMenuAction> MenuActions => _menuActions;

        protected List<IFluidityFilter> _filters;
        internal IEnumerable<IFluidityFilter> Filters => _filters;

        protected List<IFluidityBulkAction> _bulkActions;
        internal IEnumerable<IFluidityBulkAction> BulkActions => _bulkActions;

        protected List<FluidityListViewFieldConfig> _fields;
        internal IEnumerable<FluidityListViewFieldConfig> Fields => _fields;

        protected FluidityListViewConfig()
        {
            _menuActions = new List<IFluidityMenuAction>();
            _filters = new List<IFluidityFilter>();
            _bulkActions = new List<IFluidityBulkAction>();
            _fields = new List<FluidityListViewFieldConfig>();
        }
    }
}