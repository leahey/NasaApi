using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Leahey.NasaApi.CodeQuality;
using Leahey.NasaApi.Interfaces;
using Leahey.NasaApi.Models;

[assembly: InternalsVisibleTo("Leahey.NasaApi.Test")]

namespace Leahey.NasaApi.Implementations
{
    public class NasaApiClient : HttpClient, INasaApiClient
    {
        #region private fields
        private readonly IWebClientProxy _webClientProxy;
        private readonly IMarsRoverUtils _marsRoverUtils;
        #endregion

        #region consts
        public const string NasaDomain = "https://api.nasa.gov";

        public const string ApiRoute_MarsRovers = "/mars-photos/api/v1/rovers";
        public const string ApiRoute_AstronomyPicOfDay = "/planetary/apod";

        public const string ApiRoute_Photos = "photos";

        // int
        public const string QryParam_Sol = "sol";
        // YYYY-MM-DD
        public const string QryParam_EarthDate = "earth_date";
        public const string QryParam_Camera = "camera";
        public const string QryParam_Page = "page";
        public const string QryParam_ApiKey = "api_key";

        #endregion

        [ExcludeFromCodeCoverage]
        public NasaApiClient(IWebClientProxy webClientProxy, IMarsRoverUtils marsRoverUtils)
        {
            _marsRoverUtils = marsRoverUtils;
            _webClientProxy = webClientProxy;
        }

        [NDependIgnore("Pass-through method")]
        [ExcludeFromCodeCoverage]
        public void DownloadFile(string address, string fileName)
        {
            _webClientProxy.DownloadFile(address, fileName);
        }

        [NDependIgnore("Pass-through method")]
        [ExcludeFromCodeCoverage]
        public void DownloadFileAsync(string address, string fileName)
        {
            _webClientProxy.DownloadFileAsync(address, fileName);
        }

        [NDependIgnore("Pass-through method")]
        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<MarsRoverPhoto>> GetMarsRoverPhotosAsync(string roverName, 
            string apiKey, 
            int? page, 
            string earthDate, 
            string cameraName = "")
        {
            //ncrunch: no coverage start
            var url = _marsRoverUtils.BuildMarsRoverUrlString(roverName, page, null, earthDate, cameraName, apiKey);

            string responseBody = await GetStringAsync(url).ConfigureAwait(true);

            if (string.IsNullOrEmpty(responseBody))
                return null;

            PhotoCollection result = JsonSerializer.Deserialize<PhotoCollection>(responseBody);
            return result.Photos;
            //ncrunch: no coverage end
        }

        [NDependIgnore("Pass-through method")]
        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<MarsRoverPhoto>> GetMarsRoverPhotosAsync(string roverName, string apiKey, int? page, int sol, string cameraName = "")
        {
            //ncrunch: no coverage start
            var url = _marsRoverUtils.BuildMarsRoverUrlString(roverName, page, sol, null, cameraName, apiKey);

            string responseBody = await GetStringAsync(url).ConfigureAwait(true);

            if (string.IsNullOrEmpty(responseBody))
                return null;

            PhotoCollection result = JsonSerializer.Deserialize<PhotoCollection>(responseBody);
            return result.Photos;
            //ncrunch: no coverage end
        }
    }
}
