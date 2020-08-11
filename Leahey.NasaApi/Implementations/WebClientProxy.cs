using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Leahey.NasaApi.CodeQuality;
using Leahey.NasaApi.Interfaces;

namespace Leahey.NasaApi.Implementations
{
    /// <summary>
    /// Proxy class to <see cref="WebClient"/>. This allows tests to mock up a web client.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [NDependIgnore("Proxy/pass-through methods.")]
    public class WebClientProxy : IWebClientProxy
    {
        public WebClientProxy()
        {
        }

        /// <summary>
        /// Downloads the resource with the given <paramref name="address"/> to a local file.
        /// </summary>
        public void DownloadFile(string address, string filename)
        {
            var client = new WebClient();
            client.DownloadFile(address, filename);
        }

        /// <summary>
        /// Downloads, to <paramref name="filename"/>, the resource with the given <paramref name="address"/>. 
        /// This method does not block the calling thread.
        /// </summary>
        public void DownloadFileAsync(string address, string filename)
        {
            var uri = new Uri(address);
            var client = new WebClient();
            client.DownloadFileAsync(uri, filename);
        }
    }
}
