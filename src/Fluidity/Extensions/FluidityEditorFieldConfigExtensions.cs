// <copyright file="FluidityEditorFieldConfigExtensions.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using Fluidity.Configuration;

namespace Fluidity.Extensions
{
    internal static class FluidityEditorFieldConfigExtensions
    {
        internal static int GetOrCalculateDefinititionId(this FluidityEditorFieldConfig fieldConfig)
        {
            if (fieldConfig.DataTypeId != 0)
                return fieldConfig.DataTypeId;

            var dtdId = -88; // Text string (default)

            // TODO: Check for NText attribute for textarea/rte?

            if (fieldConfig.Property.Type == typeof(DateTime)
                || fieldConfig.Property.Type == typeof(DateTime?))
            {
                dtdId = -41; // Date picker
            }

            if (fieldConfig.Property.Type == typeof(bool))
            {
                dtdId = -49; // True/False
            }

            if (fieldConfig.Property.Type == typeof(int)
                || fieldConfig.Property.Type == typeof(long))
            {
                dtdId = -51; // Numeric
            }

            return dtdId;
        }
    }
}
