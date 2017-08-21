using System;
using System.Linq.Expressions;
using Umbraco.Core;

namespace Fluidity.Configuration
{
    public class FluidityDataViewConfig
    {
        protected string _alias;
        internal string Alias => _alias;

        protected string _name;
        internal string Name => _name;

        protected Expression _whereClauseExpression;
        internal Expression WhereClauseExpression => _whereClauseExpression;

        public FluidityDataViewConfig(string name, Expression whereClauseExpression)
        {
            _alias = name.ToSafeAlias();
            _name = name;
            _whereClauseExpression = whereClauseExpression;
        }
    }
}
