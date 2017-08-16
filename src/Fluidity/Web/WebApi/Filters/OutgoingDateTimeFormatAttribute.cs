using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using Umbraco.Core;

namespace Fluidity.Web.WebApi.Filters
{
    /// <summary>
    /// Sets the json outgoing/serialized datetime format
    /// Copied from https://github.com/umbraco/Umbraco-CMS/blob/release-7.6.0/src/Umbraco.Web/WebApi/Filters/OutgoingDateTimeFormatAttribute.cs
    /// </summary>
    internal sealed class OutgoingDateTimeFormatAttribute : Attribute, IControllerConfiguration
    {
        private readonly string _format = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Specify a custom format
        /// </summary>
        /// <param name="format"></param>
        public OutgoingDateTimeFormatAttribute(string format)
        {
            Mandate.ParameterNotNullOrEmpty(format, "format");
            _format = format;
        }

        /// <summary>
        /// Will use the standard ISO format
        /// </summary>
        public OutgoingDateTimeFormatAttribute()
        {

        }

        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            var jsonFormatter = controllerSettings.Formatters.OfType<JsonMediaTypeFormatter>();
            foreach (var r in jsonFormatter)
            {
                r.SerializerSettings.Converters.Add(new CustomDateTimeConvertor(_format));
            }
        }

    }
}
