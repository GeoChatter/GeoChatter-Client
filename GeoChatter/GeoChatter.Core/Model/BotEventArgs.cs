using GeoChatter.Core.Interfaces;
using GeoChatter.Model.Enums;
using System;

namespace GeoChatter.Core.Model
{
    /// <summary>
    /// Event arguments for an event bound to a <see cref="IBotBase"/> bot
    /// </summary>
    public class BotEventArgs : EventArgs
    {
        /// <summary>
        /// User name triggered the event
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User ID triggered the event
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Responsible bot
        /// </summary>
        public IBotBase Bot { get; set; }

        /// <summary>
        /// Command source
        /// </summary>
        public ICommandBase Command { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Platforms UserPlatform { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProfilePicUrl { get; set; }
/// <summary>
/// 
/// </summary>
/// <param name="userid"></param>
/// <param name="username"></param>
/// <param name="bot"></param>
/// <param name="command"></param>
        public BotEventArgs(string userid, string username, IBotBase bot, ICommandBase command,Platforms userPlatform = Platforms.Twitch, string displayName = "", string profilePicUrl = "")
        {
            Username = username;
            UserId = userid;
            Bot = bot;
            Command = command;
            UserPlatform = userPlatform;
            DisplayName = displayName;
            ProfilePicUrl = profilePicUrl;
        }
    }

    /// <summary>
    /// Event with a username target as first argument
    /// </summary>
    public sealed class TargetBotEventArgs : BotEventArgs
    {
        private string target;

        /// <summary>
        /// Target user name lower cased
        /// </summary>
        public string Target
        {
            get => target;
            set => target = value?.TrimStart('@').ToLowerInvariant();
        }

        /// <summary>
        /// Wheter target is caller itself
        /// </summary>
        public bool IsSelfReference { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid">Caller user id</param>
        /// <param name="username">Caller user name</param>
        /// <param name="targetName">Target user name</param>
        /// <param name="isSelf">Wheter target is caller itself</param>
        /// <param name="bot">Bot</param>
        /// <param name="command">Command</param>
        public TargetBotEventArgs(string userid, string username, string targetName, bool isSelf, IBotBase bot, ICommandBase command)
            : base(userid, username, bot, command)
        {
            Target = targetName;
            IsSelfReference = isSelf;
        }
    }
}
