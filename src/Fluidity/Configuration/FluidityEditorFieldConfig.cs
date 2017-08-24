using System.Linq.Expressions;
using Fluidity.Mappers;

namespace Fluidity.Configuration
{
    public abstract class FluidityEditorFieldConfig
    {
        protected FluidityPropertyConfig _property;
        internal FluidityPropertyConfig Property => _property;

        protected string _label;
        internal string Label => _label;

        protected string _description;
        internal string Description => _description;

        protected bool _isRequired;
        internal bool IsRequired => _isRequired;

        protected string _validationRegex;
        internal string ValidationRegex => _validationRegex;

        protected string _dataTypeName;
        internal string DataTypeName => _dataTypeName;

        protected int _dataTypeId;
        internal int DataTypeId => _dataTypeId;

        protected FluidityValueMapper _mapper;
        internal FluidityValueMapper Mapper => _mapper;

        protected FluidityEditorFieldConfig(LambdaExpression propertyExp)
        {
            _property = propertyExp;
        }
    }
}