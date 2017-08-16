using System;
using System.Linq.Expressions;

namespace Fluidity.Configuration
{
    public class FluidityTabConfig<TEntityType> : FluidityTabConfig
    {
        public FluidityTabConfig(string name, Action<FluidityTabConfig<TEntityType>> config = null)
            : base (name)
        {
            config?.Invoke(this);
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> AddField<TValueType>(Expression<Func<TEntityType, TValueType>> fieldExpression, Action<FluidityEditorFieldConfig<TEntityType, TValueType>> fieldConfig = null)
        {
            return AddField(new FluidityEditorFieldConfig<TEntityType, TValueType>(fieldExpression, fieldConfig));
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> AddField<TValueType>(FluidityEditorFieldConfig<TEntityType, TValueType> fieldConfig)
        {
            var field = fieldConfig;
            _fields.Add(field);
            return field;
        }
    }
}