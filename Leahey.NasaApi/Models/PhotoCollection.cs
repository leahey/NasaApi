using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Leahey.NasaApi.Implementations;

namespace Leahey.NasaApi.Models
{
    public class PhotoCollection
    {
        [JsonPropertyName("photos")]
        public List<MarsRoverPhoto> Photos { get; set; }
    }
}
