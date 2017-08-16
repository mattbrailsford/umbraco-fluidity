using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Fluidity.Configuration.Mappers;
using Fluidity.Extensions;

namespace Fluidity.Configuration
{
    public class FluidityEditorFieldConfig<TEntityType, TValueType> : FluidityEditorFieldConfig
    {
        public FluidityEditorFieldConfig(Expression<Func<TEntityType, TValueType>> property, Action<FluidityEditorFieldConfig<TEntityType, TValueType>> config = null)
            : base(property.GetPropertyInfo())
        {
            config?.Invoke(this);
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> SetLabel(string label)
        {
            _label = label;
            return this;
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> SetDescription(string description)
        {
            _description = description;
            return this;
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> SetValidationRegex(string regex)
        {
            _validationRegex = regex;
            return this;
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> SetValidationRegex(Regex regex)
        {
            _validationRegex = regex.ToString();
            return this;
        }

        public new FluidityEditorFieldConfig<TEntityType, TValueType> IsRequired()
        {
            _isRequired = true;
            return this;
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> SetDataType(string name)
        {
            _dataTypeName = name;
            return this;
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> SetDataType(int id)
        {
            _dataTypeId = id;
            return this;
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> SetMapper<TMapperType>()
            where TMapperType : FluidityValueMapper, new()
        {
            _mapper = new TMapperType();
            return this;
        }
    }
}