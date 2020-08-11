using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Leahey.NasaApi.Models
{
    [ExcludeFromCodeCoverage]
    public class MarsRoverCamera
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rover_id")]
        public int RoverId { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
        //"id": 20,
        //"name": "FHAZ",
        //"rover_id": 5,
        //"full_name": "Front Hazard Avoidance Camera"
    }
}
