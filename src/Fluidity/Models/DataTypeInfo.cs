// <copyright file="DataTypeInfo.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;

namespace Fluidity.Models
{
    internal class DataTypeInfo
    {
        public IDataTypeDefinition DataTypeDefinition { get; }

        public PropertyEditor PropertyEditor { get; }

        public PreValueCollection PreValues { get; }

        public DataTypeInfo(IDataTypeDefinition dataTypeDefinition, PropertyEditor propertyEditor, PreValueCollection preValues)
        {
            DataTypeDefinition = dataTypeDefinition;
            PropertyEditor = propertyEditor;
            PreValues = preValues;
        }
    }
}