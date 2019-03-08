// <copyright file="FluidityListViewFieldConfig`T.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq.Expressions;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity list view field configuration
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity type.</typeparam>
    /// <typeparam name="TValueType">The type of the value type.</typeparam>
    /// <seealso cref="Fluidity.Configuration.FluidityListViewFieldConfig" />
    public class FluidityListViewFieldConfig<TEntityType, TValueType> : FluidityListViewFieldConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityListViewFieldConfig{TEntityType, TValueType}"/> class.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="config">The configuration.</param>
        public FluidityListViewFieldConfig(Expression<Func<TEntityType, TValueType>> propertyExpression, Action<FluidityListViewFieldConfig<TEntityType, TValueType>> config = null)
            : base (propertyExpression)
        {
            config?.Invoke(this);
        }

        /// <summary>
        /// Changes the heading of the list view field.
        /// </summary>
        /// <param name="heading">The heading.</param>
        /// <returns>The list view field configuration.</returns>
        public FluidityListViewFieldConfig<TEntityType, TValueType> SetHeading(string heading)
        {
            _heading = heading;
            return this;
        }

        /// <summary>
        /// Sets a format function for the list view field.
        /// </summary>
        /// <param name="format">The format function.</param>
        /// <returns>The list view field configuration.</returns>
        public FluidityListViewFieldConfig<TEntityType, TValueType> SetFormat(Func<TValueType, object> format)
        {
            _format = (value, entity) => format((TValueType)value);
            return this;
        }

        /// <summary>
        /// Sets a format function for the list view field.
        /// </summary>
        /// <param name="format">The format function.</param>
        /// <returns>The list view field configuration.</returns>
        public FluidityListViewFieldConfig<TEntityType, TValueType> SetFormat(Func<TValueType, TEntityType, object> format)
        {
            _format = (value, entity) => format((TValueType)value, (TEntityType)entity);
            return this;
        }
    }
}
