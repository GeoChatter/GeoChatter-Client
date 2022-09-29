using GeoChatter.Model.Enums;
using GeoChatter.Core.Model;
using System;
using System.Threading.Tasks;
using GeoChatter.Model;

namespace GeoChatter.Core.Interfaces
{
    /// <summary>
    /// Main GeoChatter form
    /// </summary>
    public interface IMainForm
    {
#if DEBUG
        /// <summary>
        /// Show devtools window
        /// </summary>
        void ShowDevTools();

        /// <summary>
        /// Send random guesses
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="reuse"></param>
        void SendRandomGuess(int amount = 30, bool reuse = false);
#endif
        /// <summary>
        /// GeoChatter version
        /// </summary>
        public string Version { get; set; }


        public bool IsDebugEnabled();
        /// <summary>
        /// Labels path in settings
        /// </summary>
        public string LabelPath { get; }

        /// <summary>
        /// Refresh the browser at given address
        /// </summary>
        /// <param name="url"></param>
        public void RefreshBrowser(string url = null);

        /// <summary>
        /// Initialize the browser at given address
        /// </summary>
        /// <param name="url"></param>
        /// <param name="refresh"></param>
        public void InitializeBrowser(string url = "", bool refresh = false);
        /// <summary>
        /// Get client->server connection state
        /// <para>Use <see cref="ConnectionState"/> for enums</para>
        /// </summary>
        /// <returns></returns>
        public int GetConnectionState();

        /// <summary>
        /// Called when geochatter.js finishes loading
        /// </summary>
        public void ReportMainJSCompleted();

        /// <summary>
        /// Save to api
        /// </summary>
        public Task SaveGameToApi();

        /// <summary>
        /// Set guess panoid for given player
        /// </summary>
        /// <returns></returns>
        public void SetPanoIdForGuessOf(int round, string playerName, string panoid);

        /// <summary>
        /// Overwrite round data 
        /// </summary>
        /// <returns></returns>
        public void OverwriteRoundData(int round, double lat, double lng, string panoid);

        /// <summary>
        /// Verify game start even fired for given id
        /// </summary>
        /// <returns></returns>
        public void VerifyGameStarted(string id);

        /// <summary>
        /// Load url
        /// </summary>
        public void LoadURL(string url);

        public void ProcessViewerFlag(string playerid, string flag, string userName="", Platforms userPlatform=Platforms.Twitch,  string displayName = "", string profilePicUrl = "");
        public void ProcessViewerColor(string playerid, string color, string userName = "", Platforms userPlatform=Platforms.Twitch, string displayName = "", string profilePicUrl = "");


        /// <summary>
        /// Change zoom level given amount
        /// </summary>
        public void Zoom(double amount);

        /// <summary>
        /// Set refresh state to true
        /// </summary>
        public void GoingToRefresh();

        /// <summary>
        /// Current application / game state
        /// </summary>
        public AppGameState CurrentState { get; set; }

        /// <summary>
        /// Currently active bot
        /// </summary>
        public IBotBase CurrentBot { get; set; }

        /// <summary>
        /// Set streamer's last map click data for streaks games
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        void SetStreamerLastStreaksGameGuess(double lat, double lng);

        /// <summary>
        /// Connect Twitch bot
        /// </summary>
        void ConnectTwitchBot();

        /// <summary>
        /// Execute custom JS scripts
        /// </summary>
        void ExecuteUserScripts();

        /// <summary>
        /// Start setting up the game instances
        /// </summary>
        void SetupGame();
        /// <summary>
        /// Setup client cache using <paramref name="geoGame"/>
        /// </summary>
        /// <param name="geoGame"></param>
        /// <param name="retrigger"></param>
        Task<GameFoundStatus> SetupStartGame(GeoGuessrGame geoGame, bool retrigger);
        /// <summary>
        /// End the <paramref name="game"/>
        /// </summary>
        /// <param name="game"></param>
        void EndGame(GeoGuessrGame game);
        /// <summary>
        /// Process the game after client's guess is registered to GeoGuessr API
        /// </summary>
        /// <param name="game"></param>
        /// <param name="wasRandom"></param>
        void ProcessStreamerGuess(GeoGuessrGame game, bool wasRandom);
        /// <summary>
        /// Get current overlay settings
        /// </summary>
        /// <returns></returns>
        string GetOverlaySetting();
        /// <summary>
        /// Get current overlay settings
        /// </summary>
        /// <returns></returns>
        void CopyMapLink();   /// <summary>
        /// Get current overlay settings
        /// </summary>
        /// <returns></returns>
        void CopyResultLink();
        /// <summary>
        /// Apply overlay settings
        /// </summary>
        /// <param name="settingsJson"></param>
        void SetOverlaySetting(string settingsJson);
        /// <summary>
        /// Toggle guess processing
        /// </summary>
        /// <param name="open"></param>
        void ToggleGuesses(bool open);
        /// <summary>
        /// Close previous round and start next
        /// </summary>
        void CloseRound();

        /// <summary>
        /// Process a guess for given player data
        /// </summary>
        /// <param name="userId">Player ID</param>
        /// <param name="userName">Player name</param>
        /// <param name="userPlatform">Player name</param>
        /// <param name="latString">Latitude</param>
        /// <param name="lngString">Longitude</param>
        /// <param name="color">User name color</param>
        /// <param name="displayName">Display name</param>
        /// <param name="profilePicUrl">Profile picture url</param>
        /// <param name="wasRandom">Wheter guess was random</param>
        /// <returns></returns>
        GuessState ProcessViewerGuess(string userId, string userName, Platforms userPlatform, string latString, string lngString, string color, string displayName, string profilePicUrl, bool wasRandom = false, bool isTemporary = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="userPlatform"></param>
        /// <param name="latString"></param>
        /// <param name="lngString"></param>
        /// <param name="color"></param>
        /// <param name="displayName"></param>
        /// <param name="profilePicUrl"></param>
        /// <param name="wasRandom"></param>
        /// <returns></returns>
        GuessState ProcessViewerGuess(string userId, string userName, Platforms userPlatform, string latString, string lngString, string color, string displayName, Uri profilePicUrl, bool wasRandom, bool isTemporary);

        /// <summary>
        /// Try to trigger game start events manually
        /// </summary>
        Task<bool> ReTriggerStartGameEvent();

        /// <summary>
        /// Get/set current advanced settings state
        /// </summary>
        bool ShowAdvancedSettings { get; set; }

        void SendMapOptionsToMaps(MapOptions options);

        void ReconnectToGuessApi();
        void InitializeGlobalSecrets();
        void UpdateMapInTitle();

        Task ConnectToGuessApi(bool forceReconnect = false, bool login = true, bool isGGLogon = false);
    }
}
