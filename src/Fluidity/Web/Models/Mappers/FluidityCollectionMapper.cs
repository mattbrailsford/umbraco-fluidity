// <copyright file="FluidityCollectionMapper.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Linq;
using Fluidity.Configuration;
using Umbraco.Core;
using System.ComponentModel;
using System.Reflection;
using Fluidity.Models;

namespace Fluidity.Web.Models.Mappers
{
    internal class FluidityCollectionMapper
    {
        public FluidityCollectionDisplayModel ToDisplayModel(FluiditySectionConfig section, FluidityCollectionConfig collection, bool includeListView)
        {
            var m = new FluidityCollectionDisplayModel
            {
                Section = section.Alias,
                Tree = section.Tree.Alias,
                Alias = collection.Alias,
                NameSingular = collection.NameSingular,
                NamePlural = collection.NamePlural,
                IconSingular = collection.IconSingular + (!collection.IconColor.IsNullOrWhiteSpace() ? " color-" + collection.IconColor : ""),
                IconPlural = collection.IconPlural + (!collection.IconColor.IsNullOrWhiteSpace() ? " color-" + collection.IconColor : ""),
                Description = collection.Description,                
                IsSearchable = collection.SearchableProperties.Any(),
                CanCreate = collection.CanCreate,
                CanUpdate = collection.CanUpdate,
                CanDelete = collection.CanDelete,
                HasListView = collection.ViewMode == FluidityViewMode.List,
                Path = collection.Path
            };

            if (includeListView && m.HasListView)
            {
                m.ListView = new FluidityListViewDisplayModel
                {
                    PageSize = collection.ListView.PageSize,
                    DefaultOrderBy = collection.SortProperty?.Name ?? "Name",
                    DefaultOrderDirection = collection.SortDirection == SortDirection.Ascending ? "asc" : "desc",
                    Properties = collection.ListView.Fields.Select(x =>
                    {
                        // Calculate heading
                        var heading = x.Heading;
                        if (heading.IsNullOrWhiteSpace())
                        {
                            var attr = x.Property.PropertyInfo.GetCustomAttribute<DisplayNameAttribute>(true);
                            if (attr != null)
                            {
                                heading = attr.DisplayName;
                            }
                            else
                            {
                                heading = x.Property.Name.SplitPascalCasing();
                            }
                        }

                        // Build property
                        return new FluidityListViewPropertyDisplayModel // We don't include Name, as it's always automatically included
                        {
                            Alias = x.Property.Name,
                            Header = heading,
                            AllowSorting = true,
                            IsSystem = false
                        };
                    }),
                    Layouts = collection.ListView.Layouts.Select((x, idx) => new FluidityListViewLayoutDisplayModel
                    {
                        Icon = x.Icon,
                        Name = x.Name,
                        Path = x.View,
                        IsSystem = x.IsSystem,
                        Selected = true
                    }),
                    DataViews = collection.ListView.DataViewsBuilder?.GetDataViews() ?? new FluidityDataViewSummary[0],
                    BulkActions = collection.ListView.BulkActions.Select(x => new FluidityListViewBulkActionDisplayModel
                    {
                        Icon = x.Icon,
                        Alias = x.Alias,
                        Name = x.Name,
                        AngularServiceName = x.AngularServiceName
                    })
                };
            }

            return m;
        }
    }
}
