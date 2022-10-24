using AutoUpdaterDotNET;
using CefSharp;
using CefSharp.WinForms;
using GeoChatter.Extensions;
using GeoChatter.Helpers;
using GeoChatter.Core.Extensions;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Interfaces;
using GeoChatter.Core.Model;
using GeoChatter.Model;
using GeoChatter.Model.Attributes;
using GeoChatter.Model.Enums;
using GeoChatter.Core.Storage;
using GeoChatter.Forms.FlagManager;
using GeoChatter.Forms.ScoreCalculator;
using GeoChatter.FormUtils;
using GeoChatter.Handlers;
using GeoChatter.Integrations;
using GeoChatter.Properties;
using GeoChatter.Web;
using GeoChatter.Web.Twitch;
using GeoChatter.Web.YouTube;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
#if !DEBUG
using System.IO;
#endif
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeoChatter.Core.Common.Extensions;
using TwitchLib.Client.Events;
using System.Reflection;

namespace GeoChatter.Forms
{
    /// <inheritdoc/>
    [SupportedOSPlatform("windows7.0")]
    public partial class MainForm : Form, IMainForm
    {
        #region Properties & Fields & Constants
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));
        private static StreamerbotClient streamerbotClient = new();
        private static readonly OBSClient obsClient = new();
        public GuessApiClient guessApiClient;

        internal bool guessesOpen
        {
            get;
            set;
        } = true;

        private const int WM_SYSCOMMAND = 0x112;
        private const int MF_STRING = 0x0;
        private const int MF_SEPARATOR = 0x800;
        // ID for the About item on the system menu
        private const int SYSMENU_OPEN_MENU_ID = 0x1;

        private readonly Dictionary<LabelType, bool> labelSettings = new();

        private Dictionary<string, DateTime> lastMultiGuessMessage = new();

        private Dictionary<string, DateTime> lastSameGuessMessage = new();

        /// <summary>
        /// Starting page url for the browser
        /// </summary>
        private string startPageUrl { get; } = "https://www.geoguessr.com/classic";

        /// <summary>
        /// Wheter extensions are loaded
        /// </summary>
        public bool ExtensionsInitialized { get; set; }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public AppGameState CurrentState { get; set; } = AppGameState.NOTINGAME;


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IBotBase CurrentBot { get; set; }

        /// <summary>
        /// JavaScript wrappers
        /// </summary>
        public List<JSWrapper> Wrappers { get; } = new List<JSWrapper>();

        /// <inheritdoc/>
        public bool ShowAdvancedSettings { get; set; }

        /// <summary>
        /// Wheter this form is currently in fullscreen
        /// </summary>
        public bool IsFullscreen { get; private set; }



        private bool GameSetupInTitleChange { get; set; }

        /// <summary>
        /// Progress window
        /// </summary>
        public ProgressWindow Progrsswin { get; set; } = new();


        /// <summary>
        /// Random number generator
        /// </summary>
        private static Random random = new();

        /// <inheritdoc/>
        public string Version { get; set; }

        #endregion

        /// <summary>
        /// Main constructor
        /// </summary>
        public MainForm(string version)
        {
            Version = version;

            logger.Info("Configured channel: " + GCResourceRequestHandler.ClientUserID);

            CultureInfo.CurrentCulture = CoreExtensions.DEFAULTCULTURE;
            Application.CurrentCulture = CultureInfo.CurrentCulture;


            KeyPreview = true;

            SplashScreenHelper.Show();

            logger.Info("Starting GeoChatter v" + Version);
            SplashScreenHelper.SetPercentage(0, "Loading globe...");
            using ClientDbContext db = new();

            SplashScreenHelper.SetPercentage(5, "Magically creating storage...");
            db.Migrate(Settings.Default);

            SplashScreenHelper.SetPercentage(10, "Loading globe...");
            InitializeComponent();

            SplashScreenHelper.SetPercentage(15, "Redrawing GeoGuessr borders...");
            InitializeBorders();

            SplashScreenHelper.SetPercentage(25, "Adding incredible flag packs...");
            InitializeFlagPacks();

            SplashScreenHelper.SetPercentage(30, "Training bot commands...");
            InitializeBotCommands();

            SplashScreenHelper.SetPercentage(35, "Making names readable...");
            InitializeMappings();

            SplashScreenHelper.SetPercentage(40, "Finding players...");
            InitializeClientDBCache();

            SplashScreenHelper.SetPercentage(50, "Responding to SOS....");

            SplashScreenHelper.SetPercentage(55, "Tying everything together...");
            InitializeAvailableLayers();

            SplashScreenHelper.SetPercentage(60, "Preparing secret sauce...");
            SetupBinds();

            SplashScreenHelper.SetPercentage(65, "Adding the coolest scripts...");
            InitializeWrappers();

            SplashScreenHelper.SetPercentage(70, "Learning new language...");
            InitializeBrowser();

            SplashScreenHelper.SetPercentage(75, "Sending bot off to Twitch...");
            LanguageStrings.Initialize(Settings.Default);
            LoadLabelSettings();


            // SplashScreenHelper.SetPercentage(80, "Creating guess receiver...");



            SplashScreenHelper.SetPercentage(85, "Contracting Streamer.Bot...");
            ConnectToStreamerbot();

            SplashScreenHelper.SetPercentage(90, "Taking control of OBS...");
            ConnectToObs();

            SplashScreenHelper.SetPercentage(95, "Hiring useless managers...");
            InitializeManagers();


            devToolsToolStripMenuItem.Visible = Settings.Default.DebugShowDevTools;

            SplashScreenHelper.SetPercentage(100, "Connecting to guess server....");


        }

        [DiscoverableEvent]
        private void OnLog(object sender, OnLogArgs args)
        {

        }
        #region Externs
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
        // P/Invoke declarations
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);
#pragma warning restore CA2101 // Specify marshaling for P/Invoke string arguments
        #endregion

        #region Overrides
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Get a handle to a copy of this form's system (window) menu
            IntPtr hSysMenu = GetSystemMenu(Handle, false);

            // Add a separator
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, string.Empty);

            // Add the About menu item
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_OPEN_MENU_ID, "Show/hide menu…");
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Test if the About item was selected from the system menu
            if ((m.Msg == WM_SYSCOMMAND) && ((int)m.WParam == SYSMENU_OPEN_MENU_ID))
            {
                ShowMenu();
            }

        }
        #endregion

        #region Initializers & Connections
        public void InitializeGlobalSecrets()
        {
            TwitchHelper.ClientSecret = guessApiClient?.GetKey("twitchsecret");
            YoutubeBot.ClientSecret = guessApiClient?.GetKey("youtubesecret");
        }

        private readonly List<string> ScriptNames = new()
        {
        };


        private static void InitializeBorders()
        {
            BorderHelper.Initialize();
            ISO3166Helper.Initialize();
        }

        private static void InitializeMappings()
        {
            CountryHelper.Initialize();
        }

        private static void InitializeFlagPacks()
        {
            CountryHelper.LoadFlags();
            FlagPackHelper.LoadCustomFlagPacks();
        }

        private void InitializeManagers()
        {
            using UserScriptManager mgr = new(this);

            AddDocumentStartScripts();

            Task.Run(() =>
            {
                string updates = UserScriptManager.RunAutoUpdates().Trim('\n');

                if (!string.IsNullOrWhiteSpace(updates))
                {
                    ReportUpdates(updates);
                }
            });

            Task.Run(() =>
            {
                // Initialize
                using CommandManager mgr = new();
            });
        }

        private void OnGeoGuessrSignedOut(object sender, EventArgs e)
        {
            guessApiClient.Disconnect();
        }

        private void OnGeoGuessrCookieHijackedFirst(object sender, EventArgs e)
        {
            HandleGGEvent(false);
        }

        private void OnGeoGuessrSignedIn(object sender, EventArgs e)
        {
            if (Settings.Default.LastLoginVersion != FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion)
            {
                Settings.Default.LastLoginVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                Settings.Default.Save();
            }
            isRefresh = false;
            HandleGGEvent(true);
        }
        bool isRefresh;
        private void HandleGGEvent(bool isLogin)
        {
            GGProfile profile = GeoGuessrClient.GetProfile();
            if (profile != null && profile.user != null)
            {
                if (GCResourceRequestHandler.ClientUserID != profile.user.id)
                    GCResourceRequestHandler.ClientUserID = profile.user.id;
                if (GCResourceRequestHandler.ClientGeoGuessrPic != profile.user.pin.url)
                    GCResourceRequestHandler.ClientGeoGuessrPic = profile.user.pin.url;

                if (GCResourceRequestHandler.ClientGeoGuessrName != profile.user.nick)
                {
                    GCResourceRequestHandler.ClientGeoGuessrName = profile.user.nick;
                }
            }
            else
                GCResourceRequestHandler.ClientUserID = String.Empty;

            if (!string.IsNullOrEmpty(GCResourceRequestHandler.ClientUserID) && !isRefresh)
            {
                Settings.Default.ChannelId = GCResourceRequestHandler.ClientUserID;
                Settings.Default.Save();
                LanguageStrings.Initialize(Settings.Default);

                waitingForApi = true;
                ConnectToGuessApi(true, isGGLogon: isLogin);
                if (Settings.Default.EnableTwitchChatMsgs && !string.IsNullOrEmpty(Properties.Settings.Default.TwitchChannel) && !string.IsNullOrEmpty(Settings.Default.oauthToken))
                    ConnectBot();
            }
            isRefresh = false;
        }

        internal bool waitingForApi = false;



        #endregion

        #region JavaScript Events

        #endregion

        /// <inheritdoc/>
        public void ResetJSCTRLCheck()
        {
            if (browser != null && browser.IsBrowserInitialized)
            {
                browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("(()=>{window.IsCTRLDown=false;})();");
            }
        }

        /// <inheritdoc/>
        public void Zoom(double amount = 0)
        {
            if (browser != null && browser.IsBrowserInitialized && hasFocus)
            {
                browser.SetZoomLevel(Settings.Default.ZoomLevel += amount);
            }
        }
        /// <summary>
        /// Method to initialize the DB access
        /// </summary>
        private static void InitializeClientDBCache()
        {
            ClientDbCache cache = ClientDbCache.Instance;
        }

        private const string UserScriptUpdateMessage = "UserScripts were updated... Page needs to be refreshed for changes to take effect. Do you want to refresh now ?";

        private void ReportUpdates(string updates)
        {
            logger.Debug("ReportUpdates");
            MessageBox.Show("Updated following user scripts:\n" + updates, "Auto Updated UserScripts", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            DialogResult res = MessageBox.Show(UserScriptUpdateMessage, "Page Refresh Needed", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (res == DialogResult.Yes)
            {
                RefreshBrowser();
            }
        }

        public void CopyMapLink()
        {
            if (CopyMapLinkToClipboard())
                MessageBox.Show($"We have copied the map url to your clipboard!\n\rPlease make sure to share it with your viewers!\n\r\n\rYour map name is " + guessApiClient.MapIdentifier, "Map link copied", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        public void CopyResultLink()
        {
            if (CopyResultLinkToClipboard())
                MessageBox.Show($"We have copied the reuslt link to your clipboard!\n\rPlease make sure to share it with your viewers!", "Result link copied", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }



        private bool CopyResultLinkToClipboard()
        {


            if (Settings.Default.DebugUseDevApi)
                new SetClipboardHelper(DataFormats.Text, $"https://geochatter.tv/testing_results/?id={ClientDbCache.RunningGame.GeoGuessrId}").Go();
            else
                new SetClipboardHelper(DataFormats.Text, $"https://geochatter.tv/results/?id={ClientDbCache.RunningGame.GeoGuessrId}").Go(); ;

            return true;
        }


        private bool CopyMapLinkToClipboard()
        {
            if (string.IsNullOrEmpty(guessApiClient.MapIdentifier) || !guessApiClient.IsConnected)
            {
                MessageBox.Show("You are not curently connected to our guess server!\n\rPlease restart the application!", "Not connected", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (Settings.Default.DebugUseDevApi)
                new SetClipboardHelper(DataFormats.Text, $"https://geochatter.tv/testing_map/?{guessApiClient.MapIdentifier}").Go();

            else
                new SetClipboardHelper(DataFormats.Text, $"https://geochatter.tv/map/?{guessApiClient.MapIdentifier}").Go(); ;

            return true;
        }


        private void UpdatePlayerAndGuessInCache(Player player, Guess guess)
        {
            player.IdOfLastGame = ClientDbCache.RunningGame.Id;
            player.RoundNumberOfLastGuess = ClientDbCache.RunningGame.CurrentRound;

            guess.Player = player;
            player.Guesses.Add(guess);
            if (!ClientDbCache.RunningGame.Players.Contains(player))
            {
                ClientDbCache.RunningGame.Players.Add(player);
            }
        }


        private static bool OldUseUsStateFlags = Settings.Default.UseUsStateFlags;
        private static bool OldOverlayRegionalFlags = Settings.Default.OverlayRegionalFlags;
        private static bool HandledUSSettings = true;

        private static void GameModeSpecificSettings()
        {
            if (ClientDbCache.RunningGame.Source.streakType == Game.USStreaksGame)
            {
                OldUseUsStateFlags = Settings.Default.UseUsStateFlags;
                OldOverlayRegionalFlags = Settings.Default.OverlayRegionalFlags;
                Settings.Default.UseUsStateFlags = true;
                Settings.Default.OverlayRegionalFlags = true;
                HandledUSSettings = false;
            }
            else if (!HandledUSSettings)
            {
                HandledUSSettings = true;
                Settings.Default.UseUsStateFlags = OldUseUsStateFlags;
                Settings.Default.OverlayRegionalFlags = OldOverlayRegionalFlags;
            }
        }


        /// <inheritdoc/>
        public string LabelPath => Settings.Default.LabelPath;


        [DiscoverableEvent]
        private void BadStateExceptionThrown(object sender, TwitchLib.Client.TwitchClient.BadStateExceptionThrownArgs args)
        {
            if (CurrentBot is not TwitchBot tb)
            {
                return;
            }

            if (tb.LastReconnectAttempt.AddSeconds(5) < DateTime.Now)
            {
                logger.Debug("Attempting to reconnect. " + args.Message);
                tb.LastReconnectAttempt = DateTime.Now;
                ConnectTwitchBot();
            }
            else if (tb.LastReconnectAttemptMessage.AddSeconds(10) < DateTime.Now)
            {
                logger.Debug("Skipping reconnect attempt. " + args.Message);
                MessageBox.Show("Failed to connect to the Twitch bot. Please try reconnecting from the settings window. If issue persists, please write a report to GeoChatter Discord #bugs channel!", "Bot Connection", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        /* 
		 * Important: Definition order of the methods are taken as event handler calling order
		 * Important: Event handler methods must be private and non-static
		 * 
		 * Options to declare a method an event handler:
		 *  1- Name method same as the event name and add DiscoverableEvent attribute
		 *  2- Name method matching {CustomName}_{EventName} pattern and add DiscoverableEvent attribute
		 *  3- Name method whatever and add DiscoverableEvent attribute constructed with the event's name
		 */
        #region Event Handlers




        private string LastAddress { get; set; } = string.Empty;

        private bool RefreshHandled { get; set; }

        private bool NextChangeIsRefresh { get; set; }

        /// <inheritdoc/>
        public void GoingToRefresh()
        {
            NextChangeIsRefresh = true;
            isRefresh = true;
        }






        private int LastBrowserID { get; set; } = 1;

        /// <inheritdoc/>

        private void SetRefreshMenuItemsEnabledState(bool enabled)
        {
            this.InvokeDefault(f => f.browserToolStripMenuItem.Enabled =
            f.userScriptsToolStripMenuItem.Enabled =
            f.flagManagerToolStripMenuItem.Enabled = enabled);
        }



        public void UpdateMapInTitle()
        {

            this.InvokeDefault(f =>
            {
                string[] titleParts = f.Text.Split(':');
                string currentMapName = titleParts[titleParts.Length - 1].Trim();
                f.Text = f.Text.ReplaceDefault(currentMapName, string.IsNullOrEmpty(guessApiClient.MapIdentifier) ? "Disconnected" : guessApiClient.MapIdentifier);

            });
        }


        #endregion

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettingsDialog();
        }

        SettingsDialog settingsDialog;
        private delegate void Settingscallback();

        /// <summary>
        /// Shows the settings dialog when the menstrip is clicked or Ctrl+Shift+S is pressed
        /// </summary>
        internal void ShowSettingsDialog()
        {
            Invoke(delegate ()
                {
                    try
                    {
                        settingsDialog = new(this, streamerbotClient, obsClient);
                        settingsDialog.SettingsApplied += (object sender, EventArgs a) => { LoadLabelSettings(); };
                        settingsDialog.TopMost = true;
                        settingsDialog.FormClosed += (object sender, FormClosedEventArgs e) =>
                        {
                            devToolsToolStripMenuItem.Visible = Settings.Default.DebugShowDevTools;
                        };
                        settingsDialog.Show(this);

                    }
                    catch (Exception e)
                    {
                        logger.Error(e);
                    }
                    finally
                    { settingsDialog = null; }


                }
            );
            LoadLabelSettings();
        }


        /// <summary>
        /// Obvious, i think
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClientDbCache.RunningGame != null && MessageBox.Show(LanguageStrings.Get("Chat_Msg_ExitWithRunningGame"), "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            Application.Exit();
        }

        /// <summary>
        /// Loads the settings for writing labels for easy usage in the Game/Round classes
        /// </summary>
        private void LoadLabelSettings()
        {

            labelSettings.Clear();
            labelSettings.Add(LabelType.RoundWinner, Settings.Default.RoundWinnerLabel);
            labelSettings.Add(LabelType.RoundSecond, Settings.Default.RoundSecondLabel);

            labelSettings.Add(LabelType.RoundThird, Settings.Default.RoundThirdLabel);

            labelSettings.Add(LabelType.GameWinner, Settings.Default.GameWinnerLabel);

            labelSettings.Add(LabelType.GameSecond, Settings.Default.GameSecondLabel);

            labelSettings.Add(LabelType.GameThird, Settings.Default.GameThirdLabel);

            labelSettings.Add(LabelType.RoundHighScore, Settings.Default.RoundHighscoreLabel);

            labelSettings.Add(LabelType.GameHighScore, Settings.Default.GameHighscoreLabel);

        }

        /// <summary>
        /// Used to show the menustrip when the streamer presses "Alt"
        /// </summary>
        private delegate void ShowMenuCallback();

        internal void ShowMenu()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (menuStrip1.InvokeRequired)
            {
                ShowMenuCallback d = new(ShowMenu);
                Invoke(d);
            }
            else
            {
                menuStrip1.Visible = !menuStrip1.Visible;
            }
        }

        /// <summary>
        /// Used to show the menustrip when the streamer presses "Alt"
        /// </summary>
        private delegate void FullscreenCallback();

        internal void ToggleFullscreen()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (menuStrip1.InvokeRequired)
            {
                FullscreenCallback d = new(ToggleFullscreen);
                Invoke(d);
            }
            else
            {

                if (FormBorderStyle != FormBorderStyle.None)
                {
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.None;
                    Bounds = Screen.GetBounds(Location);
                    menuStrip1.Visible = false;
                    IsFullscreen = true;
                }
                else
                {
                    WindowState = FormWindowState.Maximized;
                    FormBorderStyle = FormBorderStyle.Sizable;
                    //menuStrip1.Visible = true;
                    IsFullscreen = false;
                }
            }
        }

        /// <summary>
        /// Toggle guesses opened/closed
        /// </summary>
        /// <param name="open"></param>
        public void ToggleGuesses()
        {
            ToggleGuesses(!guessesOpen);
            ToggleGuessSlider();
        }

        /// <summary>
        /// Toggle guesses opened/closed
        /// </summary>
        /// <param name="open"></param>
        /// <param name="sendMessage"></param>
        public void ToggleGuesses(bool open, bool sendMessage = true)
        {
            if (!open)
                ProcessTemporaryGuesses();
            guessesOpen = open;
            if (open)
            {
                if (sendMessage && (Settings.Default.EnableTwitchChatMsgs || Settings.Default.SendChatMsgsViaStreamerBot))
                    CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_GuessesOpenedMessage"));
            }
            else
            {
                if (sendMessage && (Settings.Default.EnableTwitchChatMsgs || Settings.Default.SendChatMsgsViaStreamerBot))
                    CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_GuessesClosedMessage"));

            }
        }

        /// <summary>
        /// Returns the Overlaysettings as JSON string for passing it to JS
        /// </summary>
        /// <returns></returns>
        public string GetOverlaySetting()
        {
            OverlaySettings settings = new()
            {
                Unit = (Units)Settings.Default.OverlayUnit,
                DisplayCorrectLocations = Settings.Default.OverlayDisplayCorrectLocations,
                RoundingDigits = Settings.Default.OverlayRoundingDigits,
                RegionalFlags = Settings.Default.OverlayRegionalFlags,
                UsStateFlags = Settings.Default.UseUsStateFlags,
                UseWrongRegionColors = Settings.Default.OverlayUseWrongRegionColors,
                FontSize = Settings.Default.FontSize,
                FontSizeUnit = Settings.Default.FontSizeUnit,
                PopupShowCoordinates = Settings.Default.OverlayInfoPopupShowCoordinates,
                PopupShowDistance = Settings.Default.OverlayInfoPopupShowDistance,
                PopupShowScore = Settings.Default.OverlayInfoPopupShowScore,
                PopupShowStreak = Settings.Default.OverlayInfoPopupShowStreak,
                PopupShowTime = Settings.Default.OverlayInfoPopupShowTime,
                ScoreboardForeground = Settings.Default.ScoreboardFG,
                ScoreboardForegroundA = Settings.Default.ScoreboardFGA,
                ScoreboardBackground = Settings.Default.ScoreboardBG,
                ScoreboardBackgroundA = Settings.Default.ScoreboardBGA,
                ScrollSpeed = Settings.Default.ScrollSpeed,
                MarkerClustersEnabled = Settings.Default.OverlayMarkerClustersEnabled,
                MaximumMarkerCountForRoundEnd = Settings.Default.OverlayNoOfMarkers,
                MaximumRowCountForAllMarkersDisplay = Settings.Default.OverlayMaxGuessDisplayCount


            };
            return JsonConvert.SerializeObject(settings);
        }


        /// <summary>
        /// Sets and saves the Overlaysettings when they are changed in the overlay
        /// </summary>
        /// <param name="settingsJson"></param>
        public void SetOverlaySetting(string settingsJson)
        {

            OverlaySettings settings = JsonConvert.DeserializeObject<OverlaySettings>(settingsJson);
            Settings.Default.OverlayUnit = (int)settings.Unit;
            Settings.Default.OverlayDisplayCorrectLocations = settings.DisplayCorrectLocations;
            Settings.Default.OverlayRoundingDigits = settings.RoundingDigits;
            Settings.Default.OverlayRegionalFlags = settings.RegionalFlags;
            Settings.Default.UseUsStateFlags = settings.UsStateFlags;
            Settings.Default.OverlayUseWrongRegionColors = settings.UseWrongRegionColors;
            Settings.Default.FontSize = string.IsNullOrWhiteSpace(settings.FontSize) ? "1" : settings.FontSize;
            Settings.Default.FontSizeUnit = string.IsNullOrWhiteSpace(settings.FontSizeUnit) ? "em" : settings.FontSizeUnit;
            Settings.Default.ScoreboardBG = settings.ScoreboardBackground;
            Settings.Default.ScoreboardFG = settings.ScoreboardForeground;
            Settings.Default.ScoreboardFGA = settings.ScoreboardForegroundA;
            Settings.Default.ScoreboardBGA = settings.ScoreboardBackgroundA;
            Settings.Default.ScrollSpeed = settings.ScrollSpeed;
            Settings.Default.OverlayInfoPopupShowCoordinates = settings.PopupShowCoordinates;
            Settings.Default.OverlayInfoPopupShowDistance = settings.PopupShowDistance;
            Settings.Default.OverlayInfoPopupShowScore = settings.PopupShowScore;
            Settings.Default.OverlayInfoPopupShowStreak = settings.PopupShowStreak;
            Settings.Default.OverlayInfoPopupShowTime = settings.PopupShowTime;
            Settings.Default.OverlayMarkerClustersEnabled = settings.MarkerClustersEnabled;
            Settings.Default.OverlayMaxGuessDisplayCount = settings.MaximumRowCountForAllMarkersDisplay;
            Settings.Default.OverlayNoOfMarkers = settings.MaximumMarkerCountForRoundEnd;
            Settings.Default.Save();
        }

        private static void ConnectToObs()
        {
            if (Settings.Default.ObsConnectAtStartUp && !string.IsNullOrEmpty(Settings.Default.ObsIP) && !string.IsNullOrEmpty(Settings.Default.ObsPort))
            {
                try
                {

                    logger.Debug("Connecting to OBS");
                    if (!obsClient.Connect("ws://" + Settings.Default.ObsIP + ":" + Settings.Default.ObsPort, Settings.Default.ObsPassword))
                    {
                        MessageBox.Show("Could not connect to OBS\r\nPlease make sure that IP, port  and password are correct,\rthat OBS and its websocket server are running!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        return;
                    }
                    obsClient.GetScenes();
                }
                catch (Exception ex) { logger.Error(ex.Summarize()); }
            }
        }

        private async void ConnectToStreamerbot()
        {
            if (Settings.Default.StreamerBotConnectAtStartup && !string.IsNullOrEmpty(Settings.Default.StreamerBotIP) && !string.IsNullOrEmpty(Settings.Default.StreamerBotPort))
            {
                try
                {
                    logger.Debug("Connecting to Streamer.Bot");
                    await streamerbotClient.Connect(Settings.Default.StreamerBotIP, Settings.Default.StreamerBotPort, Settings.Default.SendChatActionId, Settings.Default.SendChatActionName, Settings.Default.SendChatMsgsViaStreamerBot, this, Settings.Default.SendJoinMsg);
                    AttributeDiscovery.AddEventHandlers(fromMethodSource: this, toTargetInstance: streamerbotClient);
                    streamerbotClient.GetActions();
                    if (Settings.Default.SendChatMsgsViaStreamerBot)
                        CurrentBot = streamerbotClient;
                }
                catch (Exception ex) { logger.Error(ex.Summarize()); }
            }
        }

        private void commandManagerMenuItem_Click(object sender, EventArgs e)
        {
            ShowCommandManager();
        }

        private void ShowCommandManager()
        {
            Invoke(delegate ()
            {
                using CommandManager manager = new(this);

                manager.ShowDialog();
            }
            );
        }

        private void ShowUserScriptManager()
        {
            Invoke(delegate ()
            {
                using UserScriptManager manager = new(this);

                if (!manager.IsUseable)
                {
                    manager.Close();
                    return;
                }

                manager.ShowDialog();

                if (manager.ListUpdated)
                {
                    DialogResult res = MessageBox.Show(UserScriptUpdateMessage, "Page Refresh Needed", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    switch (res)
                    {
                        case DialogResult.Yes:
                        case DialogResult.OK:
                            {
                                RefreshBrowser();
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            );
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ShowDevTools()
        {
            browser?.ShowDevTools();
        }


        private void checkForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoUpdater.Start(Settings.Default.Link_VersionXML);
        }
        private void reloadExtensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GeneralPurposeUtils.OpenURL(Settings.Default.Link_GeoChatter);
        }

        private void devToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.Default.DebugShowDevTools)
                ShowDevTools();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed on work end.")]
        private void ToolStripMenuItemUploadLogs_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (sender is BackgroundWorker worker)
            {
                worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;
                worker.DoWork -= Worker_DoWork;
                worker.Dispose();
            }
            MessageBox.Show("Logfile successfully uploaded!");
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!guessApiClient.UploadLogViaSignal())
            {
                logger.Error("Failed to upload logs via signal");
            }
            else
            {
                logger.Info("Uploaded logs via signal");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.LastAddress = browser.Address;
            Settings.Default.Save();
            if (ClientDbCache.RunningGame != null)
            {
                SaveAndExit(true);
            }
            guessApiClient?.Disconnect();
            if (Settings.Default.DebugEnableRandomBotGuesses)
                randomBotImageClient?.Dispose();

            browser?.CloseDevTools();
        }

        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleFullscreen();
        }

        private void flagManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using FlagManagerDialog mgr = new(this);
            mgr.ShowDialog();
            if (mgr.HasChanged)
            {
                DialogResult res = MessageBox.Show("Flags were updated... Page needs to be refreshed for changes to take effect. Do you want to refresh the page now?", "Page Refresh Needed", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                switch (res)
                {
                    case DialogResult.Yes:
                    case DialogResult.OK:
                        {
                            SendMapOptionsToMaps(GetMapOptions());
                            RefreshBrowser();
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void scoreFormulatorMenuItem_Click(object sender, EventArgs e)
        {
            using ScoreFormulator scoreFormulator = new(this);
            if (!ScoreFormulator.IsAvailable)
            {
                MessageBox.Show("Score formulator is initializing on the background, close the window and try in a few moments later again.", "Not Initialized", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                DialogResult res = scoreFormulator.ShowDialog();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.DebugUseDevApi = false;
            Settings.Default.Save();
        }

        private void extensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUserScriptManager();
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            SplashScreenHelper.Close();
            //CheckForReloginAndRedirect();
        }

        private void CheckForReloginAndRedirect()
        {
            if (Settings.Default.LoginRequired && Settings.Default.LastLoginVersion != Version)
            {
                LogoutAndRedirect();
            }
        }

        private void LogoutAndRedirect()
        {
            SendGeoGuessrSignOutFromBrowserAndRedirect(GeoGuessrClient.GeoGuessrSignInAddress);
        }

        private void toolStripMenuItemRefresh_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure you want to refresh the page?", "Refreshing the Page", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            switch (res)
            {
                case DialogResult.Yes:
                case DialogResult.OK:
                    {

                        RefreshBrowser();
                        break;
                    }
                default:
                    break;
            }
        }

        private void toolStripMenuItemResetZoom_Click(object sender, EventArgs e)
        {
            browser?.SetZoomLevel(0);
        }
        bool hasFocus;
        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                hasFocus = !(Form.ActiveForm == null);
            }));
        }

        public bool IsDebugEnabled()
        {
            return Settings.Default.EnableDebugLogging;
        }

        private void resetStreamerBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the connection?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                streamerbotClient = new();
                ConnectToStreamerbot();
            }
        }

        private async void ResetApitoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the connection?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                guessApiClient = new();
                await ConnectToGuessApi(true);
            }
        }
    }
}
