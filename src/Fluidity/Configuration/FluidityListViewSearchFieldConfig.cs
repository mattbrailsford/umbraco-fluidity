using System.Linq.Expressions;
using System.Reflection;

namespace Fluidity.Configuration
{
    public abstract class FluidityListViewSearchFieldConfig
    {
        protected PropertyInfo _property;
        internal PropertyInfo Property => _property;

        protected LambdaExpression _propertyExp;
        internal LambdaExpression PropertyExp => _propertyExp;

        protected FluidityListViewSearchFieldConfig(LambdaExpression propertyExp, PropertyInfo property)
        {
            _propertyExp = propertyExp;
            _property = property;
        }
    }
}
