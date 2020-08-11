using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Leahey.NasaApi.Models
{
    [ExcludeFromCodeCoverage]
    public class MarsRoverPhoto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sol")]
        public int Sol { get; set; }

        [JsonPropertyName("earth_date")]
        public string EarthDate { get; set; }

        [JsonPropertyName("img_src")]
        public string ImgSource { get; set; }

        [JsonPropertyName("camera")]
        public MarsRoverCamera Camera { get; set; }

        [JsonPropertyName("rover")]
        public MarsRover Rover { get; set; }

        //"id": 102693,
        //"sol": 1000,
        //"camera": {
        //},
        //"img_src": "http://mars.jpl.nasa.gov/msl-raw-images/proj/msl/redops/ods/surface/sol/01000/opgs/edr/fcam/FLB_486265257EDR_F0481570FHAZ00323M_.JPG",
        //"earth_date": "2015-05-30",
        //"rover": {
        //}

    }
}
