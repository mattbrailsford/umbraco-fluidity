using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Umbraco.Web.Models.ContentEditing;

namespace Fluidity.Web.Models
{   
    [DataContract(Name = "entity", Namespace = "")]
    public class FluidityEntityListViewDisplay : FluidityEntityBasic
    {
        public FluidityEntityListViewDisplay()
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
