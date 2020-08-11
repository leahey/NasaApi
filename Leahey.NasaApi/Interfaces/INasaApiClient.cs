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

        Task<IEnumerable<MarsRoverPhoto>> GetMarsRoverPhotosAsync(string roverName, string apiKey, int? page, string earthDate, string cameraName = "");
        Task<IEnumerable<MarsRoverPhoto>> GetMarsRoverPhotosAsync(string roverName, string apiKey, int? page, int sol, string cameraName = "");

        void DownloadFile(string address, string fileName);
        void DownloadFileAsync(string address, string fileName);
    }
}
