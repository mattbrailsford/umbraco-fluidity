// <copyright file="CustomDateTimeConvertor.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Umbraco.Core;

namespace Fluidity.Web.WebApi
{
    /// <summary>
    /// Used to convert the format of a DateTime object when serializing
    /// Copied from https://github.com/umbraco/Umbraco-CMS/blob/release-7.6.0/src/Umbraco.Web/WebApi/CustomDateTimeConvertor.cs
    /// </summary>
    internal class CustomDateTimeConvertor : IsoDateTimeConverter
    {
        private readonly string _dateTimeFormat;

        public CustomDateTimeConvertor(string dateTimeFormat)
        {
            Mandate.ParameterNotNullOrEmpty(dateTimeFormat, "dateTimeFormat");
            _dateTimeFormat = dateTimeFormat;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(_dateTimeFormat));
        }
    }
}
