using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model.Extensions;
using GeoChatter.Core.Model.Map;
using GeoChatter.Core.Storage;
using GeoChatter.Model;
using GeoChatter.Model.Enums;
using GeoChatter.Properties;
using GeoChatter.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoChatter.Forms
{
    public partial class MainForm
    { /// <summary>
      /// gives access to the central list of trolls
      /// </summary>
      /// <returns>list of twitch ids</returns>
        internal List<string> CGTrolls()
        {
            return guessApiClient?.Trolls;
        }

        private void SendExitGameToMaps()
        {
            guessApiClient.SendExitGameToMaps();
        }

        /// <inheritdoc/>
        public int GetConnectionState()
        {
            if (guessApiClient == null)
            {
                return (int)ConnectionState.UNKNOWN;
            }

            return guessApiClient.State;
        }

        public async void SendMapOptionsToMaps(MapOptions options)
        {
            await guessApiClient.SendMapOptionsToMaps(options);
        }
        private async Task ConnectToGuessApi(bool forceReconnect = false, bool login = true, bool isGGLogon = false)
        {
            if (guessApiClient == null)
            {
                guessApiClient = GuessApiClient.Instance;
                guessApiClient.Connected += GuessApiClient_Connected;
                guessApiClient.OnDisconnect += GuessApiClient_DisConnected;
                guessApiClient.OnReconnected += GuessApiClient_ReConnected;
                guessApiClient.OnLog += GuessApiClient_Log;
            }
            else if (forceReconnect)
            {
                if (guessApiClient.IsConnected)
                {
                    await guessApiClient.Disconnect();

                }

            }


            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            string huburl = Settings.Default.GuessServer;
#if DEBUG
            huburl = Settings.Default.AlternateGuessApiUrl;
           // huburl = "https://localhost:44350/geoChatterHub";
#endif

            bool success = await guessApiClient.Initialize(huburl, this, Settings.Default.GCClientId, Settings.Default.EnableDebugLogging, isGGLogon);
            if (success)
            {
                ApiClient client = new()
                {
                    Version = version,
                    ChannelId = GCResourceRequestHandler.ClientUserID,
                    ChannelName = GCResourceRequestHandler.ClientGeoGuessrName,
                    EnableOverlay = Settings.Default.MOEnableOverlay,
                    // InstalledFlagPacks =  "{" + string.Join(",", FlagPackHelper.FlagPacks.Select(f => $"\"{f.Name}\": \"https://service.geochatter.tv/flagpacks/{f.Name.ToUpperInvariant()}.zip\"")) + "}"

                };
                MapOptions options = GetMapOptions();
                await guessApiClient.Connect(client, options, login);

            }
        }

        private static MapOptions GetMapOptions()
        {
            return new()
            {
                ShowBorders = Settings.Default.MOShowBorders,
                ShowFlags = Settings.Default.MOShowFlags,
                ShowStreamOverlay = Settings.Default.MOEnableOverlay,
                InstalledFlagPacks = "[" + string.Join(",", FlagPackHelper.FlagPacks.Select(f => $"\"{f.Name}\"")) + "]",
                Streamer = GCResourceRequestHandler.ClientUserID,
                TemporaryGuesses = Settings.Default.MOEnableTempGuesses,
                TwitchChannelName = Settings.Default.EnableTwitchChatMsgs ? Settings.Default.TwitchChannel : string.Empty
            };
        }
        /// <summary>
        /// 
        /// </summary>
        public async void ReconnectToGuessApi()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            ApiClient client = new()
            {
                Version = version,
                ChannelId = GCResourceRequestHandler.ClientUserID,
                ChannelName = GCResourceRequestHandler.ClientGeoGuessrName,
            };


            MapOptions options = GetMapOptions();

            options.IsUSStreak = ClientDbCache.RunningGame != null && ClientDbCache.RunningGame.IsUsStreak;
            options.GameMode = ClientDbCache.RunningGame != null ? ClientDbCache.RunningGame.Mode.ToString() : GameMode.DEFAULT.ToString();

            guessApiClient.Connect(client, options, true, true);
        }

        private void GuessApiClient_Log(object sender, LogEventArgs e)
        {
            switch (e.LogLevel)
            {
                case LogLevel.Error:
                    logger.Error(e.Message);
                    break;
                case LogLevel.Debug:
                    logger.Debug(e.Message);
                    break;
                case LogLevel.Information:
                    logger.Info(e.Message);
                    break;
                case LogLevel.Warning:
                    logger.Warn(e.Message);
                    break;
            }


        }

        private void GuessApiClient_ReConnected(object sender, EventArgs e)
        {
            logger.Error("Connection to GuessServer re-established: "+e?.ToString());
            LoadingScreen(false);
            //MessageBox.Show("Connection to guess server re-established!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GuessApiClient_DisConnected(object sender, LogEventArgs e)
        {
            if (e.ExceptionObject is UnauthorizedAccessException ex)
            {
                logger.Error($"Unauthorized client: GCResourceRequestHandler.ClientGeoGuessrName:{GCResourceRequestHandler.ClientGeoGuessrName}, GCResourceRequestHandler.ClientUserID:{GCResourceRequestHandler.ClientUserID}, GCClientId:{Properties.Settings.Default.GCClientId}");
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                GCResourceRequestHandler.ClientUserID = String.Empty;
                LogoutAndRedirect();
            }
            else
            {
                logger.Error("Connection to GuessServer lost: ");
                logger.Error(e.Message);
                MessageBox.Show($"Connection to guess server lost!\r\nTrying to reconnect\r\n{e.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                LoadingScreen(true, "Lost connection to GeoChatter servers...");
            }
        }

        private void GuessApiClient_Connected(object sender, ConnectedEventArgs e)
        {
            if (CopyMapLinkToClipboard())
                MessageBox.Show($"You are now connected to our guess server!\n\r\n\rWe have copied the map url to your clipboard!\n\rPlease make sure to share it with your viewers!\n\r\n\rYour map name is " + e.Message, "User verification successful", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            waitingForApi = false;
            LoadingScreen(false);
        }

        private void SendEndRoundToMaps(Round finishedRound)
        {
            List<MapResult> response = new List<MapResult>();
            foreach (RoundResult result in finishedRound.Results)
            {
                MapResult roundResult = new MapResult();
                Guess guess = result.GetGuessOf();

                roundResult.DisplayName = result.Player.DisplayName;
                roundResult.UserName = result.Player.PlayerName;
                roundResult.ProfilePicUrl = result.Player.ProfilePictureUrl;
                roundResult.WasRandom = guess.WasRandom;
                roundResult.Score = result.Score;
                roundResult.Distance = result.Distance;
                roundResult.TimeTaken = result.TimeTaken;
                roundResult.Streak = result.Streak;
                roundResult.CountryCode = guess.Country.Code;
                roundResult.ExactCountryCode = guess.CountryExact.Code;
                roundResult.GuessCount = guess.GuessCounter;
                roundResult.IsStreamerResult = guess.IsStreamerGuess;
                roundResult.GuessedBefore = guess.GuessedBefore;
                response.Add(roundResult);

            }
            guessApiClient.SendEndRoundToMaps(response);
        }

        private void SendEndGameToMaps(Game currGame)
        {
            List<MapResult> response = new List<MapResult>();
            foreach (GameResult result in currGame.Results)
            {
                MapResult gameResult = new MapResult();

                gameResult.DisplayName = result.Player.DisplayName;
                gameResult.UserName = result.Player.PlayerName;
                gameResult.ProfilePicUrl = result.Player.ProfilePictureUrl;
                gameResult.Score = result.Score;
                gameResult.Distance = result.Distance;
                gameResult.TimeTaken = result.TimeTaken;
                gameResult.Streak = result.Streak;
                gameResult.GuessCount = result.GuessCount;
                gameResult.IsStreamerResult = result.Player.PlayerName == result.Player.Channel;
                gameResult.GameId = currGame.Source?.token;
                response.Add(gameResult);

            }
            guessApiClient.SendEndGameToMaps(response);
        }

        private void SendStartGameToMaps(Game runningGame)
        {
            MapGameSettings gameSettings = new MapGameSettings()
            {
                ForbidMoving = runningGame.Source.forbidMoving,
                ForbidRotating = runningGame.Source.forbidRotating,
                ForbidZooming = runningGame.Source.forbidZooming,
                GameMode = runningGame.Source.mode,
                GameState = runningGame.Source.state,
                GameType = runningGame.Source.type,
                IsInfinite = runningGame.IsPartOfInfiniteGame,
                IsStreak = runningGame.Mode == GameMode.STREAK,
                MapID = runningGame.Source.Id,
                MapName = runningGame.Source.mapName,
                RoundCount = runningGame.Source.roundCount,
                StreakType = runningGame.Source.streakType,
                TimeLimit = runningGame.Source.timeLimit
            };
            guessApiClient.SendStartGameToMaps(gameSettings);
        }

        private void SendStartRoundToMaps(Round round)
        {
            MapRoundSettings roundSettings = new MapRoundSettings()
            {
                IsMultiGuess = round.IsMultiGuess,
                RoundNumber = round.RealRoundNumber(),
                StartTime = round.TimeStamp
            };
            guessApiClient.SendStartRoundToMaps(roundSettings);
        }

    }
}
