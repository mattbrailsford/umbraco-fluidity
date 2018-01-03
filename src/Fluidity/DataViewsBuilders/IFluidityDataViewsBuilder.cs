using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fluidity.Models;

namespace Fluidity.DataViewsBuilders
{
    [Obsolete("Use the abstract class FluidityDataViewsBuilder<TEntityType> instead")]
    public interface IFluidityDataViewsBuilder
    {
        IEnumerable<FluidityDataViewSummary> GetDataViews();

        LambdaExpression GetDataViewWhereClause(string dataViewAlias);
    }
}