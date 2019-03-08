// <copyright file="UmbracoUiHelper.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using Umbraco.Core;
using Umbraco.Core.IO;
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
                var existingSection = _sectionService.GetByAlias(sectionConfig.Alias);
                if (existingSection == null)
                {
                    _sectionService.MakeNew(
                        sectionConfig.Name,
                        sectionConfig.Alias,
                        sectionConfig.Icon);
                }

                // Add section name to default lang file
                AddSectionNameToLangFile(sectionConfig.Alias, sectionConfig.Name);

                // Add dashboard to section
                AddFluidityDashboardToSection(sectionConfig.Alias);

                // Create section tree
                if (sectionConfig.Tree != null)
                {
                    var existingTree = _treeService.GetByAlias(sectionConfig.Tree.Alias);
                    if (existingTree == null)
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
            <control>../app_plugins/fluidity/dashboards/dashboard.html</control>
        </tab>  
    </section>
</Action>");

            var action = new addDashboardSection();
            action.Execute("fluidity", xdoc.DocumentElement);
        }

        private void AddSectionNameToLangFile(string sectionAlias, string sectionName)
        {
            var langFileDir = IOHelper.MapPath(SystemDirectories.AppPlugins + "/fluidity/lang");
            var langFilePath = langFileDir + "/en-US.xml";

            IOHelper.EnsurePathExists(langFileDir);
            IOHelper.EnsureFileExists(langFilePath, @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<language alias=""en"" intName=""English (US)"" localName=""English (US)"" lcid="""" culture=""en-US"">
  <creator>
    <name>Fluidity</name>
    <link>http://our.umbraco.org</link>
  </creator>
  <area alias=""sections"">
    <key alias=""fluidity"">Fluidity</key>
  </area>
</language>");

            var xd = XmlHelper.OpenAsXmlDocument(langFilePath);

            var sectionsNode = xd.SelectSingleNode("//area[@alias='sections']");
            if (sectionsNode != null)
            {
                var changes = false;
                var existing = sectionsNode.SelectSingleNode("key[@alias='" + sectionAlias + "']");
                if (existing != null)
                {
                    if (existing.InnerText != sectionName && existing.Attributes?["autoGen"] != null)
                    {
                        existing.InnerText = sectionName;
                        changes = true;
                    }
                }
                else
                {
                    var keyNode = xd.CreateElement("key");
                    keyNode.InnerText = sectionName;

                    var aliasAttr = xd.CreateAttribute("alias");
                    aliasAttr.Value = sectionAlias;
                    keyNode.Attributes.Append(aliasAttr);

                    var autoGenAttr = xd.CreateAttribute("autoGen");
                    autoGenAttr.Value = "true";
                    keyNode.Attributes.Append(autoGenAttr);

                    sectionsNode.AppendChild(keyNode);
                    changes = true;
                }

                if (changes)
                {
                    xd.Save(langFilePath);
                }
            }
        }
    }
}
