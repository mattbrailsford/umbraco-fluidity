using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "entity", Namespace = "")]
    public class FluidityEntityBasicModel
    {
        [DataMember(Name = "id", IsRequired = true)]
        public object Id { get; set; }

        [DataMember(Name = "collection")]
        public string Collection { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}