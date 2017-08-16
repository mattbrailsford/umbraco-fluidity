using System;
using System.Reflection;

namespace Fluidity.Configuration
{
    public abstract class FluidityListViewFieldConfig
    {
        protected PropertyInfo _property;
        internal PropertyInfo Property => _property;

        protected string _heading;
        internal string Heading => _heading;

        protected Func<object, object, object> _format;
        internal Func<object, object, object> Format => _format;

        protected FluidityListViewFieldConfig(PropertyInfo property)
        {
            _property = property;
        }
    }
}