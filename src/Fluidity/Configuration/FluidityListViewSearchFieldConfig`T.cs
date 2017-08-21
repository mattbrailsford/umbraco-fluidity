using Fluidity.Extensions;
using System;
using System.Linq.Expressions;

namespace Fluidity.Configuration
{
    public class FluidityListViewSearchFieldConfig<TEntityType> : FluidityListViewSearchFieldConfig
    {
        public FluidityListViewSearchFieldConfig(Expression<Func<TEntityType, string>> property)
            : base(property, property.GetPropertyInfo())
        { }
    }
}
