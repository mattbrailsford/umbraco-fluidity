using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "collection", Namespace = "")]
    public class FluidityDashboardCollectionDisplay
    {
        [DataMember(Name = "section", IsRequired = true)]
        public object Section { get; set; }

        [DataMember(Name = "tree", IsRequired = true)]
        public object Tree { get; set; }

        [DataMember(Name = "alias", IsRequired = true)]
        public object Alias { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "isReadOnly")]
        public bool IsReadOnly { get; set; }

        [DataMember(Name = "hasListView")]
        public bool HasListView { get; set; }
    }
}
