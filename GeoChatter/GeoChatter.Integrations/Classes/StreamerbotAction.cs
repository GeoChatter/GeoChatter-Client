using System.Security.Cryptography;

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

        public static StreamerbotAction FromString (string setting)
        {
            var action = new StreamerbotAction();
            if (!string.IsNullOrEmpty(setting))
            {
                string[] parts = setting.Split(';');
                if (parts.Length == 4)
                {
                    action.id = parts[0];
                    action.name = parts[1];
                    action.group = parts[2];
                    action.enabled = bool.Parse(parts[3]);
                }
            }
            return action;

        }
        public override string ToString()
        {
            return $"{id};{name};{group};{enabled}";
        }
    }

    
}