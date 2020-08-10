using System;
using System.Net.Http;
using System.Threading.Tasks;
using Leahey.NasaApi.Implementations;

namespace Leahey.NasaApi.Interfaces
{
    public interface INasaApiClient
    {
        Task<HttpResponseMessage> GetAsync(string str);
        Task<string> GetRoverPhotoStringAsync(string str);
        Task<MarsRoverPhoto> GetRoverPhotoAsync(string str);
    }
}
