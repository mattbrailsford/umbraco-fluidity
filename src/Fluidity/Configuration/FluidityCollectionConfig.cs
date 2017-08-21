using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;

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

        protected FluidityViewMode _viewMode;
        internal FluidityViewMode ViewMode => _viewMode;

        protected Direction _sortDirection;
        internal Direction SortDirection => _sortDirection;

        protected PropertyInfo _sortProperty;
        internal PropertyInfo SortProperty => _sortProperty;

        protected Expression _sortPropertyExp;
        internal Expression SortPropertyExp => _sortPropertyExp;

        protected PropertyInfo _dateCreatedProperty;
        internal PropertyInfo DateCreatedProperty => _dateCreatedProperty;

        protected Expression _dateCreatedPropertyExp;
        internal Expression DateCreatedPropertyExp => _dateCreatedPropertyExp;

        protected PropertyInfo _dateModifiedProperty;
        internal PropertyInfo DateModifiedProperty => _dateModifiedProperty;

        protected Expression _dateModifiedPropertyExp;
        internal Expression DateModifiedPropertyExp => _dateModifiedPropertyExp;

        protected PropertyInfo _deletedProperty;
        internal PropertyInfo DeletedProperty => _deletedProperty;

        protected Expression _deletedPropertyExp;
        internal Expression DeletedPropertyExp => _deletedPropertyExp;

        protected FluidityListViewConfig _listView;
        internal FluidityListViewConfig ListView => _listView;

        protected FluidityEditorConfig _editor;
        internal FluidityEditorConfig Editor => _editor;

        protected Func<object, string> _nameFormat;
        internal Func<object, string> NameFormat => _nameFormat;

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