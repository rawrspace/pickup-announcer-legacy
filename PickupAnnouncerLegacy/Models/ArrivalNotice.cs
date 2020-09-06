using Newtonsoft.Json;

namespace PickupAnnouncerLegacy.Models
{
    public class ArrivalNotice
    {
        [JsonProperty("car")]
        public string Car { get; set; }
        [JsonProperty("cone")]
        public string Cone { get; set; }
    }
}
