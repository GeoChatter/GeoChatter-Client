using GeoChatter.Core;
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
    /// Class for twitch command definitions
    /// </summary>
    public static class YoutubeCommands
    {
        private static List<YoutubeCommand> _commands;

        /// <summary>
        /// All commands with <see cref="CommandAttribute"/> and <see cref="CommandRestrictionAttribute"/> will be registered to this instance
        /// </summary>
        public static List<YoutubeCommand> Commands => _commands?.Where(c => c.IsEnabled).ToList() ?? (_commands = new List<YoutubeCommand>());

        /// <summary>
        /// Initialize <see cref="Commands"/> list
        /// </summary>
        public static void Initialize()
        {
            _commands?.Clear();
            _commands = ICommand<YoutubeBot>.DiscoverMethods<YoutubeCommand>(typeof(YoutubeCommands));
            _commands.AddRange(CommonCommands.Command<YoutubeBot, YoutubeCommand, LiveChatMessage>());
        }

        #region COMMANDS

        [Command("random_guess", "random_plonk", "randomplonk", "randomguess", Description = "Send a random guess")]
        [CommandRestriction(commandCooldown: 5, applyTo: CooldownTarget.Individual, AllowedState = AppGameState.INROUND)]
        private static void RandomGuess(YoutubeBot bot, ICommand<YoutubeBot> command, object eventArgs, string[] args)
        {
            if (!bot.GetEventArgObject(eventArgs, out LiveChatMessage chat, out Type _))
            {
                return;
            }

            string img = chat.AuthorDetails.ProfileImageUrl;
            string username = chat.AuthorDetails.DisplayName;
            string userid = chat.AuthorDetails.ChannelId;

            bot.FireRandomGuessRecieved(new(userid, username, Platforms.YouTube, bot, command) { Arguments = string.Join(' ', args), ProfilePicture = img });
        }

        #endregion
    }
}
