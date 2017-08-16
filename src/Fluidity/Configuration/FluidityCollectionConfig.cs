using System;
using System.Reflection;
using Umbraco.Core;

namespace Fluidity.Configuration
{
    public abstract class FluidityCollectionConfig : FluidityTreeItemConfig
    {
        protected PropertyInfo _idProperty;
        internal PropertyInfo IdProperty => _idProperty;

        protected string _nameSingular;
        internal string NameSignular => _nameSingular;

        protected string _namePlural;
        internal string NamePlural => _namePlural;

        protected string _iconSingular;
        internal string IconSingular => _iconSingular;

        protected string _iconPlural;
        internal string IconPlural => _iconPlural;

        protected Type _entityType;
        internal Type EntityType => _entityType;

        protected Type _repositoryType;
        internal Type RepositoryType => _repositoryType;

        protected bool _visibleOnDashboard;
        internal bool VisibleOnDashboard => _visibleOnDashboard;

        protected bool _visibleInTree;
        internal bool VisibleInTree => _visibleInTree;

        protected bool _readOnly;
        internal bool ReadOnly => _readOnly;

        protected FluidityTreeMode _treeMode;
        internal FluidityTreeMode TreeMode => _treeMode;

        protected FluiditySortOrder _sortOrder;
        internal FluiditySortOrder SortOrder => _sortOrder;

        protected PropertyInfo _sortProperty;
        internal PropertyInfo SortProperty => _sortProperty;

        protected PropertyInfo _dateCreatedProperty;
        internal PropertyInfo DateCreated => _dateCreatedProperty;

        protected PropertyInfo _dateModifiedProperty;
        internal PropertyInfo DateModified => _dateModifiedProperty;

        protected PropertyInfo _deletedProperty;
        internal PropertyInfo DeletedProperty => _deletedProperty;

        protected FluidityListViewConfig _listView;
        internal FluidityListViewConfig ListView => _listView;

        protected FluidityEditorConfig _editor;
        internal FluidityEditorConfig Editor => _editor;

        protected FluidityCollectionConfig(Type entityType, PropertyInfo idProperty, string nameSingular, string namePlural, string iconSingular = null, string iconPlural = null)
        {
            _idProperty = idProperty;
            _entityType = entityType;
            _alias = nameSingular.ToSafeAlias(true);
            _nameSingular = nameSingular;
            _namePlural = namePlural;
            _iconSingular = iconSingular ?? "icon-folder";
            _iconPlural = iconPlural ?? "icon-folder";
            _visibleInTree = true;
        }
    }
}