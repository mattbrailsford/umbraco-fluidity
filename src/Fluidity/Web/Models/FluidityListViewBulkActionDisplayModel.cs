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