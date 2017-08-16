using System;
using System.Linq.Expressions;

namespace Fluidity.Configuration
{
    public class FluidityListViewConfig<TEntityType> : FluidityListViewConfig
    {
        public FluidityListViewConfig(Action<FluidityListViewConfig<TEntityType>> config = null)
        {
            config?.Invoke(this);
        }

        public FluidityListViewConfig<TEntityType> AddMenuAction<TActionType>()
            where TActionType : IFluidityMenuAction, new()
        {
            _menuActions.Add(new TActionType());
            return this;
        }

        public FluidityListViewConfig<TEntityType> AddFilter<TFilterType>()
            where TFilterType : IFluidityFilter, new()
        {
            _filters.Add(new TFilterType());
            return this;
        }

        public FluidityListViewConfig<TEntityType> AddBulkAction<TBulkActionType>()
            where TBulkActionType : IFluidityBulkAction, new()
        {
            _bulkActions.Add(new TBulkActionType());
            return this;
        }

        public FluidityListViewFieldConfig<TEntityType, TValueType> AddField<TValueType>(Expression<Func<TEntityType, TValueType>> fieldExpression, Action<FluidityListViewFieldConfig<TEntityType, TValueType>> fieldConfig = null)
        {
            return AddField(new FluidityListViewFieldConfig<TEntityType, TValueType>(fieldExpression, fieldConfig));
        }

        public FluidityListViewFieldConfig<TEntityType, TValueType> AddField<TValueType>(FluidityListViewFieldConfig<TEntityType, TValueType> fieldConfig)
        {
            var field = fieldConfig;
            _fields.Add(field);
            return field;
        }
    }
}