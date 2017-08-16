using Umbraco.Core;
using Umbraco.Core.Services;

namespace Fluidity.Helpers
{
    internal class UmbracoUiHelper
    {
        private ISectionService _sectionService;
        private IApplicationTreeService _treeService;

        internal UmbracoUiHelper(ISectionService sectionService,
            IApplicationTreeService treeService)
        {
            _sectionService = sectionService;
            _treeService = treeService;
        }

        internal UmbracoUiHelper()
            : this(ApplicationContext.Current.Services.SectionService, ApplicationContext.Current.Services.ApplicationTreeService)
        { }

        internal void Build(FluidityContext context)
        {
            foreach (var sectionConfig in context.Config.Sections.Values)
            {
                _sectionService.MakeNew(
                    sectionConfig.Name, 
                    sectionConfig.Alias, 
                    sectionConfig.Icon);

                if (sectionConfig.Tree != null)
                {
                    _treeService.MakeNew(
                        sectionConfig.Tree.Initialize,
                        0,
                        sectionConfig.Alias,
                        sectionConfig.Tree.Alias,
                        sectionConfig.Tree.Name,
                        sectionConfig.Tree.Icon,
                        sectionConfig.Tree.Icon,
                        "Fluidity.Web.Trees.FluidityTreeController, Fluidity");
                }
            }
        }
    }
}
