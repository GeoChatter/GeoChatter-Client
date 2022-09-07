using GeoChatter.Extensions;
using GeoChatter.Helpers;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Model.Attributes;
using GeoChatter.Forms.FlagManager;
using GeoChatter.FormUtils;
using GeoChatter.Properties;
using System;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeoChatter.Model;

namespace GeoChatter.Forms.SetupWizard
{
    /// <summary>
    /// Settings wizard
    /// </summary> 
    [SupportedOSPlatform("windows7.0")]

    public partial class SetupWizard : Form
    {
        private MainForm parent;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public SetupWizard(MainForm parent)
        {
            this.parent = parent;

            InitializeComponent();
            pageConnections.Commit += PageConnections_Commit;
            pageChat.Commit += PageChat_Commit;
            pageGame.Commit += PageGame_Commit;
            pageMarkers.Commit += PageMarkers_Commit;

        }

        private void PageMarkers_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            Settings.Default.OverlayInfoPopupShowStreak = chkGuessInfoStreak.Checked;
            Settings.Default.OverlayInfoPopupShowCoordinates = chkGuessInfoCoordinates.Checked;
            Settings.Default.OverlayInfoPopupShowDistance = chkGuessInfoDistance.Checked;
            Settings.Default.OverlayInfoPopupShowScore = chkGuessInfoScore.Checked;
            Settings.Default.OverlayInfoPopupShowTime = chkGuessInfoTime.Checked;
            Settings.Default.Save();
        }

        private void PageGame_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            Settings.Default.ResetStreakOnSkippedRound = chkResetStreakOnSkippedRound.Checked;
            Settings.Default.UseEnglishCountryNames = chkUseEnglishCountryNames.Checked;
            Settings.Default.MOEnableOverlay = chkEnableMapOverlay.Checked;

            Settings.Default.AllowSameLocationGuess = chkAllowSameLocationGuess.Checked;
            Settings.Default.Save();
        }

        private void PageChat_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            Settings.Default.SendGameEndedMsg = chkMsgsGameEnded.Checked;
            Settings.Default.SendConfirmGuessMsg = chkMsgsGuessReceived.Checked;
            Settings.Default.SendJoinMsg = chkMsgsJoinChannel.Checked;
            Settings.Default.SendRoundEndMsg = chkMsgsRoundEnd.Checked;
            Settings.Default.SendRoundStartMsg = chkMsgsRoundStarted.Checked;
            Settings.Default.SendGameStartMsg = chkMsgsStartGame.Checked;
            Settings.Default.SendDoubleGuessMsg = chkSendDoubleGuessMsg.Checked;
            Settings.Default.SendSameGuessMessage = chkSendSameGuessMessage.Checked;
            Settings.Default.Save();
        }

        private void PageConnections_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            SaveTwitchSetting();
        }

        private void SaveTwitchSetting()
        {
          

            if (Settings.Default.oauthToken != txtGeneralOauthToken.Text)
            {
                Settings.Default.oauthToken = txtGeneralOauthToken.Text;
            }

            if (Settings.Default.TwitchChannel != txtGeneralChannelName.Text)
            {
                Settings.Default.TwitchChannel = txtGeneralChannelName.Text;
            }

            Settings.Default.Save();
        }

        private void wizardControl1_SelectedPageChanged(object sender, EventArgs e)
        {

        }

        private async void txtGeneralChannelName_Leave(object sender, EventArgs e)
        {
            Player player = await TwitchHelper.GetUserDataFromTwitch(userName: txtGeneralChannelName.Text);
            if (player != null && !string.IsNullOrEmpty(player.DisplayName))
            {
                txtGeneralChannelName.Text = Settings.Default.TwitchChannel = player.DisplayName;
                Settings.Default.ChannelId = player.PlatformId;
            }

        }

        private void reconnectTwitchBotButton_Click(object sender, EventArgs e)
        {

            SaveTwitchSetting();
            reconnectTwitchBotButton.Enabled = false;
            parent?.ConnectTwitchBot();
            AttributeDiscovery.AddEventHandlers(this, parent?.CurrentBot);

        }


        [DiscoverableEvent]
        private void ChannelJoined(object sender, ChannelJoinedEventArgs args)
        {
            MessageBox.Show("Twitch bot connection successful!", "Connection successful!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            EnableTwitchButton(true);
        }

        [DiscoverableEvent]
        private void ChannelNotJoined(object sender, ChannelNotJoinedEventArgs args)
        {
            MessageBox.Show("Couldn't connect Twitch bot. Please check your settings.", "Connection failed!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            EnableTwitchButton(false);
        }
        private delegate void EnableTwitchButtonCallback(bool enable);

        private void EnableTwitchButton(bool enable)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (reconnectTwitchBotButton.InvokeRequired)
            {
                EnableTwitchButtonCallback d = new(EnableTwitchButton);
                Invoke(d, enable);
            }
            else
            {
                pageConnections.AllowNext = enable;
                reconnectTwitchBotButton.Enabled = !enable;
            }
        }

        private void wizardControl1_Finished(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using FlagManagerDialog mgr = new();
            using FlagManagerOfficialPacksDialog diag = new(mgr);
            diag.ShowDialog(this);
        }

        private void linkLabelCar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://discord.com/channels/927177720092323861/943520012726054992/943521313295847485
            GeneralPurposeUtils.OpenURL("https://discord.com/channels/927177720092323861/943520012726054992/943521313295847485");
        }

        private void linkLabelUnity_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://discord.com/channels/927177720092323861/943520012726054992/944012258126745670
            GeneralPurposeUtils.OpenURL("https://discord.com/channels/927177720092323861/943520012726054992/944012258126745670");
        }

        private void linkLabelWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //https://discord.com/channels/927177720092323861/943520012726054992/943522471888445450
            GeneralPurposeUtils.OpenURL("https://discord.com/channels/927177720092323861/943520012726054992/943522471888445450");
        }

        private void linkLabelDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GeneralPurposeUtils.OpenURL("https://discord.gg/Y6JP2uVnWs");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GeneralPurposeUtils.OpenURL("https://twitchapps.com/tmi/");
        }

        private async void btnAuthorizeAutomatically_Click(object sender, EventArgs e)
        {
            TwitchOAuthHelper.SendRequestToBrowser();
            TwitchAuthenticationModel model = await TwitchOAuthHelper.GetAuthenticationValuesAsync();
            txtGeneralOauthToken.Text = model.Token;
            reconnectTwitchBotButton_Click(reconnectTwitchBotButton, new EventArgs());
        }

        private async void btnAuthorizeManually_Click(object sender, EventArgs e)
        {
            string url = TwitchOAuthHelper.SendRequestToBrowser(false);
            Thread thread = new(() => Clipboard.SetText(url));
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();
            ShowManualAuthMessageBoxAsync();
            TwitchAuthenticationModel model = await TwitchOAuthHelper.GetAuthenticationValuesAsync();
            txtGeneralOauthToken.Text = model.Token;
            reconnectTwitchBotButton_Click(reconnectTwitchBotButton, new EventArgs());
            EnableTwitchButton(false);
        }

        internal static void ShowManualAuthMessageBoxAsync()
        {
            Task.Run(ShowManualAuthMessageBox);
        }

        internal static void ShowManualAuthMessageBox()
        {
            StringBuilder sb = new();
            sb.AppendLine("We have copied the link to manually authorize GeoChatter into your clipboard.");
            sb.AppendLine("All you have to do is perform the following steps:");
            sb.AppendLine("1) Open an incognito window or a different browser");
            sb.AppendLine("2) Paste the content into the address bar of that window");
            sb.AppendLine("3) Press enter");
            sb.AppendLine("4) Log in to Twitch with the account you want to use");
            sb.AppendLine("5) Press \"Authorize\"");
            sb.AppendLine("6) Close the window");
            MessageBox.Show(sb.ToString(), "Manual authentication", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
