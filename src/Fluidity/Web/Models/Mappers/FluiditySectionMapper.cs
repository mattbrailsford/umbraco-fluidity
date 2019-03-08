// <copyright file="FluiditySectionMapper.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Fluidity.Configuration;

namespace Fluidity.Web.Models.Mappers
{
    internal class FluiditySectionMapper
    {
        public FluiditySectionDisplayModel ToDisplayModel(FluiditySectionConfig section)
        {
            return new FluiditySectionDisplayModel
            {
                Alias = section.Alias,
                Name = section.Name,
                Tree = section.Tree.Alias
            };
        }
    }
}
