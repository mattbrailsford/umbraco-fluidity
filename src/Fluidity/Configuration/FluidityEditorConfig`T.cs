using System;
using System.Linq.Expressions;
using Fluidity.Extensions;

namespace Fluidity.Configuration
{
    public class FluidityEditorConfig<TEntityType> : FluidityEditorConfig
    {
        public FluidityEditorConfig(Action<FluidityEditorConfig<TEntityType>> config = null)
        {
            config?.Invoke(this);
        }

        public FluidityEditorConfig<TEntityType> SetNameField(Expression<Func<TEntityType, object>> nameProperty)
        {
            _namePropertyExp = nameProperty;
            _nameProperty = nameProperty.GetPropertyInfo();
            return this;
        }

        public FluidityTabConfig<TEntityType> AddTab(string name, Action<FluidityTabConfig<TEntityType>> tabConfig = null)
        {
            return AddTab(new FluidityTabConfig<TEntityType>(name, tabConfig));
        }

        public FluidityTabConfig<TEntityType> AddTab(FluidityTabConfig<TEntityType> tabConfig)
        {
            var tab = tabConfig;
            _tabs.Add(tab);
            return tab;
        }
    }
}