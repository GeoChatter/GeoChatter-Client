#pragma warning disable CS1591,CA1062
namespace GeoChatter.Forms
{
    partial class UserScriptManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserScriptManager));
            this.UserScript_InstallDialogButton = new System.Windows.Forms.Button();
            this.Details_AutoUpdateTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Details_DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.Details_DescriptionLabel = new System.Windows.Forms.Label();
            this.Details_UpdateDateTextBox = new System.Windows.Forms.MaskedTextBox();
            this.Details_CreatedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Details_VersionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Details_RemoveButton = new System.Windows.Forms.Button();
            this.Details_NameTextBox = new System.Windows.Forms.TextBox();
            this.Details_NameLabel = new System.Windows.Forms.Label();
            this.Details_LoadButton = new System.Windows.Forms.Button();
            this.Details_CheckUpdateButton = new System.Windows.Forms.Button();
            this.Details_SourceTextBox = new System.Windows.Forms.TextBox();
            this.Details_SourceLabel = new System.Windows.Forms.Label();
            this.Details_CloneButton = new System.Windows.Forms.Button();
            this.UserScriptsTable = new System.Windows.Forms.DataGridView();
            this.UserScriptName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AutoUpdateURL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AutoUpdate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Enable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Details_EditedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Details_TitleLabel = new System.Windows.Forms.Label();
            this.UserScriptListBoxLabel = new System.Windows.Forms.Label();
            this.UserScript_CreateButton = new System.Windows.Forms.Button();
            this.Details_EditButton = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UserScriptsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // Extension_InstallDialogButton
            // 
            this.UserScript_InstallDialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UserScript_InstallDialogButton.BackColor = System.Drawing.Color.White;
            this.UserScript_InstallDialogButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.UserScript_InstallDialogButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UserScript_InstallDialogButton.FlatAppearance.BorderSize = 0;
            this.UserScript_InstallDialogButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.UserScript_InstallDialogButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UserScript_InstallDialogButton.ForeColor = System.Drawing.SystemColors.Desktop;
            this.UserScript_InstallDialogButton.Location = new System.Drawing.Point(681, 9);
            this.UserScript_InstallDialogButton.Margin = new System.Windows.Forms.Padding(0);
            this.UserScript_InstallDialogButton.Name = "UserScript_InstallDialogButton";
            this.UserScript_InstallDialogButton.Size = new System.Drawing.Size(170, 30);
            this.UserScript_InstallDialogButton.TabIndex = 91;
            this.UserScript_InstallDialogButton.Text = "Install a new script...";
            this.UserScript_InstallDialogButton.UseVisualStyleBackColor = false;
            this.UserScript_InstallDialogButton.Click += new System.EventHandler(this.UserScript_InstallDialogButton_Click);
            // 
            // Details_AutoUpdateTextBox
            // 
            this.Details_AutoUpdateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_AutoUpdateTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.Details_AutoUpdateTextBox.Location = new System.Drawing.Point(139, 585);
            this.Details_AutoUpdateTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_AutoUpdateTextBox.Name = "Details_AutoUpdateTextBox";
            this.Details_AutoUpdateTextBox.ReadOnly = true;
            this.Details_AutoUpdateTextBox.Size = new System.Drawing.Size(886, 23);
            this.Details_AutoUpdateTextBox.TabIndex = 88;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(29, 586);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 15);
            this.label6.TabIndex = 87;
            this.label6.Text = "Auto-update link";
            // 
            // Details_DescriptionTextBox
            // 
            this.Details_DescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_DescriptionTextBox.Location = new System.Drawing.Point(85, 487);
            this.Details_DescriptionTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_DescriptionTextBox.Name = "Details_DescriptionTextBox";
            this.Details_DescriptionTextBox.ReadOnly = true;
            this.Details_DescriptionTextBox.Size = new System.Drawing.Size(940, 23);
            this.Details_DescriptionTextBox.TabIndex = 86;
            // 
            // Details_DescriptionLabel
            // 
            this.Details_DescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_DescriptionLabel.AutoSize = true;
            this.Details_DescriptionLabel.Location = new System.Drawing.Point(29, 490);
            this.Details_DescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Details_DescriptionLabel.Name = "Details_DescriptionLabel";
            this.Details_DescriptionLabel.Size = new System.Drawing.Size(45, 15);
            this.Details_DescriptionLabel.TabIndex = 85;
            this.Details_DescriptionLabel.Text = "Details:";
            // 
            // Details_UpdateDateTextBox
            // 
            this.Details_UpdateDateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_UpdateDateTextBox.Location = new System.Drawing.Point(271, 516);
            this.Details_UpdateDateTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_UpdateDateTextBox.Mask = "00/00/0000 90:00:00";
            this.Details_UpdateDateTextBox.Name = "Details_UpdateDateTextBox";
            this.Details_UpdateDateTextBox.ReadOnly = true;
            this.Details_UpdateDateTextBox.Size = new System.Drawing.Size(191, 23);
            this.Details_UpdateDateTextBox.TabIndex = 84;
            this.Details_UpdateDateTextBox.Text = "01011970000000";
            this.Details_UpdateDateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Details_UpdateDateTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // Details_CreatedTextBox
            // 
            this.Details_CreatedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_CreatedTextBox.Location = new System.Drawing.Point(838, 516);
            this.Details_CreatedTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_CreatedTextBox.Mask = "00/00/0000 90:00:00";
            this.Details_CreatedTextBox.Name = "Details_CreatedTextBox";
            this.Details_CreatedTextBox.ReadOnly = true;
            this.Details_CreatedTextBox.Size = new System.Drawing.Size(187, 23);
            this.Details_CreatedTextBox.TabIndex = 69;
            this.Details_CreatedTextBox.Text = "01011970000000";
            this.Details_CreatedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Details_CreatedTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(776, 519);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 15);
            this.label3.TabIndex = 67;
            this.label3.Text = "Created:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(204, 520);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 83;
            this.label2.Text = "Updated:";
            // 
            // Details_VersionTextBox
            // 
            this.Details_VersionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_VersionTextBox.Location = new System.Drawing.Point(85, 517);
            this.Details_VersionTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_VersionTextBox.Name = "Details_VersionTextBox";
            this.Details_VersionTextBox.ReadOnly = true;
            this.Details_VersionTextBox.Size = new System.Drawing.Size(62, 23);
            this.Details_VersionTextBox.TabIndex = 82;
            this.Details_VersionTextBox.Text = "0.0.0";
            this.Details_VersionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 520);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 81;
            this.label1.Text = "Version:";
            // 
            // Details_RemoveButton
            // 
            this.Details_RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_RemoveButton.Enabled = false;
            this.Details_RemoveButton.Location = new System.Drawing.Point(20, 547);
            this.Details_RemoveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_RemoveButton.Name = "Details_RemoveButton";
            this.Details_RemoveButton.Size = new System.Drawing.Size(217, 29);
            this.Details_RemoveButton.TabIndex = 79;
            this.Details_RemoveButton.Text = "Remove";
            this.Details_RemoveButton.UseVisualStyleBackColor = true;
            this.Details_RemoveButton.Click += new System.EventHandler(this.Details_RemoveButton_Click);
            // 
            // Details_NameTextBox
            // 
            this.Details_NameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_NameTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.Details_NameTextBox.Location = new System.Drawing.Point(85, 453);
            this.Details_NameTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_NameTextBox.Name = "Details_NameTextBox";
            this.Details_NameTextBox.ReadOnly = true;
            this.Details_NameTextBox.Size = new System.Drawing.Size(376, 23);
            this.Details_NameTextBox.TabIndex = 78;
            // 
            // Details_NameLabel
            // 
            this.Details_NameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_NameLabel.AutoSize = true;
            this.Details_NameLabel.Location = new System.Drawing.Point(29, 457);
            this.Details_NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Details_NameLabel.Name = "Details_NameLabel";
            this.Details_NameLabel.Size = new System.Drawing.Size(42, 15);
            this.Details_NameLabel.TabIndex = 77;
            this.Details_NameLabel.Text = "Name:";
            // 
            // Details_LoadButton
            // 
            this.Details_LoadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_LoadButton.Enabled = false;
            this.Details_LoadButton.Location = new System.Drawing.Point(481, 547);
            this.Details_LoadButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_LoadButton.Name = "Details_LoadButton";
            this.Details_LoadButton.Size = new System.Drawing.Size(545, 29);
            this.Details_LoadButton.TabIndex = 76;
            this.Details_LoadButton.Text = "Load to editor...";
            this.Details_LoadButton.UseVisualStyleBackColor = true;
            this.Details_LoadButton.Click += new System.EventHandler(this.Details_LoadButton_Click);
            // 
            // Details_CheckUpdateButton
            // 
            this.Details_CheckUpdateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_CheckUpdateButton.Enabled = false;
            this.Details_CheckUpdateButton.Location = new System.Drawing.Point(20, 621);
            this.Details_CheckUpdateButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_CheckUpdateButton.Name = "Details_CheckUpdateButton";
            this.Details_CheckUpdateButton.Size = new System.Drawing.Size(439, 30);
            this.Details_CheckUpdateButton.TabIndex = 75;
            this.Details_CheckUpdateButton.Text = "Check for updates";
            this.Details_CheckUpdateButton.UseVisualStyleBackColor = true;
            this.Details_CheckUpdateButton.Click += new System.EventHandler(this.Details_CheckUpdateButton_Click);
            // 
            // Details_SourceTextBox
            // 
            this.Details_SourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_SourceTextBox.Location = new System.Drawing.Point(533, 453);
            this.Details_SourceTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_SourceTextBox.Name = "Details_SourceTextBox";
            this.Details_SourceTextBox.ReadOnly = true;
            this.Details_SourceTextBox.Size = new System.Drawing.Size(492, 23);
            this.Details_SourceTextBox.TabIndex = 74;
            this.Details_SourceTextBox.Text = "URL or <editor>";
            // 
            // Details_SourceLabel
            // 
            this.Details_SourceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_SourceLabel.AutoSize = true;
            this.Details_SourceLabel.Location = new System.Drawing.Point(477, 457);
            this.Details_SourceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Details_SourceLabel.Name = "Details_SourceLabel";
            this.Details_SourceLabel.Size = new System.Drawing.Size(46, 15);
            this.Details_SourceLabel.TabIndex = 73;
            this.Details_SourceLabel.Text = "Source:";
            // 
            // Details_CloneButton
            // 
            this.Details_CloneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_CloneButton.Enabled = false;
            this.Details_CloneButton.Location = new System.Drawing.Point(244, 547);
            this.Details_CloneButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_CloneButton.Name = "Details_CloneButton";
            this.Details_CloneButton.Size = new System.Drawing.Size(218, 29);
            this.Details_CloneButton.TabIndex = 72;
            this.Details_CloneButton.Text = "Clone";
            this.Details_CloneButton.UseVisualStyleBackColor = true;
            this.Details_CloneButton.Click += new System.EventHandler(this.Details_CloneButton_Click);
            // 
            // ExtensionsTable
            // 
            this.UserScriptsTable.AllowUserToAddRows = false;
            this.UserScriptsTable.AllowUserToDeleteRows = false;
            this.UserScriptsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserScriptsTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UserScriptsTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.UserScriptsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UserScriptsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserScriptName,
            this.Source,
            this.AutoUpdateURL,
            this.AutoUpdate,
            this.Enable});
            this.UserScriptsTable.Location = new System.Drawing.Point(20, 44);
            this.UserScriptsTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.UserScriptsTable.Name = "UserScriptsTable";
            this.UserScriptsTable.ReadOnly = true;
            this.UserScriptsTable.RowTemplate.ReadOnly = true;
            this.UserScriptsTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.UserScriptsTable.Size = new System.Drawing.Size(1006, 372);
            this.UserScriptsTable.TabIndex = 71;
            this.UserScriptsTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Table_CellClick);
            // 
            // ExtensionName
            // 
            this.UserScriptName.FillWeight = 200F;
            this.UserScriptName.HeaderText = "Name";
            this.UserScriptName.Name = "UserScriptName";
            this.UserScriptName.ReadOnly = true;
            // 
            // Source
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Source.DefaultCellStyle = dataGridViewCellStyle2;
            this.Source.FillWeight = 200F;
            this.Source.HeaderText = "Source";
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            // 
            // AutoUpdateURL
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.AutoUpdateURL.DefaultCellStyle = dataGridViewCellStyle3;
            this.AutoUpdateURL.FillWeight = 150F;
            this.AutoUpdateURL.HeaderText = "Auto Update URL";
            this.AutoUpdateURL.Name = "AutoUpdateURL";
            this.AutoUpdateURL.ReadOnly = true;
            // 
            // AutoUpdate
            // 
            this.AutoUpdate.HeaderText = "Auto Update";
            this.AutoUpdate.Name = "AutoUpdate";
            this.AutoUpdate.ReadOnly = true;
            this.AutoUpdate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AutoUpdate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Enable
            // 
            this.Enable.HeaderText = "Enable";
            this.Enable.Name = "Enable";
            this.Enable.ReadOnly = true;
            this.Enable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Enable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Details_EditedTextBox
            // 
            this.Details_EditedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_EditedTextBox.Location = new System.Drawing.Point(533, 517);
            this.Details_EditedTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_EditedTextBox.Mask = "00/00/0000 90:00:00";
            this.Details_EditedTextBox.Name = "Details_EditedTextBox";
            this.Details_EditedTextBox.ReadOnly = true;
            this.Details_EditedTextBox.Size = new System.Drawing.Size(190, 23);
            this.Details_EditedTextBox.TabIndex = 70;
            this.Details_EditedTextBox.Text = "01011970000000";
            this.Details_EditedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Details_EditedTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(477, 520);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 15);
            this.label4.TabIndex = 68;
            this.label4.Text = "Edited:";
            // 
            // Details_TitleLabel
            // 
            this.Details_TitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Details_TitleLabel.Font = new System.Drawing.Font("Segoe UI Light", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Details_TitleLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Details_TitleLabel.Location = new System.Drawing.Point(14, 419);
            this.Details_TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Details_TitleLabel.Name = "Details_TitleLabel";
            this.Details_TitleLabel.Size = new System.Drawing.Size(523, 31);
            this.Details_TitleLabel.TabIndex = 66;
            this.Details_TitleLabel.Text = "UserScript Details ";
            // 
            // ExtensionListBoxLabel
            // 
            this.UserScriptListBoxLabel.Font = new System.Drawing.Font("Segoe UI Light", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UserScriptListBoxLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.UserScriptListBoxLabel.Location = new System.Drawing.Point(14, 10);
            this.UserScriptListBoxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserScriptListBoxLabel.Name = "UserScriptListBoxLabel";
            this.UserScriptListBoxLabel.Size = new System.Drawing.Size(523, 29);
            this.UserScriptListBoxLabel.TabIndex = 61;
            this.UserScriptListBoxLabel.Text = "UserScripts";
            // 
            // Extension_CreateButton
            // 
            this.UserScript_CreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UserScript_CreateButton.BackColor = System.Drawing.Color.White;
            this.UserScript_CreateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.UserScript_CreateButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UserScript_CreateButton.FlatAppearance.BorderSize = 0;
            this.UserScript_CreateButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.UserScript_CreateButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UserScript_CreateButton.ForeColor = System.Drawing.SystemColors.Desktop;
            this.UserScript_CreateButton.Location = new System.Drawing.Point(850, 9);
            this.UserScript_CreateButton.Margin = new System.Windows.Forms.Padding(0);
            this.UserScript_CreateButton.Name = "UserScript_CreateButton";
            this.UserScript_CreateButton.Size = new System.Drawing.Size(175, 30);
            this.UserScript_CreateButton.TabIndex = 92;
            this.UserScript_CreateButton.Text = "Create a new userscript...";
            this.UserScript_CreateButton.UseVisualStyleBackColor = false;
            this.UserScript_CreateButton.Click += new System.EventHandler(this.UserScript_CreateButton_Click);
            // 
            // Details_EditButton
            // 
            this.Details_EditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Details_EditButton.Enabled = false;
            this.Details_EditButton.Location = new System.Drawing.Point(478, 621);
            this.Details_EditButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_EditButton.Name = "Details_EditButton";
            this.Details_EditButton.Size = new System.Drawing.Size(547, 30);
            this.Details_EditButton.TabIndex = 93;
            this.Details_EditButton.Text = "Edit details...";
            this.Details_EditButton.UseVisualStyleBackColor = true;
            this.Details_EditButton.Click += new System.EventHandler(this.Details_EditButton_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(0, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(0, 0);
            this.btnClose.TabIndex = 94;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ExtensionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1040, 666);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.Details_EditButton);
            this.Controls.Add(this.UserScript_CreateButton);
            this.Controls.Add(this.UserScript_InstallDialogButton);
            this.Controls.Add(this.Details_AutoUpdateTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Details_DescriptionTextBox);
            this.Controls.Add(this.Details_DescriptionLabel);
            this.Controls.Add(this.Details_UpdateDateTextBox);
            this.Controls.Add(this.Details_CreatedTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Details_VersionTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Details_RemoveButton);
            this.Controls.Add(this.Details_NameTextBox);
            this.Controls.Add(this.Details_NameLabel);
            this.Controls.Add(this.Details_LoadButton);
            this.Controls.Add(this.Details_CheckUpdateButton);
            this.Controls.Add(this.Details_SourceTextBox);
            this.Controls.Add(this.Details_SourceLabel);
            this.Controls.Add(this.Details_CloneButton);
            this.Controls.Add(this.UserScriptsTable);
            this.Controls.Add(this.Details_EditedTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Details_TitleLabel);
            this.Controls.Add(this.UserScriptListBoxLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(1056, 705);
            this.Name = "UserScriptManager";
            this.Text = "UserScript Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserScriptManager_FormClosing);
            this.Enter += new System.EventHandler(this.UserScriptManager_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.UserScriptsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UserScript_InstallDialogButton;
        private System.Windows.Forms.TextBox Details_AutoUpdateTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Details_DescriptionTextBox;
        private System.Windows.Forms.Label Details_DescriptionLabel;
        private System.Windows.Forms.MaskedTextBox Details_UpdateDateTextBox;
        private System.Windows.Forms.MaskedTextBox Details_CreatedTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Details_VersionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Details_RemoveButton;
        private System.Windows.Forms.TextBox Details_NameTextBox;
        private System.Windows.Forms.Label Details_NameLabel;
        private System.Windows.Forms.Button Details_LoadButton;
        private System.Windows.Forms.Button Details_CheckUpdateButton;
        private System.Windows.Forms.TextBox Details_SourceTextBox;
        private System.Windows.Forms.Label Details_SourceLabel;
        private System.Windows.Forms.Button Details_CloneButton;
        private System.Windows.Forms.DataGridView UserScriptsTable;
        private System.Windows.Forms.MaskedTextBox Details_EditedTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Details_TitleLabel;
        private System.Windows.Forms.Label UserScriptListBoxLabel;
        private System.Windows.Forms.Button UserScript_CreateButton;
        private System.Windows.Forms.Button Details_EditButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserScriptName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn AutoUpdateURL;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AutoUpdate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enable;
        private System.Windows.Forms.Button btnClose;
    }
}
#pragma warning restore CS1591,CA1062
