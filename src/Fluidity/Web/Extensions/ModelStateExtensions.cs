// <copyright file="ModelStateExtensions.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Fluidity.Web.Extensions
{
    internal static class ModelStateExtensions
    {
        internal static void AddPropertyError(this System.Web.Http.ModelBinding.ModelStateDictionary modelState, ValidationResult result, string propertyAlias)
        {
            //if there are no member names supplied then we assume that the validation message is for the overall property
            // not a sub field on the property editor
            if (!result.MemberNames.Any())
            {
                //add a model state error for the entire property
                modelState.AddModelError($"_Properties.{propertyAlias}", result.ErrorMessage);
            }
            else
            {
                //there's assigned field names so we'll combine the field name with the property name
                // so that we can try to match it up to a real sub field of this editor
                foreach (var field in result.MemberNames)
                {
                    modelState.AddModelError($"_Properties.{propertyAlias}.{field}", result.ErrorMessage);
                }
            }
        }

        internal static IDictionary<string, object> ToErrorDictionary(this System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            var modelStateError = new Dictionary<string, object>();
            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    modelStateError.Add(key, errors.Select(error => error.ErrorMessage));
                }
            }
            return modelStateError;
        }
    }
}
