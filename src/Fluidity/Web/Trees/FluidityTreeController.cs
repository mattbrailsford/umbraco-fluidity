using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Fluidity.Configuration;
using Fluidity.Extensions;
using umbraco;
using umbraco.BusinessLogic.Actions;
using Umbraco.Core.IO;
using Umbraco.Core.Services;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Web.WebApi.Filters;

namespace Fluidity.Web.Trees
{
    [PluginController("fluidity")]
    [Tree("fluidity", "fluidity", "Fluidity", initialize:false)]
    public class FluidityTreeController : TreeController
    {
        internal FluidityContext Context => FluidityContext.Current;

        protected string SectionAlias
        {
            get
            {
                // TODO: Error checking in case of no HTTP context or querystring
                var query = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                return query["application"];
            }
        }

        protected FluidityTreeConfig TreeConfig => Context.Config.Sections[SectionAlias].Tree;

        public override string RootNodeDisplayName => TreeConfig.Name;

        public override string TreeAlias => TreeConfig.Alias;

        [HttpQueryStringFilter("queryStrings")]
        public TreeNode GetTreeNode(string id, FormDataCollection queryStrings)
        {
            var alias = id; 
            object entityId = null;

            // Extract the entity Id if we have one
            var seperatorIndex = alias.IndexOf("!", StringComparison.InvariantCulture);
            if (seperatorIndex >= 0)
            {
                entityId = alias.Substring(seperatorIndex + 1);
                alias = alias.Substring(0, seperatorIndex);
            }

            var collectionConfig = TreeConfig.FalttenedTreeItems[alias] as FluidityCollectionConfig;
            if (collectionConfig == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var entity = Context.Services.EntityService.Get(collectionConfig, entityId);
            if (entity == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var node = CreateEntityTreeNode(collectionConfig, entity, queryStrings);

            //add the tree alias to the node since it is standalone (has no root for which this normally belongs)
            node.AdditionalData["treeAlias"] = TreeAlias;

            return node;
        }

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            var alias = id;
            var currentItemConfig = alias == "-1" ? TreeConfig : TreeConfig.FalttenedTreeItems[alias];

            var currentFolderConfig = currentItemConfig as FluidityContainerTreeItemConfig;
            if (currentFolderConfig != null)
            {
                // Render the folder contents
                foreach (var treeItem in currentFolderConfig.TreeItems.Values)
                {
                    var folderTreeItem = treeItem as FluidityFolderConfig;
                    if (folderTreeItem != null)
                    {
                        // Render folder
                        var node = CreateTreeNode(
                            folderTreeItem.Alias,
                            folderTreeItem.ParentAlias,
                            queryStrings,
                            folderTreeItem.Name,
                            folderTreeItem.Icon,
                            true,
                            folderTreeItem.TreeMode == FluidityTreeMode.Tree
                                ? SectionAlias // Tree mode so just show the default dashboard
                                : SectionAlias + "/fluidity/folder/" + folderTreeItem.Alias);

                        node.Path = folderTreeItem.Path;

                        if (folderTreeItem.TreeMode == FluidityTreeMode.List)
                        {
                            node.SetContainerStyle();
                        }

                        nodes.Add(node);
                    }

                    var collectionTreeItem = treeItem as FluidityCollectionConfig;
                    if (collectionTreeItem != null && collectionTreeItem.VisibleInTree)
                    {
                        // Render collection folder
                        var node = CreateTreeNode(
                            collectionTreeItem.Alias,
                            collectionTreeItem.ParentAlias,
                            queryStrings,
                            collectionTreeItem.NamePlural,
                            collectionTreeItem.IconPlural,
                            collectionTreeItem.TreeMode != FluidityTreeMode.List,
                            collectionTreeItem.TreeMode == FluidityTreeMode.Tree 
                                ? SectionAlias // Tree mode so just show the default dashboard
                                : SectionAlias + "/fluidity/collection/" + collectionTreeItem.Alias);

                        node.Path = collectionTreeItem.Path;

                        if (collectionTreeItem.TreeMode == FluidityTreeMode.List)
                        {
                            node.SetContainerStyle();
                        }

                        nodes.Add(node);
                    }
                }
            }

            var currentCollectionConfig = currentItemConfig as FluidityCollectionConfig;
            if (currentCollectionConfig != null && currentCollectionConfig.VisibleInTree && currentCollectionConfig.TreeMode != FluidityTreeMode.List)
            {
                // Render collection items
                var items = Context.Services.EntityService.GetAll(currentCollectionConfig);
                nodes.AddRange(items.Select(item => CreateEntityTreeNode(currentCollectionConfig, item, queryStrings)));
            }

            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection(); 

            var createText = Services.TextService.Localize("actions/" + ActionNew.Instance.Alias);
            var deleteText = Services.TextService.Localize("actions/" + ActionDelete.Instance.Alias);
            var refreshText = Services.TextService.Localize("actions/" + ActionRefresh.Instance.Alias);

            var alias = id;
            object entityId = null; 

            // Extract the entity Id if we have one
            var seperatorIndex = alias.IndexOf("!", StringComparison.InvariantCulture);
            if (seperatorIndex >= 0)
            {
                entityId = alias.Substring(seperatorIndex + 1);
                alias = alias.Substring(0, seperatorIndex);
            }
            
            var currentItemConfig = alias == "-1" ? TreeConfig : TreeConfig.FalttenedTreeItems[alias];

            var currentFolderConfig = currentItemConfig as FluidityFolderConfig;
            if (currentFolderConfig != null)
            {
                if (currentFolderConfig.TreeMode != FluidityTreeMode.List)
                {
                    menu.Items.Add<RefreshNode, ActionRefresh>(refreshText, true);
                }
            }

            var currentCollectionConfig = currentItemConfig as FluidityCollectionConfig;
            if (currentCollectionConfig != null)
            {
                if (entityId != null) // If we have an id, we must be editing an entity
                {
                    if (!currentCollectionConfig.ReadOnly)
                    {
                        // We create a custom item as we need to direct all fluidity delete commands to the
                        // same view, where as the in built delete dialog looks for seperate views per tree
                        var menuItem = new MenuItem("delete", deleteText) { Icon = "delete" };
                        menuItem.LaunchDialogView(IOHelper.ResolveUrl($"{SystemDirectories.AppPlugins}/fluidity/backoffice/fluidity/delete.html"), deleteText);
                        menu.Items.Add(menuItem);
                    }
                }
                else
                {
                    if (!currentCollectionConfig.ReadOnly)
                    {
                        // We create a custom item as we need to direct all fluidity create commands to the
                        // same view, where as the in built create dialog looks for seperate views per tree
                        var menuItem = new MenuItem("create", createText) { Icon = "add" };
                        menuItem.NavigateToRoute(SectionAlias + "/fluidity/edit/" + currentCollectionConfig.Alias);
                        menu.Items.Add(menuItem);
                    }

                    if (currentCollectionConfig.TreeMode != FluidityTreeMode.List)
                    {
                        menu.Items.Add<RefreshNode, ActionRefresh>(refreshText, true);
                    }
                }
            }

            return menu;
        }

        protected TreeNode CreateEntityTreeNode(FluidityCollectionConfig collectionConfig, object entity, FormDataCollection queryStrings)
        {
            var itemId = entity.GetPropertyValue(collectionConfig.IdProperty);
            var compositeId = collectionConfig.Alias + "!" + itemId;

            var node = CreateTreeNode(
                compositeId,
                collectionConfig.Alias,
                queryStrings,
                entity.ToString(),
                collectionConfig.IconSingular,
                false,
                SectionAlias + "/fluidity/edit/" + compositeId);

            node.Path = collectionConfig.Path + FluidityConstants.PATH_SEPERATOR + compositeId;
            node.AdditionalData.AddOrUpdate("entityId", itemId);

            return node;
        }
    }
}
