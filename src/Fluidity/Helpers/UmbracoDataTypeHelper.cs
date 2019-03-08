// <copyright file="UmbracoDataTypeHelper.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using Fluidity.Configuration;
using Fluidity.Extensions;
using Fluidity.Models;
using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Services;

namespace Fluidity.Helpers
{
    internal class UmbracoDataTypeHelper
    {
        private IDataTypeService _dataTypeService;
        private PropertyEditorResolver _propertyEditorResolver;
        private ICacheProvider _cacheProvider;

        internal UmbracoDataTypeHelper(IDataTypeService dataTypeService, PropertyEditorResolver propertyEditorResolver,
            ICacheProvider cacheProvider)
        {
            _dataTypeService = dataTypeService;
            _propertyEditorResolver = propertyEditorResolver;
            _cacheProvider = cacheProvider;
        }

        internal UmbracoDataTypeHelper()
            : this(ApplicationContext.Current.Services.DataTypeService, 
                  PropertyEditorResolver.Current,
                  ApplicationContext.Current.ApplicationCache.RequestCache)
        { }

        internal DataTypeInfo ResolveDataType(FluidityEditorFieldConfig fieldConfig, bool isReadOnly = false)
        {
            var dtdKey = !fieldConfig.DataTypeName.IsNullOrWhiteSpace()
                ? fieldConfig.DataTypeName
                : fieldConfig.GetOrCalculateDefinititionId().ToString();
            dtdKey += $"_{isReadOnly}";

            return _cacheProvider.GetCacheItem<DataTypeInfo>($"fluidity_datatypeinfo_{dtdKey}", () =>
            {
                IDataTypeDefinition dataTypeDefinition = null;

                if (!fieldConfig.DataTypeName.IsNullOrWhiteSpace())
                {
                    dataTypeDefinition = _dataTypeService.GetDataTypeDefinitionByName(fieldConfig.DataTypeName);
                }

                if (dataTypeDefinition == null)
                {
                    var dataTypeId = fieldConfig.DataTypeId == 0 && isReadOnly
                        ? -92 // If readonly and no explicit datatype defined, default to label
                        : fieldConfig.GetOrCalculateDefinititionId();
                    dataTypeDefinition = _dataTypeService.GetDataTypeDefinitionById(dataTypeId);
                }

                var preValues = _dataTypeService.GetPreValuesCollectionByDataTypeId(dataTypeDefinition.Id);
                var propEditor = _propertyEditorResolver.GetByAlias(dataTypeDefinition.PropertyEditorAlias);

                return new DataTypeInfo(dataTypeDefinition, propEditor, preValues);
            });
        }
    }
}
