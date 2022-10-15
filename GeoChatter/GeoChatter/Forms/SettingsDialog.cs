using GeoChatter.Controls;
using GeoChatter.Helpers;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Model.Attributes;
using GeoChatter.Core.Storage;
using GeoChatter.FormUtils;
using GeoChatter.Integrations;
using GeoChatter.Integrations.Classes;
using GeoChatter.Properties;
using log4net;
using log4net.Core;
using log4net.Layout;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;
using GeoChatter.Model;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Web;
using System.Drawing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Svg;
using System.Windows.Media;
using GeoChatter.Core.Handlers;
using CefSharp.DevTools.Accessibility;

namespace GeoChatter.Forms
{
    /// <summary>
    /// Settings form
    /// </summary>
    [SupportedOSPlatform("windows7.0")]
    public partial class SettingsDialog : Form
    {
        private readonly MainForm parent;
        private readonly StreamerbotClient streamerbotClient;
        private readonly OBSClient obsClient;
        private static readonly ILog logger = LogManager.GetLogger(typeof(SettingsDialog));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="streamerbotClient"></param>
        /// <param name="obsClient"></param>
        public SettingsDialog(MainForm parent, StreamerbotClient streamerbotClient, OBSClient obsClient)
        {
            InitializeComponent();
            this.parent = parent;
            this.streamerbotClient = streamerbotClient;
            
            this.obsClient = obsClient;
            showAdvancedSettings = parent?.ShowAdvancedSettings ?? false;
            HandleAdvancedSettings();
            WindowState = FormWindowState.Minimized;
            WindowState = FormWindowState.Normal;
            // tabControl1.TabPages.Remove(tabPageOverlay);
            // tabControl1.TabPages.Remove(tabGeneral);
            //propertyGrid1.SetLabelColumnWidth1(3);
            this.parent.ResetJSCTRLCheck();
            this.tabControl1.TabPages.Remove(tabDevelopment);
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SaveSettings()
        {
            logger.Debug("Saving settings");

            foreach (Player item in chkListBoxBannedPlayers.Items)
            {
                item.IsBanned = chkListBoxBannedPlayers.GetItemChecked(chkListBoxBannedPlayers.Items.IndexOf(item));
            }
            bool reconnectApi = false;
            bool updateMap = false;
            if (Settings.Default.MOEnableOverlay != chkEnableMapOverlay.Checked ||
                Settings.Default.MOShowBorders != chkShowBorders.Checked ||
                Settings.Default.MOShowFlags != chkShowFlags.Checked ||
                Settings.Default.MOEnableTempGuesses != checkBoxEnableTempGuesses.Checked )
            {
                updateMap = true;
            }

            Settings.Default.ShortcutsSettingsModifiers = scrSettings.Modifiers;
            Settings.Default.ShortcutsSettingsKeycode = scrSettings.KeyCode;

            Settings.Default.ShortcutsMenuModifiers = scrMenu.Modifiers;
            Settings.Default.ShortcutsMenuKey = scrMenu.KeyCode;

            Settings.Default.ShortcutsFullscreenModifiers = scrFullscreen.Modifiers;
            Settings.Default.ShortcutsFullscreenKey = scrFullscreen.KeyCode;
            if (tabControl1.TabPages.Contains(tabDevelopment))
            {
                bool reconnectNeeded = Settings.Default.DebugUseDevApi != chkDEVUseDevApi.Checked;
                Settings.Default.DebugUseDevApi = chkDEVUseDevApi.Checked;
                Settings.Default.DebugShowDevTools = chkDEVShowDevTools.Checked;
                Settings.Default.DebugEnableRandomBotGuesses = chkDEVEnableRandomBotGuess.Checked;
                if(reconnectNeeded)
                    parent.ConnectToGuessApi(true, true);
            }


            Settings.Default.CustomRandomGuessingEnabled = allowCustomRandomGuesses.Checked;

            Settings.Default.TwitchChannel = txtGeneralChannelName.Text;
            
            Settings.Default.oauthToken = txtGeneralOauthToken.Text;
            Settings.Default.autoUpdate = checkBoxAutoUpdate.Checked;
            Settings.Default.AutoBanCGTrolls = chkAutoBanCGTrolls.Checked;
            Settings.Default.SendGameEndedMsg = chkMsgsGameEnded.Checked;
            Settings.Default.SendConfirmGuessMsg = chkMsgsGuessReceived.Checked;
            Settings.Default.SendJoinMsg = chkMsgsJoinChannel.Checked;
            Settings.Default.SendRoundEndMsg = chkMsgsRoundEnd.Checked;
            Settings.Default.SendRoundStartMsg = chkMsgsRoundStarted.Checked;
            Settings.Default.SendGameStartMsg = chkMsgsStartGame.Checked;
            Settings.Default.SendFlagSelected = chkFlagMessages.Checked;
            Settings.Default.SendColorSelected = chkColorMessage.Checked;
            Settings.Default.LabelPath = textBoxLabelPath.Text;

            Settings.Default.RoundWinnerLabel = checkBoxEventWriteRoundWinner.Checked;
            Settings.Default.RoundSecondLabel = checkBoxEventWriteRoundSecond.Checked;
            Settings.Default.RoundThirdLabel = checkBoxEventWriteRoundThird.Checked;
            Settings.Default.RoundHighscoreLabel = checkBoxEventWriteRoundHighscore.Checked;

            Settings.Default.GameWinnerLabel = checkBoxEventWriteGameWinner.Checked;
            Settings.Default.GameSecondLabel = checkBoxEventWriteGameSecond.Checked;
            Settings.Default.GameThirdLabel = checkBoxEventWriteGameThird.Checked;
            Settings.Default.GameHighscoreLabel = checkBoxEventWriteGameHighscore.Checked;

            Settings.Default.SpecialDistanceLabel = checkBoxSpecialDistanceLabel.Checked;
            Settings.Default.SpecialDistanceFrom = txtSpecialDistanceFrom.Text?.ParseAsDouble(-1) ?? -1;
            Settings.Default.SpecialScoreLabel = checkBoxSpecialScoreLabel.Checked;
            double scoreFrom = txtSpecialScoreFrom.Text?.ParseAsDouble(-1) ?? -1;
            double scoreTo = txtSpecialScoreTo.Text?.ParseAsDouble(-1) ?? -1;
            if (scoreTo > 0 && scoreTo < scoreFrom)
            {
                scoreTo = 0;
            }

            Settings.Default.SpecialScoreFrom = scoreFrom;
            Settings.Default.SpecialScoreTo = scoreTo;
            double disanceFrom = txtSpecialDistanceFrom.Text?.ParseAsDouble(-1) ?? -1;
            double distanceTo = txtSpecialDistanceTo.Text?.ParseAsDouble(-1) ?? -1;
            if (distanceTo > 0 && distanceTo < disanceFrom)
            {
                distanceTo = 0;
            }

            Settings.Default.SpecialDistanceFrom = disanceFrom;
            Settings.Default.SpecialDistanceTo = distanceTo;

          
            Settings.Default.StreamerBotIP = txtStreamerBotIP.Text;
            Settings.Default.StreamerBotPort = txtStreamerBotPort.Text;
            Settings.Default.StreamerBotConnectAtStartup = chkStreamerBotConnectAtStartup.Checked;

            Settings.Default.OverlayDisplayCorrectLocations = checkBoxOverlayDisplayCorrectLocations.Checked;
            Settings.Default.OverlayMarkerClustersEnabled = checkBoxMarkerClustersEnabled.Checked;
            Settings.Default.OverlayRegionalFlags = checkBoxOverlayRegionalFlags.Checked;
            Settings.Default.OverlayUseWrongRegionColors = checkBoxOverlayUseWrongRegionColors.Checked;
            Settings.Default.FontSize = OverlayFontSizetextBox1.Text.CanBeDouble() ? OverlayFontSizetextBox1.Text : "1";
            Settings.Default.FontSizeUnit = OverlayFontSizeUnitcomboBox1.SelectedItem.ToStringDefault();
            Settings.Default.OverlayRoundingDigits = decimal.ToInt32(numericRoundingDigit.Value);
            Settings.Default.OverlayNoOfMarkers = decimal.ToInt32(numMaxMarkerCount.Value);
            Settings.Default.OverlayMaxGuessDisplayCount = decimal.ToInt32(numMaxGuessDisplayCount.Value);
            Settings.Default.OverlayUnit = comboBoxOverlayUnits.SelectedIndex;
            Settings.Default.UseUsStateFlags = chkUseUsStateFlags.Checked;
            Settings.Default.ScoreboardBG = "#"
                + ScoreboardBGDisplaybutton.BackColor.R.ToHex()
                + ScoreboardBGDisplaybutton.BackColor.G.ToHex()
                + ScoreboardBGDisplaybutton.BackColor.B.ToHex();

            Settings.Default.ScoreboardBGA = (byte)ScoreboardBGAnumericUpDown2.Value;

            Settings.Default.ScoreboardFG = "#"
                + ScoreboardFGDisplaybutton.BackColor.R.ToHex()
                + ScoreboardFGDisplaybutton.BackColor.G.ToHex()
                + ScoreboardFGDisplaybutton.BackColor.B.ToHex();

            Settings.Default.ScoreboardFGA = (byte)ScoreboardFGAnumericUpDown2.Value;

            Settings.Default.ScrollSpeed = (int)ScoreboardScrollSpeednumericUpDown1.Value;
            if (chkStreamerBotConnectAtStartup.Checked && streamerbotClient.IsRunning())
            {
                Settings.Default.SpecialDistanceAction = checkBoxSpecialDistanceAction.Checked;
                Settings.Default.SpecialDistanceActionID = ctrlBotSpecialDistance.ActionGuid;
                Settings.Default.SpecialDistanceActionName = ctrlBotSpecialDistance.ActionName;

                Settings.Default.SpecialScoreAction = checkBoxSpecialScoreAction.Checked;
                Settings.Default.SpecialScoreActionID = ctrlBotSpecialScore.ActionGuid;
                Settings.Default.SpecialScoreActionName = ctrlBotSpecialScore.ActionName;

                Settings.Default.RoundStartAction = checkBoxRoundTimerExecuteStreamerBotAction.Checked;
                Settings.Default.RoundStartActionID = ctrlBotRoundStart.ActionGuid; 
                Settings.Default.RoundStartActionName = ctrlBotRoundStart.ActionName; 


                Settings.Default.SendChatMsgsViaStreamerBot = checkBoxSendChatMsgsViaStreamerBot.Checked;
                Settings.Default.SendChatActionId = ctrlBotChatMessages.ActionGuid;
                Settings.Default.SendChatActionName = ctrlBotChatMessages.ActionName;

                Settings.Default.RoundEndAction = chkBotRoundEndExecute.Checked;
                Settings.Default.RoundEndActionID = ctrlBotRoundEnd.ActionGuid;
                Settings.Default.RoundEndActionName = ctrlBotRoundEnd.ActionName;

                Settings.Default.GameEndAction = chkBotGameEndExecute.Checked;
                Settings.Default.GameEndActionID = ctrlBotGameEnd.ActionGuid;
                Settings.Default.GameEndActionName = ctrlBotGameEnd.ActionName;
            }
            Settings.Default.OverlayInfoPopupShowStreak = chkGuessInfoStreak.Checked;
            Settings.Default.OverlayInfoPopupShowCoordinates = chkGuessInfoCoordinates.Checked;
            Settings.Default.OverlayInfoPopupShowDistance = chkGuessInfoDistance.Checked;
            Settings.Default.OverlayInfoPopupShowScore = chkGuessInfoScore.Checked;
            Settings.Default.OverlayInfoPopupShowTime = chkGuessInfoTime.Checked;

            Settings.Default.ObsPassword = txtObsPassword.Text;
            Settings.Default.ObsIP = txtObsIp.Text;
            Settings.Default.ObsPort = txtObsPort.Text;
            Settings.Default.ObsConnectAtStartUp = chkObsConnectAtStartup.Checked;
            if (chkObsConnectAtStartup.Checked && obsClient.IsAlive())
            {
                Settings.Default.ObsRoundStartExecute = chkRoundTimerOBS.Checked;
                Settings.Default.ObsRoundStartScene = comboRoundTimerObsScene.SelectedValue?.ToStringDefault();
                Settings.Default.ObsRoundStartSource = (comboRoundTimerObsSource.SelectedValue == null) ? 0 : comboRoundTimerObsSource.SelectedValue.ToStringDefault().ParseAsInt();
                Settings.Default.ObsRoundStartAction = comboRoundTimerObsAction.SelectedItem?.ToStringDefault();

                Settings.Default.ObsRoundEndExecute = chkOBSRoundEndExecute.Checked;
                Settings.Default.ObsRoundEndScene = comboOBSRoundEndScene.SelectedValue?.ToStringDefault();
                Settings.Default.ObsRoundEndSource = (comboOBSRoundEndSource.SelectedValue == null) ? 0 : comboOBSRoundEndSource.SelectedValue.ToStringDefault().ParseAsInt();
                Settings.Default.ObsRoundEndAction = comboOBSRoundEndAction.SelectedItem?.ToStringDefault();

                Settings.Default.ObsGameEndExecute = chkOBSGameEndExecute.Checked;
                Settings.Default.ObsGameEndScene = comboOBSGameEndScene.SelectedValue?.ToStringDefault();
                Settings.Default.ObsGameEndSource = (comboOBSGameEndSource.SelectedValue == null) ? 0 : comboOBSGameEndSource.SelectedValue.ToStringDefault().ParseAsInt();
                Settings.Default.ObsGameEndAction = comboOBSGameEndAction.SelectedItem?.ToStringDefault();


                Settings.Default.ObsSpecialDistanceExecute = chkSpecialDistanceObs.Checked;
                Settings.Default.ObsSpecialDistanceScene = comboSpecialDistanceObsScene.SelectedValue?.ToStringDefault();
                Settings.Default.ObsSpecialDistanceSource = (comboSpecialDistanceObsSource.SelectedValue == null) ? 0 : comboSpecialDistanceObsSource.SelectedValue.ToStringDefault().ParseAsInt();
                Settings.Default.ObsSpecialDistanceAction = comboSpecialDistanceObsAction.SelectedItem?.ToStringDefault();
                Settings.Default.ObsSpecialScoreExecute = chkSpecialScoreObs.Checked;
                Settings.Default.ObsSpecialScoreScene = comboSpecialScoreObsScene.SelectedValue?.ToStringDefault();
                Settings.Default.ObsSpecialScoreSource = (comboSpecialScoreObsSource.SelectedValue == null) ? 0 : comboSpecialScoreObsSource.SelectedValue.ToStringDefault().ParseAsInt();
                Settings.Default.ObsSpecialScoreAction = comboSpecialScoreObsAction.SelectedItem?.ToStringDefault();
            }
            var player = ClientDbCache.Instance.Streamer;
            if(player != null)
            {
                player.DisplayName = txtStreamerDisplayname.Text;

                try
                {
                    if (streamerFlag.SelectedIndex >= 0)
                    {
                        player.PlayerFlag = FlagCodeFromComboboxItem(streamerFlag.Items[streamerFlag.SelectedIndex].ToStringDefault());
                        player.PlayerFlagName = Flags[player.PlayerFlag.ToUpperInvariant()];
                    }

                    player.Color = "#"
                        + streamerColor.BackColor.R.ToHex()
                        + streamerColor.BackColor.G.ToHex()
                        + streamerColor.BackColor.B.ToHex();
                }
                catch (Exception err)
                {
                    logger.Error(err);
                }
            }

            Settings.Default.EnableDebugLogging = chkEnableDebugLogging.Checked;
            if (Settings.Default.EnableDebugLogging)
            {
                log4net.Appender.RollingFileAppender appender = new()
                {
                    AppendToFile = true,
                    File = "debug.log",
                    RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Size,
                    MaximumFileSize = "10MB",
                    MaxSizeRollBackups = 10,
                    Threshold = Level.Debug,
                    Layout = new PatternLayout("%date [%2thread] %-5level - %message%newline"),
                    Name = "DebugAppender"
                };

                bool exists = false;
                foreach (log4net.Appender.IAppender append in ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Appenders)
                {
                    if (append.Name == "DebugAppender")
                    { exists = true; break; }
                }

                if (!exists)
                {
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.AddAppender(appender);
                }
            }
            else
            {
                ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.RemoveAppender("DebugAppender");
            }
            ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
            Settings.Default.AllowSameLocationGuess = chkAllowSameLocationGuess.Checked;
            Settings.Default.SendDoubleGuessMsg = chkSendDoubleGuessMsg.Checked;
            Settings.Default.SendSameGuessMessage = chkSendSameGuessMessage.Checked;

            Settings.Default.ResetStreakOnSkippedRound = chkResetStreakOnSkippedRound.Checked;
            Settings.Default.UseEnglishCountryNames = chkUseEnglishCountryNames.Checked;



            Settings.Default.MOEnableOverlay = chkEnableMapOverlay.Checked;
            Settings.Default.MOShowBorders = chkShowBorders.Checked;
            Settings.Default.MOShowFlags = chkShowFlags.Checked;
            Settings.Default.MOEnableTempGuesses = checkBoxEnableTempGuesses.Checked;

            Settings.Default.AutoBanUsers = chkAutoBan.Checked;
            Settings.Default.CheckStreamerGuessForSpecialValues = chkCheckStreamer.Checked;
            if (propertyGrid1.SelectedObject is DictionaryPropertyGridAdapter values)
            {
                Dictionary<string, string> dic = values.Dictionary as Dictionary<string, string>;
                foreach (KeyValuePair<string, string> defaultValue in dic)
                {
                    string key = defaultValue.Key;
                    string value = defaultValue.Value;
                    ChatMessage msg = LanguageStrings.Strings.FirstOrDefault(c => c.Name == key);
                    if (msg != null)
                    {
                        msg.Message = value;
                    }
                }
                LanguageStrings.Save();
            }
            if (string.IsNullOrEmpty(Settings.Default.oauthToken))
                Settings.Default.EnableTwitchChatMsgs = false;
            else
                Settings.Default.EnableTwitchChatMsgs = checkBoxEnableChatMsgs.Checked;
            Settings.Default.Save();
            LanguageStrings.Initialize(Settings.Default);
            parent?.SendSettingsUpdateToJS();

            if (updateMap)
            {
                parent.SendMapOptionsToMaps(new MapOptions()
                {
                    ShowBorders = chkShowBorders.Checked,
                    ShowFlags = chkShowFlags.Checked,
                    ShowStreamOverlay = chkEnableMapOverlay.Checked,
                    
                    Streamer = txtGeneralChannelName.Text,
                    InstalledFlagPacks = "{" + string.Join(",", FlagPackHelper.FlagPacks.Select(f => $"\"{f.Name}\": \"https://service.geochatter.tv/flagpacks/{f.Name.ToUpperInvariant()}.zip\"")) + "}",
                    TemporaryGuesses = checkBoxEnableTempGuesses.Checked,
                });
            }

            if (reconnectApi)
            {
                parent.ReconnectToGuessApi();
            }

            logger.Debug("Settings saved");
        }

        private TableOptions GetTableOptionsClassFromTreeView()
        {
            TableOptions to = new();
            try
            {
                TreeNode rootNode = treeView1.Nodes[0];
                foreach (TreeNode gameNode in rootNode.Nodes)
                {
                    GameOptions gameOptions = new() { Mode = gameNode.Text };
                    foreach (TreeNode stageNode in gameNode.Nodes)
                    {
                        StageOptions stageOptions = new() { Stage = stageNode.Text };
                        foreach (TreeNode colNode in stageNode.Nodes)
                        {
                            if (colNode.Tag is TableColumn col)
                            {
                                stageOptions.Columns.Add(col);
                            }
                        }
                        gameOptions.Stages.Add(stageOptions);
                    }
                    to.Options.Add(gameOptions);
                }
            }
            catch (Exception ex) { logger.Error(ex.Summarize()); }
            return to;
        }

        [DiscoverableEvent]
        private void ChannelJoined(object sender, ChannelJoinedEventArgs args)
        {

            MessageBox.Show("Twitch bot connection successful!", "Connection successful!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            EnableTwitchButton();
        }

        [DiscoverableEvent]
        private void ChannelNotJoined(object sender, ChannelNotJoinedEventArgs args)
        {
            MessageBox.Show("Couldn't connect Twitch bot. Please check your settings.", "Connection failed!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            EnableTwitchButton();
        }

        private delegate void EnableTwitchButtonCallback();

        private void EnableTwitchButton()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (reconnectTwitchBotButton.InvokeRequired)
            {
                EnableTwitchButtonCallback d = new(EnableTwitchButton);
                Invoke(d);
            }
            else
            {
                reconnectTwitchBotButton.Enabled = true;
                
            }
            
        }

        private delegate void EnableSaveButtonCallback();

        private void EnableSaveButton()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (btnSave.InvokeRequired)
            {
                EnableSaveButtonCallback d = new(EnableSaveButton);
                Invoke(d);
            }
            else
            {
                btnSave.Enabled = true;

            }
        }
        private delegate void EnableApplyButtonCallback();

        private void EnableApplyButton()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (btnApply.InvokeRequired)
            {
                EnableApplyButtonCallback d = new(EnableApplyButton);
                Invoke(d);
            }
            else
            {
                btnApply.Enabled = true;

            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private static void ControlEnabledState(bool state, params Control[] controls)
        {
            controls.ToList().ForEach(control => control.Enabled = state);
        }

        private void btnSelectLabelFolder_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            parent.Invoke(() =>
            {
                DialogResult res = folderBrowserLabelPath.ShowDialog();
                if (res == DialogResult.OK)
                {



                    path = folderBrowserLabelPath.SelectedPath;
                }
            });
            bool exists = Directory.Exists(folderBrowserLabelPath.SelectedPath);
            if (!exists)
            {
                MessageBox.Show("The specified path does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                textBoxLabelPath.Text = path;
                try
                {
                    FileStream fs = File.Create(path + "\\RoundWinner.txt");
                    fs.Close();
                    fs = File.Create(path + "\\RoundSecond.txt");
                    fs.Close();
                    fs = File.Create(path + "\\RoundThird.txt");
                    fs.Close();
                    fs = File.Create(path + "\\GameWinner.txt");
                    fs.Close();
                    fs = File.Create(path + "\\GameSecond.txt");
                    fs.Close();
                    fs = File.Create(path + "\\GameThird.txt");
                    fs.Close();
                    fs = File.Create(path + "\\RoundHighScore.txt");
                    fs.Close();
                    fs = File.Create(path + "\\GameHighScore.txt");
                    fs.Close();
                    fs = File.Create(path + "\\SpecialDistance.txt");
                    fs.Close();
                    fs = File.Create(path + "\\SpecialScore.txt");
                    fs.Close();
                    fs.Dispose();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Summarize());
                }
            }
            ControlEnabledState(exists,
                    checkBoxEventWriteGameHighscore,
                    checkBoxEventWriteGameWinner,
                    checkBoxEventWriteGameSecond,
                    checkBoxEventWriteGameThird,
                    checkBoxEventWriteRoundHighscore,
                    checkBoxEventWriteRoundWinner,
                    checkBoxEventWriteRoundSecond,
                    checkBoxEventWriteRoundThird,
                    checkBoxSpecialScoreLabel,
                    checkBoxSpecialDistanceLabel);

        }


        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            logger.Debug("Loading settings");
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                Text = "GeoChatter v" + version;

                LoadSettings();

                foreach (Player player in ClientDbCache.Instance.Players.OrderBy(p => p.DisplayName).ThenBy(p => p.PlayerName))
                {
                    if (player.PlatformId == GCResourceRequestHandler.ClientUserID && player.SourcePlatform == Model.Enums.Platforms.GeoGuessr)
                    {
                        continue;
                    }

                    chkListBoxBannedPlayers.Items.Add(player, player.IsBanned);

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
            logger.Debug("setting up streamer.bot");
            if (chkStreamerBotConnectAtStartup.Checked && streamerbotClient.IsRunning())
            {
            //    btnSave.Enabled = btnApply.Enabled = false;
              //  streamerbotClient.GetActions();
            }

            SetUpStreambotInputs();
            logger.Debug("Setting up OBS");
            if (chkObsConnectAtStartup.Checked && obsClient.IsAlive())
            {
                obsClient.GetScenes();
                GetAndSetOBSScenes();
            }

            SetUpObsInputs();
            logger.Debug("Finished loading settings");
        }

        private Dictionary<string, string> Flags { get; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        private static string FlagCodeFromComboboxItem(string item)
        {
            string[] spl = item.Split(' ');
            if (spl.Length == 0)
            {
                return string.Empty;
            }
            return spl[0].Trim().ToLowerInvariant();
        }

        private static string FlagNameAndCodeToComboboxItem(string code, string name)
        {
            return $"{code} {name}".Trim();
        }

        private void SetFlagDisplay(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return;
                }

                code = code.ToLowerInvariant();
                string svg = GCSchemeHandler.ResourceDictionary
                    .FirstOrDefault(kv => kv.Key.EndsWithDefault($"/{code}.svg")).Value.Value;
                if (string.IsNullOrEmpty(svg))
                {
                    var pack = FlagPackHelper.FlagPacks.FirstOrDefault(p => !string.IsNullOrEmpty(p.Flags.FirstOrDefault(f => f.Value.ToLowerInvariant() == code).Value));
                    svg = File.ReadAllText($"Styles/flags/{pack.Name}/{code}.svg");
                }

                if (!string.IsNullOrEmpty(svg))
                {
                    streamerFlagDisplay.BackgroundImageLayout = ImageLayout.Zoom;
                    SvgDocument img = SvgDocument.FromSvg<SvgDocument>(svg);
                    Bitmap bit = img.Draw();
                    streamerFlagDisplay.BackgroundImage = bit;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        private void LoadSettings()
        {
            Dictionary<string, string> settings = new();
            foreach (ChatMessage msg in LanguageStrings.Strings)
            {
                settings.Add(msg.Name, msg.Message);
            }

            logger.Debug("Loading flag name and codes");
            var mapped = CountryHelper.MappedNames;

            List<string> uniqmapped = mapped.Values.Map(m => m.ToLowerInvariant()).Distinct().ToList();
            uniqmapped.AddRange(CountryHelper.FlagCodes);
            uniqmapped = uniqmapped.Distinct().ToList();

            List<string> comboitems = new List<string>();
            foreach (string code in uniqmapped)
            {
                string c = code;
                if (!CountryHelper.CheckFlagCode(ref c, out string name, mapped) || string.IsNullOrEmpty(name) || (name == Country.UnknownCountryName && c != Country.UnknownCountryCode))
                {
                    name = FlagPackHelper.MappedFlagPackNames.FirstOrDefault(f => f.Value.ToLowerInvariant() == code).Key;
                }
                Flags.Add(c, name);
                comboitems.Add(FlagNameAndCodeToComboboxItem(c, name));
            }
            comboitems.Sort();
            comboitems.ForEach(c => streamerFlag.Items.Add(c));

            logger.Debug("Loaded flag name and codes");

            propertyGrid1.SelectedObject = new DictionaryPropertyGridAdapter(settings);
            checkBoxEnableChatMsgs.Checked = Settings.Default.EnableTwitchChatMsgs && !string.IsNullOrEmpty(Settings.Default.oauthToken);
            btnForgetTwitch.Enabled = !string.IsNullOrEmpty(Settings.Default.oauthToken);
            btnAuthorizeAutomatically.Enabled = btnAuthorizeManually.Enabled = string.IsNullOrEmpty(Settings.Default.oauthToken);

            var player = ClientDbCache.Instance.Streamer;
            if (player == null)
            {
                player = ClientDbCache.Instance.GetPlayerByIDOrName(GCResourceRequestHandler.ClientUserID, GCResourceRequestHandler.ClientGeoGuessrName, profilePicUrl: GCResourceRequestHandler.ClientGeoGuessrPic, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), isStreamer: true);
                if (player == null)
                {
                    MessageBox.Show("Could not load your player record. Please make sure you're logged into GeoGuessr!");
                    DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            if (player != null)
            {
                try
                {
                    streamerColor.BackColor = ColorTranslator.FromHtml(string.IsNullOrEmpty(player.Color) ? "#FFFFFF" : player.Color);

                    streamerFlag.SelectedItem = string.IsNullOrEmpty(player.PlayerFlag)
                    ? null
                    : FlagNameAndCodeToComboboxItem(player.PlayerFlag.ToUpperInvariant(), Flags[player.PlayerFlag.ToUpperInvariant()]);

                    SetFlagDisplay(player.PlayerFlag?.ToUpperInvariant());
                }
                catch (Exception err)
                {
                    player.PlayerFlag = string.Empty;
                    player.PlayerFlagName = string.Empty;
                    logger.Error(err.Summarize());
                }
            }


            allowCustomRandomGuesses.Checked = Settings.Default.CustomRandomGuessingEnabled;

            txtGeneralChannelName.Text = Settings.Default.TwitchChannel;
            
            txtGeneralOauthToken.Text = Settings.Default.oauthToken;
            checkBoxAutoUpdate.Checked = Settings.Default.autoUpdate;
            chkMsgsGameEnded.Checked = Settings.Default.SendGameEndedMsg;
            chkColorMessage.Checked = Settings.Default.SendColorSelected;
            chkFlagMessages.Checked = Settings.Default.SendFlagSelected;
            chkMsgsGuessReceived.Checked = Settings.Default.SendConfirmGuessMsg;
            chkMsgsJoinChannel.Checked = Settings.Default.SendJoinMsg;
            chkMsgsRoundEnd.Checked = Settings.Default.SendRoundEndMsg;
            chkMsgsRoundStarted.Checked = Settings.Default.SendRoundStartMsg;
            chkMsgsStartGame.Checked = Settings.Default.SendGameStartMsg;
            textBoxLabelPath.Text = Settings.Default.LabelPath;

            checkBoxEventWriteRoundWinner.Checked = Settings.Default.RoundWinnerLabel;
            checkBoxEventWriteRoundSecond.Checked = Settings.Default.RoundSecondLabel;
            checkBoxEventWriteRoundThird.Checked = Settings.Default.RoundThirdLabel;
            checkBoxEventWriteRoundHighscore.Checked = Settings.Default.RoundHighscoreLabel;

            checkBoxEventWriteGameWinner.Checked = Settings.Default.GameWinnerLabel;
            checkBoxEventWriteGameSecond.Checked = Settings.Default.GameSecondLabel;
            checkBoxEventWriteGameThird.Checked = Settings.Default.GameThirdLabel;
            checkBoxEventWriteGameHighscore.Checked = Settings.Default.GameHighscoreLabel;
            chkEnableMapOverlay.Checked = Settings.Default.MOEnableOverlay;
            chkShowFlags.Checked = Settings.Default.MOShowFlags;
            chkShowBorders.Checked = Settings.Default.MOShowBorders;
            checkBoxEnableTempGuesses.Checked = Settings.Default.MOEnableTempGuesses;
            checkBoxSpecialDistanceLabel.Checked = Settings.Default.SpecialDistanceLabel;
            txtSpecialDistanceFrom.Text = Settings.Default.SpecialDistanceFrom.ToStringDefault();
            txtSpecialDistanceTo.Text = Settings.Default.SpecialDistanceTo.ToStringDefault();
            chkSpecialDistanceRange.Checked = Settings.Default.SpecialDistanceTo > 0;
            checkBoxSpecialScoreLabel.Checked = Settings.Default.SpecialScoreLabel;
            txtSpecialScoreFrom.Text = Settings.Default.SpecialScoreFrom.ToStringDefault();
            txtSpecialScoreTo.Text = Settings.Default.SpecialScoreTo.ToStringDefault();
            chkSpecialScoreRange.Checked = Settings.Default.SpecialScoreTo > 0;
            comboBoxOverlayUnits.SelectedIndex = Settings.Default.OverlayUnit;
            chkCheckStreamer.Checked = Settings.Default.CheckStreamerGuessForSpecialValues;
            OverlayFontSizetextBox1.Text = Settings.Default.FontSize;
            OverlayFontSizeUnitcomboBox1.SelectedItem = Settings.Default.FontSizeUnit;

            checkBoxOverlayDisplayCorrectLocations.Checked = Settings.Default.OverlayDisplayCorrectLocations;
            checkBoxMarkerClustersEnabled.Checked = Settings.Default.OverlayMarkerClustersEnabled;
            checkBoxOverlayRegionalFlags.Checked = Settings.Default.OverlayRegionalFlags;
            checkBoxOverlayUseWrongRegionColors.Checked = Settings.Default.OverlayUseWrongRegionColors;
            numericRoundingDigit.Value = Settings.Default.OverlayRoundingDigits;
            comboBoxOverlayUnits.SelectedIndex = Settings.Default.OverlayUnit;

            ScoreboardBGDisplaybutton.BackColor = ColorTranslator.FromHtml(Settings.Default.ScoreboardBG);
            ScoreboardFGDisplaybutton.BackColor = ColorTranslator.FromHtml(Settings.Default.ScoreboardFG);
            ScoreboardBGAnumericUpDown2.Value = Settings.Default.ScoreboardBGA;
            ScoreboardFGAnumericUpDown2.Value = Settings.Default.ScoreboardFGA;
            chkUseUsStateFlags.Checked = Settings.Default.UseUsStateFlags;
            ScoreboardScrollSpeednumericUpDown1.Value = Settings.Default.ScrollSpeed;
            chkAutoBan.Checked = Settings.Default.AutoBanUsers;
            chkGuessInfoStreak.Checked = Settings.Default.OverlayInfoPopupShowStreak;
            chkGuessInfoCoordinates.Checked = Settings.Default.OverlayInfoPopupShowCoordinates;
            chkGuessInfoDistance.Checked = Settings.Default.OverlayInfoPopupShowDistance;
            chkGuessInfoScore.Checked = Settings.Default.OverlayInfoPopupShowScore;
            chkGuessInfoTime.Checked = Settings.Default.OverlayInfoPopupShowTime;
            numMaxMarkerCount.Value = Settings.Default.OverlayNoOfMarkers;
            numMaxGuessDisplayCount.Value = Settings.Default.OverlayMaxGuessDisplayCount;
            chkEnableDebugLogging.Checked = Settings.Default.EnableDebugLogging;
            if (Settings.Default.EnableDebugLogging)
            {
                chkEnableDebugLogging.Visible = true;
            }
            //TableOptionsClass to = TableOptionsClass.Load();
            //BindTableOptionsToTreeview(to);
            Player streamer = ClientDbCache.Instance.Streamer;

            if (streamer == null)
                streamer = ClientDbCache.Instance.GetPlayerByIDOrName(GCResourceRequestHandler.ClientUserID, GCResourceRequestHandler.ClientGeoGuessrName, profilePicUrl: GCResourceRequestHandler.ClientGeoGuessrPic, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), isStreamer: true);
            if(streamer != null)
                txtStreamerDisplayname.Text = streamer.DisplayName;
            this.ctrlBotRoundStart.SetAction(streamerbotClient, Settings.Default.RoundStartActionName, Settings.Default.RoundStartActionID);
            this.ctrlBotRoundEnd.SetAction(streamerbotClient, Settings.Default.RoundEndActionName, Settings.Default.RoundEndActionID);
            this.ctrlBotGameEnd.SetAction(streamerbotClient, Settings.Default.GameEndActionName, Settings.Default.GameEndActionID);
            this.ctrlBotSpecialDistance.SetAction(streamerbotClient, Settings.Default.SpecialDistanceActionName, Settings.Default.SpecialDistanceActionID);
            this.ctrlBotSpecialScore.SetAction(streamerbotClient, Settings.Default.SpecialScoreActionName, Settings.Default.SpecialScoreActionID);
            this.ctrlBotChatMessages.SetAction(streamerbotClient, Settings.Default.SendChatActionName, Settings.Default.SendChatActionId);
            //SetStreamerBotActions();
            checkBoxSpecialDistanceAction.Checked = Settings.Default.SpecialDistanceAction;
            checkBoxSpecialScoreAction.Checked = Settings.Default.SpecialScoreAction;
            checkBoxRoundTimerExecuteStreamerBotAction.Checked = Settings.Default.RoundStartAction;
            chkBotRoundEndExecute.Checked = Settings.Default.RoundEndAction;
            chkBotGameEndExecute.Checked = Settings.Default.GameEndAction;
            chkAutoBanCGTrolls.Checked = Settings.Default.AutoBanCGTrolls;

            txtStreamerBotIP.Text = Settings.Default.StreamerBotIP;
            txtStreamerBotPort.Text = Settings.Default.StreamerBotPort;
            chkStreamerBotConnectAtStartup.Checked = Settings.Default.StreamerBotConnectAtStartup;

            txtObsPassword.Text = Settings.Default.ObsPassword;
            txtObsIp.Text = Settings.Default.ObsIP;
            txtObsPort.Text = Settings.Default.ObsPort;
            chkObsConnectAtStartup.Checked = Settings.Default.ObsConnectAtStartUp;

            chkRoundTimerOBS.Checked = Settings.Default.ObsRoundStartExecute;
            comboRoundTimerObsAction.SelectedItem = Settings.Default.ObsRoundStartAction;
            chkSpecialDistanceObs.Checked = Settings.Default.ObsSpecialDistanceExecute;
            comboSpecialDistanceObsAction.SelectedItem = Settings.Default.ObsSpecialDistanceAction;
            chkSpecialScoreObs.Checked = Settings.Default.ObsSpecialScoreExecute;
            comboSpecialScoreObsAction.SelectedItem = Settings.Default.ObsSpecialScoreAction;
            chkSpecialScoreObs.Checked = Settings.Default.ObsSpecialScoreExecute;
            comboOBSRoundEndAction.SelectedItem = Settings.Default.ObsSpecialScoreAction;
            chkOBSRoundEndExecute.Checked = Settings.Default.ObsRoundEndExecute;
            comboOBSGameEndAction.SelectedItem = Settings.Default.ObsSpecialScoreAction;
            chkOBSGameEndExecute.Checked = Settings.Default.ObsGameEndExecute;
            checkBoxSendChatMsgsViaStreamerBot.Checked = Settings.Default.SendChatMsgsViaStreamerBot;
            chkSendDoubleGuessMsg.Checked = Settings.Default.SendDoubleGuessMsg;
            chkSendSameGuessMessage.Checked = Settings.Default.SendSameGuessMessage;
            chkAllowSameLocationGuess.Checked = Settings.Default.AllowSameLocationGuess;

            chkResetStreakOnSkippedRound.Checked = Settings.Default.ResetStreakOnSkippedRound;
            chkUseEnglishCountryNames.Checked = Settings.Default.UseEnglishCountryNames;
            scrSettings.SetShortcut(Settings.Default.ShortcutsSettingsKeycode, Settings.Default.ShortcutsSettingsModifiers);
            scrMenu.SetShortcut(Settings.Default.ShortcutsMenuKey, Settings.Default.ShortcutsMenuModifiers);
            scrFullscreen.SetShortcut(Settings.Default.ShortcutsFullscreenKey, Settings.Default.ShortcutsFullscreenModifiers);
            SetTwitchInputs();
        }
        List<StreamerbotAction> roundStartList, scoreList,distanceList, roundendList, gameendList, chatList;
       
        
        
        
        //private void SetStreamerBotActions()
        //{
        //    if(streamerbotClient.IsRunning() && !streamerbotClient.Actions.Any())
        //    {
        //        streamerbotClient.GetActions();
        //        Thread.Sleep(10);
        //    }
        //    if (streamerbotClient.Actions.Any())
        //    {
        //        chatList = new List<StreamerbotAction>(streamerbotClient.Actions).OrderBy(a => a.name).ToList();
        //        comboBoxStreamerBotChatAction.DataSource = chatList;
        //        comboBoxStreamerBotChatAction.DisplayMember = "name";
        //        comboBoxStreamerBotChatAction.ValueMember = "id";
        //        logger.Debug("setting chat msgs actions");

        //        if (!string.IsNullOrEmpty(Settings.Default.SendChatActionId))
        //        {

        //            comboBoxStreamerBotChatAction.SelectedValue = Settings.Default.SendChatActionId;
        //        }
        //        if (tabControl1.TabPages.Contains(tabPageEvents))
        //        {


        //            logger.Debug("Actions from streamer bot received");

        //            logger.Debug("setting distance actions");
        //            if (!streamerbotClient.Actions.Any(a => a.name == "[none]"))
        //                streamerbotClient.Actions.Add(new StreamerbotAction() { name = "[none]", id = string.Empty });

        //            distanceList = new List<StreamerbotAction>(streamerbotClient.Actions).OrderBy(a => a.name).ToList();
        //            comboBoxSpecialDistanceActions.DataSource = distanceList;
        //            comboBoxSpecialDistanceActions.DisplayMember = "name";
        //            comboBoxSpecialDistanceActions.ValueMember = "id";

        //            scoreList = new List<StreamerbotAction>(streamerbotClient.Actions).OrderBy(a => a.name).ToList();
        //            comboBoxSpecialScoreActions.DataSource = scoreList;
        //            comboBoxSpecialScoreActions.DisplayMember = "name";
        //            comboBoxSpecialScoreActions.ValueMember = "id";

        //            roundStartList = new List<StreamerbotAction>(streamerbotClient.Actions).OrderBy(a => a.name).ToList();
        //            comboBoxRoundStartAction.DataSource = roundStartList;
        //            comboBoxRoundStartAction.DisplayMember = "name";
        //            comboBoxRoundStartAction.ValueMember = "id";



        //            roundendList = new List<StreamerbotAction>(streamerbotClient.Actions).OrderBy(a => a.name).ToList();
        //            comboBoxRoundEndAction.DataSource = roundendList;
        //            comboBoxRoundEndAction.DisplayMember = "name";
        //            comboBoxRoundEndAction.ValueMember = "id";

        //            gameendList = new List<StreamerbotAction>(streamerbotClient.Actions).OrderBy(a => a.name).ToList();
        //            comboBoxGameEndAction.DataSource = gameendList;
        //            comboBoxGameEndAction.DisplayMember = "name";
        //            comboBoxGameEndAction.ValueMember = "id";

        //            if (!string.IsNullOrEmpty(Settings.Default.SpecialDistanceActionID))
        //            {

        //                comboBoxSpecialDistanceActions.SelectedValue = Settings.Default.SpecialDistanceActionID;
        //            }

        //            logger.Debug("Setting score actions");

        //            if (!string.IsNullOrEmpty(Settings.Default.SpecialScoreActionID))
        //            {
        //                comboBoxSpecialScoreActions.SelectedValue = Settings.Default.SpecialScoreActionID;
        //            }
        //            logger.Debug("setting round start actions");

        //            if (!string.IsNullOrEmpty(Settings.Default.RoundStartActionID))
        //            {

        //                comboBoxRoundStartAction.SelectedValue = Settings.Default.RoundStartActionID;
        //            }

        //            logger.Debug("setting round end actions");

        //            if (!string.IsNullOrEmpty(Settings.Default.RoundEndActionID))
        //            {
        //                comboBoxRoundEndAction.SelectedValue = Settings.Default.RoundEndActionID;

        //            }
        //            logger.Debug("setting game end actions");

        //            if (!string.IsNullOrEmpty(Settings.Default.GameEndActionID))
        //            {
        //                comboBoxGameEndAction.SelectedValue = Settings.Default.GameEndActionID;
        //            }


        //            if (chkStreamerBotConnectAtStartup.Checked)
        //            {
        //                buttonConnectStreamerBot.Enabled = true;
        //            }
        //        }
        //    }
        //}

        private void BindTableOptionsToTreeview(TableOptions to)
        {
            TreeNode rootNode = new("Game modes")
            {
                Tag = to
            };
            foreach (GameOptions game in to.Options)
            {
                TreeNode gameNode = new(game.Mode)
                {
                    Tag = game
                };
                foreach (StageOptions stage in game.Stages)
                {
                    TreeNode stageNode = new(stage.Stage)
                    {
                        Tag = stage
                    };
                    foreach (TableColumn col in stage.Columns.OrderBy(c => c.DataField))
                    {
                        TreeNode colNode = new(col.Name + " (" + col.DataField + ")")
                        {
                            Tag = col
                        };

                        stageNode.Nodes.Add(colNode);
                    }
                    gameNode.Nodes.Add(stageNode);
                }
                rootNode.Nodes.Add(gameNode);
            }
            treeView1.Nodes.Add(rootNode);
            treeView1.ExpandAll();
        }



        private void txtStreamerBot_TextChanged(object sender, EventArgs e)
        {
            buttonConnectStreamerBot.Enabled = txtStreamerBotIP.Text.Length > 0 && txtStreamerBotPort.Text.Length > 0;
        }

        private async void buttonConnectStreamerBot_Click(object sender, EventArgs e)
        {
            logger.Debug("connecting to streamer bot");
            buttonConnectStreamerBot.Enabled = false;
            txtStreamerBotIP.Enabled = false;
            txtStreamerBotPort.Enabled = false;
            chkStreamerBotConnectAtStartup.Enabled = false;
            if (chkStreamerBotConnectAtStartup.Checked)
            {
                logger.Debug("connection because at startup");
                if (!streamerbotClient.IsRunning())
                {
                    logger.Debug("disconnected, connect needed");
                    try
                    {
                        if (streamerbotClient.Connect(txtStreamerBotIP.Text, txtStreamerBotPort.Text, Settings.Default.SendChatActionId, Settings.Default.SendChatActionName, Settings.Default.SendChatMsgsViaStreamerBot, parent).Result)
                        {
                            logger.Debug("connection succeeded");
                        }
                        else
                        {
                            MessageBox.Show("Connection failed!\nPlease check IP and port!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                    catch (Exception ex) { logger.Error(ex.Summarize()); }
                }
                else
                {
                    logger.Debug("already connected");
                    if (sender == buttonConnectStreamerBot)
                    {
                        var closed = await streamerbotClient.CloseConnection();
                    }
                }

            }
            else
            {
                logger.Debug("testing connection");
                if (sender != this)
                {
                    try
                    {
                        if (streamerbotClient.TestConnection(txtStreamerBotIP.Text, txtStreamerBotPort.Text))
                        {
                            logger.Debug("test successful");
                            MessageBox.Show("Connection succeeded!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        else
                        {
                            logger.Debug("Test failed");
                            MessageBox.Show("Connection failed!\nPlease check IP and port!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                    catch (Exception ex) { logger.Error(ex.Summarize()); }
                }

            }
            SetUpStreambotInputs();
            
        }

        private void chkStreamerBotConnectAtStartup_CheckedChanged(object sender, EventArgs e)
        {
            SetUpStreambotInputs();



        }

        private void SetUpStreambotInputs()
        {
            if (chkStreamerBotConnectAtStartup.Checked)
            {
                if (streamerbotClient.IsRunning())
                {
                    buttonConnectStreamerBot.Text = "Disconnect";
                    txtStreamerBotIP.Enabled =
                    txtStreamerBotPort.Enabled =
                    chkStreamerBotConnectAtStartup.Enabled = false;
                }

                else
                {
                    buttonConnectStreamerBot.Text = "Connect";
                    txtStreamerBotIP.Enabled =
                    chkStreamerBotConnectAtStartup.Enabled =
                    txtStreamerBotPort.Enabled = true;
                }
            }
            else
            {
                buttonConnectStreamerBot.Text = "Test connection";
            }
            buttonConnectStreamerBot.Enabled = true;
            checkBoxSpecialScoreAction.Enabled =
               checkBoxSpecialDistanceAction.Enabled =
               ctrlBotSpecialDistance.Enabled =
               checkBoxRoundTimerExecuteStreamerBotAction.Enabled =
               ctrlBotRoundStart.Enabled =
               chkBotGameEndExecute.Enabled =
               ctrlBotGameEnd.Enabled =
               chkBotRoundEndExecute.Enabled =
               ctrlBotRoundEnd.Enabled =
               checkBoxSendChatMsgsViaStreamerBot.Enabled =
               ctrlBotSpecialScore.Enabled =
               ctrlBotChatMessages.Enabled = chkStreamerBotConnectAtStartup.Checked && streamerbotClient.IsRunning();
        }

        private void tabPageOverlay_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            panel1.Controls.Clear();
            if (e.Node.Tag != null && e.Node.Tag.GetType() == typeof(TableColumn))
            {
                TableColumnEditControl ctrl = new(e.Node.Tag as TableColumn)
                {
                    Dock = DockStyle.Fill
                };
                ctrl.ColumnChanged += TableColunnEditor_ColumnChanged;
                panel1.Controls.Add(ctrl);
            }
        }

        private void TableColunnEditor_ColumnChanged(object sender, TableColumnChangedEventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag != null && treeView1.SelectedNode.Tag.GetType() == typeof(TableColumn))
            {
                treeView1.SelectedNode.Tag = e;
                treeView1.SelectedNode.Text = e.Column.Name + " (" + e.Column.DataField + ")";
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TreeNode stageNode = treeView1.SelectedNode;
            if (stageNode.Tag == null)
            {
                e.Cancel = true;
            }
            else if (typeof(TableColumn) == stageNode.Tag.GetType())
            {
                newColumnToolStripMenuItem.Visible = false;
                deleteColumnToolStripMenuItem.Visible = true;
            }
            else if (typeof(StageOptions) == stageNode.Tag.GetType())
            {
                newColumnToolStripMenuItem.Visible = true;
                deleteColumnToolStripMenuItem.Visible = false;
            }

        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

        }

        private void newColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode stageNode = treeView1.SelectedNode;
            TableColumn col = new()
            {
                DataField = string.Empty,
                Name = "N/A"
            };
            TreeNode colNode = new(col.Name + " (" + col.DataField + ")")
            {
                Tag = col
            };

            stageNode.Nodes.Add(colNode);

        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);

                if (treeView1.SelectedNode != null)
                {
                    contextMenuStrip1.Show(treeView1, e.Location);
                }
            }

        }

        private void deleteColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode stageNode = treeView1.SelectedNode;
            treeView1.Nodes.Remove(stageNode);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            txtObsPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        private void ctnConnectObs_Click(object sender, EventArgs e)
        {
            btnConnectObs.Enabled = false;
            chkObsConnectAtStartup.Enabled = false;
            txtObsIp.Enabled = false;
            txtObsPort.Enabled = false;
            txtObsPassword.Enabled = false;
            if (chkObsConnectAtStartup.Checked)
            {
                logger.Debug("OBS at startup set");
                if (!obsClient.IsAlive())
                {
                    logger.Debug("obs client not alive, connecting");
                    btnConnectObs.Text = "Disconnect";
                    try
                    {
                        if (obsClient.Connect("ws://" + txtObsIp.Text + ":" + txtObsPort.Text, txtObsPassword.Text) && obsClient.IsAlive())
                        {
                            logger.Debug("obs connected");
                            GetAndSetOBSScenes();
                        }
                        else
                        {
                            logger.Debug("obs Connection failed");
                            MessageBox.Show("Connection failed!\nPlease check IP and port!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                    catch (Exception ex) { logger.Error(ex.Summarize()); }
                }
                else
                {
                    logger.Debug("obs client is alove, no connect needed");
                    if (sender == btnConnectObs)
                    {
                        btnConnectObs.Text = "Connect";
                        obsClient.Disconnect();

                    }
                    else
                    {
                        btnConnectObs.Text = "Disconnect";
                        GetAndSetOBSScenes();
                    }
                }

            }
            else
            {
                logger.Debug("Connect obs at startup not set, testing");
                if (sender != this)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(txtObsIp.Text) && !string.IsNullOrEmpty(txtObsPort.Text) && obsClient.TestConnection("ws://" + txtObsIp.Text + ":" + txtObsPort.Text, txtObsPassword.Text))
                        {
                            MessageBox.Show("Connection succeeded!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                        else
                        {
                            MessageBox.Show("Connection failed!\nPlease check IP and port!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        }
                    }
                    catch (Exception ex) { logger.Error(ex.Summarize()); }
                }

                btnConnectObs.Enabled = true;
                txtObsIp.Enabled = true;
                chkObsConnectAtStartup.Enabled = true;
                txtObsPort.Enabled = true;
                txtObsPassword.Enabled = true;
            }
            SetUpObsInputs();
        }
        List<MyOBSScene> roundstartScenes, roundendScenes, gameendScenes, distanceScenes, scoreScenes;
        private void GetAndSetOBSScenes()
        {
            if(obsClient.IsAlive() && !obsClient.Scenes.Any())
            {
                obsClient.GetScenes();
            }
            if (obsClient.Scenes.Any() && tabControl1.TabPages.Contains(tabPageEvents))
            {
                logger.Debug("Getting round start scenes");
                roundstartScenes = new(obsClient.Scenes.OrderBy(o => o.Name));
                comboRoundTimerObsScene.DataSource = roundstartScenes;
                comboRoundTimerObsScene.DisplayMember = "DisplayName";
                comboRoundTimerObsScene.ValueMember = "DisplayName";

                logger.Debug("Round start set, getting round end");
                roundendScenes = new(obsClient.Scenes.OrderBy(o => o.Name));
                comboOBSRoundEndScene.DataSource = roundendScenes;
                comboOBSRoundEndScene.DisplayMember = "DisplayName";
                comboOBSRoundEndScene.ValueMember = "DisplayName";

                logger.Debug("Round end set, getting game end");
                gameendScenes = new(obsClient.Scenes.OrderBy(o => o.Name));
                comboOBSGameEndScene.DataSource = gameendScenes;
                comboOBSGameEndScene.DisplayMember = "DisplayName";
                comboOBSGameEndScene.ValueMember = "DisplayName";

                logger.Debug("game end set, getting distnace");
                distanceScenes = new(obsClient.Scenes.OrderBy(o => o.Name));
                comboSpecialDistanceObsScene.DataSource = distanceScenes;
                comboSpecialDistanceObsScene.ValueMember = "DisplayName";
                comboSpecialDistanceObsScene.DisplayMember = "DisplayName";

                logger.Debug("distance set, getting score");
                scoreScenes = new(obsClient.Scenes.OrderBy(o => o.Name));
                comboSpecialScoreObsScene.DataSource = scoreScenes;
                comboSpecialScoreObsScene.ValueMember = "DisplayName";
                comboSpecialScoreObsScene.DisplayMember = "DisplayName";

                if (!string.IsNullOrEmpty(Settings.Default.ObsRoundStartScene))
                {
                    comboRoundTimerObsScene.SelectedValue = Settings.Default.ObsRoundStartScene;
                }

                if (!string.IsNullOrEmpty(Settings.Default.ObsRoundEndScene))
                {
                    comboOBSRoundEndScene.SelectedValue = Settings.Default.ObsRoundEndScene; 
                }

                if (!string.IsNullOrEmpty(Settings.Default.ObsGameEndScene))
                {
                    comboOBSGameEndScene.SelectedValue = Settings.Default.ObsGameEndScene;
                }

                if (!string.IsNullOrEmpty(Settings.Default.ObsSpecialDistanceScene))
                {
                    comboSpecialDistanceObsScene.SelectedValue = Settings.Default.ObsSpecialDistanceScene;

                }

                if (!string.IsNullOrEmpty(Settings.Default.ObsSpecialScoreScene))
                {
                    comboSpecialScoreObsScene.SelectedValue = Settings.Default.ObsSpecialScoreScene;

                }

                logger.Debug("All OBS scenes successfully set");
            }
        }

        private void SetUpObsInputs()
        {
            if (chkObsConnectAtStartup.Checked)
            {
                if (obsClient.IsAlive())
                {
                    btnConnectObs.Text = "Disconnect";
                    txtObsIp.Enabled =
                    txtObsPassword.Enabled =
                    chkObsConnectAtStartup.Enabled =
                    txtObsPort.Enabled = false;
                }

                else
                {
                    btnConnectObs.Text = "Connect";
                    txtObsPort.Enabled =
                    txtObsPassword.Enabled =
                    chkObsConnectAtStartup.Enabled =
                    txtObsIp.Enabled = true;
                }
            }
            else
            {
                btnConnectObs.Text = "Test connection";
            }
            btnConnectObs.Enabled = true;
            chkRoundTimerOBS.Enabled =
                comboRoundTimerObsScene.Enabled =
                comboRoundTimerObsAction.Enabled =
                comboRoundTimerObsSource.Enabled =
                chkOBSRoundEndExecute.Enabled =
                comboOBSRoundEndScene.Enabled =
                comboOBSRoundEndAction.Enabled =
                comboOBSRoundEndSource.Enabled =
                chkOBSGameEndExecute.Enabled =
                comboOBSGameEndScene.Enabled =
                comboOBSGameEndAction.Enabled =
                comboOBSGameEndSource.Enabled =
                chkSpecialDistanceObs.Enabled =
                comboSpecialDistanceObsSource.Enabled =
                comboSpecialDistanceObsScene.Enabled =
                comboSpecialDistanceObsAction.Enabled =
                chkSpecialScoreObs.Enabled =
                comboSpecialScoreObsScene.Enabled =
                comboSpecialScoreObsSource.Enabled =
                comboSpecialScoreObsAction.Enabled = chkObsConnectAtStartup.Checked && obsClient.IsAlive();
        }

        private void chkObsConnectAtStartup_CheckedChanged(object sender, EventArgs e)
        {
            SetUpObsInputs();
        }

        private void txtObs_TextChanged(object sender, EventArgs e)
        {
            btnConnectObs.Enabled = txtObsPassword.Text.Length > 0 && txtObsPort.Text.Length > 0 && txtObsIp.Text.Length > 0;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            string link = ((LinkLabel)sender).Text ?? string.Empty;
            if (link.ContainsDefault('@'))
            {
                link = "mailto:" + link;
            }
            else
                link = "https://geochatter.tv";
            // Navigate to a URL.
            GeneralPurposeUtils.OpenURL(link);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GeneralPurposeUtils.OpenURL(Settings.Default.Link_TwitchAuth);
        }

        private void comboRoundTimerObsSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null && ((MyOBSScene)((ComboBox)sender).SelectedItem).Items.Any())
            {
                logger.Debug("Setting round start source");
                comboRoundTimerObsSource.DataSource = ((MyOBSScene)((ComboBox)sender).SelectedItem).Items;
                comboRoundTimerObsSource.DisplayMember = "Name";
                comboRoundTimerObsSource.ValueMember = "Id";
                if (Settings.Default.ObsRoundStartSource != 0)
                {
                    comboRoundTimerObsSource.SelectedValue = Settings.Default.ObsRoundStartSource;
                }

                logger.Debug("Finished setting round start source");
            }
        }

        private void comboSpecialScoreObsScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null && ((MyOBSScene)((ComboBox)sender).SelectedItem).Items.Any())
            {
                logger.Debug("Setting special score source");
                comboSpecialScoreObsSource.DataSource = ((MyOBSScene)((ComboBox)sender).SelectedItem).Items;
                comboSpecialScoreObsSource.DisplayMember = "Name";
                comboSpecialScoreObsSource.ValueMember = "Id";
                if (Settings.Default.ObsSpecialScoreSource != 0)
                {
                    comboSpecialScoreObsSource.SelectedValue = Settings.Default.ObsSpecialScoreSource;
                }

                logger.Debug("Finished setting special score source");
            }
        }

        private void comboSpecialDistanceObsScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null && ((MyOBSScene)((ComboBox)sender).SelectedItem).Items.Any())
            {
                logger.Debug("Setting special distance source");
                comboSpecialDistanceObsSource.DataSource = ((MyOBSScene)((ComboBox)sender).SelectedItem).Items;
                comboSpecialDistanceObsSource.DisplayMember = "Name";
                comboSpecialDistanceObsSource.ValueMember = "Id";
                if (Settings.Default.ObsSpecialDistanceSource != 0)
                {
                    comboSpecialDistanceObsSource.SelectedValue = Settings.Default.ObsSpecialDistanceSource;
                }

                logger.Debug("Finished setting special distance source");
            }
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GeneralPurposeUtils.OpenURL(Settings.Default.Link_Discord);
        }

        private void comboOBSGameEndScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null && ((MyOBSScene)((ComboBox)sender).SelectedItem).Items.Any())
            {
                logger.Debug("Setting game end source");
                comboOBSGameEndSource.DataSource = ((MyOBSScene)((ComboBox)sender).SelectedItem).Items;
                comboOBSGameEndSource.DisplayMember = "Name";
                comboOBSGameEndSource.ValueMember = "Id";
                if (Settings.Default.ObsGameEndSource != 0)
                {
                    comboOBSGameEndSource.SelectedValue = Settings.Default.ObsGameEndSource;
                }

                logger.Debug("Finished setting game end source");
            }
        }

        private void comboOBSRoundEndScene_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedItem != null && ((MyOBSScene)((ComboBox)sender).SelectedItem).Items.Any())
            {
                logger.Debug("Setting round end source");
                comboOBSRoundEndSource.DataSource = ((MyOBSScene)((ComboBox)sender).SelectedItem).Items;
                comboOBSRoundEndSource.DisplayMember = "Name";
                comboOBSRoundEndSource.ValueMember = "Id";
                if (Settings.Default.ObsRoundEndSource != 0)
                {
                    comboOBSRoundEndSource.SelectedValue = Settings.Default.ObsRoundEndSource;
                }

                logger.Debug("Finished setting round end source");
            }
        }

        private void reconnectTwitchBotButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtGeneralChannelName.Text))
            {
                MessageBox.Show("You have to enter a channel name!", "Channel name is empty", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (Settings.Default.oauthToken != txtGeneralOauthToken.Text)
            {
                Settings.Default.oauthToken = txtGeneralOauthToken.Text;
            }

            if (Settings.Default.TwitchChannel != txtGeneralChannelName.Text)
            {
                Settings.Default.TwitchChannel = txtGeneralChannelName.Text;
            }
            Settings.Default.Save();
            reconnectTwitchBotButton.Enabled = false;

            parent?.ConnectTwitchBot();
            AttributeDiscovery.AddEventHandlers(this, parent?.CurrentBot);
        }

        private void ScoreboardBGDisplaybutton_Click(object sender, EventArgs e)
        {
            DialogResult res = colorDialog2.ShowDialog();
            if (res == DialogResult.OK)
            {
                ScoreboardBGDisplaybutton.BackColor = colorDialog2.Color;
            }
        }

        private void ScoreboardFGDisplaybutton_Click(object sender, EventArgs e)
        {
            DialogResult res = colorDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                ScoreboardFGDisplaybutton.BackColor = colorDialog1.Color;
            }
        }

        private void chkSpecialScoreRange_CheckedChanged(object sender, EventArgs e)
        {
            txtSpecialScoreTo.Enabled = chkSpecialScoreRange.Checked;
            if (!chkSpecialScoreRange.Checked)
            {
                txtSpecialScoreTo.Text = "";
            }
        }

        private void chkSpecialDistanceRange_CheckedChanged(object sender, EventArgs e)
        {
            txtSpecialDistanceTo.Enabled = chkSpecialDistanceRange.Checked;
            if (!chkSpecialDistanceRange.Checked)
            {
                txtSpecialDistanceTo.Text = "";
            }
        }

        private async void txtGeneralChannelName_Leave(object sender, EventArgs e)
        {
            if (txtGeneralChannelName.Text != Settings.Default.TwitchChannel)
            {
                Player player = await TwitchHelper.GetUserDataFromTwitch(userName: txtGeneralChannelName.Text);
                if (player != null && !string.IsNullOrEmpty(player.DisplayName))
                {
                    txtGeneralChannelName.Text = Settings.Default.TwitchChannel = player.DisplayName;
                    btnAuthorizeManually.Enabled = btnAuthorizeAutomatically.Enabled= true;
                }
            }
        }

        private void SettingsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        /// <summary>
        /// Settings Apply button click event
        /// </summary>
        public event EventHandler<EventArgs> SettingsApplied;
        /// <summary>
        /// Fires <see cref="SettingsApplied"/>
        /// </summary>
        public void FireSettingsApplied()
        {
            SettingsApplied?.Invoke(this, new());
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSettings();
            MessageBox.Show("Settings saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            FireSettingsApplied();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private bool showAdvancedSettings;
        private void btnShowAdvanced_Click(object sender, EventArgs e)
        {
            showAdvancedSettings = true;
            parent.ShowAdvancedSettings = true;
            HandleAdvancedSettings();
        }

        private void HandleAdvancedSettings()
        {
            chkEnableDebugLogging.Visible = chkEnableDebugLogging.Checked || showAdvancedSettings;

            groupGameSettings.Visible =
            
            //grpStreamerBotConnection.Visible =
            //grpObsConnection.Visible =
            chkMsgsJoinChannel.Visible =
            chkMsgsStartGame.Visible =
            chkMsgsRoundStarted.Visible =
            chkSendDoubleGuessMsg.Visible =
            chkSendSameGuessMessage.Visible =
            panelScoreboardCheckboxes.Visible =
            panelScoreboardRounding.Visible =
            panelScoreBoardSpeed.Visible =
            panelScoreBoardUnit.Visible =
            propertyGrid1.Visible = showAdvancedSettings;

            if (showAdvancedSettings && !tabControl1.TabPages.Contains(tabPageEvents))
            {
                tabControl1.TabPages.Insert(3, tabPageEvents);
                GetAndSetOBSScenes();
            }
            else if (!showAdvancedSettings && tabControl1.TabPages.Contains(tabPageEvents))
            {
                tabControl1.TabPages.Remove(tabPageEvents);
            }
            //if (showAdvancedSettings && !tabControl1.TabPages.Contains(tabPageConnections))
            //{
            //    tabControl1.TabPages.Insert(3, tabPageConnections);
            //}
            //else if (!showAdvancedSettings && tabControl1.TabPages.Contains(tabPageConnections))
            //{
            //    tabControl1.TabPages.Remove(tabPageConnections);
            //}

            
                tabControl1.TabPages.Remove(tabPageLabels);
            
            btnShowAdvanced.Enabled = !showAdvancedSettings;
        }

        private async void btnAuthorizeAutomatically_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtGeneralChannelName.Text))
            {
                MessageBox.Show("You have to enter a channel name!", "Channel name is empty", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            TwitchOAuthHelper.SendRequestToBrowser();
            TwitchAuthenticationModel model = await TwitchOAuthHelper.GetAuthenticationValuesAsync();
            if (!string.IsNullOrEmpty(model.Token))
            {
                txtGeneralOauthToken.Text = model.Token;
                btnAuthorizeAutomatically.Enabled = btnAuthorizeManually.Enabled = false;
                btnForgetTwitch.Enabled = true;
            }
            reconnectTwitchBotButton_Click(reconnectTwitchBotButton, new EventArgs());
        }
        private void linkLabel36_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GeneralPurposeUtils.OpenURL("https://github.com/GeoChatter");
        }
        int clickCounter = 0;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
                

            if (clickCounter == 5)
            {
                clickCounter = 0;
                if (Control.ModifierKeys == (Keys.Shift | Keys.Control))
                {
                    this.tabControl1.TabPages.Add(tabDevelopment);
                    chkDEVUseDevApi.Checked = Settings.Default.DebugUseDevApi;
                    chkDEVShowDevTools.Checked = Settings.Default.DebugShowDevTools;
                    chkDEVEnableRandomBotGuess.Checked = Settings.Default.DebugEnableRandomBotGuesses;
                }
            }
            if (clickCounter < 5)
                clickCounter++;


        }

        private async void btnAuthorizeManually_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtGeneralChannelName.Text))
            {
                MessageBox.Show("You have to enter a channel name!", "Channel name is empty", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string url = TwitchOAuthHelper.SendRequestToBrowser(false);
            Thread thread = new(() => Clipboard.SetText(url));
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();
            SetupWizard.SetupWizard.ShowManualAuthMessageBoxAsync();
            TwitchAuthenticationModel model = await TwitchOAuthHelper.GetAuthenticationValuesAsync();
            
            if (!string.IsNullOrEmpty(model.Token))
            {
                txtGeneralOauthToken.Text = model.Token;
                btnAuthorizeAutomatically.Enabled = btnAuthorizeManually.Enabled = false;
                btnForgetTwitch.Enabled= true;
            }
            reconnectTwitchBotButton_Click(reconnectTwitchBotButton, new EventArgs());
        }

        private void SettingsDialog_Enter(object sender, EventArgs e)
        {
            parent?.ResetJSCTRLCheck();
        }

        private void checkBoxEnableChatMsgs_CheckedChanged(object sender, EventArgs e)
        {
            SetTwitchInputs();

        }

        private void SetTwitchInputs()
        {
            grpTwitchChatConnection.Enabled = chkEnableMapOverlay.Enabled = chkAutoBan.Enabled = checkBoxEnableChatMsgs.Checked;

            if (!checkBoxEnableChatMsgs.Checked)
                chkEnableMapOverlay.Checked = chkAutoBan.Checked = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will reset your Twitch connection!\n\rYou will have to re-authorize GeoChatter!\n\rAre you sure?", "Really?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Settings.Default.oauthToken = txtGeneralOauthToken.Text = String.Empty;
                parent?.DisconnectTwitchBot();
                checkBoxEnableChatMsgs.Checked = 
                    Settings.Default.EnableTwitchChatMsgs = btnForgetTwitch.Enabled = false;
                Settings.Default.Save();
                btnAuthorizeAutomatically.Enabled = btnAuthorizeManually.Enabled = true;
              //  flowLayoutPanel1.Enabled = false;
            }
        }

        private void streamerColor_Click(object sender, EventArgs e)
        {
            DialogResult res = usernameColorDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                streamerColor.BackColor = usernameColorDialog.Color;
            }
        }

        private void randomFlag_Click(object sender, EventArgs e)
        {
            streamerFlag.SelectedIndex = Random.Shared.Next(streamerFlag.Items.Count);
        }

        private void betaTestersLink_Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GeneralPurposeUtils.OpenURL("https://twitch.tv/" + ((LinkLabel)sender).Text);
        }

        private void streamerFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (streamerFlag.SelectedIndex >= 0)
            {
                SetFlagDisplay(FlagCodeFromComboboxItem(streamerFlag.Items[streamerFlag.SelectedIndex].ToStringDefault()));
            }
        }
    }
}
