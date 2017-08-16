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

        //public FluidityListViewFieldConfig<TEntityType> SetView<TViewType>()
        //    where TViewType : FluidityView, new()
        //{
        //    _view = new TViewType();
        //    return this;
        //}

        //public FluidityListViewFieldConfig<TEntityType> SetView<TViewType, TConfigType>(Action<TConfigType> viewConfig = null)
        //    where TViewType : FluidityView<TConfigType>, new()
        //    where TConfigType : new()
        //{
        //    if (typeof(TConfigType).Name != typeof(TViewType).Name + "Config")
        //    {
        //        throw new ArgumentException("Config type name must be the same as the view type name suffixed with 'Config', ie the config type name for the view type FluidityLabelView must be FluidityLabelViewConfig");
        //    }

        //    var config = new TConfigType();
        //    viewConfig?.Invoke(config);

        //    return SetView<TViewType, TConfigType>(config);
        //}

        //public FluidityListViewFieldConfig<TEntityType> SetView<TViewType, TConfigType>(TConfigType viewConfig)
        //    where TViewType : FluidityView<TConfigType>, new()
        //    where TConfigType : new()
        //{
        //    if (typeof(TConfigType).Name != typeof(TViewType).Name + "Config")
        //    {
        //        throw new ArgumentException("Config type name must be the same as the view type name suffixed with 'Config', ie the config type name for the view type FluidityLabelView must be FluidityLabelViewConfig");
        //    }

        //    _view = new TViewType { Config = viewConfig };

        //    return this;
        //}
    }
}