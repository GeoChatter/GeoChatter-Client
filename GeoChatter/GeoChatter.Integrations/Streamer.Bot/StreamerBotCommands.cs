using GeoChatter.Core;
using GeoChatter.Core.Attributes;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Interfaces;
using GeoChatter.Integrations;
using GeoChatter.Integrations.Classes;
using GeoChatter.Model.Enums;

using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Websocket.Client;

namespace GeoChatter.Integrations.StreamerBot
{
    /// <summary>
    /// Class for twitch command definitions
    /// </summary>
    public static class StreamerBotCommands
    {
        private static List<StreamerBotCommand> _commands;

        /// <summary>
        /// All commands with <see cref="CommandAttribute"/> and <see cref="CommandRestrictionAttribute"/> will be registered to this instance
        /// </summary>
        public static List<StreamerBotCommand> Commands => _commands?.Where(c => c.IsEnabled).ToList() ?? (_commands = new List<StreamerBotCommand>());

        /// <summary>
        /// Initialize <see cref="Commands"/> list
        /// </summary>
        public static void Initialize()
        {
            _commands?.Clear();
            _commands = ICommand<StreamerbotClient>.DiscoverMethods<StreamerBotCommand>(typeof(StreamerBotCommands));
            _commands.AddRange(CommonCommands.Command<StreamerbotClient, StreamerBotCommand, StreamerBotCommandMessagePart>());
        }

        #region COMMANDS

        [Command("random_guess", "random_plonk", "randomplonk", "randomguess", "r", Description = "Send a random guess")]
        [CommandRestriction(commandCooldown: 5, applyTo: CooldownTarget.Individual, AllowedState = AppGameState.INROUND)]
        private static void RandomGuess(StreamerbotClient bot, ICommand<StreamerbotClient> command, object eventArgs, string[] args)
        {

            if (!bot.GetEventArgObject(eventArgs, out StreamerBotCommandMessagePart msg, out Type _))
            {
                return;
            }

            string username = msg.user.display_name;
            string userid = msg.user.id.ToStringDefault();
            Platforms userPlatform = Platforms.Unknown;
            switch (msg.user.type.ToLowerInvariant())
            {
                case "twitch":
                default:
                    userPlatform = Platforms.Twitch;
                    break;
                case "youtube":
                    userPlatform = Platforms.YouTube;
                    break;

            }
            ;

            bot.FireRandomGuessRecieved(new(userid, username, userPlatform, bot, command) { Arguments = string.Join(' ', args) });
        }


        #endregion
    }
}
