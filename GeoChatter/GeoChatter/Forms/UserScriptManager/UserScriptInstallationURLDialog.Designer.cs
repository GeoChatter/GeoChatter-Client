#pragma warning disable CS1591,CA1062
namespace GeoChatter.Forms
{
    partial class UserScriptInstallationURLDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserScriptInstallationURLDialog));
            this.NewUserScript_URLTextBox = new System.Windows.Forms.TextBox();
            this.NewUserScript_NameLabel = new System.Windows.Forms.Label();
            this.NewUserScript_NameTextBox = new System.Windows.Forms.TextBox();
            this.NewUserScript_URLLabel = new System.Windows.Forms.Label();
            this.NewUserScript_InstallURLButton = new System.Windows.Forms.Button();
            this.NewUserScript_TitleLabel = new System.Windows.Forms.Label();
            this.closeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NewUserScript_URLTextBox
            // 
            this.NewUserScript_URLTextBox.Location = new System.Drawing.Point(80, 77);
            this.NewUserScript_URLTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NewUserScript_URLTextBox.Name = "NewUserScript_URLTextBox";
            this.NewUserScript_URLTextBox.Size = new System.Drawing.Size(279, 23);
            this.NewUserScript_URLTextBox.TabIndex = 52;
            // 
            // NewExtension_NameLabel
            // 
            this.NewUserScript_NameLabel.AutoSize = true;
            this.NewUserScript_NameLabel.Location = new System.Drawing.Point(26, 48);
            this.NewUserScript_NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NewUserScript_NameLabel.Name = "NewExtension_NameLabel";
            this.NewUserScript_NameLabel.Size = new System.Drawing.Size(42, 15);
            this.NewUserScript_NameLabel.TabIndex = 51;
            this.NewUserScript_NameLabel.Text = "Name:";
            // 
            // NewUserScript_NameTextBox
            // 
            this.NewUserScript_NameTextBox.Location = new System.Drawing.Point(80, 45);
            this.NewUserScript_NameTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NewUserScript_NameTextBox.Name = "NewUserScript_NameTextBox";
            this.NewUserScript_NameTextBox.Size = new System.Drawing.Size(279, 23);
            this.NewUserScript_NameTextBox.TabIndex = 50;
            // 
            // NewExtension_URLLabel
            // 
            this.NewUserScript_URLLabel.AutoSize = true;
            this.NewUserScript_URLLabel.Location = new System.Drawing.Point(33, 81);
            this.NewUserScript_URLLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NewUserScript_URLLabel.Name = "NewExtension_URLLabel";
            this.NewUserScript_URLLabel.Size = new System.Drawing.Size(31, 15);
            this.NewUserScript_URLLabel.TabIndex = 49;
            this.NewUserScript_URLLabel.Text = "URL:";
            // 
            // NewUserScript_InstallURLButton
            // 
            this.NewUserScript_InstallURLButton.Location = new System.Drawing.Point(20, 112);
            this.NewUserScript_InstallURLButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NewUserScript_InstallURLButton.Name = "NewUserScript_InstallURLButton";
            this.NewUserScript_InstallURLButton.Size = new System.Drawing.Size(341, 27);
            this.NewUserScript_InstallURLButton.TabIndex = 48;
            this.NewUserScript_InstallURLButton.Text = "Install from URL";
            this.NewUserScript_InstallURLButton.UseVisualStyleBackColor = true;
            this.NewUserScript_InstallURLButton.Click += new System.EventHandler(this.NewUserScript_InstallURLButton_Click);
            // 
            // NewExtension_TitleLabel
            // 
            this.NewUserScript_TitleLabel.Font = new System.Drawing.Font("Segoe UI Light", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.NewUserScript_TitleLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.NewUserScript_TitleLabel.Location = new System.Drawing.Point(14, 10);
            this.NewUserScript_TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NewUserScript_TitleLabel.Name = "NewExtension_TitleLabel";
            this.NewUserScript_TitleLabel.Size = new System.Drawing.Size(346, 31);
            this.NewUserScript_TitleLabel.TabIndex = 47;
            this.NewUserScript_TitleLabel.Text = "New UserScript";
            // 
            // closeBtn
            // 
            this.closeBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeBtn.Location = new System.Drawing.Point(273, 10);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(0, 0);
            this.closeBtn.TabIndex = 53;
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // UserScriptInstallationURLDialog
            // 
            this.AcceptButton = this.NewUserScript_InstallURLButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.closeBtn;
            this.ClientSize = new System.Drawing.Size(374, 145);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.NewUserScript_URLTextBox);
            this.Controls.Add(this.NewUserScript_NameLabel);
            this.Controls.Add(this.NewUserScript_NameTextBox);
            this.Controls.Add(this.NewUserScript_URLLabel);
            this.Controls.Add(this.NewUserScript_InstallURLButton);
            this.Controls.Add(this.NewUserScript_TitleLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(390, 184);
            this.Name = "UserScriptInstallationURLDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserScript Installer (URL)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox NewUserScript_URLTextBox;
        private System.Windows.Forms.Label NewUserScript_NameLabel;
        private System.Windows.Forms.TextBox NewUserScript_NameTextBox;
        private System.Windows.Forms.Label NewUserScript_URLLabel;
        private System.Windows.Forms.Button NewUserScript_InstallURLButton;
        private System.Windows.Forms.Label NewUserScript_TitleLabel;
        private System.Windows.Forms.Button closeBtn;
    }
}
#pragma warning restore CS1591,CA1062
