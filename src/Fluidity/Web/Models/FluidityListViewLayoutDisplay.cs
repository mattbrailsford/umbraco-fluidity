using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "layout", Namespace = "")]
    public class FluidityListViewLayoutDisplay
    {
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        [DataMember(Name = "icon", IsRequired = true)]
        public string Icon { get; set; }

        [DataMember(Name = "path", IsRequired = true)]
        public string Path { get; set; }

        [DataMember(Name = "isSystem", IsRequired = true)]
        public bool IsSystem { get; set; }

        [DataMember(Name = "selected", IsRequired = true)]
        public bool Selected { get; set; }
    }
}