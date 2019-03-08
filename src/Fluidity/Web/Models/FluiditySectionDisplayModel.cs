// <copyright file="FluiditySectionDisplayModel.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "section", Namespace = "")]
    public class FluiditySectionDisplayModel
    {
        [DataMember(Name = "alias", IsRequired = true)]
        public object Alias { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public object Name { get; set; }

        [DataMember(Name = "tree", IsRequired = true)]
        public object Tree { get; set; }
    }
}
