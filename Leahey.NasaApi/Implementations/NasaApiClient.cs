using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Leahey.NasaApi.Interfaces;
using Leahey.NasaApi.Models;

namespace Leahey.NasaApi.Implementations
{
    public class NasaApiClient : HttpClient, INasaApiClient
    {
        //https://api.nasa.gov/mars-photos/api/v1/rovers/spirit/photos?sol=500&api_key=DEMO_KEY


        private static readonly string NasaDomain = "https://api.nasa.gov";

        private static readonly string ApiRoute_MarsRovers = "/mars-photos/api/v1/rovers";
        private static readonly string ApiRoute_AstronomyPicOfDay = "/planetary/apod";

        private static readonly string Rover_Opportunity = "/opportunity";
        private static readonly string Rover_Spirit = "/spirit";
        private static readonly string Rover_Curiosity = "/curiosity";

        private static readonly string RoverCam_FrontHazardAvoidanceCam = "fhaz";
        private static readonly string RoverCam_RearHazardAvoidanceCam = "rhaz";
        private static readonly string RoverCam_MastCam = "mast";
        private static readonly string RoverCam_ChemCam = "chemcam";
        private static readonly string RoverCam_MarsHandLensImager = "mahli";
        private static readonly string RoverCam_MarsDescentImager = "mardi";



        private static readonly string ApiKey = "HLwOWblObCvPU07yhPPS70hNlvo32SC1Xa0Na7yp";

        public async Task<MarsRoverPhoto> GetRoverPhotoAsync(string str)
        {
            string responseBody = GetRoverPhotoStringAsync(str).Result;

            if(string.IsNullOrEmpty(responseBody))
                return null;

            PhotoCollection result = JsonSerializer.Deserialize<PhotoCollection>(responseBody);
            return result.Photos[0];

            throw new NotImplementedException();
        }

        public async Task<string> GetRoverPhotoStringAsync(string str)
        {
            return await GetStringAsync(str);
        }
    }
}
