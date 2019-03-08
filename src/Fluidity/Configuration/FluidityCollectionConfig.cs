// <copyright file="FluidityCollectionConfig.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Un-typed base class for a <see cref="FluidityCollectionConfig{TEntityType}"/>
    /// </summary>
    /// <seealso cref="Fluidity.Configuration.FluidityTreeItemConfig" />
    public abstract class FluidityCollectionConfig : FluidityTreeItemConfig
    {
        protected FluidityPropertyConfig _idProperty;
        internal FluidityPropertyConfig IdProperty => _idProperty;

        protected FluidityPropertyConfig _nameProperty;
        internal FluidityPropertyConfig NameProperty => _nameProperty;

        protected Func<object, string> _nameFormat;
        internal Func<object, string> NameFormat => _nameFormat;

        protected string _nameSingular;
        internal string NameSingular => _nameSingular;

        protected string _namePlural;
        internal string NamePlural => _namePlural;

        protected string _iconSingular;
        internal string IconSingular => _iconSingular;

        protected string _iconPlural;
        internal string IconPlural => _iconPlural;

        protected string _description;
        internal string Description => _description;

        protected string _connectionString;
        internal string ConnectionString => _connectionString;

        protected Type _entityType;
        internal Type EntityType => _entityType;

        protected Type _repositoryType;
        internal Type RepositoryType => _repositoryType;

        protected bool _isVisibleOnDashboard;
        internal bool IsVisibleOnDashboard => _isVisibleOnDashboard;

        protected bool _isVisibleInTree;
        internal bool IsVisibleInTree => _isVisibleInTree;

        protected bool _canCreate;
        internal bool CanCreate => _canCreate;

        protected bool _canUpdate;
        internal bool CanUpdate => _canUpdate;

        protected bool _canDelete;
        internal bool CanDelete => _canDelete;

        protected FluidityViewMode _viewMode;
        internal FluidityViewMode ViewMode => _viewMode;

        protected SortDirection _sortDirection;
        internal SortDirection SortDirection => _sortDirection;

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

        protected List<FluidityPropertyConfig> _encryptedProperties;
        internal IEnumerable<FluidityPropertyConfig> EncryptedProperties => _encryptedProperties;

        protected LambdaExpression _filterExpression;
        internal LambdaExpression FilterExpression => _filterExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityCollectionConfig"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="idPropertyExpression">The identifier property expression.</param>
        /// <param name="nameSingular">The singular name.</param>
        /// <param name="namePlural">The plural name.</param>
        /// <param name="description">The description.</param>
        /// <param name="iconSingular">The singular icon.</param>
        /// <param name="iconPlural">The plural icon.</param>
        protected FluidityCollectionConfig(Type entityType, LambdaExpression idPropertyExpression, string nameSingular, string namePlural, string description, string iconSingular = null, string iconPlural = null)
        {
            _idProperty = idPropertyExpression;
            _entityType = entityType;
            _alias = nameSingular.ToSafeAlias(true);
            _nameSingular = nameSingular;
            _namePlural = namePlural;
            _description = description;
            _iconSingular = iconSingular ?? "icon-folder";
            _iconPlural = iconPlural ?? "icon-folder";
            _isVisibleInTree = true;
            _canCreate = true;
            _canUpdate = true;
            _canDelete = true;

            _containerMenuItems = new List<MenuItem>();
            _entityMenuItems = new List<MenuItem>();
            _searchableProperties = new List<FluidityPropertyConfig>();
            _encryptedProperties = new List<FluidityPropertyConfig>();
        }
    }
}