// <copyright file="FluidityEditorFieldConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Linq.Expressions;
using System;
using Fluidity.ValueMappers;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Un typed base class  for a <see cref="FluidityEditorFieldConfig{TEntityType, TValueType}"/>
    /// </summary>
    public abstract class FluidityEditorFieldConfig
    {
        protected FluidityPropertyConfig _property;
        internal FluidityPropertyConfig Property => _property;
        
        protected string _label;
        internal string Label => _label;

        protected string _description;
        internal string Description => _description;

        protected bool _isRequired;
        internal bool IsRequired => _isRequired;

        protected string _validationRegex;
        internal string ValidationRegex => _validationRegex;

        protected string _dataTypeName;
        internal string DataTypeName => _dataTypeName;

        protected int _dataTypeId;
        internal int DataTypeId => _dataTypeId;

        protected FluidityValueMapper _valueMapper;
        internal FluidityValueMapper ValueMapper => _valueMapper;

        protected Func<object> _defaultValueFunc;
        internal Func<object> DefaultValueFunc => _defaultValueFunc;

	      protected bool _isReadOnly;
	      internal bool IsReadOnly => _isReadOnly;

		    /// <summary>
		    /// Initializes a new instance of the <see cref="FluidityEditorFieldConfig"/> class.
		    /// </summary>
		    /// <param name="propertyExpression">The property exp.</param>
		    protected FluidityEditorFieldConfig(LambdaExpression propertyExpression)
        {
            _property = propertyExpression;
        }
    }
}
