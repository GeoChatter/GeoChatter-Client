#pragma warning disable CS1591,CA1062
namespace GeoChatter.Controls
{
    partial class ShortcutRecorderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEditSave = new System.Windows.Forms.Button();
            this.txtShortcut = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEditSave
            // 
            this.btnEditSave.Location = new System.Drawing.Point(165, 1);
            this.btnEditSave.Name = "btnEditSave";
            this.btnEditSave.Size = new System.Drawing.Size(75, 23);
            this.btnEditSave.TabIndex = 0;
            this.btnEditSave.Text = "Edit";
            this.btnEditSave.UseVisualStyleBackColor = true;
            this.btnEditSave.Click += new System.EventHandler(this.btnEditSave_Click);
            // 
            // txtShortcut
            // 
            this.txtShortcut.Enabled = false;
            this.txtShortcut.Location = new System.Drawing.Point(3, 3);
            this.txtShortcut.Name = "txtShortcut";
            this.txtShortcut.ReadOnly = true;
            this.txtShortcut.Size = new System.Drawing.Size(156, 20);
            this.txtShortcut.TabIndex = 1;
            this.txtShortcut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            this.txtShortcut.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBox1_PreviewKeyDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(246, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ShortcutRecorderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtShortcut);
            this.Controls.Add(this.btnEditSave);
            this.Name = "ShortcutRecorderControl";
            this.Size = new System.Drawing.Size(314, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEditSave;
        private System.Windows.Forms.TextBox txtShortcut;
        private System.Windows.Forms.Button btnCancel;
    }
}

#pragma warning restore CS1591,CA1062
