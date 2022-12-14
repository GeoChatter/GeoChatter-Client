#pragma warning disable CS1591, CA1062

using GeoChatter.Handlers;

namespace GeoChatter.Forms
{
    partial class CommandManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandManager));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.CommandsTable = new System.Windows.Forms.DataGridView();
            this.Bot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserLevel = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Trigger = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CommandName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Aliases = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cooldown = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MessageCooldown = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CooldownTarget = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.AvailableStages = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeveloperBypass = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Enable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.commandManagerRowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CommandsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandManagerRowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.8718F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.1282F));
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 357);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1135, 38);
            this.tableLayoutPanel1.TabIndex = 90;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(4, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(1127, 32);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save Changes";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.CommandsTable, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1135, 357);
            this.tableLayoutPanel2.TabIndex = 91;
            // 
            // CommandsTable
            // 
            this.CommandsTable.AllowUserToAddRows = false;
            this.CommandsTable.AllowUserToDeleteRows = false;
            this.CommandsTable.AllowUserToOrderColumns = true;
            this.CommandsTable.AutoGenerateColumns = false;
            this.CommandsTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CommandsTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.CommandsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CommandsTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Bot,
            this.UserLevel,
            this.Trigger,
            this.CommandName,
            this.Aliases,
            this.Description,
            this.Cooldown,
            this.MessageCooldown,
            this.CooldownTarget,
            this.AvailableStages,
            this.DeveloperBypass,
            this.Enable});
            this.CommandsTable.DataSource = this.commandManagerRowBindingSource;
            this.CommandsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CommandsTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.CommandsTable.Location = new System.Drawing.Point(4, 3);
            this.CommandsTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CommandsTable.Name = "CommandsTable";
            this.CommandsTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CommandsTable.Size = new System.Drawing.Size(1127, 351);
            this.CommandsTable.TabIndex = 91;
            this.CommandsTable.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.CommandsTable_CellBeginEdit);
            this.CommandsTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CommandsTable_CellClick);
            this.CommandsTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CommandsTable_CellContentClick);
            this.CommandsTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.CommandsTable_CellEndEdit);
            this.CommandsTable.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.CommandsTable_DataError);
            // 
            // Bot
            // 
            this.Bot.DataPropertyName = "Bot";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Bot.DefaultCellStyle = dataGridViewCellStyle2;
            this.Bot.FillWeight = 20F;
            this.Bot.HeaderText = "Bot";
            this.Bot.MaxInputLength = 100;
            this.Bot.Name = "Bot";
            this.Bot.ReadOnly = true;
            this.Bot.ToolTipText = "Bot name this command is configured for";
            // 
            // UserLevel
            // 
            this.UserLevel.DataPropertyName = "UserLevel";
            this.UserLevel.FillWeight = 25F;
            this.UserLevel.HeaderText = "User Level";
            this.UserLevel.Name = "UserLevel";
            this.UserLevel.ToolTipText = "Minimum user level to be able to use this command";
            // 
            // Trigger
            // 
            this.Trigger.DataPropertyName = "Trigger";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Trigger.DefaultCellStyle = dataGridViewCellStyle3;
            this.Trigger.FillWeight = 18F;
            this.Trigger.HeaderText = "Trigger";
            this.Trigger.MaxInputLength = 1;
            this.Trigger.Name = "Trigger";
            this.Trigger.ToolTipText = "The character to begin chat messages with before using the command name or aliase" +
    "s (Recommended characters: \"!\", \"/\" , \"?\" )  ";
            // 
            // CommandName
            // 
            this.CommandName.DataPropertyName = "CommandName";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.CommandName.DefaultCellStyle = dataGridViewCellStyle4;
            this.CommandName.FillWeight = 30F;
            this.CommandName.HeaderText = "Command Name";
            this.CommandName.MaxInputLength = 100;
            this.CommandName.Name = "CommandName";
            this.CommandName.ReadOnly = true;
            this.CommandName.ToolTipText = "Fixed command name";
            // 
            // Aliases
            // 
            this.Aliases.DataPropertyName = "Aliases";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.Aliases.DefaultCellStyle = dataGridViewCellStyle5;
            this.Aliases.FillWeight = 45F;
            this.Aliases.HeaderText = "Aliases";
            this.Aliases.MaxInputLength = 1000;
            this.Aliases.Name = "Aliases";
            this.Aliases.ToolTipText = "Alternative names for this command, seperated with spaces";
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.FillWeight = 50F;
            this.Description.HeaderText = "Description";
            this.Description.MaxInputLength = 1000;
            this.Description.Name = "Description";
            this.Description.ToolTipText = "Command information";
            // 
            // Cooldown
            // 
            this.Cooldown.DataPropertyName = "Cooldown";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = "0";
            this.Cooldown.DefaultCellStyle = dataGridViewCellStyle6;
            this.Cooldown.FillWeight = 20F;
            this.Cooldown.HeaderText = "Cooldown";
            this.Cooldown.MaxInputLength = 4;
            this.Cooldown.Name = "Cooldown";
            this.Cooldown.ToolTipText = "Seconds to wait between command executions (Does NOT apply to moderator level & a" +
    "bove)";
            // 
            // MessageCooldown
            // 
            this.MessageCooldown.DataPropertyName = "MessageCooldown";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = "0";
            this.MessageCooldown.DefaultCellStyle = dataGridViewCellStyle7;
            this.MessageCooldown.FillWeight = 30F;
            this.MessageCooldown.HeaderText = "Msg Cooldown";
            this.MessageCooldown.MaxInputLength = 4;
            this.MessageCooldown.Name = "MessageCooldown";
            this.MessageCooldown.ToolTipText = "Seconds to wait between messages sent within the command (Doesn\'t stop command fr" +
    "om executing)";
            // 
            // CooldownTarget
            // 
            this.CooldownTarget.DataPropertyName = "CooldownTarget";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.CooldownTarget.DefaultCellStyle = dataGridViewCellStyle8;
            this.CooldownTarget.FillWeight = 30F;
            this.CooldownTarget.HeaderText = "Cooldown Target";
            this.CooldownTarget.Items.AddRange(new object[] {
            "Global",
            "Individual"});
            this.CooldownTarget.Name = "CooldownTarget";
            this.CooldownTarget.ToolTipText = "Wheter the cooldowns should be checked individually or globally";
            // 
            // AvailableStages
            // 
            this.AvailableStages.DataPropertyName = "AvailableStages";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.AvailableStages.DefaultCellStyle = dataGridViewCellStyle9;
            this.AvailableStages.FillWeight = 30F;
            this.AvailableStages.HeaderText = "Availability";
            this.AvailableStages.Name = "AvailableStages";
            this.AvailableStages.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AvailableStages.ToolTipText = "Game stages (seperated with \',\') allowed for usage: (Anytime, NotInGame, InRound," +
    " RoundEnd, GameEnd)";
            // 
            // DeveloperBypass
            // 
            this.DeveloperBypass.DataPropertyName = "DeveloperBypass";
            this.DeveloperBypass.FillWeight = 25F;
            this.DeveloperBypass.HeaderText = "Bypass For Devs";
            this.DeveloperBypass.Name = "DeveloperBypass";
            this.DeveloperBypass.ToolTipText = "Allow developers to use this command anytime";
            // 
            // Enable
            // 
            this.Enable.DataPropertyName = "Enable";
            this.Enable.FillWeight = 20F;
            this.Enable.HeaderText = "Enable";
            this.Enable.Name = "Enable";
            this.Enable.ToolTipText = "Enable/Disable the command";
            // 
            // commandManagerRowBindingSource
            // 
            this.commandManagerRowBindingSource.DataSource = typeof(GeoChatter.Handlers.CommandManagerRow);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(158, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(0, 0);
            this.btnClose.TabIndex = 92;
            this.btnClose.Text = "button1";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CommandManager
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1135, 395);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommandManager";
            this.Text = "Command Manager";
            this.Load += new System.EventHandler(this.CommandManager_Load);
            this.Enter += new System.EventHandler(this.CommandManager_Enter);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CommandsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandManagerRowBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView CommandsTable;
        private System.Windows.Forms.BindingSource commandManagerRowBindingSource;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bot;
        private System.Windows.Forms.DataGridViewComboBoxColumn UserLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Trigger;
        private System.Windows.Forms.DataGridViewTextBoxColumn CommandName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Aliases;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cooldown;
        private System.Windows.Forms.DataGridViewTextBoxColumn MessageCooldown;
        private System.Windows.Forms.DataGridViewComboBoxColumn CooldownTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailableStages;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DeveloperBypass;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Enable;
    }
}
#pragma warning restore CS1591,CA1062
