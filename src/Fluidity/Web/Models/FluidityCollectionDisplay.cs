using System.Runtime.Serialization;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "collection", Namespace = "")]
    public class FluidityCollectionDisplay
    {
        [DataMember(Name = "section", IsRequired = true)]
        public object Section { get; set; }

        [DataMember(Name = "tree", IsRequired = true)]
        public object Tree { get; set; }

        [DataMember(Name = "alias", IsRequired = true)]
        public object Alias { get; set; }

        [DataMember(Name = "nameSingular")]
        public string NameSingular { get; set; }

        [DataMember(Name = "namePlural")]
        public string NamePlural { get; set; }

        [DataMember(Name = "iconSingular")]
        public string IconSingular { get; set; }

        [DataMember(Name = "iconPlural")]
        public string IconPlural { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "listView")]
        public FluidityListViewDisplay ListView { get; set; }
    }
}
