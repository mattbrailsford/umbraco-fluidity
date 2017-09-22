using System;

namespace Fluidity.Mappers
{
    internal class ReadOnlyValueMapper : FluidityValueMapper
    {
        private readonly Func<object, string> _format;
        public ReadOnlyValueMapper(Func<object, string> format)
        {
            _format = format;
        }

        public override object ModelToEditor(object input)
        {
            return _format(input);
        }

        public override object EditorToModel(object input)
        {
            return _format(input);
        }
    }
}