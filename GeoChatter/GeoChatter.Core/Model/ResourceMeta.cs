using Newtonsoft.Json;
using System.IO;

namespace GeoChatter.Core.Model
{
    /// <summary>
    /// Resource meta file model
    /// </summary>
    public sealed class ResourceMeta
    {
        /// <summary>
        /// Resource name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Resource version
        /// </summary>
        public int version { get; set; }
        /// <summary>
        /// Resource target root directory
        /// </summary>
        public string target { get; set; }
        /// <summary>
        /// Resource target content directory
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// Resource change log from last version
        /// </summary>
        public string changelog { get; set; }

        /// <summary>
        /// Content files target directory
        /// </summary>
        [JsonIgnore]
        public string TargetDirectory => Path.Combine(target, content);
    }
}
