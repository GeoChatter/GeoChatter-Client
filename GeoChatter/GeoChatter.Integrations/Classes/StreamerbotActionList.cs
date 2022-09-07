using System.Collections.Generic;

namespace GeoChatter.Integrations.Classes
{
    /// <summary>
    /// List of Streamerbot actions
    /// </summary>
    public class StreamerbotActionList
    {
        /// <summary>
        /// List ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Action count
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// List of actions
        /// </summary>
        public List<StreamerbotAction> actions { get; } = new List<StreamerbotAction>();
        /// <summary>
        /// Action status
        /// </summary>
        public string status { get; set; }
    }
}