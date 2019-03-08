// <copyright file="FluidityEntityDisplayModel.cs" company="Matt Brailsford">
// Copyright (c) 2019 Matt Brailsford and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Umbraco.Web.Models.ContentEditing;

namespace Fluidity.Web.Models
{   
    [DataContract(Name = "entity", Namespace = "")]
    public class FluidityEntityDisplayModel : FluidityEntityBasicModel
    {
        public FluidityEntityDisplayModel()
        {
            Properties = new List<ContentPropertyBasic>();
        }

        [DataMember(Name = "section", IsRequired = true)]
        public string Section { get; set; }

        [DataMember(Name = "tree", IsRequired = true)]
        public string Tree { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "properties")]
        public IEnumerable<ContentPropertyBasic> Properties { get; set; }

        [DataMember(Name = "createDate")]
        public DateTime CreateDate { get; set; }

        [DataMember(Name = "updateDate")]
        public DateTime UpdateDate { get; set; }

        [DataMember(Name = "editPath")]
        public string EditPath { get; set; }

    }
}
