using Newtonsoft.Json;

namespace Fluidity.Models
{
    public class FluidityDataViewSummary
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }
    }
}
