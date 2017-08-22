using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
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
                // Create the section
                _sectionService.MakeNew(
                    sectionConfig.Name, 
                    sectionConfig.Alias, 
                    sectionConfig.Icon);

                // Add dashboard to section
                AddFluidityDashboardToSection(sectionConfig.Alias);

                // Create section tree
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

        private void AddFluidityDashboardToSection(string sectionAlias)
        {
            // TODO: Make this a bit smarter by updating existing if on exists rather than keep adding new ones

            var xdoc = new XmlDocument();
            xdoc.LoadXml($@"<Action runat=""install"" alias=""addDashboardSection"" dashboardAlias=""fluidity_{sectionAlias}"">
    <section>
        <areas>
            <area>{sectionAlias}</area>
        </areas>
        <tab caption=""Summary"">
            <control>../app_plugins/fluidity/views/dashboard.html</control>
        </tab>  
    </section>
</Action>");

            var action = new addDashboardSection();
            action.Execute("fluidity", xdoc.DocumentElement);
        }
    }
}
