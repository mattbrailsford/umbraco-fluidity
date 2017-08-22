using System;
using System.Collections.Generic;
using Fluidity.Configuration;
using Fluidity.Extensions;
using Umbraco.Web.Models.ContentEditing;

namespace Fluidity.Web.Models.Mappers
{
    internal class FluidityEntityListViewMapper
    {
        public FluidityEntityListViewDisplay ToDisplay(FluiditySectionConfig section, FluidityCollectionConfig collection, object entity)
        {
            var entityId = entity?.GetPropertyValue(collection.IdProperty);
            var entityCompositeId = entityId != null
                ? collection.Alias + "!" + entityId
                : null;

            var name = "";
            if (collection.ListView?.NameFormat != null)
            {
                name = collection.ListView.NameFormat(entity);
            }
            else if (collection.NameFormat != null)
            {
                name = collection.NameFormat(entity);
            }
            else
            {
                name = entity?.ToString();
            }

            var display = new FluidityEntityListViewDisplay
            {
                Id = entity?.GetPropertyValue(collection.IdProperty),
                Name = name,
                Icon = collection.IconSingular + (collection.Color != null ? " color-" + collection.Color : ""),
                Section = section.Alias,
                Tree = section.Tree.Alias,
                Collection = collection.Alias,
                CreateDate = entity != null && collection.DateCreatedProperty != null ? (DateTime)entity.GetPropertyValue(collection.DateCreatedProperty) : DateTime.MinValue,
                UpdateDate = entity != null && collection.DateModifiedProperty != null ? (DateTime)entity.GetPropertyValue(collection.DateModifiedProperty) : DateTime.MinValue,
                EditPath = $"{section.Alias}/fluidity/edit/{entityCompositeId}",
            };

            if (collection.Editor?.Tabs != null)
            {
                var properties = new List<ContentPropertyBasic>();
                if (collection.ListView != null)
                {
                    foreach (var field in collection.ListView.Fields)
                    {
                        var value = entity?.GetPropertyValue(field.Property);

                        if (field.Format != null)
                        {
                            value = field.Format(value, entity);
                        }

                        var propertyScaffold = new ContentPropertyBasic
                        {
                            Id = properties.Count,
                            Alias = field.Property.Name,
                            Value = value?.ToString()
                        };

                        properties.Add(propertyScaffold);
                    }
                }

                display.Properties = properties;
            }

            return display;
        }
    }
}
