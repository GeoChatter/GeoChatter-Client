using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Storage;
using GeoChatter.Model.Enums;
using GeoChatter.Model;
using GeoChatter.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using GeoChatter.Extensions;
using System.Linq;
using GeoChatter.Core.Interfaces;
using GeoChatter.Helpers;
using GeoChatter.Web.Twitch;
using GeoChatter.Web.YouTube;
using System.Windows.Forms;
using Antlr4.Runtime.Misc;
using GeoChatter.Integrations.StreamerBot;

namespace GeoChatter.Forms
{
    public partial class MainForm
    {

        private static void InitializeBotCommands()
        {
            TwitchCommands.Initialize();
            YoutubeCommands.Initialize();
            StreamerBotCommands.Initialize();
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ConnectBot()
        {
            // TODO: Choose between connections ( Twitch and Youtube etc )
            ConnectTwitchBot();
            // FUCK YOUTUBE
            //  ConnectYoutubeBot();
        }

        public void DisconnectTwitchBot()
        {
            if (CurrentBot is TwitchBot tb)
            {
                tb.Disconnect();
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ConnectTwitchBot()
        {
            logger.Debug("ConnectTwitchBot");
            string botname = Settings.Default.TwitchChannel.ToLowerInvariant();
            string channel = Settings.Default.TwitchChannel.ToLowerInvariant();
            string oathtkn = Settings.Default.oauthToken;
            try
            {


                if (!string.IsNullOrEmpty(botname) && !string.IsNullOrEmpty(channel) && !string.IsNullOrEmpty(oathtkn))
                {
                    logger.Debug("ConnectTwitchBot initializing");
                    if (CurrentBot is TwitchBot tb)
                    {
                        tb.Disconnect();
                    }
                    CurrentBot = new TwitchBot(this, botname, channel, oathtkn, Settings.Default.SendJoinMsg);

                    AttributeDiscovery.AddEventHandlers(fromMethodSource: this, toTargetInstance: CurrentBot);
                    TwitchHelper.TwitchAccessToken();
                }
                else
                {
                    MessageBox.Show("Couldn't connect Twitch bot. Please check your settings.", "Failed To Connect", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                MessageBox.Show(ex.Message, "Failed To Connect");
            }
        }
        private void SendStartGameMessage(Game game)
        {
            if (Settings.Default.SendGameStartMsg && (!game.IsPartOfInfiniteGame || game.Previous == null))
            {
                if (Settings.Default.EnableTwitchChatMsgs || Settings.Default.SendChatMsgsViaStreamerBot)
                {
                    CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_gameStart"));
                }
            }
        }
        private void SendRoundEndMessage(Game game)
        {
                Round round = game.Rounds.FirstOrDefault(r => r.RoundNumber == game.Rounds.Count);
                SendEndRoundMessage(round);
        }
        /// <summary>
        /// Sends the start round msg to chat
        /// </summary>
        /// <param name="round"></param>
        private void SendEndRoundMessage(Round round)
        {
            if (Settings.Default.SendRoundEndMsg)
            {
                //logger.Info(round.Results);
                RoundResult result = round.Results
                    .BuildOrderBy(round.Game.GetTableOptions().GetDefaultFiltersFor(round.Game.Mode, GameStage.ENDROUND))
                    .FirstOrDefault();
                string playerName = ClientDbCache.Instance.Players.FirstOrDefault(p => p.PlatformId == result?.Player.PlatformId)?.DisplayName;
                if (ClientDbCache.RunningGame.Mode == GameMode.STREAK)
                {
                    int roundn = ClientDbCache.RunningGame.CurrentRound <= -1
                        ? ClientDbCache.RunningGame.Rounds.Count
                        : ClientDbCache.RunningGame.CurrentRound - 1;
                    ;
                   if(Settings.Default.EnableTwitchChatMsgs || Settings.Default.SendChatMsgsViaStreamerBot)
                        CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_EndStreakRound", new Dictionary<string, string>() { { "roundNumber", roundn.ToStringDefault() }, { "winnerName", (playerName ?? "<unknown>") } }));
                }
                else
                {
                    if (Settings.Default.EnableTwitchChatMsgs || Settings.Default.SendChatMsgsViaStreamerBot)
                        CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_roundEnd", new Dictionary<string, string>() { { "winner", playerName ?? "<unknown>" } }));
                }
            }
        }
        private static void CooledDownMessage(int cooldown, string message, string userId, IBotBase bot, Dictionary<string, DateTime> lookup)
        {
            bool send = false;
            bool firstTime = !lookup.ContainsKey(userId);

            if (firstTime)
            {
                lookup.Add(userId, DateTime.Now);
                send = true;
            }
            else if (lookup[userId].AddSeconds(cooldown) < DateTime.Now)
            {
                lookup[userId] = DateTime.Now;
                send = true;
            }

            if (send)
            {
                if (Settings.Default.EnableTwitchChatMsgs || Settings.Default.SendChatMsgsViaStreamerBot)
                {
                    bot?.SendMessage(message);
                }
            }
        }
        private void SendStartRoundMessage(Round round)
        {
            if (Settings.Default.SendRoundStartMsg)
            {
                string roundNumber = round.RealRoundNumber().ToStringDefault();
                string msg = LanguageStrings.Get("Chat_Msg_roundStart", new Dictionary<string, string>() { { "roundNumber", roundNumber } });
                if (Settings.Default.EnableTwitchChatMsgs || Settings.Default.SendChatMsgsViaStreamerBot)
                    CurrentBot?.SendMessage(msg);
            }
        }

    }
}
