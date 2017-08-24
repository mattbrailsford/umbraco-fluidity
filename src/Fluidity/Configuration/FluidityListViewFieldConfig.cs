using System;
using System.Linq.Expressions;

namespace Fluidity.Configuration
{
    public abstract class FluidityListViewFieldConfig
    {
        protected FluidityPropertyConfig _property;
        internal FluidityPropertyConfig Property => _property;

        protected string _heading;
        internal string Heading => _heading;

        protected Func<object, object, object> _format;
        internal Func<object, object, object> Format => _format;

        protected FluidityListViewFieldConfig(LambdaExpression propertyExp)
        {
            _property = propertyExp;
        }
    }
}