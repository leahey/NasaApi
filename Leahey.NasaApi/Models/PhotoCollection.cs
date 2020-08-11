using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Leahey.NasaApi.Models
{
    [ExcludeFromCodeCoverage]
    public class PhotoCollection
    {
        [JsonPropertyName("photos")]
        public List<MarsRoverPhoto> Photos { get; set; }
    }
}
