// <copyright file="FluidityCollectionConfig`T.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fluidity.Actions;
using Fluidity.Data;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Strongly type based class for Fluidity collection configuration
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity.</typeparam>
    /// <seealso cref="Fluidity.Configuration.FluidityCollectionConfig" />
    public class FluidityCollectionConfig<TEntityType> : FluidityCollectionConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluidityCollectionConfig{TEntityType}"/> class.
        /// </summary>
        /// <param name="idPropertyExpression">The identifier property expression.</param>
        /// <param name="nameSingular">The singular name.</param>
        /// <param name="namePlural">The plural name.</param>
        /// <param name="description">The description.</param>
        /// <param name="iconSingular">The singular icon.</param>
        /// <param name="iconPlural">The plural icon.</param>
        /// <param name="config">A configuration delegate.</param>
        public FluidityCollectionConfig(Expression<Func<TEntityType, object>> idPropertyExpression, string nameSingular, string namePlural, string description, string iconSingular = null, string iconPlural = null, Action<FluidityCollectionConfig<TEntityType>> config = null)
            : base (typeof(TEntityType), idPropertyExpression, nameSingular, namePlural, description, iconSingular, iconPlural)
        {
            config?.Invoke(this);
        }

        /// <summary>
        /// Changes the alias of the collection.
        /// </summary>
        /// <remarks>
        /// An alias will automatically be generated from the collection name however you can use SetAlias to change this if required
        /// </remarks>
        /// <param name="alias">The alias.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        /// <summary>
        /// Changes the icon color of the collection icons.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetIconColor(string color)
        {
            _iconColor = color;
            return this;
        }

        /// <summary>
        /// Sets the name property of the collection.
        /// </summary>
        /// <remarks>
        /// By default the name property will also become the default sort property and will be added to the searchable properties collection
        /// </remarks>
        /// <param name="namePropertyExpression">The name property expression.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetNameProperty(Expression<Func<TEntityType, string>> namePropertyExpression)
        {
            _nameProperty = namePropertyExpression;
            _searchableProperties.Add(_nameProperty);

            if (_sortProperty == null)
            {
                // Convert lambda to object based property accessor for the sort property
                var entityParam = namePropertyExpression.Parameters[0];
                var memberExp = (MemberExpression)namePropertyExpression.Body;
                var convertExp = Expression.Convert(memberExp, typeof(object));
                var lambda = Expression.Lambda<Func<TEntityType, object>>(convertExp, entityParam);

                _sortProperty = lambda;
            }

            return this;
        }

        /// <summary>
        /// Sets a name format function for when there is no single name property on an entity.
        /// </summary>
        /// <param name="format">The format function.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetNameFormat(Func<TEntityType, string> format)
        {
            _nameFormat = (entity) => format((TEntityType)entity);
            return this;
        }

        /// <summary>
        /// Sets the connection string for the collection.
        /// </summary>
        /// <remarks>
        /// Default to using the Umbraco connection string.
        /// </remarks>
        /// <param name="connectionStringName">Name of the connection string to use.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetConnectionString(string connectionStringName)
        {
            _connectionString = connectionStringName;
            return this;
        }

        /// <summary>
        /// Changes the repository to use for this collection.
        /// </summary>
        /// <remarks>
        /// The default repository uses a simple PetaPoco based data strategy.
        /// </remarks>
        /// <typeparam name="TRepositoryType">The type of the repository to use.</typeparam>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetRepositoryType<TRepositoryType>()
            where TRepositoryType : IFluidityRepository
        {
            return SetRepositoryType(typeof(TRepositoryType));
        }

        /// <summary>
        /// Changes the repository to use for this collection.
        /// </summary>
        /// <remarks>
        /// The default repository uses a simple PetaPoco based data strategy.
        /// </remarks>
        /// <param name="repositoryType">Type of the repository to use.</param>
        /// <returns>The collection configuration.</returns>
        /// <exception cref="System.ArgumentException">repositoryType</exception>
        public FluidityCollectionConfig<TEntityType> SetRepositoryType(Type repositoryType)
        {
            if (!repositoryType.Implements<IFluidityRepository>())
            {
                throw new ArgumentException($"{repositoryType.Name} does not implement IFluidityRepository", nameof(repositoryType));
            }

            _repositoryType = repositoryType;
            return this;
        }

        /// <summary>
        /// Sets which property to use as a 'Deleted' flag.
        /// </summary>
        /// <param name="deletedPropertyExpression">The deleted property expression.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetDeletedProperty(Expression<Func<TEntityType, bool>> deletedPropertyExpression)
        {
            _deletedProperty = deletedPropertyExpression;
            return this;
        }

        /// <summary>
        /// Sets the default property to sort on.
        /// </summary>
        /// <remarks>
        /// Defaults to asscending sort order.
        /// </remarks>
        /// <param name="sortPropertyExpression">The sort property expression.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetSortProperty(Expression<Func<TEntityType, object>> sortPropertyExpression)
        {
            return SetSortProperty(sortPropertyExpression, SortDirection.Ascending);
        }

        /// <summary>
        /// Sets the default property to sort on and the desired sort direction.
        /// </summary>
        /// <param name="sortPropertyExpression">The sort property expression.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetSortProperty(Expression<Func<TEntityType, object>> sortPropertyExpression, SortDirection sortDirection)
        {

            _sortProperty = sortPropertyExpression;
            _sortDirection = sortDirection;
            return this;
        }

        /// <summary>
        /// Sets a filter for the collection.
        /// </summary>
        /// <param name="filterExpression">The filter where clause expression.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetFilter(Expression<Func<TEntityType, bool>> whereClause)
        {
            _filterExpression = whereClause;
            return this;
        }

        /// <summary>
        /// Shows the collection on the section dashboard.
        /// </summary>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> ShowOnDashboard()
        {
            _isVisibleOnDashboard = true;
            return this;
        }

        /// <summary>
        /// Hides the collection from the section tree.
        /// </summary>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> HideFromTree()
        {
            _isVisibleInTree = false;
            return this;
        }

        /// <summary>
        /// Makes the collection read only.
        /// </summary>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> MakeReadOnly()
        {                        
            DisableCreate();
            DisableUpdate();
            DisableDelete();
            return this;
        }

        /// <summary>
        /// Disable creating entities in collection.
        /// </summary>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> DisableCreate()
        {
            _canCreate = false;
            return this;
        }

        /// <summary>
        /// Disable updating entities in collection.
        /// </summary>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> DisableUpdate()
        {
            _canUpdate = false;
            return this;
        }

        /// <summary>
        /// Disable deleting entities in collection.
        /// </summary>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> DisableDelete()
        {
            _canDelete = false;        
            if (_listView != null)
            {
                _listView.DefaultBulkActions.RemoveAll(x => x.GetType() == typeof(FluidityDeleteBulkAction));
            }
            
            return this;
        }

        /// <summary>
        /// Sets the view mode of the collection. Can be either 'Tree' or 'List'.
        /// </summary>
        /// <param name="viewMode">The view mode.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetViewMode(FluidityViewMode viewMode)
        {
            _viewMode = viewMode;
            return this;
        }

        /// <summary>
        /// Adds a menu item to collection container.
        /// </summary>
        /// <typeparam name="TMenuItem">The type of the menu item.</typeparam>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> AddContainerMenuItem<TMenuItem>()
            where TMenuItem : MenuItem, new()
        {
            _containerMenuItems.Add(new TMenuItem());
            return this;
        }

        /// <summary>
        /// Adds a menu item to collection container.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> AddContainerMenuItem(MenuItem menuItem)
        {
            _containerMenuItems.Add(menuItem);
            return this;
        }

        /// <summary>
        /// Adds a menu item to a collections entities.
        /// </summary>
        /// <typeparam name="TMenuItem">The type of the menu item.</typeparam>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> AddEntityMenuItem<TMenuItem>()
            where TMenuItem : MenuItem, new()
        {
            _entityMenuItems.Add(new TMenuItem());
            return this;
        }

        /// <summary>
        /// Adds a menu item to a collections entities.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> AddEntityMenuItem(MenuItem menuItem)
        {
            _entityMenuItems.Add(menuItem);
            return this;
        }

        /// <summary>
        /// Adds a property to the searchable properties collection.
        /// </summary>
        /// <param name="searchablePropertyExpression">The searchable property expression.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> AddSearchableProperty(Expression<Func<TEntityType, string>> searchablePropertyExpression)
        {
            _searchableProperties.Add(searchablePropertyExpression);
            return this;
        }

        /// <summary>
        /// Adds a property that will be encrypted / decrypted when retrived from the repository.
        /// </summary>
        /// <param name="encryptedPropertyExpression">The encrypted property expression.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> AddEncryptedProperty(Expression<Func<TEntityType, string>> encryptedPropertyExpression)
        {
            _encryptedProperties.Add(encryptedPropertyExpression);
            return this;
        }

        /// <summary>
        /// Sets which property to use as the Date Created property.
        /// </summary>
        /// <param name="dateCreatedPropertyExpression">The date created property expression.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetDateCreatedProperty(Expression<Func<TEntityType, object>> dateCreatedPropertyExpression)
        {
            _dateCreatedProperty = dateCreatedPropertyExpression;
            return this;
        }

        /// <summary>
        /// Sets which property to use as the Date Modified property.
        /// </summary>
        /// <param name="dateModifiedPropertyExpression">The date modified property expression.</param>
        /// <returns>The collection configuration.</returns>
        public FluidityCollectionConfig<TEntityType> SetDateModifiedProperty(Expression<Func<TEntityType, object>> dateModifiedPropertyExpression)
        {
            _dateModifiedProperty = dateModifiedPropertyExpression;
            return this;
        }

        /// <summary>
        /// Configures a list view for the collection
        /// </summary>
        /// <param name="listViewConfig">A list view configuration delegate.</param>
        /// <returns>The list view configuration.</returns>
        public new FluidityListViewConfig<TEntityType> ListView(Action<FluidityListViewConfig<TEntityType>> listViewConfig = null)
        {
            return ListView(new FluidityListViewConfig<TEntityType>(listViewConfig));
        }

        /// <summary>
        /// Configures a list view for the collection
        /// </summary>
        /// <param name="listViewConfig">The list view configuration.</param>
        /// <returns>The list view configuration.</returns>
        public new FluidityListViewConfig<TEntityType> ListView(FluidityListViewConfig<TEntityType> listViewConfig)
        {
            if (_canDelete)
            {
                listViewConfig.DefaultBulkActions.Add(new FluidityDeleteBulkAction());
            }

            _listView = listViewConfig;                       
            return listViewConfig;
        }

        /// <summary>
        /// Configures the editor for a collection.
        /// </summary>
        /// <param name="editorConfig">An editor configuration delegate.</param>
        /// <returns>The editor configuration.</returns>
        public new FluidityEditorConfig<TEntityType> Editor(Action<FluidityEditorConfig<TEntityType>> editorConfig = null)
        {
            return Editor(new FluidityEditorConfig<TEntityType>(editorConfig));
        }

        /// <summary>
        /// Configures the editor for a collection.
        /// </summary>
        /// <param name="editorConfig">The editor configuration.</param>
        /// <returns>The editor configuration.</returns>
        public new FluidityEditorConfig<TEntityType> Editor(FluidityEditorConfig<TEntityType> editorConfig)
        {
            _editor = editorConfig;
            return editorConfig;
        }
    }
}