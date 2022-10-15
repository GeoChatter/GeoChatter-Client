using GeoChatter.Core.Interfaces;
using GeoChatter.Model;
using GeoChatter.Model.Enums;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GeoChatter.Core.Model
{
    #region Command
    /// <summary>
    /// Arguments for <see cref="IBotBase.GuessReceived"/>
    /// </summary>
    public class GuessReceivedEventArgs : BotEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="platform"></param>
        /// <param name="bot"></param>
        /// <param name="command"></param>
        public GuessReceivedEventArgs(string userid, string username, Platforms platform, IBotBase bot, ICommandBase command)
        : base(userid, username, bot, command, platform) { }

        /// <summary>
        /// Guess latitude
        /// </summary>
        public string Lat { get; set; }
        /// <summary>
        /// Guess longitude
        /// </summary>
        public string Lng { get; set; }
        /// <summary>
        /// User name color
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// Wheter guess was random
        /// </summary>
        public bool WasRandom { get; set; }
        /// <summary>
        /// See <see cref="Guess.RandomGuessArgs"/>
        /// </summary>
        public string RandomGuessArgs { get; set; }
        /// <summary>
        /// See <see cref="Guess.Source"/>
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// See <see cref="Guess.Layer"/>
        /// </summary>
        public string Layer { get; set; }
    }

    /// <summary>
    /// Arguments for <see cref="IBotBase.FlagRequestReceived"/>
    /// </summary>
    public class FlagRequestReceivedEventArgs : BotEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="bot"></param>
        /// <param name="command"></param>
        /// <param name="userPlatform"></param>
        /// <param name="displayName"></param>
        /// <param name="profilePicUrl"></param>
        public FlagRequestReceivedEventArgs(string userid, string username, IBotBase bot, ICommandBase command, Platforms userPlatform = Platforms.Twitch, string displayName = "", string profilePicUrl = "")
: base(userid, username, bot, command, userPlatform, displayName, profilePicUrl) { }

        /// <summary>
        /// Requested flag code or name
        /// </summary>
        public string Flag { get; set; }
    }

    /// <summary>
    /// Arguments for <see cref="IBotBase.ColorRequestReceived"/>
    /// </summary>
    public class ColorRequestReceivedEventArgs : BotEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="bot"></param>
        /// <param name="command"></param>
        /// <param name="userPlatform"></param>
        /// <param name="displayName"></param>
        /// <param name="profilePicUrl"></param>
        public ColorRequestReceivedEventArgs(string userid, string username, IBotBase bot, ICommandBase command, Platforms userPlatform = Platforms.Twitch, string displayName = "", string profilePicUrl = "")
: base(userid, username, bot, command, userPlatform, displayName, profilePicUrl) { }

        /// <summary>
        /// Requested color name or value
        /// </summary>
        public string Color { get; set; }
    }

    /// <summary>
    /// Arguments for <see cref="IBotBase.BannedUserReceived"/>
    /// </summary>
    public class BannedUserReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public BannedUserReceivedEventArgs(string userId) { UserId = userId; }

        /// <summary>
        /// User ID to be banned
        /// </summary>
        public string UserId { get; set; }
    }

    /// <summary>
    /// Arguments for <see cref="IBotBase.RandomGuessRecieved"/>
    /// </summary>
    public class RandomGuessRecievedEventArgs : BotEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="bot"></param>
        /// <param name="command"></param>
        /// <param name="platform"></param>
        public RandomGuessRecievedEventArgs(string userid, string username, Platforms platform, IBotBase bot, ICommandBase command)
            : base(userid, username, bot, command, platform) { }

        /// <summary>
        /// Extra guess arguments
        /// </summary>
        public string Arguments { get; set; }
        /// <summary>
        /// User name color
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// User profile picture
        /// </summary>
        public string ProfilePicture { get; set; }
        /// <summary>
        /// See <see cref="Guess.RandomGuessArgs"/>
        /// </summary>
        public string RandomGuessArgs { get; set; }
        /// <summary>
        /// See <see cref="Guess.Source"/>
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// See <see cref="Guess.Layer"/>
        /// </summary>
        public string Layer { get; set; }
    }

    /// <summary>
    /// Arguments for <see cref="IBotBase.ResetStatsRecieved"/>
    /// </summary>
    public class ResetStatsRecievedEventArgs : BotEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="bot"></param>
        /// <param name="command"></param>
        /// <param name="platform"></param>
        public ResetStatsRecievedEventArgs(string userid, string username, Platforms platform, IBotBase bot, ICommandBase command)
            : base(userid, username, bot, command, platform) { }

        /// <summary>
        /// Extra arguments
        /// </summary>
        public string Arguments { get; set; }
    }

#if DEBUG
    /// <summary>
    /// Arguments for <see cref="IBotBase.RandomBotGuessRecieved"/>
    /// </summary>
    public class RandomBotGuessRecievedEventArgs : BotEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="bot"></param>
        /// <param name="command"></param>
        /// <param name="platform"></param>
        public RandomBotGuessRecievedEventArgs(string userid, string username, Platforms platform, IBotBase bot, ICommandBase command)
            : base(userid, username, bot, command, platform) { }

        /// <summary>
        /// Amount of guesses
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Wheter to re-use previously selected bots
        /// </summary>
        public bool Reuse { get; set; }
    }
#endif
    #endregion

    #region Non Command
    /// <summary>
    /// Arguments for bot failing to join chat
    /// </summary>
    public class ChannelNotJoinedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        public ChannelNotJoinedEventArgs(string channel, string msg)
        {
            Channel = channel;
            Message = msg;
        }
        /// <summary>
        /// Channel name
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// Fail message
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Arguments for bot joining a channel chat
    /// </summary>
    public class ChannelJoinedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="bot"></param>
        public ChannelJoinedEventArgs(string channel, string bot)
        {
            Channel = channel;
            BotName = bot;
        }
        /// <summary>
        /// Channel name
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// Bot name
        /// </summary>
        public string BotName { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class TableColumnChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public TableColumn Column { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        public TableColumnChangedEventArgs(TableColumn col)
        {
            Column = col;
        }

    }
    #endregion
}
