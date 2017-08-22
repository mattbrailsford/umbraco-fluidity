using System.Linq;
using Fluidity.Configuration;
using Umbraco.Core;

namespace Fluidity.Web.Models.Mappers
{
    internal class FluidityCollectionMapper
    {
        public FluidityCollectionDisplay ToDisplay(FluiditySectionConfig section, FluidityCollectionConfig collection)
        {
            return new FluidityCollectionDisplay
            {
                Section = section.Alias,
                Tree = section.Tree.Alias,
                Alias = collection.Alias,
                NameSingular = collection.NameSignular,
                NamePlural = collection.NamePlural,
                IconSingular = collection.IconSingular,
                IconPlural = collection.IconPlural,
                Description = collection.Description,
                Path = collection.Path,
                ListView = new FluidityListViewDisplay
                {
                    Properties = collection.ListView.Fields.Select(x => new FluidityListViewPropertyDisplay // We don't include Name, as it's always automatically included
                    {
                        Alias = x.Property.Name,
                        Header = x.Heading ?? x.Property.Name.SplitPascalCasing(),
                        AllowSorting = true,
                        IsSystem = false
                    }),
                    Layouts = collection.ListView.Layouts.Select((x, idx) => new FluidityListViewLayoutDisplay
                    {
                        Icon = x.Icon,
                        Name = x.Name,
                        Path = x.View,
                        IsSystem = x.IsSystem,
                        Selected = true
                    }),
                    DataViews = collection.ListView.DataViews.Select(x => new FluidityListViewDataViewDisplay
                    {
                        Alias = x.Alias,
                        Name = x.Name
                    }),
                    BulkActions = collection.ListView.BulkActions.Select(x => new FluidityBulkActionDisplay
                    {
                        Icon = x.Icon,
                        Alias = x.Alias,
                        Name = x.Name,
                        AngularServiceName = x.AngularServiceName
                    })
                }
            };
        }

        public FluidityDashboardCollectionDisplay ToDashboardDisplay(FluiditySectionConfig section, FluidityCollectionConfig collection)
        {
            return new FluidityDashboardCollectionDisplay
            {
                Section = section.Alias,
                Tree = section.Tree.Alias,
                Alias = collection.Alias,
                Name = collection.NamePlural,
                Icon = collection.IconPlural + (!collection.Color.IsNullOrWhiteSpace() ? " color-" + collection.Color : ""),
                Description = collection.Description,
                IsReadOnly = collection.IsReadOnly,
                HasListView = collection.ViewMode == FluidityViewMode.List
            };
        }
    }
}
