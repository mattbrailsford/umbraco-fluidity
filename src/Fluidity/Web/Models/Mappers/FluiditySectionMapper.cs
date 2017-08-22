using Fluidity.Configuration;

namespace Fluidity.Web.Models.Mappers
{
    internal class FluiditySectionMapper
    {
        public FluiditySectionDisplay ToDisplay(FluiditySectionConfig section)
        {
            return new FluiditySectionDisplay
            {
                Alias = section.Alias,
                Name = section.Name,
                Tree = section.Tree.Alias
            };
        }
    }
}
