using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Leahey.NasaApi.Test")]

namespace Leahey.NasaApi.Interfaces
{
    public interface IWebClientProxy
    {
        /// <summary>
        /// Downloads the resource with the given <paramref name="address"/> to a local file.
        /// </summary>
        void DownloadFile(string address, string fileName);

        /// <summary>
        /// Downloads, to <paramref name="fileName"/>, the resource with the given <paramref name="address"/>. 
        /// This method does not block the calling thread.
        /// </summary>
        void DownloadFileAsync(string address, string fileName);
    }
}
