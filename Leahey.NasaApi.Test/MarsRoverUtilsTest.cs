using System;
using Leahey.NasaApi.Implementations;
using Xunit;

namespace Leahey.NasaApi.Test
{
    public class MarsRoverUtilsTest
    {
        private MarsRoverUtils tested;

        public MarsRoverUtilsTest()
        {
            tested = new MarsRoverUtils();
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(" ", false)]
        [InlineData("asdf", false)]
        [InlineData("Opportunity", false)]
        [InlineData("opportunity", true)]
        [InlineData("Spirit", false)]
        [InlineData("spirit", true)]
        [InlineData("Curiosity", false)]
        [InlineData("curiosity", true)]
        public void ValidateRoverName_ForNameGiven_ShouldReturnValidity(string roverName, bool expected)
        {
            var actual = tested.ValidateRoverName(roverName);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("opportunity", "fhaz", true)]
        [InlineData("opportunity", "rhaz", true)]
        [InlineData("opportunity", "mast", false)]
        [InlineData("opportunity", "chemcam", false)]
        [InlineData("opportunity", "mahli", false)]
        [InlineData("opportunity", "mardi", false)]
        [InlineData("opportunity", "navcam", true)]
        [InlineData("opportunity", "pancam", true)]
        [InlineData("opportunity", "minites", true)]
        [InlineData("spirit", "fhaz", true)]
        [InlineData("spirit", "rhaz", true)]
        [InlineData("spirit", "mast", false)]
        [InlineData("spirit", "chemcam", false)]
        [InlineData("spirit", "mahli", false)]
        [InlineData("spirit", "mardi", false)]
        [InlineData("spirit", "navcam", true)]
        [InlineData("spirit", "pancam", true)]
        [InlineData("spirit", "minites", true)]
        [InlineData("curiosity", "fhaz", true)]
        [InlineData("curiosity", "rhaz", true)]
        [InlineData("curiosity", "mast", true)]
        [InlineData("curiosity", "chemcam", true)]
        [InlineData("curiosity", "mahli", true)]
        [InlineData("curiosity", "mardi", true)]
        [InlineData("curiosity", "navcam", true)]
        [InlineData("curiosity", "pancam", false)]
        [InlineData("curiosity", "minites", false)]
        public void ValidateCameraName_ForValuesGiven_ShouldReturnValidity(string roverName, string cameraName, bool expected)
        {
            var actual = tested.ValidateCameraName(roverName, cameraName);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("67/18/2010")]
        [InlineData("09/31/2010")]
        [InlineData("April 31, 2018")] // acceptance criteria - invalid date
        public void ConvertEarthDateString_GivenValueIsInvalid_ShouldThrow(string invalidDate)
        {
            Assert.Throws<MarsRoverPhotoApiException>(() => tested.ConvertEarthDateString(invalidDate));
        }

        [Theory]
        [InlineData("67/18/2010", false)]
        [InlineData("09/31/2010", false)]
        [InlineData("April 31, 2018", false)] // acceptance criteria - invalid date
        [InlineData("7/18/2010", true)]
        [InlineData("09/1/2010", true)]
        [InlineData("April 30, 2018", true)]
        public void TryConvertEarthDateString_GivenValueIsInvalid_ShouldReturnSuccess(string invalidDate, bool expected)
        {
            var actual = tested.TryConvertEarthDateString(invalidDate, out _);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1/18/2010", "2010-01-18")]
        [InlineData("April 1 2005", "2005-04-01")]
        [InlineData("Thursday, August 17, 2000 16:32:32", "2000-08-17")]
        [InlineData("02/27/17", "2017-02-27")] // acceptance criteria
        [InlineData("June 2, 2018", "2018-06-02")] // acceptance criteria
        [InlineData("Jul-13-2016", "2016-07-13")] // acceptance criteria
        public void ConvertEarthDateString_GivenValueIsValid_ShouldReturnProperlyFormattedString(string earthDate, string expected)
        {
            var actual = tested.ConvertEarthDateString(earthDate);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(null, typeof(ArgumentException))]
        [InlineData("invalid", typeof(MarsRoverPhotoApiException))]
        public void BuildMarsRoverUrlString_GivenInvalidRoverName_ShouldThrow(string roverName, Type exceptionType)
        {
            Assert.Throws(exceptionType, () => tested.BuildMarsRoverUrlString(roverName, null, null));
        }

        [Theory]
        [InlineData("opportunity", "mast")]
        [InlineData("opportunity", "chemcam")]
        [InlineData("opportunity", "mahli")]
        [InlineData("opportunity", "mardi")]
        [InlineData("opportunity", "invalid")]
        [InlineData("spirit", "mast")]
        [InlineData("spirit", "chemcam")]
        [InlineData("spirit", "mahli")]
        [InlineData("spirit", "mardi")]
        [InlineData("spirit", "invalid")]
        [InlineData("curiosity", "pancam")]
        [InlineData("curiosity", "minites")]
        [InlineData("curiosity", "invalid")]
        public void BuildMarsRoverUrlString_GivenInvalidRoverAndCameraCombination_ShouldThrow(string roverName, string cameraName)
        {
            Assert.Throws<MarsRoverPhotoApiException>(() => tested.BuildMarsRoverUrlString(roverName, null, null, "", cameraName));
        }

        [Theory]
        [InlineData("opportunity", null, 1000, "", "", "https://api.nasa.gov/mars-photos/api/v1/rovers/opportunity/photos?sol=1000&api_key=DEMO_KEY")]
        [InlineData("opportunity", null, null, "2010-04-01", "", "https://api.nasa.gov/mars-photos/api/v1/rovers/opportunity/photos?earth_date=2010-04-01&api_key=DEMO_KEY")]
        [InlineData("opportunity", null, 1000, "", "fhaz", "https://api.nasa.gov/mars-photos/api/v1/rovers/opportunity/photos?sol=1000&camera=fhaz&api_key=DEMO_KEY")]
        [InlineData("opportunity", null, 1000, "", "rhaz", "https://api.nasa.gov/mars-photos/api/v1/rovers/opportunity/photos?sol=1000&camera=rhaz&api_key=DEMO_KEY")]
        [InlineData("opportunity", null, 1000, "", "navcam", "https://api.nasa.gov/mars-photos/api/v1/rovers/opportunity/photos?sol=1000&camera=navcam&api_key=DEMO_KEY")]
        [InlineData("spirit", null, 1000, "", "", "https://api.nasa.gov/mars-photos/api/v1/rovers/spirit/photos?sol=1000&api_key=DEMO_KEY")]
        [InlineData("spirit", null, 1000, "2010-04-01", "", "https://api.nasa.gov/mars-photos/api/v1/rovers/spirit/photos?sol=1000&api_key=DEMO_KEY")] // if both sol and earthDate are given, sol is used.
        [InlineData("spirit", null, 1000, "", "rhaz", "https://api.nasa.gov/mars-photos/api/v1/rovers/spirit/photos?sol=1000&camera=rhaz&api_key=DEMO_KEY")]
        [InlineData("spirit", null, 1000, "", "fhaz", "https://api.nasa.gov/mars-photos/api/v1/rovers/spirit/photos?sol=1000&camera=fhaz&api_key=DEMO_KEY")]
        [InlineData("spirit", null, 1000, "", "navcam", "https://api.nasa.gov/mars-photos/api/v1/rovers/spirit/photos?sol=1000&camera=navcam&api_key=DEMO_KEY")]
        [InlineData("curiosity", null, 1000, "", "", "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&api_key=DEMO_KEY")]
        [InlineData("curiosity", 2, 1000, "", "", "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&page=2&api_key=DEMO_KEY")]
        [InlineData("curiosity", null, 1000, "", "navcam", "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=navcam&api_key=DEMO_KEY")]
        [InlineData("curiosity", null, 1000, "", "mast", "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=mast&api_key=DEMO_KEY")]
        [InlineData("curiosity", null, 1000, "", "chemcam", "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=chemcam&api_key=DEMO_KEY")]
        [InlineData("curiosity", null, 1000, "", "mahli", "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=mahli&api_key=DEMO_KEY")]
        [InlineData("curiosity", null, 1000, "", "mardi", "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&camera=mardi&api_key=DEMO_KEY")]
        public void BuildMarsRoverUrlString_ForGivenValues_ShouldReturnProperUrl(string roverName,
            int? page,
            int? sol,
            string earthDate,
            string cameraName,
            string expected)
        {
            var actual = tested.BuildMarsRoverUrlString(roverName, page, sol, earthDate, cameraName);
            Assert.Equal(expected, actual);
        }
    }
}
