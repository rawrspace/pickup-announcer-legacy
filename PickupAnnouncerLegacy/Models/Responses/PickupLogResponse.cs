using Newtonsoft.Json;
using System.Collections.Generic;

namespace PickupAnnouncerLegacy.Models.Responses
{
    public class PickupLogResponse
    {
        [JsonProperty("announcements")]
        public IEnumerable<PickupNotice> Announcements { get; set; }
    }
}
