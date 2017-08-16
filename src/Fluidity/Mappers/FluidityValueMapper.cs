namespace Fluidity.Mappers
{
    public abstract class FluidityValueMapper
    {
        public abstract object ModelToEditor(object input);

        public abstract object EditorToModel(object input);
    }
}
