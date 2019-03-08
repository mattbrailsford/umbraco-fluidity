// <copyright file="FluidityEntityPickerValueConverter.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using Fluidity.Configuration;
using Fluidity.Helpers;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Fluidity.Converters
{
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    [PropertyValueType(typeof(IEnumerable<object>))]
    public class FluidityEntityPickerValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.InvariantEquals("Fluidity.EntityPicker");
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            try
            {
                if (source == null || source.ToString().IsNullOrWhiteSpace())
                    return null;

                var ids = source.ToString().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Length == 0)
                    return null;

                var preValues = propertyType.GetPreValues();
                if (preValues == null || !preValues.ContainsKey("collection"))
                    throw new ApplicationException($"Fluidity DataType {propertyType.DataTypeId} has no 'collection' pre value.");

                var collectionParts = preValues["collection"].Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (collectionParts.Length < 2)
                    throw new ApplicationException($"Fluidity DataType {propertyType.DataTypeId} has an invalid 'collection' pre value.");

                var section = FluidityContext.Current.Config.Sections[collectionParts[0]];
                if (section == null)
                    throw new ApplicationException($"Fluidity DataType {propertyType.DataTypeId} has an invalid 'collection' pre value. No section found with the alias {collectionParts[0]}");

                var collection = section.Tree.FlattenedTreeItems[collectionParts[1]] as FluidityCollectionConfig;
                if (collection == null)
                    throw new ApplicationException($"Fluidity DataType {propertyType.DataTypeId} has an invalid 'collection' pre value. No collection found with the alias {collectionParts[1]}");

                return FluidityContext.Current.Services.EntityService.GetEntitiesByIds(section, collection, ids);

            }
            catch (Exception e)
            {
                LogHelper.Error<FluidityEntityPickerValueConverter>("Error converting value", e);
            }

            return null;
        }
    }
}
