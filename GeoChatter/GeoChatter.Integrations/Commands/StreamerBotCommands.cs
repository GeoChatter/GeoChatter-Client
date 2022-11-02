using GeoChatter.Core.Attributes;
using GeoChatter.Core.Interfaces;
using GeoChatter.Integrations;
using GeoChatter.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoChatter.Web.Twitch
{
    /// <summary>
    /// Class for twitch command definitions
    /// </summary>
    public static class TwitchCommands
    {
        private static List<StreamerBotCommand> _commands;

        /// <summary>
        /// All commands with <see cref="CommandAttribute"/> and <see cref="CommandRestrictionAttribute"/> will be registered to this instance
        /// </summary>
        public static List<StreamerBotCommand> Commands => _commands?.Where(c => c.IsEnabled).ToList() ?? (_commands = new List<TwitchCommand>());

        /// <summary>
        /// Initialize <see cref="Commands"/> list
        /// </summary>
        public static void Initialize()
        {
            _commands?.Clear();
            _commands = ICommand<StreamerbotClient>.DiscoverMethods<StreamerBotCommand>(typeof(StreamerBotCommand));
            _commands.AddRange(CommonCommands.Command<StreamerbotClient, StreamerBotCommand, TwitchLibMessage>());
        }

        #region COMMANDS

        [Command("random_guess", "random_plonk", "randomplonk", "randomguess", "r", Description = "Send a random guess")]
        [CommandRestriction(commandCooldown: 5, applyTo: CooldownTarget.Individual, AllowedState = AppGameState.INROUND)]
        private static void RandomGuess(TwitchBot bot, ICommand<TwitchBot> command, object eventArgs, string[] args)
        {
            // Get event arguments object
            if (!bot.GetEventArgObject(eventArgs, out TwitchLibMessage message, out Type _))
            {
                return; // Fired from unknown event
            }

            string username = message.Username;
            string userid = message.UserId;
            string color = message.ColorHex;

            bot.FireRandomGuessRecieved(new(userid, username, bot, command) { Arguments = string.Join(' ', args), Color = color });
        }


        #endregion
    }
}
