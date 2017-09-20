// <copyright file="FluidityDataView.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;

namespace Fluidity.DataViews
{
    public interface IFluidityDataView
    {
        string Name { get; }

        string GetWhereClause(object entity, out IEnumerable<object> parameters);
    }

    public abstract class FluidityDataView<TEntityType> : IFluidityDataView
    {
        public abstract string Name { get; }
        
        public abstract string GetWhereClause(TEntityType entity, out IEnumerable<object> parameters);

        string IFluidityDataView.GetWhereClause(object entity, out IEnumerable<object> parameters)
        {
            return GetWhereClause((TEntityType)entity, out parameters);
        }
    }
}
