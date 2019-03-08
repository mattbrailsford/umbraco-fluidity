// <copyright file="FluidityDataViewConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Linq.Expressions;
using Umbraco.Core;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity data view configuration
    /// </summary>
    public class FluidityDataViewConfig
    {
        protected string _alias;
        internal string Alias => _alias;

        protected string _name;
        internal string Name => _name;

        protected LambdaExpression _whereClauseExpression;
        internal LambdaExpression WhereClauseExpression => _whereClauseExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityDataViewConfig"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="whereClauseExpression">The where clause expression.</param>
        public FluidityDataViewConfig(string name, LambdaExpression whereClauseExpression)
        {
            _alias = name.ToSafeAlias(true);
            _name = name;
            _whereClauseExpression = whereClauseExpression;
        }
    }
}
