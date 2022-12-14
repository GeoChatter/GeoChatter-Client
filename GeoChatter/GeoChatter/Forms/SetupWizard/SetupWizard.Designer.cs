#pragma warning disable CS1591,CA1062

using System;

namespace GeoChatter.Forms.SetupWizard
{
    partial class SetupWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupWizard));
            this.wizardControl1 = new AeroWizard.WizardControl();
            this.pageWelcome = new AeroWizard.WizardPage();
            this.labelWelcome = new System.Windows.Forms.Label();
            this.pageConnections = new AeroWizard.WizardPage();
            this.reconnectTwitchBotButton = new System.Windows.Forms.Button();
            this.txtGeneralChannelName = new System.Windows.Forms.TextBox();
            this.txtGeneralBotName = new System.Windows.Forms.TextBox();
            this.labelChannelName = new System.Windows.Forms.Label();
            this.btnAuthorizeManually = new System.Windows.Forms.Button();
            this.btnAuthorizeAutomatically = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.labelBot = new System.Windows.Forms.Label();
            this.pageGame = new AeroWizard.WizardPage();
            this.groupGameSettings = new System.Windows.Forms.GroupBox();
            this.chkEnableMapOverlay = new System.Windows.Forms.CheckBox();
            this.chkAllowSameLocationGuess = new System.Windows.Forms.CheckBox();
            this.chkUseEnglishCountryNames = new System.Windows.Forms.CheckBox();
            this.chkResetStreakOnSkippedRound = new System.Windows.Forms.CheckBox();
            this.pageMarkers = new AeroWizard.WizardPage();
            this.groupMarker = new System.Windows.Forms.GroupBox();
            this.chkGuessInfoTime = new System.Windows.Forms.CheckBox();
            this.chkGuessInfoCoordinates = new System.Windows.Forms.CheckBox();
            this.chkGuessInfoDistance = new System.Windows.Forms.CheckBox();
            this.chkGuessInfoScore = new System.Windows.Forms.CheckBox();
            this.chkGuessInfoStreak = new System.Windows.Forms.CheckBox();
            this.labelMarkers = new System.Windows.Forms.Label();
            this.pageChat = new AeroWizard.WizardPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkMsgsJoinChannel = new System.Windows.Forms.CheckBox();
            this.chkMsgsStartGame = new System.Windows.Forms.CheckBox();
            this.chkMsgsRoundStarted = new System.Windows.Forms.CheckBox();
            this.chkMsgsGuessReceived = new System.Windows.Forms.CheckBox();
            this.chkSendDoubleGuessMsg = new System.Windows.Forms.CheckBox();
            this.chkSendSameGuessMessage = new System.Windows.Forms.CheckBox();
            this.chkMsgsRoundEnd = new System.Windows.Forms.CheckBox();
            this.chkMsgsGameEnded = new System.Windows.Forms.CheckBox();
            this.pagePacks = new AeroWizard.WizardPage();
            this.labelFlagPacks = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pageExtensions = new AeroWizard.WizardPage();
            this.linkLabelWiki = new System.Windows.Forms.LinkLabel();
            this.linkLabelUnity = new System.Windows.Forms.LinkLabel();
            this.linkLabelCar = new System.Windows.Forms.LinkLabel();
            this.labelJS = new System.Windows.Forms.Label();
            this.pageFormulars = new AeroWizard.WizardPage();
            this.lblFormulas = new System.Windows.Forms.Label();
            this.PageFinish = new AeroWizard.WizardPage();
            this.linkLabelOauth = new System.Windows.Forms.LinkLabel();
            this.txtGeneralOauthToken = new System.Windows.Forms.TextBox();
            this.labelOauth = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
            this.pageWelcome.SuspendLayout();
            this.pageConnections.SuspendLayout();
            this.pageGame.SuspendLayout();
            this.groupGameSettings.SuspendLayout();
            this.pageMarkers.SuspendLayout();
            this.groupMarker.SuspendLayout();
            this.pageChat.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pagePacks.SuspendLayout();
            this.pageExtensions.SuspendLayout();
            this.pageFormulars.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.BackColor = System.Drawing.Color.White;
            this.wizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.wizardControl1.Location = new System.Drawing.Point(0, 0);
            this.wizardControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.Pages.Add(this.pageWelcome);
            this.wizardControl1.Pages.Add(this.pageConnections);
            this.wizardControl1.Pages.Add(this.pageGame);
            this.wizardControl1.Pages.Add(this.pageMarkers);
            this.wizardControl1.Pages.Add(this.pageChat);
            this.wizardControl1.Pages.Add(this.pagePacks);
            this.wizardControl1.Pages.Add(this.pageExtensions);
            this.wizardControl1.Pages.Add(this.pageFormulars);
            this.wizardControl1.Pages.Add(this.PageFinish);
            this.wizardControl1.Size = new System.Drawing.Size(670, 479);
            this.wizardControl1.TabIndex = 0;
            this.wizardControl1.Title = "GeoChatter initial configuration";
            this.wizardControl1.Finished += new System.EventHandler(this.wizardControl1_Finished);
            this.wizardControl1.SelectedPageChanged += new System.EventHandler(this.wizardControl1_SelectedPageChanged);
            // 
            // pageWelcome
            // 
            this.pageWelcome.AllowBack = false;
            this.pageWelcome.Controls.Add(this.labelWelcome);
            this.pageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageWelcome.Location = new System.Drawing.Point(0, 0);
            this.pageWelcome.Margin = new System.Windows.Forms.Padding(0);
            this.pageWelcome.Name = "pageWelcome";
            this.pageWelcome.NextPage = this.pageConnections;
            this.pageWelcome.Size = new System.Drawing.Size(623, 325);
            this.pageWelcome.TabIndex = 0;
            this.pageWelcome.Text = "Welcome";
            // 
            // labelWelcome
            // 
            this.labelWelcome.AutoSize = true;
            this.labelWelcome.Location = new System.Drawing.Point(0, 0);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new System.Drawing.Size(513, 135);
            this.labelWelcome.TabIndex = 0;
            this.labelWelcome.Text = resources.GetString("labelWelcome.Text");
            // 
            // pageConnections
            // 
            this.pageConnections.AllowNext = false;
            this.pageConnections.Controls.Add(this.reconnectTwitchBotButton);
            this.pageConnections.Controls.Add(this.txtGeneralChannelName);
            this.pageConnections.Controls.Add(this.txtGeneralBotName);
            this.pageConnections.Controls.Add(this.labelChannelName);
            this.pageConnections.Controls.Add(this.btnAuthorizeManually);
            this.pageConnections.Controls.Add(this.btnAuthorizeAutomatically);
            this.pageConnections.Controls.Add(this.label27);
            this.pageConnections.Controls.Add(this.labelBot);
            this.pageConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageConnections.Location = new System.Drawing.Point(0, 0);
            this.pageConnections.Margin = new System.Windows.Forms.Padding(0);
            this.pageConnections.Name = "pageConnections";
            this.pageConnections.NextPage = this.pageGame;
            this.pageConnections.Size = new System.Drawing.Size(623, 325);
            this.pageConnections.TabIndex = 1;
            this.pageConnections.Text = "Twitch connection";
            // 
            // reconnectTwitchBotButton
            // 
            this.reconnectTwitchBotButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reconnectTwitchBotButton.Location = new System.Drawing.Point(31, 213);
            this.reconnectTwitchBotButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.reconnectTwitchBotButton.Name = "reconnectTwitchBotButton";
            this.reconnectTwitchBotButton.Size = new System.Drawing.Size(372, 27);
            this.reconnectTwitchBotButton.TabIndex = 17;
            this.reconnectTwitchBotButton.Text = "Test Twitch-bot connection to continue";
            this.toolTip1.SetToolTip(this.reconnectTwitchBotButton, "To continue the wizard you need to make sure that the Twitch connection is workin" +
        "g by clicking this button");
            this.reconnectTwitchBotButton.UseVisualStyleBackColor = true;
            this.reconnectTwitchBotButton.Click += new System.EventHandler(this.reconnectTwitchBotButton_Click);
            // 
            // txtGeneralChannelName
            // 
            this.txtGeneralChannelName.Location = new System.Drawing.Point(159, 24);
            this.txtGeneralChannelName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtGeneralChannelName.Name = "txtGeneralChannelName";
            this.txtGeneralChannelName.Size = new System.Drawing.Size(243, 23);
            this.txtGeneralChannelName.TabIndex = 14;
            this.toolTip1.SetToolTip(this.txtGeneralChannelName, "Enter your Twitch channel name here");
            this.txtGeneralChannelName.Leave += new System.EventHandler(this.txtGeneralChannelName_Leave);
            // 
            // txtGeneralBotName
            // 
            this.txtGeneralBotName.Location = new System.Drawing.Point(159, 54);
            this.txtGeneralBotName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtGeneralBotName.Name = "txtGeneralBotName";
            this.txtGeneralBotName.Size = new System.Drawing.Size(243, 23);
            this.txtGeneralBotName.TabIndex = 15;
            this.toolTip1.SetToolTip(this.txtGeneralBotName, "Enter your Twitch bot name here. If you don\'t have or want to use a bot, enter yo" +
        "u channel name here again.");
            // 
            // labelChannelName
            // 
            this.labelChannelName.AutoSize = true;
            this.labelChannelName.Location = new System.Drawing.Point(28, 27);
            this.labelChannelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelChannelName.Name = "labelChannelName";
            this.labelChannelName.Size = new System.Drawing.Size(84, 15);
            this.labelChannelName.TabIndex = 12;
            this.labelChannelName.Text = "Channel name";
            // 
            // btnAuthorizeManually
            // 
            this.btnAuthorizeManually.Location = new System.Drawing.Point(210, 96);
            this.btnAuthorizeManually.Name = "btnAuthorizeManually";
            this.btnAuthorizeManually.Size = new System.Drawing.Size(159, 43);
            this.btnAuthorizeManually.TabIndex = 12;
            this.btnAuthorizeManually.Text = "Authorize manually";
            this.btnAuthorizeManually.UseVisualStyleBackColor = true;
            this.btnAuthorizeManually.Click += new System.EventHandler(this.btnAuthorizeManually_Click);
            // 
            // btnAuthorizeAutomatically
            // 
            this.btnAuthorizeAutomatically.Location = new System.Drawing.Point(27, 96);
            this.btnAuthorizeAutomatically.Name = "btnAuthorizeAutomatically";
            this.btnAuthorizeAutomatically.Size = new System.Drawing.Size(159, 43);
            this.btnAuthorizeAutomatically.TabIndex = 11;
            this.btnAuthorizeAutomatically.Text = "Authorize automatically\r\nwith logged in account";
            this.btnAuthorizeAutomatically.UseVisualStyleBackColor = true;
            this.btnAuthorizeAutomatically.Click += new System.EventHandler(this.btnAuthorizeAutomatically_Click);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(7, 150);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(463, 45);
            this.label27.TabIndex = 13;
            this.label27.Text = resources.GetString("label27.Text");
            this.label27.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelBot
            // 
            this.labelBot.AutoSize = true;
            this.labelBot.Location = new System.Drawing.Point(28, 57);
            this.labelBot.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBot.Name = "labelBot";
            this.labelBot.Size = new System.Drawing.Size(58, 15);
            this.labelBot.TabIndex = 11;
            this.labelBot.Text = "Bot name";
            // 
            // pageGame
            // 
            this.pageGame.Controls.Add(this.groupGameSettings);
            this.pageGame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageGame.Location = new System.Drawing.Point(0, 0);
            this.pageGame.Margin = new System.Windows.Forms.Padding(0);
            this.pageGame.Name = "pageGame";
            this.pageGame.NextPage = this.pageMarkers;
            this.pageGame.Size = new System.Drawing.Size(623, 325);
            this.pageGame.TabIndex = 3;
            this.pageGame.Text = "Game settings";
            // 
            // groupGameSettings
            // 
            this.groupGameSettings.Controls.Add(this.chkEnableMapOverlay);
            this.groupGameSettings.Controls.Add(this.chkAllowSameLocationGuess);
            this.groupGameSettings.Controls.Add(this.chkUseEnglishCountryNames);
            this.groupGameSettings.Controls.Add(this.chkResetStreakOnSkippedRound);
            this.groupGameSettings.Location = new System.Drawing.Point(4, 3);
            this.groupGameSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupGameSettings.Name = "groupGameSettings";
            this.groupGameSettings.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupGameSettings.Size = new System.Drawing.Size(273, 178);
            this.groupGameSettings.TabIndex = 9;
            this.groupGameSettings.TabStop = false;
            // 
            // chkEnableMapOverlay
            // 
            this.chkEnableMapOverlay.AutoSize = true;
            this.chkEnableMapOverlay.Checked = true;
            this.chkEnableMapOverlay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableMapOverlay.Location = new System.Drawing.Point(9, 125);
            this.chkEnableMapOverlay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkEnableMapOverlay.Name = "chkEnableMapOverlay";
            this.chkEnableMapOverlay.Size = new System.Drawing.Size(219, 19);
            this.chkEnableMapOverlay.TabIndex = 10;
            this.chkEnableMapOverlay.Text = "Enable stream popup on viewer map";
            this.toolTip1.SetToolTip(this.chkEnableMapOverlay, "If you want your viewers to be able to watch you stream in a small popup on top o" +
        "f the map, enable this setting.");
            this.chkEnableMapOverlay.UseVisualStyleBackColor = true;
            // 
            // chkAllowSameLocationGuess
            // 
            this.chkAllowSameLocationGuess.AutoSize = true;
            this.chkAllowSameLocationGuess.Location = new System.Drawing.Point(9, 75);
            this.chkAllowSameLocationGuess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkAllowSameLocationGuess.Name = "chkAllowSameLocationGuess";
            this.chkAllowSameLocationGuess.Size = new System.Drawing.Size(216, 19);
            this.chkAllowSameLocationGuess.TabIndex = 9;
            this.chkAllowSameLocationGuess.Text = "Allow guessing in the same location";
            this.toolTip1.SetToolTip(this.chkAllowSameLocationGuess, "Enable this setting to allow you viewers to guess in the same location in two con" +
        "secutive rounds.");
            this.chkAllowSameLocationGuess.UseVisualStyleBackColor = true;
            // 
            // chkUseEnglishCountryNames
            // 
            this.chkUseEnglishCountryNames.AutoSize = true;
            this.chkUseEnglishCountryNames.Location = new System.Drawing.Point(9, 48);
            this.chkUseEnglishCountryNames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkUseEnglishCountryNames.Name = "chkUseEnglishCountryNames";
            this.chkUseEnglishCountryNames.Size = new System.Drawing.Size(168, 19);
            this.chkUseEnglishCountryNames.TabIndex = 8;
            this.chkUseEnglishCountryNames.Text = "Use english country names";
            this.toolTip1.SetToolTip(this.chkUseEnglishCountryNames, "Enable this setting to force english country names, independent from you Windows " +
        "language.");
            this.chkUseEnglishCountryNames.UseVisualStyleBackColor = true;
            // 
            // chkResetStreakOnSkippedRound
            // 
            this.chkResetStreakOnSkippedRound.AutoSize = true;
            this.chkResetStreakOnSkippedRound.Checked = true;
            this.chkResetStreakOnSkippedRound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResetStreakOnSkippedRound.Location = new System.Drawing.Point(9, 22);
            this.chkResetStreakOnSkippedRound.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkResetStreakOnSkippedRound.Name = "chkResetStreakOnSkippedRound";
            this.chkResetStreakOnSkippedRound.Size = new System.Drawing.Size(228, 19);
            this.chkResetStreakOnSkippedRound.TabIndex = 7;
            this.chkResetStreakOnSkippedRound.Text = "Reset country streak on skipped round";
            this.toolTip1.SetToolTip(this.chkResetStreakOnSkippedRound, "If this is enabled, viewers country streaks are reset if they skip a round or gam" +
        "e.");
            this.chkResetStreakOnSkippedRound.UseVisualStyleBackColor = true;
            // 
            // pageMarkers
            // 
            this.pageMarkers.Controls.Add(this.groupMarker);
            this.pageMarkers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageMarkers.Location = new System.Drawing.Point(0, 0);
            this.pageMarkers.Margin = new System.Windows.Forms.Padding(0);
            this.pageMarkers.Name = "pageMarkers";
            this.pageMarkers.NextPage = this.pageChat;
            this.pageMarkers.Size = new System.Drawing.Size(623, 325);
            this.pageMarkers.TabIndex = 4;
            this.pageMarkers.Text = "Marker settings";
            // 
            // groupMarker
            // 
            this.groupMarker.Controls.Add(this.chkGuessInfoTime);
            this.groupMarker.Controls.Add(this.chkGuessInfoCoordinates);
            this.groupMarker.Controls.Add(this.chkGuessInfoDistance);
            this.groupMarker.Controls.Add(this.chkGuessInfoScore);
            this.groupMarker.Controls.Add(this.chkGuessInfoStreak);
            this.groupMarker.Controls.Add(this.labelMarkers);
            this.groupMarker.Location = new System.Drawing.Point(3, 0);
            this.groupMarker.Name = "groupMarker";
            this.groupMarker.Size = new System.Drawing.Size(312, 194);
            this.groupMarker.TabIndex = 32;
            this.groupMarker.TabStop = false;
            // 
            // chkGuessInfoTime
            // 
            this.chkGuessInfoTime.AutoSize = true;
            this.chkGuessInfoTime.Checked = true;
            this.chkGuessInfoTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuessInfoTime.Location = new System.Drawing.Point(6, 159);
            this.chkGuessInfoTime.Name = "chkGuessInfoTime";
            this.chkGuessInfoTime.Size = new System.Drawing.Size(159, 19);
            this.chkGuessInfoTime.TabIndex = 5;
            this.chkGuessInfoTime.Text = "Guess time (e.g. \"18.25s\")";
            this.toolTip1.SetToolTip(this.chkGuessInfoTime, "This displays the guess time on the little guess marker on summary pages.");
            this.chkGuessInfoTime.UseVisualStyleBackColor = true;
            // 
            // chkGuessInfoCoordinates
            // 
            this.chkGuessInfoCoordinates.AutoSize = true;
            this.chkGuessInfoCoordinates.Checked = true;
            this.chkGuessInfoCoordinates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuessInfoCoordinates.Location = new System.Drawing.Point(6, 84);
            this.chkGuessInfoCoordinates.Name = "chkGuessInfoCoordinates";
            this.chkGuessInfoCoordinates.Size = new System.Drawing.Size(246, 19);
            this.chkGuessInfoCoordinates.TabIndex = 4;
            this.chkGuessInfoCoordinates.Text = "Coordinates (e.g. \"[FLAG]: -12.000, 7.000\")";
            this.toolTip1.SetToolTip(this.chkGuessInfoCoordinates, "This displays the coordinates of a guess on the little guess marker on summary pa" +
        "ges.");
            this.chkGuessInfoCoordinates.UseVisualStyleBackColor = true;
            // 
            // chkGuessInfoDistance
            // 
            this.chkGuessInfoDistance.AutoSize = true;
            this.chkGuessInfoDistance.Checked = true;
            this.chkGuessInfoDistance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuessInfoDistance.Location = new System.Drawing.Point(6, 109);
            this.chkGuessInfoDistance.Name = "chkGuessInfoDistance";
            this.chkGuessInfoDistance.Size = new System.Drawing.Size(170, 19);
            this.chkGuessInfoDistance.TabIndex = 3;
            this.chkGuessInfoDistance.Text = "Distance (e.g. \"10400.8km\")";
            this.toolTip1.SetToolTip(this.chkGuessInfoDistance, "This displays the distance of a guess on the little guess marker on summary pages" +
        ".");
            this.chkGuessInfoDistance.UseVisualStyleBackColor = true;
            // 
            // chkGuessInfoScore
            // 
            this.chkGuessInfoScore.AutoSize = true;
            this.chkGuessInfoScore.Checked = true;
            this.chkGuessInfoScore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuessInfoScore.Location = new System.Drawing.Point(6, 134);
            this.chkGuessInfoScore.Name = "chkGuessInfoScore";
            this.chkGuessInfoScore.Size = new System.Drawing.Size(146, 19);
            this.chkGuessInfoScore.TabIndex = 2;
            this.chkGuessInfoScore.Text = "Score (e.g. \"15 points\")";
            this.toolTip1.SetToolTip(this.chkGuessInfoScore, "This displays the players score on the little guess marker on summary pages.");
            this.chkGuessInfoScore.UseVisualStyleBackColor = true;
            // 
            // chkGuessInfoStreak
            // 
            this.chkGuessInfoStreak.AutoSize = true;
            this.chkGuessInfoStreak.Checked = true;
            this.chkGuessInfoStreak.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuessInfoStreak.Location = new System.Drawing.Point(6, 59);
            this.chkGuessInfoStreak.Name = "chkGuessInfoStreak";
            this.chkGuessInfoStreak.Size = new System.Drawing.Size(183, 19);
            this.chkGuessInfoStreak.TabIndex = 1;
            this.chkGuessInfoStreak.Text = "Current streak (e.g. \"5 streak\")";
            this.toolTip1.SetToolTip(this.chkGuessInfoStreak, "This displays the players current country streak on the little guess marker on su" +
        "mmary pages.");
            this.chkGuessInfoStreak.UseVisualStyleBackColor = true;
            // 
            // labelMarkers
            // 
            this.labelMarkers.AutoSize = true;
            this.labelMarkers.Location = new System.Drawing.Point(6, 19);
            this.labelMarkers.Name = "labelMarkers";
            this.labelMarkers.Size = new System.Drawing.Size(248, 30);
            this.labelMarkers.TabIndex = 0;
            this.labelMarkers.Text = "Choose which information you want to show \r\non the info popup above a guess marke" +
    "r";
            // 
            // pageChat
            // 
            this.pageChat.Controls.Add(this.flowLayoutPanel1);
            this.pageChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageChat.Location = new System.Drawing.Point(0, 0);
            this.pageChat.Margin = new System.Windows.Forms.Padding(0);
            this.pageChat.Name = "pageChat";
            this.pageChat.NextPage = this.pagePacks;
            this.pageChat.Size = new System.Drawing.Size(623, 325);
            this.pageChat.TabIndex = 2;
            this.pageChat.Text = "Chat messages";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.chkMsgsJoinChannel);
            this.flowLayoutPanel1.Controls.Add(this.chkMsgsStartGame);
            this.flowLayoutPanel1.Controls.Add(this.chkMsgsRoundStarted);
            this.flowLayoutPanel1.Controls.Add(this.chkMsgsGuessReceived);
            this.flowLayoutPanel1.Controls.Add(this.chkSendDoubleGuessMsg);
            this.flowLayoutPanel1.Controls.Add(this.chkSendSameGuessMessage);
            this.flowLayoutPanel1.Controls.Add(this.chkMsgsRoundEnd);
            this.flowLayoutPanel1.Controls.Add(this.chkMsgsGameEnded);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(321, 204);
            this.flowLayoutPanel1.TabIndex = 11;
            // 
            // chkMsgsJoinChannel
            // 
            this.chkMsgsJoinChannel.AutoSize = true;
            this.chkMsgsJoinChannel.Checked = true;
            this.chkMsgsJoinChannel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMsgsJoinChannel.Location = new System.Drawing.Point(4, 3);
            this.chkMsgsJoinChannel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkMsgsJoinChannel.Name = "chkMsgsJoinChannel";
            this.chkMsgsJoinChannel.Size = new System.Drawing.Size(203, 19);
            this.chkMsgsJoinChannel.TabIndex = 0;
            this.chkMsgsJoinChannel.Text = "Send message on joining channel";
            this.chkMsgsJoinChannel.UseVisualStyleBackColor = true;
            // 
            // chkMsgsStartGame
            // 
            this.chkMsgsStartGame.AutoSize = true;
            this.chkMsgsStartGame.Checked = true;
            this.chkMsgsStartGame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMsgsStartGame.Location = new System.Drawing.Point(4, 28);
            this.chkMsgsStartGame.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkMsgsStartGame.Name = "chkMsgsStartGame";
            this.chkMsgsStartGame.Size = new System.Drawing.Size(188, 19);
            this.chkMsgsStartGame.TabIndex = 1;
            this.chkMsgsStartGame.Text = "Send \"Game starting\" message";
            this.chkMsgsStartGame.UseVisualStyleBackColor = true;
            // 
            // chkMsgsRoundStarted
            // 
            this.chkMsgsRoundStarted.AutoSize = true;
            this.chkMsgsRoundStarted.Checked = true;
            this.chkMsgsRoundStarted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMsgsRoundStarted.Location = new System.Drawing.Point(4, 53);
            this.chkMsgsRoundStarted.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkMsgsRoundStarted.Name = "chkMsgsRoundStarted";
            this.chkMsgsRoundStarted.Size = new System.Drawing.Size(192, 19);
            this.chkMsgsRoundStarted.TabIndex = 2;
            this.chkMsgsRoundStarted.Text = "Send \"Round starting\" message";
            this.chkMsgsRoundStarted.UseVisualStyleBackColor = true;
            // 
            // chkMsgsGuessReceived
            // 
            this.chkMsgsGuessReceived.AutoSize = true;
            this.chkMsgsGuessReceived.Checked = true;
            this.chkMsgsGuessReceived.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMsgsGuessReceived.Location = new System.Drawing.Point(4, 78);
            this.chkMsgsGuessReceived.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkMsgsGuessReceived.Name = "chkMsgsGuessReceived";
            this.chkMsgsGuessReceived.Size = new System.Drawing.Size(240, 19);
            this.chkMsgsGuessReceived.TabIndex = 5;
            this.chkMsgsGuessReceived.Text = "Send message to confirm received guess";
            this.chkMsgsGuessReceived.UseVisualStyleBackColor = true;
            // 
            // chkSendDoubleGuessMsg
            // 
            this.chkSendDoubleGuessMsg.AutoSize = true;
            this.chkSendDoubleGuessMsg.Checked = true;
            this.chkSendDoubleGuessMsg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSendDoubleGuessMsg.Location = new System.Drawing.Point(4, 103);
            this.chkSendDoubleGuessMsg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSendDoubleGuessMsg.Name = "chkSendDoubleGuessMsg";
            this.chkSendDoubleGuessMsg.Size = new System.Drawing.Size(200, 19);
            this.chkSendDoubleGuessMsg.TabIndex = 8;
            this.chkSendDoubleGuessMsg.Text = "Send \"Already guessed\" message";
            this.chkSendDoubleGuessMsg.UseVisualStyleBackColor = true;
            // 
            // chkSendSameGuessMessage
            // 
            this.chkSendSameGuessMessage.AutoSize = true;
            this.chkSendSameGuessMessage.Checked = true;
            this.chkSendSameGuessMessage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSendSameGuessMessage.Location = new System.Drawing.Point(4, 128);
            this.chkSendSameGuessMessage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSendSameGuessMessage.Name = "chkSendSameGuessMessage";
            this.chkSendSameGuessMessage.Size = new System.Drawing.Size(189, 19);
            this.chkSendSameGuessMessage.TabIndex = 9;
            this.chkSendSameGuessMessage.Text = "Send \"Same location\" message";
            this.chkSendSameGuessMessage.UseVisualStyleBackColor = true;
            // 
            // chkMsgsRoundEnd
            // 
            this.chkMsgsRoundEnd.AutoSize = true;
            this.chkMsgsRoundEnd.Checked = true;
            this.chkMsgsRoundEnd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMsgsRoundEnd.Location = new System.Drawing.Point(4, 153);
            this.chkMsgsRoundEnd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkMsgsRoundEnd.Name = "chkMsgsRoundEnd";
            this.chkMsgsRoundEnd.Size = new System.Drawing.Size(194, 19);
            this.chkMsgsRoundEnd.TabIndex = 3;
            this.chkMsgsRoundEnd.Text = "Send \"Round finished\" message";
            this.chkMsgsRoundEnd.UseVisualStyleBackColor = true;
            // 
            // chkMsgsGameEnded
            // 
            this.chkMsgsGameEnded.AutoSize = true;
            this.chkMsgsGameEnded.Checked = true;
            this.chkMsgsGameEnded.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMsgsGameEnded.Location = new System.Drawing.Point(4, 178);
            this.chkMsgsGameEnded.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkMsgsGameEnded.Name = "chkMsgsGameEnded";
            this.chkMsgsGameEnded.Size = new System.Drawing.Size(181, 19);
            this.chkMsgsGameEnded.TabIndex = 4;
            this.chkMsgsGameEnded.Text = "Send \"Game ended\" message";
            this.chkMsgsGameEnded.UseVisualStyleBackColor = true;
            // 
            // pagePacks
            // 
            this.pagePacks.Controls.Add(this.labelFlagPacks);
            this.pagePacks.Controls.Add(this.button1);
            this.pagePacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagePacks.Location = new System.Drawing.Point(0, 0);
            this.pagePacks.Margin = new System.Windows.Forms.Padding(0);
            this.pagePacks.Name = "pagePacks";
            this.pagePacks.NextPage = this.pageExtensions;
            this.pagePacks.Size = new System.Drawing.Size(623, 325);
            this.pagePacks.TabIndex = 5;
            this.pagePacks.Text = "Flagpacks";
            // 
            // labelFlagPacks
            // 
            this.labelFlagPacks.AutoSize = true;
            this.labelFlagPacks.Location = new System.Drawing.Point(18, 19);
            this.labelFlagPacks.Name = "labelFlagPacks";
            this.labelFlagPacks.Size = new System.Drawing.Size(454, 105);
            this.labelFlagPacks.TabIndex = 1;
            this.labelFlagPacks.Text = resources.GetString("labelFlagPacks.Text");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(125, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Install custom packs";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pageExtensions
            // 
            this.pageExtensions.Controls.Add(this.linkLabelWiki);
            this.pageExtensions.Controls.Add(this.linkLabelUnity);
            this.pageExtensions.Controls.Add(this.linkLabelCar);
            this.pageExtensions.Controls.Add(this.labelJS);
            this.pageExtensions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageExtensions.Location = new System.Drawing.Point(0, 0);
            this.pageExtensions.Margin = new System.Windows.Forms.Padding(0);
            this.pageExtensions.Name = "pageExtensions";
            this.pageExtensions.NextPage = this.pageFormulars;
            this.pageExtensions.Size = new System.Drawing.Size(623, 325);
            this.pageExtensions.TabIndex = 6;
            this.pageExtensions.Text = "JS Extensions";
            // 
            // linkLabelWiki
            // 
            this.linkLabelWiki.AutoSize = true;
            this.linkLabelWiki.Location = new System.Drawing.Point(104, 89);
            this.linkLabelWiki.Name = "linkLabelWiki";
            this.linkLabelWiki.Size = new System.Drawing.Size(103, 15);
            this.linkLabelWiki.TabIndex = 3;
            this.linkLabelWiki.TabStop = true;
            this.linkLabelWiki.Text = "Install instructions";
            this.linkLabelWiki.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWiki_LinkClicked);
            // 
            // linkLabelUnity
            // 
            this.linkLabelUnity.AutoSize = true;
            this.linkLabelUnity.Location = new System.Drawing.Point(88, 74);
            this.linkLabelUnity.Name = "linkLabelUnity";
            this.linkLabelUnity.Size = new System.Drawing.Size(103, 15);
            this.linkLabelUnity.TabIndex = 2;
            this.linkLabelUnity.TabStop = true;
            this.linkLabelUnity.Text = "Install instructions";
            this.linkLabelUnity.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUnity_LinkClicked);
            // 
            // linkLabelCar
            // 
            this.linkLabelCar.AutoSize = true;
            this.linkLabelCar.Location = new System.Drawing.Point(168, 59);
            this.linkLabelCar.Name = "linkLabelCar";
            this.linkLabelCar.Size = new System.Drawing.Size(103, 15);
            this.linkLabelCar.TabIndex = 1;
            this.linkLabelCar.TabStop = true;
            this.linkLabelCar.Text = "Install instructions";
            this.linkLabelCar.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCar_LinkClicked);
            // 
            // labelJS
            // 
            this.labelJS.AutoSize = true;
            this.labelJS.Location = new System.Drawing.Point(11, 14);
            this.labelJS.Name = "labelJS";
            this.labelJS.Size = new System.Drawing.Size(435, 90);
            this.labelJS.TabIndex = 0;
            this.labelJS.Text = "GeoChatter supports the installation and use of GeoGuessr JavaScript extentsions!" +
    "\r\n\r\nYou can install and use scripts like:\r\n- No car and compass script:\r\n- Unity" +
    " script:\r\n- Wiki extension";
            // 
            // pageFormulars
            // 
            this.pageFormulars.Controls.Add(this.lblFormulas);
            this.pageFormulars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageFormulars.Location = new System.Drawing.Point(0, 0);
            this.pageFormulars.Margin = new System.Windows.Forms.Padding(0);
            this.pageFormulars.Name = "pageFormulars";
            this.pageFormulars.NextPage = this.PageFinish;
            this.pageFormulars.Size = new System.Drawing.Size(623, 325);
            this.pageFormulars.TabIndex = 7;
            this.pageFormulars.Text = "Score formulas";
            // 
            // lblFormulas
            // 
            this.lblFormulas.AutoSize = true;
            this.lblFormulas.Location = new System.Drawing.Point(3, 12);
            this.lblFormulas.Name = "lblFormulas";
            this.lblFormulas.Size = new System.Drawing.Size(378, 150);
            this.lblFormulas.TabIndex = 0;
            this.lblFormulas.Text = resources.GetString("lblFormulas.Text");
            // 
            // PageFinish
            // 
            this.PageFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PageFinish.IsFinishPage = true;
            this.PageFinish.Location = new System.Drawing.Point(0, 0);
            this.PageFinish.Margin = new System.Windows.Forms.Padding(0);
            this.PageFinish.Name = "PageFinish";
            this.PageFinish.Size = new System.Drawing.Size(623, 325);
            this.PageFinish.TabIndex = 8;
            this.PageFinish.Text = "That\'s all folks!";
            // 
            // linkLabelOauth
            // 
            this.linkLabelOauth.AutoSize = true;
            this.linkLabelOauth.Location = new System.Drawing.Point(28, 111);
            this.linkLabelOauth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabelOauth.Name = "linkLabelOauth";
            this.linkLabelOauth.Size = new System.Drawing.Size(304, 30);
            this.linkLabelOauth.TabIndex = 18;
            this.linkLabelOauth.TabStop = true;
            this.linkLabelOauth.Text = "Get the oAuth-token here:  https://twitchapps.com/tmi/\r\n(Logged in as the bot use" +
    "r)";
            this.linkLabelOauth.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel5_LinkClicked);
            // 
            // txtGeneralOauthToken
            // 
            this.txtGeneralOauthToken.Location = new System.Drawing.Point(159, 84);
            this.txtGeneralOauthToken.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtGeneralOauthToken.Name = "txtGeneralOauthToken";
            this.txtGeneralOauthToken.PasswordChar = '*';
            this.txtGeneralOauthToken.Size = new System.Drawing.Size(243, 23);
            this.txtGeneralOauthToken.TabIndex = 16;
            this.toolTip1.SetToolTip(this.txtGeneralOauthToken, resources.GetString("txtGeneralOauthToken.ToolTip"));
            // 
            // labelOauth
            // 
            this.labelOauth.AutoSize = true;
            this.labelOauth.Location = new System.Drawing.Point(28, 87);
            this.labelOauth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOauth.Name = "labelOauth";
            this.labelOauth.Size = new System.Drawing.Size(77, 15);
            this.labelOauth.TabIndex = 13;
            this.labelOauth.Text = "OAuth-token";
            // 
            // SetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 479);
            this.Controls.Add(this.wizardControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupWizard";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Initial Setup";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
            this.pageWelcome.ResumeLayout(false);
            this.pageWelcome.PerformLayout();
            this.pageConnections.ResumeLayout(false);
            this.pageConnections.PerformLayout();
            this.pageGame.ResumeLayout(false);
            this.groupGameSettings.ResumeLayout(false);
            this.groupGameSettings.PerformLayout();
            this.pageMarkers.ResumeLayout(false);
            this.groupMarker.ResumeLayout(false);
            this.groupMarker.PerformLayout();
            this.pageChat.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.pagePacks.ResumeLayout(false);
            this.pagePacks.PerformLayout();
            this.pageExtensions.ResumeLayout(false);
            this.pageExtensions.PerformLayout();
            this.pageFormulars.ResumeLayout(false);
            this.pageFormulars.PerformLayout();
            this.ResumeLayout(false);

        }

       

        #endregion

        private AeroWizard.WizardControl wizardControl1;
        private AeroWizard.WizardPage pageWelcome;
        private System.Windows.Forms.Label labelWelcome;
        private AeroWizard.WizardPage pageConnections;
        private System.Windows.Forms.Button reconnectTwitchBotButton;
        private System.Windows.Forms.LinkLabel linkLabelOauth;
        private System.Windows.Forms.TextBox txtGeneralOauthToken;
        private System.Windows.Forms.TextBox txtGeneralChannelName;
        private System.Windows.Forms.TextBox txtGeneralBotName;
        private System.Windows.Forms.Label labelOauth;
        private System.Windows.Forms.Label labelChannelName;
        private System.Windows.Forms.Label labelBot;
        private AeroWizard.WizardPage pageChat;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox chkMsgsJoinChannel;
        private System.Windows.Forms.CheckBox chkMsgsStartGame;
        private System.Windows.Forms.CheckBox chkMsgsRoundStarted;
        private System.Windows.Forms.CheckBox chkMsgsGuessReceived;
        private System.Windows.Forms.CheckBox chkSendDoubleGuessMsg;
        private System.Windows.Forms.CheckBox chkSendSameGuessMessage;
        private System.Windows.Forms.CheckBox chkMsgsRoundEnd;
        private System.Windows.Forms.CheckBox chkMsgsGameEnded;
        private AeroWizard.WizardPage pageGame;
        private System.Windows.Forms.GroupBox groupGameSettings;
        private System.Windows.Forms.CheckBox chkEnableMapOverlay;
        private System.Windows.Forms.CheckBox chkAllowSameLocationGuess;
        private System.Windows.Forms.CheckBox chkUseEnglishCountryNames;
        private System.Windows.Forms.CheckBox chkResetStreakOnSkippedRound;
        private AeroWizard.WizardPage pageMarkers;
        private System.Windows.Forms.GroupBox groupMarker;
        private System.Windows.Forms.CheckBox chkGuessInfoTime;
        private System.Windows.Forms.CheckBox chkGuessInfoCoordinates;
        private System.Windows.Forms.CheckBox chkGuessInfoDistance;
        private System.Windows.Forms.CheckBox chkGuessInfoScore;
        private System.Windows.Forms.CheckBox chkGuessInfoStreak;
        private System.Windows.Forms.Label labelMarkers;
        private AeroWizard.WizardPage pagePacks;
        private System.Windows.Forms.Label labelFlagPacks;
        private System.Windows.Forms.Button button1;
        private AeroWizard.WizardPage pageExtensions;
        private System.Windows.Forms.LinkLabel linkLabelCar;
        private System.Windows.Forms.Label labelJS;
        private System.Windows.Forms.LinkLabel linkLabelWiki;
        private System.Windows.Forms.LinkLabel linkLabelUnity;
        private System.Windows.Forms.ToolTip toolTip1;
        private AeroWizard.WizardPage pageFormulars;
        private System.Windows.Forms.Label lblFormulas;
        private AeroWizard.WizardPage PageFinish;
        private System.Windows.Forms.Button btnAuthorizeAutomatically;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button btnAuthorizeManually;
    }
}

#pragma warning restore CS1591,CA1062
