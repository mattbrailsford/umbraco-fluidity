using System;
using System.Linq.Expressions;
using System.Reflection;
using Fluidity.Extensions;

namespace Fluidity.Configuration
{
    public class FluidityPropertyConfig
    {
        protected PropertyInfo _propertyInfo;
        internal PropertyInfo PropertyInfo => _propertyInfo;

        protected LambdaExpression _propertyExp;
        internal LambdaExpression PropertyExpression => _propertyExp;

        internal string Name => _propertyInfo.Name;

        internal Type Type => _propertyInfo.PropertyType;

        public FluidityPropertyConfig(LambdaExpression propertyExp)
        {
            _propertyExp = propertyExp;
            _propertyInfo = propertyExp.GetPropertyInfo();
        }

        public static implicit operator PropertyInfo(FluidityPropertyConfig propertyConfig)
        {
            return propertyConfig.PropertyInfo;
        }

        public static implicit operator LambdaExpression(FluidityPropertyConfig propertyConfig)
        {
            return propertyConfig.PropertyExpression;
        }

        public static implicit operator FluidityPropertyConfig(LambdaExpression propertyExp)
        {
            return new FluidityPropertyConfig(propertyExp);
        }
    }
}
