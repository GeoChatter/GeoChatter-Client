#pragma warning disable CS1591,CA1062
namespace GeoChatter.Controls
{
    partial class StreamerBotActionControl
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
            this.btnSelectAction = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSelectAction
            // 
            this.btnSelectAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAction.Location = new System.Drawing.Point(3, 3);
            this.btnSelectAction.Name = "btnSelectAction";
            this.btnSelectAction.Size = new System.Drawing.Size(227, 26);
            this.btnSelectAction.TabIndex = 0;
            this.btnSelectAction.Text = "Select action";
            this.btnSelectAction.UseVisualStyleBackColor = true;
            this.btnSelectAction.Click += new System.EventHandler(this.btnSelectAction_Click);
            // 
            // StreamerBotActionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSelectAction);
            this.Name = "StreamerBotActionControl";
            this.Size = new System.Drawing.Size(233, 32);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelectAction;
    }
}

#pragma warning restore CS1591,CA1062
