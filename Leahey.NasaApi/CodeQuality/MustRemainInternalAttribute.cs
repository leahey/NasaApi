using System;
using System.Diagnostics.CodeAnalysis;

namespace Leahey.NasaApi.CodeQuality
{
    /// <summary>
    /// NDepend flags methods that could be lower visibility, but doesn't recognize
    /// when methods are internal for testing purposes. This attribute causes NDepend 
    /// to ignore decorated methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [ExcludeFromCodeCoverage]
    public sealed class MustRemainInternalAttribute : Attribute
    {
        public string Reason { get; }

        public MustRemainInternalAttribute(string reason)
        {
            Reason = reason;
        }
    }
}
