using System;
using System.Linq;
using System.Web.Http.ModelBinding;
using Fluidity.Configuration;
using Fluidity.Helpers;
using Fluidity.Web.Extensions;
using Fluidity.Web.Models;
using Umbraco.Core;

namespace Fluidity.Web.WebApi.Validation
{
    // Base on https://github.com/umbraco/Umbraco-CMS/blob/release-7.6.0/src/Umbraco.Web/WebApi/Filters/ContentItemValidationHelper.cs
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

        public void Validate(ModelStateDictionary modelState, FluidityEntityPostModel entity, FluidityCollectionConfig config)
        {
            var configProps = config.Editor?.Tabs.SelectMany(x => x.Fields).ToArray() ?? new FluidityEditorFieldConfig[0];

            if (ValidateProperties(modelState, entity, configProps) == false) return;
            if (ValidatePropertyData(modelState, entity, configProps) == false) return;
        }

        protected virtual bool ValidateProperties(ModelStateDictionary modelState, FluidityEntityPostModel entity, FluidityEditorFieldConfig[] configProps)
        {
            foreach (var p in entity.Properties)
            {
                if (configProps.Any(property => property.Property.Name == p.Alias) == false)
                {
                    throw new InvalidOperationException($"property with alias: {p.Alias} was not found");
                }
            }

            return true;
        }

        protected virtual bool ValidatePropertyData(ModelStateDictionary modelState, FluidityEntityPostModel entity, FluidityEditorFieldConfig[] configProps)
        {
            foreach (var p in configProps)
            {
                var dataTypeInfo = _dataTypeHelper.ResolveDataType(p);
                var postedValue = entity.Properties.Single(x => x.Alias == p.Property.Name).Value;

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
    }
}
