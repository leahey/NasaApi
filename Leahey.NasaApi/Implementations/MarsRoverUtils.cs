using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Leahey.NasaApi.Interfaces;

[assembly: InternalsVisibleTo("Leahey.NasaApi.Test")]

namespace Leahey.NasaApi.Implementations
{
    public class MarsRoverUtils : IMarsRoverUtils
    {
        #region consts
        public const string Rover_Opportunity = "opportunity";
        public const string Rover_Spirit = "spirit";
        public const string Rover_Curiosity = "curiosity";

        private const string RoverCam_FrontHazardAvoidanceCam = "fhaz";
        private const string RoverCam_RearHazardAvoidanceCam = "rhaz";
        private const string RoverCam_MastCam = "mast";
        private const string RoverCam_ChemCam = "chemcam";
        private const string RoverCam_MarsHandLensImager = "mahli";
        private const string RoverCam_MarsDescentImager = "mardi";
        private const string RoverCam_NavigationCam = "navcam";
        private const string RoverCam_PanoramicCam = "pancam";
        private const string RoverCam_MiniatureThermalEmissionSpectrometer = "minites";
        private const string EarthDateFormatString = "yyyy-MM-dd";
        #endregion

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
        public string BuildMarsRoverUrlString(string roverName,
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

            StringBuilder builder = new StringBuilder($"{NasaApiClient.NasaDomain}{NasaApiClient.ApiRoute_MarsRovers}/{roverName}/{NasaApiClient.ApiRoute_Photos}?");

            // add date/sol param
            if (sol.HasValue)
            {
                builder.Append($"{NasaApiClient.QryParam_Sol}={sol}");
            }
            else if (!string.IsNullOrEmpty(earthDate))
            {
                var formattedDate = ConvertEarthDateString(earthDate);
                builder.Append($"{NasaApiClient.QryParam_EarthDate}={formattedDate}");
            }

            // add page param
            if (page.HasValue)
            {
                builder.Append($"&{NasaApiClient.QryParam_Page}={page}");
            }

            // add camera param
            if (!string.IsNullOrEmpty(cameraName))
            {
                builder.Append($"&{NasaApiClient.QryParam_Camera}={cameraName}");
            }

            builder.Append($"&{NasaApiClient.QryParam_ApiKey}={apiKey}");
            return builder.ToString();
        }

        /// <summary>
        /// Attempts to convert given <paramref name="earthDateString" /> into date format expected by NASA (YYYY-MM-DD).
        /// If given value can not be properly converted, throws.
        /// </summary>
        /// <exception cref="MarsRoverPhotoApiException">Given <paramref name="earthDateString"/> cannot be converted.</exception>
        public string ConvertEarthDateString(string earthDateString)
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

            return earthDate.ToString(EarthDateFormatString, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Attempts to convert given <paramref name="earthDateString"/> into date format expected by NASA (YYYY-MM-DD), 
        /// returning success or failure. If an exception is thrown, it is caught and the method returns <c>false</c>.
        /// </summary>
        public bool TryConvertEarthDateString(string earthDateString, out string formattedDate)
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
        public bool ValidateCameraName(string roverName, string cameraName)
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
        public bool ValidateRoverName(string roverName)
        {
            return roverName == Rover_Opportunity || roverName == Rover_Spirit || roverName == Rover_Curiosity;
        }
    }
}
