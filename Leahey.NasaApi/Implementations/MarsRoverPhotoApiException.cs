using System;
using System.Runtime.Serialization;

namespace Leahey.NasaApi.Implementations
{
    [Serializable]
    public class MarsRoverPhotoApiException : NasaApiException
    {
        /// <summary>
        /// Constructs a new NasaApiException.
        /// </summary>
        public MarsRoverPhotoApiException()
        {

        }

        /// <summary>
        /// Constructs a new NasaApiException.
        /// </summary>
        /// <param name="message">The exception message</param>
        public MarsRoverPhotoApiException(string message) : base(message)
        {

        }

        /// <summary>
        /// Constructs a new NasaApiException.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The inner exception</param>
        public MarsRoverPhotoApiException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        protected MarsRoverPhotoApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
