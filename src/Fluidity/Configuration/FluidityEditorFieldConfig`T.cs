// <copyright file="FluidityEditorFieldConfig`T.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Fluidity.Mappers;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity editor field configiguration
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity.</typeparam>
    /// <typeparam name="TValueType">The type of the property value.</typeparam>
    /// <seealso cref="Fluidity.Configuration.FluidityEditorFieldConfig" />
    public class FluidityEditorFieldConfig<TEntityType, TValueType> : FluidityEditorFieldConfig
    {
            private bool _isReadonly = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityEditorFieldConfig{TEntityType, TValueType}"/> class.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="config">A configuration delegate.</param>
        public FluidityEditorFieldConfig(Expression<Func<TEntityType, TValueType>> propertyExpression, Action<FluidityEditorFieldConfig<TEntityType, TValueType>> config = null)
            : base(propertyExpression)
        {
            config?.Invoke(this);
        }

        /// <summary>
        /// Changes the label of the field.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetLabel(string label)
        {
            _label = label;
            return this;
        }

        /// <summary>
        /// Sets the description of the field to display below the label.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetDescription(string description)
        {
            _description = description;
            return this;
        }

        /// <summary>
        /// Sets a validation regex for the field.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetValidationRegex(string regex)
        {
            if(!_isReadonly)
                _validationRegex = regex;
            return this;
        }

        /// <summary>
        /// Sets a validation regex for the field.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetValidationRegex(Regex regex)
        {
            if(!_isReadonly)
                _validationRegex = regex.ToString();
            return this;
        }

        /// <summary>
        /// Sets this field as required.
        /// </summary>
        /// <returns>The editor field configuration.</returns>
        public new FluidityEditorFieldConfig<TEntityType, TValueType> IsRequired()
        {
            _isRequired = true;
            return this;
        }

        /// <summary>
        /// Sets the Umbraco data type of the field.
        /// </summary>
        /// <param name="name">The data type name.</param>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetDataType(string name)
        {
            if(!_isReadonly)
                _dataTypeName = name;
            return this;
        }

        /// <summary>
        /// Sets the Umbraco data type of the field.
        /// </summary>
        /// <param name="id">The data type identifier.</param>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetDataType(int id)
        {
            if (!_isReadonly)
                _dataTypeId = id;
            return this;
        }

        /// <summary>
        /// Sets the field value mapper.
        /// </summary>
        /// <typeparam name="TValueMapperType">The type of the value mapper.</typeparam>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetValueMapper<TValueMapperType>()
            where TValueMapperType : FluidityValueMapper, new()
        {
            if(!_isReadonly)
                _valueMapper = new TValueMapperType();
            return this;
        }

        /// <summary>
        /// Sets the field value mapper.
        /// </summary>
        /// <param name="valueMapper">The value mapper.</param>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetValueMapper(FluidityValueMapper valueMapper)
        {
            if(!_isReadonly)
                _valueMapper = valueMapper;
            return this;
        }

        /// <summary>
        /// Sets the field default value.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetDefaultValue(TValueType defaultValue)
        {
            _defaultValueFunc = () => defaultValue;
            return this;
        }

        /// <summary>
        /// Sets the field default value via a function for dynamic values.
        /// </summary>
        /// <param name="defaultValueFunc">The default value function.</param>
        /// <returns>The editor field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> SetDefaultValue(Func<TValueType> defaultValueFunc)
        {
            _defaultValueFunc = () => defaultValueFunc();
            return this;
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> IsReadonly()
        {
            //TODO: Create defaults for different primitives
            return IsReadonly(type => type.ToString());
        }

        public FluidityEditorFieldConfig<TEntityType, TValueType> IsReadonly(Func<TValueType, string> format) {
            _valueMapper = new ReadOnlyValueMapper(value => format((TValueType)value));
            _dataTypeId = -92;
            _isReadonly = true;
            return this;
        }
    }
}