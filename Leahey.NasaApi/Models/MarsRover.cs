using System;
using System.Text.Json.Serialization;
using Leahey.NasaApi.Interfaces;

namespace Leahey.NasaApi.Implementations
{
    public class MarsRover
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("landing_date")]
        public string Landing_Date { get; set; }

        [JsonPropertyName("launch_date")]
        public string Launch_Date { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
        //"id": 5,
        //"name": "Curiosity",
        //"landing_date": "2012-08-06",
        //"launch_date": "2011-11-26",
        //"status": "active"
    }
}
