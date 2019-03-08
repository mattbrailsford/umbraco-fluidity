// <copyright file="FluidityListViewLayoutDisplayModel.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "layout", Namespace = "")]
    public class FluidityListViewLayoutDisplayModel
    {
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "icon", IsRequired = true)]
        public string Icon { get; set; }

        [DataMember(Name = "path", IsRequired = true)]
        public string Path { get; set; }

        [DataMember(Name = "isSystem", IsRequired = true)]
        public bool IsSystem { get; set; }

        [DataMember(Name = "selected", IsRequired = true)]
        public bool Selected { get; set; }
    }
}