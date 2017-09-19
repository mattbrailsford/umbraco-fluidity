using System;
using System.Linq.Expressions;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Un typed base class for a <see cref="FluidityListViewFieldConfig{TEntityType, TValueType}"/>
    /// </summary>
    public abstract class FluidityListViewFieldConfig
    {
        protected FluidityPropertyConfig _property;
        internal FluidityPropertyConfig Property => _property;

        protected string _heading;
        internal string Heading => _heading;

        protected Func<object, object, object> _format;
        internal Func<object, object, object> Format => _format;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityListViewFieldConfig"/> class.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        protected FluidityListViewFieldConfig(LambdaExpression propertyExpression)
        {
            _property = propertyExpression;
        }
    }
}