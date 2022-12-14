#pragma warning disable CS1591,CA1062


namespace GeoChatter.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.geochatterLogoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUploadLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemResetZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.userScriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flagManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commandManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scoreFormulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IntegrationstoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResetApitoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetStreamerBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geochatterLogoToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.browserToolStripMenuItem,
            this.userScriptsToolStripMenuItem,
            this.flagManagerToolStripMenuItem,
            this.commandManagerToolStripMenuItem,
            this.scoreFormulatorToolStripMenuItem,
            this.IntegrationstoolStripMenuItem,
            this.devToolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(933, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // geochatterLogoToolStripMenuItem
            // 
            this.geochatterLogoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("geochatterLogoToolStripMenuItem.Image")));
            this.geochatterLogoToolStripMenuItem.Name = "geochatterLogoToolStripMenuItem";
            this.geochatterLogoToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.geochatterLogoToolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingToolStripMenuItem,
            this.fullscreenToolStripMenuItem,
            this.toolStripMenuItemUploadLogs,
            this.checkForUpdateToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.settingToolStripMenuItem.Text = "&Settings";
            this.settingToolStripMenuItem.Click += new System.EventHandler(this.settingToolStripMenuItem_Click);
            // 
            // fullscreenToolStripMenuItem
            // 
            this.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem";
            this.fullscreenToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.fullscreenToolStripMenuItem.Text = "&Fullscreen";
            this.fullscreenToolStripMenuItem.Click += new System.EventHandler(this.fullscreenToolStripMenuItem_Click);
            // 
            // toolStripMenuItemUploadLogs
            // 
            this.toolStripMenuItemUploadLogs.Name = "toolStripMenuItemUploadLogs";
            this.toolStripMenuItemUploadLogs.Size = new System.Drawing.Size(165, 22);
            this.toolStripMenuItemUploadLogs.Text = "Upload Logs";
            this.toolStripMenuItemUploadLogs.Click += new System.EventHandler(this.ToolStripMenuItemUploadLogs_Click);
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.checkForUpdateToolStripMenuItem.Text = "&Check for update";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdateToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // browserToolStripMenuItem
            // 
            this.browserToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemRefresh,
            this.toolStripMenuItemResetZoom});
            this.browserToolStripMenuItem.Name = "browserToolStripMenuItem";
            this.browserToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
            this.browserToolStripMenuItem.Text = "&Browser actions";
            // 
            // toolStripMenuItemRefresh
            // 
            this.toolStripMenuItemRefresh.Name = "toolStripMenuItemRefresh";
            this.toolStripMenuItemRefresh.Size = new System.Drawing.Size(137, 22);
            this.toolStripMenuItemRefresh.Text = "&Refresh";
            this.toolStripMenuItemRefresh.Click += new System.EventHandler(this.toolStripMenuItemRefresh_Click);
            // 
            // toolStripMenuItemResetZoom
            // 
            this.toolStripMenuItemResetZoom.Name = "toolStripMenuItemResetZoom";
            this.toolStripMenuItemResetZoom.Size = new System.Drawing.Size(137, 22);
            this.toolStripMenuItemResetZoom.Text = "Reset &Zoom";
            this.toolStripMenuItemResetZoom.Visible = false;
            this.toolStripMenuItemResetZoom.Click += new System.EventHandler(this.toolStripMenuItemResetZoom_Click);
            // 
            // userScriptsToolStripMenuItem
            // 
            this.userScriptsToolStripMenuItem.Name = "userScriptsToolStripMenuItem";
            this.userScriptsToolStripMenuItem.Size = new System.Drawing.Size(131, 20);
            this.userScriptsToolStripMenuItem.Text = "&UserScript Manager...";
            this.userScriptsToolStripMenuItem.Click += new System.EventHandler(this.extensionsToolStripMenuItem_Click);
            // 
            // flagManagerToolStripMenuItem
            // 
            this.flagManagerToolStripMenuItem.Name = "flagManagerToolStripMenuItem";
            this.flagManagerToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.flagManagerToolStripMenuItem.Text = "Flag Manager...";
            this.flagManagerToolStripMenuItem.Click += new System.EventHandler(this.flagManagerToolStripMenuItem_Click);
            // 
            // commandManagerToolStripMenuItem
            // 
            this.commandManagerToolStripMenuItem.Name = "commandManagerToolStripMenuItem";
            this.commandManagerToolStripMenuItem.Size = new System.Drawing.Size(135, 20);
            this.commandManagerToolStripMenuItem.Text = "&Command Manager...";
            this.commandManagerToolStripMenuItem.Click += new System.EventHandler(this.commandManagerMenuItem_Click);
            // 
            // scoreFormulatorToolStripMenuItem
            // 
            this.scoreFormulatorToolStripMenuItem.Name = "scoreFormulatorToolStripMenuItem";
            this.scoreFormulatorToolStripMenuItem.Size = new System.Drawing.Size(119, 20);
            this.scoreFormulatorToolStripMenuItem.Text = "Score Formulator...";
            this.scoreFormulatorToolStripMenuItem.Click += new System.EventHandler(this.scoreFormulatorMenuItem_Click);
            // 
            // devToolsToolStripMenuItem
            // 
            this.devToolsToolStripMenuItem.Name = "devToolsToolStripMenuItem";
            this.devToolsToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.devToolsToolStripMenuItem.Text = "DevTools...";
            this.devToolsToolStripMenuItem.Visible = false;
            this.devToolsToolStripMenuItem.Click += new System.EventHandler(this.devToolsToolStripMenuItem_Click);
            // 
            // IntegrationstoolStripMenuItem
            // 
            this.IntegrationstoolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResetApitoolStripMenuItem,
            this.resetStreamerBotToolStripMenuItem});
            this.IntegrationstoolStripMenuItem.Name = "IntegrationstoolStripMenuItem";
            this.IntegrationstoolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.IntegrationstoolStripMenuItem.Text = "Integrations";
            // 
            // ResetApitoolStripMenuItem
            // 
            this.ResetApitoolStripMenuItem.Name = "ResetApitoolStripMenuItem";
            this.ResetApitoolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.ResetApitoolStripMenuItem.Text = "Reset GuessApi connection";
            this.ResetApitoolStripMenuItem.Click += new System.EventHandler(this.ResetApitoolStripMenuItem_Click);
            // 
            // resetStreamerBotToolStripMenuItem
            // 
            this.resetStreamerBotToolStripMenuItem.Name = "resetStreamerBotToolStripMenuItem";
            this.resetStreamerBotToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.resetStreamerBotToolStripMenuItem.Text = "Reset Streamer.Bot connection";
            this.resetStreamerBotToolStripMenuItem.Click += new System.EventHandler(this.resetStreamerBotToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(933, 495);
            this.panel1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "GeoChatter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.MainForm_Deactivate);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userScriptsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem geochatterLogoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem devToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flagManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUploadLogs;
        private System.Windows.Forms.ToolStripMenuItem commandManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scoreFormulatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRefresh;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemResetZoom;
        private System.Windows.Forms.ToolStripMenuItem IntegrationstoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResetApitoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetStreamerBotToolStripMenuItem;
    }
}



#pragma warning restore CS1591, CA1062