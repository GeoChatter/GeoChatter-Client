using GeoChatter.Core.Attributes;
using System;
using System.Collections.Generic;

namespace GeoChatter.Core.Interfaces
{
    /// <summary>
    /// Base command interface all commands must implement
    /// </summary>
    public interface ICommandBase
    {
        /// <summary>
        /// List of developer identifiers for <see cref="CommandRestrictionAttribute.CanDeveloperBypass"/> checks
        /// </summary>
        public static List<string> DeveloperIDs { get; } = new List<string>()
        {
            "91322274",  // rhinoooo_
            "206992018", // NoBuddyIsPerfect
            "510610636", // Soeren_______
        };

        /// <summary>
        /// Meta attributes
        /// </summary>
        public CommandAttribute Meta { get; set; }

        /// <summary>
        /// Restriction attributes
        /// </summary>
        public CommandRestrictionAttribute Restrictions { get; set; }

        /// <summary>
        /// Last time ever command was successfully called and fired events
        /// </summary>
        public DateTime LastGlobalCall { get; set; }

        /// <summary>
        /// Last time ever command successfully sent a message
        /// </summary>
        public DateTime LastGlobalMessage { get; set; }

        /// <summary>
        /// Last time per user that the command was successfully called and fired events
        /// </summary>
        public Dictionary<string, DateTime> LastUserCall { get; set; }

        /// <summary>
        /// Command enabled state
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Command description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Command trigger character
        /// </summary>
        public char TriggerChar { get; set; }

        /// <summary>
        /// Command name without trigger character
        /// </summary>
        public List<string> CommandNames { get; set; }

        /// <summary>
        /// Try sending a message using the <paramref name="bot"/>
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="message"></param>
        /// <param name="isModOrAbove"></param>
        /// <returns></returns>
        public bool SendMessage(IBotBase bot, string message, bool isModOrAbove = false);

        /// <summary>
        /// Check if command can be executed with current cooldown restrictions
        /// </summary>
        /// <param name="bot">Bot to use with eventargs data</param>
        /// <param name="eventArgs">Event arguments object to be cast</param>
        /// <returns></returns>
        public bool CommandCooldownAvailablity(IBotBase bot, object eventArgs);

        /// <summary>
        /// Get command name from message, that is the first word of the message
        /// </summary>
        /// <param name="msg">Message to use</param>
        /// <returns></returns>
        public static string GetCommandFromMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return string.Empty;
            }

            string[] parts = msg.Split(' ');
            return parts[0].Trim();
        }
    }
}
