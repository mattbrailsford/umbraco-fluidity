using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluidity.Configuration
{
    public abstract class FluidityEditorConfig
    {
        protected PropertyInfo _nameProperty;
        internal PropertyInfo NameProperty => _nameProperty;

        protected LambdaExpression _namePropertyExp;
        internal LambdaExpression NamePropertyExp => _namePropertyExp;

        protected List<FluidityTabConfig> _tabs;
        internal IEnumerable<FluidityTabConfig> Tabs => _tabs;

        protected FluidityEditorConfig()
        {
            _tabs = new List<FluidityTabConfig>();
        }
    }
}