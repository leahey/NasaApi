using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Leahey.NasaApi.Models;

namespace Leahey.NasaApi.Interfaces
{
    public interface INasaApiClient
    {
        Task<HttpResponseMessage> GetAsync(string url);

        //[Obsolete("Use new GetRoverPhotosAsync() instead.")]
        //Task<MarsRoverPhoto> GetRoverPhotoAsync(string url);

        Task<IEnumerable<MarsRoverPhoto>> GetMarsRoverPhotosAsync(string roverName, string apiKey, int? page, string earthDate, string cameraName = "");
        Task<IEnumerable<MarsRoverPhoto>> GetMarsRoverPhotosAsync(string roverName, string apiKey, int? page, int sol, string cameraName = "");
    }
}
