// <copyright file="DefaultFluidityDataViewBuilder.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fluidity.Configuration;
using Fluidity.Models;
using Umbraco.Core;

namespace Fluidity.DataViewsBuilders
{
    internal class DefaultFluidityDataViewsBuilder : IFluidityDataViewsBuilder
    {
        protected FluidityListViewConfig Config { get; }

        internal DefaultFluidityDataViewsBuilder(FluidityListViewConfig cfg)
        {
            Config = cfg;
        }

        public IEnumerable<FluidityDataViewSummary> GetDataViews()
        {
            return Config.DataViews.Select(x => new FluidityDataViewSummary
            {
                Name = x.Name,
                Alias = x.Alias
            });
        }

        public LambdaExpression GetDataViewWhereClause(string dataViewAlias)
        {
            var dv = Config.DataViews.FirstOrDefault(x => x.Alias.InvariantEquals(dataViewAlias));
            return dv?.WhereClauseExpression;
        }
    }
}
