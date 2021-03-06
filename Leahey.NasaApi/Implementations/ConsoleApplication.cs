﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Leahey.NasaApi.CodeQuality;
using Leahey.NasaApi.Interfaces;
using Leahey.NasaApi.Models;

namespace Leahey.NasaApi.Implementations
{
    //ncrunch: no coverage start
    public class ConsoleApplication : IConsoleApplication
    {
        #region private
        private readonly INasaApiClient _nasaApiClient;
        private const string ApiKey = "HLwOWblObCvPU07yhPPS70hNlvo32SC1Xa0Na7yp";

        [NDependIgnore]
        [ExcludeFromCodeCoverage]
        private async Task RetrieveRoverPhotos(string roverName, int? page, string earthDate)
        {
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
                Console.WriteLine($"No photos for {roverName} for the given date '{earthDate}'.");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Prompts for what type of action to take.
        /// </summary>
        [NDependIgnore]
        [ExcludeFromCodeCoverage]
        private int InitialMenu()
        {
            Console.WriteLine("Select from the following:");
            Console.WriteLine("1. Get all photos for Curiosity on June 3, 2015");
            Console.WriteLine("2. Get all photos for a specified rover on a certain date.");
            Console.WriteLine("3. Get all photos for all rovers on a certain date.");
            Console.WriteLine("4. Get all photos for all rovers for dates in Dates.txt.");

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
                    if (menuitem == 0      // exit 
                        || menuitem == 1   // Curiosity, 6/4/2015
                        || menuitem == 2   // any rover, any date
                        || menuitem == 3   // all rovers, any date
                        || menuitem == 4)  // read dates from file
                    {
                        result = menuitem;
                    }
                }
            } while (result < 0);

            return result;
        }

        /// <summary>
        /// Prompts the user to select a rover and enter a date.
        /// </summary>
        [NDependIgnore]
        [ExcludeFromCodeCoverage]
        private (string name, string date) GetRoverNameAndDate()
        {
            Console.WriteLine("Select a rover:");
            Console.WriteLine("[S]pirit");
            Console.WriteLine("[O]pportunity");
            Console.WriteLine("[C]uriosity");

            const string exit = "exit";

            var roverName = string.Empty;
            var earthDate = string.Empty;

            do
            {
                Console.WriteLine("Type the first letter of your rover choice. Press 'X' to exit.");
                var roverKey = Console.ReadKey();
                if (roverKey.Key == ConsoleKey.S)
                {
                    roverName = MarsRoverUtils.Rover_Spirit;
                }
                else if (roverKey.Key == ConsoleKey.O)
                {
                    roverName = MarsRoverUtils.Rover_Opportunity;
                }
                else if (roverKey.Key == ConsoleKey.C)
                {
                    roverName = MarsRoverUtils.Rover_Curiosity;
                }
                else if (roverKey.Key == ConsoleKey.X)
                {
                    roverName = exit;
                }
                Console.WriteLine();
            } while (string.IsNullOrEmpty(roverName));

            if(roverName != exit)
            {
                earthDate = GetEarthDate();
            }

            return (roverName, earthDate);
        }

        /// <summary>
        /// Prompts the user to enter an earth date.
        /// </summary>
        [NDependIgnore]
        [ExcludeFromCodeCoverage]
        private static string GetEarthDate()
        {
            var result = string.Empty;
            do
            {
                Console.WriteLine("Input an Earth date:");
                var input = Console.ReadLine();

                if (DateTime.TryParse(input, out _))
                {
                    result = input;
                }
                else
                {
                    Console.WriteLine($"'{input}' could not be converted into a date value.");
                    Console.WriteLine();
                }
            }
            while (string.IsNullOrEmpty(result));

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

            static string GetImagePath(MarsRoverPhoto photo)
            {
                return Path.Combine(Path.GetTempPath(), "NasaPhotos", "MarsRovers", photo.Rover.Name, photo.Camera.Name, photo.EarthDate);
            }
        }

        #endregion

        private async Task HandleAction01Async()
        {
            await RetrieveRoverPhotos("curiosity", null, "2015-06-03").ConfigureAwait(true);
        }

        private async Task<int> HandleAction02Async()
        {
            var result = 2;

            var values = GetRoverNameAndDate();
            if (values.name == "exit")
            {
                result = 0;
            }
            else
            {
                await RetrieveRoverPhotos(values.name, null, values.date).ConfigureAwait(true);
            }

            return result;
        }

        private async Task HandleAction03Async()
        {
            var earthDate = GetEarthDate();

            await GetRoverPhotosForDate(earthDate).ConfigureAwait(true);
        }

        private async Task GetRoverPhotosForDate(string earthDate)
        {
            // get photos for all 3 rovers
            await RetrieveRoverPhotos(MarsRoverUtils.Rover_Spirit, null, earthDate).ConfigureAwait(true);
            await RetrieveRoverPhotos(MarsRoverUtils.Rover_Opportunity, null, earthDate).ConfigureAwait(true);
            await RetrieveRoverPhotos(MarsRoverUtils.Rover_Curiosity, null, earthDate).ConfigureAwait(true);
        }

        private async Task HandleAction04Async(string dataFilename)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location);
            var filepath = Path.Combine(assemblyPath, "data", dataFilename);

            if (File.Exists(filepath))
            {
                var contents = File.ReadAllLines(filepath);

                foreach (var date in contents)
                {
                    if (!DateTime.TryParse(date, out _))
                    {
                        Console.WriteLine($"Given date string, '{date}' is an invalid date and cannot be used for querying.");
                        Console.WriteLine();
                    }
                    else
                    {
                        await GetRoverPhotosForDate(date).ConfigureAwait(true);
                    }
                }
            }
        }

        [NDependIgnore]
        public async Task Run()
        {
            try
            {
                Console.WriteLine("*** Downloads images for selected date and rover ***");
                Console.WriteLine();

                var actionNumber = InitialMenu();
                while (actionNumber > 0)
                {
                    if (actionNumber == 1)
                    {
                        await HandleAction01Async().ConfigureAwait(true);
                    }
                    else if (actionNumber == 2)
                    {
                        actionNumber = await HandleAction02Async().ConfigureAwait(true);
                    }
                    else if (actionNumber == 3)
                    {
                        await HandleAction03Async().ConfigureAwait(true);
                    }
                    else if (actionNumber == 4)
                    {
                        await HandleAction04Async("dates.txt").ConfigureAwait(true);
                    }

                    if (actionNumber > 0)
                    {
                        actionNumber = InitialMenu();
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine($"Message :{e.Message} ");
            }
        }


        [NDependIgnore]
        [ExcludeFromCodeCoverage]
        public ConsoleApplication(INasaApiClient nasaApiClient)
        {
            _nasaApiClient = nasaApiClient;
        }
    }
    //ncrunch: no coverage end
}
