using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "dataView", Namespace = "")]
    public class FluidityListViewDataViewDisplay
    {
        [DataMember(Name = "alias", IsRequired = true)]
        public string Alias { get; set; }

        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }
    }
}