using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "section", Namespace = "")]
    public class FluiditySectionDisplay
    {
        [DataMember(Name = "alias", IsRequired = true)]
        public object Alias { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public object Name { get; set; }

        [DataMember(Name = "tree", IsRequired = true)]
        public object Tree { get; set; }
    }
}
