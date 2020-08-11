using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Leahey.NasaApi.Models
{
    public class PhotoCollection
    {
        [JsonPropertyName("photos")]
        public List<MarsRoverPhoto> Photos { get; set; }
    }
}
