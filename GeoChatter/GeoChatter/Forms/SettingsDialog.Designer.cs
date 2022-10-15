#pragma warning disable CS1591,CA1062


namespace GeoChatter.Forms
{
    partial class SettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.streamerFlagDisplay = new System.Windows.Forms.Button();
            this.label42 = new System.Windows.Forms.Label();
            this.randomFlag = new System.Windows.Forms.Button();
            this.label41 = new System.Windows.Forms.Label();
            this.streamerColor = new System.Windows.Forms.Button();
            this.streamerFlag = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStreamerDisplayname = new System.Windows.Forms.TextBox();
            this.grpViewermap = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableTempGuesses = new System.Windows.Forms.CheckBox();
            this.chkShowBorders = new System.Windows.Forms.CheckBox();
            this.chkEnableMapOverlay = new System.Windows.Forms.CheckBox();
            this.chkShowFlags = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.scrSettings = new GeoChatter.Controls.ShortcutRecorderControl();
            this.label38 = new System.Windows.Forms.Label();
            this.scrFullscreen = new GeoChatter.Controls.ShortcutRecorderControl();
            this.label37 = new System.Windows.Forms.Label();
            this.scrMenu = new GeoChatter.Controls.ShortcutRecorderControl();
            this.label36 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkEnableDebugLogging = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.groupGameSettings = new System.Windows.Forms.GroupBox();
            this.allowCustomRandomGuesses = new System.Windows.Forms.CheckBox();
            this.chkCheckStreamer = new System.Windows.Forms.CheckBox();
            this.chkAllowSameLocationGuess = new System.Windows.Forms.CheckBox();
            this.chkUseEnglishCountryNames = new System.Windows.Forms.CheckBox();
            this.chkResetStreakOnSkippedRound = new System.Windows.Forms.CheckBox();
            this.tabTwitch = new System.Windows.Forms.TabPage();
            this.groupBoxChatMessages = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableChatMsgs = new System.Windows.Forms.CheckBox();
            this.grpTwitchChatConnection = new System.Windows.Forms.GroupBox();
            this.btnForgetTwitch = new System.Windows.Forms.Button();
            this.label27 = new System.Windows.Forms.Label();
            this.btnAuthorizeManually = new System.Windows.Forms.Button();
            this.btnAuthorizeAutomatically = new System.Windows.Forms.Button();
            this.reconnectTwitchBotButton = new System.Windows.Forms.Button();
            this.txtGeneralOauthToken = new System.Windows.Forms.TextBox();
            this.txtGeneralChannelName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageConnections = new System.Windows.Forms.TabPage();
            this.groupBoxOtherChatGateways = new System.Windows.Forms.GroupBox();
            this.checkBoxSendChatMsgsViaStreamerBot = new System.Windows.Forms.CheckBox();
            this.grpObsConnection = new System.Windows.Forms.GroupBox();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtObsPassword = new System.Windows.Forms.TextBox();
            this.chkObsConnectAtStartup = new System.Windows.Forms.CheckBox();
            this.btnConnectObs = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtObsPort = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtObsIp = new System.Windows.Forms.TextBox();
            this.grpStreamerBotConnection = new System.Windows.Forms.GroupBox();
            this.chkStreamerBotConnectAtStartup = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStreamerBotPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStreamerBotIP = new System.Windows.Forms.TextBox();
            this.buttonConnectStreamerBot = new System.Windows.Forms.Button();
            this.tabChatMessages = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label43 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkMsgsJoinChannel = new System.Windows.Forms.CheckBox();
            this.chkMsgsStartGame = new System.Windows.Forms.CheckBox();
            this.chkMsgsRoundStarted = new System.Windows.Forms.CheckBox();
            this.chkMsgsGuessReceived = new System.Windows.Forms.CheckBox();
            this.chkSendDoubleGuessMsg = new System.Windows.Forms.CheckBox();
            this.chkSendSameGuessMessage = new System.Windows.Forms.CheckBox();
            this.chkMsgsRoundEnd = new System.Windows.Forms.CheckBox();
            this.chkMsgsGameEnded = new System.Windows.Forms.CheckBox();
            this.chkColorMessage = new System.Windows.Forms.CheckBox();
            this.chkFlagMessages = new System.Windows.Forms.CheckBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tabPageEvents = new System.Windows.Forms.TabPage();
            this.groupBoxGameEnd = new System.Windows.Forms.GroupBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.comboOBSGameEndSource = new System.Windows.Forms.ComboBox();
            this.comboOBSGameEndAction = new System.Windows.Forms.ComboBox();
            this.comboOBSGameEndScene = new System.Windows.Forms.ComboBox();
            this.chkOBSGameEndExecute = new System.Windows.Forms.CheckBox();
            this.chkBotGameEndExecute = new System.Windows.Forms.CheckBox();
            this.groupBoxRoundEnd = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.comboOBSRoundEndSource = new System.Windows.Forms.ComboBox();
            this.comboOBSRoundEndAction = new System.Windows.Forms.ComboBox();
            this.comboOBSRoundEndScene = new System.Windows.Forms.ComboBox();
            this.chkOBSRoundEndExecute = new System.Windows.Forms.CheckBox();
            this.chkBotRoundEndExecute = new System.Windows.Forms.CheckBox();
            this.groupBoxRoundTimer = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.comboRoundTimerObsSource = new System.Windows.Forms.ComboBox();
            this.comboRoundTimerObsAction = new System.Windows.Forms.ComboBox();
            this.comboRoundTimerObsScene = new System.Windows.Forms.ComboBox();
            this.chkRoundTimerOBS = new System.Windows.Forms.CheckBox();
            this.checkBoxRoundTimerExecuteStreamerBotAction = new System.Windows.Forms.CheckBox();
            this.groupBoxEventDistance = new System.Windows.Forms.GroupBox();
            this.chkSpecialDistanceRange = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtSpecialDistanceTo = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.comboSpecialDistanceObsScene = new System.Windows.Forms.ComboBox();
            this.comboSpecialDistanceObsAction = new System.Windows.Forms.ComboBox();
            this.comboSpecialDistanceObsSource = new System.Windows.Forms.ComboBox();
            this.checkBoxSpecialDistanceAction = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkSpecialDistanceObs = new System.Windows.Forms.CheckBox();
            this.txtSpecialDistanceFrom = new System.Windows.Forms.TextBox();
            this.checkBoxSpecialDistanceLabel = new System.Windows.Forms.CheckBox();
            this.groupBoxEventSpecial = new System.Windows.Forms.GroupBox();
            this.chkSpecialScoreRange = new System.Windows.Forms.CheckBox();
            this.txtSpecialScoreTo = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.comboSpecialScoreObsAction = new System.Windows.Forms.ComboBox();
            this.comboSpecialScoreObsScene = new System.Windows.Forms.ComboBox();
            this.comboSpecialScoreObsSource = new System.Windows.Forms.ComboBox();
            this.chkSpecialScoreObs = new System.Windows.Forms.CheckBox();
            this.checkBoxSpecialScoreAction = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSpecialScoreFrom = new System.Windows.Forms.TextBox();
            this.checkBoxSpecialScoreLabel = new System.Windows.Forms.CheckBox();
            this.tabPageLabels = new System.Windows.Forms.TabPage();
            this.groupBoxEventGeneral = new System.Windows.Forms.GroupBox();
            this.btnSelectLabelFolder = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLabelPath = new System.Windows.Forms.TextBox();
            this.groupBoxEventGameEnd = new System.Windows.Forms.GroupBox();
            this.checkBoxEventWriteGameHighscore = new System.Windows.Forms.CheckBox();
            this.checkBoxEventWriteGameThird = new System.Windows.Forms.CheckBox();
            this.checkBoxEventWriteGameSecond = new System.Windows.Forms.CheckBox();
            this.checkBoxEventWriteGameWinner = new System.Windows.Forms.CheckBox();
            this.groupBoxEventRoundEnd = new System.Windows.Forms.GroupBox();
            this.checkBoxEventWriteRoundHighscore = new System.Windows.Forms.CheckBox();
            this.checkBoxEventWriteRoundThird = new System.Windows.Forms.CheckBox();
            this.checkBoxEventWriteRoundSecond = new System.Windows.Forms.CheckBox();
            this.checkBoxEventWriteRoundWinner = new System.Windows.Forms.CheckBox();
            this.tabPageOverlay = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupScoreboard = new System.Windows.Forms.GroupBox();
            this.flowScoreboard = new System.Windows.Forms.FlowLayoutPanel();
            this.panelScoreBoardUnit = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxOverlayUnits = new System.Windows.Forms.ComboBox();
            this.panelScoreboardRounding = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.numericRoundingDigit = new System.Windows.Forms.NumericUpDown();
            this.panelScoreboardFontSize = new System.Windows.Forms.Panel();
            this.label28 = new System.Windows.Forms.Label();
            this.OverlayFontSizetextBox1 = new System.Windows.Forms.TextBox();
            this.OverlayFontSizeUnitcomboBox1 = new System.Windows.Forms.ComboBox();
            this.panelScoreBoardColors = new System.Windows.Forms.Panel();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.ScoreboardFGDisplaybutton = new System.Windows.Forms.Button();
            this.ScoreboardBGDisplaybutton = new System.Windows.Forms.Button();
            this.label31 = new System.Windows.Forms.Label();
            this.ScoreboardFGAnumericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.ScoreboardBGAnumericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label32 = new System.Windows.Forms.Label();
            this.panelScoreBoardSpeed = new System.Windows.Forms.Panel();
            this.label34 = new System.Windows.Forms.Label();
            this.ScoreboardScrollSpeednumericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label33 = new System.Windows.Forms.Label();
            this.panelScoreboardCheckboxes = new System.Windows.Forms.Panel();
            this.chkUseUsStateFlags = new System.Windows.Forms.CheckBox();
            this.checkBoxOverlayDisplayCorrectLocations = new System.Windows.Forms.CheckBox();
            this.checkBoxOverlayRegionalFlags = new System.Windows.Forms.CheckBox();
            this.checkBoxOverlayUseWrongRegionColors = new System.Windows.Forms.CheckBox();
            this.groupMarker = new System.Windows.Forms.GroupBox();
            this.chkGuessInfoTime = new System.Windows.Forms.CheckBox();
            this.chkGuessInfoCoordinates = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMaxGuessDisplayCount = new System.Windows.Forms.Label();
            this.numMaxGuessDisplayCount = new System.Windows.Forms.NumericUpDown();
            this.lblNoOfMarkers = new System.Windows.Forms.Label();
            this.numMaxMarkerCount = new System.Windows.Forms.NumericUpDown();
            this.checkBoxMarkerClustersEnabled = new System.Windows.Forms.CheckBox();
            this.chkGuessInfoDistance = new System.Windows.Forms.CheckBox();
            this.chkGuessInfoScore = new System.Windows.Forms.CheckBox();
            this.chkGuessInfoStreak = new System.Windows.Forms.CheckBox();
            this.label39 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.chkAutoBanCGTrolls = new System.Windows.Forms.CheckBox();
            this.chkAutoBan = new System.Windows.Forms.CheckBox();
            this.label35 = new System.Windows.Forms.Label();
            this.chkListBoxBannedPlayers = new System.Windows.Forms.CheckedListBox();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.linkLabel8 = new System.Windows.Forms.LinkLabel();
            this.linkLabel26 = new System.Windows.Forms.LinkLabel();
            this.linkLabel10 = new System.Windows.Forms.LinkLabel();
            this.linkLabel11 = new System.Windows.Forms.LinkLabel();
            this.linkLabel12 = new System.Windows.Forms.LinkLabel();
            this.linkLabel31 = new System.Windows.Forms.LinkLabel();
            this.linkLabel18 = new System.Windows.Forms.LinkLabel();
            this.linkLabel17 = new System.Windows.Forms.LinkLabel();
            this.linkLabel16 = new System.Windows.Forms.LinkLabel();
            this.linkLabel35 = new System.Windows.Forms.LinkLabel();
            this.linkLabel15 = new System.Windows.Forms.LinkLabel();
            this.linkLabel14 = new System.Windows.Forms.LinkLabel();
            this.linkLabel13 = new System.Windows.Forms.LinkLabel();
            this.linkLabel33 = new System.Windows.Forms.LinkLabel();
            this.linkLabel9 = new System.Windows.Forms.LinkLabel();
            this.linkLabel24 = new System.Windows.Forms.LinkLabel();
            this.linkLabel23 = new System.Windows.Forms.LinkLabel();
            this.linkLabel34 = new System.Windows.Forms.LinkLabel();
            this.linkLabel22 = new System.Windows.Forms.LinkLabel();
            this.linkLabel21 = new System.Windows.Forms.LinkLabel();
            this.linkLabel20 = new System.Windows.Forms.LinkLabel();
            this.linkLabel19 = new System.Windows.Forms.LinkLabel();
            this.linkLabel32 = new System.Windows.Forms.LinkLabel();
            this.linkLabel30 = new System.Windows.Forms.LinkLabel();
            this.linkLabel25 = new System.Windows.Forms.LinkLabel();
            this.linkLabel29 = new System.Windows.Forms.LinkLabel();
            this.linkLabel28 = new System.Windows.Forms.LinkLabel();
            this.linkLabel27 = new System.Windows.Forms.LinkLabel();
            this.label40 = new System.Windows.Forms.Label();
            this.linkLabel7 = new System.Windows.Forms.LinkLabel();
            this.linkLabel6 = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label16 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label15 = new System.Windows.Forms.Label();
            this.tabDevelopment = new System.Windows.Forms.TabPage();
            this.chkDEVEnableRandomBotGuess = new System.Windows.Forms.CheckBox();
            this.chkDEVShowDevTools = new System.Windows.Forms.CheckBox();
            this.chkDEVUseDevApi = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnShowAdvanced = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.folderBrowserLabelPath = new System.Windows.Forms.FolderBrowserDialog();
            this.usernameColorDialog = new System.Windows.Forms.ColorDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpViewermap.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupGameSettings.SuspendLayout();
            this.tabTwitch.SuspendLayout();
            this.groupBoxChatMessages.SuspendLayout();
            this.grpTwitchChatConnection.SuspendLayout();
            this.tabPageConnections.SuspendLayout();
            this.groupBoxOtherChatGateways.SuspendLayout();
            this.grpObsConnection.SuspendLayout();
            this.grpStreamerBotConnection.SuspendLayout();
            this.tabChatMessages.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabPageEvents.SuspendLayout();
            this.groupBoxGameEnd.SuspendLayout();
            this.groupBoxRoundEnd.SuspendLayout();
            this.groupBoxRoundTimer.SuspendLayout();
            this.groupBoxEventDistance.SuspendLayout();
            this.groupBoxEventSpecial.SuspendLayout();
            this.tabPageLabels.SuspendLayout();
            this.groupBoxEventGeneral.SuspendLayout();
            this.groupBoxEventGameEnd.SuspendLayout();
            this.groupBoxEventRoundEnd.SuspendLayout();
            this.tabPageOverlay.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupScoreboard.SuspendLayout();
            this.flowScoreboard.SuspendLayout();
            this.panelScoreBoardUnit.SuspendLayout();
            this.panelScoreboardRounding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericRoundingDigit)).BeginInit();
            this.panelScoreboardFontSize.SuspendLayout();
            this.panelScoreBoardColors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScoreboardFGAnumericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScoreboardBGAnumericUpDown2)).BeginInit();
            this.panelScoreBoardSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScoreboardScrollSpeednumericUpDown1)).BeginInit();
            this.panelScoreboardCheckboxes.SuspendLayout();
            this.groupMarker.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxGuessDisplayCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxMarkerCount)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabUsers.SuspendLayout();
            this.tabPageAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel3.SuspendLayout();
            this.tabDevelopment.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnApply, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnShowAdvanced, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.02F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1063, 743);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(535, 703);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(524, 37);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabControl1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 2);
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabTwitch);
            this.tabControl1.Controls.Add(this.tabPageConnections);
            this.tabControl1.Controls.Add(this.tabChatMessages);
            this.tabControl1.Controls.Add(this.tabPageEvents);
            this.tabControl1.Controls.Add(this.tabPageLabels);
            this.tabControl1.Controls.Add(this.tabPageOverlay);
            this.tabControl1.Controls.Add(this.tabUsers);
            this.tabControl1.Controls.Add(this.tabPageAbout);
            this.tabControl1.Controls.Add(this.tabDevelopment);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(4, 3);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabControl1.Name = "tabControl1";
            this.tableLayoutPanel1.SetRowSpan(this.tabControl1, 2);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1055, 650);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.groupBox2);
            this.tabGeneral.Controls.Add(this.grpViewermap);
            this.tabGeneral.Controls.Add(this.groupBox6);
            this.tabGeneral.Controls.Add(this.groupBox5);
            this.tabGeneral.Controls.Add(this.groupGameSettings);
            this.tabGeneral.Location = new System.Drawing.Point(4, 24);
            this.tabGeneral.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabGeneral.Size = new System.Drawing.Size(1047, 622);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.streamerFlagDisplay);
            this.groupBox2.Controls.Add(this.label42);
            this.groupBox2.Controls.Add(this.randomFlag);
            this.groupBox2.Controls.Add(this.label41);
            this.groupBox2.Controls.Add(this.streamerColor);
            this.groupBox2.Controls.Add(this.streamerFlag);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtStreamerDisplayname);
            this.groupBox2.Location = new System.Drawing.Point(7, 313);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(415, 131);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Streamer";
            // 
            // streamerFlagDisplay
            // 
            this.streamerFlagDisplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.streamerFlagDisplay.Location = new System.Drawing.Point(34, 45);
            this.streamerFlagDisplay.Name = "streamerFlagDisplay";
            this.streamerFlagDisplay.Size = new System.Drawing.Size(28, 23);
            this.streamerFlagDisplay.TabIndex = 20;
            this.streamerFlagDisplay.UseVisualStyleBackColor = true;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(63, 49);
            this.label42.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(57, 15);
            this.label42.TabIndex = 19;
            this.label42.Text = "Your flag:";
            // 
            // randomFlag
            // 
            this.randomFlag.Location = new System.Drawing.Point(341, 45);
            this.randomFlag.Name = "randomFlag";
            this.randomFlag.Size = new System.Drawing.Size(67, 23);
            this.randomFlag.TabIndex = 18;
            this.randomFlag.Text = "Random";
            this.randomFlag.UseVisualStyleBackColor = true;
            this.randomFlag.Click += new System.EventHandler(this.randomFlag_Click);
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(1, 78);
            this.label41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(119, 15);
            this.label41.TabIndex = 17;
            this.label41.Text = "Your username color:";
            // 
            // streamerColor
            // 
            this.streamerColor.Location = new System.Drawing.Point(123, 74);
            this.streamerColor.Name = "streamerColor";
            this.streamerColor.Size = new System.Drawing.Size(23, 23);
            this.streamerColor.TabIndex = 16;
            this.streamerColor.UseVisualStyleBackColor = true;
            this.streamerColor.Click += new System.EventHandler(this.streamerColor_Click);
            // 
            // streamerFlag
            // 
            this.streamerFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.streamerFlag.FormattingEnabled = true;
            this.streamerFlag.Location = new System.Drawing.Point(123, 45);
            this.streamerFlag.Name = "streamerFlag";
            this.streamerFlag.Size = new System.Drawing.Size(212, 23);
            this.streamerFlag.TabIndex = 14;
            this.streamerFlag.SelectedIndexChanged += new System.EventHandler(this.streamerFlag_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "Your display name:";
            // 
            // txtStreamerDisplayname
            // 
            this.txtStreamerDisplayname.Location = new System.Drawing.Point(123, 16);
            this.txtStreamerDisplayname.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtStreamerDisplayname.Name = "txtStreamerDisplayname";
            this.txtStreamerDisplayname.Size = new System.Drawing.Size(212, 23);
            this.txtStreamerDisplayname.TabIndex = 4;
            // 
            // grpViewermap
            // 
            this.grpViewermap.Controls.Add(this.checkBoxEnableTempGuesses);
            this.grpViewermap.Controls.Add(this.chkShowBorders);
            this.grpViewermap.Controls.Add(this.chkEnableMapOverlay);
            this.grpViewermap.Controls.Add(this.chkShowFlags);
            this.grpViewermap.Location = new System.Drawing.Point(430, 176);
            this.grpViewermap.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpViewermap.Name = "grpViewermap";
            this.grpViewermap.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpViewermap.Size = new System.Drawing.Size(578, 131);
            this.grpViewermap.TabIndex = 16;
            this.grpViewermap.TabStop = false;
            this.grpViewermap.Text = "Viewer-Map settings";
            // 
            // checkBoxEnableTempGuesses
            // 
            this.checkBoxEnableTempGuesses.AutoSize = true;
            this.checkBoxEnableTempGuesses.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBoxEnableTempGuesses.ForeColor = System.Drawing.Color.Black;
            this.checkBoxEnableTempGuesses.Location = new System.Drawing.Point(8, 98);
            this.checkBoxEnableTempGuesses.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEnableTempGuesses.Name = "checkBoxEnableTempGuesses";
            this.checkBoxEnableTempGuesses.Size = new System.Drawing.Size(331, 19);
            this.checkBoxEnableTempGuesses.TabIndex = 11;
            this.checkBoxEnableTempGuesses.Text = "Enable temporary guesses (same behaviour as GeoGuessr)";
            this.checkBoxEnableTempGuesses.UseVisualStyleBackColor = true;
            // 
            // chkShowBorders
            // 
            this.chkShowBorders.AutoSize = true;
            this.chkShowBorders.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chkShowBorders.ForeColor = System.Drawing.Color.Black;
            this.chkShowBorders.Location = new System.Drawing.Point(8, 73);
            this.chkShowBorders.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkShowBorders.Name = "chkShowBorders";
            this.chkShowBorders.Size = new System.Drawing.Size(142, 19);
            this.chkShowBorders.TabIndex = 9;
            this.chkShowBorders.Text = "Show borders on map";
            this.chkShowBorders.UseVisualStyleBackColor = true;
            // 
            // chkEnableMapOverlay
            // 
            this.chkEnableMapOverlay.AutoSize = true;
            this.chkEnableMapOverlay.Location = new System.Drawing.Point(8, 22);
            this.chkEnableMapOverlay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkEnableMapOverlay.Name = "chkEnableMapOverlay";
            this.chkEnableMapOverlay.Size = new System.Drawing.Size(457, 19);
            this.chkEnableMapOverlay.TabIndex = 10;
            this.chkEnableMapOverlay.Text = "Enable stream popup on viewer map (requires a configured Twitch channel name)";
            this.toolTip1.SetToolTip(this.chkEnableMapOverlay, "Enable the stream popup on top of the viewer map");
            this.chkEnableMapOverlay.UseVisualStyleBackColor = true;
            // 
            // chkShowFlags
            // 
            this.chkShowFlags.AutoSize = true;
            this.chkShowFlags.Location = new System.Drawing.Point(8, 48);
            this.chkShowFlags.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkShowFlags.Name = "chkShowFlags";
            this.chkShowFlags.Size = new System.Drawing.Size(127, 19);
            this.chkShowFlags.TabIndex = 8;
            this.chkShowFlags.Text = "Show flags on map";
            this.chkShowFlags.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.scrSettings);
            this.groupBox6.Controls.Add(this.label38);
            this.groupBox6.Controls.Add(this.scrFullscreen);
            this.groupBox6.Controls.Add(this.label37);
            this.groupBox6.Controls.Add(this.scrMenu);
            this.groupBox6.Controls.Add(this.label36);
            this.groupBox6.Location = new System.Drawing.Point(430, 8);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox6.Size = new System.Drawing.Size(578, 162);
            this.groupBox6.TabIndex = 15;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Shortcuts";
            // 
            // scrSettings
            // 
            this.scrSettings.KeyCode = 0;
            this.scrSettings.Location = new System.Drawing.Point(74, 22);
            this.scrSettings.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.scrSettings.Modifiers = System.Windows.Forms.Keys.None;
            this.scrSettings.Name = "scrSettings";
            this.scrSettings.Size = new System.Drawing.Size(366, 32);
            this.scrSettings.TabIndex = 9;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(10, 110);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(38, 15);
            this.label38.TabIndex = 14;
            this.label38.Text = "Menu";
            // 
            // scrFullscreen
            // 
            this.scrFullscreen.KeyCode = 0;
            this.scrFullscreen.Location = new System.Drawing.Point(74, 61);
            this.scrFullscreen.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.scrFullscreen.Modifiers = System.Windows.Forms.Keys.None;
            this.scrFullscreen.Name = "scrFullscreen";
            this.scrFullscreen.Size = new System.Drawing.Size(366, 32);
            this.scrFullscreen.TabIndex = 10;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(10, 69);
            this.label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(60, 15);
            this.label37.TabIndex = 13;
            this.label37.Text = "Fullscreen";
            // 
            // scrMenu
            // 
            this.scrMenu.KeyCode = 0;
            this.scrMenu.Location = new System.Drawing.Point(74, 100);
            this.scrMenu.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.scrMenu.Modifiers = System.Windows.Forms.Keys.None;
            this.scrMenu.Name = "scrMenu";
            this.scrMenu.Size = new System.Drawing.Size(366, 32);
            this.scrMenu.TabIndex = 11;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(10, 30);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(49, 15);
            this.label36.TabIndex = 12;
            this.label36.Text = "Settings";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chkEnableDebugLogging);
            this.groupBox5.Controls.Add(this.checkBoxAutoUpdate);
            this.groupBox5.Location = new System.Drawing.Point(7, 7);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox5.Size = new System.Drawing.Size(415, 115);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Application settings";
            // 
            // chkEnableDebugLogging
            // 
            this.chkEnableDebugLogging.AutoSize = true;
            this.chkEnableDebugLogging.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkEnableDebugLogging.ForeColor = System.Drawing.Color.Red;
            this.chkEnableDebugLogging.Location = new System.Drawing.Point(7, 47);
            this.chkEnableDebugLogging.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkEnableDebugLogging.Name = "chkEnableDebugLogging";
            this.chkEnableDebugLogging.Size = new System.Drawing.Size(144, 19);
            this.chkEnableDebugLogging.TabIndex = 9;
            this.chkEnableDebugLogging.Text = "Enable debug logging";
            this.chkEnableDebugLogging.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoUpdate
            // 
            this.checkBoxAutoUpdate.AutoSize = true;
            this.checkBoxAutoUpdate.Location = new System.Drawing.Point(7, 22);
            this.checkBoxAutoUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            this.checkBoxAutoUpdate.Size = new System.Drawing.Size(196, 19);
            this.checkBoxAutoUpdate.TabIndex = 7;
            this.checkBoxAutoUpdate.Text = "Check for new version at startup";
            this.checkBoxAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // groupGameSettings
            // 
            this.groupGameSettings.Controls.Add(this.allowCustomRandomGuesses);
            this.groupGameSettings.Controls.Add(this.chkCheckStreamer);
            this.groupGameSettings.Controls.Add(this.chkAllowSameLocationGuess);
            this.groupGameSettings.Controls.Add(this.chkUseEnglishCountryNames);
            this.groupGameSettings.Controls.Add(this.chkResetStreakOnSkippedRound);
            this.groupGameSettings.Location = new System.Drawing.Point(7, 129);
            this.groupGameSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupGameSettings.Name = "groupGameSettings";
            this.groupGameSettings.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupGameSettings.Size = new System.Drawing.Size(415, 178);
            this.groupGameSettings.TabIndex = 7;
            this.groupGameSettings.TabStop = false;
            this.groupGameSettings.Text = "Game settings";
            this.groupGameSettings.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // allowCustomRandomGuesses
            // 
            this.allowCustomRandomGuesses.AutoSize = true;
            this.allowCustomRandomGuesses.Location = new System.Drawing.Point(7, 145);
            this.allowCustomRandomGuesses.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.allowCustomRandomGuesses.Name = "allowCustomRandomGuesses";
            this.allowCustomRandomGuesses.Size = new System.Drawing.Size(254, 19);
            this.allowCustomRandomGuesses.TabIndex = 12;
            this.allowCustomRandomGuesses.Text = "Enable random guessing in specific regions";
            this.toolTip1.SetToolTip(this.allowCustomRandomGuesses, "When enabled, players can use random guessing command and the map to make random " +
        "guesses in their choice of regions with customized probabilities for each choice" +
        "");
            this.allowCustomRandomGuesses.UseVisualStyleBackColor = true;
            // 
            // chkCheckStreamer
            // 
            this.chkCheckStreamer.AutoSize = true;
            this.chkCheckStreamer.Location = new System.Drawing.Point(7, 125);
            this.chkCheckStreamer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkCheckStreamer.Name = "chkCheckStreamer";
            this.chkCheckStreamer.Size = new System.Drawing.Size(251, 19);
            this.chkCheckStreamer.TabIndex = 11;
            this.chkCheckStreamer.Text = "Check streamers guesses for special events";
            this.toolTip1.SetToolTip(this.chkCheckStreamer, "Should streamer guesses be checked for special score/special distance guesses as " +
        "well?");
            this.chkCheckStreamer.UseVisualStyleBackColor = true;
            // 
            // chkAllowSameLocationGuess
            // 
            this.chkAllowSameLocationGuess.AutoSize = true;
            this.chkAllowSameLocationGuess.Location = new System.Drawing.Point(7, 75);
            this.chkAllowSameLocationGuess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkAllowSameLocationGuess.Name = "chkAllowSameLocationGuess";
            this.chkAllowSameLocationGuess.Size = new System.Drawing.Size(216, 19);
            this.chkAllowSameLocationGuess.TabIndex = 9;
            this.chkAllowSameLocationGuess.Text = "Allow guessing in the same location";
            this.toolTip1.SetToolTip(this.chkAllowSameLocationGuess, "When enabled country names are displayed in english, otherwise they are displayed" +
        " in the Windows display language");
            this.chkAllowSameLocationGuess.UseVisualStyleBackColor = true;
            // 
            // chkUseEnglishCountryNames
            // 
            this.chkUseEnglishCountryNames.AutoSize = true;
            this.chkUseEnglishCountryNames.Location = new System.Drawing.Point(7, 48);
            this.chkUseEnglishCountryNames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkUseEnglishCountryNames.Name = "chkUseEnglishCountryNames";
            this.chkUseEnglishCountryNames.Size = new System.Drawing.Size(168, 19);
            this.chkUseEnglishCountryNames.TabIndex = 8;
            this.chkUseEnglishCountryNames.Text = "Use english country names";
            this.toolTip1.SetToolTip(this.chkUseEnglishCountryNames, "When enabled country names are displayed in english, otherwise they are displayed" +
        " in the Windows display language");
            this.chkUseEnglishCountryNames.UseVisualStyleBackColor = true;
            // 
            // chkResetStreakOnSkippedRound
            // 
            this.chkResetStreakOnSkippedRound.AutoSize = true;
            this.chkResetStreakOnSkippedRound.Location = new System.Drawing.Point(7, 22);
            this.chkResetStreakOnSkippedRound.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkResetStreakOnSkippedRound.Name = "chkResetStreakOnSkippedRound";
            this.chkResetStreakOnSkippedRound.Size = new System.Drawing.Size(228, 19);
            this.chkResetStreakOnSkippedRound.TabIndex = 7;
            this.chkResetStreakOnSkippedRound.Text = "Reset country streak on skipped round";
            this.toolTip1.SetToolTip(this.chkResetStreakOnSkippedRound, "When enabled the viewers country streak will reset when they skip a round");
            this.chkResetStreakOnSkippedRound.UseVisualStyleBackColor = true;
            // 
            // tabTwitch
            // 
            this.tabTwitch.Controls.Add(this.groupBoxChatMessages);
            this.tabTwitch.Location = new System.Drawing.Point(4, 24);
            this.tabTwitch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabTwitch.Name = "tabTwitch";
            this.tabTwitch.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabTwitch.Size = new System.Drawing.Size(1047, 622);
            this.tabTwitch.TabIndex = 1;
            this.tabTwitch.Text = "Twitch";
            this.tabTwitch.UseVisualStyleBackColor = true;
            // 
            // groupBoxChatMessages
            // 
            this.groupBoxChatMessages.Controls.Add(this.checkBoxEnableChatMsgs);
            this.groupBoxChatMessages.Controls.Add(this.grpTwitchChatConnection);
            this.groupBoxChatMessages.Location = new System.Drawing.Point(7, 6);
            this.groupBoxChatMessages.Name = "groupBoxChatMessages";
            this.groupBoxChatMessages.Size = new System.Drawing.Size(860, 591);
            this.groupBoxChatMessages.TabIndex = 11;
            this.groupBoxChatMessages.TabStop = false;
            this.groupBoxChatMessages.Text = "Twitch settings";
            // 
            // checkBoxEnableChatMsgs
            // 
            this.checkBoxEnableChatMsgs.AutoSize = true;
            this.checkBoxEnableChatMsgs.BackColor = System.Drawing.SystemColors.Control;
            this.checkBoxEnableChatMsgs.Location = new System.Drawing.Point(9, 20);
            this.checkBoxEnableChatMsgs.Name = "checkBoxEnableChatMsgs";
            this.checkBoxEnableChatMsgs.Size = new System.Drawing.Size(122, 19);
            this.checkBoxEnableChatMsgs.TabIndex = 11;
            this.checkBoxEnableChatMsgs.Text = "Connect to Twitch";
            this.checkBoxEnableChatMsgs.UseVisualStyleBackColor = false;
            this.checkBoxEnableChatMsgs.CheckedChanged += new System.EventHandler(this.checkBoxEnableChatMsgs_CheckedChanged);
            // 
            // grpTwitchChatConnection
            // 
            this.grpTwitchChatConnection.Controls.Add(this.btnForgetTwitch);
            this.grpTwitchChatConnection.Controls.Add(this.label27);
            this.grpTwitchChatConnection.Controls.Add(this.btnAuthorizeManually);
            this.grpTwitchChatConnection.Controls.Add(this.btnAuthorizeAutomatically);
            this.grpTwitchChatConnection.Controls.Add(this.reconnectTwitchBotButton);
            this.grpTwitchChatConnection.Controls.Add(this.txtGeneralOauthToken);
            this.grpTwitchChatConnection.Controls.Add(this.txtGeneralChannelName);
            this.grpTwitchChatConnection.Controls.Add(this.label3);
            this.grpTwitchChatConnection.Controls.Add(this.label2);
            this.grpTwitchChatConnection.Location = new System.Drawing.Point(7, 45);
            this.grpTwitchChatConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpTwitchChatConnection.Name = "grpTwitchChatConnection";
            this.grpTwitchChatConnection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpTwitchChatConnection.Size = new System.Drawing.Size(478, 247);
            this.grpTwitchChatConnection.TabIndex = 11;
            this.grpTwitchChatConnection.TabStop = false;
            // 
            // btnForgetTwitch
            // 
            this.btnForgetTwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnForgetTwitch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnForgetTwitch.ForeColor = System.Drawing.Color.Red;
            this.btnForgetTwitch.Location = new System.Drawing.Point(8, 207);
            this.btnForgetTwitch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnForgetTwitch.Name = "btnForgetTwitch";
            this.btnForgetTwitch.Size = new System.Drawing.Size(102, 27);
            this.btnForgetTwitch.TabIndex = 14;
            this.btnForgetTwitch.Text = "Forget account";
            this.btnForgetTwitch.UseVisualStyleBackColor = true;
            this.btnForgetTwitch.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(8, 112);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(463, 45);
            this.label27.TabIndex = 13;
            this.label27.Text = resources.GetString("label27.Text");
            this.label27.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnAuthorizeManually
            // 
            this.btnAuthorizeManually.Location = new System.Drawing.Point(266, 58);
            this.btnAuthorizeManually.Name = "btnAuthorizeManually";
            this.btnAuthorizeManually.Size = new System.Drawing.Size(159, 43);
            this.btnAuthorizeManually.TabIndex = 12;
            this.btnAuthorizeManually.Text = "Authorize manually";
            this.btnAuthorizeManually.UseVisualStyleBackColor = true;
            this.btnAuthorizeManually.Click += new System.EventHandler(this.btnAuthorizeManually_Click);
            // 
            // btnAuthorizeAutomatically
            // 
            this.btnAuthorizeAutomatically.Location = new System.Drawing.Point(53, 58);
            this.btnAuthorizeAutomatically.Name = "btnAuthorizeAutomatically";
            this.btnAuthorizeAutomatically.Size = new System.Drawing.Size(159, 43);
            this.btnAuthorizeAutomatically.TabIndex = 11;
            this.btnAuthorizeAutomatically.Text = "Authorize automatically\r\nwith logged in account";
            this.btnAuthorizeAutomatically.UseVisualStyleBackColor = true;
            this.btnAuthorizeAutomatically.Click += new System.EventHandler(this.btnAuthorizeAutomatically_Click);
            // 
            // reconnectTwitchBotButton
            // 
            this.reconnectTwitchBotButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reconnectTwitchBotButton.Location = new System.Drawing.Point(53, 174);
            this.reconnectTwitchBotButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.reconnectTwitchBotButton.Name = "reconnectTwitchBotButton";
            this.reconnectTwitchBotButton.Size = new System.Drawing.Size(372, 27);
            this.reconnectTwitchBotButton.TabIndex = 10;
            this.reconnectTwitchBotButton.Text = "Test Twitch-bot connection";
            this.reconnectTwitchBotButton.UseVisualStyleBackColor = true;
            this.reconnectTwitchBotButton.Click += new System.EventHandler(this.reconnectTwitchBotButton_Click);
            // 
            // txtGeneralOauthToken
            // 
            this.txtGeneralOauthToken.Location = new System.Drawing.Point(144, 81);
            this.txtGeneralOauthToken.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtGeneralOauthToken.Name = "txtGeneralOauthToken";
            this.txtGeneralOauthToken.PasswordChar = '*';
            this.txtGeneralOauthToken.Size = new System.Drawing.Size(243, 23);
            this.txtGeneralOauthToken.TabIndex = 9;
            this.txtGeneralOauthToken.Visible = false;
            // 
            // txtGeneralChannelName
            // 
            this.txtGeneralChannelName.Location = new System.Drawing.Point(182, 21);
            this.txtGeneralChannelName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtGeneralChannelName.Name = "txtGeneralChannelName";
            this.txtGeneralChannelName.Size = new System.Drawing.Size(243, 23);
            this.txtGeneralChannelName.TabIndex = 3;
            this.txtGeneralChannelName.Leave += new System.EventHandler(this.txtGeneralChannelName_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 84);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "OAuth-token";
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Channel name";
            // 
            // tabPageConnections
            // 
            this.tabPageConnections.Controls.Add(this.groupBoxOtherChatGateways);
            this.tabPageConnections.Controls.Add(this.grpObsConnection);
            this.tabPageConnections.Controls.Add(this.grpStreamerBotConnection);
            this.tabPageConnections.Location = new System.Drawing.Point(4, 24);
            this.tabPageConnections.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageConnections.Name = "tabPageConnections";
            this.tabPageConnections.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageConnections.Size = new System.Drawing.Size(1047, 622);
            this.tabPageConnections.TabIndex = 4;
            this.tabPageConnections.Text = "Connections";
            this.tabPageConnections.UseVisualStyleBackColor = true;
            // 
            // groupBoxOtherChatGateways
            // 
            this.groupBoxOtherChatGateways.Controls.Add(this.checkBoxSendChatMsgsViaStreamerBot);
            this.groupBoxOtherChatGateways.Location = new System.Drawing.Point(8, 208);
            this.groupBoxOtherChatGateways.Name = "groupBoxOtherChatGateways";
            this.groupBoxOtherChatGateways.Size = new System.Drawing.Size(478, 271);
            this.groupBoxOtherChatGateways.TabIndex = 13;
            this.groupBoxOtherChatGateways.TabStop = false;
            this.groupBoxOtherChatGateways.Text = "Other chat gateways";
            // 
            // checkBoxSendChatMsgsViaStreamerBot
            // 
            this.checkBoxSendChatMsgsViaStreamerBot.AutoSize = true;
            this.checkBoxSendChatMsgsViaStreamerBot.Location = new System.Drawing.Point(7, 24);
            this.checkBoxSendChatMsgsViaStreamerBot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxSendChatMsgsViaStreamerBot.Name = "checkBoxSendChatMsgsViaStreamerBot";
            this.checkBoxSendChatMsgsViaStreamerBot.Size = new System.Drawing.Size(313, 19);
            this.checkBoxSendChatMsgsViaStreamerBot.TabIndex = 16;
            this.checkBoxSendChatMsgsViaStreamerBot.Text = "Use this Streamer.Bot action to send messages to chat:";
            this.toolTip1.SetToolTip(this.checkBoxSendChatMsgsViaStreamerBot, "Choose an action in Streamer.bot to execute when a game round starts.");
            this.checkBoxSendChatMsgsViaStreamerBot.UseVisualStyleBackColor = true;
            // 
            // grpObsConnection
            // 
            this.grpObsConnection.Controls.Add(this.chkShowPassword);
            this.grpObsConnection.Controls.Add(this.label14);
            this.grpObsConnection.Controls.Add(this.txtObsPassword);
            this.grpObsConnection.Controls.Add(this.chkObsConnectAtStartup);
            this.grpObsConnection.Controls.Add(this.btnConnectObs);
            this.grpObsConnection.Controls.Add(this.label12);
            this.grpObsConnection.Controls.Add(this.txtObsPort);
            this.grpObsConnection.Controls.Add(this.label13);
            this.grpObsConnection.Controls.Add(this.txtObsIp);
            this.grpObsConnection.Location = new System.Drawing.Point(494, 6);
            this.grpObsConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpObsConnection.Name = "grpObsConnection";
            this.grpObsConnection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpObsConnection.Size = new System.Drawing.Size(478, 196);
            this.grpObsConnection.TabIndex = 6;
            this.grpObsConnection.TabStop = false;
            this.grpObsConnection.Text = "OBS";
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.Location = new System.Drawing.Point(365, 110);
            this.chkShowPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(65, 21);
            this.chkShowPassword.TabIndex = 5;
            this.chkShowPassword.Text = "Show";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 112);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 15);
            this.label14.TabIndex = 7;
            this.label14.Text = "Password:";
            // 
            // txtObsPassword
            // 
            this.txtObsPassword.Location = new System.Drawing.Point(144, 108);
            this.txtObsPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtObsPassword.Name = "txtObsPassword";
            this.txtObsPassword.PasswordChar = '*';
            this.txtObsPassword.Size = new System.Drawing.Size(207, 23);
            this.txtObsPassword.TabIndex = 4;
            this.txtObsPassword.TextChanged += new System.EventHandler(this.txtObs_TextChanged);
            // 
            // chkObsConnectAtStartup
            // 
            this.chkObsConnectAtStartup.AutoSize = true;
            this.chkObsConnectAtStartup.Location = new System.Drawing.Point(8, 23);
            this.chkObsConnectAtStartup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkObsConnectAtStartup.Name = "chkObsConnectAtStartup";
            this.chkObsConnectAtStartup.Size = new System.Drawing.Size(124, 19);
            this.chkObsConnectAtStartup.TabIndex = 1;
            this.chkObsConnectAtStartup.Text = "Connect at startup";
            this.chkObsConnectAtStartup.UseVisualStyleBackColor = true;
            this.chkObsConnectAtStartup.CheckedChanged += new System.EventHandler(this.chkObsConnectAtStartup_CheckedChanged);
            // 
            // btnConnectObs
            // 
            this.btnConnectObs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectObs.Location = new System.Drawing.Point(10, 147);
            this.btnConnectObs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConnectObs.Name = "btnConnectObs";
            this.btnConnectObs.Size = new System.Drawing.Size(152, 27);
            this.btnConnectObs.TabIndex = 6;
            this.btnConnectObs.Text = "Connect";
            this.btnConnectObs.UseVisualStyleBackColor = true;
            this.btnConnectObs.Click += new System.EventHandler(this.ctnConnectObs_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 82);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 15);
            this.label12.TabIndex = 4;
            this.label12.Text = "Port:";
            // 
            // txtObsPort
            // 
            this.txtObsPort.Location = new System.Drawing.Point(144, 78);
            this.txtObsPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtObsPort.Name = "txtObsPort";
            this.txtObsPort.Size = new System.Drawing.Size(243, 23);
            this.txtObsPort.TabIndex = 3;
            this.txtObsPort.TextChanged += new System.EventHandler(this.txtObs_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 52);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(20, 15);
            this.label13.TabIndex = 1;
            this.label13.Text = "IP:";
            // 
            // txtObsIp
            // 
            this.txtObsIp.Location = new System.Drawing.Point(144, 48);
            this.txtObsIp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtObsIp.Name = "txtObsIp";
            this.txtObsIp.Size = new System.Drawing.Size(243, 23);
            this.txtObsIp.TabIndex = 2;
            this.txtObsIp.TextChanged += new System.EventHandler(this.txtObs_TextChanged);
            // 
            // grpStreamerBotConnection
            // 
            this.grpStreamerBotConnection.Controls.Add(this.chkStreamerBotConnectAtStartup);
            this.grpStreamerBotConnection.Controls.Add(this.label9);
            this.grpStreamerBotConnection.Controls.Add(this.txtStreamerBotPort);
            this.grpStreamerBotConnection.Controls.Add(this.label8);
            this.grpStreamerBotConnection.Controls.Add(this.txtStreamerBotIP);
            this.grpStreamerBotConnection.Controls.Add(this.buttonConnectStreamerBot);
            this.grpStreamerBotConnection.Location = new System.Drawing.Point(8, 6);
            this.grpStreamerBotConnection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpStreamerBotConnection.Name = "grpStreamerBotConnection";
            this.grpStreamerBotConnection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpStreamerBotConnection.Size = new System.Drawing.Size(478, 196);
            this.grpStreamerBotConnection.TabIndex = 3;
            this.grpStreamerBotConnection.TabStop = false;
            this.grpStreamerBotConnection.Text = "Streamer.Bot";
            // 
            // chkStreamerBotConnectAtStartup
            // 
            this.chkStreamerBotConnectAtStartup.AutoSize = true;
            this.chkStreamerBotConnectAtStartup.Location = new System.Drawing.Point(8, 23);
            this.chkStreamerBotConnectAtStartup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkStreamerBotConnectAtStartup.Name = "chkStreamerBotConnectAtStartup";
            this.chkStreamerBotConnectAtStartup.Size = new System.Drawing.Size(124, 19);
            this.chkStreamerBotConnectAtStartup.TabIndex = 5;
            this.chkStreamerBotConnectAtStartup.Text = "Connect at startup";
            this.chkStreamerBotConnectAtStartup.UseVisualStyleBackColor = true;
            this.chkStreamerBotConnectAtStartup.CheckedChanged += new System.EventHandler(this.chkStreamerBotConnectAtStartup_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 82);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 15);
            this.label9.TabIndex = 4;
            this.label9.Text = "Port:";
            // 
            // txtStreamerBotPort
            // 
            this.txtStreamerBotPort.Location = new System.Drawing.Point(144, 78);
            this.txtStreamerBotPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtStreamerBotPort.Name = "txtStreamerBotPort";
            this.txtStreamerBotPort.Size = new System.Drawing.Size(243, 23);
            this.txtStreamerBotPort.TabIndex = 3;
            this.txtStreamerBotPort.TextChanged += new System.EventHandler(this.txtStreamerBot_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 52);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = "IP:";
            // 
            // txtStreamerBotIP
            // 
            this.txtStreamerBotIP.Location = new System.Drawing.Point(144, 48);
            this.txtStreamerBotIP.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtStreamerBotIP.Name = "txtStreamerBotIP";
            this.txtStreamerBotIP.Size = new System.Drawing.Size(243, 23);
            this.txtStreamerBotIP.TabIndex = 0;
            this.txtStreamerBotIP.TextChanged += new System.EventHandler(this.txtStreamerBot_TextChanged);
            // 
            // buttonConnectStreamerBot
            // 
            this.buttonConnectStreamerBot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConnectStreamerBot.Location = new System.Drawing.Point(10, 147);
            this.buttonConnectStreamerBot.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonConnectStreamerBot.Name = "buttonConnectStreamerBot";
            this.buttonConnectStreamerBot.Size = new System.Drawing.Size(152, 27);
            this.buttonConnectStreamerBot.TabIndex = 2;
            this.buttonConnectStreamerBot.Text = "Connect";
            this.buttonConnectStreamerBot.UseVisualStyleBackColor = true;
            this.buttonConnectStreamerBot.Click += new System.EventHandler(this.buttonConnectStreamerBot_Click);
            // 
            // tabChatMessages
            // 
            this.tabChatMessages.Controls.Add(this.groupBox3);
            this.tabChatMessages.Location = new System.Drawing.Point(4, 24);
            this.tabChatMessages.Name = "tabChatMessages";
            this.tabChatMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tabChatMessages.Size = new System.Drawing.Size(1047, 622);
            this.tabChatMessages.TabIndex = 8;
            this.tabChatMessages.Text = "ChatMessages";
            this.tabChatMessages.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label43);
            this.groupBox3.Controls.Add(this.flowLayoutPanel1);
            this.groupBox3.Controls.Add(this.propertyGrid1);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(954, 411);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Chat messages";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(6, 19);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(359, 60);
            this.label43.TabIndex = 13;
            this.label43.Text = "Here you can choose which chat messages to send.\r\n\r\nYou can also modify the chat " +
    "messages.\r\nPlease make sure to keep the variables names within the messages.\r\n";
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
            this.flowLayoutPanel1.Controls.Add(this.chkColorMessage);
            this.flowLayoutPanel1.Controls.Add(this.chkFlagMessages);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 105);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(264, 287);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // chkMsgsJoinChannel
            // 
            this.chkMsgsJoinChannel.AutoSize = true;
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
            this.chkMsgsGameEnded.Location = new System.Drawing.Point(4, 178);
            this.chkMsgsGameEnded.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkMsgsGameEnded.Name = "chkMsgsGameEnded";
            this.chkMsgsGameEnded.Size = new System.Drawing.Size(181, 19);
            this.chkMsgsGameEnded.TabIndex = 4;
            this.chkMsgsGameEnded.Text = "Send \"Game ended\" message";
            this.chkMsgsGameEnded.UseVisualStyleBackColor = true;
            // 
            // chkColorMessage
            // 
            this.chkColorMessage.AutoSize = true;
            this.chkColorMessage.Location = new System.Drawing.Point(4, 203);
            this.chkColorMessage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkColorMessage.Name = "chkColorMessage";
            this.chkColorMessage.Size = new System.Drawing.Size(194, 19);
            this.chkColorMessage.TabIndex = 10;
            this.chkColorMessage.Text = "Send \"Color selected\" messages";
            this.chkColorMessage.UseVisualStyleBackColor = true;
            // 
            // chkFlagMessages
            // 
            this.chkFlagMessages.AutoSize = true;
            this.chkFlagMessages.Location = new System.Drawing.Point(4, 228);
            this.chkFlagMessages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkFlagMessages.Name = "chkFlagMessages";
            this.chkFlagMessages.Size = new System.Drawing.Size(187, 19);
            this.chkFlagMessages.TabIndex = 11;
            this.chkFlagMessages.Text = "Send \"Flag selected\" messages";
            this.chkFlagMessages.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CommandsVisibleIfAvailable = false;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.propertyGrid1.Location = new System.Drawing.Point(289, 105);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(519, 284);
            this.propertyGrid1.TabIndex = 11;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // tabPageEvents
            // 
            this.tabPageEvents.Controls.Add(this.groupBoxGameEnd);
            this.tabPageEvents.Controls.Add(this.groupBoxRoundEnd);
            this.tabPageEvents.Controls.Add(this.groupBoxRoundTimer);
            this.tabPageEvents.Controls.Add(this.groupBoxEventDistance);
            this.tabPageEvents.Controls.Add(this.groupBoxEventSpecial);
            this.tabPageEvents.Location = new System.Drawing.Point(4, 24);
            this.tabPageEvents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageEvents.Name = "tabPageEvents";
            this.tabPageEvents.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageEvents.Size = new System.Drawing.Size(1047, 622);
            this.tabPageEvents.TabIndex = 2;
            this.tabPageEvents.Text = "Events";
            this.tabPageEvents.UseVisualStyleBackColor = true;
            // 
            // groupBoxGameEnd
            // 
            this.groupBoxGameEnd.Controls.Add(this.label25);
            this.groupBoxGameEnd.Controls.Add(this.label26);
            this.groupBoxGameEnd.Controls.Add(this.comboOBSGameEndSource);
            this.groupBoxGameEnd.Controls.Add(this.comboOBSGameEndAction);
            this.groupBoxGameEnd.Controls.Add(this.comboOBSGameEndScene);
            this.groupBoxGameEnd.Controls.Add(this.chkOBSGameEndExecute);
            this.groupBoxGameEnd.Controls.Add(this.chkBotGameEndExecute);
            this.groupBoxGameEnd.Location = new System.Drawing.Point(7, 261);
            this.groupBoxGameEnd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxGameEnd.Name = "groupBoxGameEnd";
            this.groupBoxGameEnd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxGameEnd.Size = new System.Drawing.Size(506, 119);
            this.groupBoxGameEnd.TabIndex = 25;
            this.groupBoxGameEnd.TabStop = false;
            this.groupBoxGameEnd.Text = "Game End";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(161, 85);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(43, 15);
            this.label25.TabIndex = 23;
            this.label25.Text = "Source";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(161, 54);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(38, 15);
            this.label26.TabIndex = 22;
            this.label26.Text = "Scene";
            // 
            // comboOBSGameEndSource
            // 
            this.comboOBSGameEndSource.Enabled = false;
            this.comboOBSGameEndSource.FormattingEnabled = true;
            this.comboOBSGameEndSource.Location = new System.Drawing.Point(212, 82);
            this.comboOBSGameEndSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboOBSGameEndSource.Name = "comboOBSGameEndSource";
            this.comboOBSGameEndSource.Size = new System.Drawing.Size(192, 23);
            this.comboOBSGameEndSource.TabIndex = 21;
            // 
            // comboOBSGameEndAction
            // 
            this.comboOBSGameEndAction.Enabled = false;
            this.comboOBSGameEndAction.FormattingEnabled = true;
            this.comboOBSGameEndAction.Items.AddRange(new object[] {
            "Show",
            "Hide",
            "Toggle"});
            this.comboOBSGameEndAction.Location = new System.Drawing.Point(412, 82);
            this.comboOBSGameEndAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboOBSGameEndAction.Name = "comboOBSGameEndAction";
            this.comboOBSGameEndAction.Size = new System.Drawing.Size(80, 23);
            this.comboOBSGameEndAction.TabIndex = 20;
            // 
            // comboOBSGameEndScene
            // 
            this.comboOBSGameEndScene.Enabled = false;
            this.comboOBSGameEndScene.FormattingEnabled = true;
            this.comboOBSGameEndScene.Location = new System.Drawing.Point(212, 51);
            this.comboOBSGameEndScene.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboOBSGameEndScene.Name = "comboOBSGameEndScene";
            this.comboOBSGameEndScene.Size = new System.Drawing.Size(279, 23);
            this.comboOBSGameEndScene.TabIndex = 19;
            this.comboOBSGameEndScene.SelectedIndexChanged += new System.EventHandler(this.comboOBSGameEndScene_SelectedIndexChanged);
            // 
            // chkOBSGameEndExecute
            // 
            this.chkOBSGameEndExecute.AutoSize = true;
            this.chkOBSGameEndExecute.Location = new System.Drawing.Point(7, 53);
            this.chkOBSGameEndExecute.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkOBSGameEndExecute.Name = "chkOBSGameEndExecute";
            this.chkOBSGameEndExecute.Size = new System.Drawing.Size(127, 19);
            this.chkOBSGameEndExecute.TabIndex = 18;
            this.chkOBSGameEndExecute.Text = "Modify OBS source";
            this.toolTip1.SetToolTip(this.chkOBSGameEndExecute, "You can show, hide or toggle any OBS source.");
            this.chkOBSGameEndExecute.UseVisualStyleBackColor = true;
            // 
            // chkBotGameEndExecute
            // 
            this.chkBotGameEndExecute.AutoSize = true;
            this.chkBotGameEndExecute.Location = new System.Drawing.Point(7, 22);
            this.chkBotGameEndExecute.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkBotGameEndExecute.Name = "chkBotGameEndExecute";
            this.chkBotGameEndExecute.Size = new System.Drawing.Size(174, 19);
            this.chkBotGameEndExecute.TabIndex = 14;
            this.chkBotGameEndExecute.Text = "Execute Streamer.Bot action";
            this.toolTip1.SetToolTip(this.chkBotGameEndExecute, "Choose an action in Streamer.bot to execute when a game round starts.");
            this.chkBotGameEndExecute.UseVisualStyleBackColor = true;
            // 
            // groupBoxRoundEnd
            // 
            this.groupBoxRoundEnd.Controls.Add(this.label19);
            this.groupBoxRoundEnd.Controls.Add(this.label24);
            this.groupBoxRoundEnd.Controls.Add(this.comboOBSRoundEndSource);
            this.groupBoxRoundEnd.Controls.Add(this.comboOBSRoundEndAction);
            this.groupBoxRoundEnd.Controls.Add(this.comboOBSRoundEndScene);
            this.groupBoxRoundEnd.Controls.Add(this.chkOBSRoundEndExecute);
            this.groupBoxRoundEnd.Controls.Add(this.chkBotRoundEndExecute);
            this.groupBoxRoundEnd.Location = new System.Drawing.Point(7, 133);
            this.groupBoxRoundEnd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxRoundEnd.Name = "groupBoxRoundEnd";
            this.groupBoxRoundEnd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxRoundEnd.Size = new System.Drawing.Size(506, 119);
            this.groupBoxRoundEnd.TabIndex = 24;
            this.groupBoxRoundEnd.TabStop = false;
            this.groupBoxRoundEnd.Text = "Round End";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(161, 85);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(43, 15);
            this.label19.TabIndex = 23;
            this.label19.Text = "Source";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(161, 54);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(38, 15);
            this.label24.TabIndex = 22;
            this.label24.Text = "Scene";
            // 
            // comboOBSRoundEndSource
            // 
            this.comboOBSRoundEndSource.Enabled = false;
            this.comboOBSRoundEndSource.FormattingEnabled = true;
            this.comboOBSRoundEndSource.Location = new System.Drawing.Point(212, 82);
            this.comboOBSRoundEndSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboOBSRoundEndSource.Name = "comboOBSRoundEndSource";
            this.comboOBSRoundEndSource.Size = new System.Drawing.Size(192, 23);
            this.comboOBSRoundEndSource.TabIndex = 21;
            // 
            // comboOBSRoundEndAction
            // 
            this.comboOBSRoundEndAction.Enabled = false;
            this.comboOBSRoundEndAction.FormattingEnabled = true;
            this.comboOBSRoundEndAction.Items.AddRange(new object[] {
            "Show",
            "Hide",
            "Toggle"});
            this.comboOBSRoundEndAction.Location = new System.Drawing.Point(412, 82);
            this.comboOBSRoundEndAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboOBSRoundEndAction.Name = "comboOBSRoundEndAction";
            this.comboOBSRoundEndAction.Size = new System.Drawing.Size(80, 23);
            this.comboOBSRoundEndAction.TabIndex = 20;
            // 
            // comboOBSRoundEndScene
            // 
            this.comboOBSRoundEndScene.Enabled = false;
            this.comboOBSRoundEndScene.FormattingEnabled = true;
            this.comboOBSRoundEndScene.Location = new System.Drawing.Point(212, 51);
            this.comboOBSRoundEndScene.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboOBSRoundEndScene.Name = "comboOBSRoundEndScene";
            this.comboOBSRoundEndScene.Size = new System.Drawing.Size(279, 23);
            this.comboOBSRoundEndScene.TabIndex = 19;
            this.comboOBSRoundEndScene.SelectedIndexChanged += new System.EventHandler(this.comboOBSRoundEndScene_SelectedIndexChanged);
            // 
            // chkOBSRoundEndExecute
            // 
            this.chkOBSRoundEndExecute.AutoSize = true;
            this.chkOBSRoundEndExecute.Location = new System.Drawing.Point(7, 53);
            this.chkOBSRoundEndExecute.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkOBSRoundEndExecute.Name = "chkOBSRoundEndExecute";
            this.chkOBSRoundEndExecute.Size = new System.Drawing.Size(127, 19);
            this.chkOBSRoundEndExecute.TabIndex = 18;
            this.chkOBSRoundEndExecute.Text = "Modify OBS source";
            this.toolTip1.SetToolTip(this.chkOBSRoundEndExecute, "You can show, hide or toggle any OBS source.");
            this.chkOBSRoundEndExecute.UseVisualStyleBackColor = true;
            // 
            // chkBotRoundEndExecute
            // 
            this.chkBotRoundEndExecute.AutoSize = true;
            this.chkBotRoundEndExecute.Location = new System.Drawing.Point(7, 22);
            this.chkBotRoundEndExecute.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkBotRoundEndExecute.Name = "chkBotRoundEndExecute";
            this.chkBotRoundEndExecute.Size = new System.Drawing.Size(174, 19);
            this.chkBotRoundEndExecute.TabIndex = 14;
            this.chkBotRoundEndExecute.Text = "Execute Streamer.Bot action";
            this.toolTip1.SetToolTip(this.chkBotRoundEndExecute, "Choose an action in Streamer.bot to execute when a game round starts.");
            this.chkBotRoundEndExecute.UseVisualStyleBackColor = true;
            // 
            // groupBoxRoundTimer
            // 
            this.groupBoxRoundTimer.Controls.Add(this.label18);
            this.groupBoxRoundTimer.Controls.Add(this.label17);
            this.groupBoxRoundTimer.Controls.Add(this.comboRoundTimerObsSource);
            this.groupBoxRoundTimer.Controls.Add(this.comboRoundTimerObsAction);
            this.groupBoxRoundTimer.Controls.Add(this.comboRoundTimerObsScene);
            this.groupBoxRoundTimer.Controls.Add(this.chkRoundTimerOBS);
            this.groupBoxRoundTimer.Controls.Add(this.checkBoxRoundTimerExecuteStreamerBotAction);
            this.groupBoxRoundTimer.Location = new System.Drawing.Point(7, 7);
            this.groupBoxRoundTimer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxRoundTimer.Name = "groupBoxRoundTimer";
            this.groupBoxRoundTimer.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxRoundTimer.Size = new System.Drawing.Size(506, 119);
            this.groupBoxRoundTimer.TabIndex = 10;
            this.groupBoxRoundTimer.TabStop = false;
            this.groupBoxRoundTimer.Text = "Round Start";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(161, 85);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(43, 15);
            this.label18.TabIndex = 23;
            this.label18.Text = "Source";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(161, 54);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(38, 15);
            this.label17.TabIndex = 22;
            this.label17.Text = "Scene";
            // 
            // comboRoundTimerObsSource
            // 
            this.comboRoundTimerObsSource.Enabled = false;
            this.comboRoundTimerObsSource.FormattingEnabled = true;
            this.comboRoundTimerObsSource.Location = new System.Drawing.Point(212, 82);
            this.comboRoundTimerObsSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboRoundTimerObsSource.Name = "comboRoundTimerObsSource";
            this.comboRoundTimerObsSource.Size = new System.Drawing.Size(192, 23);
            this.comboRoundTimerObsSource.TabIndex = 21;
            // 
            // comboRoundTimerObsAction
            // 
            this.comboRoundTimerObsAction.Enabled = false;
            this.comboRoundTimerObsAction.FormattingEnabled = true;
            this.comboRoundTimerObsAction.Items.AddRange(new object[] {
            "Show",
            "Hide",
            "Toggle"});
            this.comboRoundTimerObsAction.Location = new System.Drawing.Point(412, 82);
            this.comboRoundTimerObsAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboRoundTimerObsAction.Name = "comboRoundTimerObsAction";
            this.comboRoundTimerObsAction.Size = new System.Drawing.Size(80, 23);
            this.comboRoundTimerObsAction.TabIndex = 20;
            // 
            // comboRoundTimerObsScene
            // 
            this.comboRoundTimerObsScene.Enabled = false;
            this.comboRoundTimerObsScene.FormattingEnabled = true;
            this.comboRoundTimerObsScene.Location = new System.Drawing.Point(212, 51);
            this.comboRoundTimerObsScene.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboRoundTimerObsScene.Name = "comboRoundTimerObsScene";
            this.comboRoundTimerObsScene.Size = new System.Drawing.Size(279, 23);
            this.comboRoundTimerObsScene.TabIndex = 19;
            this.comboRoundTimerObsScene.SelectedIndexChanged += new System.EventHandler(this.comboRoundTimerObsSource_SelectedIndexChanged);
            // 
            // chkRoundTimerOBS
            // 
            this.chkRoundTimerOBS.AutoSize = true;
            this.chkRoundTimerOBS.Location = new System.Drawing.Point(7, 53);
            this.chkRoundTimerOBS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkRoundTimerOBS.Name = "chkRoundTimerOBS";
            this.chkRoundTimerOBS.Size = new System.Drawing.Size(127, 19);
            this.chkRoundTimerOBS.TabIndex = 18;
            this.chkRoundTimerOBS.Text = "Modify OBS source";
            this.toolTip1.SetToolTip(this.chkRoundTimerOBS, "You can show, hide or toggle any OBS source.");
            this.chkRoundTimerOBS.UseVisualStyleBackColor = true;
            // 
            // checkBoxRoundTimerExecuteStreamerBotAction
            // 
            this.checkBoxRoundTimerExecuteStreamerBotAction.AutoSize = true;
            this.checkBoxRoundTimerExecuteStreamerBotAction.Location = new System.Drawing.Point(7, 22);
            this.checkBoxRoundTimerExecuteStreamerBotAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxRoundTimerExecuteStreamerBotAction.Name = "checkBoxRoundTimerExecuteStreamerBotAction";
            this.checkBoxRoundTimerExecuteStreamerBotAction.Size = new System.Drawing.Size(174, 19);
            this.checkBoxRoundTimerExecuteStreamerBotAction.TabIndex = 14;
            this.checkBoxRoundTimerExecuteStreamerBotAction.Text = "Execute Streamer.Bot action";
            this.toolTip1.SetToolTip(this.checkBoxRoundTimerExecuteStreamerBotAction, "Choose an action in Streamer.bot to execute when a game round starts.");
            this.checkBoxRoundTimerExecuteStreamerBotAction.UseVisualStyleBackColor = true;
            // 
            // groupBoxEventDistance
            // 
            this.groupBoxEventDistance.Controls.Add(this.chkSpecialDistanceRange);
            this.groupBoxEventDistance.Controls.Add(this.label22);
            this.groupBoxEventDistance.Controls.Add(this.txtSpecialDistanceTo);
            this.groupBoxEventDistance.Controls.Add(this.label23);
            this.groupBoxEventDistance.Controls.Add(this.comboSpecialDistanceObsScene);
            this.groupBoxEventDistance.Controls.Add(this.comboSpecialDistanceObsAction);
            this.groupBoxEventDistance.Controls.Add(this.comboSpecialDistanceObsSource);
            this.groupBoxEventDistance.Controls.Add(this.checkBoxSpecialDistanceAction);
            this.groupBoxEventDistance.Controls.Add(this.label7);
            this.groupBoxEventDistance.Controls.Add(this.chkSpecialDistanceObs);
            this.groupBoxEventDistance.Controls.Add(this.txtSpecialDistanceFrom);
            this.groupBoxEventDistance.Controls.Add(this.checkBoxSpecialDistanceLabel);
            this.groupBoxEventDistance.Location = new System.Drawing.Point(520, 182);
            this.groupBoxEventDistance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventDistance.Name = "groupBoxEventDistance";
            this.groupBoxEventDistance.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventDistance.Size = new System.Drawing.Size(506, 177);
            this.groupBoxEventDistance.TabIndex = 3;
            this.groupBoxEventDistance.TabStop = false;
            this.groupBoxEventDistance.Text = "Special distance event";
            // 
            // chkSpecialDistanceRange
            // 
            this.chkSpecialDistanceRange.AutoSize = true;
            this.chkSpecialDistanceRange.Location = new System.Drawing.Point(315, 25);
            this.chkSpecialDistanceRange.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSpecialDistanceRange.Name = "chkSpecialDistanceRange";
            this.chkSpecialDistanceRange.Size = new System.Drawing.Size(37, 19);
            this.chkSpecialDistanceRange.TabIndex = 29;
            this.chkSpecialDistanceRange.Text = "to";
            this.toolTip1.SetToolTip(this.chkSpecialDistanceRange, "To value must be higher than from value");
            this.chkSpecialDistanceRange.UseVisualStyleBackColor = true;
            this.chkSpecialDistanceRange.CheckedChanged += new System.EventHandler(this.chkSpecialDistanceRange_CheckedChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(161, 121);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(43, 15);
            this.label22.TabIndex = 27;
            this.label22.Text = "Source";
            // 
            // txtSpecialDistanceTo
            // 
            this.txtSpecialDistanceTo.Enabled = false;
            this.txtSpecialDistanceTo.Location = new System.Drawing.Point(356, 22);
            this.txtSpecialDistanceTo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSpecialDistanceTo.Name = "txtSpecialDistanceTo";
            this.txtSpecialDistanceTo.Size = new System.Drawing.Size(135, 23);
            this.txtSpecialDistanceTo.TabIndex = 28;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(161, 90);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(38, 15);
            this.label23.TabIndex = 26;
            this.label23.Text = "Scene";
            // 
            // comboSpecialDistanceObsScene
            // 
            this.comboSpecialDistanceObsScene.Enabled = false;
            this.comboSpecialDistanceObsScene.FormattingEnabled = true;
            this.comboSpecialDistanceObsScene.Location = new System.Drawing.Point(212, 85);
            this.comboSpecialDistanceObsScene.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboSpecialDistanceObsScene.Name = "comboSpecialDistanceObsScene";
            this.comboSpecialDistanceObsScene.Size = new System.Drawing.Size(279, 23);
            this.comboSpecialDistanceObsScene.TabIndex = 18;
            this.comboSpecialDistanceObsScene.SelectedIndexChanged += new System.EventHandler(this.comboSpecialDistanceObsScene_SelectedIndexChanged);
            // 
            // comboSpecialDistanceObsAction
            // 
            this.comboSpecialDistanceObsAction.Enabled = false;
            this.comboSpecialDistanceObsAction.FormattingEnabled = true;
            this.comboSpecialDistanceObsAction.Items.AddRange(new object[] {
            "Show",
            "Hide",
            "Toggle"});
            this.comboSpecialDistanceObsAction.Location = new System.Drawing.Point(422, 116);
            this.comboSpecialDistanceObsAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboSpecialDistanceObsAction.Name = "comboSpecialDistanceObsAction";
            this.comboSpecialDistanceObsAction.Size = new System.Drawing.Size(69, 23);
            this.comboSpecialDistanceObsAction.TabIndex = 17;
            // 
            // comboSpecialDistanceObsSource
            // 
            this.comboSpecialDistanceObsSource.Enabled = false;
            this.comboSpecialDistanceObsSource.FormattingEnabled = true;
            this.comboSpecialDistanceObsSource.Location = new System.Drawing.Point(212, 116);
            this.comboSpecialDistanceObsSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboSpecialDistanceObsSource.Name = "comboSpecialDistanceObsSource";
            this.comboSpecialDistanceObsSource.Size = new System.Drawing.Size(205, 23);
            this.comboSpecialDistanceObsSource.TabIndex = 16;
            // 
            // checkBoxSpecialDistanceAction
            // 
            this.checkBoxSpecialDistanceAction.AutoSize = true;
            this.checkBoxSpecialDistanceAction.Location = new System.Drawing.Point(7, 56);
            this.checkBoxSpecialDistanceAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxSpecialDistanceAction.Name = "checkBoxSpecialDistanceAction";
            this.checkBoxSpecialDistanceAction.Size = new System.Drawing.Size(174, 19);
            this.checkBoxSpecialDistanceAction.TabIndex = 12;
            this.checkBoxSpecialDistanceAction.Text = "Execute Streamer.Bot action";
            this.checkBoxSpecialDistanceAction.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 25);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 15);
            this.label7.TabIndex = 11;
            this.label7.Text = "Distance to achieve:";
            this.toolTip1.SetToolTip(this.label7, "When a player achieves the specified distance, the actions below are executed!");
            // 
            // chkSpecialDistanceObs
            // 
            this.chkSpecialDistanceObs.AutoSize = true;
            this.chkSpecialDistanceObs.Location = new System.Drawing.Point(7, 88);
            this.chkSpecialDistanceObs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSpecialDistanceObs.Name = "chkSpecialDistanceObs";
            this.chkSpecialDistanceObs.Size = new System.Drawing.Size(127, 19);
            this.chkSpecialDistanceObs.TabIndex = 15;
            this.chkSpecialDistanceObs.Text = "Modify OBS source";
            this.toolTip1.SetToolTip(this.chkSpecialDistanceObs, "You can show, hide or toggle any OBS source.");
            this.chkSpecialDistanceObs.UseVisualStyleBackColor = true;
            // 
            // txtSpecialDistanceFrom
            // 
            this.txtSpecialDistanceFrom.Location = new System.Drawing.Point(164, 22);
            this.txtSpecialDistanceFrom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSpecialDistanceFrom.Name = "txtSpecialDistanceFrom";
            this.txtSpecialDistanceFrom.Size = new System.Drawing.Size(143, 23);
            this.txtSpecialDistanceFrom.TabIndex = 10;
            // 
            // checkBoxSpecialDistanceLabel
            // 
            this.checkBoxSpecialDistanceLabel.AutoSize = true;
            this.checkBoxSpecialDistanceLabel.Location = new System.Drawing.Point(6, 152);
            this.checkBoxSpecialDistanceLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxSpecialDistanceLabel.Name = "checkBoxSpecialDistanceLabel";
            this.checkBoxSpecialDistanceLabel.Size = new System.Drawing.Size(168, 19);
            this.checkBoxSpecialDistanceLabel.TabIndex = 9;
            this.checkBoxSpecialDistanceLabel.Text = "Write special distance label";
            this.checkBoxSpecialDistanceLabel.UseVisualStyleBackColor = true;
            this.checkBoxSpecialDistanceLabel.Visible = false;
            // 
            // groupBoxEventSpecial
            // 
            this.groupBoxEventSpecial.Controls.Add(this.chkSpecialScoreRange);
            this.groupBoxEventSpecial.Controls.Add(this.txtSpecialScoreTo);
            this.groupBoxEventSpecial.Controls.Add(this.label20);
            this.groupBoxEventSpecial.Controls.Add(this.label21);
            this.groupBoxEventSpecial.Controls.Add(this.comboSpecialScoreObsAction);
            this.groupBoxEventSpecial.Controls.Add(this.comboSpecialScoreObsScene);
            this.groupBoxEventSpecial.Controls.Add(this.comboSpecialScoreObsSource);
            this.groupBoxEventSpecial.Controls.Add(this.chkSpecialScoreObs);
            this.groupBoxEventSpecial.Controls.Add(this.checkBoxSpecialScoreAction);
            this.groupBoxEventSpecial.Controls.Add(this.label6);
            this.groupBoxEventSpecial.Controls.Add(this.txtSpecialScoreFrom);
            this.groupBoxEventSpecial.Controls.Add(this.checkBoxSpecialScoreLabel);
            this.groupBoxEventSpecial.Location = new System.Drawing.Point(520, 7);
            this.groupBoxEventSpecial.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventSpecial.Name = "groupBoxEventSpecial";
            this.groupBoxEventSpecial.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventSpecial.Size = new System.Drawing.Size(506, 168);
            this.groupBoxEventSpecial.TabIndex = 1;
            this.groupBoxEventSpecial.TabStop = false;
            this.groupBoxEventSpecial.Text = "Special score event";
            // 
            // chkSpecialScoreRange
            // 
            this.chkSpecialScoreRange.AutoSize = true;
            this.chkSpecialScoreRange.Location = new System.Drawing.Point(315, 25);
            this.chkSpecialScoreRange.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSpecialScoreRange.Name = "chkSpecialScoreRange";
            this.chkSpecialScoreRange.Size = new System.Drawing.Size(37, 19);
            this.chkSpecialScoreRange.TabIndex = 27;
            this.chkSpecialScoreRange.Text = "to";
            this.toolTip1.SetToolTip(this.chkSpecialScoreRange, "To value must be higher than from value");
            this.chkSpecialScoreRange.UseVisualStyleBackColor = true;
            this.chkSpecialScoreRange.CheckedChanged += new System.EventHandler(this.chkSpecialScoreRange_CheckedChanged);
            // 
            // txtSpecialScoreTo
            // 
            this.txtSpecialScoreTo.Enabled = false;
            this.txtSpecialScoreTo.Location = new System.Drawing.Point(356, 22);
            this.txtSpecialScoreTo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSpecialScoreTo.Name = "txtSpecialScoreTo";
            this.txtSpecialScoreTo.Size = new System.Drawing.Size(135, 23);
            this.txtSpecialScoreTo.TabIndex = 26;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(161, 119);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(43, 15);
            this.label20.TabIndex = 25;
            this.label20.Text = "Source";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(161, 88);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(38, 15);
            this.label21.TabIndex = 24;
            this.label21.Text = "Scene";
            // 
            // comboSpecialScoreObsAction
            // 
            this.comboSpecialScoreObsAction.Enabled = false;
            this.comboSpecialScoreObsAction.FormattingEnabled = true;
            this.comboSpecialScoreObsAction.Items.AddRange(new object[] {
            "Show",
            "Hide",
            "Toggle"});
            this.comboSpecialScoreObsAction.Location = new System.Drawing.Point(425, 115);
            this.comboSpecialScoreObsAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboSpecialScoreObsAction.Name = "comboSpecialScoreObsAction";
            this.comboSpecialScoreObsAction.Size = new System.Drawing.Size(66, 23);
            this.comboSpecialScoreObsAction.TabIndex = 14;
            // 
            // comboSpecialScoreObsScene
            // 
            this.comboSpecialScoreObsScene.Enabled = false;
            this.comboSpecialScoreObsScene.FormattingEnabled = true;
            this.comboSpecialScoreObsScene.Location = new System.Drawing.Point(212, 84);
            this.comboSpecialScoreObsScene.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboSpecialScoreObsScene.Name = "comboSpecialScoreObsScene";
            this.comboSpecialScoreObsScene.Size = new System.Drawing.Size(279, 23);
            this.comboSpecialScoreObsScene.TabIndex = 14;
            this.comboSpecialScoreObsScene.SelectedIndexChanged += new System.EventHandler(this.comboSpecialScoreObsScene_SelectedIndexChanged);
            // 
            // comboSpecialScoreObsSource
            // 
            this.comboSpecialScoreObsSource.Enabled = false;
            this.comboSpecialScoreObsSource.FormattingEnabled = true;
            this.comboSpecialScoreObsSource.Location = new System.Drawing.Point(212, 115);
            this.comboSpecialScoreObsSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboSpecialScoreObsSource.Name = "comboSpecialScoreObsSource";
            this.comboSpecialScoreObsSource.Size = new System.Drawing.Size(205, 23);
            this.comboSpecialScoreObsSource.TabIndex = 13;
            // 
            // chkSpecialScoreObs
            // 
            this.chkSpecialScoreObs.AutoSize = true;
            this.chkSpecialScoreObs.Location = new System.Drawing.Point(10, 87);
            this.chkSpecialScoreObs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkSpecialScoreObs.Name = "chkSpecialScoreObs";
            this.chkSpecialScoreObs.Size = new System.Drawing.Size(127, 19);
            this.chkSpecialScoreObs.TabIndex = 12;
            this.chkSpecialScoreObs.Text = "Modify OBS source";
            this.toolTip1.SetToolTip(this.chkSpecialScoreObs, "You can show, hide or toggle any OBS source.");
            this.chkSpecialScoreObs.UseVisualStyleBackColor = true;
            // 
            // checkBoxSpecialScoreAction
            // 
            this.checkBoxSpecialScoreAction.AutoSize = true;
            this.checkBoxSpecialScoreAction.Location = new System.Drawing.Point(10, 55);
            this.checkBoxSpecialScoreAction.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxSpecialScoreAction.Name = "checkBoxSpecialScoreAction";
            this.checkBoxSpecialScoreAction.Size = new System.Drawing.Size(174, 19);
            this.checkBoxSpecialScoreAction.TabIndex = 10;
            this.checkBoxSpecialScoreAction.Text = "Execute Streamer.Bot action";
            this.checkBoxSpecialScoreAction.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 25);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "Score to achieve:";
            this.toolTip1.SetToolTip(this.label6, "When a player achieves the specified score, the actions below are executed!");
            // 
            // txtSpecialScoreFrom
            // 
            this.txtSpecialScoreFrom.Location = new System.Drawing.Point(164, 22);
            this.txtSpecialScoreFrom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSpecialScoreFrom.Name = "txtSpecialScoreFrom";
            this.txtSpecialScoreFrom.Size = new System.Drawing.Size(143, 23);
            this.txtSpecialScoreFrom.TabIndex = 3;
            // 
            // checkBoxSpecialScoreLabel
            // 
            this.checkBoxSpecialScoreLabel.AutoSize = true;
            this.checkBoxSpecialScoreLabel.Location = new System.Drawing.Point(10, 148);
            this.checkBoxSpecialScoreLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxSpecialScoreLabel.Name = "checkBoxSpecialScoreLabel";
            this.checkBoxSpecialScoreLabel.Size = new System.Drawing.Size(155, 19);
            this.checkBoxSpecialScoreLabel.TabIndex = 8;
            this.checkBoxSpecialScoreLabel.Text = "Write special score label ";
            this.checkBoxSpecialScoreLabel.UseVisualStyleBackColor = true;
            this.checkBoxSpecialScoreLabel.Visible = false;
            // 
            // tabPageLabels
            // 
            this.tabPageLabels.Controls.Add(this.groupBoxEventGeneral);
            this.tabPageLabels.Controls.Add(this.groupBoxEventGameEnd);
            this.tabPageLabels.Controls.Add(this.groupBoxEventRoundEnd);
            this.tabPageLabels.Location = new System.Drawing.Point(4, 24);
            this.tabPageLabels.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageLabels.Name = "tabPageLabels";
            this.tabPageLabels.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageLabels.Size = new System.Drawing.Size(1047, 622);
            this.tabPageLabels.TabIndex = 6;
            this.tabPageLabels.Text = "Labels";
            this.tabPageLabels.UseVisualStyleBackColor = true;
            // 
            // groupBoxEventGeneral
            // 
            this.groupBoxEventGeneral.Controls.Add(this.btnSelectLabelFolder);
            this.groupBoxEventGeneral.Controls.Add(this.label4);
            this.groupBoxEventGeneral.Controls.Add(this.textBoxLabelPath);
            this.groupBoxEventGeneral.Location = new System.Drawing.Point(268, 7);
            this.groupBoxEventGeneral.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventGeneral.Name = "groupBoxEventGeneral";
            this.groupBoxEventGeneral.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventGeneral.Size = new System.Drawing.Size(506, 115);
            this.groupBoxEventGeneral.TabIndex = 5;
            this.groupBoxEventGeneral.TabStop = false;
            this.groupBoxEventGeneral.Text = "General settings";
            // 
            // btnSelectLabelFolder
            // 
            this.btnSelectLabelFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectLabelFolder.Location = new System.Drawing.Point(412, 21);
            this.btnSelectLabelFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSelectLabelFolder.Name = "btnSelectLabelFolder";
            this.btnSelectLabelFolder.Size = new System.Drawing.Size(88, 23);
            this.btnSelectLabelFolder.TabIndex = 2;
            this.btnSelectLabelFolder.Text = "...";
            this.btnSelectLabelFolder.UseVisualStyleBackColor = true;
            this.btnSelectLabelFolder.Click += new System.EventHandler(this.btnSelectLabelFolder_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 24);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Label output folder";
            // 
            // textBoxLabelPath
            // 
            this.textBoxLabelPath.Location = new System.Drawing.Point(125, 21);
            this.textBoxLabelPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxLabelPath.Name = "textBoxLabelPath";
            this.textBoxLabelPath.ReadOnly = true;
            this.textBoxLabelPath.Size = new System.Drawing.Size(279, 23);
            this.textBoxLabelPath.TabIndex = 0;
            // 
            // groupBoxEventGameEnd
            // 
            this.groupBoxEventGameEnd.Controls.Add(this.checkBoxEventWriteGameHighscore);
            this.groupBoxEventGameEnd.Controls.Add(this.checkBoxEventWriteGameThird);
            this.groupBoxEventGameEnd.Controls.Add(this.checkBoxEventWriteGameSecond);
            this.groupBoxEventGameEnd.Controls.Add(this.checkBoxEventWriteGameWinner);
            this.groupBoxEventGameEnd.Location = new System.Drawing.Point(7, 216);
            this.groupBoxEventGameEnd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventGameEnd.Name = "groupBoxEventGameEnd";
            this.groupBoxEventGameEnd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventGameEnd.Size = new System.Drawing.Size(233, 162);
            this.groupBoxEventGameEnd.TabIndex = 4;
            this.groupBoxEventGameEnd.TabStop = false;
            this.groupBoxEventGameEnd.Text = "After a game ends";
            this.toolTip1.SetToolTip(this.groupBoxEventGameEnd, "Write these labels when a game ends");
            // 
            // checkBoxEventWriteGameHighscore
            // 
            this.checkBoxEventWriteGameHighscore.AutoSize = true;
            this.checkBoxEventWriteGameHighscore.Location = new System.Drawing.Point(7, 102);
            this.checkBoxEventWriteGameHighscore.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEventWriteGameHighscore.Name = "checkBoxEventWriteGameHighscore";
            this.checkBoxEventWriteGameHighscore.Size = new System.Drawing.Size(137, 19);
            this.checkBoxEventWriteGameHighscore.TabIndex = 7;
            this.checkBoxEventWriteGameHighscore.Text = "Write highscore label";
            this.checkBoxEventWriteGameHighscore.UseVisualStyleBackColor = true;
            // 
            // checkBoxEventWriteGameThird
            // 
            this.checkBoxEventWriteGameThird.AutoSize = true;
            this.checkBoxEventWriteGameThird.Location = new System.Drawing.Point(7, 75);
            this.checkBoxEventWriteGameThird.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEventWriteGameThird.Name = "checkBoxEventWriteGameThird";
            this.checkBoxEventWriteGameThird.Size = new System.Drawing.Size(133, 19);
            this.checkBoxEventWriteGameThird.TabIndex = 6;
            this.checkBoxEventWriteGameThird.Text = "Write 3rd place label";
            this.checkBoxEventWriteGameThird.UseVisualStyleBackColor = true;
            // 
            // checkBoxEventWriteGameSecond
            // 
            this.checkBoxEventWriteGameSecond.AutoSize = true;
            this.checkBoxEventWriteGameSecond.Location = new System.Drawing.Point(7, 48);
            this.checkBoxEventWriteGameSecond.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEventWriteGameSecond.Name = "checkBoxEventWriteGameSecond";
            this.checkBoxEventWriteGameSecond.Size = new System.Drawing.Size(136, 19);
            this.checkBoxEventWriteGameSecond.TabIndex = 5;
            this.checkBoxEventWriteGameSecond.Text = "Write 2nd place label";
            this.checkBoxEventWriteGameSecond.UseVisualStyleBackColor = true;
            // 
            // checkBoxEventWriteGameWinner
            // 
            this.checkBoxEventWriteGameWinner.AutoSize = true;
            this.checkBoxEventWriteGameWinner.Location = new System.Drawing.Point(7, 22);
            this.checkBoxEventWriteGameWinner.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEventWriteGameWinner.Name = "checkBoxEventWriteGameWinner";
            this.checkBoxEventWriteGameWinner.Size = new System.Drawing.Size(121, 19);
            this.checkBoxEventWriteGameWinner.TabIndex = 4;
            this.checkBoxEventWriteGameWinner.Text = "Write winner label";
            this.checkBoxEventWriteGameWinner.UseVisualStyleBackColor = true;
            // 
            // groupBoxEventRoundEnd
            // 
            this.groupBoxEventRoundEnd.Controls.Add(this.checkBoxEventWriteRoundHighscore);
            this.groupBoxEventRoundEnd.Controls.Add(this.checkBoxEventWriteRoundThird);
            this.groupBoxEventRoundEnd.Controls.Add(this.checkBoxEventWriteRoundSecond);
            this.groupBoxEventRoundEnd.Controls.Add(this.checkBoxEventWriteRoundWinner);
            this.groupBoxEventRoundEnd.Location = new System.Drawing.Point(7, 7);
            this.groupBoxEventRoundEnd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventRoundEnd.Name = "groupBoxEventRoundEnd";
            this.groupBoxEventRoundEnd.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxEventRoundEnd.Size = new System.Drawing.Size(233, 202);
            this.groupBoxEventRoundEnd.TabIndex = 3;
            this.groupBoxEventRoundEnd.TabStop = false;
            this.groupBoxEventRoundEnd.Text = "After a round ends";
            this.toolTip1.SetToolTip(this.groupBoxEventRoundEnd, "Write these labels when a round ends");
            // 
            // checkBoxEventWriteRoundHighscore
            // 
            this.checkBoxEventWriteRoundHighscore.AutoSize = true;
            this.checkBoxEventWriteRoundHighscore.Location = new System.Drawing.Point(7, 103);
            this.checkBoxEventWriteRoundHighscore.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEventWriteRoundHighscore.Name = "checkBoxEventWriteRoundHighscore";
            this.checkBoxEventWriteRoundHighscore.Size = new System.Drawing.Size(137, 19);
            this.checkBoxEventWriteRoundHighscore.TabIndex = 3;
            this.checkBoxEventWriteRoundHighscore.Text = "Write highscore label";
            this.checkBoxEventWriteRoundHighscore.UseVisualStyleBackColor = true;
            // 
            // checkBoxEventWriteRoundThird
            // 
            this.checkBoxEventWriteRoundThird.AutoSize = true;
            this.checkBoxEventWriteRoundThird.Location = new System.Drawing.Point(7, 76);
            this.checkBoxEventWriteRoundThird.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEventWriteRoundThird.Name = "checkBoxEventWriteRoundThird";
            this.checkBoxEventWriteRoundThird.Size = new System.Drawing.Size(133, 19);
            this.checkBoxEventWriteRoundThird.TabIndex = 2;
            this.checkBoxEventWriteRoundThird.Text = "Write 3rd place label";
            this.checkBoxEventWriteRoundThird.UseVisualStyleBackColor = true;
            // 
            // checkBoxEventWriteRoundSecond
            // 
            this.checkBoxEventWriteRoundSecond.AutoSize = true;
            this.checkBoxEventWriteRoundSecond.Location = new System.Drawing.Point(7, 50);
            this.checkBoxEventWriteRoundSecond.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEventWriteRoundSecond.Name = "checkBoxEventWriteRoundSecond";
            this.checkBoxEventWriteRoundSecond.Size = new System.Drawing.Size(136, 19);
            this.checkBoxEventWriteRoundSecond.TabIndex = 1;
            this.checkBoxEventWriteRoundSecond.Text = "Write 2nd place label";
            this.checkBoxEventWriteRoundSecond.UseVisualStyleBackColor = true;
            // 
            // checkBoxEventWriteRoundWinner
            // 
            this.checkBoxEventWriteRoundWinner.AutoSize = true;
            this.checkBoxEventWriteRoundWinner.Location = new System.Drawing.Point(7, 23);
            this.checkBoxEventWriteRoundWinner.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxEventWriteRoundWinner.Name = "checkBoxEventWriteRoundWinner";
            this.checkBoxEventWriteRoundWinner.Size = new System.Drawing.Size(121, 19);
            this.checkBoxEventWriteRoundWinner.TabIndex = 0;
            this.checkBoxEventWriteRoundWinner.Text = "Write winner label";
            this.checkBoxEventWriteRoundWinner.UseVisualStyleBackColor = true;
            // 
            // tabPageOverlay
            // 
            this.tabPageOverlay.Controls.Add(this.flowLayoutPanel2);
            this.tabPageOverlay.Controls.Add(this.panel1);
            this.tabPageOverlay.Controls.Add(this.treeView1);
            this.tabPageOverlay.Location = new System.Drawing.Point(4, 24);
            this.tabPageOverlay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageOverlay.Name = "tabPageOverlay";
            this.tabPageOverlay.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageOverlay.Size = new System.Drawing.Size(1047, 622);
            this.tabPageOverlay.TabIndex = 3;
            this.tabPageOverlay.Text = "Overlay";
            this.tabPageOverlay.UseVisualStyleBackColor = true;
            this.tabPageOverlay.Click += new System.EventHandler(this.tabPageOverlay_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.groupScoreboard);
            this.flowLayoutPanel2.Controls.Add(this.groupMarker);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(10, 6);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1030, 355);
            this.flowLayoutPanel2.TabIndex = 33;
            // 
            // groupScoreboard
            // 
            this.groupScoreboard.Controls.Add(this.flowScoreboard);
            this.groupScoreboard.Location = new System.Drawing.Point(3, 3);
            this.groupScoreboard.Name = "groupScoreboard";
            this.groupScoreboard.Size = new System.Drawing.Size(391, 341);
            this.groupScoreboard.TabIndex = 32;
            this.groupScoreboard.TabStop = false;
            this.groupScoreboard.Text = "Scoreboard settings";
            // 
            // flowScoreboard
            // 
            this.flowScoreboard.Controls.Add(this.panelScoreBoardUnit);
            this.flowScoreboard.Controls.Add(this.panelScoreboardRounding);
            this.flowScoreboard.Controls.Add(this.panelScoreboardFontSize);
            this.flowScoreboard.Controls.Add(this.panelScoreBoardColors);
            this.flowScoreboard.Controls.Add(this.panelScoreBoardSpeed);
            this.flowScoreboard.Controls.Add(this.panelScoreboardCheckboxes);
            this.flowScoreboard.Location = new System.Drawing.Point(3, 17);
            this.flowScoreboard.Name = "flowScoreboard";
            this.flowScoreboard.Size = new System.Drawing.Size(385, 319);
            this.flowScoreboard.TabIndex = 33;
            // 
            // panelScoreBoardUnit
            // 
            this.panelScoreBoardUnit.Controls.Add(this.label5);
            this.panelScoreBoardUnit.Controls.Add(this.comboBoxOverlayUnits);
            this.panelScoreBoardUnit.Location = new System.Drawing.Point(3, 3);
            this.panelScoreBoardUnit.Name = "panelScoreBoardUnit";
            this.panelScoreBoardUnit.Size = new System.Drawing.Size(171, 30);
            this.panelScoreBoardUnit.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Unit";
            // 
            // comboBoxOverlayUnits
            // 
            this.comboBoxOverlayUnits.FormattingEnabled = true;
            this.comboBoxOverlayUnits.Items.AddRange(new object[] {
            "km/m",
            "mi/ft"});
            this.comboBoxOverlayUnits.Location = new System.Drawing.Point(41, 2);
            this.comboBoxOverlayUnits.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBoxOverlayUnits.Name = "comboBoxOverlayUnits";
            this.comboBoxOverlayUnits.Size = new System.Drawing.Size(121, 23);
            this.comboBoxOverlayUnits.TabIndex = 0;
            // 
            // panelScoreboardRounding
            // 
            this.panelScoreboardRounding.Controls.Add(this.label10);
            this.panelScoreboardRounding.Controls.Add(this.label11);
            this.panelScoreboardRounding.Controls.Add(this.numericRoundingDigit);
            this.panelScoreboardRounding.Location = new System.Drawing.Point(3, 39);
            this.panelScoreboardRounding.Name = "panelScoreboardRounding";
            this.panelScoreboardRounding.Size = new System.Drawing.Size(242, 30);
            this.panelScoreboardRounding.TabIndex = 33;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 7);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 15);
            this.label10.TabIndex = 7;
            this.label10.Text = "Round distances to";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(193, 7);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 15);
            this.label11.TabIndex = 9;
            this.label11.Text = "digits";
            // 
            // numericRoundingDigit
            // 
            this.numericRoundingDigit.Location = new System.Drawing.Point(133, 4);
            this.numericRoundingDigit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numericRoundingDigit.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericRoundingDigit.Name = "numericRoundingDigit";
            this.numericRoundingDigit.Size = new System.Drawing.Size(52, 23);
            this.numericRoundingDigit.TabIndex = 8;
            this.toolTip1.SetToolTip(this.numericRoundingDigit, "Amount of digits to display after \".\"");
            this.numericRoundingDigit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // panelScoreboardFontSize
            // 
            this.panelScoreboardFontSize.Controls.Add(this.label28);
            this.panelScoreboardFontSize.Controls.Add(this.OverlayFontSizetextBox1);
            this.panelScoreboardFontSize.Controls.Add(this.OverlayFontSizeUnitcomboBox1);
            this.panelScoreboardFontSize.Location = new System.Drawing.Point(3, 75);
            this.panelScoreboardFontSize.Name = "panelScoreboardFontSize";
            this.panelScoreboardFontSize.Size = new System.Drawing.Size(256, 27);
            this.panelScoreboardFontSize.TabIndex = 34;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 9);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(114, 15);
            this.label28.TabIndex = 13;
            this.label28.Text = "Scoreboard font size";
            // 
            // OverlayFontSizetextBox1
            // 
            this.OverlayFontSizetextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OverlayFontSizetextBox1.Location = new System.Drawing.Point(134, 3);
            this.OverlayFontSizetextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.OverlayFontSizetextBox1.Name = "OverlayFontSizetextBox1";
            this.OverlayFontSizetextBox1.Size = new System.Drawing.Size(52, 21);
            this.OverlayFontSizetextBox1.TabIndex = 15;
            this.OverlayFontSizetextBox1.Text = "1";
            this.toolTip1.SetToolTip(this.OverlayFontSizetextBox1, "Scoreboard font size value");
            // 
            // OverlayFontSizeUnitcomboBox1
            // 
            this.OverlayFontSizeUnitcomboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OverlayFontSizeUnitcomboBox1.FormattingEnabled = true;
            this.OverlayFontSizeUnitcomboBox1.Items.AddRange(new object[] {
            "cm",
            "mm",
            "in",
            "pt",
            "px",
            "%",
            "rem",
            "em",
            "vw",
            "vh",
            "vmin",
            "vmax"});
            this.OverlayFontSizeUnitcomboBox1.Location = new System.Drawing.Point(194, 3);
            this.OverlayFontSizeUnitcomboBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.OverlayFontSizeUnitcomboBox1.Name = "OverlayFontSizeUnitcomboBox1";
            this.OverlayFontSizeUnitcomboBox1.Size = new System.Drawing.Size(53, 21);
            this.OverlayFontSizeUnitcomboBox1.TabIndex = 14;
            this.OverlayFontSizeUnitcomboBox1.Text = "rem";
            this.toolTip1.SetToolTip(this.OverlayFontSizeUnitcomboBox1, "Scoreboard font size unit name");
            // 
            // panelScoreBoardColors
            // 
            this.panelScoreBoardColors.Controls.Add(this.label29);
            this.panelScoreBoardColors.Controls.Add(this.label30);
            this.panelScoreBoardColors.Controls.Add(this.ScoreboardFGDisplaybutton);
            this.panelScoreBoardColors.Controls.Add(this.ScoreboardBGDisplaybutton);
            this.panelScoreBoardColors.Controls.Add(this.label31);
            this.panelScoreBoardColors.Controls.Add(this.ScoreboardFGAnumericUpDown2);
            this.panelScoreBoardColors.Controls.Add(this.ScoreboardBGAnumericUpDown2);
            this.panelScoreBoardColors.Controls.Add(this.label32);
            this.panelScoreBoardColors.Location = new System.Drawing.Point(3, 108);
            this.panelScoreBoardColors.Name = "panelScoreBoardColors";
            this.panelScoreBoardColors.Size = new System.Drawing.Size(381, 60);
            this.panelScoreBoardColors.TabIndex = 35;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(4, 8);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(120, 15);
            this.label29.TabIndex = 16;
            this.label29.Text = "Scoreboard text color";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(4, 36);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(164, 15);
            this.label30.TabIndex = 17;
            this.label30.Text = "Scoreboard background color";
            // 
            // ScoreboardFGDisplaybutton
            // 
            this.ScoreboardFGDisplaybutton.Location = new System.Drawing.Point(192, 3);
            this.ScoreboardFGDisplaybutton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ScoreboardFGDisplaybutton.Name = "ScoreboardFGDisplaybutton";
            this.ScoreboardFGDisplaybutton.Size = new System.Drawing.Size(27, 27);
            this.ScoreboardFGDisplaybutton.TabIndex = 20;
            this.toolTip1.SetToolTip(this.ScoreboardFGDisplaybutton, "Click to open a color dialog");
            this.ScoreboardFGDisplaybutton.UseVisualStyleBackColor = true;
            this.ScoreboardFGDisplaybutton.Click += new System.EventHandler(this.ScoreboardFGDisplaybutton_Click);
            // 
            // ScoreboardBGDisplaybutton
            // 
            this.ScoreboardBGDisplaybutton.Location = new System.Drawing.Point(192, 30);
            this.ScoreboardBGDisplaybutton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ScoreboardBGDisplaybutton.Name = "ScoreboardBGDisplaybutton";
            this.ScoreboardBGDisplaybutton.Size = new System.Drawing.Size(27, 27);
            this.ScoreboardBGDisplaybutton.TabIndex = 21;
            this.toolTip1.SetToolTip(this.ScoreboardBGDisplaybutton, "Click to open a color dialog");
            this.ScoreboardBGDisplaybutton.UseVisualStyleBackColor = true;
            this.ScoreboardBGDisplaybutton.Click += new System.EventHandler(this.ScoreboardBGDisplaybutton_Click);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(226, 8);
            this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(88, 15);
            this.label31.TabIndex = 22;
            this.label31.Text = "Opacity (0-255)";
            // 
            // ScoreboardFGAnumericUpDown2
            // 
            this.ScoreboardFGAnumericUpDown2.Location = new System.Drawing.Point(325, 6);
            this.ScoreboardFGAnumericUpDown2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ScoreboardFGAnumericUpDown2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ScoreboardFGAnumericUpDown2.Name = "ScoreboardFGAnumericUpDown2";
            this.ScoreboardFGAnumericUpDown2.Size = new System.Drawing.Size(52, 23);
            this.ScoreboardFGAnumericUpDown2.TabIndex = 24;
            this.toolTip1.SetToolTip(this.ScoreboardFGAnumericUpDown2, "0: Transparent; 255: Opaque");
            this.ScoreboardFGAnumericUpDown2.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // ScoreboardBGAnumericUpDown2
            // 
            this.ScoreboardBGAnumericUpDown2.Location = new System.Drawing.Point(325, 34);
            this.ScoreboardBGAnumericUpDown2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ScoreboardBGAnumericUpDown2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ScoreboardBGAnumericUpDown2.Name = "ScoreboardBGAnumericUpDown2";
            this.ScoreboardBGAnumericUpDown2.Size = new System.Drawing.Size(52, 23);
            this.ScoreboardBGAnumericUpDown2.TabIndex = 25;
            this.toolTip1.SetToolTip(this.ScoreboardBGAnumericUpDown2, "0: Transparent; 255: Opaque");
            this.ScoreboardBGAnumericUpDown2.Value = new decimal(new int[] {
            127,
            0,
            0,
            0});
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(226, 36);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(88, 15);
            this.label32.TabIndex = 26;
            this.label32.Text = "Opacity (0-255)";
            // 
            // panelScoreBoardSpeed
            // 
            this.panelScoreBoardSpeed.Controls.Add(this.label34);
            this.panelScoreBoardSpeed.Controls.Add(this.ScoreboardScrollSpeednumericUpDown1);
            this.panelScoreBoardSpeed.Controls.Add(this.label33);
            this.panelScoreBoardSpeed.Location = new System.Drawing.Point(3, 174);
            this.panelScoreBoardSpeed.Name = "panelScoreBoardSpeed";
            this.panelScoreBoardSpeed.Size = new System.Drawing.Size(249, 29);
            this.panelScoreBoardSpeed.TabIndex = 35;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(4, 5);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(132, 15);
            this.label34.TabIndex = 27;
            this.label34.Text = "Scoreboard scroll speed";
            // 
            // ScoreboardScrollSpeednumericUpDown1
            // 
            this.ScoreboardScrollSpeednumericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ScoreboardScrollSpeednumericUpDown1.Location = new System.Drawing.Point(150, 3);
            this.ScoreboardScrollSpeednumericUpDown1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ScoreboardScrollSpeednumericUpDown1.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.ScoreboardScrollSpeednumericUpDown1.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.ScoreboardScrollSpeednumericUpDown1.Name = "ScoreboardScrollSpeednumericUpDown1";
            this.ScoreboardScrollSpeednumericUpDown1.Size = new System.Drawing.Size(52, 23);
            this.ScoreboardScrollSpeednumericUpDown1.TabIndex = 28;
            this.toolTip1.SetToolTip(this.ScoreboardScrollSpeednumericUpDown1, "Pixel per second speed of auto scroll");
            this.ScoreboardScrollSpeednumericUpDown1.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(209, 5);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(30, 15);
            this.label33.TabIndex = 29;
            this.label33.Text = "px/s";
            // 
            // panelScoreboardCheckboxes
            // 
            this.panelScoreboardCheckboxes.Controls.Add(this.chkUseUsStateFlags);
            this.panelScoreboardCheckboxes.Controls.Add(this.checkBoxOverlayDisplayCorrectLocations);
            this.panelScoreboardCheckboxes.Controls.Add(this.checkBoxOverlayRegionalFlags);
            this.panelScoreboardCheckboxes.Controls.Add(this.checkBoxOverlayUseWrongRegionColors);
            this.panelScoreboardCheckboxes.Location = new System.Drawing.Point(3, 209);
            this.panelScoreboardCheckboxes.Name = "panelScoreboardCheckboxes";
            this.panelScoreboardCheckboxes.Size = new System.Drawing.Size(338, 105);
            this.panelScoreboardCheckboxes.TabIndex = 36;
            // 
            // chkUseUsStateFlags
            // 
            this.chkUseUsStateFlags.AutoSize = true;
            this.chkUseUsStateFlags.Location = new System.Drawing.Point(4, 82);
            this.chkUseUsStateFlags.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkUseUsStateFlags.Name = "chkUseUsStateFlags";
            this.chkUseUsStateFlags.Size = new System.Drawing.Size(118, 19);
            this.chkUseUsStateFlags.TabIndex = 30;
            this.chkUseUsStateFlags.Text = "Use US state flags";
            this.toolTip1.SetToolTip(this.chkUseUsStateFlags, "On: Display US state flags instead of flag of USA; Off: Display USA flag on any c" +
        "ase");
            this.chkUseUsStateFlags.UseVisualStyleBackColor = true;
            // 
            // checkBoxOverlayDisplayCorrectLocations
            // 
            this.checkBoxOverlayDisplayCorrectLocations.AutoSize = true;
            this.checkBoxOverlayDisplayCorrectLocations.Location = new System.Drawing.Point(4, 4);
            this.checkBoxOverlayDisplayCorrectLocations.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxOverlayDisplayCorrectLocations.Name = "checkBoxOverlayDisplayCorrectLocations";
            this.checkBoxOverlayDisplayCorrectLocations.Size = new System.Drawing.Size(260, 19);
            this.checkBoxOverlayDisplayCorrectLocations.TabIndex = 6;
            this.checkBoxOverlayDisplayCorrectLocations.Text = "Display correct location\'s flag on scoreboard";
            this.toolTip1.SetToolTip(this.checkBoxOverlayDisplayCorrectLocations, "On: Display flags of each round in game summary scoreboard; Off: No extra display" +
        " for flags");
            this.checkBoxOverlayDisplayCorrectLocations.UseVisualStyleBackColor = true;
            // 
            // checkBoxOverlayRegionalFlags
            // 
            this.checkBoxOverlayRegionalFlags.AutoSize = true;
            this.checkBoxOverlayRegionalFlags.Location = new System.Drawing.Point(4, 57);
            this.checkBoxOverlayRegionalFlags.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxOverlayRegionalFlags.Name = "checkBoxOverlayRegionalFlags";
            this.checkBoxOverlayRegionalFlags.Size = new System.Drawing.Size(314, 19);
            this.checkBoxOverlayRegionalFlags.TabIndex = 4;
            this.checkBoxOverlayRegionalFlags.Text = "Display regional flags over country flags when possible";
            this.toolTip1.SetToolTip(this.checkBoxOverlayRegionalFlags, "On: Display regional flags where possible; Off: Only use country flags");
            this.checkBoxOverlayRegionalFlags.UseVisualStyleBackColor = true;
            // 
            // checkBoxOverlayUseWrongRegionColors
            // 
            this.checkBoxOverlayUseWrongRegionColors.AutoSize = true;
            this.checkBoxOverlayUseWrongRegionColors.Location = new System.Drawing.Point(4, 29);
            this.checkBoxOverlayUseWrongRegionColors.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxOverlayUseWrongRegionColors.Name = "checkBoxOverlayUseWrongRegionColors";
            this.checkBoxOverlayUseWrongRegionColors.Size = new System.Drawing.Size(332, 19);
            this.checkBoxOverlayUseWrongRegionColors.TabIndex = 5;
            this.checkBoxOverlayUseWrongRegionColors.Text = "Use different flag background for incorrect region guesses";
            this.toolTip1.SetToolTip(this.checkBoxOverlayUseWrongRegionColors, "On: Display purple background for flags of incorrect region guesses; Off: Prefer " +
        "green color for incorrect regions");
            this.checkBoxOverlayUseWrongRegionColors.UseVisualStyleBackColor = true;
            // 
            // groupMarker
            // 
            this.groupMarker.Controls.Add(this.chkGuessInfoTime);
            this.groupMarker.Controls.Add(this.chkGuessInfoCoordinates);
            this.groupMarker.Controls.Add(this.groupBox1);
            this.groupMarker.Controls.Add(this.chkGuessInfoDistance);
            this.groupMarker.Controls.Add(this.chkGuessInfoScore);
            this.groupMarker.Controls.Add(this.chkGuessInfoStreak);
            this.groupMarker.Controls.Add(this.label39);
            this.groupMarker.Location = new System.Drawing.Point(400, 3);
            this.groupMarker.Name = "groupMarker";
            this.groupMarker.Size = new System.Drawing.Size(620, 341);
            this.groupMarker.TabIndex = 31;
            this.groupMarker.TabStop = false;
            this.groupMarker.Text = "Marker popup settings";
            // 
            // chkGuessInfoTime
            // 
            this.chkGuessInfoTime.AutoSize = true;
            this.chkGuessInfoTime.Location = new System.Drawing.Point(6, 159);
            this.chkGuessInfoTime.Name = "chkGuessInfoTime";
            this.chkGuessInfoTime.Size = new System.Drawing.Size(159, 19);
            this.chkGuessInfoTime.TabIndex = 5;
            this.chkGuessInfoTime.Text = "Guess time (e.g. \"18.25s\")";
            this.chkGuessInfoTime.UseVisualStyleBackColor = true;
            // 
            // chkGuessInfoCoordinates
            // 
            this.chkGuessInfoCoordinates.AutoSize = true;
            this.chkGuessInfoCoordinates.Location = new System.Drawing.Point(6, 84);
            this.chkGuessInfoCoordinates.Name = "chkGuessInfoCoordinates";
            this.chkGuessInfoCoordinates.Size = new System.Drawing.Size(246, 19);
            this.chkGuessInfoCoordinates.TabIndex = 4;
            this.chkGuessInfoCoordinates.Text = "Coordinates (e.g. \"[FLAG]: -12.000, 7.000\")";
            this.chkGuessInfoCoordinates.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMaxGuessDisplayCount);
            this.groupBox1.Controls.Add(this.numMaxGuessDisplayCount);
            this.groupBox1.Controls.Add(this.lblNoOfMarkers);
            this.groupBox1.Controls.Add(this.numMaxMarkerCount);
            this.groupBox1.Controls.Add(this.checkBoxMarkerClustersEnabled);
            this.groupBox1.Location = new System.Drawing.Point(0, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(620, 158);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General marker settings";
            // 
            // lblMaxGuessDisplayCount
            // 
            this.lblMaxGuessDisplayCount.AutoSize = true;
            this.lblMaxGuessDisplayCount.Location = new System.Drawing.Point(6, 73);
            this.lblMaxGuessDisplayCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxGuessDisplayCount.Name = "lblMaxGuessDisplayCount";
            this.lblMaxGuessDisplayCount.Size = new System.Drawing.Size(243, 30);
            this.lblMaxGuessDisplayCount.TabIndex = 11;
            this.lblMaxGuessDisplayCount.Text = "Display top X players guesses when showing \r\nguesses from selected rounds:";
            // 
            // numMaxGuessDisplayCount
            // 
            this.numMaxGuessDisplayCount.Location = new System.Drawing.Point(256, 77);
            this.numMaxGuessDisplayCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numMaxGuessDisplayCount.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.numMaxGuessDisplayCount.Name = "numMaxGuessDisplayCount";
            this.numMaxGuessDisplayCount.Size = new System.Drawing.Size(52, 23);
            this.numMaxGuessDisplayCount.TabIndex = 12;
            this.toolTip1.SetToolTip(this.numMaxGuessDisplayCount, "Display top X players guesses when showing \r\nguesses from selected rounds\r\n");
            this.numMaxGuessDisplayCount.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // lblNoOfMarkers
            // 
            this.lblNoOfMarkers.AutoSize = true;
            this.lblNoOfMarkers.Location = new System.Drawing.Point(6, 44);
            this.lblNoOfMarkers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNoOfMarkers.Name = "lblNoOfMarkers";
            this.lblNoOfMarkers.Size = new System.Drawing.Size(242, 15);
            this.lblNoOfMarkers.TabIndex = 9;
            this.lblNoOfMarkers.Text = "Number of markers to display on round end:";
            // 
            // numMaxMarkerCount
            // 
            this.numMaxMarkerCount.Location = new System.Drawing.Point(256, 42);
            this.numMaxMarkerCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numMaxMarkerCount.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.numMaxMarkerCount.Name = "numMaxMarkerCount";
            this.numMaxMarkerCount.Size = new System.Drawing.Size(52, 23);
            this.numMaxMarkerCount.TabIndex = 10;
            this.toolTip1.SetToolTip(this.numMaxMarkerCount, "Number of markers to display on round end");
            this.numMaxMarkerCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // checkBoxMarkerClustersEnabled
            // 
            this.checkBoxMarkerClustersEnabled.AutoSize = true;
            this.checkBoxMarkerClustersEnabled.Location = new System.Drawing.Point(6, 22);
            this.checkBoxMarkerClustersEnabled.Name = "checkBoxMarkerClustersEnabled";
            this.checkBoxMarkerClustersEnabled.Size = new System.Drawing.Size(158, 19);
            this.checkBoxMarkerClustersEnabled.TabIndex = 0;
            this.checkBoxMarkerClustersEnabled.Text = "Enable clusters/grouping";
            this.checkBoxMarkerClustersEnabled.UseVisualStyleBackColor = true;
            // 
            // chkGuessInfoDistance
            // 
            this.chkGuessInfoDistance.AutoSize = true;
            this.chkGuessInfoDistance.Location = new System.Drawing.Point(6, 109);
            this.chkGuessInfoDistance.Name = "chkGuessInfoDistance";
            this.chkGuessInfoDistance.Size = new System.Drawing.Size(170, 19);
            this.chkGuessInfoDistance.TabIndex = 3;
            this.chkGuessInfoDistance.Text = "Distance (e.g. \"10400.8km\")";
            this.chkGuessInfoDistance.UseVisualStyleBackColor = true;
            // 
            // chkGuessInfoScore
            // 
            this.chkGuessInfoScore.AutoSize = true;
            this.chkGuessInfoScore.Location = new System.Drawing.Point(6, 134);
            this.chkGuessInfoScore.Name = "chkGuessInfoScore";
            this.chkGuessInfoScore.Size = new System.Drawing.Size(146, 19);
            this.chkGuessInfoScore.TabIndex = 2;
            this.chkGuessInfoScore.Text = "Score (e.g. \"15 points\")";
            this.chkGuessInfoScore.UseVisualStyleBackColor = true;
            // 
            // chkGuessInfoStreak
            // 
            this.chkGuessInfoStreak.AutoSize = true;
            this.chkGuessInfoStreak.Location = new System.Drawing.Point(6, 59);
            this.chkGuessInfoStreak.Name = "chkGuessInfoStreak";
            this.chkGuessInfoStreak.Size = new System.Drawing.Size(183, 19);
            this.chkGuessInfoStreak.TabIndex = 1;
            this.chkGuessInfoStreak.Text = "Current streak (e.g. \"5 streak\")";
            this.chkGuessInfoStreak.UseVisualStyleBackColor = true;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(6, 19);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(248, 30);
            this.label39.TabIndex = 0;
            this.label39.Text = "Choose which information you want to show \r\non the info popup above a guess marke" +
    "r";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(229, 431);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 153);
            this.panel1.TabIndex = 11;
            this.panel1.Visible = false;
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(10, 431);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(212, 152);
            this.treeView1.TabIndex = 10;
            this.treeView1.Visible = false;
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newColumnToolStripMenuItem,
            this.deleteColumnToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(152, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // newColumnToolStripMenuItem
            // 
            this.newColumnToolStripMenuItem.Name = "newColumnToolStripMenuItem";
            this.newColumnToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.newColumnToolStripMenuItem.Text = "New column";
            this.newColumnToolStripMenuItem.Click += new System.EventHandler(this.newColumnToolStripMenuItem_Click);
            // 
            // deleteColumnToolStripMenuItem
            // 
            this.deleteColumnToolStripMenuItem.Name = "deleteColumnToolStripMenuItem";
            this.deleteColumnToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.deleteColumnToolStripMenuItem.Text = "Delete column";
            this.deleteColumnToolStripMenuItem.Click += new System.EventHandler(this.deleteColumnToolStripMenuItem_Click);
            // 
            // tabUsers
            // 
            this.tabUsers.Controls.Add(this.chkAutoBanCGTrolls);
            this.tabUsers.Controls.Add(this.chkAutoBan);
            this.tabUsers.Controls.Add(this.label35);
            this.tabUsers.Controls.Add(this.chkListBoxBannedPlayers);
            this.tabUsers.Location = new System.Drawing.Point(4, 24);
            this.tabUsers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabUsers.Size = new System.Drawing.Size(1047, 622);
            this.tabUsers.TabIndex = 7;
            this.tabUsers.Text = "Users";
            this.tabUsers.UseVisualStyleBackColor = true;
            // 
            // chkAutoBanCGTrolls
            // 
            this.chkAutoBanCGTrolls.AutoSize = true;
            this.chkAutoBanCGTrolls.Location = new System.Drawing.Point(8, 105);
            this.chkAutoBanCGTrolls.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkAutoBanCGTrolls.Name = "chkAutoBanCGTrolls";
            this.chkAutoBanCGTrolls.Size = new System.Drawing.Size(254, 19);
            this.chkAutoBanCGTrolls.TabIndex = 8;
            this.chkAutoBanCGTrolls.Text = "Automatically ban known ChatGuessr trolls";
            this.toolTip1.SetToolTip(this.chkAutoBanCGTrolls, "When activated, banning a user on Twitch automatically bans them from playing GC " +
        "as well");
            this.chkAutoBanCGTrolls.UseVisualStyleBackColor = true;
            // 
            // chkAutoBan
            // 
            this.chkAutoBan.AutoSize = true;
            this.chkAutoBan.Location = new System.Drawing.Point(8, 80);
            this.chkAutoBan.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkAutoBan.Name = "chkAutoBan";
            this.chkAutoBan.Size = new System.Drawing.Size(339, 19);
            this.chkAutoBan.TabIndex = 7;
            this.chkAutoBan.Text = "Automatically ban users when they are banned from Twitch";
            this.toolTip1.SetToolTip(this.chkAutoBan, "When activated, banning a user on Twitch automatically bans them from playing GC " +
        "as well");
            this.chkAutoBan.UseVisualStyleBackColor = true;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(7, 9);
            this.label35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(294, 60);
            this.label35.TabIndex = 2;
            this.label35.Text = "This is a list of all your players.\r\n\r\nTo ban a player select them and activate t" +
    "he checkbox.\r\nTo unban a player, select and uncheck them.";
            // 
            // chkListBoxBannedPlayers
            // 
            this.chkListBoxBannedPlayers.FormattingEnabled = true;
            this.chkListBoxBannedPlayers.Location = new System.Drawing.Point(4, 136);
            this.chkListBoxBannedPlayers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkListBoxBannedPlayers.Name = "chkListBoxBannedPlayers";
            this.chkListBoxBannedPlayers.Size = new System.Drawing.Size(307, 436);
            this.chkListBoxBannedPlayers.TabIndex = 1;
            // 
            // tabPageAbout
            // 
            this.tabPageAbout.Controls.Add(this.pictureBox1);
            this.tabPageAbout.Controls.Add(this.flowLayoutPanel3);
            this.tabPageAbout.Controls.Add(this.label40);
            this.tabPageAbout.Controls.Add(this.linkLabel7);
            this.tabPageAbout.Controls.Add(this.linkLabel6);
            this.tabPageAbout.Controls.Add(this.linkLabel4);
            this.tabPageAbout.Controls.Add(this.linkLabel3);
            this.tabPageAbout.Controls.Add(this.linkLabel2);
            this.tabPageAbout.Controls.Add(this.label16);
            this.tabPageAbout.Controls.Add(this.linkLabel1);
            this.tabPageAbout.Controls.Add(this.label15);
            this.tabPageAbout.Location = new System.Drawing.Point(4, 24);
            this.tabPageAbout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageAbout.Name = "tabPageAbout";
            this.tabPageAbout.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPageAbout.Size = new System.Drawing.Size(1047, 622);
            this.tabPageAbout.TabIndex = 5;
            this.tabPageAbout.Text = "About GeoChatter";
            this.tabPageAbout.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(19, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 40;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.linkLabel5);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel8);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel26);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel10);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel11);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel12);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel31);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel18);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel17);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel16);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel35);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel15);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel14);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel13);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel33);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel9);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel24);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel23);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel34);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel22);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel21);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel20);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel19);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel32);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel30);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel25);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel29);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel28);
            this.flowLayoutPanel3.Controls.Add(this.linkLabel27);
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(627, 181);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(417, 247);
            this.flowLayoutPanel3.TabIndex = 39;
            // 
            // linkLabel5
            // 
            this.linkLabel5.AutoSize = true;
            this.linkLabel5.Location = new System.Drawing.Point(3, 15);
            this.linkLabel5.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(83, 15);
            this.linkLabel5.TabIndex = 9;
            this.linkLabel5.TabStop = true;
            this.linkLabel5.Text = "AshSmashhhh";
            this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel8
            // 
            this.linkLabel8.AutoSize = true;
            this.linkLabel8.Location = new System.Drawing.Point(3, 45);
            this.linkLabel8.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel8.Name = "linkLabel8";
            this.linkLabel8.Size = new System.Drawing.Size(68, 15);
            this.linkLabel8.TabIndex = 10;
            this.linkLabel8.TabStop = true;
            this.linkLabel8.Text = "buncharted";
            this.linkLabel8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel26
            // 
            this.linkLabel26.AutoSize = true;
            this.linkLabel26.Location = new System.Drawing.Point(3, 75);
            this.linkLabel26.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel26.Name = "linkLabel26";
            this.linkLabel26.Size = new System.Drawing.Size(94, 15);
            this.linkLabel26.TabIndex = 38;
            this.linkLabel26.TabStop = true;
            this.linkLabel26.Text = "calebwinsgames";
            this.linkLabel26.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel10
            // 
            this.linkLabel10.AutoSize = true;
            this.linkLabel10.Location = new System.Drawing.Point(3, 105);
            this.linkLabel10.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel10.Name = "linkLabel10";
            this.linkLabel10.Size = new System.Drawing.Size(54, 15);
            this.linkLabel10.TabIndex = 12;
            this.linkLabel10.TabStop = true;
            this.linkLabel10.Text = "Credulus";
            this.linkLabel10.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel11
            // 
            this.linkLabel11.AutoSize = true;
            this.linkLabel11.Location = new System.Drawing.Point(3, 135);
            this.linkLabel11.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel11.Name = "linkLabel11";
            this.linkLabel11.Size = new System.Drawing.Size(64, 15);
            this.linkLabel11.TabIndex = 13;
            this.linkLabel11.TabStop = true;
            this.linkLabel11.Text = "DeltaLeeds";
            this.linkLabel11.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel12
            // 
            this.linkLabel12.AutoSize = true;
            this.linkLabel12.Location = new System.Drawing.Point(3, 165);
            this.linkLabel12.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel12.Name = "linkLabel12";
            this.linkLabel12.Size = new System.Drawing.Size(70, 15);
            this.linkLabel12.TabIndex = 14;
            this.linkLabel12.TabStop = true;
            this.linkLabel12.Text = "Dori_Explori";
            this.linkLabel12.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel31
            // 
            this.linkLabel31.AutoSize = true;
            this.linkLabel31.Location = new System.Drawing.Point(3, 195);
            this.linkLabel31.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel31.Name = "linkLabel31";
            this.linkLabel31.Size = new System.Drawing.Size(63, 15);
            this.linkLabel31.TabIndex = 39;
            this.linkLabel31.TabStop = true;
            this.linkLabel31.Text = "EveBonnet";
            // 
            // linkLabel18
            // 
            this.linkLabel18.AutoSize = true;
            this.linkLabel18.Location = new System.Drawing.Point(3, 225);
            this.linkLabel18.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel18.Name = "linkLabel18";
            this.linkLabel18.Size = new System.Drawing.Size(81, 15);
            this.linkLabel18.TabIndex = 15;
            this.linkLabel18.TabStop = true;
            this.linkLabel18.Text = "hikikomoriinu";
            this.linkLabel18.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel17
            // 
            this.linkLabel17.AutoSize = true;
            this.linkLabel17.Location = new System.Drawing.Point(103, 15);
            this.linkLabel17.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel17.Name = "linkLabel17";
            this.linkLabel17.Size = new System.Drawing.Size(52, 15);
            this.linkLabel17.TabIndex = 16;
            this.linkLabel17.TabStop = true;
            this.linkLabel17.Text = "Interjace";
            this.linkLabel17.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel16
            // 
            this.linkLabel16.AutoSize = true;
            this.linkLabel16.Location = new System.Drawing.Point(103, 45);
            this.linkLabel16.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel16.Name = "linkLabel16";
            this.linkLabel16.Size = new System.Drawing.Size(46, 15);
            this.linkLabel16.TabIndex = 17;
            this.linkLabel16.TabStop = true;
            this.linkLabel16.Text = "ItsJaqs_";
            this.linkLabel16.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel35
            // 
            this.linkLabel35.AutoSize = true;
            this.linkLabel35.Location = new System.Drawing.Point(103, 75);
            this.linkLabel35.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel35.Name = "linkLabel35";
            this.linkLabel35.Size = new System.Drawing.Size(72, 15);
            this.linkLabel35.TabIndex = 40;
            this.linkLabel35.TabStop = true;
            this.linkLabel35.Text = "KingJRagnar";
            // 
            // linkLabel15
            // 
            this.linkLabel15.AutoSize = true;
            this.linkLabel15.Location = new System.Drawing.Point(103, 105);
            this.linkLabel15.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel15.Name = "linkLabel15";
            this.linkLabel15.Size = new System.Drawing.Size(68, 15);
            this.linkLabel15.TabIndex = 18;
            this.linkLabel15.TabStop = true;
            this.linkLabel15.Text = "KingMoo92";
            this.linkLabel15.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel14
            // 
            this.linkLabel14.AutoSize = true;
            this.linkLabel14.Location = new System.Drawing.Point(103, 135);
            this.linkLabel14.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel14.Name = "linkLabel14";
            this.linkLabel14.Size = new System.Drawing.Size(51, 15);
            this.linkLabel14.TabIndex = 19;
            this.linkLabel14.TabStop = true;
            this.linkLabel14.Text = "Kizawski";
            this.linkLabel14.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel13
            // 
            this.linkLabel13.AutoSize = true;
            this.linkLabel13.Location = new System.Drawing.Point(103, 165);
            this.linkLabel13.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel13.Name = "linkLabel13";
            this.linkLabel13.Size = new System.Drawing.Size(53, 15);
            this.linkLabel13.TabIndex = 20;
            this.linkLabel13.TabStop = true;
            this.linkLabel13.Text = "LeBjoern";
            this.linkLabel13.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel33
            // 
            this.linkLabel33.AutoSize = true;
            this.linkLabel33.Location = new System.Drawing.Point(103, 195);
            this.linkLabel33.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel33.Name = "linkLabel33";
            this.linkLabel33.Size = new System.Drawing.Size(61, 15);
            this.linkLabel33.TabIndex = 34;
            this.linkLabel33.TabStop = true;
            this.linkLabel33.Text = "LeftyROFL";
            this.linkLabel33.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel9
            // 
            this.linkLabel9.AutoSize = true;
            this.linkLabel9.Location = new System.Drawing.Point(103, 225);
            this.linkLabel9.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel9.Name = "linkLabel9";
            this.linkLabel9.Size = new System.Drawing.Size(78, 15);
            this.linkLabel9.TabIndex = 36;
            this.linkLabel9.TabStop = true;
            this.linkLabel9.Text = "LookItsMikeo";
            this.linkLabel9.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel24
            // 
            this.linkLabel24.AutoSize = true;
            this.linkLabel24.Location = new System.Drawing.Point(187, 15);
            this.linkLabel24.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel24.Name = "linkLabel24";
            this.linkLabel24.Size = new System.Drawing.Size(84, 15);
            this.linkLabel24.TabIndex = 21;
            this.linkLabel24.TabStop = true;
            this.linkLabel24.Text = "Meow_Tomcat";
            this.linkLabel24.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel23
            // 
            this.linkLabel23.AutoSize = true;
            this.linkLabel23.Location = new System.Drawing.Point(187, 45);
            this.linkLabel23.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel23.Name = "linkLabel23";
            this.linkLabel23.Size = new System.Drawing.Size(76, 15);
            this.linkLabel23.TabIndex = 22;
            this.linkLabel23.TabStop = true;
            this.linkLabel23.Text = "NateExplores";
            this.linkLabel23.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel34
            // 
            this.linkLabel34.AutoSize = true;
            this.linkLabel34.Location = new System.Drawing.Point(187, 75);
            this.linkLabel34.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel34.Name = "linkLabel34";
            this.linkLabel34.Size = new System.Drawing.Size(84, 15);
            this.linkLabel34.TabIndex = 33;
            this.linkLabel34.TabStop = true;
            this.linkLabel34.Text = "Not_A_Gopher";
            this.linkLabel34.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel22
            // 
            this.linkLabel22.AutoSize = true;
            this.linkLabel22.Location = new System.Drawing.Point(187, 105);
            this.linkLabel22.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel22.Name = "linkLabel22";
            this.linkLabel22.Size = new System.Drawing.Size(51, 15);
            this.linkLabel22.TabIndex = 23;
            this.linkLabel22.TabStop = true;
            this.linkLabel22.Text = "Nuujaka";
            this.linkLabel22.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel21
            // 
            this.linkLabel21.AutoSize = true;
            this.linkLabel21.Location = new System.Drawing.Point(187, 135);
            this.linkLabel21.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel21.Name = "linkLabel21";
            this.linkLabel21.Size = new System.Drawing.Size(103, 15);
            this.linkLabel21.TabIndex = 24;
            this.linkLabel21.TabStop = true;
            this.linkLabel21.Text = "oneProudPenguin";
            this.linkLabel21.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel20
            // 
            this.linkLabel20.AutoSize = true;
            this.linkLabel20.Location = new System.Drawing.Point(187, 165);
            this.linkLabel20.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel20.Name = "linkLabel20";
            this.linkLabel20.Size = new System.Drawing.Size(40, 15);
            this.linkLabel20.TabIndex = 25;
            this.linkLabel20.TabStop = true;
            this.linkLabel20.Text = "Petery";
            this.linkLabel20.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel19
            // 
            this.linkLabel19.AutoSize = true;
            this.linkLabel19.Location = new System.Drawing.Point(187, 195);
            this.linkLabel19.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel19.Name = "linkLabel19";
            this.linkLabel19.Size = new System.Drawing.Size(95, 15);
            this.linkLabel19.TabIndex = 26;
            this.linkLabel19.TabStop = true;
            this.linkLabel19.Text = "QuantumGravity";
            this.linkLabel19.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel32
            // 
            this.linkLabel32.AutoSize = true;
            this.linkLabel32.Location = new System.Drawing.Point(187, 225);
            this.linkLabel32.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel32.Name = "linkLabel32";
            this.linkLabel32.Size = new System.Drawing.Size(110, 15);
            this.linkLabel32.TabIndex = 35;
            this.linkLabel32.TabStop = true;
            this.linkLabel32.Text = "Riley_or_something";
            this.linkLabel32.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel30
            // 
            this.linkLabel30.AutoSize = true;
            this.linkLabel30.Location = new System.Drawing.Point(303, 15);
            this.linkLabel30.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel30.Name = "linkLabel30";
            this.linkLabel30.Size = new System.Drawing.Size(70, 15);
            this.linkLabel30.TabIndex = 27;
            this.linkLabel30.TabStop = true;
            this.linkLabel30.Text = "RivereedsYT";
            this.linkLabel30.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel25
            // 
            this.linkLabel25.AutoSize = true;
            this.linkLabel25.Location = new System.Drawing.Point(303, 45);
            this.linkLabel25.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel25.Name = "linkLabel25";
            this.linkLabel25.Size = new System.Drawing.Size(56, 15);
            this.linkLabel25.TabIndex = 37;
            this.linkLabel25.TabStop = true;
            this.linkLabel25.Text = "ryves_mc";
            this.linkLabel25.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel29
            // 
            this.linkLabel29.AutoSize = true;
            this.linkLabel29.Location = new System.Drawing.Point(303, 75);
            this.linkLabel29.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel29.Name = "linkLabel29";
            this.linkLabel29.Size = new System.Drawing.Size(51, 15);
            this.linkLabel29.TabIndex = 28;
            this.linkLabel29.TabStop = true;
            this.linkLabel29.Text = "Saynada";
            this.linkLabel29.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel28
            // 
            this.linkLabel28.AutoSize = true;
            this.linkLabel28.Location = new System.Drawing.Point(303, 105);
            this.linkLabel28.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel28.Name = "linkLabel28";
            this.linkLabel28.Size = new System.Drawing.Size(87, 15);
            this.linkLabel28.TabIndex = 29;
            this.linkLabel28.TabStop = true;
            this.linkLabel28.Text = "WilkoWilkins17";
            this.linkLabel28.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // linkLabel27
            // 
            this.linkLabel27.AutoSize = true;
            this.linkLabel27.Location = new System.Drawing.Point(303, 135);
            this.linkLabel27.Margin = new System.Windows.Forms.Padding(3, 15, 3, 0);
            this.linkLabel27.Name = "linkLabel27";
            this.linkLabel27.Size = new System.Drawing.Size(41, 15);
            this.linkLabel27.TabIndex = 30;
            this.linkLabel27.TabStop = true;
            this.linkLabel27.Text = "xal1na";
            this.linkLabel27.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.betaTestersLink_Clicked);
            // 
            // label40
            // 
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label40.Location = new System.Drawing.Point(630, 95);
            this.label40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(409, 86);
            this.label40.TabIndex = 8;
            this.label40.Text = "We could never have developed GeoChatter without an amazing group of Beta testers" +
    "!\r\n\r\nThank you all so much for your invaluable help!";
            // 
            // linkLabel7
            // 
            this.linkLabel7.AutoSize = true;
            this.linkLabel7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linkLabel7.Location = new System.Drawing.Point(167, 408);
            this.linkLabel7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel7.Name = "linkLabel7";
            this.linkLabel7.Size = new System.Drawing.Size(145, 20);
            this.linkLabel7.TabIndex = 7;
            this.linkLabel7.TabStop = true;
            this.linkLabel7.Text = "https://geobingo.io";
            this.linkLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel6
            // 
            this.linkLabel6.AutoSize = true;
            this.linkLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linkLabel6.Location = new System.Drawing.Point(126, 141);
            this.linkLabel6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel6.Name = "linkLabel6";
            this.linkLabel6.Size = new System.Drawing.Size(194, 29);
            this.linkLabel6.TabIndex = 6;
            this.linkLabel6.TabStop = true;
            this.linkLabel6.Text = "Support Discord";
            this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel6_LinkClicked);
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linkLabel4.Location = new System.Drawing.Point(140, 346);
            this.linkLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(212, 20);
            this.linkLabel4.TabIndex = 5;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "geochatter.app@gmail.com";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linkLabel3.Location = new System.Drawing.Point(279, 329);
            this.linkLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(262, 20);
            this.linkLabel3.TabIndex = 4;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "https://twitch.tv/NoBuddyIsPerfect";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linkLabel2.Location = new System.Drawing.Point(214, 309);
            this.linkLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(211, 20);
            this.linkLabel2.TabIndex = 3;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "https://github.com/semihM/";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label16.Location = new System.Drawing.Point(57, 268);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(496, 180);
            this.label16.TabIndex = 2;
            this.label16.Text = resources.GetString("label16.Text");
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.linkLabel1.Location = new System.Drawing.Point(126, 103);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(235, 29);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://geochatter.tv";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label15.Location = new System.Drawing.Point(126, 45);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(454, 58);
            this.label15.TabIndex = 0;
            this.label15.Text = "GeoChatter\r\nA solution to play GeoGuessr with Chat";
            // 
            // tabDevelopment
            // 
            this.tabDevelopment.Controls.Add(this.chkDEVEnableRandomBotGuess);
            this.tabDevelopment.Controls.Add(this.chkDEVShowDevTools);
            this.tabDevelopment.Controls.Add(this.chkDEVUseDevApi);
            this.tabDevelopment.Location = new System.Drawing.Point(4, 24);
            this.tabDevelopment.Name = "tabDevelopment";
            this.tabDevelopment.Padding = new System.Windows.Forms.Padding(3);
            this.tabDevelopment.Size = new System.Drawing.Size(1047, 622);
            this.tabDevelopment.TabIndex = 9;
            this.tabDevelopment.Text = "DEV SETTINGS";
            this.tabDevelopment.UseVisualStyleBackColor = true;
            // 
            // chkDEVEnableRandomBotGuess
            // 
            this.chkDEVEnableRandomBotGuess.AutoSize = true;
            this.chkDEVEnableRandomBotGuess.Location = new System.Drawing.Point(60, 98);
            this.chkDEVEnableRandomBotGuess.Name = "chkDEVEnableRandomBotGuess";
            this.chkDEVEnableRandomBotGuess.Size = new System.Drawing.Size(158, 19);
            this.chkDEVEnableRandomBotGuess.TabIndex = 2;
            this.chkDEVEnableRandomBotGuess.Text = "Enable !randomBotGuess";
            this.chkDEVEnableRandomBotGuess.UseVisualStyleBackColor = true;
            // 
            // chkDEVShowDevTools
            // 
            this.chkDEVShowDevTools.AutoSize = true;
            this.chkDEVShowDevTools.Location = new System.Drawing.Point(60, 73);
            this.chkDEVShowDevTools.Name = "chkDEVShowDevTools";
            this.chkDEVShowDevTools.Size = new System.Drawing.Size(106, 19);
            this.chkDEVShowDevTools.TabIndex = 1;
            this.chkDEVShowDevTools.Text = "Show dev tools";
            this.chkDEVShowDevTools.UseVisualStyleBackColor = true;
            // 
            // chkDEVUseDevApi
            // 
            this.chkDEVUseDevApi.AutoSize = true;
            this.chkDEVUseDevApi.Location = new System.Drawing.Point(60, 48);
            this.chkDEVUseDevApi.Name = "chkDEVUseDevApi";
            this.chkDEVUseDevApi.Size = new System.Drawing.Size(86, 19);
            this.chkDEVUseDevApi.TabIndex = 0;
            this.chkDEVUseDevApi.Text = "Use dev api";
            this.chkDEVUseDevApi.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(4, 703);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(523, 37);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Location = new System.Drawing.Point(534, 659);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(526, 38);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnShowAdvanced
            // 
            this.btnShowAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowAdvanced.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnShowAdvanced.ForeColor = System.Drawing.Color.Red;
            this.btnShowAdvanced.Location = new System.Drawing.Point(3, 659);
            this.btnShowAdvanced.Name = "btnShowAdvanced";
            this.btnShowAdvanced.Size = new System.Drawing.Size(525, 38);
            this.btnShowAdvanced.TabIndex = 4;
            this.btnShowAdvanced.Text = "Show advanced settings";
            this.btnShowAdvanced.UseVisualStyleBackColor = true;
            this.btnShowAdvanced.Click += new System.EventHandler(this.btnShowAdvanced_Click);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.Color = System.Drawing.Color.White;
            this.colorDialog1.FullOpen = true;
            this.colorDialog1.ShowHelp = true;
            // 
            // colorDialog2
            // 
            this.colorDialog2.AnyColor = true;
            this.colorDialog2.FullOpen = true;
            this.colorDialog2.ShowHelp = true;
            // 
            // usernameColorDialog
            // 
            this.usernameColorDialog.AnyColor = true;
            this.usernameColorDialog.FullOpen = true;
            this.usernameColorDialog.SolidColorOnly = true;
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1063, 743);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsDialog_FormClosing);
            this.Load += new System.EventHandler(this.SettingsDialog_Load);
            this.Enter += new System.EventHandler(this.SettingsDialog_Enter);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpViewermap.ResumeLayout(false);
            this.grpViewermap.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupGameSettings.ResumeLayout(false);
            this.groupGameSettings.PerformLayout();
            this.tabTwitch.ResumeLayout(false);
            this.groupBoxChatMessages.ResumeLayout(false);
            this.groupBoxChatMessages.PerformLayout();
            this.grpTwitchChatConnection.ResumeLayout(false);
            this.grpTwitchChatConnection.PerformLayout();
            this.tabPageConnections.ResumeLayout(false);
            this.groupBoxOtherChatGateways.ResumeLayout(false);
            this.groupBoxOtherChatGateways.PerformLayout();
            this.grpObsConnection.ResumeLayout(false);
            this.grpObsConnection.PerformLayout();
            this.grpStreamerBotConnection.ResumeLayout(false);
            this.grpStreamerBotConnection.PerformLayout();
            this.tabChatMessages.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tabPageEvents.ResumeLayout(false);
            this.groupBoxGameEnd.ResumeLayout(false);
            this.groupBoxGameEnd.PerformLayout();
            this.groupBoxRoundEnd.ResumeLayout(false);
            this.groupBoxRoundEnd.PerformLayout();
            this.groupBoxRoundTimer.ResumeLayout(false);
            this.groupBoxRoundTimer.PerformLayout();
            this.groupBoxEventDistance.ResumeLayout(false);
            this.groupBoxEventDistance.PerformLayout();
            this.groupBoxEventSpecial.ResumeLayout(false);
            this.groupBoxEventSpecial.PerformLayout();
            this.tabPageLabels.ResumeLayout(false);
            this.groupBoxEventGeneral.ResumeLayout(false);
            this.groupBoxEventGeneral.PerformLayout();
            this.groupBoxEventGameEnd.ResumeLayout(false);
            this.groupBoxEventGameEnd.PerformLayout();
            this.groupBoxEventRoundEnd.ResumeLayout(false);
            this.groupBoxEventRoundEnd.PerformLayout();
            this.tabPageOverlay.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.groupScoreboard.ResumeLayout(false);
            this.flowScoreboard.ResumeLayout(false);
            this.panelScoreBoardUnit.ResumeLayout(false);
            this.panelScoreBoardUnit.PerformLayout();
            this.panelScoreboardRounding.ResumeLayout(false);
            this.panelScoreboardRounding.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericRoundingDigit)).EndInit();
            this.panelScoreboardFontSize.ResumeLayout(false);
            this.panelScoreboardFontSize.PerformLayout();
            this.panelScoreBoardColors.ResumeLayout(false);
            this.panelScoreBoardColors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScoreboardFGAnumericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScoreboardBGAnumericUpDown2)).EndInit();
            this.panelScoreBoardSpeed.ResumeLayout(false);
            this.panelScoreBoardSpeed.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScoreboardScrollSpeednumericUpDown1)).EndInit();
            this.panelScoreboardCheckboxes.ResumeLayout(false);
            this.panelScoreboardCheckboxes.PerformLayout();
            this.groupMarker.ResumeLayout(false);
            this.groupMarker.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxGuessDisplayCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxMarkerCount)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabUsers.ResumeLayout(false);
            this.tabUsers.PerformLayout();
            this.tabPageAbout.ResumeLayout(false);
            this.tabPageAbout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.tabDevelopment.ResumeLayout(false);
            this.tabDevelopment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabTwitch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupGameSettings;
        private System.Windows.Forms.TabPage tabPageEvents;
        private System.Windows.Forms.GroupBox groupBoxEventSpecial;
        private System.Windows.Forms.TextBox txtSpecialDistanceFrom;
        private System.Windows.Forms.TextBox txtSpecialScoreFrom;
        private System.Windows.Forms.CheckBox checkBoxSpecialDistanceLabel;
        private System.Windows.Forms.CheckBox checkBoxSpecialScoreLabel;
        private System.Windows.Forms.TabPage tabPageOverlay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxOverlayUnits;
        private System.Windows.Forms.GroupBox groupBoxEventDistance;
        private System.Windows.Forms.CheckBox checkBoxSpecialDistanceAction;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBoxSpecialScoreAction;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPageConnections;
        private System.Windows.Forms.GroupBox grpStreamerBotConnection;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtStreamerBotPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStreamerBotIP;
        private System.Windows.Forms.Button buttonConnectStreamerBot;
        private System.Windows.Forms.CheckBox chkStreamerBotConnectAtStartup;
        private System.Windows.Forms.CheckBox checkBoxOverlayDisplayCorrectLocations;
        private System.Windows.Forms.CheckBox checkBoxOverlayUseWrongRegionColors;
        private System.Windows.Forms.CheckBox checkBoxOverlayRegionalFlags;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numericRoundingDigit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem newColumnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteColumnToolStripMenuItem;
        private System.Windows.Forms.GroupBox grpObsConnection;
        private System.Windows.Forms.CheckBox chkObsConnectAtStartup;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtObsPort;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtObsIp;
        private System.Windows.Forms.Button btnConnectObs;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtObsPassword;
        private System.Windows.Forms.ComboBox comboSpecialScoreObsAction;
        private System.Windows.Forms.ComboBox comboSpecialDistanceObsAction;
        private System.Windows.Forms.ComboBox comboSpecialDistanceObsSource;
        private System.Windows.Forms.CheckBox chkSpecialDistanceObs;
        private System.Windows.Forms.ComboBox comboSpecialScoreObsSource;
        private System.Windows.Forms.CheckBox chkSpecialScoreObs;
        private System.Windows.Forms.TabPage tabPageAbout;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox comboSpecialScoreObsScene;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox comboSpecialDistanceObsScene;
        private System.Windows.Forms.LinkLabel linkLabel6;
        private System.Windows.Forms.GroupBox groupBoxRoundTimer;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox comboRoundTimerObsSource;
        private System.Windows.Forms.ComboBox comboRoundTimerObsAction;
        private System.Windows.Forms.ComboBox comboRoundTimerObsScene;
        private System.Windows.Forms.CheckBox chkRoundTimerOBS;
        private System.Windows.Forms.CheckBox checkBoxRoundTimerExecuteStreamerBotAction;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdate;
        private System.Windows.Forms.TabPage tabPageLabels;
        private System.Windows.Forms.GroupBox groupBoxEventGeneral;
        private System.Windows.Forms.Button btnSelectLabelFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxLabelPath;
        private System.Windows.Forms.GroupBox groupBoxEventGameEnd;
        private System.Windows.Forms.CheckBox checkBoxEventWriteGameHighscore;
        private System.Windows.Forms.CheckBox checkBoxEventWriteGameThird;
        private System.Windows.Forms.CheckBox checkBoxEventWriteGameSecond;
        private System.Windows.Forms.CheckBox checkBoxEventWriteGameWinner;
        private System.Windows.Forms.GroupBox groupBoxEventRoundEnd;
        private System.Windows.Forms.CheckBox checkBoxEventWriteRoundHighscore;
        private System.Windows.Forms.CheckBox checkBoxEventWriteRoundThird;
        private System.Windows.Forms.CheckBox checkBoxEventWriteRoundSecond;
        private System.Windows.Forms.CheckBox checkBoxEventWriteRoundWinner;
        private System.Windows.Forms.GroupBox groupBoxRoundEnd;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox comboOBSRoundEndSource;
        private System.Windows.Forms.ComboBox comboOBSRoundEndAction;
        private System.Windows.Forms.ComboBox comboOBSRoundEndScene;
        private System.Windows.Forms.CheckBox chkOBSRoundEndExecute;
        private System.Windows.Forms.CheckBox chkBotRoundEndExecute;
        private System.Windows.Forms.GroupBox groupBoxGameEnd;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox comboOBSGameEndSource;
        private System.Windows.Forms.ComboBox comboOBSGameEndAction;
        private System.Windows.Forms.ComboBox comboOBSGameEndScene;
        private System.Windows.Forms.CheckBox chkOBSGameEndExecute;
        private System.Windows.Forms.CheckBox chkBotGameEndExecute;
        private System.Windows.Forms.CheckBox chkResetStreakOnSkippedRound;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkUseEnglishCountryNames;
        private System.Windows.Forms.ComboBox OverlayFontSizeUnitcomboBox1;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox OverlayFontSizetextBox1;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Button ScoreboardBGDisplaybutton;
        private System.Windows.Forms.Button ScoreboardFGDisplaybutton;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.NumericUpDown ScoreboardBGAnumericUpDown2;
        private System.Windows.Forms.NumericUpDown ScoreboardFGAnumericUpDown2;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.NumericUpDown ScoreboardScrollSpeednumericUpDown1;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.LinkLabel linkLabel7;
        private System.Windows.Forms.CheckBox chkAllowSameLocationGuess;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.CheckedListBox chkListBoxBannedPlayers;
        private System.Windows.Forms.Label label35;
        private Controls.ShortcutRecorderControl scrSettings;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private Controls.ShortcutRecorderControl scrMenu;
        private Controls.ShortcutRecorderControl scrFullscreen;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox chkSpecialScoreRange;
        private System.Windows.Forms.TextBox txtSpecialScoreTo;
        private System.Windows.Forms.CheckBox chkSpecialDistanceRange;
        private System.Windows.Forms.TextBox txtSpecialDistanceTo;
        private System.Windows.Forms.CheckBox chkAutoBan;
        private System.Windows.Forms.CheckBox chkUseUsStateFlags;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserLabelPath;
        private System.Windows.Forms.GroupBox groupScoreboard;
        private System.Windows.Forms.GroupBox groupMarker;
        private System.Windows.Forms.CheckBox chkGuessInfoTime;
        private System.Windows.Forms.CheckBox chkGuessInfoCoordinates;
        private System.Windows.Forms.CheckBox chkGuessInfoDistance;
        private System.Windows.Forms.CheckBox chkGuessInfoScore;
        private System.Windows.Forms.CheckBox chkGuessInfoStreak;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.CheckBox chkAutoBanCGTrolls;
        private System.Windows.Forms.CheckBox chkEnableMapOverlay;
        private System.Windows.Forms.CheckBox chkCheckStreamer;
        private System.Windows.Forms.Button btnShowAdvanced;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowScoreboard;
        private System.Windows.Forms.Panel panelScoreBoardUnit;
        private System.Windows.Forms.Panel panelScoreboardRounding;
        private System.Windows.Forms.Panel panelScoreboardFontSize;
        private System.Windows.Forms.Panel panelScoreBoardColors;
        private System.Windows.Forms.Panel panelScoreBoardSpeed;
        private System.Windows.Forms.Panel panelScoreboardCheckboxes;
        private System.Windows.Forms.CheckBox chkEnableDebugLogging;
        private System.Windows.Forms.GroupBox grpViewermap;
        private System.Windows.Forms.CheckBox chkShowBorders;
        private System.Windows.Forms.CheckBox chkShowFlags;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxMarkerClustersEnabled;
        private System.Windows.Forms.CheckBox checkBoxEnableTempGuesses;
        private System.Windows.Forms.Label lblMaxGuessDisplayCount;
        private System.Windows.Forms.NumericUpDown numMaxGuessDisplayCount;
        private System.Windows.Forms.Label lblNoOfMarkers;
        private System.Windows.Forms.NumericUpDown numMaxMarkerCount;
        private System.Windows.Forms.CheckBox checkBoxEnableChatMsgs;
        private System.Windows.Forms.GroupBox groupBoxChatMessages;
        private System.Windows.Forms.GroupBox grpTwitchChatConnection;
        private System.Windows.Forms.Button btnForgetTwitch;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button btnAuthorizeManually;
        private System.Windows.Forms.Button btnAuthorizeAutomatically;
        private System.Windows.Forms.Button reconnectTwitchBotButton;
        private System.Windows.Forms.TextBox txtGeneralOauthToken;
        private System.Windows.Forms.TextBox txtGeneralChannelName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStreamerDisplayname;
        private System.Windows.Forms.LinkLabel linkLabel32;
        private System.Windows.Forms.LinkLabel linkLabel33;
        private System.Windows.Forms.LinkLabel linkLabel34;
        private System.Windows.Forms.LinkLabel linkLabel27;
        private System.Windows.Forms.LinkLabel linkLabel28;
        private System.Windows.Forms.LinkLabel linkLabel29;
        private System.Windows.Forms.LinkLabel linkLabel30;
        private System.Windows.Forms.LinkLabel linkLabel19;
        private System.Windows.Forms.LinkLabel linkLabel20;
        private System.Windows.Forms.LinkLabel linkLabel21;
        private System.Windows.Forms.LinkLabel linkLabel22;
        private System.Windows.Forms.LinkLabel linkLabel23;
        private System.Windows.Forms.LinkLabel linkLabel24;
        private System.Windows.Forms.LinkLabel linkLabel13;
        private System.Windows.Forms.LinkLabel linkLabel14;
        private System.Windows.Forms.LinkLabel linkLabel15;
        private System.Windows.Forms.LinkLabel linkLabel16;
        private System.Windows.Forms.LinkLabel linkLabel17;
        private System.Windows.Forms.LinkLabel linkLabel18;
        private System.Windows.Forms.LinkLabel linkLabel12;
        private System.Windows.Forms.LinkLabel linkLabel11;
        private System.Windows.Forms.LinkLabel linkLabel10;
        private System.Windows.Forms.LinkLabel linkLabel8;
        private System.Windows.Forms.LinkLabel linkLabel5;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.LinkLabel linkLabel26;
        private System.Windows.Forms.LinkLabel linkLabel25;
        private System.Windows.Forms.LinkLabel linkLabel9;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.LinkLabel linkLabel31;
        private System.Windows.Forms.LinkLabel linkLabel35;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Button streamerColor;
        private System.Windows.Forms.ComboBox streamerFlag;
        private System.Windows.Forms.ColorDialog usernameColorDialog;
        private System.Windows.Forms.Button randomFlag;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Button streamerFlagDisplay;
        private System.Windows.Forms.GroupBox groupBoxOtherChatGateways;
        private System.Windows.Forms.CheckBox checkBoxSendChatMsgsViaStreamerBot;
        private System.Windows.Forms.TabPage tabChatMessages;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox chkMsgsJoinChannel;
        private System.Windows.Forms.CheckBox chkMsgsStartGame;
        private System.Windows.Forms.CheckBox chkMsgsRoundStarted;
        private System.Windows.Forms.CheckBox chkMsgsGuessReceived;
        private System.Windows.Forms.CheckBox chkSendDoubleGuessMsg;
        private System.Windows.Forms.CheckBox chkSendSameGuessMessage;
        private System.Windows.Forms.CheckBox chkMsgsRoundEnd;
        private System.Windows.Forms.CheckBox chkMsgsGameEnded;
        private System.Windows.Forms.CheckBox chkColorMessage;
        private System.Windows.Forms.CheckBox chkFlagMessages;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TabPage tabDevelopment;
        private System.Windows.Forms.CheckBox chkDEVUseDevApi;
        private Controls.StreamerBotActionControl ctrlBotGameEnd;
        private Controls.StreamerBotActionControl ctrlBotRoundEnd;
        private Controls.StreamerBotActionControl ctrlBotRoundStart;
        private Controls.StreamerBotActionControl ctrlBotSpecialDistance;
        private Controls.StreamerBotActionControl ctrlBotSpecialScore;
        private Controls.StreamerBotActionControl ctrlBotChatMessages;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox chkDEVEnableRandomBotGuess;
        private System.Windows.Forms.CheckBox chkDEVShowDevTools;
        private System.Windows.Forms.CheckBox allowCustomRandomGuesses;
    }
}

#pragma warning restore CS1591, CA1062