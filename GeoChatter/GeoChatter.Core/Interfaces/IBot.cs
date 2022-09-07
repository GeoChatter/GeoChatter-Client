using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GeoChatter.Core.Interfaces
{
    /// <summary>
    /// Bot interface for events and common methods and properties
    /// </summary>
    /// <typeparam name="TCommandType">Command type associated with this bot</typeparam>
    /// <typeparam name="TMessageModel">Message model</typeparam>
    public interface IBot<TCommandType, TMessageModel> : IBotBase
    {
        /// <summary>
        /// Returns the commands discovered for the bot
        /// </summary>
        public static List<TCommandType> Commands { get; }

        /// <summary>
        /// Get event arguments object as <typeparamref name="TMessageModel"/>. Returns <see langword="true"/> if <paramref name="e"/> was cast to <paramref name="message"/>.
        /// </summary>
        /// <param name="e">event arguments</param>
        /// <param name="message">message object</param>
        /// <param name="underlyingType">if <paramref name="message"/> had a base class, this will be the type</param>
        public bool GetEventArgObject([NotNullWhen(true)] object e, [NotNullWhen(true)] out TMessageModel message, [NotNullWhen(true)] out Type underlyingType);

    }
}
