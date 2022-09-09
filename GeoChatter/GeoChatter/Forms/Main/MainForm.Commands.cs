using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Core.Storage;
using GeoChatter.Extensions;
using GeoChatter.Model;
using GeoChatter.Model.Attributes;
using GeoChatter.Model.Enums;
using GeoChatter.Properties;
using GeoChatter.Web;
using GeoChatter.Web.Twitch;
using GeoChatter.Web.YouTube;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GeoChatter.Forms
{
    public partial class MainForm
    {

        public void ProcessViewerFlag(string playerid, string flag, string userName = "", Platforms userPlatform = Platforms.Twitch, string displayName = "", string profilePicUrl = "")
        {
            FlagRequestReceivedEventArgs args = new(playerid, string.Empty, CurrentBot, null, userPlatform, displayName, profilePicUrl)
            {
                Flag = flag,
                UserId = playerid,
                Bot = CurrentBot
            };
            FlagRequestReceived(null, args);
        }

        public void ProcessViewerColor(string playerid, string color, string userName = "", Platforms userPlatform = Platforms.Twitch, string displayName = "", string profilePicUrl = "")
        {
            ColorRequestReceivedEventArgs args = new(playerid, string.Empty, CurrentBot, null, userPlatform, displayName, profilePicUrl)
            {
                Color = color
            };
            ColorRequestReceived(null, args);
        }
        #region Command Handler Methods

        /// <summary>
        /// Handles a viewers flag request
        /// </summary>
        [DiscoverableEvent]
        private void ColorRequestReceived(object sender, ColorRequestReceivedEventArgs args)
        {
            try
            {
                Player player = ClientDbCache.RunningGame?.Players?.FirstOrDefault(p => p.PlatformId == args.UserId && p.SourcePlatform == args.UserPlatform);
                if (player == null)
                {
                    player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, args.DisplayName, args.ProfilePicUrl, GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
                }

                if (player == null || player.IsBanned)
                {
                    return;
                }

                string color_arg = args.Color.Trim();
                string color = color_arg.ToLowerInvariant();

                if (color == "remove")
                {
                    player.Color = args.Color;
                    if (Settings.Default.SendColorSelected && Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_ColorRemovedMessage", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }));
                }
                else if (color == "random")
                {
                    player.Color = args.Color;
                    if (Settings.Default.SendColorSelected && Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_ColorRandomMessage", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }));
                }
                else
                {
                    logger.Debug("ColorRequest received " + player.FullDisplayName + " selected color " + color_arg);
                    string cssColor = ColorHelper.ParseColor(color, out string colorRealName, out Color _);
                    logger.Debug($"ColorRequest parsed {player.FullDisplayName} : {color_arg} -> {colorRealName} ({cssColor})");

                    if (string.IsNullOrEmpty(cssColor))
                    {
                        if (Settings.Default.SendColorSelected && Settings.Default.EnableTwitchChatMsgs)
                            args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_ColorNotFoundMessage", new Dictionary<string, string>() { { "playerName", player.FullDisplayName }, { "color", colorRealName } }));
                    }
                    else
                    {
                        player.Color = cssColor;
                        if (Settings.Default.SendColorSelected && Settings.Default.EnableTwitchChatMsgs)
                            args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_ColorAssignedMessage", new Dictionary<string, string>() { { "playerName", player.FullDisplayName }, { "color", colorRealName } }));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        /// <summary>
        /// Handles a viewers flag request
        /// </summary>
        [DiscoverableEvent]
        private void FlagRequestReceived(object sender, FlagRequestReceivedEventArgs args)
        {
            try
            {
                Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);

                if (player == null || player.IsBanned)
                {
                    return;
                }

                string flag = args.Flag.Trim();
                if (flag.ToLowerInvariant() == "remove")
                {
                    player.PlayerFlag = "";
                    player.PlayerFlagName = "";
                    if (Settings.Default.SendFlagSelected && Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_FlagRemovedMessage", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }));
                }
                else if (CountryHelper.CheckFlagCode(ref flag, out string cname))
                {
                    logger.Debug("FlagRequest received " + player.FullDisplayName + " selected flag " + flag);
                    player.PlayerFlag = flag.ToLowerInvariant();

                    Country _ = CountryHelper.GetCountryInformation(flag, null, null, Settings.Default.UseEnglishCountryNames, out Country result);

                    if (string.IsNullOrWhiteSpace(cname))
                    {
                        cname = result.Name == Country.UnknownCountryName ? player.PlayerFlag : result.Name;
                    }
                    player.PlayerFlagName = cname;
                    if (Settings.Default.SendFlagSelected && Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_FlagAssignedMessage", new Dictionary<string, string>() { { "playerName", player.FullDisplayName }, { "name", cname }, { "flag", flag } }));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }
        
        [DiscoverableEvent]
        private void ColorOfTargetReceived(object sender, TargetBotEventArgs args)
        {
            try
            {
                logger.Debug("ColorOfTarget received");
                Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
                if (player == null)
                {
                    return;
                }

                if (player.IsBanned)
                {
                    if (Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_BannedStatsRequest", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }));
                    return;
                }

                Player target = args.IsSelfReference
                    ? player
                    : ClientDbCache.Instance.GetByName(args.Target);

                if (target == null)
                {

                    if (Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_NoRecordsFound", new Dictionary<string, string>() { { "playerName", args.Username }, { "targetName", args.Target } }));
                    return;
                }
                else if (string.IsNullOrWhiteSpace(target.Color))
                {
                    if (args.IsSelfReference)
                    {

                        if (Settings.Default.EnableTwitchChatMsgs)
                            args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_NoColor", new Dictionary<string, string>() { { "playerName", args.Username } }));
                        return;
                    }

                    if (Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_NoTargetColor", new Dictionary<string, string>() { { "playerName", args.Username }, { "targetName", args.Target } }));
                    return;
                }

                string color = ColorHelper.GetHexColorFromCSS(target.Color);
                if (args.IsSelfReference)
                {

                    if (Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_CurrentColor", new Dictionary<string, string>() { { "playerName", args.Username }, { "color", color } }));
                    return;
                }

                if (Settings.Default.EnableTwitchChatMsgs)
                    args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_CurrentTargetColor", new Dictionary<string, string>() { { "playerName", args.Username }, { "targetName", args.Target }, { "color", color } }));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        [DiscoverableEvent]
        private void FlagOfTargetReceived(object sender, TargetBotEventArgs args)
        {
            try
            {
                logger.Debug("FlagOfTarget received");
                Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
                if (player == null || player.IsBanned)
                {
                    return;
                }

                Player target = args.IsSelfReference
                    ? player
                    : ClientDbCache.Instance.GetByName(args.Target);

                if (target == null)
                {

                    if (Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_NoRecordsFound", new Dictionary<string, string>() { { "playerName", args.Username }, { "targetName", args.Target } }));
                    return;
                }
                else if (string.IsNullOrWhiteSpace(target.PlayerFlagName))
                {
                    if (args.IsSelfReference)
                    {

                        if (Settings.Default.EnableTwitchChatMsgs)
                            args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_NoFlag", new Dictionary<string, string>() { { "playerName", args.Username } }));
                        return;
                    }
                    if (Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_NoTargetFlag", new Dictionary<string, string>() { { "playerName", args.Username }, { "targetName", args.Target } }));
                    return;
                }

                if (args.IsSelfReference)
                {

                    if (Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_CurrentFlag", new Dictionary<string, string>() { { "playerName", args.Username }, { "flagName", target.PlayerFlagName }, { "flagCode", target.PlayerFlag.ToUpperInvariant() } }));
                    return;
                }
                if (Settings.Default.EnableTwitchChatMsgs)
                    args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_CurrentFlagTarget", new Dictionary<string, string>() { { "playerName", args.Username }, { "targetName", args.Target }, { "flagName", target.PlayerFlagName }, { "flagCode", target.PlayerFlag.ToUpperInvariant() } }));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        [DiscoverableEvent]
        private void RandomGuessRecieved(object sender, RandomGuessRecievedEventArgs args)
        {
            try
            {
                logger.Debug("RandomGuess received");
                Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
                if (player == null)
                {
                    return;
                }

                if (player.IsBanned)
                {
                    return;
                }

                Coordinates rand = BorderHelper.GetRandomPointCloseOrWithinAPolygon();

                GuessReceivedEventArgs g = new(args.UserId, args.Username, args.Bot, args.Command)
                {
                    Lat = rand.Latitude.ToStringDefault(),
                    Lng = rand.Longitude.ToStringDefault(),
                    Color = args.Color,
                    WasRandom = true
                };

                CurrentBot?.FireGuessReceived(g);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        [DiscoverableEvent]
        private void ResetStatsRecieved(object sender, BotEventArgs args)
        {
            try
            {
                logger.Debug("ResetStats received");
                Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
                if (player == null || player.IsBanned)
                {
                    return;
                }
                player.ResetStats();
                if (Settings.Default.EnableTwitchChatMsgs)
                    args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_StatsReset", new Dictionary<string, string>() { { "playerName", args.Username } }));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        [DiscoverableEvent]
        private void LinkRequestReceived(object sender, BotEventArgs args)
        {
            try
            {
                logger.Debug("LinkRequest received");
                Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
                if (player == null || player.IsBanned)
                {
                    return;
                }

                if (Settings.Default.EnableTwitchChatMsgs)
                {
                    string message = LanguageStrings.Get("Chat_Msg_linkMessage");
#if DEBUG
                    message = message.Replace("/map/", "/testing_map/", StringComparison.InvariantCulture);
#endif
                    args.Bot.SendMessage(message);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        [DiscoverableEvent]
        private void MapRequestReceived(object sender, BotEventArgs args)
        {
            try
            {
                logger.Debug("MapRequest received");
                Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
                if (player == null || player.IsBanned)
                {
                    return;
                }

                if (Settings.Default.EnableTwitchChatMsgs)
                    args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_MapExplanationMessage", new Dictionary<string, string>() { { "playerName", args.Username } }));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        [DiscoverableEvent]
        private void BestOfRequestReceived(object sender, TargetBotEventArgs args)
        {
            try
            {
                logger.Debug("BestOfRequest received");
                Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
                if (player == null)
                {
                    return;
                }

                if (player.IsBanned)
                {
                    if (Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_BannedStatsRequest", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }));
                    return;
                }
                else
                {
                    Player target = args.IsSelfReference
                        ? player
                        : ClientDbCache.Instance.GetByName(args.Target);

                    if (target == null)
                    {
                        return;
                    }

                    if (target.IsBanned)
                    {
                        if (Settings.Default.EnableTwitchChatMsgs)
                            args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_BannedStatsRequest", new Dictionary<string, string>() { { "playerName", target.FullDisplayName } }));
                        return;
                    }
                    if (Settings.Default.EnableTwitchChatMsgs)
                    {
                        string msg = target.GetStatsMessage((Units)Settings.Default.OverlayUnit);
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_Stats", new Dictionary<string, string>() { { "targetName", target.FullDisplayName }, { "msg", msg } }));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        [DiscoverableEvent]
        private void MeRequestReceived(object sender, BotEventArgs args)
        {
            try
            {
                logger.Debug("MeRequest received");
                Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
                if (player == null)
                {
                    return;
                }

                if (player.IsBanned)
                {
                    if (Settings.Default.EnableTwitchChatMsgs)
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_BannedStatsRequest", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }));
                    return;
                }
                else
                {

                    if (Settings.Default.EnableTwitchChatMsgs)
                    {
                        string msg = player.GetStatsMessage((Units)Settings.Default.OverlayUnit);
                        args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_Stats", new Dictionary<string, string>() { { "targetName", args.Username }, { "msg", msg } }));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }


        [DiscoverableEvent]
        private void BestRequestReceived(object sender, BotEventArgs args)
        {
            try
            {
                logger.Debug("BestRequest received");


                Player avgPlayer = ClientDbCache.Instance.GetBestAverage();
                Player streakPlayer = ClientDbCache.Instance.GetBestStreak();
                // Player roundPlayer = ClientDbCache.GetBestRound();
                Player winPlayer = ClientDbCache.Instance.GetBestWins();
                Player perfectPlayer = ClientDbCache.Instance.GetBestPerfects();
                Player no5kPlayer = ClientDbCache.Instance.GetBestNoOf5ks();


                logger.Debug("Calculated highscores");
                string msg = $"@Highscores  (15sec cooldown):" +
                    ((avgPlayer.SumOfGuesses > 0 && avgPlayer.NoOfGuesses > 1) ? $" Average score: {avgPlayer.OverallAverage.ToStringDefault("F4")} ({avgPlayer.FullDisplayName}) | " : string.Empty) +
                    ((streakPlayer.BestStreak > 0) ? $"Streak : {streakPlayer.BestStreak} ({streakPlayer.FullDisplayName}) | " : string.Empty) +
                    //      ((roundPlayer.BestRound > 0) ? $"Best round: {roundPlayer.BestRound} ({roundPlayer.DisplayName}) | " : string.Empty) +
                    ((no5kPlayer.NoOf5kGuesses > 0) ? $"Perfect rounds: {no5kPlayer.NoOf5kGuesses} ({no5kPlayer.FullDisplayName}) " : string.Empty) +
                    ((winPlayer.Wins > 0) ? $"| Wins: {winPlayer.Wins} ({winPlayer.FullDisplayName}) " : string.Empty) +
                  ((perfectPlayer.Perfects > 0) ? $"| Perfect games: {perfectPlayer.Perfects} ({perfectPlayer.FullDisplayName})" : string.Empty);

                msg = msg.TrimEnd();
                msg = msg.TrimEnd('|');
                msg = msg.TrimEnd();
                if (Settings.Default.EnableTwitchChatMsgs)
                    args.Bot.SendMessage(msg);
                logger.Debug("BestMessage sent");

            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }

        }

        [DiscoverableEvent]
        private void BannedUserReceived(object sender, BannedUserReceivedEventArgs args)
        {
            if (Settings.Default.AutoBanUsers)
            {
                Player bannedPlayer = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: Platforms.Twitch);
                bannedPlayer.IsBanned = true;
            }
        }

        [DiscoverableEvent]
        private void FlagsRequestReceived(object sender, BotEventArgs args)
        {
            Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
            if (player == null || player.IsBanned)
            {
                return;
            }

            if (Settings.Default.EnableTwitchChatMsgs)

                args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_AvailableFlags", new Dictionary<string, string>() { { "flagUrl", Settings.Default.FlagsPageURL } }));

        }

        [DiscoverableEvent]
        private void FlagpacksRequestReceived(object sender, BotEventArgs args)
        {


            Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
            if (player == null || player.IsBanned)
            {
                return;
            }
            IEnumerable<string> packs = FlagPackHelper.FlagPacks.Select(p => p.Name);
            if (Settings.Default.EnableTwitchChatMsgs)
                args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_FlagPacks", new Dictionary<string, string>() { { "packs", string.Join(", ", packs) } }));

        }

        [DiscoverableEvent]
        private void FlagpacksLinkRequestReceived(object sender, BotEventArgs args)
        {

            Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
            if (player == null || player.IsBanned)
            {
                return;
            }
            if (Settings.Default.EnableTwitchChatMsgs)
                args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_AvailableFlagPacks", new Dictionary<string, string>() { { "flagUrl", Settings.Default.FlagpacksPageURL } }));

        }

        [DiscoverableEvent]
        private void CommandsRequestReceived(object sender, BotEventArgs args)
        {
            Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
            if (player == null || player.IsBanned)
            {
                return;
            }
            if (Settings.Default.EnableTwitchChatMsgs)
                args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_Commands", new Dictionary<string, string>() { { "commandUrl", Settings.Default.CommandsPageURL } }));

        }

        [DiscoverableEvent]
        private void VersionRequestRecieved(object sender, BotEventArgs args)
        {
            Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
            if (player == null || player.IsBanned)
            {
                return;
            }
            if (Settings.Default.EnableTwitchChatMsgs)
                args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_CurrentVersion", new Dictionary<string, string>() { { "version", Version } }));

        }

        [DiscoverableEvent]
        private void ColorsRequestReceived(object sender, BotEventArgs args)
        {
            Player player = ClientDbCache.Instance.GetPlayerByIDOrName(args.UserId, args.Username, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: args.UserPlatform);
            if (player == null || player.IsBanned)
            {
                return;
            }
            if (Settings.Default.EnableTwitchChatMsgs)
                args.Bot.SendMessage(LanguageStrings.Get("Chat_Msg_Colors", new Dictionary<string, string>() { }));

        }
        /// <summary>
        /// Handles a viewers guess
        /// </summary>
        [DiscoverableEvent]
        private void GuessReceived(object sender, GuessReceivedEventArgs args)
        {
            string userId = args.UserId;
            string userName = args.Username;
            string latString = args.Lat;
            string lngString = args.Lng;
            string color = args.Color;
            ProcessViewerGuess(userId, userName, args.Bot is TwitchBot ? Platforms.Twitch : Platforms.YouTube, latString, lngString, color, wasRandom: args.WasRandom);
        }

        #endregion

    }
}
