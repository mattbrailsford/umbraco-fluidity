using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "listView", Namespace = "")]
    public class FluidityListViewDisplay
    {
        [DataMember(Name = "bulkActions")]
        public IEnumerable<FluidityBulkActionDisplay> BulkActions { get; set; }

        [DataMember(Name = "properties", IsRequired = true)]
        public IEnumerable<FluidityListViewPropertyDisplay> Properties { get; set; }

        [DataMember(Name = "layouts", IsRequired = true)]
        public IEnumerable<FluidityListViewLayoutDisplay> Layouts { get; set; }

        [DataMember(Name = "dataViews")]
        public IEnumerable<FluidityListViewDataViewDisplay> DataViews { get; set; }

        [DataMember(Name = "isSearchable")]
        public bool IsSearchable { get; set; }
    }
}