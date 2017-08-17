using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "listView", Namespace = "")]
    public class FluidityListViewDisplay
    {
        [DataMember(Name = "properties", IsRequired = true)]
        public IEnumerable<FluidityListViewPropertyDisplay> Properties { get; set; }

        [DataMember(Name = "layouts", IsRequired = true)]
        public IEnumerable<FluidityListViewLayoutDisplay> Layouts { get; set; }
    }
}