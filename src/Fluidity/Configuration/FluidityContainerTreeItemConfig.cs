using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fluidity.Collections;
using Umbraco.Core;

namespace Fluidity.Configuration
{
    public abstract class FluidityContainerTreeItemConfig : FluidityTreeItemConfig
    {
        protected string _name;
        internal string Name => _name;

        protected string _icon;
        internal string Icon => _icon;

        protected ConcurrentDictionary<string, FluidityTreeItemConfig> _treeItems;
        internal IReadOnlyDictionary<string, FluidityTreeItemConfig> TreeItems => _treeItems;

        protected FluidityContainerTreeItemConfig(string name, string icon = null)
        {
            _alias = name.ToSafeAlias(true);
            _name = name;
            _icon = icon ?? "icon-folder";

            _treeItems = new ConcurrentDictionary<string, FluidityTreeItemConfig>();
        }

        public virtual FluidityFolderConfig AddFolder(string name, Action<FluidityFolderConfig> folderConfig = null)
        {
            return AddFolder(new FluidityFolderConfig(name, config: folderConfig));
        }

        public virtual FluidityFolderConfig AddFolder(string name, string icon, Action<FluidityFolderConfig> folderConfig = null)
        {
            return AddFolder(new FluidityFolderConfig(name, icon, folderConfig));
        }

        public virtual FluidityFolderConfig AddFolder(FluidityFolderConfig folderConfig)
        {
            var folder = folderConfig;
            _treeItems.AddOrUpdate(folder.Alias, folder, (s, config) => { throw new ApplicationException($"A tree item with the alias '{config.Alias}' has already been added"); });
            return folder;
        }

        public virtual FluidityCollectionConfig<TEntityType> AddCollection<TEntityType>(Expression<Func<TEntityType, object>> idFieldExpression, string nameSingular, string namePlural, string description, Action<FluidityCollectionConfig<TEntityType>> collectionConfig = null)
        {
            return AddCollection(new FluidityCollectionConfig<TEntityType>(idFieldExpression, Path, nameSingular, namePlural, description, config: collectionConfig));
        }

        public virtual FluidityCollectionConfig<TEntityType> AddCollection<TEntityType>(Expression<Func<TEntityType, object>> idFieldExpression, string nameSingular, string namePlural, string description, string iconSingular, string iconPlural, Action<FluidityCollectionConfig<TEntityType>> collectionConfig = null)
        {
            return AddCollection(new FluidityCollectionConfig<TEntityType>(idFieldExpression, nameSingular, namePlural, description, iconSingular, iconPlural, collectionConfig));
        }

        public virtual FluidityCollectionConfig<TEntityType> AddCollection<TEntityType>(FluidityCollectionConfig<TEntityType> collectionConfig)
        {
            var collection = collectionConfig;
            _treeItems.AddOrUpdate(collection.Alias, collection, (s, config) => { throw new ApplicationException($"A tree item with the alias '{config.Alias}' has already been added"); });
            return collection;
        }
    }
}