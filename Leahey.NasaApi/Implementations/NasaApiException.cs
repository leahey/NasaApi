using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Leahey.NasaApi.Implementations
{
    /// <summary>
    /// Base exception class for NasaApi exceptions.
    /// </summary>
    [Serializable]
    public class NasaApiException : Exception
    {
        /// <summary>
        /// Constructs a new NasaApiException.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public NasaApiException() { }

        /// <summary>
        /// Constructs a new NasaApiException.
        /// </summary>
        /// <param name="message">The exception message</param>
        public NasaApiException(string message) : base(message) { }

        /// <summary>
        /// Constructs a new NasaApiException.
        /// </summary>
        /// <param name="message">The exception message</param>
        /// <param name="innerException">The inner exception</param>
        public NasaApiException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected NasaApiException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
