// <copyright file="FluidityEntityMapper.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Fluidity.Configuration;
using Fluidity.Extensions;
using Fluidity.Helpers;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Editors;
using Umbraco.Core.Services;
using Umbraco.Web.Models.ContentEditing;
using ObjectExtensions = Fluidity.Extensions.ObjectExtensions;

namespace Fluidity.Web.Models.Mappers
{
    internal class FluidityEntityMapper
    {
        private UmbracoDataTypeHelper _dataTypeHelper;
        private IDataTypeService _dataTypeService;

        public FluidityEntityMapper(UmbracoDataTypeHelper dataTypeHelper, IDataTypeService dataTypeService)
        {
            _dataTypeHelper = dataTypeHelper;
            _dataTypeService = dataTypeService;
        }

        public FluidityEntityMapper()
            : this(new UmbracoDataTypeHelper(), ApplicationContext.Current.Services.DataTypeService)
        { }

        public FluidityEntityDisplayModel ToDisplayModel(FluiditySectionConfig section, FluidityCollectionConfig collection, object entity)
        {
            var entityId = entity?.GetPropertyValue(collection.IdProperty);
            var entityCompositeId = entityId != null
                ? collection.Alias + "!" + entityId
                : null;

            var name = "";
            if (collection.NameProperty != null)
            {
                name = entity.GetPropertyValue(collection.NameProperty).ToString();
            }
            else if (collection.NameFormat != null)
            {
                name = collection.NameFormat(entity);
            }
            else
            {
                name = entity?.ToString();
            }

            var display = new FluidityEntityDisplayModel
            {
                Id = entity?.GetPropertyValue(collection.IdProperty),
                Name = name,
                Icon = collection.IconSingular + (collection.IconColor != null ? " color-" + collection.IconColor : ""),
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

        public FluidityEntityEditModel ToEditModel(FluiditySectionConfig section, FluidityCollectionConfig collection, object entity)
        {
            var entityId = entity?.GetPropertyValue(collection.IdProperty);
            var entityCompositeId = entityId != null
                ? collection.Alias + "!" + entityId
                : null;

            var display = new FluidityEntityEditModel
            {
                Id = entity?.GetPropertyValue(collection.IdProperty),
                Name = collection?.NameProperty != null ? entity?.GetPropertyValue(collection.NameProperty).ToString() : collection.NameSignular,
                HasNameProperty = collection.NameProperty != null,
                Section = section.Alias,
                Tree = section.Tree.Alias,
                Collection = collection.Alias,
                CollectionNameSingular = collection.NameSignular,
                CollectionNamePlural = collection.NamePlural,
                CollectionIconSingular = collection.IconSingular,
                CollectionIconPlural = collection.IconPlural,
                IsChildOfListView = collection.ViewMode == FluidityViewMode.List,
                IsChildOfTreeView = collection.ViewMode == FluidityViewMode.Tree,
                TreeNodeUrl = "/umbraco/backoffice/fluidity/FluidityTree/GetTreeNode/" + entityCompositeId + "?application=" + section.Alias,
                CreateDate = entity != null && collection.DateCreatedProperty != null ? (DateTime)entity.GetPropertyValue(collection.DateCreatedProperty) : DateTime.MinValue,
                UpdateDate = entity != null && collection.DateModifiedProperty != null ? (DateTime)entity.GetPropertyValue(collection.DateCreatedProperty) : DateTime.MinValue,
                Path = collection.Path + (entity != null ? FluidityConstants.PATH_SEPERATOR + collection.Alias + "!" + entity.GetPropertyValue(collection.IdProperty) : string.Empty)
            };

            if (collection.Editor?.Tabs != null)
            {
                var tabs = new List<Tab<ContentPropertyDisplay>>();
                foreach (var tab in collection.Editor.Tabs)
                {
                    var tabScaffold = new Tab<ContentPropertyDisplay>
                    {
                        Id = tabs.Count,
                        Alias = tab.Name,
                        Label = tab.Name,
                        IsActive = tabs.Count == 0
                    };

                    var properties = new List<ContentPropertyDisplay>();
                    if (tab.Fields != null)
                    {
                        foreach (var field in tab.Fields)
                        {
                            var dataTypeInfo = _dataTypeHelper.ResolveDataType(field);

                            dataTypeInfo.PropertyEditor.ValueEditor.ConfigureForDisplay(dataTypeInfo.PreValues);

                            var propEditorConfig = dataTypeInfo.PropertyEditor.PreValueEditor.ConvertDbToEditor(dataTypeInfo.PropertyEditor.DefaultPreValues,
                                dataTypeInfo.PreValues);

                            var value = entity?.GetPropertyValue(field.Property);

                            if (field.ValueMapper != null)
                            {
                                value = field.ValueMapper.ModelToEditor(value);
                            }

                            var dummyProp = new Property(new PropertyType(dataTypeInfo.DataTypeDefinition), value);
                            value = dataTypeInfo.PropertyEditor.ValueEditor.ConvertDbToEditor(dummyProp, dummyProp.PropertyType, _dataTypeService);

                            var propertyScaffold = new ContentPropertyDisplay
                            {
                                Id = properties.Count,
                                Alias = field.Property.Name,
                                Label = field.Label ?? field.Property.Name.SplitPascalCasing(),
                                Description = field.Description,
                                Editor = dataTypeInfo.PropertyEditor.Alias,
                                View = dataTypeInfo.PropertyEditor.ValueEditor.View,
                                Config = propEditorConfig,
                                HideLabel = dataTypeInfo.PropertyEditor.ValueEditor.HideLabel,
                                Value = value
                            };

                            properties.Add(propertyScaffold);
                        }

                    }
                    tabScaffold.Properties = properties;

                    tabs.Add(tabScaffold);
                }

                display.Tabs = tabs;
            }

            return display;
        }

        public object FromPostModel(FluiditySectionConfig section, FluidityCollectionConfig collection, FluidityEntityPostModel postModel, object entity)
        {
            var editorProps = collection.Editor.Tabs.SelectMany(x => x.Fields).ToArray();

            // Update the name property
            if (collection.NameProperty != null)
            {
                entity.SetPropertyValue(collection.NameProperty, postModel.Name);
            }

            // Update the individual properties
            foreach (var prop in postModel.Properties)
            {
                // Get the prop config
                var propConfig = editorProps.First(x => x.Property.Name == prop.Alias);

                // Create additional data for file handling
                var additionalData = new Dictionary<string, object>();
                
                // Grab any uploaded files and add them to the additional data
                var files = postModel.UploadedFiles.Where(x => x.PropertyAlias == prop.Alias).ToArray();
                if (files.Length > 0)
                {
                    additionalData.Add("files", files);
                }

                // Ensure safe filenames
                foreach (var file in files)
                {
                    file.FileName = file.FileName.ToSafeFileName();
                }

                // Add extra things needed to figure out where to put the files
                // Looking into the core code, these are not actually used for any lookups,
                // rather they are used to generate a unique path, so we just use the nearest
                // equivilaants from the fluidity api. 
                var cuid = $"{section.Alias}_{collection.Alias}_{entity.GetPropertyValue(collection.IdProperty)}";
                var puid = $"{section.Alias}_{collection.Alias}_{propConfig.Property.Name}";

                additionalData.Add("cuid", ObjectExtensions.EncodeAsGuid(cuid));
                additionalData.Add("puid", ObjectExtensions.EncodeAsGuid(puid));

                var dataTypeInfo = _dataTypeHelper.ResolveDataType(propConfig);
                var data = new ContentPropertyData(prop.Value, dataTypeInfo.PreValues, additionalData);

                if (!dataTypeInfo.PropertyEditor.ValueEditor.IsReadOnly)
                {
                    var currentValue = entity.GetPropertyValue(propConfig.Property);
                    if (propConfig.ValueMapper != null)
                    {
                        currentValue = propConfig.ValueMapper.ModelToEditor(currentValue);
                    }

                    var propVal = dataTypeInfo.PropertyEditor.ValueEditor.ConvertEditorToDb(data, currentValue);
                    if (propConfig.ValueMapper != null)
                    {
                        propVal = propConfig.ValueMapper.EditorToModel(propVal);
                    }

                    if (!propVal.GetType().IsAssignableFrom(propConfig.Property.Type))
                    {
                        var convert = propVal.TryConvertTo(propConfig.Property.Type);
                        if (convert.Success)
                        {
                            propVal = convert.Result;
                        }
                    }

                    var supportTagsAttribute = TagExtractor.GetAttribute(dataTypeInfo.PropertyEditor);
                    if (supportTagsAttribute != null)
                    {
                        var dummyProp = new Property(new PropertyType(dataTypeInfo.DataTypeDefinition), propVal);
                        TagExtractor.SetPropertyTags(dummyProp, data, propVal, supportTagsAttribute);
                    }
                    else
                    {
                        entity.SetPropertyValue(propConfig.Property, propVal);
                    }
                }
            }

            return entity;
        }
    }
}
