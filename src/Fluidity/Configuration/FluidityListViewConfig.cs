using System;
using System.Collections.Generic;
using System.Linq;
using Fluidity.ListViewLayouts;

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

        protected int _pageSize;
        internal int PageSize => _pageSize;

        protected List<FluidityDataViewConfig> _dataViews;
        internal IEnumerable<FluidityDataViewConfig> DataViews => _dataViews; 

        protected Func<object, string> _nameFormat;
        internal Func<object, string> NameFormat => _nameFormat;

        protected List<FluidityListViewFieldConfig> _fields;
        internal IEnumerable<FluidityListViewFieldConfig> Fields => _fields;

        protected List<FluidityListViewSearchFieldConfig> _searchFields;
        internal IEnumerable<FluidityListViewSearchFieldConfig> SearchFields => _searchFields;

        protected List<FluidityListViewLayout> _defaultLayouts;
        protected List<FluidityListViewLayout> _layouts;
        internal IEnumerable<FluidityListViewLayout> Layouts => _layouts.Any() ? _layouts : _defaultLayouts;

        protected FluidityListViewConfig()
        {
            _menuActions = new List<IFluidityMenuAction>();
            _filters = new List<IFluidityFilter>();
            _bulkActions = new List<IFluidityBulkAction>();
            _dataViews = new List<FluidityDataViewConfig>();
            _fields = new List<FluidityListViewFieldConfig>();
            _searchFields = new List<FluidityListViewSearchFieldConfig>();
            _defaultLayouts = new List<FluidityListViewLayout>(new FluidityListViewLayout [] { new FluidityTableListViewLayout(), new FluidityGridListViewLayout() } );
            _layouts = new List<FluidityListViewLayout>();

            _pageSize = 20;
        }
    }
}