using GeoChatter.Core.Storage;
using GeoChatter.Model.Enums;
using GeoChatter.Model;
using GeoChatter.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeoChatter.Extensions;
using GeoChatter.Core.Model;
using System.Linq;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Properties;
using GeoChatter.Core.Helpers;
using GeoChatter.Helpers;
using GeoChatter.Core.Interfaces;

namespace GeoChatter.Forms
{
    public partial class MainForm
    {


        /// <inheritdoc/>
        public void SetupGame()
        {
            if (ClientDbCache.RunningGame == null ||
                ClientDbCache.RunningGame.CurrentRound == -1)
            {
                return;
            }
            else if (ClientDbCache.RunningGame.CurrentRound == -2)
            {
                logger.Warn("Attempted to resume finished infinite game");
                return;
            }

            logger.Info("SetupGame");
            Round firstRound = ClientDbCache.RunningGame.GetCurrentRound();
            if (!isRefresh)
                firstRound.TimeStamp = DateTime.Now;
            firstRound.Flags = ClientDbCache.Instance.NextRoundRoundOptions;
            ClientDbCache.Instance.NextRoundRoundOptions = RoundOption.DEFAULT;

            SetCountry(firstRound);
            SetRefreshMenuItemsEnabledState(true);
            SendStartRoundToMaps(firstRound);
            SendStartRoundToJS(firstRound);
            ToggleGuesses(true);
            TriggerRoundStartActions();
        }

        public async Task<GameFoundStatus> SetupStartGame(GeoGuessrGame geoGame, bool retrigger)
        {
            logger.Info($"SetupStartGame(retrigger={retrigger})");
            GameFoundStatus status = GameFoundStatus.NOTFOUND;
            if (!retrigger)
            {
                ClientDbCache.RunningGame = geoGame?.CreateFromGeoGuessrGame(ClientDbCache.Instance, labelSettings, GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), out status);

                if (ClientDbCache.RunningGame == null)
                {
                    switch (status)
                    {
                        case GameFoundStatus.NOTFOUND:
                            {
                                MessageBox.Show($"Game {geoGame?.token} is unavailable or doesn't exist.", "Game Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                return status;
                            }
                        case GameFoundStatus.FINISHED:
                            {
                                MessageBox.Show($"Game {geoGame?.token} was already completed and can not be continued.", "Game Already Finished", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                return status;
                            }
                    }
                }
            }

            if (LastBrowserID != browser.GetBrowser().Identifier)
            {
                if (ClientDbCache.RunningGame != null)
                {
                    bool sstatus = await ClientDbCache.SaveGame(ClientDbCache.RunningGame);
                    if (!sstatus)
                    {
                        logger.Error("Failed to save current game before refresh game event");
                    }
                    SendRefreshGameToJS(ClientDbCache.RunningGame);
                }
                LastBrowserID = browser.GetBrowser().Identifier;
            }

            if (ClientDbCache.RunningGame == null)
            {
                return status;
            }
            else if (ClientDbCache.RunningGame.CurrentRound > -1)
            {
                status = GameFoundStatus.FOUND;
                SetRefreshMenuItemsEnabledState(false);
                SendStartGameToJS(ClientDbCache.RunningGame);
                SendStartGameToMaps(ClientDbCache.RunningGame);
            }
            else if (ClientDbCache.RunningGame.CurrentRound == -2)
            {
                logger.Warn("Attempted to resume finished infinite game");
            }
            return status;
        }
        /// <inheritdoc/>
        public async Task<bool> ReTriggerStartGameEvent()
        {
            return browser.IsBrowserInitialized && await InitializeGamefromAddress(browser.Address, true);
        }

        private List<string> GameStartFires { get; } = new();

        /// <inheritdoc/>
        public void VerifyGameStarted(string id)
        {
            id = id.Trim();
            if (GameStartFires.Contains(id))
            {
                logger.Info("Verifying game start for " + id);
                GameStartFires.Remove(id);
                ToggleGuesses(true);
            }
            else
            {
                logger.Debug("Failed to verify game start(not in the list) for " + id + "\r\n\t" + string.Join(", ", GameStartFires));
            }
        }

        private Dictionary<string, int> GameStartFiresRetries { get; } = new();

        private async Task<bool> InitializeGamefromAddress(string address, bool retrigger = false)
        {
            logger.Debug($"InitializeGamefromAddress(retrigger={retrigger}): {address}, Last: {LastAddress}");
            GameID gameid = new() { ID = GeoGuessrClient.GetGameId(address), IsChallenge = GeoGuessrClient.IsChallengeGame(address) };
            bool samegame = ClientDbCache.RunningGame != null && ClientDbCache.RunningGame.GeoGuessrId == gameid.ID;

            if (GameStartFires.Contains(gameid.ID))
            {
                if (GameStartFiresRetries.ContainsKey(gameid.ID))
                {
                    GameStartFiresRetries[gameid.ID]++;
                }
                else
                {
                    GameStartFiresRetries.Add(gameid.ID, 1);
                }

                if (GameStartFiresRetries[gameid.ID] >= 30)
                {
                    logger.Warn("Giving up game start fire retries and forcing retrigger for: " + gameid.ID);
                    GameStartFiresRetries.Remove(gameid.ID);
                }
                else
                {
                    logger.Debug("Ignoring extra initializer firing for " + gameid.ID);
                    return false;
                }
            }

            if (!retrigger && samegame)
            {
                logger.Warn("Skipping game initialize for same address: " + address);
                return false;
            }

            GeoGuessrGame geoGame = samegame ? ClientDbCache.RunningGame.Source : GeoGuessrClient.GetGameData(gameid);

            int tries = 0;
            const int maxtries = 3;
            while (geoGame == null && tries++ < maxtries)
            {
                geoGame = GeoGuessrClient.GetGameData(gameid);
            }

            if (geoGame == null)
            {
                MessageBox.Show("Error", "Failed to get GeoGuessr game data. Try restarting the app and checking your internet connection.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return true;
            }
            else if (geoGame.rounds == null)
            {
                ClientDbCache.RunningGame = null;
                // TODO: Handle daily challenge better
            }
            else
            {
                GameFoundStatus status = await SetupStartGame(geoGame, retrigger);
                if (status != GameFoundStatus.FOUND)
                {
                    return true; // True to ignore
                }

                SetupGame();
                return ClientDbCache.RunningGame != null;
            }
            return true;
        }

        private void SaveAndExitPreviousGameIfDifferent(string newaddress)
        {
            logger.Debug($"SaveAndExitPreviousGameIfDifferent({LastAddress}, {NextChangeIsRefresh}): " + newaddress);
            if (ClientDbCache.RunningGame != null
                && (NextChangeIsRefresh || (LastAddress != newaddress && GeoGuessrClient.IsGameAddress(LastAddress))))
            {
                SaveAndExit();
            }
        }

        private async Task SaveAndExit(bool instantexit = false)
        {
            logger.Info("Exiting current game: " + instantexit);
            guessesOpen = false;
            try
            {
                bool status = await ClientDbCache.SaveGame(ClientDbCache.RunningGame);
                if (!status)
                {
                    logger.Error("Failed to save while exiting");
                }

                if (!instantexit)
                {
                    SetRefreshMenuItemsEnabledState(true);
                    SendExitGameToJS(ClientDbCache.RunningGame);
                    SendExitGameToMaps();
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
            finally
            {
                ClientDbCache.RunningGame = null;
            }
        } /// <inheritdoc/>
        public void OverwriteRoundData(int round, double lat, double lng, string panoid)
        {
            if (ClientDbCache.RunningGame != null && !string.IsNullOrEmpty(panoid) && ClientDbCache.RunningGame.Source.rounds.Count >= round)
            {
                GGRound r = ClientDbCache.RunningGame.Source.rounds[round - 1];
                r.lat = lat;
                r.lng = lng;
                r.panoId = panoid;

                Round ro = ClientDbCache.RunningGame.Rounds.FirstOrDefault(r => r.RoundNumber == round);
                if (ro == null)
                {
                    return;
                }

                ro.CorrectLocation.Latitude = lat;
                ro.CorrectLocation.Longitude = lng;
            }
        }
        /// <summary>
        /// Save to api
        /// </summary>
        public Task SaveGameToApi()
        {
            Game g = ClientDbCache.RunningGame;
            while (g.Previous != null)
            {
                g = g.Previous;
            }

            return guessApiClient.SaveGameToApi(g);
        }

        private static void FinalizeGame()
        {
            bool first = true;
            IEnumerable<GameResult> res = ClientDbCache.RunningGame.Results
                .BuildOrderBy(ClientDbCache.RunningGame.GetTableOptions().GetDefaultFiltersFor(ClientDbCache.RunningGame.Mode, GameStage.ENDGAME));

            int totalRoundCount = ClientDbCache.RunningGame.GetCurrentOrLastStartedRound().RealRoundNumber();

            foreach (GameResult item in res)
            {

                Player player = item.Player;
                if (player != null)
                {
                    double score = item.Score;

                    if (first)
                    {
                        player.Wins++;
                        first = false;
                    }

                    if (player.BestGame < score)
                    {
                        player.BestGame = score;
                    }

                    if (score == (ScoreFormulatorExpression.PerfectScoreStatic * totalRoundCount))
                    {
                        player.Perfects++;
                    }
                }
            }
        }

        private static void SetCountry(Round round)
        {
            string code = BorderHelper.GetFeatureHitBy(new double[] { round.CorrectLocation.Longitude, round.CorrectLocation.Latitude }, out GeoJson geo, out Feature hitFeature, out Polygon _);

            Country countryInfo = CountryHelper.GetCountryInformation(code, geo, hitFeature, Settings.Default.UseEnglishCountryNames, out Country exactcountry);

            GameModeSpecificSettings();

            if (ClientDbCache.RunningGame.Source.streakType == Game.USStreaksGame)
            {
                countryInfo = new(exactcountry.Code, exactcountry.Name);
            }

            if (!string.IsNullOrEmpty(countryInfo.Code))
            {

                round.Country = countryInfo;
                round.ExactCountry = exactcountry;
            }

        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="game"></param>
        public void EndGame(GeoGuessrGame game)
        {
            if (ClientDbCache.RunningGame != null && game != null && ClientDbCache.RunningGame.GeoGuessrId == game.token && Settings.Default.SendGameEndedMsg)
            {
                CurrentState = AppGameState.GAMEEND;

                TriggerGameEndActions();

                GameResult winningResult = ClientDbCache.RunningGame.Results.FirstOrDefault();

                GCUtils.ThrowIfNull(winningResult, nameof(winningResult), "No winner found for game result.");

                Player player = winningResult.Player;

                foreach (GameResult result in ClientDbCache.RunningGame.Results)
                {
                    string channel = GCResourceRequestHandler.ClientUserID.ToLowerInvariant();
                    if (result.Player.PlayerName.ToLowerInvariant() != channel || Settings.Default.CheckStreamerGuessForSpecialValues)
                    {
                        TriggerSpecialDistanceActions(result.Player, result.Distance);
                        TriggerSpecialScoreActions(result.Player, result.Score);
                    }
                }
                if (ClientDbCache.RunningGame.Mode == GameMode.STREAK && Settings.Default.EnableTwitchChatMsgs)
                {

                    Game g = ClientDbCache.RunningGame;
                    if (!guessApiClient.SummaryEnabled)
                        CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_EndStreak", new Dictionary<string, string>() { { "endRoundNumber", (ClientDbCache.RunningGame.Rounds.Count - 1).ToStringDefault() } }));
                    else
                        CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_EndStreakSummary", new Dictionary<string, string>() { { "winner", player.FullDisplayName }, { "gameId", g.GeoGuessrId }, { "endRoundNumber", (ClientDbCache.RunningGame.Rounds.Count - 1).ToStringDefault() } }));

                }
                else
                {
                    
                    Game g = ClientDbCache.RunningGame;
                    while (g.Previous != null)
                    {
                        g = g.Previous;
                    }
                    if (Settings.Default.EnableTwitchChatMsgs)
                        if (guessApiClient.SummaryEnabled)
                            CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_gameEnd", new Dictionary<string, string>() { { "winner", player.FullDisplayName }, { "gameId", g.GeoGuessrId } }));
                        else
                            CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_gameEndNoSummary", new Dictionary<string, string>() { { "winner", player.FullDisplayName } }));
                }
                SendEndGameToMaps(ClientDbCache.RunningGame);
                if (ClientDbCache.RunningGame.IsPartOfInfiniteGame)
                {
                    SendEndInfinityGameToJS(ClientDbCache.RunningGame);
                }
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void CloseRound()
        {
            logger.Debug("CloseRound");
            Round round = ClientDbCache.RunningGame.GetCurrentRound();
            if (round == null)
            {
                logger.Error("CloseRound: Round was null.");
                return;
            }

            round.TimeStamp = DateTime.Now;
            round.Flags = ClientDbCache.Instance.NextRoundRoundOptions;
            ClientDbCache.Instance.NextRoundRoundOptions = RoundOption.DEFAULT;
            SetRefreshMenuItemsEnabledState(true);
            SendStartRoundToJS(round);
            SendStartRoundToMaps(round);
            ToggleGuesses(true);
            TriggerRoundStartActions();
        }
    }
}
