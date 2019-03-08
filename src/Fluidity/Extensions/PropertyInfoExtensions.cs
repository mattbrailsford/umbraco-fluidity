// <copyright file="PropertyInfoExtensions.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Fluidity.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static string GetColumnName(this PropertyInfo propertyInfo)
        {
            var columnAttr = propertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr != null && !columnAttr.Name.IsNullOrWhiteSpace())
            {
                return columnAttr.Name.Trim('[', ']');
            }

            return propertyInfo.Name;
        }
    }
}
