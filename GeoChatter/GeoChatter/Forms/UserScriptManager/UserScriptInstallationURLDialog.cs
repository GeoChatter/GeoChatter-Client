using GeoChatter.Core.Helpers;
using GeoChatter.FormUtils;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Forms
{
    /// <summary>
    /// Install JS userscript from URL
    /// </summary>
    public partial class UserScriptInstallationURLDialog : Form
    {
        private UserScriptManager manager { get; set; }

        /// <summary>
        /// List of userscripts installed during this dialog's lifetime
        /// </summary>
        public List<JSUserScript> Installed { get; } = new();

        /// <summary>
        /// Wheter any userscripts were installed
        /// </summary>
        public bool InstalledAny => Installed.Count > 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public UserScriptInstallationURLDialog(UserScriptManager manager)
        {
            this.manager = manager;

            InitializeComponent();
        }

        private void NewUserScript_InstallURLButton_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewUserScript_NameTextBox.Text))
            {
                MessageBox.Show("A name has to be given to the userscript!");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewUserScript_URLTextBox.Text))
            {
                MessageBox.Show("Invalid userscript URL!");
                return;
            }

            string name = GeneralPurposeUtils.MakeValidFileName(UserScriptManager.UserScripts.Select(e => e.Name), NewUserScript_NameTextBox.Text.Trim());
            string url = JSUserScript.GetFixedSourceURL(NewUserScript_URLTextBox.Text.Trim());

            if (!url.EndsWithDefault(".js"))
            {
                MessageBox.Show("Invalid userscript URL! Expected direct link to a javascript (.js) file!");
                return;
            }

            JSUserScript us = new(name, url, JSWrapperType.URL)
            {
                Source = url
            };

            string oldtxt = NewUserScript_InstallURLButton.Text;

            NewUserScript_InstallURLButton.Text = "Validating the script...";
            NewUserScript_InstallURLButton.Enabled = false;

            bool valid = us.PrepareForExecution();

            if (!valid)
            {
                MessageBox.Show("Failed to install userscript from given URL. Please make sure URL links to a valid script!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                us.Dispose();
            }
            else
            {
                us.DiscoverAndSetAll();
                if (string.IsNullOrWhiteSpace(us.AutoUpdateURLString))
                {
                    us.SetAutoUpdateURL(url);
                }
                manager.ListUpdated = true;
                Installed.Add(us);

                MessageBox.Show("Installed the userscript from: " + url, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

            NewUserScript_InstallURLButton.Enabled = true;
            NewUserScript_InstallURLButton.Text = oldtxt;

            if (valid)
            {
                Close();
            }
        }

        private void closeBtn_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
