using Fluidity.Configuration;

namespace Fluidity.Web.Models.Mappers
{
    internal class FluiditySectionMapper
    {
        public FluiditySectionDisplayModel ToDisplayModel(FluiditySectionConfig section)
        {
            return new FluiditySectionDisplayModel
            {
                Alias = section.Alias,
                Name = section.Name,
                Tree = section.Tree.Alias
            };
        }
    }
}
