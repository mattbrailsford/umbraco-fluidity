// <copyright file="FluidityEntityPostValidator.cs" company="Umbraco, Matt Brailsford">
// Original work Copyright (c) 2013 Umbraco and contributors.
// Original work licensed under the MIT License (https://github.com/umbraco/Umbraco-CMS/blob/release-7.6.0/LICENSE.md).
// Modified work Copyright (c) 2019 Matt Brailsford and contributors.
// Modified work licensed under the Apache License, Version 2.0.
// </copyright>
// <remarks>
// Based on https://github.com/umbraco/Umbraco-CMS/blob/release-7.6.0/src/Umbraco.Web/WebApi/Filters/ContentItemValidationHelper.cs
// </remarks>

using System;
using System.Linq;
using System.Web.Http.ModelBinding;
using Fluidity.Configuration;
using Fluidity.Helpers;
using Fluidity.Web.Extensions;
using Fluidity.Web.Models;
using Umbraco.Core;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Fluidity.Web.WebApi.Validation
{
    internal class FluidityEntityPostValidator
    {
        private UmbracoDataTypeHelper _dataTypeHelper;

        public FluidityEntityPostValidator(UmbracoDataTypeHelper dataTypeHelper)
        {
            _dataTypeHelper = dataTypeHelper;
        }

        public FluidityEntityPostValidator()
            : this(new UmbracoDataTypeHelper())
        { }

        public void Validate(ModelStateDictionary modelState, FluidityEntityPostModel postModel, object entity, FluidityCollectionConfig config, bool isReadOnly)
        {
            var configProps = config.Editor?.Tabs.SelectMany(x => x.Fields).ToArray() ?? new FluidityEditorFieldConfig[0];

            if (ValidateProperties(modelState, postModel, configProps) == false) return;
            if (ValidatePropertyData(modelState, postModel, configProps, isReadOnly) == false) return;
            if (ValidateDataAnnotations(modelState, entity) == false) return;
        }

        protected virtual bool ValidateProperties(ModelStateDictionary modelState, FluidityEntityPostModel postModel, FluidityEditorFieldConfig[] configProps)
        {
            foreach (var p in postModel.Properties)
            {
                if (configProps.Any(property => property.Property.Name == p.Alias) == false)
                {
                    throw new InvalidOperationException($"property with alias: {p.Alias} was not found");
                }
            }

            return true;
        }

        protected virtual bool ValidatePropertyData(ModelStateDictionary modelState, FluidityEntityPostModel postModel, FluidityEditorFieldConfig[] configProps, bool isReadOnly)
        {
            foreach (var p in configProps)
            {
                var dataTypeInfo = _dataTypeHelper.ResolveDataType(p, isReadOnly);
                var postedValue = postModel.Properties.Single(x => x.Alias == p.Property.Name).Value;

                // Validate against the prop editor validators
                foreach (var result in dataTypeInfo.PropertyEditor.ValueEditor.Validators
                    .SelectMany(v => v.Validate(postedValue, dataTypeInfo.PreValues, dataTypeInfo.PropertyEditor)))
                {
                    modelState.AddPropertyError(result, p.Property.Name);
                }

                // Now we need to validate the property based on the PropertyType validation (i.e. regex and required)
                // NOTE: These will become legacy once we have pre-value overrides.
                if (p.IsRequired)
                {
                    foreach (var result in dataTypeInfo.PropertyEditor.ValueEditor.RequiredValidator
                        .Validate(postedValue, "", dataTypeInfo.PreValues, dataTypeInfo.PropertyEditor))
                    {
                        modelState.AddPropertyError(result, p.Property.Name);
                    }
                }

                if (!p.ValidationRegex.IsNullOrWhiteSpace())
                {
                    // We only want to execute the regex statement if:
                    //  * the value is null or empty AND it is required OR
                    //  * the value is not null or empty
                    // See: http://issues.umbraco.org/issue/U4-4669

                    var asString = postedValue as string;

                    if (
                        //Value is not null or empty
                        (postedValue != null && asString.IsNullOrWhiteSpace() == false)
                        //It's required
                        || p.IsRequired)
                    {
                        foreach (var result in dataTypeInfo.PropertyEditor.ValueEditor.RegexValidator
                            .Validate(postedValue, p.ValidationRegex, dataTypeInfo.PreValues, dataTypeInfo.PropertyEditor))
                        {
                            modelState.AddPropertyError(result, p.Property.Name);
                        }
                    }
                }
            }

            return modelState.IsValid;
        }

        protected virtual bool ValidateDataAnnotations(ModelStateDictionary modelState, object entity)
        {
            var validationContext = new ValidationContext(entity, null, null);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(entity, validationContext, results, true);

            foreach (var result in results)
            {
                foreach (var field in result.MemberNames)
                {
                    modelState.AddModelError($"_Properties.{field}", result.ErrorMessage);
                }
            }

            return modelState.IsValid;
        }
    }
}
