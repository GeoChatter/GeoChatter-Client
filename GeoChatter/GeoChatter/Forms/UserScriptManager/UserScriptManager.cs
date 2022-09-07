using GeoChatter.Extensions;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Storage;
using GeoChatter.FormUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Forms
{
    /// <summary>
    /// JS UserScript manager
    /// </summary>
    public partial class UserScriptManager : Form
    {
        private const string Details_NameTextBoxPlaceHolder = "MyUserScript";
        private const string Details_DescriptionTextBoxPlaceHolder = "Information about the userscript...";
        private const string UserScriptNameColumn = "UserScriptName";
        private const string UserScriptPath = "userscripts";
        private const string MetaFile = "metadata.json";

        /// <summary>
        /// List of all userscripts
        /// </summary>
        public static List<JSUserScript> UserScripts { get; } = new();

        private static DataStorage storage { get; } = new DataStorage(UserScriptPath, ".js");

        private MainForm mainForm { get; set; }

        /// <summary>
        /// Wheter there were any changes to <see cref="UserScripts"/>
        /// </summary>
        public bool ListUpdated { get; set; }

        /// <summary>
        /// Currently selected userscript by clicking
        /// </summary>
        public JSUserScript CurrentUserScript { get; private set; }

        /// <summary>
        /// Wheter manager is useable (If <see cref="ManagerAvailable"/> is <see langword="false"/> at the time, this is set to <see langword="false"/>)
        /// <para>If this value is not <see langword="true"/> after instance is constructed, manager should be closed and disposed.</para>
        /// </summary>
        public bool IsUseable { get; private set; } = true;
        /// <summary>
        /// Wheter manager is currently available and not checking auto updates
        /// </summary>
        public static bool ManagerAvailable { get; private set; } = true;
        /// <summary>
        /// 
        /// </summary>
        public UserScriptManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Check <see cref="IsUseable"/> after constructing to decide wheter to close the manager. See <see cref="IsUseable"/> docs.
        /// </summary>
        /// <param name="parent"></param>
        public UserScriptManager(MainForm parent) : this()
        {
            mainForm = parent;

            Details_DescriptionTextBox.AddPlaceHolder(Details_DescriptionTextBoxPlaceHolder);
            Details_NameTextBox.AddPlaceHolder(Details_NameTextBoxPlaceHolder);

            if (ManagerAvailable)
            {
                LoadLocalUserScripts(this);
            }
            else
            {
                MessageBox.Show("UserScript manager is currently searching for auto-updates. It will not be available until the update process finishes! Try again a bit later.", "Update In Progress", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                IsUseable = false;
            }
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
            mainForm.ResetJSCTRLCheck();
#pragma warning restore CA1416 // Validate platform compatibility
#endif
        }

        private static void SetMetas()
        {
            List<UserScriptMeta> metas = new();

            UserScripts.ForEach(ext => metas.Add(CreateMetaFromUserScript(ext)));

            string metaData = JsonConvert.SerializeObject(metas);
            storage.WriteOtherFile(MetaFile, metaData);
        }

        private static List<UserScriptMeta> GetMetas()
        {
            string meta = storage.ReadOtherFile(MetaFile);
            if (string.IsNullOrWhiteSpace(meta))
            {
                return new();
            }

            List<UserScriptMeta> metaData = JsonConvert.DeserializeObject<List<UserScriptMeta>>(meta);

            return metaData;
        }

        /// <summary>
        /// Run auto updates on loaded userscripts
        /// </summary>
        /// <returns></returns>
        public static string RunAutoUpdates()
        {
            try
            {
                ManagerAvailable = false;
                string updates = string.Empty;

                foreach (JSUserScript us in UserScripts)
                {
                    if (!us.IsAutoUpdateEnabled)
                    {
                        continue;
                    }

                    us.DiscoverAndSetAll();

                    string url = us.AutoUpdateURLString;
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        if (us.Source == UserScriptEditor.SourceName)
                        {
                            continue;
                        }
                        url = us.Source;
                    }

                    if (string.IsNullOrWhiteSpace(url))
                    {
                        continue;
                    }

                    string oldv = us.Version;

                    bool updated = us.CheckForUpdates(url);
                    if (updated)
                    {
                        updates += us.Name + ": " + oldv + " -> " + us.Version + "\n";
                        storage.WriteFile(us.Name, us.Wrapper.StoredRawScript);
                    }
                }

                SetMetas();
                return updates;
            }
            catch (Exception err)
            {
                MessageBox.Show("UserScript manager returned an error: " + err.Message, "Auto Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            finally
            {
                ManagerAvailable = true;
            }

            return string.Empty;
        }

        internal static List<string> GetUserScriptNames()
        {
            return UserScripts.Select(e => e.Name).ToList();
        }

        private static void LoadLocalUserScripts(UserScriptManager manager)
        {
            Dictionary<string, string> usrscripts = storage.ReadAllFiles();
            usrscripts.ForEach(pair => manager.CreateUserScriptsFromLocal(pair.Key, pair.Value));

            List<UserScriptMeta> metas = GetMetas();

            foreach (UserScriptMeta met in metas)
            {
                foreach (JSUserScript us in UserScripts)
                {
                    if (us.Name == met.Name)
                    {
                        SetUserScriptFromMeta(us, met);
                    }
                }
            }
        }

        private void CreateUserScriptsFromLocal(string file, string script)
        {
            JSUserScript e = UserScripts.FirstOrDefault(e => e.Name == file);
            if (e == null) // Added manually to folder (untracked)
            {
                AddUserScript(new(file, script, JSWrapperType.Raw));
            }
            else // Reload, re-prepare
            {
                e.Wrapper.StoredRawScript = script;
                e.Wrapper.DependencyScopes = null;
                e.PrepareForExecution();
                e.DiscoverAndSetAll();
                AddUserScriptsToTable(e);
            }
        }

        private void UserScript_NameChanged(object sender, UserScriptUpdateEventArgs e)
        {
            RenameUserScriptFile(e.OldValue.ToString(), e.NewValue.ToString(), e.UserScript);
        }

        private static void RenameUserScriptFile(string old, string name, JSUserScript ext)
        {
            if (!storage.RenameFile(old, name))
            {
                ext.SetNameDirect(old);
                MessageBox.Show("Failed to set script name. Rolled back to old name.", "Name Change Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private static void SetUserScriptToTableCells(DataGridViewCellCollection cells, JSUserScript us)
        {
            if (us == null)
            {
                return;
            }

            cells[UserScriptNameColumn].Value = us.Name;
            cells["Source"].Value = us.Source;
            cells["AutoUpdateURL"].Value = us.AutoUpdateURLString;
            cells["AutoUpdate"].Value = us.IsAutoUpdateEnabled;
            cells["Enable"].Value = us.IsEnabled;
        }

        private void AddUserScriptsToTable(JSUserScript ext)
        {
            if (UserScriptsTable == null)
            {
                return;
            }
            SetUserScriptToTableCells(UserScriptsTable.Rows[UserScriptsTable.Rows.Add()].Cells, ext);
        }

        private int GetUserScriptIndex(JSUserScript ext, string nameoverride = "")
        {
            string name = string.IsNullOrWhiteSpace(nameoverride) ? ext.Name : nameoverride;
            for (int i = 0; i < UserScriptsTable.RowCount; i++)
            {
                if (UserScriptsTable.Rows[i].Cells[UserScriptNameColumn].Value.ToString() == name)
                {
                    return i;
                }
            }

            return -1;
        }

        private void RemoveUserScriptFromTable(JSUserScript us)
        {
            int idx = GetUserScriptIndex(us);
            if (idx == -1)
            {
                return;
            }

            UserScriptsTable.Rows.RemoveAt(idx);
            storage.RemoveFile(us.Name);
        }

        internal void SaveUserScriptChanges(JSUserScript us, string nameoverride = "")
        {
            if (us == null)
            {
                return;
            }

            int idx = GetUserScriptIndex(us, nameoverride);
            if (idx == -1)
            {
                return;
            }
            us.LastEdited = DateTime.Now;
            SetUserScriptToTableCells(UserScriptsTable.Rows[idx].Cells, us);
            SetSelectionDetailState(true);

            storage.WriteFile(us.Name, us.Wrapper.StoredRawScript);
        }

        private static void SetUserScriptFromMeta(JSUserScript us, UserScriptMeta meta)
        {
            if (meta == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(meta.Description))
            {
                us.Description = meta.Description;
            }
            if (!string.IsNullOrWhiteSpace(meta.UpdateURL))
            {
                us.SetAutoUpdateURL(meta.UpdateURL);
            }
            if (!string.IsNullOrWhiteSpace(meta.Source))
            {
                us.Source = meta.Source;
            }

            us.IsEnabled = meta.IsEnabled;
            us.IsAutoUpdateEnabled = meta.AutoUpdateEnabled;
            us.Created = meta.Created;
            us.LastEdited = meta.LastEdited;
            us.LastUpdated = meta.LastUpdated;
        }

        private static UserScriptMeta CreateMetaFromUserScript(JSUserScript us)
        {
            UserScriptMeta meta = new();

            if (!string.IsNullOrWhiteSpace(us.Name))
            {
                meta.Name = us.Name;
            }
            if (!string.IsNullOrWhiteSpace(us.Description))
            {
                meta.Description = us.Description;
            }
            if (!string.IsNullOrWhiteSpace(us.AutoUpdateURLString))
            {
                meta.UpdateURL = us.AutoUpdateURLString;
            }
            if (!string.IsNullOrWhiteSpace(us.Source))
            {
                meta.Source = us.Source;
            }

            meta.IsEnabled = us.IsEnabled;
            meta.AutoUpdateEnabled = us.IsAutoUpdateEnabled;
            meta.Created = us.Created;
            meta.LastEdited = us.LastEdited;
            meta.LastUpdated = us.LastUpdated;

            return meta;
        }

        internal void AddUserScript(JSUserScript us)
        {
            if (us == null)
            {
                return;
            }

            us.DiscoverAndSetAll();
            us.NameChanged += UserScript_NameChanged;

            UserScripts.Add(us);

            us.PrepareForExecution();

            AddUserScriptsToTable(us);

            storage.WriteFile(us.Name, us.Wrapper.StoredRawScript);
        }

        private void RemoveUserScript(JSUserScript us)
        {
            if (us == null)
            {
                return;
            }
            UserScripts.Remove(us);
            RemoveUserScriptFromTable(us);
        }

        private void SetSelectionDetailState(bool state)
        {
            Details_CheckUpdateButton.Enabled = state;
            Details_LoadButton.Enabled = state;
            Details_RemoveButton.Enabled = state;
            Details_CloneButton.Enabled = state;
            Details_EditButton.Enabled = state;
            Details_AutoUpdateTextBox.Enabled = state;
            SetSelectionDetailInfos(state);
        }

        private void SetSelectionDetailInfos(bool fill)
        {
            if (fill && CurrentUserScript != null)
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
            else
            {
                Details_NameTextBox.Text = string.Empty;
                Details_SourceTextBox.Text = string.Empty;
                Details_DescriptionTextBox.Text = string.Empty;
                Details_VersionTextBox.Text = string.Empty;
                Details_UpdateDateTextBox.Text = string.Empty;
                Details_EditedTextBox.Text = string.Empty;
                Details_CreatedTextBox.Text = string.Empty;
                Details_AutoUpdateTextBox.Text = string.Empty;
            }
        }

        private void SelectUserScript(int index)
        {
            if (index < 0 || UserScriptsTable.RowCount <= index)
            {
                return;
            }

            string selectedName = UserScriptsTable.Rows[index].Cells[UserScriptNameColumn].Value.ToString();
            CurrentUserScript = UserScripts.FirstOrDefault(ext => ext.Name == selectedName);

            if (CurrentUserScript == null)
            {
                return;
            }

            SetSelectionDetailState(true);
        }

        private void DeselectCurrentUserScript()
        {
            CurrentUserScript = null;
            SetSelectionDetailState(false);
        }

        private void UserScript_InstallDialogButton_Click(object sender, EventArgs e)
        {
            Invoke(delegate ()
            {
                using UserScriptInstallationURLDialog diag = new(this);
                diag.ShowDialog();
                if (diag.InstalledAny)
                {
                    diag.Installed.ForEach(AddUserScript);
                    CurrentUserScript = diag.Installed.Last();
                    SetSelectionDetailState(true);
                }
            }
            );
        }

        private void UserScript_CreateButton_Click(object sender, EventArgs e)
        {
            Invoke(delegate ()
            {
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
                using UserScriptEditor diag = new(this);
                diag.ShowDialog();
#pragma warning restore CA1416 // Validate platform compatibility
#endif
            }
            );
        }

        private void Details_RemoveButton_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show($"Are you sure you want to remove the userscript named \"{CurrentUserScript.Name}\" ?", "Deletion Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            switch (res)
            {
                case DialogResult.Yes:
                    {
                        RemoveUserScript(CurrentUserScript);
                        DeselectCurrentUserScript();
                        break;
                    }
                default:
                    break;
            }
        }

        private void Table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex >= 3)
            {
                ListUpdated = true;
                string name = UserScriptsTable.Rows[e.RowIndex].Cells[UserScriptNameColumn].Value.ToString();
                JSUserScript us = UserScripts.FirstOrDefault(usc => usc.Name == name);
                if (e.ColumnIndex == 4)
                {
                    us.IsEnabled = !us.IsEnabled;
                }
                else
                {
                    us.IsAutoUpdateEnabled = !us.IsAutoUpdateEnabled;
                }
                SetUserScriptToTableCells(UserScriptsTable.Rows[e.RowIndex].Cells, us);
            }
            else
            {
                SelectUserScript(e.RowIndex);
            }
        }

        private void Details_LoadButton_Click(object sender, EventArgs e)
        {
            if (CurrentUserScript == null)
            {
                return;
            }

            Invoke(delegate ()
            {
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
                using UserScriptEditor diag = new(this, CurrentUserScript);
                diag.ShowDialog();
#pragma warning restore CA1416 // Validate platform compatibility
#endif
                SetSelectionDetailInfos(true);
            }
            );
        }

        private void Details_EditButton_Click(object sender, EventArgs e)
        {
            if (CurrentUserScript == null)
            {
                return;
            }

            Invoke(delegate ()
            {
                using UserScriptDetailEditorDialog diag = new(this, CurrentUserScript);
                diag.ShowDialog();
                SetSelectionDetailInfos(true);
            }
            );
        }

        private void Details_CloneButton_Click(object sender, EventArgs e)
        {
            if (CurrentUserScript == null)
            {
                return;
            }

            DialogResult res = MessageBox.Show($"Are you sure you want to clone \"{CurrentUserScript.Name}\"?", "Cloning UserScript", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (res == DialogResult.Yes)
            {
                string name = CurrentUserScript.Name + "_clone";
                name = GeneralPurposeUtils.MakeValidFileName(UserScripts.Select(e => e.Name), name);
                JSUserScript copy = CurrentUserScript.Copy();
                copy.Name = name;
                AddUserScript(copy);
            }
        }


        private void Details_CheckUpdateButton_Click(object sender, EventArgs e)
        {
            if (CurrentUserScript == null)
            {
                return;
            }

            string url = CurrentUserScript.AutoUpdateURLString;
            if (string.IsNullOrWhiteSpace(url))
            {
                if (CurrentUserScript.Source == UserScriptEditor.SourceName)
                {
                    MessageBox.Show("An auto-update URL for the editor sourced userscript is required. Please set an update link and try again!", "Update URL Missing", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                DialogResult res = MessageBox.Show("An auto-update URL for the userscript wasn't provided. Would you like to try using the installation URL to check for updates?", "Provide Update Link", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res != DialogResult.Yes)
                {
                    return;
                }
                url = CurrentUserScript.Source;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("An auto-update URL for the editor sourced userscript is required. Please set an update link and try again!", "Update URL Missing", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }

            string oldtxt = Details_CheckUpdateButton.Text;

            Details_CheckUpdateButton.Text = "Looking for updates...";
            Details_CheckUpdateButton.Enabled = false;

            bool updated = CurrentUserScript.CheckForUpdates(url);

            if (updated)
            {
                MessageBox.Show($"Updated \"{CurrentUserScript.Name}\" to version \"{CurrentUserScript.Version}\"", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                CurrentUserScript.SetAutoUpdateURL(CurrentUserScript.DiscoverUpdateURL());
                CurrentUserScript.Version = CurrentUserScript.DiscoverVersion();
                CurrentUserScript.Description = CurrentUserScript.DiscoverDescription();
                SaveUserScriptChanges(CurrentUserScript);
            }
            else
            {
                MessageBox.Show("No update found compared to version given in: " + url, "No Update", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

            Details_CheckUpdateButton.Enabled = true;
            Details_CheckUpdateButton.Text = oldtxt;
        }

        private void UserScriptManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetMetas();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UserScriptManager_Enter(object sender, EventArgs e)
        {
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
            mainForm?.ResetJSCTRLCheck();
#pragma warning restore CA1416 // Validate platform compatibility
#endif
        }
    }
}
