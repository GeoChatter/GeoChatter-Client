using System;

namespace GeoChatter.Core.Attributes
{
    /// <summary>
    /// Twitch command attribute for directly registering a method to given trigger
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CommandAttribute : Attribute
    {
        /// <summary>
        /// Command names and aliases to check wheter 
        /// <para>First name is taken as base name, rest are considered aliases</para>
        /// </summary>
        public string[] Names { get; private set; }
        /// <summary>
        /// Base name of the command
        /// </summary>
        public string Name => Names != null && Names.Length > 0 ? Names[0] : string.Empty;

        /// <summary>
        /// Command information
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="names"></param>
        public CommandAttribute(params string[] names)
        {
            Names = names;
        }
    }
}
