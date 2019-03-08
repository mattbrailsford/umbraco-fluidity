// <copyright file="FluidityTreeController.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Fluidity.Configuration;
using Fluidity.Extensions;
using Fluidity.Web.Extensions;
using umbraco.BusinessLogic.Actions;
using Umbraco.Core;
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

            var collectionConfig = TreeConfig.FlattenedTreeItems[alias] as FluidityCollectionConfig;
            if (collectionConfig == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            var entity = Context.Services.EntityService.GetEntity(collectionConfig, entityId);
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
            var currentItemConfig = alias == "-1" ? TreeConfig : TreeConfig.FlattenedTreeItems[alias];

            var currentFolderConfig = currentItemConfig as FluidityContainerTreeItemConfig;
            if (currentFolderConfig != null)
            {
                // Render the folder contents
                foreach (var treeItem in currentFolderConfig.TreeItems.Values.OrderBy(x => x.Ordinal))
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
                            SectionAlias); // Tree mode so just show the default dashboard

                        if (!currentFolderConfig.IconColor.IsNullOrWhiteSpace())
                        {
                            node.SetColorStyle(currentFolderConfig.IconColor);
                        }

                        node.Path = folderTreeItem.Path;

                        nodes.Add(node);
                    }

                    var collectionTreeItem = treeItem as FluidityCollectionConfig;
                    if (collectionTreeItem != null && collectionTreeItem.IsVisibleInTree)
                    {
                        // Render collection folder
                        var node = CreateTreeNode(
                            collectionTreeItem.Alias,
                            collectionTreeItem.ParentAlias,
                            queryStrings,
                            collectionTreeItem.NamePlural,
                            collectionTreeItem.IconPlural,
                            collectionTreeItem.ViewMode == FluidityViewMode.Tree,
                            collectionTreeItem.ViewMode == FluidityViewMode.Tree 
                                ? SectionAlias // Tree mode so just show the default dashboard
                                : SectionAlias + "/fluidity/list/" + collectionTreeItem.Alias);

                        node.Path = collectionTreeItem.Path;

                        if (collectionTreeItem.ViewMode == FluidityViewMode.List)
                        {
                            node.SetContainerStyle();
                        }

                        if (!collectionTreeItem.IconColor.IsNullOrWhiteSpace())
                        {
                            node.SetColorStyle(collectionTreeItem.IconColor);
                        }

                        nodes.Add(node);
                    }
                }
            }

            var currentCollectionConfig = currentItemConfig as FluidityCollectionConfig;
            if (currentCollectionConfig != null && currentCollectionConfig.IsVisibleInTree && currentCollectionConfig.ViewMode == FluidityViewMode.Tree)
            {
                // Render collection items
                var items = Context.Services.EntityService.GetAllEntities(currentCollectionConfig);
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
            
            var currentItemConfig = alias == "-1" ? TreeConfig : TreeConfig.FlattenedTreeItems[alias];

            var currentFolderConfig = currentItemConfig as FluidityFolderConfig;
            if (currentFolderConfig != null)
            {
                menu.Items.Add<RefreshNode, ActionRefresh>(refreshText, true);
            }

            var currentCollectionConfig = currentItemConfig as FluidityCollectionConfig;
            if (currentCollectionConfig != null)
            {
                if (entityId != null) // If we have an id, we must be editing an entity
                {
                    var hasMenuItems = false;

                    if (currentCollectionConfig.EntityMenuItems.Any())
                    {
                        menu.Items.AddRange(currentCollectionConfig.EntityMenuItems);
                        hasMenuItems = true;
                    }

                    if (currentCollectionConfig.CanDelete)
                    {
                        // We create a custom item as we need to direct all fluidity delete commands to the
                        // same view, where as the in built delete dialog looks for seperate views per tree
                        var menuItem = new MenuItem("delete", deleteText) { Icon = "delete" };
                        menuItem.LaunchDialogView(IOHelper.ResolveUrl($"{SystemDirectories.AppPlugins}/fluidity/backoffice/fluidity/delete.html"), deleteText);
                        menuItem.SeperatorBefore = hasMenuItems;
                        menu.Items.Add(menuItem);
                    }
                }
                else
                {
                    var hasMenuItems = false;

                    if (currentCollectionConfig.CanCreate)
                    {
                        // We create a custom item as we need to direct all fluidity create commands to the
                        // same view, where as the in built create dialog looks for seperate views per tree
                        var menuItem = new MenuItem("create", createText) { Icon = "add" };
                        menuItem.NavigateToRoute(SectionAlias + "/fluidity/edit/" + currentCollectionConfig.Alias);
                        menu.Items.Add(menuItem);
                        hasMenuItems = true;
                    }

                    if (currentCollectionConfig.ContainerMenuItems.Any())
                    {
                        if (hasMenuItems)
                        {
                            currentCollectionConfig.ContainerMenuItems.First().SeperatorBefore = true;
                        }

                        menu.Items.AddRange(currentCollectionConfig.ContainerMenuItems);
                    }

                    if (currentCollectionConfig.ViewMode == FluidityViewMode.Tree)
                    {
                        menu.Items.Add<RefreshNode, ActionRefresh>(refreshText, true);

                        if (hasMenuItems)
                        {
                            menu.Items.Last().SeperatorBefore = true;
                        }
                    }
                }
            }

            return menu;
        }

        protected TreeNode CreateEntityTreeNode(FluidityCollectionConfig collection, object entity, FormDataCollection queryStrings)
        {
            var itemId = entity.GetPropertyValue(collection.IdProperty);
            var compositeId = collection.Alias + "!" + itemId;

            var entityName = collection.NameProperty != null ? entity.GetPropertyValue(collection.NameProperty).ToString()
                                : collection.NameFormat != null ? collection.NameFormat(entity) : entity.ToString();

            var node = CreateTreeNode(
                compositeId,
                collection.Alias,
                queryStrings,
                entityName,
                collection.IconSingular,
                false,
                SectionAlias + "/fluidity/edit/" + compositeId);

            if (!collection.IconColor.IsNullOrWhiteSpace())
            {
                node.SetColorStyle(collection.IconColor);
            }

            node.Path = collection.Path + FluidityConstants.PATH_SEPERATOR + compositeId;
            node.AdditionalData.AddOrUpdate("entityId", itemId);
            node.AdditionalData.AddOrUpdate("collectionAlias", collection.Alias);
            node.AdditionalData.AddOrUpdate("sectionAlias", SectionAlias);
            node.AdditionalData.AddOrUpdate("dashboardRoute", collection.ViewMode == FluidityViewMode.Tree
                ? SectionAlias // Tree mode so just show the default dashboard
                : SectionAlias + "/fluidity/list/" + collection.Alias); // List view so show the list

            return node;
        }
    }
}
