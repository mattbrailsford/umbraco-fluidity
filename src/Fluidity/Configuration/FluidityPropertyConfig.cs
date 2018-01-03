// <copyright file="FluidityPropertyConfig.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq.Expressions;
using System.Reflection;
using Fluidity.Extensions;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity property configuration
    /// </summary>
    public class FluidityPropertyConfig
    {
        protected PropertyInfo _propertyInfo;
        internal PropertyInfo PropertyInfo => _propertyInfo;

        protected LambdaExpression _propertyExp;
        internal LambdaExpression PropertyExpression => _propertyExp;

        internal string Name => _propertyInfo.Name;

        internal Type Type => _propertyInfo.PropertyType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityPropertyConfig"/> class.
        /// </summary>
        /// <param name="propertyExp">The property exp.</param>
        public FluidityPropertyConfig(LambdaExpression propertyExp)
        {
            _propertyExp = propertyExp;
            _propertyInfo = propertyExp.GetPropertyInfo();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="FluidityPropertyConfig"/> to <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyConfig">The property configuration.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator PropertyInfo(FluidityPropertyConfig propertyConfig)
        {
            return propertyConfig.PropertyInfo;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="FluidityPropertyConfig"/> to <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="propertyConfig">The property configuration.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator LambdaExpression(FluidityPropertyConfig propertyConfig)
        {
            return propertyConfig.PropertyExpression;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="LambdaExpression"/> to <see cref="FluidityPropertyConfig"/>.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator FluidityPropertyConfig(LambdaExpression propertyExpression)
        {
            return new FluidityPropertyConfig(propertyExpression);
        }
    }
}
