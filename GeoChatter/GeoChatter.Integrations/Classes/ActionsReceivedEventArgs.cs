using System;
using System.Collections.Generic;

namespace GeoChatter.Integrations.Classes
{
    /// <summary>
    /// Streamerbot action event arguments
    /// </summary>
    public class ActionsReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// List of actions
        /// </summary>
        public List<StreamerbotAction> Actions { get; } = new List<StreamerbotAction>();
    }
}
