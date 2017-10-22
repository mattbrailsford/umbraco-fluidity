// <copyright file="DefaultFluidityDataViewBuilder.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fluidity.Configuration;
using Fluidity.Models;
using Umbraco.Core;

namespace Fluidity.DataViewBuilders
{
    internal class DefaultFluidityDataViewBuilder : FluidityDataViewBuilder
    {
        protected FluidityListViewConfig Config { get; }

        internal DefaultFluidityDataViewBuilder(FluidityListViewConfig cfg)
        {
            Config = cfg;
        }

        public override bool HasDataViews => Config.DataViews.Any();

        public override IEnumerable<FluidityDataViewSummary> GetDataViews()
        {
            return Config.DataViews.Select(x => new FluidityDataViewSummary
            {
                Name = x.Name,
                Alias = x.Alias
            });
        }

        public override LambdaExpression GetDataViewWhereClause(string dataViewAlias)
        {
            var dv = Config.DataViews.FirstOrDefault(x => x.Alias.InvariantEquals(dataViewAlias));
            return dv?.WhereClauseExpression;
        }
    }
}
