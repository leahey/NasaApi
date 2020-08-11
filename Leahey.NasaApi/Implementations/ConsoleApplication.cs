using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Leahey.NasaApi.CodeQuality;
using Leahey.NasaApi.Interfaces;
using Leahey.NasaApi.Models;

namespace Leahey.NasaApi.Implementations
{
    public class ConsoleApplication : IConsoleApplication
    {
        #region private
        private readonly INasaApiClient _nasaApiClient;
        private const string ApiKey = "HLwOWblObCvPU07yhPPS70hNlvo32SC1Xa0Na7yp";
        private const string Date = "June 2, 2018";
        #endregion

        [NDependIgnore]
        public async Task Run()
        {
            try
            {
                //var response = await _nasaApiClient.GetAsync("https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=fhaz&api_key=DEMO_KEY").ConfigureAwait(true);
                //response.EnsureSuccessStatusCode();
                //var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                // Above three lines can be replaced with new helper method below
                //var responseBody = await _nasaApiClient.GetRoverPhotoStringAsync("https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=fhaz&api_key=DEMO_KEY").ConfigureAwait(true);
                //var photo = await _nasaApiClient.GetRoverPhotoAsync("https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=fhaz&api_key=DEMO_KEY").ConfigureAwait(true);

                IEnumerable<MarsRoverPhoto> photos =
                    await _nasaApiClient.GetMarsRoverPhotosAsync(roverName: "opportunity",
                        apiKey: ApiKey,
                        page: 1,
                        earthDate: Date)
                    .ConfigureAwait(true);

                if (photos.Any())
                {
                    Console.WriteLine(photos.ToList()[0].ImgSource);
                }
                else
                {
                    Console.WriteLine($"No photos for the given date '{Date}'.");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message} ");
            }
        }

        [NDependIgnore]
        public ConsoleApplication(INasaApiClient nasaApiClient)
        {
            _nasaApiClient = nasaApiClient;
        }
    }
}
