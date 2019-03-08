// <copyright file="FluidityTabConfig`T.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq.Expressions;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity tab configuration
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity type.</typeparam>
    /// <seealso cref="Fluidity.Configuration.FluidityTabConfig" />
    public class FluidityTabConfig<TEntityType> : FluidityTabConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityTabConfig{TEntityType}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="config">A configuration delegate.</param>
        public FluidityTabConfig(string name, Action<FluidityTabConfig<TEntityType>> config = null)
            : base (name)
        {
            config?.Invoke(this);
        }

        /// <summary>
        /// Adds a field to the tab.
        /// </summary>
        /// <typeparam name="TValueType">The type of the value.</typeparam>
        /// <param name="fieldExpression">The field expression.</param>
        /// <param name="fieldConfig">A field configuration delegate.</param>
        /// <returns>The field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> AddField<TValueType>(Expression<Func<TEntityType, TValueType>> fieldExpression, Action<FluidityEditorFieldConfig<TEntityType, TValueType>> fieldConfig = null)
        {
            return AddField(new FluidityEditorFieldConfig<TEntityType, TValueType>(fieldExpression, fieldConfig));
        }

        /// <summary>
        /// Adds a field to the tab.
        /// </summary>
        /// <typeparam name="TValueType">The type of the value type.</typeparam>
        /// <param name="fieldConfig">The field configuration.</param>
        /// <returns>The field configuration.</returns>
        public FluidityEditorFieldConfig<TEntityType, TValueType> AddField<TValueType>(FluidityEditorFieldConfig<TEntityType, TValueType> fieldConfig)
        {
            var field = fieldConfig;
            _fields.Add(field);
            return field;
        }
    }
}