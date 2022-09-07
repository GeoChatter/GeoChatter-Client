using GeoChatter.Core.Helpers;
using GeoChatter.FormUtils;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace GeoChatter.Forms
{
    /// <summary>
    /// Dialog for installing a userscript from <see cref="UserScriptEditor"/>
    /// </summary>
    public partial class UserScriptInstallationEditorDialog : Form
    {
        /// <summary>
        /// Parent editor
        /// </summary>
        public UserScriptEditor Editor { get; set; }

        /// <summary>
        /// Userscript to be installed
        /// </summary>
        public JSUserScript CurrentUserScript { get; set; }

        /// <summary>
        /// Wheter installation succeeded
        /// </summary>
        public bool InstallSucceded { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editor"></param>
        public UserScriptInstallationEditorDialog(UserScriptEditor editor)
        {
            InitializeComponent();

            Editor = editor;
        }

        /// <summary>
        /// Initialize the dialog
        /// </summary>
        /// <param name="editor">Parent editor</param>
        /// <param name="name">Userscript name</param>
        /// <param name="sourceText">Userscript code</param>
        public UserScriptInstallationEditorDialog(UserScriptEditor editor, string name, string sourceText) : this(editor)
        {
            CurrentUserScript = new(name, sourceText, JSWrapperType.Raw);
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
            CurrentUserScript.Source = UserScriptEditor.SourceName;
#pragma warning restore CA1416 // Validate platform compatibility
#else
            CurrentUserScript.Source = "unknown";
#endif
            CurrentUserScript.Description = CurrentUserScript.DiscoverDescription();
            CurrentUserScript.SetAutoUpdateURL(CurrentUserScript.DiscoverUpdateURL());
            CurrentUserScript.Version = CurrentUserScript.DiscoverVersion();

            SetupInfoBoxes();
        }

        private void SetupInfoBoxes()
        {
            NewUserScript_NameTextBox.Text = CurrentUserScript.Name;
            NewUserScript_SourceTextBox.Text = CurrentUserScript.Source;
            string desc = CurrentUserScript.Description;
            NewUserScript_DescriptionTextBox.Text = string.IsNullOrWhiteSpace(desc) ? NewUserScript_DescriptionTextBox.Text : desc;
            string url = CurrentUserScript.AutoUpdateURLString;
            NewUserScript_AutoUpdateLinkTextBox.Text = string.IsNullOrWhiteSpace(url) ? NewUserScript_AutoUpdateLinkTextBox.Text : url;
        }

        private void NewUserScript_InstallEditorButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(NewUserScript_NameTextBox.Text))
            {
                MessageBox.Show("A name has to be given to the userscript!");
                return;
            }

            string name = GeneralPurposeUtils.MakeValidFileName(UserScriptManager.UserScripts.Select(e => e.Name), NewUserScript_NameTextBox.Text.Trim());

            string oldtxt = NewUserScript_InstallEditorButton.Text;

            NewUserScript_InstallEditorButton.Text = "Validating the userscript...";
            NewUserScript_InstallEditorButton.Enabled = false;

            bool valid = CurrentUserScript.PrepareForExecution();

            if (!valid)
            {
                MessageBox.Show("Failed to install userscript from the editor!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                CurrentUserScript.Dispose();
            }
            else
            {
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
                Editor.Manager.ListUpdated = true;
#pragma warning restore CA1416 // Validate platform compatibility
#endif
                InstallSucceded = true;
                MessageBox.Show($"Installed the userscript from the editor!\nUserscript name: {name}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
            Editor.SetName(NewUserScript_NameTextBox.Text);
#pragma warning restore CA1416 // Validate platform compatibility
#endif
            CurrentUserScript.Name = NewUserScript_NameTextBox.Text;
            CurrentUserScript.Description = NewUserScript_DescriptionTextBox.Text;
            CurrentUserScript.IsAutoUpdateEnabled = NewUserScript_AutoUpdateCheckBox.Checked;
            CurrentUserScript.SetAutoUpdateURL(NewUserScript_AutoUpdateLinkTextBox.Text);

            NewUserScript_InstallEditorButton.Enabled = true;
            NewUserScript_InstallEditorButton.Text = oldtxt;

            if (valid)
            {
                Close();
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
