#pragma warning disable CS1591,CA1062
namespace GeoChatter.Forms
{
    partial class UserScriptDetailEditorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserScriptDetailEditorDialog));
            this.Details_EditSaveButton = new System.Windows.Forms.Button();
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
            this.Details_NameTextBox = new System.Windows.Forms.TextBox();
            this.Details_NameLabel = new System.Windows.Forms.Label();
            this.Details_SourceTextBox = new System.Windows.Forms.TextBox();
            this.Details_SourceLabel = new System.Windows.Forms.Label();
            this.Details_EditedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Details_TitleLabel = new System.Windows.Forms.Label();
            this.closeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Details_EditSaveButton
            // 
            this.Details_EditSaveButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Details_EditSaveButton.Location = new System.Drawing.Point(33, 168);
            this.Details_EditSaveButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_EditSaveButton.Name = "Details_EditSaveButton";
            this.Details_EditSaveButton.Size = new System.Drawing.Size(887, 32);
            this.Details_EditSaveButton.TabIndex = 115;
            this.Details_EditSaveButton.Text = "Save changes";
            this.Details_EditSaveButton.UseVisualStyleBackColor = true;
            this.Details_EditSaveButton.Click += new System.EventHandler(this.Details_EditSaveButton_Click);
            // 
            // Details_AutoUpdateTextBox
            // 
            this.Details_AutoUpdateTextBox.Location = new System.Drawing.Point(147, 138);
            this.Details_AutoUpdateTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_AutoUpdateTextBox.Name = "Details_AutoUpdateTextBox";
            this.Details_AutoUpdateTextBox.Size = new System.Drawing.Size(772, 23);
            this.Details_AutoUpdateTextBox.TabIndex = 114;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(29, 142);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 15);
            this.label6.TabIndex = 113;
            this.label6.Text = "Auto-update link";
            // 
            // Details_DescriptionTextBox
            // 
            this.Details_DescriptionTextBox.Location = new System.Drawing.Point(85, 78);
            this.Details_DescriptionTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_DescriptionTextBox.Name = "Details_DescriptionTextBox";
            this.Details_DescriptionTextBox.Size = new System.Drawing.Size(834, 23);
            this.Details_DescriptionTextBox.TabIndex = 112;
            // 
            // Details_DescriptionLabel
            // 
            this.Details_DescriptionLabel.AutoSize = true;
            this.Details_DescriptionLabel.Location = new System.Drawing.Point(29, 82);
            this.Details_DescriptionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Details_DescriptionLabel.Name = "Details_DescriptionLabel";
            this.Details_DescriptionLabel.Size = new System.Drawing.Size(45, 15);
            this.Details_DescriptionLabel.TabIndex = 111;
            this.Details_DescriptionLabel.Text = "Details:";
            // 
            // Details_UpdateDateTextBox
            // 
            this.Details_UpdateDateTextBox.Location = new System.Drawing.Point(300, 108);
            this.Details_UpdateDateTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_UpdateDateTextBox.Mask = "00/00/0000 90:00:00";
            this.Details_UpdateDateTextBox.Name = "Details_UpdateDateTextBox";
            this.Details_UpdateDateTextBox.ReadOnly = true;
            this.Details_UpdateDateTextBox.Size = new System.Drawing.Size(162, 23);
            this.Details_UpdateDateTextBox.TabIndex = 110;
            this.Details_UpdateDateTextBox.Text = "01011970000000";
            this.Details_UpdateDateTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Details_UpdateDateTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // Details_CreatedTextBox
            // 
            this.Details_CreatedTextBox.Location = new System.Drawing.Point(755, 108);
            this.Details_CreatedTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_CreatedTextBox.Mask = "00/00/0000 90:00:00";
            this.Details_CreatedTextBox.Name = "Details_CreatedTextBox";
            this.Details_CreatedTextBox.ReadOnly = true;
            this.Details_CreatedTextBox.Size = new System.Drawing.Size(164, 23);
            this.Details_CreatedTextBox.TabIndex = 97;
            this.Details_CreatedTextBox.Text = "01011970000000";
            this.Details_CreatedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Details_CreatedTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(693, 112);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 15);
            this.label3.TabIndex = 95;
            this.label3.Text = "Created:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 112);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 109;
            this.label2.Text = "Updated:";
            // 
            // Details_VersionTextBox
            // 
            this.Details_VersionTextBox.Location = new System.Drawing.Point(85, 108);
            this.Details_VersionTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_VersionTextBox.Name = "Details_VersionTextBox";
            this.Details_VersionTextBox.ReadOnly = true;
            this.Details_VersionTextBox.Size = new System.Drawing.Size(62, 23);
            this.Details_VersionTextBox.TabIndex = 108;
            this.Details_VersionTextBox.Text = "1.0.0";
            this.Details_VersionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 112);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 107;
            this.label1.Text = "Version:";
            // 
            // Details_NameTextBox
            // 
            this.Details_NameTextBox.Location = new System.Drawing.Point(85, 45);
            this.Details_NameTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_NameTextBox.Name = "Details_NameTextBox";
            this.Details_NameTextBox.Size = new System.Drawing.Size(376, 23);
            this.Details_NameTextBox.TabIndex = 105;
            // 
            // Details_NameLabel
            // 
            this.Details_NameLabel.AutoSize = true;
            this.Details_NameLabel.Location = new System.Drawing.Point(29, 48);
            this.Details_NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Details_NameLabel.Name = "Details_NameLabel";
            this.Details_NameLabel.Size = new System.Drawing.Size(42, 15);
            this.Details_NameLabel.TabIndex = 104;
            this.Details_NameLabel.Text = "Name:";
            // 
            // Details_SourceTextBox
            // 
            this.Details_SourceTextBox.Location = new System.Drawing.Point(533, 45);
            this.Details_SourceTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_SourceTextBox.Name = "Details_SourceTextBox";
            this.Details_SourceTextBox.ReadOnly = true;
            this.Details_SourceTextBox.Size = new System.Drawing.Size(386, 23);
            this.Details_SourceTextBox.TabIndex = 101;
            this.Details_SourceTextBox.Text = "URL or <editor>";
            // 
            // Details_SourceLabel
            // 
            this.Details_SourceLabel.AutoSize = true;
            this.Details_SourceLabel.Location = new System.Drawing.Point(477, 48);
            this.Details_SourceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Details_SourceLabel.Name = "Details_SourceLabel";
            this.Details_SourceLabel.Size = new System.Drawing.Size(46, 15);
            this.Details_SourceLabel.TabIndex = 100;
            this.Details_SourceLabel.Text = "Source:";
            // 
            // Details_EditedTextBox
            // 
            this.Details_EditedTextBox.Location = new System.Drawing.Point(523, 108);
            this.Details_EditedTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Details_EditedTextBox.Mask = "00/00/0000 90:00:00";
            this.Details_EditedTextBox.Name = "Details_EditedTextBox";
            this.Details_EditedTextBox.ReadOnly = true;
            this.Details_EditedTextBox.Size = new System.Drawing.Size(163, 23);
            this.Details_EditedTextBox.TabIndex = 98;
            this.Details_EditedTextBox.Text = "01011970000000";
            this.Details_EditedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Details_EditedTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(469, 112);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 15);
            this.label4.TabIndex = 96;
            this.label4.Text = "Edited:";
            // 
            // Details_TitleLabel
            // 
            this.Details_TitleLabel.Font = new System.Drawing.Font("Segoe UI Light", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Details_TitleLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Details_TitleLabel.Location = new System.Drawing.Point(14, 10);
            this.Details_TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Details_TitleLabel.Name = "Details_TitleLabel";
            this.Details_TitleLabel.Size = new System.Drawing.Size(523, 31);
            this.Details_TitleLabel.TabIndex = 94;
            this.Details_TitleLabel.Text = "UserScript Details ";
            // 
            // closeBtn
            // 
            this.closeBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeBtn.Location = new System.Drawing.Point(832, 10);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(0, 0);
            this.closeBtn.TabIndex = 116;
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // UserScriptDetailEditorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.closeBtn;
            this.ClientSize = new System.Drawing.Size(933, 223);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.Details_EditSaveButton);
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
            this.Controls.Add(this.Details_NameTextBox);
            this.Controls.Add(this.Details_NameLabel);
            this.Controls.Add(this.Details_SourceTextBox);
            this.Controls.Add(this.Details_SourceLabel);
            this.Controls.Add(this.Details_EditedTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Details_TitleLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "UserScriptDetailEditorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserScript Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Details_EditSaveButton;
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
        private System.Windows.Forms.TextBox Details_NameTextBox;
        private System.Windows.Forms.Label Details_NameLabel;
        private System.Windows.Forms.TextBox Details_SourceTextBox;
        private System.Windows.Forms.Label Details_SourceLabel;
        private System.Windows.Forms.MaskedTextBox Details_EditedTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Details_TitleLabel;
        private System.Windows.Forms.Button closeBtn;
    }
}
#pragma warning restore CS1591,CA1062
