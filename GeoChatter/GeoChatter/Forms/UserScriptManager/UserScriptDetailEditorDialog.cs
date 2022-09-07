using GeoChatter.Extensions;
using GeoChatter.Core.Helpers;
using GeoChatter.FormUtils;
using System.Windows.Forms;

namespace GeoChatter.Forms
{
    /// <summary>
    /// Dialog for editing userscript details
    /// </summary>
    public partial class UserScriptDetailEditorDialog : Form
    {
        /// <summary>
        /// Parent manager
        /// </summary>
        public UserScriptManager Manager { get; set; }

        private JSUserScript CurrentUserScript { get; set; }
        /// <summary>
        /// Wheter any details were changed
        /// </summary>
        public bool ChangesMade { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="current"></param>
        public UserScriptDetailEditorDialog(UserScriptManager parent, JSUserScript current)
        {
            InitializeComponent();
            Manager = parent;
            CurrentUserScript = current;

            if (CurrentUserScript == null)
            {
                Close();
            }
            else
            {
                Details_NameTextBox.Text = CurrentUserScript.Name;
                Details_SourceTextBox.Text = CurrentUserScript.Source;
                Details_DescriptionTextBox.Text = CurrentUserScript.Description;
                Details_VersionTextBox.Text = CurrentUserScript.Version;
                Details_UpdateDateTextBox.Text = CurrentUserScript.LastUpdated.ToDialogFriendlyString();
                Details_EditedTextBox.Text = CurrentUserScript.LastEdited.ToDialogFriendlyString();
                Details_CreatedTextBox.Text = CurrentUserScript.Created.ToDialogFriendlyString();
                Details_AutoUpdateTextBox.Text = CurrentUserScript.AutoUpdateURLString;
            }
        }

        private void Details_EditSaveButton_Click(object sender, System.EventArgs e)
        {
            string name = Details_NameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("A name has to be given to the userscript!");
                return;
            }

            string ask = "Are you sure you want to apply the changes below ?";
            bool changed = false;
            string oldname = CurrentUserScript.Name;

            if (CurrentUserScript.Name != name)
            {
                name = GeneralPurposeUtils.MakeValidFileName(UserScriptManager.GetUserScriptNames(), name);
                ask += $"\nName: \"{oldname}\" -> \"{name}\"";
                changed = true;
            }
            if (CurrentUserScript.Description != Details_DescriptionTextBox.Text)
            {
                ask += $"\nDescription: \"{CurrentUserScript.Description}\" -> \"{Details_DescriptionTextBox.Text}\"";
                changed = true;
            }
            if (CurrentUserScript.AutoUpdateURLString != Details_AutoUpdateTextBox.Text)
            {
                ask += $"\nUpdate URL: \"{CurrentUserScript.AutoUpdateURLString}\" -> \"{Details_AutoUpdateTextBox.Text}\"";
                changed = true;
            }

            if (!changed)
            {
                MessageBox.Show("No changes made to save!");
                return;
            }

            DialogResult res = MessageBox.Show(ask, "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (res != DialogResult.Yes)
            {
                return;
            }

            ChangesMade = true;
            CurrentUserScript.Name = name;
            CurrentUserScript.Description = Details_DescriptionTextBox.Text;
            CurrentUserScript.SetAutoUpdateURL(Details_AutoUpdateTextBox.Text);
            Manager.SaveUserScriptChanges(CurrentUserScript, oldname);
            Close();
        }

        private void closeBtn_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
