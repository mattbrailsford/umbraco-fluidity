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

        protected LambdaExpression _whereClauseExpression;
        internal LambdaExpression WhereClauseExpression => _whereClauseExpression;

        public FluidityDataViewConfig(string name, LambdaExpression whereClauseExpression)
        {
            _alias = name.ToSafeAlias(true);
            _name = name;
            _whereClauseExpression = whereClauseExpression;
        }
    }
}
