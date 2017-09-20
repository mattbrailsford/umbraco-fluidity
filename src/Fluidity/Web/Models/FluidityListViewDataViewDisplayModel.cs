// <copyright file="FluidityListViewDataViewDisplayModel.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "dataView", Namespace = "")]
    public class FluidityListViewDataViewDisplayModel
    {
        [DataMember(Name = "alias", IsRequired = true)]
        public string Alias { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }
    }
}