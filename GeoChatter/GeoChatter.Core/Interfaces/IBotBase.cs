using GeoChatter.Core.Model;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoChatter.Core.Interfaces
{
    /// <summary>
    /// Bot interface base for events and common methods and properties
    /// </summary>
    public interface IBotBase : IDisposable
    {
        /// <summary>
        /// Form this bot belongs to
        /// </summary>
        public IMainForm Parent { get; }

        /// <summary>
        /// Get user info from event args object
        /// </summary>
        /// <param name="eventargs"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="userlevel"></param>
        public bool GetUserInfo(object eventargs, [NotNullWhen(true)] out string userid, [NotNullWhen(true)] out string username, out int userlevel);

        /// <summary>
        /// Send a message using the bot
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message);

        /// <summary>
        /// Trigger existing twitch bot commands with given event arguments
        /// </summary>
        /// <param name="eventArgs">Event arguments object to be cast later</param>
        public void TriggerCommands(object eventArgs);

        /// <summary>
        /// Guess recieving event
        /// </summary>
        public event EventHandler<GuessReceivedEventArgs> GuessReceived;
        /// <summary>
        /// Flag request event
        /// </summary>
        public event EventHandler<FlagRequestReceivedEventArgs> FlagRequestReceived;
        /// <summary>
        /// FlagOf request event
        /// </summary>
        public event EventHandler<TargetBotEventArgs> FlagOfTargetReceived;
        /// <summary>
        /// Random guess recieving event
        /// </summary>
        public event EventHandler<RandomGuessRecievedEventArgs> RandomGuessRecieved;
        /// <summary>
        /// Color request event
        /// </summary>
        public event EventHandler<ColorRequestReceivedEventArgs> ColorRequestReceived;
        /// <summary>
        /// ColorOf request event
        /// </summary>
        public event EventHandler<TargetBotEventArgs> ColorOfTargetReceived;
        /// <summary>
        /// Personal stats request event
        /// </summary>
        public event EventHandler<BotEventArgs> MeRequestReceived;
        /// <summary>
        /// Channel best stats request event
        /// </summary>
        public event EventHandler<BotEventArgs> BestRequestReceived;
        /// <summary>
        /// Best stats of target request event
        /// </summary>
        public event EventHandler<TargetBotEventArgs> BestOfRequestReceived;
        /// <summary>
        /// Map information request event
        /// </summary>
        public event EventHandler<BotEventArgs> MapRequestReceived;
        /// <summary>
        /// Map site request event
        /// </summary>
        public event EventHandler<BotEventArgs> LinkRequestReceived;
        /// <summary>
        /// Available colors request event
        /// </summary>
        public event EventHandler<BotEventArgs> ColorsRequestReceived;
        /// <summary>
        /// Available flags request event
        /// </summary>
        public event EventHandler<BotEventArgs> FlagsRequestReceived;
        /// <summary>
        /// Available commands request event
        /// </summary>
        public event EventHandler<BotEventArgs> CommandsRequestReceived;
        /// <summary>
        /// Reset personal stats request event
        /// </summary>
        public event EventHandler<BotEventArgs> ResetStatsRecieved;
        /// <summary>
        /// Application version request event
        /// </summary>
        public event EventHandler<BotEventArgs> VersionRequestRecieved;
        /// <summary>
        /// Banned user event
        /// </summary>
        public event EventHandler<BannedUserReceivedEventArgs> BannedUserReceived;
#if DEBUG
        /// <summary>
        /// Random bot guess request event
        /// </summary>
        public event EventHandler<RandomBotGuessRecievedEventArgs> RandomBotGuessRecieved;
        /// <summary>
        /// Fire <see cref="RandomBotGuessRecieved"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireRandomBotGuessRecieved(RandomBotGuessRecievedEventArgs args);
#endif
        /// <summary>
        /// Fire <see cref="FlagsRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireFlagsRequestReceived(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="FireFlagpacksRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireFlagpacksRequestReceived(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="FireFlagpacksLinkRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireFlagpacksLinkRequestReceived(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="CommandsRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireCommandsRequestReceived(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="VersionRequestRecieved"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireVersionRequestRecieved(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="ResetStatsRecieved"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireResetStatsRecieved(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="ColorsRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireColorsRequestReceived(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="MapRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireMapRequestReceived(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="LinkRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireLinkRequestReceived(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="BestRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireBestRequestReceived(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="BestOfRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireBestOfRequestReceived(TargetBotEventArgs args);
        /// <summary>
        /// Fire <see cref="MeRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireMeRequestReceived(BotEventArgs args);
        /// <summary>
        /// Fire <see cref="GuessReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireGuessReceived(GuessReceivedEventArgs args);
        /// <summary>
        /// Fire <see cref="RandomGuessRecieved"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireRandomGuessRecieved(RandomGuessRecievedEventArgs args);
        /// <summary>
        /// Fire <see cref="FlagOfTargetReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireFlagOfTargetReceived(TargetBotEventArgs args);
        /// <summary>
        /// Fire <see cref="FlagRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireFlagRequestReceived(FlagRequestReceivedEventArgs args);
        /// <summary>
        /// Fire <see cref="ColorRequestReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireColorRequestReceived(ColorRequestReceivedEventArgs args);
        /// <summary>
        /// Fire <see cref="ColorOfTargetReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireColorOfTargetReceived(TargetBotEventArgs args);
        /// <summary>
        /// Fire <see cref="BannedUserReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireBannedUserReceived(BannedUserReceivedEventArgs args);
    }
}
