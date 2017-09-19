using System.Collections.Generic;
using System.Linq;
using Fluidity.Actions;
using Fluidity.ListViewLayouts;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Un typed base class for a <see cref="FluidityListViewConfig{TEntityType}"/>
    /// </summary>
    public abstract class FluidityListViewConfig
    {
        protected int _pageSize;
        internal int PageSize => _pageSize;

        protected List<FluidityBulkAction> _defaultBulkActions;
        protected List<FluidityBulkAction> _bulkActions;
        internal IEnumerable<FluidityBulkAction> BulkActions => _bulkActions.Concat(_defaultBulkActions);

        protected List<FluidityDataViewConfig> _dataViews;
        internal IEnumerable<FluidityDataViewConfig> DataViews => _dataViews; 

        protected List<FluidityListViewFieldConfig> _fields;
        internal IEnumerable<FluidityListViewFieldConfig> Fields => _fields;

        protected List<FluidityListViewLayout> _defaultLayouts;
        protected List<FluidityListViewLayout> _layouts;
        internal IEnumerable<FluidityListViewLayout> Layouts => _layouts.Any() ? _layouts : _defaultLayouts;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityListViewConfig"/> class.
        /// </summary>
        protected FluidityListViewConfig()
        {
            _pageSize = 20;
            _defaultBulkActions = new List<FluidityBulkAction>(new [] { new FluidityDeleteBulkAction() });
            _bulkActions = new List<FluidityBulkAction>();
            _dataViews = new List<FluidityDataViewConfig>();
            _fields = new List<FluidityListViewFieldConfig>();
            _defaultLayouts = new List<FluidityListViewLayout>(new FluidityListViewLayout [] { new FluidityTableListViewLayout(), new FluidityGridListViewLayout() } );
            _layouts = new List<FluidityListViewLayout>();
        }
    }
}