// <copyright file="FluidityListViewConfig`T.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Linq.Expressions;
using Fluidity.Actions;
using Fluidity.DataViewsBuilders;
using Fluidity.ListViewLayouts;

namespace Fluidity.Configuration
{
    /// <summary>
    /// Fluidity list view configuration
    /// </summary>
    /// <typeparam name="TEntityType">The type of the entity type.</typeparam>
    /// <seealso cref="Fluidity.Configuration.FluidityListViewConfig" />
    public class FluidityListViewConfig<TEntityType> : FluidityListViewConfig
    {
        public FluidityListViewConfig(Action<FluidityListViewConfig<TEntityType>> config = null)
        {
            config?.Invoke(this);
        }

        /// <summary>
        /// Adds a bulk action to the list view.
        /// </summary>
        /// <typeparam name="TBulkActionType">The type of the bulk action.</typeparam>
        /// <returns>The list view configuration.</returns>
        public FluidityListViewConfig<TEntityType> AddBulkAction<TBulkActionType>()
            where TBulkActionType : FluidityBulkAction, new()
        {
            _bulkActions.Add(new TBulkActionType());
            return this;
        }

        /// <summary>
        /// Sets the page size for the list view.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>The list view configuration.</returns>
        public FluidityListViewConfig<TEntityType> SetPageSize(int pageSize)
        {
            _pageSize = pageSize;
            return this;
        }

        /// <summary>
        /// Adds a field to the list view.
        /// </summary>
        /// <typeparam name="TValueType">The type of the value.</typeparam>
        /// <param name="fieldExpression">The field expression.</param>
        /// <param name="fieldConfig">A field configuration delegate.</param>
        /// <returns>The list view field configuration.</returns>
        public FluidityListViewFieldConfig<TEntityType, TValueType> AddField<TValueType>(Expression<Func<TEntityType, TValueType>> fieldExpression, Action<FluidityListViewFieldConfig<TEntityType, TValueType>> fieldConfig = null)
        {
            return AddField(new FluidityListViewFieldConfig<TEntityType, TValueType>(fieldExpression, fieldConfig));
        }

        /// <summary>
        /// Adds a field to the list view.
        /// </summary>
        /// <typeparam name="TValueType">The type of the value.</typeparam>
        /// <param name="fieldConfig">The field configuration.</param>
        /// <returns>The list view field configuration.</returns>
        public FluidityListViewFieldConfig<TEntityType, TValueType> AddField<TValueType>(FluidityListViewFieldConfig<TEntityType, TValueType> fieldConfig)
        {
            var field = fieldConfig;
            _fields.Add(field);
            return field;
        }

        /// <summary>
        /// Adds a custom layout to the list view.
        /// </summary>
        /// <remarks>
        /// By adding a custom list view layout this will remove any default list view layouts so if you still require these you'll need to explicitly add them again.
        /// </remarks>
        /// <typeparam name="TListViewLayoutType">The type of the ListView layout type.</typeparam>
        /// <returns>The list view configuration.</returns>
        public FluidityListViewConfig<TEntityType> AddLayout<TListViewLayoutType>()
            where TListViewLayoutType : FluidityListViewLayout, new()
        {
            _layouts.Add(new TListViewLayoutType());
            return this;
        }

        /// <summary>
        /// Adds a data view to the list view.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <returns>The list view configuration.</returns>
        public FluidityListViewConfig<TEntityType> AddDataView(string name, Expression<Func<TEntityType, bool>> whereClause)
        {
            _dataViews.Add(new FluidityDataViewConfig(name, whereClause));
            return this;
        }

        /// <summary>
        /// Sets the data views builder for the list view.
        /// </summary>
        /// <remarks>
        /// By setting a data view builder this will override any data views configured using the AddDataView method.
        /// </remarks>
        /// <typeparam name="TDataViewsBuilderType">The type of the data views builder.</typeparam>
        /// <returns>The list view configuration.</returns>
        public FluidityListViewConfig<TEntityType> SetDataViewsBuilder<TDataViewsBuilderType>()
            where TDataViewsBuilderType : FluidityDataViewsBuilder<TEntityType>, new()
        {
            _dataViewsBuilder = new TDataViewsBuilderType();
            return this;
        }
    }
}