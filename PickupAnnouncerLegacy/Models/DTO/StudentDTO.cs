using Newtonsoft.Json;

namespace PickupAnnouncerLegacy.Models.DTO
{
    public class StudentDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("teacher")]
        public string Teacher { get; set; }
        [JsonProperty("gradeLevel")]
        public string GradeLevel { get; set; }
        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }
        [JsonProperty("textColor")]
        public string TextColor { get; set; }
    }
}
