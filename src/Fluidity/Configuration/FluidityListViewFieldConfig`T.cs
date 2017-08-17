using System;
using System.Linq.Expressions;
using Fluidity.Extensions;

namespace Fluidity.Configuration
{
    public class FluidityListViewFieldConfig<TEntityType, TValueType> : FluidityListViewFieldConfig
    {
        public FluidityListViewFieldConfig(Expression<Func<TEntityType, TValueType>> property, Action<FluidityListViewFieldConfig<TEntityType, TValueType>> config = null)
            : base (property.GetPropertyInfo())
        {
            config?.Invoke(this);
        }

        public FluidityListViewFieldConfig<TEntityType, TValueType> SetHeading(string heading)
        {
            _heading = heading;
            return this;
        }

        public FluidityListViewFieldConfig<TEntityType, TValueType> SetFormat(Func<TValueType, object> format)
        {
            _format = (value, entity) => format((TValueType)value);
            return this;
        }

        public FluidityListViewFieldConfig<TEntityType, TValueType> SetFormat(Func<TValueType, TEntityType, object> format)
        {
            _format = (value, entity) => format((TValueType)value, (TEntityType)entity);
            return this;
        }
    }
}