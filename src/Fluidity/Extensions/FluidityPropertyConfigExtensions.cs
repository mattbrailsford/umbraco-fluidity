using Fluidity.Configuration;

namespace Fluidity.Extensions
{
    internal static class FluidityPropertyConfigExtensions
    {
        public static string GetColumnName(this FluidityPropertyConfig propertyConfig)
        {
            return propertyConfig.PropertyInfo.GetColumnName();
        }
    }
}
