using GeoChatter.Core.Attributes;
using GeoChatter.Core.Interfaces;
using GeoChatter.Model.Enums;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoChatter.Web.YouTube
{
    /// <summary>
    /// Twitch command for the bot
    /// </summary>
    public class YoutubeCommand : ICommand<YoutubeBot>
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Description { get => Meta.Description; set => Meta.Description = value; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public CommandAttribute Meta { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public CommandRestrictionAttribute Restrictions { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime LastGlobalCall { get; set; } = DateTime.MinValue;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime LastGlobalMessage { get; set; } = DateTime.MinValue;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Dictionary<string, DateTime> LastUserCall { get; set; } = new();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public char TriggerChar { get; set; } = '!';

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public List<string> CommandNames { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICommand<YoutubeBot>.CommandDef Command { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool SendMessage(IBotBase bot, string message, bool isModOrAbove = false)
        {
            DateTime now = DateTime.Now;
            if (!isModOrAbove && LastGlobalMessage.AddSeconds(Restrictions.MessageCooldownSeconds) > now)
            {
                return false;
            }

            LastGlobalMessage = now;

            bot?.SendMessage(message);

            return true;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanBeTriggeredWith(YoutubeBot bot, object eventArgs)
        {
            if (bot == null
                || eventArgs == null
                || !bot.GetEventArgObject(eventArgs, out LiveChatMessage chat, out Type _))
            {
                return false;
            }

            string message = chat.Snippet.DisplayMessage;

            if (!message.StartsWith(TriggerChar))
            {
                return false;
            }
            message = message.TrimStart(TriggerChar);

            return CommandNames.FirstOrDefault(c => c == message) != null;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CommandCooldownAvailablity(IBotBase bot, object eventArgs)
        {
            if (bot == null || !bot.GetUserInfo(eventArgs, out string userid, out string _, out int _))
            {
                return false;
            }

            switch (Restrictions.CooldownTarget)
            {
                case CooldownTarget.Individual:
                    if (LastUserCall.ContainsKey(userid))
                    {
                        return LastUserCall[userid].AddSeconds(Restrictions.CommandCooldownSeconds) <= DateTime.Now;
                    }
                    else
                    {
                        LastUserCall.Add(userid, DateTime.Now);
                        break;
                    }

                case CooldownTarget.Global:
                    return LastGlobalCall.AddSeconds(Restrictions.CommandCooldownSeconds) <= DateTime.Now;

                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void CallCommand(YoutubeBot bot, object eventArgs)
        {
            if (bot == null
                || eventArgs == null
                || !bot.GetUserInfo(eventArgs, out string userid, out string _, out int _)
                || !bot.GetEventArgObject(eventArgs, out LiveChatMessage chat, out Type _)
                || (Restrictions.AllowedState != AppGameState.ANYTIME && (bot.Parent.CurrentState & Restrictions.AllowedState) == 0)
                || ((!Restrictions.CanDeveloperBypass || !ICommandBase.DeveloperIDs.Contains(userid)) && !CommandCooldownAvailablity(bot, eventArgs)))
            {
                return;
            }

            string message = chat.Snippet.DisplayMessage;

            LastGlobalCall = DateTime.Now;

            if (string.IsNullOrWhiteSpace(message))
            {
                Command(bot, this, eventArgs, Array.Empty<string>());
            }
            else
            {
                string[] args = message.Split(' ').Select(arg => arg.Trim()).ToArray();

                Command(bot, this, eventArgs, args);
            }
        }
    }
}
