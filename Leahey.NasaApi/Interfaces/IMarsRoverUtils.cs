using System;
using System.Runtime.CompilerServices;
using Leahey.NasaApi.Implementations;

[assembly: InternalsVisibleTo("Leahey.NasaApi.Test")]

namespace Leahey.NasaApi.Interfaces
{
    public interface IMarsRoverUtils
    {
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
        string BuildMarsRoverUrlString(string roverName, int? page, int? sol, string earthDate = "", string cameraName = "", string apiKey = "DEMO_KEY");

        /// <summary>
        /// Attempts to convert given <paramref name="earthDateString" /> into date format expected by NASA (YYYY-MM-DD).
        /// If given value can not be properly converted, throws.
        /// </summary>
        string ConvertEarthDateString(string earthDateString);

        /// <summary>
        /// Attempts to convert given <paramref name="earthDateString"/> into date format expected by NASA (YYYY-MM-DD), 
        /// returning success or failure. If an exception is thrown, it is caught and the method returns <c>false</c>.
        /// </summary>
        bool TryConvertEarthDateString(string earthDateString, out string formattedDate);

        /// <summary>
        /// Validates that for the given <paramref name="roverName"/>, <paramref name="cameraName"/> is one of the proper values.
        /// </summary>
        bool ValidateCameraName(string roverName, string cameraName);

        /// <summary>
        /// Validates that <paramref name="roverName"/> is one of the proper values.
        /// </summary>
        bool ValidateRoverName(string roverName);
    }
}
