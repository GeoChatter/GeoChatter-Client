using GeoChatter.Core.Attributes;
using GeoChatter.Core.Interfaces;
using GeoChatter.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GeoChatter.Integrations
{
    /// <summary>
    /// Twitch command for the bot
    /// </summary>
    public class StreamerBotCommand : ICommand<StreamerbotClient>
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
        public ICommand<StreamerbotClient>.CommandDef Command { get; set; }

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
        public bool CanBeTriggeredWith(StreamerbotClient bot, object eventArgs)
        {
            if (bot == null || !bot.GetEventArgObject(eventArgs, out TwitchLibMessage message, out Type type))
            {
                return false;
            }

            string msg = ICommandBase.GetCommandFromMessage(message.Message);

            if (!msg.StartsWith(TriggerChar))
            {
                return false;
            }
            msg = msg.TrimStart(TriggerChar);

            return CommandNames.FirstOrDefault(c => c == msg) != null;
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
        /// Call command with given arguments
        /// </summary>
        /// <param name="bot">Parent bot</param>
        /// <param name="eventArgs">Event arguments object</param>
        public void CallCommand(StreamerbotClient bot, object eventArgs)
        {
            if (bot == null
                || !bot.GetUserInfo(eventArgs, out string userid, out string _, out int userlevel)
                || !bot.GetEventArgObject(eventArgs, out TwitchLibMessage msg, out Type _)
                || (Restrictions.AllowedState != AppGameState.ANYTIME && (bot.Parent.CurrentState & Restrictions.AllowedState) == 0))
            {
                return;
            }

            int minlevel = Restrictions.AccessLevel;

            bool isModOrAbove = userlevel >= (int)CommonUserLevel.Moderator;

            bool hasRequiredLevel = userlevel >= minlevel;
            bool isDevAndCanBypass = Restrictions.CanDeveloperBypass && ICommandBase.DeveloperIDs.Contains(userid);
            bool isAvailable = CommandCooldownAvailablity(bot, eventArgs);
            if (((!hasRequiredLevel
                && !isModOrAbove) || !isAvailable) && !isDevAndCanBypass)
            {
                return;
            }


            LastGlobalCall = DateTime.Now;

            string message = msg.Message;

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
