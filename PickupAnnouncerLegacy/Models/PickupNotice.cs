using Newtonsoft.Json;
using PickupAnnouncerLegacy.Models.DTO;
using System;
using System.Collections.Generic;

namespace PickupAnnouncerLegacy.Models
{
    public class PickupNotice
    {
        [JsonProperty("car")]
        public string Car { get; set; }
        [JsonProperty("cone")]
        public string Cone { get; set; }
        [JsonProperty("students")]
        public IEnumerable<StudentDTO> Students { get; set; }
        [JsonProperty("pickupTimeUTC")]
        public DateTimeOffset PickupTimeUTC { get; set; }
    }
}
