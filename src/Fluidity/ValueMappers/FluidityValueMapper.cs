// <copyright file="FluidityValueMapper.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Fluidity.ValueMappers
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
