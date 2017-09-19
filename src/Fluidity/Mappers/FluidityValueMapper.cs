namespace Fluidity.Mappers
{
    /// <summary>
    /// Fluidity value mapper
    /// </summary>
    public abstract class FluidityValueMapper
    {
        /// <summary>
        /// Map from Fluidity model to editor model.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The mapped entity.</returns>
        public abstract object ModelToEditor(object input);

        /// <summary>
        /// Map from editor model to Fluidity model.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The mapped entity.</returns>
        public abstract object EditorToModel(object input);
    }
}
