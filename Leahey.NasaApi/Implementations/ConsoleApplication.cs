using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
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

        [NDependIgnore]
        [ExcludeFromCodeCoverage]
        private int InitialMenu()
        {
            //TODO: smart up this console app hackery
            Console.WriteLine("*** Downloads images for selected date and rover ***");
            Console.WriteLine();
            Console.WriteLine("Select from the following:");
            Console.WriteLine("1. Get all photos for Curiosity on June 3, 2015");

            Console.WriteLine("0. Exit");
            Console.WriteLine();

            int result = -1;

            do
            {
                Console.WriteLine("Press a number to select action.");
                var key = Console.ReadKey();
                Console.WriteLine();
                if (char.IsDigit(key.KeyChar) && int.TryParse(key.KeyChar.ToString(), out int menuitem))
                {
                    if (menuitem == 0 || // exit 
                        menuitem == 1)   // Curiosity, 6/4/2015
                    {
                        result = menuitem;
                    }
                }
            } while (result < 0);

            return result;
        }

        [NDependIgnore]
        [ExcludeFromCodeCoverage]
        private void SavePhotosToDrive(IEnumerable<MarsRoverPhoto> photos)
        {
            foreach (var photo in photos)
            {
                var imagePath = GetImagePath(photo);
                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                var filename = Path.Combine(imagePath, Path.GetFileName(photo.ImgSource));

                _nasaApiClient.DownloadFile(photo.ImgSource, filename);

                Console.WriteLine($"Saved {photo.Rover.Name} {photo.Camera.Name} photo to {filename}.");
            }

            //ncrunch: no coverage start
            static string GetImagePath(MarsRoverPhoto photo)
            {
                return Path.Combine(Path.GetTempPath(), "NasaPhotos", "MarsRovers", photo.Rover.Name, photo.Camera.Name, photo.EarthDate);
            }
            //ncrunch: no coverage end
        }
        #endregion

        [NDependIgnore]
        public async Task Run()
        {
            //ncrunch: no coverage start
            try
            {
                var actionNumber = InitialMenu();
                while (actionNumber > 0)
                {
                    string roverName = "opportunity";
                    int? page = null;
                    string earthDate = "2012-04-01";

                    if (actionNumber == 1)
                    {
                        roverName = "curiosity";
                        earthDate = "2015-06-03";
                    }

                    IEnumerable<MarsRoverPhoto> photos =
                        await _nasaApiClient.GetMarsRoverPhotosAsync(roverName: roverName,
                            apiKey: ApiKey,
                            page: page,
                            earthDate: earthDate)
                        .ConfigureAwait(true);

                    if (photos.Any())
                    {
                        Console.WriteLine($"{photos.Count()} photos were found for {roverName} rover on {earthDate}");
                        Console.WriteLine();
                        SavePhotosToDrive(photos);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine($"No photos for the given date '{earthDate}'.");
                    }


                    actionNumber = InitialMenu();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message} ");
            }
            //ncrunch: no coverage end
        }

        [NDependIgnore]
        [ExcludeFromCodeCoverage]
        public ConsoleApplication(INasaApiClient nasaApiClient)
        {
            _nasaApiClient = nasaApiClient;
        }
    }
}
