// <copyright file="FluidityListViewPropertyDisplayModel.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "property", Namespace = "")]
    public class FluidityListViewPropertyDisplayModel
    {
        [DataMember(Name = "alias", IsRequired = true)]
        public string Alias { get; set; }

        [DataMember(Name = "header", IsRequired = true)]
        public string Header { get; set; }

        [DataMember(Name = "allowSorting", IsRequired = true)]
        public bool AllowSorting { get; set; }

        [DataMember(Name = "isSystem", IsRequired = true)]
        public bool IsSystem { get; set; }
    }
}