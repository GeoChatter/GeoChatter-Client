using GeoChatter.Extensions;
using GeoChatter.Core.Extensions;
using GeoChatter.Core.Handlers;
using GeoChatter.Core.Interfaces;
using GeoChatter.Core.Model;
using GeoChatter.Model.Interfaces;
using GeoChatter.Core.Storage;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GeoChatter.Model;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Helpers;
using CefSharp;
using GeoChatter.Core.Model.Map;

namespace GeoChatter.Web
{
    /// <summary>
    /// Methods and properties to expose to JS
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Accessed from JS")]
    public class JsToCsHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(JsToCsHelper));

        private IMainForm mainForm;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="main"></param>
        public JsToCsHelper(IMainForm main)
        {
            mainForm = main;
        }

        /// <summary>
        /// Execute local JS user scripts
        /// </summary>
        public void ExecuteUserScripts()
        {
            mainForm.ExecuteUserScripts();
        }

#if DEBUG
        /// <summary>
        /// Display devtools window
        /// </summary>
        public void ShowDevTools()
        {
            mainForm?.ShowDevTools();
        }

        /// <summary>
        /// Send random guesses
        /// </summary>
        /// <param name="amount"></param>
        public void SendRandomGuess(int amount = 30)
        {
            mainForm?.SendRandomGuess(amount);
        }

#endif
        /// <summary>
        /// Change zoom given amount
        /// </summary>
        /// <returns></returns>
        public void Zoom(double amount = 0)
        {
            mainForm?.Zoom(amount);
        }

        /// <summary>
        /// Set refresh state
        /// </summary>
        /// <returns></returns>
        public void GoingToRefresh(string url)
        {
            mainForm?.RefreshBrowser(url);
        }

        /// <summary>
        /// Set refresh state
        /// </summary>
        /// <returns></returns>
        public int ConnectionState()
        {
            return mainForm?.GetConnectionState() ?? -1;
        }

        /// <summary>
        /// Verify game start even fired for given id
        /// </summary>
        /// <returns></returns>
        public void VerifyGameStarted(string id)
        {
            mainForm?.VerifyGameStarted(id);
        }

        private class BufferData
        {
            public string type { get; set; }
            public byte[] data { get; set; }
        }

        /// <summary>
        /// Directory of exported files
        /// </summary>
        public const string ExportsDirectory = "exports";

        internal void ValidateExportsDirectory()
        {
            if (!Directory.Exists(ExportsDirectory))
            {
                Directory.CreateDirectory(ExportsDirectory);
            }
        }

        /// <summary>
        /// Export scoreboard
        /// </summary>
        /// <returns></returns>
        public string ExportScoreboard(string name, string arr)
        {
            try
            {
                ValidateExportsDirectory();
                BufferData data = JsonConvert.DeserializeObject<BufferData>(arr);
                File.WriteAllBytes(Path.Combine(ExportsDirectory, name), data.data);
                return string.Empty;
            }
            catch (Exception e)
            {
                logger.Error("Failed to export scoreboard. " + e.Summarize());
                return e.Message;
            }
        }
        /// <summary>
        /// Set streamer's guess from map click for streaks games
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public void SetStreamerLastStreaksGameGuess(double lat, double lng)
        {
            mainForm?.SetStreamerLastStreaksGameGuess(lat, lng);
        }

        /// <summary>
        /// End game with given <paramref name="gameId"/>
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="challenge"></param>
        public void EndGame(string gameId, bool challenge = false)
        {
            if (GeoGuessrClient.UpdateGameData(ClientDbCache.RunningGame.Source, challenge))
            {
                ClientDbCache.Instance.NextRoundRoundOptions = RoundOption.DEFAULT;
                mainForm?.EndGame(ClientDbCache.RunningGame.Source);
            }
            else
            {
                logger.Error("Failed to EndGame " + gameId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EndInfiniteGame()
        {
            logger.Info("Ending infinite game");
            if (GeoGuessrClient.UpdateGameData(ClientDbCache.RunningGame.Source, ClientDbCache.RunningGame.Source.type == "challenge"))
            {
                ClientDbCache.Instance.NextRoundRoundOptions = RoundOption.DEFAULT;

                ClientDbCache.RunningGame.CalculateResults(true);

                ClientDbCache.RunningGame.WriteGameResultLabels(mainForm?.LabelPath);
                ClientDbCache.RunningGame.CurrentRound = -2;
                ClientDbCache.RunningGame.End = DateTime.Now;
                mainForm?.EndGame(ClientDbCache.RunningGame.Source);

                return true;
            }
            else
            {
                logger.Error("Failed to EndGame " + ClientDbCache.RunningGame.GeoGuessrId);
                return false;
            }
        }

        /// <summary>
        /// Start next game as an infinite game
        /// </summary>
        public void SetGuessPanoId(int round, string player, string pano)
        {
            mainForm?.SetPanoIdForGuessOf(round, player, pano);
        }

        /// <summary>
        /// Start next game as an infinite game
        /// </summary>
        public void OverwriteRoundData(int round, double lat, double lng, string pano)
        {
            mainForm?.OverwriteRoundData(round, lat, lng, pano);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveGameToClient()
        {
            return await ClientDbCache.SaveGame(ClientDbCache.RunningGame);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async void SaveGameToServer()
        {
            await mainForm?.SaveGameToApi();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ReportMainJSCompleted()
        {
            logger.Debug("ReportMainJSCompleted");
            mainForm?.ReportMainJSCompleted();
        }

        /// <summary>
        /// Start next game as an infinite game
        /// </summary>
        public void MarkNextStartAsInfinite()
        {
            logger.Info("Marking next game start as infinite game");
            ClientDbCache.Instance.NextGameGameOptions |= GameOption.INFINITE;
        }

        /// <summary>
        /// Get list of available layer names
        /// </summary>
        /// <returns></returns>
        public List<string> GetAvailableLayers()
        {
            return mainForm?.AvailableLayers ?? new List<string>();
        }

        /// <summary>
        /// Change map round setting for next round
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        /// <param name="forClient"></param>
        public void ChangeRoundSetting(string settingName, object value, bool forClient)
        {
            if (mainForm == null || mainForm.RoundSettingsPreference == null)
            {
                return;
            }

            switch (settingName)
            {
                case nameof(MapRoundSettings.Blurry):
                    {
                        if (value is bool val)
                        {
                            mainForm.RoundSettingsPreference.Blurry = val;
                        }
                        break;
                    }
                case nameof(MapRoundSettings.UpsideDown):
                    {
                        if (value is bool val)
                        {
                            mainForm.RoundSettingsPreference.UpsideDown = val;
                        }
                        break;
                    }
                case nameof(MapRoundSettings.Is3dEnabled):
                    {
                        if (value is bool val)
                        {
                            mainForm.RoundSettingsPreference.Is3dEnabled = val;
                        }
                        break;
                    }
                case nameof(MapRoundSettings.BlackAndWhite):
                    {
                        if (value is bool val)
                        {
                            mainForm.RoundSettingsPreference.BlackAndWhite = val;
                        }
                        break;
                    }
                case nameof(MapRoundSettings.Sepia):
                    {
                        if (value is bool val)
                        {
                            mainForm.RoundSettingsPreference.Sepia = val;
                        }
                        break;
                    }
                case nameof(MapRoundSettings.Mirrored):
                    {
                        if (value is bool val)
                        {
                            mainForm.RoundSettingsPreference.Mirrored = val;
                        }
                        break;
                    }
                case nameof(MapRoundSettings.Layers):
                    {
                        if (value is string val && !string.IsNullOrWhiteSpace(val))
                        {
                            if (!mainForm.RoundSettingsPreference.Layers.Remove(val))
                            {
                                mainForm.RoundSettingsPreference.Layers.Add(val);
                            }
                        }
                        break;
                    }
                case nameof(MapRoundSettings.MaxZoomLevel):
                    {
                        if (value is string v)
                        {
                            int val = v.ParseAsInt();
                            mainForm.RoundSettingsPreference.MaxZoomLevel = val > 1 ? (val < 23 ? val : 23) : 1;
                        }
                        else if (value is int i)
                        {
                            mainForm.RoundSettingsPreference.MaxZoomLevel = i > 1 ? (i < 23 ? i : 23) : 1;
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Start next round as with multiguessing enabled
        /// </summary>
        public void MarkNextRoundAsMultiGuess(bool state)
        {
            if (state)
            {
                logger.Info("Marking next round as multiguess");
                ClientDbCache.Instance.NextRoundRoundOptions |= RoundOption.MULTIGUESS;
            }
            else
            {
                logger.Info("Marking next round as non-multiguess");
                ClientDbCache.Instance.NextRoundRoundOptions &= ~RoundOption.MULTIGUESS;
            }
        }

        /// <summary>
        /// Create a game and return gg data
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "False positive")]
        public string CreateGame(string serialized)
        {
            GGPostGame pg = JsonConvert.DeserializeObject<GGPostGame>(serialized);
            GeoGuessrGame game = GeoGuessrClient.CreateGame(pg);
            return game != null ? JsonConvert.SerializeObject(game) : string.Empty;
        }

        /// <summary>
        /// Creation state of a new game in a chain of games
        /// </summary>
        public enum ChainGameCreateState
        {
            /// <summary>
            /// Failed to create a new game
            /// </summary>
            CreateFailed = -2,
            /// <summary>
            /// Failed to create the previous or the new game
            /// </summary>
            SaveFailed = -1,
            /// <summary>
            /// Internal error while creating the game
            /// </summary>
            InternalError = 0,
            /// <summary>
            /// Successfully created and chained a new game
            /// </summary>
            Success = 1
        }

        /// <summary>
        /// Create a game and return gg data
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "False positive")]
        public async Task<ChainGameCreateState> CreateNextGameInChain()
        {
            try
            {
                logger.Info($"Creating a new game for the chain. Previous game: {ClientDbCache.RunningGame.GeoGuessrId}");

                GeoGuessrGame ggGame = GeoGuessrClient.CreateGame(new()
                {
                    forbidMoving = ClientDbCache.RunningGame.Source.forbidMoving,
                    forbidZooming = ClientDbCache.RunningGame.Source.forbidZooming,
                    forbidRotating = ClientDbCache.RunningGame.Source.forbidRotating,
                    timeLimit = ClientDbCache.RunningGame.Source.timeLimit,
                    map = ClientDbCache.RunningGame.Source.map,
                    type = ClientDbCache.RunningGame.Source.type,
                });

                Game oldgame = ClientDbCache.RunningGame;
                Game newgame = ggGame.CreateFreshFromGeoGuessrGame(ClientDbCache.RunningGame.ParentCache, ClientDbCache.RunningGame.LabelSettings, ClientDbCache.RunningGame.Channel);
                if (newgame == null)
                {
                    logger.Error("Failed to create new game to be added to chain");
                    return ChainGameCreateState.CreateFailed;
                }
                ClientDbCache.Instance.NextGameGameOptions |= oldgame.Flags;
                newgame.GetCurrentRound().TimeStamp = DateTime.Now;
                newgame.PositionInChainFromStart = ClientDbCache.RunningGame.PositionInChainFromStart + 1;
                newgame.Mode = oldgame.Mode;

                bool status = await ClientDbCache.SaveGame(oldgame);

                if (!status)
                {
                    logger.Error("Failed to save the old game while creating the next game in chain.");
                    return ChainGameCreateState.SaveFailed;
                }

                newgame.Previous = ClientDbCache.RunningGame;
                ClientDbCache.RunningGame.Next = newgame;

                status = await ClientDbCache.SaveGame(newgame);
                if (!status)
                {
                    logger.Error("Failed to save the new game while creating the next game in chain.");
                    return ChainGameCreateState.SaveFailed;
                }

                ClientDbCache.RunningGame = newgame;

                string url = $"{GeoGuessrClient.GeoGuessrGameAddress}{newgame.GeoGuessrId}";
                logger.Info($"Loading URL to main form: {url}");
                mainForm.LoadURL(url);

                return ChainGameCreateState.Success;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return ChainGameCreateState.InternalError;
            }
        }
        /// <summary>
        /// Try firing game start event again
        /// </summary>
        public async Task<bool> ReTriggerStartGameEvent()
        {
            return (ClientDbCache.RunningGame == null || ClientDbCache.RunningGame.CurrentRound > -1)
&& mainForm != null && await mainForm.ReTriggerStartGameEvent();
        }

        /// <summary>
        /// Get scoreboard options data
        /// </summary>
        /// <returns></returns>
        public string GetTableOptions()
        {
            return JsonConvert.SerializeObject(ClientDbCache.Instance.TableOptions.Load());

        }
        /// <summary>
        /// Set scoreboard options data
        /// </summary>
        /// <param name="settingsJson"></param>
        public void SetTableOptions(string settingsJson)
        {
            TableOptions to = JsonConvert.DeserializeObject<TableOptions>($"{{\"{nameof(ITableOptions.Options)}\":{settingsJson}}}");
            to.Save();
            return;
        }
        /// <summary>
        /// Get scheme handler data
        /// </summary>
        /// <returns></returns>
        public string GetSchemeSettings()
        {
            string json = $"{{\"Name\": \"{GCSchemeHandlerFactory.SchemeName}\", \"RefreshParam\": \"{GCSchemeHandler.RefreshParam}\", \"Dynamic\": {GCSchemeHandler.EnableDynamicResources.ToStringDefault()} }}";
            return json;
        }
        /// <summary>
        /// Get a random point close or within a border polygon
        /// </summary>
        /// <returns></returns>
        public string GetRandomCoordinates()
        {
            Coordinates coor = Core.Helpers.BorderHelper.GetRandomPointWithinARandomPolygon();
            return $"{{\"Latitude\": {coor.Latitude.ToStringDefault()}, \"Longitude\": {coor.Longitude.ToStringDefault()}}}";
        }
        
        /// <summary>
        /// Get a random point close or within a border polygon
        /// </summary>
        /// <returns></returns>
        public void CopyMapLink()
        {
           mainForm?.CopyMapLink();
        }
            /// <summary>
        /// Get a random point close or within a border polygon
        /// </summary>
        /// <returns></returns>
        public void PlayRandomMap()
        {
           mainForm?.PlayRandomMap();
        }
        
        /// <summary>
        /// Get a random point close or within a border polygon
        /// </summary>
        /// <returns></returns>
        public void CopyResultLink()
        {
           mainForm?.CopyResultLink();
        }
        /// <summary>
        /// Get overlay settings
        /// </summary>
        /// <returns></returns>
        public string GetOverlaySettings()
        {
            return mainForm?.GetOverlaySetting();
        }

        /// <summary>
        /// Set overlay settings
        /// </summary>
        /// <param name="settingsJson"></param>
        public void SetOverlaySettings(string settingsJson)
        {
            mainForm?.SetOverlaySetting(settingsJson);
        }

        /// <summary>
        /// Set guess recieve processing methods' state
        /// </summary>
        /// <param name="open">Wheter to open guesses</param>
        public void ToggleGuesses(bool open)
        {
            mainForm?.ToggleGuesses(open);
        }

        /// <summary>
        /// End current round for <paramref name="gameId"/>
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="challenge"></param>
        /// <param name="randomStreamerGuess"></param>
        /// <returns></returns>
        public string EndRound(string gameId, bool challenge = false, bool randomStreamerGuess = false)
        {
            if (GeoGuessrClient.UpdateGameData(ClientDbCache.RunningGame.Source, challenge))
            {
                mainForm?.ProcessStreamerGuess(ClientDbCache.RunningGame.Source, randomStreamerGuess);
                Round r = ClientDbCache.RunningGame.GetCurrentOrFinishedRound();
                return CreateCustomEventArgsScriptObject(r.ToStandingsJson());
            }
            else
            {
                logger.Error("Failed to EndRound " + gameId);
                return "";
            }
        }

        /// <summary>
        /// Called after a round ended with <see cref="EndRound(string, bool, bool)"/> to start the next round with "Next Round" button
        /// </summary>
        public void CloseRound()
        {
            mainForm?.CloseRound();
        }

        /// <summary>
        /// Set export settings
        /// </summary>
        /// <param name="format"></param>
        /// <param name="alert"></param>
        /// <param name="autorounds"></param>
        /// <param name="autostandings"></param>
        /// <param name="autogames"></param>
        public void SetExportPreferences(string format, bool alert, bool autorounds, bool autostandings, bool autogames)
        {
            ClientDbCache.Instance.PreferredExportFormat = format;
            ClientDbCache.Instance.AlertOnExportSuccess = alert;
            ClientDbCache.Instance.AutoExportGames = autogames;
            ClientDbCache.Instance.AutoExportStandings = autostandings;
            ClientDbCache.Instance.AutoExportRounds = autorounds;
        }

        /// <summary>
        /// Custom event names
        /// </summary>
        public enum JSCustomEvent
        {
            /// <summary>
            /// Register listeners for guess button
            /// </summary>
            RegisterClick,
            /// <summary>
            /// Game started
            /// </summary>
            StartGame,
            /// <summary>
            /// Round started
            /// </summary>
            StartRound,
            /// <summary>
            /// Round ended
            /// </summary>
            EndRound,
            /// <summary>
            /// Game ended
            /// </summary>
            EndGame,
            /// <summary>
            /// Infinity Game ended
            /// </summary>
            EndInfinityGame,
            /// <summary>
            /// Overlay settings updated
            /// </summary>
            SettingsUpdate,
            /// <summary>
            /// Game exited
            /// </summary>
            ExitGame,
            /// <summary>
            /// Page refreshed during game
            /// </summary>
            RefreshGame,
            /// <summary>
            /// New guess recieved
            /// </summary>
            NewGuess,
            /// <summary>
            /// Opened game start main page address
            /// </summary>
            AddressMainPlayScreen,
            /// <summary>
            /// Triggers the drawing of the map link button
            /// </summary>
            DrawMapLinkButton,
            /// <summary>
            /// Loading screen load-unload
            /// </summary>
            LoadingScreen,
            /// <summary>
            /// Sign out event
            /// </summary>
            SignOut,
            /// <summary>
            /// Sign out event
            /// </summary>
            ToggleGuessSlider,
        }

        /// <summary>
        /// Make sure game has a parent cache reference
        /// </summary>
        /// <param name="game"></param>
        private static void ValidateParentCache(Game game)
        {
            if (game != null && game.ParentCache == null)
            {
                game.ParentCache = ClientDbCache.Instance;
            }
        }

        /// <summary>
        /// Create a JS executable script for firing {<paramref name="eventname"/>}Event with <paramref name="details"/>
        /// </summary>
        /// <param name="eventname">Use <see cref="JSCustomEvent"/> events</param>
        /// <param name="details">Object to send as event arguments in form of <c>{detail: details}</c></param>
        /// <returns></returns>
        public static string CreateDispatchScript(string eventname, string details = null)
        {
            string s = $"var evt = new CustomEvent('{eventname}Event', {CreateCustomEventArgsScriptObject(details)}); window.dispatchEvent(evt);";
            return s;
        }

        /// <summary>
        /// Event args object for JS
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreateCustomEventArgsScriptObject(string data)
        {
            return $"{{\"detail\": {data ?? "''"}}}";
        }

        /// <summary>
        /// Generates the script neccessary for binding to the Guess-Button
        /// </summary>
        /// <returns></returns>
        public static string GetRegisterClickToJsScript()
        {
            return CreateDispatchScript(nameof(JSCustomEvent.RegisterClick));
        }

        /// <summary>
        /// Show loading screen, use given message
        /// </summary>
        public static string EnableLoadingScreen(string message)
        {
            return CreateDispatchScript(nameof(JSCustomEvent.LoadingScreen), $"\"{message.ReplaceDefault("\"", "'")}\"");
        }

        /// <summary>
        /// Hide loading screen
        /// </summary>
        public static string DisableLoadingScreen()
        {
            return CreateDispatchScript(nameof(JSCustomEvent.LoadingScreen));
        }
        public static string ToggleGuessSlider()
        {
            return CreateDispatchScript(nameof(JSCustomEvent.ToggleGuessSlider));
        }

        /// <summary>
        /// Create <see cref="JSCustomEvent.EndRound"/> script for <paramref name="round"/>
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public static string GetEndRoundJsScript(Round round)
        {
            ValidateParentCache(round?.Game);
            return round == null ? string.Empty : CreateDispatchScript(nameof(JSCustomEvent.EndRound), round.ToJson());
        }

        /// <summary>
        /// Create <see cref="JSCustomEvent.StartRound"/> script for <paramref name="round"/>
        /// </summary>
        /// <param name="round"></param>
        /// <returns></returns>
        public static string GetStartRoundJsScript(Round round)
        {
            ValidateParentCache(round?.Game);
            return round == null ? string.Empty : CreateDispatchScript(nameof(JSCustomEvent.StartRound), round.ToJson());
        }

        /// <summary>
        /// Create <see cref="JSCustomEvent.SettingsUpdate"/> script 
        /// </summary>
        /// <returns></returns>
        public static string GetSettingsUpdateJsScript()
        {
            return CreateDispatchScript(nameof(JSCustomEvent.SettingsUpdate));
        }

        /// <summary>
        /// Create <see cref="JSCustomEvent.EndGame"/> script for <paramref name="game"/>
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static string GetEndGameJsScript(Game game)
        {
            ValidateParentCache(game);
            return game == null ? string.Empty : CreateDispatchScript(nameof(JSCustomEvent.EndGame), game.ResultsToJson());
        }

        /// <summary>
        /// Create <see cref="JSCustomEvent.EndInfinityGame"/> script for <paramref name="game"/>
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static string GetEndInfinityGameJsScript(Game game)
        {
            ValidateParentCache(game);
            return game == null ? string.Empty : CreateDispatchScript(nameof(JSCustomEvent.EndInfinityGame), game.ResultsToJson());
        }
        /// <summary>
        /// Create <see cref="JSCustomEvent.StartGame"/> script for <paramref name="game"/>
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static string GetStartGameJsScript(Game game)
        {
            ValidateParentCache(game);
            return game == null ? string.Empty : CreateDispatchScript(nameof(JSCustomEvent.StartGame), game.ToJson());
        }
        /// <summary>
        /// Create <see cref="JSCustomEvent.ExitGame"/> script for <paramref name="game"/>
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static string GetExitGameJsScript(Game game)
        {
            ValidateParentCache(game);
            return game == null ? string.Empty : CreateDispatchScript(nameof(JSCustomEvent.ExitGame), $"'{game.GeoGuessrId}'");
        }
        /// <summary>
        /// Create <see cref="JSCustomEvent.AddressMainPlayScreen"/> script
        /// </summary>
        /// <returns></returns>
        public static string GetAddressMainPlayScreenJsScript()
        {
            return CreateDispatchScript(nameof(JSCustomEvent.AddressMainPlayScreen), "''");
        }
        /// <summary>
        /// Create <see cref="JSCustomEvent.AddressMainPlayScreen"/> script
        /// </summary>
        /// <returns></returns>
        public static string DrawMapLinkButton()
        {
            return CreateDispatchScript(nameof(JSCustomEvent.DrawMapLinkButton), "''");
        }
        /// <summary>
        /// Create <see cref="JSCustomEvent.RefreshGame"/> script for <paramref name="game"/>
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static string GetRefreshGameJsScript(Game game)
        {
            ValidateParentCache(game);
            return game == null ? string.Empty : CreateDispatchScript(nameof(JSCustomEvent.RefreshGame), $"'{game.GeoGuessrId}'");
        }

        /// <summary>
        /// Sign out from the site
        /// </summary>
        /// <param name="red"></param>
        /// <returns></returns>
        public static string GetSendSignOutDataToJs(string red)
        {
            return CreateDispatchScript(nameof(JSCustomEvent.SignOut), $"'{(string.IsNullOrEmpty(red) ? "/signin" : red)}'");
        }

        /// <summary>
        /// Create <see cref="JSCustomEvent.NewGuess"/> script for <paramref name="guess"/>
        /// </summary>
        /// <param name="guess"></param>
        /// <returns></returns>
        public static string GetSendGuessObjToJsScript(Guess guess)
        {
            if (guess == null)
            {
                return string.Empty;
            }
            return CreateDispatchScript(nameof(JSCustomEvent.NewGuess), guess.ToJson());
        }

    }

}
