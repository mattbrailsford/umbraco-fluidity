// <copyright file="FluidityCollectionDisplayModel.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "collection", Namespace = "")]
    public class FluidityCollectionDisplayModel
    {
        [DataMember(Name = "section", IsRequired = true)]
        public object Section { get; set; }

        [DataMember(Name = "tree", IsRequired = true)]
        public object Tree { get; set; }

        [DataMember(Name = "alias", IsRequired = true)]
        public object Alias { get; set; }

        [DataMember(Name = "nameSingular")]
        public string NameSingular { get; set; }

        [DataMember(Name = "namePlural")]
        public string NamePlural { get; set; }

        [DataMember(Name = "iconSingular")]
        public string IconSingular { get; set; }

        [DataMember(Name = "iconPlural")]
        public string IconPlural { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "isSearchable")]
        public bool IsSearchable { get; set; }

        [DataMember(Name = "canCreate")]
        public bool CanCreate { get; set; }

        [DataMember(Name = "canUpdate")]
        public bool CanUpdate { get; set; }

        [DataMember(Name = "canDelete")]
        public bool CanDelete { get; set; }

        [DataMember(Name = "hasListView")]
        public bool HasListView { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "listView")]
        public FluidityListViewDisplayModel ListView { get; set; }
    }
}
