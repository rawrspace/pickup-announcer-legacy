using Newtonsoft.Json;

namespace PickupAnnouncerLegacy.Models.Requests
{
    public class GradeLevelRequest
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }
        [JsonProperty("textColor")]
        public string TextColor { get; set; }
    }
}
