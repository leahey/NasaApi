using System;
using System.Diagnostics.CodeAnalysis;

namespace Leahey.NasaApi.CodeQuality
{
    /// <summary>
    /// Instructs NDepend to ignore the decorated symbol entirely.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    [ExcludeFromCodeCoverage]
    public sealed class NDependIgnoreAttribute : Attribute
    {
        public NDependIgnoreAttribute() :
            this(string.Empty)
        {

        }

        public NDependIgnoreAttribute(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }
}
