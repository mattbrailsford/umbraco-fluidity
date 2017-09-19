using System;
using System.Linq;
using System.Linq.Expressions;
using Fluidity.Data;
using Umbraco.Core;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
using Umbraco.Web.Models.Trees;

namespace Fluidity.Configuration
{
    public class FluidityCollectionConfig<TEntityType> : FluidityCollectionConfig
    {
        public FluidityCollectionConfig(Expression<Func<TEntityType, object>> idProperty, string nameSingular, string namePlural, string description, string iconSingular = null, string iconPlural = null, Action<FluidityCollectionConfig<TEntityType>> config = null)
            : base (typeof(TEntityType), idProperty, nameSingular, namePlural, description, iconSingular, iconPlural)
        {
            config?.Invoke(this);
        }

        public FluidityCollectionConfig<TEntityType> SetAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetIconColor(string color)
        {
            _iconColor = color;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetNameProperty(Expression<Func<TEntityType, string>> nameProperty)
        {
            _nameProperty = nameProperty;
            _searchableProperties.Add(_nameProperty);

            if (_sortProperty == null)
            {
                // Convert lambda to object based property accessor for the sort property
                var entityParam = nameProperty.Parameters[0];
                var memberExp = (MemberExpression)nameProperty.Body;
                var convertExp = Expression.Convert(memberExp, typeof(object));
                var lambda = Expression.Lambda<Func<TEntityType, object>>(convertExp, entityParam);

                _sortProperty = lambda;
            }

            return this;
        }

        //public FluidityCollectionConfig<TEntityType> SetDescription(string description)
        //{
        //    _description = description;
        //    return this;
        //}

        public FluidityCollectionConfig<TEntityType> SetConnectionString(string connectionStringName)
        {
            _connectionStringg = connectionStringName;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetRepositoryType<TRepositoryType>()
            where TRepositoryType : IFluidityRepository
        {
            return SetRepositoryType(typeof(TRepositoryType));
        }

        public FluidityCollectionConfig<TEntityType> SetRepositoryType(Type repositoryType)
        {
            if (!repositoryType.Implements<IFluidityRepository>())
            {
                throw new ArgumentException($"{repositoryType.Name} does not implement IFluidityRepository", nameof(repositoryType));
            }

            _repositoryType = repositoryType;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetDeletedProperty(Expression<Func<TEntityType, bool>> deletedProperty)
        {
            _deletedProperty = deletedProperty;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetSortProperty(Expression<Func<TEntityType, object>> sortProperty)
        {
            return SetSortProperty(sortProperty, Direction.Ascending);
        }

        public FluidityCollectionConfig<TEntityType> SetSortProperty(Expression<Func<TEntityType, object>> sortProperty, Direction sortDirection)
        {

            _sortProperty = sortProperty;
            _sortDirection = sortDirection;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> ShowOnDashboard()
        {
            _isVisibleOnDashboard = true;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> HideFromTree()
        {
            _isVisibleInTree = false;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> MakeReadOnly()
        {
            _isReadOnly = true;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetViewMode(FluidityViewMode viewMode)
        {
            _viewMode = viewMode;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> AddContainerMenuItem<TMenuItem>()
            where TMenuItem : MenuItem, new()
        {
            _containerMenuItems.Add(new TMenuItem());
            return this;
        }

        public FluidityCollectionConfig<TEntityType> AddContainerMenuItem(MenuItem menuItem)
        {
            _containerMenuItems.Add(menuItem);
            return this;
        }

        public FluidityCollectionConfig<TEntityType> AddEntityMenuItem<TMenuItem>()
            where TMenuItem : MenuItem, new()
        {
            _entityMenuItems.Add(new TMenuItem());
            return this;
        }

        public FluidityCollectionConfig<TEntityType> AddEntityMenuItem(MenuItem menuItem)
        {
            _entityMenuItems.Add(menuItem);
            return this;
        }

        public FluidityCollectionConfig<TEntityType> AddSearchableProperty(Expression<Func<TEntityType, string>> searchableProp)
        {
            _searchableProperties.Add(searchableProp);
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetDateCreatedProperty(Expression<Func<TEntityType, object>> dateCreatedProperty)
        {
            _dateCreatedProperty = dateCreatedProperty;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetDateModifiedProperty(Expression<Func<TEntityType, object>> dateModifiedProperty)
        {
            _dateModifiedProperty = dateModifiedProperty;
            return this;
        }

        public new FluidityListViewConfig<TEntityType> ListView(Action<FluidityListViewConfig<TEntityType>> listViewConfig = null)
        {
            return ListView(new FluidityListViewConfig<TEntityType>(listViewConfig));
        }

        public new FluidityListViewConfig<TEntityType> ListView(FluidityListViewConfig<TEntityType> listViewConfig)
        {
            _listView = listViewConfig;
            return listViewConfig;
        }

        public new FluidityEditorConfig<TEntityType> Editor(Action<FluidityEditorConfig<TEntityType>> editorConfig = null)
        {
            return Editor(new FluidityEditorConfig<TEntityType>(editorConfig));
        }

        public new FluidityEditorConfig<TEntityType> Editor(FluidityEditorConfig<TEntityType> editorConfig)
        {
            _editor = editorConfig;
            return editorConfig;
        }

        public FluidityCollectionConfig<TEntityType> SetNameFormat(Func<TEntityType, string> format)
        {
            _nameFormat = (entity) => format((TEntityType)entity);
            return this;
        }
    }
}