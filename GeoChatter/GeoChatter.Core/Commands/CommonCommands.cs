using GeoChatter.Core.Attributes;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Interfaces;
using GeoChatter.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Core
{ /// <summary>
  /// Class for common command definitions
  /// </summary>
    public static class CommonCommands
    {
        private static Dictionary<Type, List<ICommandBase>> _commands = new();

        /// <summary>
        /// All commands with <see cref="CommandAttribute"/> and <see cref="CommandRestrictionAttribute"/> will be registered to this instance
        /// </summary>
        public static List<TCommand> Command<TBot, TCommand, TMessageModel>()
            where TCommand : class, ICommand<TBot>, new()
            where TBot : class, IBot<TCommand, TMessageModel>, new()
        {
            if (!_commands.ContainsKey(typeof(TBot)))
            {
                Initialize<TBot, TCommand, TMessageModel>();
            }
            return _commands[typeof(TBot)]?.Where(c => c.IsEnabled).Select(c => (TCommand)c).ToList();
        }

        /// <summary>
        /// Initialize commands from given bot, command and message models
        /// </summary>
        /// <typeparam name="TBot">Bot type</typeparam>
        /// <typeparam name="TCommand">Command type</typeparam>
        /// <typeparam name="TMessageModel">Message type for the bot</typeparam>
        public static void Initialize<TBot, TCommand, TMessageModel>()
            where TCommand : class, ICommand<TBot>, new()
            where TBot : class, IBot<TCommand, TMessageModel>, new()
        {
            _commands.Add(typeof(TBot), ICommand<TBot>.DiscoverMethods<TCommand>(typeof(CommonCommands)).Select(c => (ICommandBase)c).ToList());
        }

        private static List<string> SelfReferenceArguments { get; } = new()
        {
            "me",
            "self"
        };

        private static bool GetTargetUserNameFromArgs(string callername, ref string[] args, out string targetUser, out bool isSelfReference)
        {
            targetUser = string.Empty;
            isSelfReference = false;
            switch (args.Length)
            {
                case 2: // !flagof target
                    if (SelfReferenceArguments.Contains(args[1]))
                    {
                        args[1] = callername;
                        isSelfReference = true;
                    }
                    targetUser = args[1];
                    break;

                case 1: // !flagof
                    targetUser = callername;
                    isSelfReference = true;
                    break;

                default:
                    return false;
            }
            return true;
        }

        #region COMMANDS

#if DEBUG
        [Command("randomBotGuess", Description = "Create a random bot guess")]
        [CommandRestriction(AccessLevel = (int)CommonUserLevel.Moderator)]
        private static void RandomBotGuess(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            int count = 1;
            bool reuse = false;
            switch (args.Length)
            {
                case 3:
                    count = args[1]?.ParseAsInt(1) ?? 1;
                    reuse = bool.TryParse(args[2], out bool shallReuse) && shallReuse;
                    break;
                case 2:
                    count = args[1]?.ParseAsInt(1) ?? 1;
                    break;

                case 1:
                    break;

                default:
                    return;
            }

            bot.FireRandomBotGuessRecieved(new(userid, username, userPlatform, bot, command) { Count = count, Reuse = reuse });
        }
#endif

        [Command("flagof", Description = "Get the flag of given player")]
        [CommandRestriction(commandCooldown: 3, applyTo: CooldownTarget.Individual)]
        private static void GetFlagOf(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform)
                || !GetTargetUserNameFromArgs(username, ref args, out string targetUser, out bool isSelfReference))
            {
                return;
            }

            bot.FireFlagOfTargetReceived(new(userid, username, targetUser, isSelfReference, userPlatform, bot, command));
        }

        [Command("flag", "f", Description = "Set flag to given flag")]
        [CommandRestriction(commandCooldown: 3, applyTo: CooldownTarget.Individual)]
        private static void ChangeFlag(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            string flagName = string.Empty;

            switch (args.Length)
            {
                case 2: // !f CODE
                    flagName = args[1];
                    break;

                case 1:
                    return;

                default:
                    for (int i = 1; i < args.Length; i++)
                    {
                        flagName += args[i] + " ";
                    }
                    break;
            }
            // Fire custom event
            bot.FireFlagRequestReceived(new(userid, username, bot, command, userPlatform) { Flag = flagName });
        }

        [Command("colorof", "colourof", Description = "Get the color of given player's name")]
        [CommandRestriction(commandCooldown: 3, applyTo: CooldownTarget.Individual)]
        private static void GetColorOf(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform)
                || !GetTargetUserNameFromArgs(username, ref args, out string targetUser, out bool isSelfReference))
            {
                return;
            }

            bot.FireColorOfTargetReceived(new(userid, username, targetUser, isSelfReference, userPlatform, bot, command));
        }

        [Command("color", "colour", "c", Description = "Set color to given color name or value")]
        [CommandRestriction(commandCooldown: 3, applyTo: CooldownTarget.Individual)]
        private static void SetColor(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            string color = string.Empty;

            switch (args.Length)
            {
                case 2: // !c name
                    color = args[1];
                    break;

                case 1:
                    return;

                default:
                    for (int i = 1; i < args.Length; i++)
                    {
                        color += args[i] + " ";
                    }
                    break;
            }
            // Fire custom event
            bot.FireColorRequestReceived(new(userid, username, bot, command, userPlatform) { Color = color });
        }

        [Command("me", "m", Description = "Get personal statistics")]
        [CommandRestriction(commandCooldown: 15, applyTo: CooldownTarget.Individual)]
        private static void RequestMeStats(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            // Fire custom event
            bot.FireMeRequestReceived(new(userid, username, bot, command, userPlatform));
        }

        [Command("gc", "geo", "GC", "Gc", "gC", Description = "Get guessing map link")]
        [CommandRestriction(commandCooldown: 10, applyTo: CooldownTarget.Global)]
        private static void RequestMapLink(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            // Fire custom event
            bot.FireLinkRequestReceived(new(userid, username, bot, command, userPlatform));
        }

        [Command("best", "b", Description = "Get best channel statistics")]
        [CommandRestriction(commandCooldown: 15, applyTo: CooldownTarget.Global)]
        private static void RequestBestStats(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }
         
            // Fire custom event
            bot.FireBestRequestReceived(new(userid, username, bot, command, userPlatform));
        }

        [Command("bestof", Description = "Get best statistics of a given player")]
        [CommandRestriction(commandCooldown: 5, applyTo: CooldownTarget.Individual)]
        private static void RequestBestOfStats(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform)
                || !GetTargetUserNameFromArgs(username, ref args, out string targetUser, out bool isSelfReference))
            {
                return;
            }

            bot.FireBestOfRequestReceived(new(userid, username, targetUser, isSelfReference, userPlatform, bot, command));
        }

        [Command("map", Description = "Get information about the map")]
        [CommandRestriction(commandCooldown: 15, applyTo: CooldownTarget.Global)]
        private static void RequestMapExplanation(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            // Fire custom event
            bot.FireMapRequestReceived(new(userid, username, bot, command, userPlatform));
        }


        [Command("colors", "colours", "gc_colors", "gccolors", Description = "Get a link of an image showing available named colors")]
        [CommandRestriction(commandCooldown: 8, applyTo: CooldownTarget.Global)]
        private static void RequestColors(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            // Fire custom event
            bot.FireColorsRequestReceived(new(userid, username, bot, command, userPlatform));
        }

        [Command("flags", "gc_flags", "gcflags", Description = "Get a link for checking out available flag names and codes")]
        [CommandRestriction(commandCooldown: 8, applyTo: CooldownTarget.Global)]
        private static void RequestFlags(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            // Fire custom event
            bot.FireFlagsRequestReceived(new(userid, username, bot, command, userPlatform));
        }
        [Command("packs", Description = "Get a list of installed flag packs")]
        [CommandRestriction(commandCooldown: 8, applyTo: CooldownTarget.Global)]
        private static void RequestFlagpacks(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            // Fire custom event
            bot.FireFlagpacksRequestReceived(new(userid, username, bot, command, userPlatform));
        }

        [Command("flagpacks", "gc_flagpacks", "gcflagpacks", Description = "Get a link for checking out available flag names and codes in curated flag packs")]
        [CommandRestriction(commandCooldown: 8, applyTo: CooldownTarget.Global)]
        private static void RequestFlagpacksLink(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            // Fire custom event
            bot.FireFlagpacksLinkRequestReceived(new(userid, username, bot, command, userPlatform));
        }

        [Command("commands", "gc_commands", "gccommands", Description = "Get a link for checking out available flag names and codes")]
        [CommandRestriction(commandCooldown: 8, applyTo: CooldownTarget.Global)]
        private static void RequestCommands(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            // Fire custom event
            bot.FireCommandsRequestReceived(new(userid, username, bot, command, userPlatform));
        }

        [Command("reset", Description = "Reset your stats")]
        [CommandRestriction(commandCooldown: 60, applyTo: CooldownTarget.Individual)]
        private static void ResetStats(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            // Get event arguments object
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }

            bot.FireResetStatsRecieved(new(userid, username, bot, command, userPlatform));
        }

        [Command("version", Description = "Send GC version to chat")]
        [CommandRestriction(commandCooldown: 60, applyTo: CooldownTarget.Global, AccessLevel = (int)CommonUserLevel.Moderator, CanDeveloperBypass = true)]
        private static void VersionRequest(IBotBase bot, ICommandBase command, object eventArgs, string[] args)
        {
            if (!bot.GetUserInfo(eventArgs, out string userid, out string username, out _, out Platforms userPlatform))
            {
                return;
            }


            bot.FireVersionRequestRecieved(new(userid, username, bot, command, userPlatform));
        }

        #endregion
    }

    /// <summary>
    /// Common user level names across platforms for command restriction use
    /// </summary>
    public enum CommonUserLevel
    {
        /// <summary>
        /// Regular viewer with no special previliges
        /// </summary>
        Viewer = 0,//TwitchLib.Client.Enums.UserType.Viewer,
        /// <summary>
        /// Moderator with moderation privileges
        /// </summary>
        Moderator = 1,// TwitchLib.Client.Enums.UserType.Moderator,
        /// <summary>
        /// Broadcaster with all privileges
        /// </summary>
        Broadcaster = 3//TwitchLib.Client.Enums.UserType.Broadcaster,
    }

}
