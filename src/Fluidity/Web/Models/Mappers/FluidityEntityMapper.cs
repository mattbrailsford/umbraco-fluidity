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

        public FluidityEntityDisplay ToDisplay(FluiditySectionConfig section, FluidityCollectionConfig config, object entity)
        {
            var entityId = entity?.GetPropertyValue(config.IdProperty);
            var entityCompositeId = entityId != null
                ? config.Alias + "!" + entityId
                : null;

            var display = new FluidityEntityDisplay
            {
                Id = entity?.GetPropertyValue(config.IdProperty),
                Name = entity != null && config.Editor?.NameProperty != null ? entity.GetPropertyValue(config.Editor.NameProperty).ToString() : null,
                Section = section.Alias,
                Tree = section.Tree.Alias,
                Collection = config.Alias,
                CollectionNameSingular = config.NameSignular,
                CollectionNamePlural = config.NamePlural,
                CollectionIconSingular = config.IconSingular,
                CollectionIconPlural = config.IconPlural,
                HasNameProperty = config.Editor?.NameProperty != null,
                IsChildOfListView = config.TreeMode != FluidityTreeMode.Tree,
                IsChildOfTreeView = config.TreeMode != FluidityTreeMode.List,
                TreeNodeUrl = "/umbraco/backoffice/fluidity/FluidityTree/GetTreeNode/" + entityCompositeId + "?application=" + section.Alias,
                CreateDate = entity != null && config.DateCreated != null ? (DateTime)entity.GetPropertyValue(config.DateCreated) : DateTime.Now,
                UpdateDate = entity != null && config.DateModified != null ? (DateTime)entity.GetPropertyValue(config.DateCreated) : DateTime.Now,
                Path = config.Path + (entity != null ? FluidityConstants.PATH_SEPERATOR + config.Alias + "!" + entity.GetPropertyValue(config.IdProperty) : string.Empty)
            };

            if (config.Editor?.Tabs != null)
            {
                var tabs = new List<Tab<ContentPropertyDisplay>>();
                foreach (var tab in config.Editor.Tabs)
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

                            if (field.Mapper != null)
                            {
                                value = field.Mapper.ModelToEditor(value);
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

        public object FromPost(FluiditySectionConfig section, FluidityCollectionConfig config, FluidityEntityPost postModel, object entity)
        {
            var editorProps = config.Editor.Tabs.SelectMany(x => x.Fields).ToArray();

            // Update the name property
            if (config.Editor.NameProperty != null)
            {
                entity.SetPropertyValue(config.Editor.NameProperty, postModel.Name);
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
                var cuid = $"{section.Alias}_{config.Alias}_{entity.GetPropertyValue(config.IdProperty)}";
                var puid = $"{section.Alias}_{config.Alias}_{propConfig.Property.Name}";

                additionalData.Add("cuid", ObjectExtensions.EncodeAsGuid(cuid));
                additionalData.Add("puid", ObjectExtensions.EncodeAsGuid(puid));

                var dataTypeInfo = _dataTypeHelper.ResolveDataType(propConfig);
                var data = new ContentPropertyData(prop.Value, dataTypeInfo.PreValues, additionalData);

                if (!dataTypeInfo.PropertyEditor.ValueEditor.IsReadOnly)
                {
                    var currentValue = entity.GetPropertyValue(propConfig.Property);
                    if (propConfig.Mapper != null)
                    {
                        currentValue = propConfig.Mapper.ModelToEditor(currentValue);
                    }

                    var propVal = dataTypeInfo.PropertyEditor.ValueEditor.ConvertEditorToDb(data, currentValue);
                    if (propConfig.Mapper != null)
                    {
                        propVal = propConfig.Mapper.EditorToModel(propVal);
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
