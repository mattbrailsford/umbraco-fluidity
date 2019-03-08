// <copyright file="FluidityListViewBulkActionDisplayModel.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "bulkAction", Namespace = "")]
    public class FluidityListViewBulkActionDisplayModel
    {
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "angularServiceName")]
        public string AngularServiceName { get; set; }
    }
}