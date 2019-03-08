// <copyright file="FluidityPropertyConfigExtensions.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Fluidity.Configuration;

namespace Fluidity.Extensions
{
    internal static class FluidityPropertyConfigExtensions
    {
        public static string GetColumnName(this FluidityPropertyConfig propertyConfig)
        {
            return propertyConfig.PropertyInfo.GetColumnName();
        }
    }
}
