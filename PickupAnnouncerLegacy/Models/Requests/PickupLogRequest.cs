using Newtonsoft.Json;
using System;

namespace PickupAnnouncerLegacy.Models.Requests
{
    public class PickupLogRequest
    {
        [JsonProperty("startOfDayUTC")]
        public DateTimeOffset StartOfDayUTC { get; set; }
    }
}
