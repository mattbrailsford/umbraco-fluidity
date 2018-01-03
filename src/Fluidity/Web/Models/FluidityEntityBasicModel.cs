// <copyright file="FluidityEntityBasicModel.cs" company="Matt Brailsford">
// Copyright (c) 2017 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "entity", Namespace = "")]
    public class FluidityEntityBasicModel
    {
        [DataMember(Name = "id", IsRequired = true)]
        public object Id { get; set; }

        // Currently hard coding parent id as nuPickers requires passing
        // a parent id into it's functions even though it may not be used.
        [DataMember(Name = "parentId")]
        public object ParentId => -1;

        [DataMember(Name = "collection")]
        public string Collection { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}