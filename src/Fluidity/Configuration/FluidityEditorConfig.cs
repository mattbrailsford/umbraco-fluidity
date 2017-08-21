using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluidity.Configuration
{
    public abstract class FluidityEditorConfig
    {
        protected List<Type> _menuActionTypes;
        internal IEnumerable<Type> MenuActionTypes => _menuActionTypes;

        protected PropertyInfo _nameProperty;
        internal PropertyInfo NameProperty => _nameProperty;

        protected LambdaExpression _namePropertyExp;
        internal LambdaExpression NamePropertyExp => _namePropertyExp;

        protected List<FluidityTabConfig> _tabs;
        internal IEnumerable<FluidityTabConfig> Tabs => _tabs;

        protected FluidityEditorConfig()
        {
            _menuActionTypes = new List<Type>();
            _tabs = new List<FluidityTabConfig>();
        }
    }
}