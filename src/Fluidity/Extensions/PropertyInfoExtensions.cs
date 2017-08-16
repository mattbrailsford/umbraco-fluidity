using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace Fluidity.Extensions
{
    internal static class PropertyInfoExtensions
    {
        public static string GetColumnName(this PropertyInfo propertyInfo)
        {
            var columnAttr = propertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr != null && !columnAttr.Name.IsNullOrWhiteSpace())
            {
                return columnAttr.Name.Trim('[', ']');
            }

            return propertyInfo.Name;
        }
    }
}
