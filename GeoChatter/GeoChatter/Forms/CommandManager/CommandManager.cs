using GeoChatter.Core.Interfaces;
using GeoChatter.Core.Model;
using GeoChatter.Model.Enums;
using GeoChatter.Core.Storage;
using GeoChatter.FormUtils;
using GeoChatter.Handlers;
using GeoChatter.Web;
using GeoChatter.Web.Twitch;
using GeoChatter.Web.YouTube;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GeoChatter.Core;

namespace GeoChatter.Forms
{
    /// <summary>
    /// Command manager form
    /// </summary>
    public partial class CommandManager : Form
    {
        private const char AliasSeperator = ' ';
        private const string CommandsPath = "commands";
        private const string MetaFile = "metadata.json";

        private static DataStorage storage { get; } = new DataStorage(CommandsPath, ".json");

        /// <summary>
        /// List of all commands available
        /// </summary>
        public static List<ICommandBase> Commands { get; } = new();

        private static Dictionary<string, int> UserLevels { get; } = new()
        {
            { nameof(CommonUserLevel.Viewer), (int)CommonUserLevel.Viewer },
            { nameof(CommonUserLevel.Moderator), (int)CommonUserLevel.Moderator },
            { nameof(CommonUserLevel.Broadcaster), (int)CommonUserLevel.Broadcaster },
        };

        private static bool InitializedCommands { get; set; }

        private MainForm mainForm { get; set; }

        private DataTable datatable { get; } = new();
        /// <summary>
        /// 
        /// </summary>
        public CommandManager()
        {
            InitializeComponent();
            LoadCommandsAndMetas();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public CommandManager(MainForm parent) : this()
        {
            mainForm = parent;

            UserLevel.Items.AddRange(UserLevels.Keys.ToArray());

            datatable.Columns.Add(nameof(CommandManagerRow.Bot), typeof(string));
            datatable.Columns.Add(nameof(CommandManagerRow.UserLevel), typeof(string));
            datatable.Columns.Add(nameof(CommandManagerRow.Trigger), typeof(char));
            datatable.Columns.Add(nameof(CommandManagerRow.CommandName), typeof(string));
            datatable.Columns.Add(nameof(CommandManagerRow.Aliases), typeof(string));
            datatable.Columns.Add(nameof(CommandManagerRow.Description), typeof(string));
            datatable.Columns.Add(nameof(CommandManagerRow.Cooldown), typeof(double));
            datatable.Columns.Add(nameof(CommandManagerRow.MessageCooldown), typeof(double));
            datatable.Columns.Add(nameof(CommandManagerRow.CooldownTarget), typeof(string));
            datatable.Columns.Add(nameof(CommandManagerRow.AvailableStages), typeof(string));
            datatable.Columns.Add(nameof(CommandManagerRow.DeveloperBypass), typeof(bool));
            datatable.Columns.Add(nameof(CommandManagerRow.Enable), typeof(bool));

            datatable.Locale = System.Globalization.CultureInfo.InvariantCulture;

            Commands.ForEach(cmd => AddCommandToTable(datatable, cmd));

            CommandsTable.DataSource = new BindingSource() { DataSource = datatable };

#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
            mainForm?.ResetJSCTRLCheck();
#pragma warning restore CA1416 // Validate platform compatibility
#endif
        }

        private static void CreateCommandsFromLocal(string file, string script)
        {
            // TODO: custom commands
        }

        private static void InitializeCommands()
        {
            Commands.Clear();

            Commands.AddRange(TwitchBot.Commands);
            //  Commands.AddRange(YoutubeBot.Commands);
        }
        /// <summary>
        /// Initialize <see cref="Commands"/> and load command meta data
        /// </summary>
        public static void LoadCommandsAndMetas()
        {
            if (!InitializedCommands)
            {
                InitializedCommands = true;
                InitializeCommands();
            }
            //Dictionary<string, string> cmds = storage.ReadAllFiles();

            // TODO: Get custom commands from local
            List<CommandMeta> metas = GetMetas();

            foreach (CommandMeta met in metas)
            {
                ICommandBase c = Commands.FirstOrDefault(cmd => cmd.Meta.Name == met.CommandName && GetBotNameOfCommand(cmd) == met.BotName);
                if (c != null)
                {
                    SetCommandFromMeta(c, met);
                }
            }
        }

        private static void SetCommandFromMeta(ICommandBase cmd, CommandMeta meta)
        {
            if (meta == null)
            {
                return;
            }

            cmd.Restrictions.AccessLevel = GetUserLevelFromNameForCommand(meta.UserLevel);
            cmd.TriggerChar = meta.TriggerChar;
            cmd.CommandNames = new List<string>() { meta.CommandName };
            cmd.CommandNames.AddRange(meta.Aliases);
            cmd.Description = meta.Description ?? string.Empty;
            cmd.Restrictions.CommandCooldownSeconds = meta.Cooldown;
            cmd.Restrictions.MessageCooldownSeconds = meta.MessageCooldown;
            cmd.Restrictions.CanDeveloperBypass = meta.CanDeveloperBypass;
            cmd.Restrictions.AllowedState = (AppGameState)meta.AllowedState;
            cmd.Restrictions.CooldownTarget = Enum.TryParse(meta.CooldownTarget, out Core.Attributes.CooldownTarget res) ? res : Core.Attributes.CooldownTarget.Individual;
            cmd.IsEnabled = meta.IsEnabled;
        }


        private static CommandMeta SetMetaFromCommand(ICommandBase cmd)
        {
            CommandMeta meta = new()
            {
                CommandName = cmd.Meta.Name,
                Cooldown = cmd.Restrictions.CommandCooldownSeconds,
                MessageCooldown = cmd.Restrictions.MessageCooldownSeconds,
                CanDeveloperBypass = cmd.Restrictions.CanDeveloperBypass,
                AllowedState = (int)cmd.Restrictions.AllowedState,
                CooldownTarget = Enum.GetName(cmd.Restrictions.CooldownTarget) ?? nameof(Core.Attributes.CooldownTarget.Individual),
                BotName = GetBotNameOfCommand(cmd),
                UserLevel = GetUserLevelFromCommand(cmd),
                TriggerChar = cmd.TriggerChar,
                Description = cmd.Description,
                IsEnabled = cmd.IsEnabled
            };
            meta.Aliases.AddRange(cmd.CommandNames.Where(n => n != cmd.Meta.Name).ToList());

            return meta;
        }

        private static void SetMetas()
        {
            List<CommandMeta> metas = new();

            Commands.ForEach(cmd => metas.Add(SetMetaFromCommand(cmd)));

            string metaData = JsonConvert.SerializeObject(metas);
            storage.WriteOtherFile(MetaFile, metaData);
        }

        private static List<CommandMeta> GetMetas()
        {
            string meta = storage.ReadOtherFile(MetaFile);
            if (string.IsNullOrWhiteSpace(meta))
            {
                return new();
            }

            List<CommandMeta> metaData = JsonConvert.DeserializeObject<List<CommandMeta>>(meta);

            return metaData;
        }

        private static string GetBotNameOfCommand(ICommandBase cmd)
        {
            return cmd is TwitchCommand t ? nameof(TwitchBot) : cmd is YoutubeCommand y ? nameof(YoutubeBot) : "Custom";
        }

        private static string GetUserLevelFromCommand(ICommandBase cmd)
        {
            return cmd is TwitchCommand t
                ? UserLevels.FirstOrDefault(p => p.Value == t.Restrictions.AccessLevel).Key ?? string.Empty
                : cmd is YoutubeCommand y
                    ? UserLevels.FirstOrDefault(p => p.Value == y.Restrictions.AccessLevel).Key ?? string.Empty
                    : throw new NotImplementedException("Unknown command type " + cmd.GetType().Name);
        }

        private void AddCommandToTable(DataTable tbl, ICommandBase cmd)
        {
            if (CommandsTable == null)
            {
                return;
            }

            tbl.LoadDataRow(new object[]
            {
                GetBotNameOfCommand(cmd),
                GetUserLevelFromCommand(cmd),
                cmd.TriggerChar,
                cmd.Meta.Name,
                string.Join(AliasSeperator, cmd.CommandNames.Where(n => n != cmd.Meta.Name)),
                cmd.Description,
                cmd.Restrictions.CommandCooldownSeconds,
                cmd.Restrictions.MessageCooldownSeconds,
                Enum.GetName(cmd.Restrictions.CooldownTarget),
                string.Join(",", Enum.GetNames(typeof(AppGameState)).Where(n => (Enum.Parse<AppGameState>(n) & cmd.Restrictions.AllowedState) > 0).Distinct()),
                cmd.Restrictions.CanDeveloperBypass,
                cmd.IsEnabled
            }, true);
            //SetCommandToTableCells(CommandsTable.Rows[CommandsTable.Rows.Add()].Cells, cmd);
        }

        private int GetCommandIndex(ICommandBase cmd, string nameoverride = "")
        {
            string name = string.IsNullOrWhiteSpace(nameoverride) ? cmd.Meta.Name : nameoverride;
            for (int i = 0; i < CommandsTable.RowCount; i++)
            {
                if (CommandsTable.Rows[i].Cells[nameof(CommandManagerRow.CommandName)].Value.ToString().Trim() == name
                    && GetBotNameOfCommand(cmd) == CommandsTable.Rows[i].Cells[nameof(CommandManagerRow.Bot)].Value.ToString())
                {
                    return i;
                }
            }

            return -1;
        }

        private static int GetUserLevelFromNameForCommand(string name)
        {
            return UserLevels[name];
        }

        private static bool SaveCommandFromDatatable(ICommandBase cmd, DataGridViewCellCollection cells)
        {
            try
            {
                cmd.Restrictions.AccessLevel = GetUserLevelFromNameForCommand(cells[nameof(CommandManagerRow.UserLevel)].Value?.ToString());

                cmd.TriggerChar = string.IsNullOrEmpty(cells[nameof(CommandManagerRow.Trigger)].Value?.ToString()) ? cmd.TriggerChar : cells[nameof(CommandManagerRow.Trigger)].Value.ToString()[0];

                cmd.Description = cells[nameof(CommandManagerRow.Description)].Value?.ToString() ?? string.Empty;

                cmd.CommandNames.Clear();
                cmd.CommandNames.Add(cmd.Meta.Name);
                if (!string.IsNullOrEmpty(cells[nameof(CommandManagerRow.Aliases)].Value?.ToString()))
                {
                    cmd.CommandNames.AddRange(cells[nameof(CommandManagerRow.Aliases)].Value.ToString().Trim().Split(AliasSeperator));
                }

                if (!string.IsNullOrEmpty(cells[nameof(CommandManagerRow.Cooldown)].Value?.ToString()))
                {
                    double old = cmd.Restrictions.CommandCooldownSeconds;
                    cmd.Restrictions.CommandCooldownSeconds = (double)cells[nameof(CommandManagerRow.Cooldown)].Value;
                    if (cmd.Restrictions.CommandCooldownSeconds < 0)
                    {
                        cmd.Restrictions.CommandCooldownSeconds = old;
                    }
                }

                if (!string.IsNullOrEmpty(cells[nameof(CommandManagerRow.MessageCooldown)].Value?.ToString()))
                {
                    double old = cmd.Restrictions.MessageCooldownSeconds;
                    cmd.Restrictions.MessageCooldownSeconds = (double)cells[nameof(CommandManagerRow.MessageCooldown)].Value;
                    if (cmd.Restrictions.MessageCooldownSeconds < 0)
                    {
                        cmd.Restrictions.MessageCooldownSeconds = old;
                    }
                }

                List<string> stg = ((string)cells[nameof(CommandManagerRow.AvailableStages)].Value).Split(",").Select(x => x.Trim()).ToList();

                cmd.Restrictions.CooldownTarget = Enum.TryParse((string)cells[nameof(CommandManagerRow.CooldownTarget)].Value, out Core.Attributes.CooldownTarget res) ? res : cmd.Restrictions.CooldownTarget;
                cmd.Restrictions.AllowedState = stg.Count == 0 ? cmd.Restrictions.AllowedState : stg.Select(s => Enum.TryParse(s.ToUpperInvariant(), out AppGameState r) ? r : 0).Aggregate((p, c) => p | c);
                cmd.Restrictions.CanDeveloperBypass = (bool)cells[nameof(CommandManagerRow.DeveloperBypass)].Value;
                cmd.IsEnabled = (bool)cells[nameof(CommandManagerRow.Enable)].Value;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save command: '{cmd.Meta.Name}'\n{ex.Message}", "Save Aborted", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (ICommandBase cmd in Commands)
            {
                int i = GetCommandIndex(cmd);
                if (i == -1)
                {
                    cmd.IsEnabled = false;
                }
                else if (!SaveCommandFromDatatable(cmd, CommandsTable.Rows[i].Cells))
                {
                    return;
                }
            }

            SetMetas();
            Close();
        }

        private void CommandManager_Load(object sender, EventArgs e)
        {

        }

        private void CommandsTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void CommandsTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CommandsTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CommandsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void CommandsTable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;
            MessageBox.Show("Invalid value type. Expected type: " + datatable.Columns[e.ColumnIndex].DataType.GetFriendlyName(), "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CommandManager_Enter(object sender, EventArgs e)
        {
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
            mainForm?.ResetJSCTRLCheck();
#pragma warning restore CA1416 // Validate platform compatibility
#endif
        }
    }
}
