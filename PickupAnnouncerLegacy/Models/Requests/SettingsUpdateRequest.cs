using Newtonsoft.Json;

namespace PickupAnnouncerLegacy.Models.Requests
{
    public class SettingsUpdateRequest
    {
        [JsonProperty("numberOfCones")]
        public int NumberOfCones { get; set; }
    }
}
