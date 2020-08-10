using System;
using System.Net.Http;
using System.Threading.Tasks;
using Leahey.NasaApi.Interfaces;

namespace Leahey.NasaApi.Implementations
{
    public class ConsoleApplication : IConsoleApplication
    {
        private readonly INasaApiClient _nasaApiClient;

        public async Task Run()
        {
            try
            {
                //var response = await _nasaApiClient.GetAsync("https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=fhaz&api_key=DEMO_KEY").ConfigureAwait(true);
                //response.EnsureSuccessStatusCode();
                //var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                // Above three lines can be replaced with new helper method below
                //var responseBody = await _nasaApiClient.GetRoverPhotoStringAsync("https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=fhaz&api_key=DEMO_KEY").ConfigureAwait(true);
                var photo = await _nasaApiClient.GetRoverPhotoAsync("https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=fhaz&api_key=DEMO_KEY").ConfigureAwait(true);

                Console.WriteLine(photo.ImgSource);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message} ");
            }
        }

        public ConsoleApplication(INasaApiClient nasaApiClient)
        {
            _nasaApiClient = nasaApiClient;
        }
    }
}
