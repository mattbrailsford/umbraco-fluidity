// <copyright file="FluidityDataViewBuilder.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fluidity.Models;

namespace Fluidity.DataViewsBuilders
{
    /// <summary>
    /// Fluidity Data View Builder
    /// </summary>
    public abstract class FluidityDataViewsBuilder<TEntityType> : IFluidityDataViewsBuilder
    {
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
        public abstract Expression<Func<TEntityType, bool>> GetDataViewWhereClause(string dataViewAlias);

        /// <summary>
        /// Gets the where clause for the given data view.
        /// </summary>
        /// <param name="dataViewAlias">The data view alias.</param>
        /// <returns>A boolean lambda expression where clause.</returns>
        LambdaExpression IFluidityDataViewsBuilder.GetDataViewWhereClause(string dataViewAlias)
        {
            return GetDataViewWhereClause(dataViewAlias);
        }
    }
}
