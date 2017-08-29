using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Fluidity.Helpers
{
    internal static class UmbracoPublishedPropertyTypeHelper
    {
        public static IDictionary<string, PreValue> GetPreValues(this PublishedPropertyType propType)
        {
            return (IDictionary<string, PreValue>)ApplicationContext.Current.ApplicationCache.RequestCache.GetCacheItem($"UmbracoPublishedPropertyTypeHelper.GetPreValues_{propType.DataTypeId}", () => 
                ApplicationContext.Current.Services.DataTypeService.GetPreValuesCollectionByDataTypeId(propType.DataTypeId).PreValuesAsDictionary);
        } 
    }
}
