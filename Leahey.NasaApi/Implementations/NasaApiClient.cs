using System;
using System.Collections.Generic;
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
        #region consts
        private const string NasaDomain = "https://api.nasa.gov";

        private const string ApiRoute_MarsRovers = "/mars-photos/api/v1/rovers";
        private const string ApiRoute_AstronomyPicOfDay = "/planetary/apod";

        private const string Rover_Opportunity = "opportunity";
        private const string Rover_Spirit = "spirit";
        private const string Rover_Curiosity = "curiosity";

        private const string ApiRoute_Photos = "photos";

        private const string RoverCam_FrontHazardAvoidanceCam = "fhaz";
        private const string RoverCam_RearHazardAvoidanceCam = "rhaz";
        private const string RoverCam_MastCam = "mast";
        private const string RoverCam_ChemCam = "chemcam";
        private const string RoverCam_MarsHandLensImager = "mahli";
        private const string RoverCam_MarsDescentImager = "mardi";
        private const string RoverCam_NavigationCam = "navcam";
        private const string RoverCam_PanoramicCam = "pancam";
        private const string RoverCam_MiniatureThermalEmissionSpectrometer = "minites";

        // int
        private const string QryParam_Sol = "sol";
        // YYYY-MM-DD
        private const string QryParam_EarthDate = "earth_date";
        private const string QryParam_Camera = "camera";
        private const string QryParam_Page = "page";
        private const string QryParam_ApiKey = "api_key";

        #endregion

        #region internal methods
        /// <summary>
        /// Brute force uri builder for Mars Rover api calls.
        /// </summary>
        /// <param name="roverName">Name of Mars rover. Required.</param>
        /// <param name="page">Page number; 25 items per page. Optional.</param>
        /// <param name="sol">Sol number for which to request photos. Optional. If both <paramref name="sol"/> and 
        /// <paramref name="earthDate"/> are provided, <paramref name="sol"/> value will be preferred.</param>
        /// <param name="cameraName">Name of rover camera. Optional.</param>
        /// <exception cref="ArgumentException"><paramref name="roverName"/> is null or empty.</exception>
        /// <exception cref="MarsRoverPhotoApiException"><paramref name="roverName"/> contains an invalid value.</exception>
        /// <returns>A new, concatenated uri string.</returns>
        [MustRemainInternal("Is tested")]
        internal static string BuildMarsRoverUrlString(string roverName,
            int? page,
            int? sol,
            string earthDate = "",
            string cameraName = "",
            string apiKey = "DEMO_KEY")
        {
            //TODO: Smart this up. 

            if (string.IsNullOrEmpty(roverName))
                throw new ArgumentException($"{nameof(roverName)} is null or empty.", nameof(roverName));
            if (!ValidateRoverName(roverName))
                throw new MarsRoverPhotoApiException($"{nameof(roverName)} contains an invalid value ('{roverName}')");
            if (!string.IsNullOrEmpty(cameraName) && !ValidateCameraName(roverName, cameraName))
                throw new MarsRoverPhotoApiException($"{nameof(cameraName)} contains an invalid value ('{cameraName}')");

            StringBuilder builder = new StringBuilder($"{NasaDomain}{ApiRoute_MarsRovers}/{roverName}/{ApiRoute_Photos}?");

            // add date/sol param
            if (sol.HasValue)
            {
                builder.Append($"{QryParam_Sol}={sol}");
            }
            else if (!string.IsNullOrEmpty(earthDate))
            {
                var formattedDate = ConvertEarthDateString(earthDate);
                builder.Append($"{QryParam_EarthDate}={formattedDate}");
            }

            // add page param
            if (page.HasValue)
            {
                builder.Append($"&{QryParam_Page}={page}");
            }

            // add camera param
            if (!string.IsNullOrEmpty(cameraName))
            {
                builder.Append($"&{QryParam_Camera}={cameraName}");
            }

            builder.Append($"&{QryParam_ApiKey}={apiKey}");
            return builder.ToString();
        }

        /// <summary>
        /// Attempts to convert given <paramref name="earthDateString"/> into date format expected by NASA (YYYY-MM-DD).
        /// If given value can not be properly converted, throws.
        /// </summary>
        /// <exception cref="MarsRoverPhotoApiException">Given <paramref name="earthDateString"/> cannot be converted.</exception>
        [MustRemainInternal("Is tested")]
        internal static string ConvertEarthDateString(string earthDateString)
        {
            DateTime earthDate;

            try
            {
                earthDate = DateTime.Parse(earthDateString, CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                throw new MarsRoverPhotoApiException($"Given {nameof(earthDateString)} could not be converted. Value was {earthDateString}.", ex);
            }

            return earthDate.ToString("yyyy-MM-dd", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Attempts to convert given <paramref name="earthDateString"/> into date format expected by NASA (YYYY-MM-DD), 
        /// returning success or failure. If an exception is thrown, it is caught and the method returns <c>false</c>.
        /// </summary>
        [MustRemainInternal("Is tested")]
        internal static bool TryConvertEarthDateString(string earthDateString, out string formattedDate)
        {
            formattedDate = string.Empty;

            bool result;
            try
            {
                formattedDate = ConvertEarthDateString(earthDateString);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }


        /// <summary>
        /// Validates that for the given <paramref name="roverName"/>, <paramref name="cameraName"/> is one of the proper values.
        /// </summary>
        [MustRemainInternal("Is tested")]
        internal static bool ValidateCameraName(string roverName, string cameraName)
        {
            var result = false;

            // all 3 rovers have these three cameras
            if (cameraName == RoverCam_FrontHazardAvoidanceCam || cameraName == RoverCam_RearHazardAvoidanceCam || cameraName == RoverCam_NavigationCam)
            {
                result = true;
            }
            else if (roverName == Rover_Opportunity || roverName == Rover_Spirit)
            {
                result = (cameraName == RoverCam_PanoramicCam || 
                          cameraName == RoverCam_MiniatureThermalEmissionSpectrometer);
            }
            else if (roverName == Rover_Curiosity)
            {
                result = (cameraName == RoverCam_MastCam ||
                          cameraName == RoverCam_ChemCam ||
                          cameraName == RoverCam_MarsHandLensImager ||
                          cameraName == RoverCam_MarsDescentImager);
            }

            return result;
        }

        /// <summary>
        /// Validates that <paramref name="roverName"/> is one of the proper values.
        /// </summary>
        [MustRemainInternal("Is tested")]
        internal static bool ValidateRoverName(string roverName)
        {
            return roverName == Rover_Opportunity || roverName == Rover_Spirit || roverName == Rover_Curiosity;
        }
        #endregion
        
        //[Obsolete("Use new GetRoverPhotoAsync() instead.")]
        //public async Task<MarsRoverPhoto> GetRoverPhotoAsync(string url)
        //{
        //    string responseBody = await GetStringAsync(url).ConfigureAwait(true);

        //    if(string.IsNullOrEmpty(responseBody))
        //        return null;

        //    PhotoCollection result = JsonSerializer.Deserialize<PhotoCollection>(responseBody);
        //    return result.Photos[0];

        //    throw new NotImplementedException();
        //}

        public async Task<IEnumerable<MarsRoverPhoto>> GetMarsRoverPhotosAsync(string roverName, 
            string apiKey, 
            int? page, 
            string earthDate, 
            string cameraName = "")
        {
            var url = BuildMarsRoverUrlString(roverName, page, null, earthDate, cameraName, apiKey);

            string responseBody = await GetStringAsync(url).ConfigureAwait(true);

            if (string.IsNullOrEmpty(responseBody))
                return null;

            PhotoCollection result = JsonSerializer.Deserialize<PhotoCollection>(responseBody);
            return result.Photos;
        }

        public async Task<IEnumerable<MarsRoverPhoto>> GetMarsRoverPhotosAsync(string roverName, string apiKey, int? page, int sol, string cameraName = "")
        {
            var url = BuildMarsRoverUrlString(roverName, page, sol, null, cameraName, apiKey);

            string responseBody = await GetStringAsync(url).ConfigureAwait(true);

            if (string.IsNullOrEmpty(responseBody))
                return null;

            PhotoCollection result = JsonSerializer.Deserialize<PhotoCollection>(responseBody);
            return result.Photos;
        }
    }
}
