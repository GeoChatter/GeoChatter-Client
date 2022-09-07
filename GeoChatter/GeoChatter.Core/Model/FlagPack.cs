using System.Collections.Generic;

namespace GeoChatter.Core.Model
{
    /// <summary>
    /// Flag pack model
    /// </summary>
    public class FlagPack
    {
        /// <summary>
        /// Flag pack name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Pack information
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Flag aliases and file name mappings
        /// </summary>
        public Dictionary<string, string> Flags { get; set; }
    }
}
