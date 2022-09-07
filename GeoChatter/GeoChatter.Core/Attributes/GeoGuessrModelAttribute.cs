using System;

namespace GeoChatter.Core.Attributes
{
    /// <summary>
    /// For GG API method definitions
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class GeoGuessrAPIResponseAttribute : Attribute
    {
        /// <summary>
        /// Response
        /// </summary>
        public Type ResponseObjectType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseType"></param>
        public GeoGuessrAPIResponseAttribute(Type responseType)
        {
            ResponseObjectType = responseType;
        }
    }
}
