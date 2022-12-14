#pragma warning disable CS1591,CA1062
namespace GeoChatter.Forms.FlagManager
{
    partial class FlagManagerDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlagManagerDialog));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.installPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.grpMapPack = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.linkLblDownloadPacks = new System.Windows.Forms.LinkLabel();
            this.btnUnInstall = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.coll1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installPackToolStripMenuItem,
            this.deletePackToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(136, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // installPackToolStripMenuItem
            // 
            this.installPackToolStripMenuItem.Name = "installPackToolStripMenuItem";
            this.installPackToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.installPackToolStripMenuItem.Text = "Install pack";
            this.installPackToolStripMenuItem.Click += new System.EventHandler(this.installPackToolStripMenuItem_Click);
            // 
            // deletePackToolStripMenuItem
            // 
            this.deletePackToolStripMenuItem.Name = "deletePackToolStripMenuItem";
            this.deletePackToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.deletePackToolStripMenuItem.Text = "Delete pack";
            this.deletePackToolStripMenuItem.Click += new System.EventHandler(this.deletePackToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.55704F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.38272F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.16049F));
            this.tableLayoutPanel1.Controls.Add(this.treeView1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpMapPack, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.linkLblDownloadPacks, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnUnInstall, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 462F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(810, 572);
            this.tableLayoutPanel1.TabIndex = 4;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(3, 43);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(243, 456);
            this.treeView1.TabIndex = 5;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // grpMapPack
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.grpMapPack, 2);
            this.grpMapPack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMapPack.Location = new System.Drawing.Point(274, 43);
            this.grpMapPack.Name = "grpMapPack";
            this.grpMapPack.Size = new System.Drawing.Size(533, 456);
            this.grpMapPack.TabIndex = 6;
            this.grpMapPack.TabStop = false;
            this.grpMapPack.Text = "Map pack";
            // 
            // btnClose
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnClose, 3);
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(3, 505);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(804, 39);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // linkLblDownloadPacks
            // 
            this.linkLblDownloadPacks.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.linkLblDownloadPacks, 3);
            this.linkLblDownloadPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLblDownloadPacks.Location = new System.Drawing.Point(3, 547);
            this.linkLblDownloadPacks.Name = "linkLblDownloadPacks";
            this.linkLblDownloadPacks.Size = new System.Drawing.Size(804, 25);
            this.linkLblDownloadPacks.TabIndex = 9;
            this.linkLblDownloadPacks.TabStop = true;
            this.linkLblDownloadPacks.Text = "Download official flag packs manually";
            this.linkLblDownloadPacks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLblDownloadPacks.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLblDownloadPacks_LinkClicked);
            // 
            // btnUnInstall
            // 
            this.btnUnInstall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnInstall.Location = new System.Drawing.Point(673, 3);
            this.btnUnInstall.Name = "btnUnInstall";
            this.btnUnInstall.Size = new System.Drawing.Size(134, 34);
            this.btnUnInstall.TabIndex = 8;
            this.btnUnInstall.Text = "Install from .zip";
            this.btnUnInstall.UseVisualStyleBackColor = true;
            this.btnUnInstall.Click += new System.EventHandler(this.btnUnInstall_Click);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(534, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 34);
            this.button1.TabIndex = 9;
            this.button1.Text = "Install official packs";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // coll1
            // 
            this.coll1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.coll1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("coll1.ImageStream")));
            this.coll1.TransparentColor = System.Drawing.Color.Transparent;
            this.coll1.Images.SetKeyName(0, "icon_smaller.ico");
            // 
            // FlagManagerDialog
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(810, 572);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FlagManagerDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Flag Manager";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FlagManagerDialog_FormClosed);
            this.Load += new System.EventHandler(this.FlagManager_Load);
            this.Enter += new System.EventHandler(this.FlagManagerDialog_Enter);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem installPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deletePackToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList coll1;
        private System.Windows.Forms.GroupBox grpMapPack;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnUnInstall;
        private System.Windows.Forms.LinkLabel linkLblDownloadPacks;
        private System.Windows.Forms.Button button1;
    }
}
#pragma warning restore CS1591,CA1062
