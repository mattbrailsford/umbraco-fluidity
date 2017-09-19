using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Umbraco.Core;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
using Umbraco.Web.Models.Trees;

namespace Fluidity.Configuration
{
    public abstract class FluidityCollectionConfig : FluidityTreeItemConfig
    {
        protected FluidityPropertyConfig _idProperty;
        internal FluidityPropertyConfig IdProperty => _idProperty;

        protected FluidityPropertyConfig _nameProperty;
        internal FluidityPropertyConfig NameProperty => _nameProperty;

        protected Func<object, string> _nameFormat;
        internal Func<object, string> NameFormat => _nameFormat;

        protected string _nameSingular;
        internal string NameSignular => _nameSingular;

        protected string _namePlural;
        internal string NamePlural => _namePlural;

        protected string _iconSingular;
        internal string IconSingular => _iconSingular;

        protected string _iconPlural;
        internal string IconPlural => _iconPlural;

        protected string _description;
        internal string Description => _description;

        protected string _connectionStringg;
        internal string ConnectionString => _connectionStringg;

        protected Type _entityType;
        internal Type EntityType => _entityType;

        protected Type _repositoryType;
        internal Type RepositoryType => _repositoryType;

        protected bool _isVisibleOnDashboard;
        internal bool IsVisibleOnDashboard => _isVisibleOnDashboard;

        protected bool _isVisibleInTree;
        internal bool IsVisibleInTree => _isVisibleInTree;

        protected bool _isReadOnly;
        internal bool IsReadOnly => _isReadOnly;

        protected FluidityViewMode _viewMode;
        internal FluidityViewMode ViewMode => _viewMode;

        protected Direction _sortDirection;
        internal Direction SortDirection => _sortDirection;

        protected FluidityPropertyConfig _sortProperty;
        internal FluidityPropertyConfig SortProperty => _sortProperty;

        protected FluidityPropertyConfig _dateCreatedProperty;
        internal FluidityPropertyConfig DateCreatedProperty => _dateCreatedProperty;

        protected FluidityPropertyConfig _dateModifiedProperty;
        internal FluidityPropertyConfig DateModifiedProperty => _dateModifiedProperty;

        protected FluidityPropertyConfig _deletedProperty;
        internal FluidityPropertyConfig DeletedProperty => _deletedProperty;

        protected FluidityListViewConfig _listView;
        internal FluidityListViewConfig ListView => _listView;

        protected FluidityEditorConfig _editor;
        internal FluidityEditorConfig Editor => _editor;

        protected List<MenuItem> _containerMenuItems;
        internal IEnumerable<MenuItem> ContainerMenuItems => _containerMenuItems;

        protected List<MenuItem> _entityMenuItems;
        internal IEnumerable<MenuItem> EntityMenuItems => _entityMenuItems;

        protected List<FluidityPropertyConfig> _searchableProperties;
        internal IEnumerable<FluidityPropertyConfig> SearchableProperties => _searchableProperties;

        protected FluidityCollectionConfig(Type entityType, LambdaExpression idPropertyExp, string nameSingular, string namePlural, string description, string iconSingular = null, string iconPlural = null)
        {
            _idProperty = idPropertyExp;
            _entityType = entityType;
            _alias = nameSingular.ToSafeAlias(true);
            _nameSingular = nameSingular;
            _namePlural = namePlural;
            _description = description;
            _iconSingular = iconSingular ?? "icon-folder";
            _iconPlural = iconPlural ?? "icon-folder";
            _isVisibleInTree = true;

            _containerMenuItems = new List<MenuItem>();
            _entityMenuItems = new List<MenuItem>();
            _searchableProperties = new List<FluidityPropertyConfig>();
        }
    }
}