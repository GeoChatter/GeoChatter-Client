namespace GeoChatter.Integrations.Classes
{
    /// <summary>
    /// Streamerbot action model
    /// </summary>
    public class StreamerbotAction
    {
        /// <summary>
        /// Action ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Action name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Action group name
        /// </summary>
        public string group { get; set; }
        /// <summary>
        /// Wheter action is enabled
        /// </summary>
        public bool enabled { get; set; }
    }
}