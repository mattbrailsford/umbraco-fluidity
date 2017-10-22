// <copyright file="FluidityDataViewBuilder.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;
using System.Linq.Expressions;
using Fluidity.Models;

namespace Fluidity.DataViewBuilders
{
    /// <summary>
    /// Fluidity Data View Builder
    /// </summary>
    public abstract class FluidityDataViewBuilder
    {
        /// <summary>
        /// Gets a value indicating whether this instance has any data views.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has data views; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasDataViews { get; }

        /// <summary>
        /// Gets the list of data views.
        /// </summary>
        /// <returns>A list of data views.</returns>
        public abstract IEnumerable<FluidityDataViewSummary> GetDataViews();

        /// <summary>
        /// Gets the where clause for the given data view.
        /// </summary>
        /// <param name="dataViewAlias">The data view alias.</param>
        /// <returns>A boolean lambda expression where clause.</returns>
        public abstract LambdaExpression GetDataViewWhereClause(string dataViewAlias);
    }
}
