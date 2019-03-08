// <copyright file="FluidityContainerTreeItemConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fluidity.Collections;
using Umbraco.Core;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Base class for Fluidity container tree item configuration
    /// </summary>
    /// <seealso cref="Fluidity.Configuration.FluidityTreeItemConfig" />
    public abstract class FluidityContainerTreeItemConfig : FluidityTreeItemConfig
    {
        protected string _name;
        internal string Name => _name;

        protected string _icon;
        internal string Icon => _icon;

        protected ConcurrentDictionary<string, FluidityTreeItemConfig> _treeItems;
        internal IReadOnlyDictionary<string, FluidityTreeItemConfig> TreeItems => _treeItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityContainerTreeItemConfig"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="icon">The icon.</param>
        protected FluidityContainerTreeItemConfig(string name, string icon = null)
        {
            _alias = name.ToSafeAlias(true);
            _name = name;
            _icon = icon ?? "icon-folder";

            _treeItems = new ConcurrentDictionary<string, FluidityTreeItemConfig>();
        }

        /// <summary>
        /// Adds a folder to the container.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="folderConfig">A folder configuration delegate.</param>
        /// <returns>The folder configuration.</returns>
        public virtual FluidityFolderConfig AddFolder(string name, Action<FluidityFolderConfig> folderConfig = null)
        {
            return AddFolder(new FluidityFolderConfig(name, config: folderConfig));
        }

        /// <summary>
        /// Adds a folder to the container.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="folderConfig">A folder configuration delegate.</param>
        /// <returns>The folder configuration.</returns>
        public virtual FluidityFolderConfig AddFolder(string name, string icon, Action<FluidityFolderConfig> folderConfig = null)
        {
            return AddFolder(new FluidityFolderConfig(name, icon, folderConfig));
        }

        /// <summary>
        /// Adds a folder to the container.
        /// </summary>
        /// <param name="folderConfig">A folder configuration.</param>
        /// <returns>The folder configuration.</returns>
        public virtual FluidityFolderConfig AddFolder(FluidityFolderConfig folderConfig)
        {
            var folder = folderConfig;
            folder.Ordinal = _treeItems.Count + 1;
            _treeItems.AddOrUpdate(folder.Alias, folder, (s, config) => { throw new ApplicationException($"A tree item with the alias '{config.Alias}' has already been added"); });
            return folder;
        }

        /// <summary>
        /// Adds a collection to the container.
        /// </summary>
        /// <typeparam name="TEntityType">The collection entity type.</typeparam>
        /// <param name="idPropertyExpression">The identifier property expression.</param>
        /// <param name="nameSingular">The singular name.</param>
        /// <param name="namePlural">The plural name.</param>
        /// <param name="description">A description.</param>
        /// <param name="collectionConfig">A collection configuration delegate.</param>
        /// <returns>The collection configuration.</returns>
        public virtual FluidityCollectionConfig<TEntityType> AddCollection<TEntityType>(Expression<Func<TEntityType, object>> idPropertyExpression, string nameSingular, string namePlural, string description, Action<FluidityCollectionConfig<TEntityType>> collectionConfig = null)
        {
            return AddCollection(new FluidityCollectionConfig<TEntityType>(idPropertyExpression, nameSingular, namePlural, description, config: collectionConfig));
        }

        /// <summary>
        /// Adds a collection to the container.
        /// </summary>
        /// <typeparam name="TEntityType">The collection entity type.</typeparam>
        /// <param name="idPropertyExpression">The identifier property expression.</param>
        /// <param name="nameSingular">The singular name.</param>
        /// <param name="namePlural">The plural name.</param>
        /// <param name="description">The description.</param>
        /// <param name="iconSingular">The singular icon.</param>
        /// <param name="iconPlural">The plural icon.</param>
        /// <param name="collectionConfig">A collection configuration delegate.</param>
        /// <returns>The collection configuration.</returns>
        public virtual FluidityCollectionConfig<TEntityType> AddCollection<TEntityType>(Expression<Func<TEntityType, object>> idPropertyExpression, string nameSingular, string namePlural, string description, string iconSingular, string iconPlural, Action<FluidityCollectionConfig<TEntityType>> collectionConfig = null)
        {
            return AddCollection(new FluidityCollectionConfig<TEntityType>(idPropertyExpression, nameSingular, namePlural, description, iconSingular, iconPlural, collectionConfig));
        }

        /// <summary>
        /// Adds a collection to the container.
        /// </summary>
        /// <typeparam name="TEntityType">The collection entity type.</typeparam>
        /// <param name="collectionConfig">The collection configuration.</param>
        /// <returns>The collection configuration.</returns>
        public virtual FluidityCollectionConfig<TEntityType> AddCollection<TEntityType>(FluidityCollectionConfig<TEntityType> collectionConfig)
        {
            var collection = collectionConfig;
            collection.Ordinal = _treeItems.Count + 1;
            _treeItems.AddOrUpdate(collection.Alias, collection, (s, config) => { throw new ApplicationException($"A tree item with the alias '{config.Alias}' has already been added"); });
            return collection;
        }
    }
}