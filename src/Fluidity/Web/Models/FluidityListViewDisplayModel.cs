using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "listView", Namespace = "")]
    public class FluidityListViewDisplayModel
    {
        [DataMember(Name = "pageSize")]
        public int PageSize { get; set; }

        [DataMember(Name = "bulkActions")]
        public IEnumerable<FluidityListViewBulkActionDisplayModel> BulkActions { get; set; }

        [DataMember(Name = "properties", IsRequired = true)]
        public IEnumerable<FluidityListViewPropertyDisplayModel> Properties { get; set; }

        [DataMember(Name = "layouts", IsRequired = true)]
        public IEnumerable<FluidityListViewLayoutDisplayModel> Layouts { get; set; }

        [DataMember(Name = "dataViews")]
        public IEnumerable<FluidityListViewDataViewDisplayModel> DataViews { get; set; }
    }
}