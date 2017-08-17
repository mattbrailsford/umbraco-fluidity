using System;
using System.Linq.Expressions;
using Fluidity.Data;
using Fluidity.Extensions;
using Umbraco.Core;

namespace Fluidity.Configuration
{
    public class FluidityCollectionConfig<TEntityType> : FluidityCollectionConfig
    {
        public FluidityCollectionConfig(Expression<Func<TEntityType, object>> idProperty, string nameSingular, string namePlural, string iconSingular = null, string iconPlural = null, Action<FluidityCollectionConfig<TEntityType>> config = null)
            : base (typeof(TEntityType), idProperty.GetPropertyInfo(), nameSingular, namePlural, iconSingular, iconPlural)
        {
            config?.Invoke(this);
        }

        public FluidityCollectionConfig<TEntityType> SetAlias(string alias)
        {
            _alias = alias;
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
            _deletedProperty = deletedProperty.GetPropertyInfo();
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetSortProperty(Expression<Func<TEntityType, object>> sortProperty)
        {
            return SetSortProperty(sortProperty, FluiditySortOrder.Asc);
        }

        public FluidityCollectionConfig<TEntityType> SetSortProperty(Expression<Func<TEntityType, object>> sortProperty, FluiditySortOrder sortOrder)
        {
            _sortProperty = sortProperty.GetPropertyInfo();
            _sortOrder = sortOrder;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> ShowOnDashboard()
        {
            _visibleOnDashboard = true;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> HideFromTree()
        {
            _visibleInTree = false;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> MakeReadOnly()
        {
            _readOnly = true;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetViewMode(FluidityViewMode viewMode)
        {
            _viewMode = viewMode;
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetDateCreatedProperty(Expression<Func<TEntityType, object>> dateCreatedProperty)
        {
            _dateCreatedProperty = dateCreatedProperty.GetPropertyInfo();
            return this;
        }

        public FluidityCollectionConfig<TEntityType> SetDateModifiedProperty(Expression<Func<TEntityType, object>> dateModifiedProperty)
        {
            _dateModifiedProperty = dateModifiedProperty.GetPropertyInfo();
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