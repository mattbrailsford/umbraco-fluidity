using System.Collections.Generic;
using System.Runtime.Serialization;
using Umbraco.Web.Models.ContentEditing;

namespace Fluidity.Web.Models
{
    [DataContract(Name = "entity", Namespace = "")]
    public class FluidityEntityPost : FluidityEntityBasic, IHaveUploadedFiles
    {
        public FluidityEntityPost()
        {
            Properties = new List<ContentPropertyBasic>();
            UploadedFiles = new List<ContentItemFile>();
        }

        [DataMember(Name = "section")]
        public string Section { get; set; }

        [DataMember(Name = "properties")]
        public IEnumerable<ContentPropertyBasic> Properties { get; set; }

        [IgnoreDataMember]
        public List<ContentItemFile> UploadedFiles { get; private set; }
    }
}
