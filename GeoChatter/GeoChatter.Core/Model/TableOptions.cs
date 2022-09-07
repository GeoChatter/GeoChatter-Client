using GeoChatter.Model.Enums;
using GeoChatter.Model.Interfaces;
using GeoChatter.Core.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GeoChatter.Model;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Interfaces;
using log4net;

namespace GeoChatter.Core.Model
{
/// <summary>
/// Scoreboard column options models
/// </summary>
    public class TableOptions : ITableOptions
{
        private static readonly ILog logger = LogManager.GetLogger(typeof(TableOptions));

        /// <inheritdoc/>
        public List<GameOptions> Options { get; set; } = new List<GameOptions>();

        /// <summary>
        /// 
        /// </summary>
        public TableOptions()
        {

        }
        /// <inheritdoc/>
        public Tuple<string, ListSortDirection>[] GetFiltersFor(GameMode mode, GameStage stage = GameStage.ENDROUND)
        {
            return Options?
                .First(g => g.Mode == mode.ToStringDefault()).Stages
                .First(s => s.Stage == stage.ToStringDefault()).Columns
                .Where(c => c.Sortable && c.SortIndex >= 0)
                .OrderBy(c => c.SortIndex)
                .Select(c => new Tuple<string, ListSortDirection>(c.DataField, c.SortOrder == "asc" ? ListSortDirection.Ascending : ListSortDirection.Descending))
                .ToArray();
        }

        /// <inheritdoc/>
        public Tuple<string, ListSortDirection>[] GetDefaultFiltersFor(GameMode mode, GameStage stage = GameStage.ENDROUND)
        {
            return Options?
                .First(g => g.Mode == mode.ToStringDefault()).Stages
                .First(s => s.Stage == stage.ToStringDefault()).Columns
                .Where(c => c.Sortable && c.DefaultSortIndex >= 0)
                .OrderBy(c => c.DefaultSortIndex)
                .Select(c => new Tuple<string, ListSortDirection>(c.DataField, c.DefaultSortOrder == "asc" ? ListSortDirection.Ascending : ListSortDirection.Descending))
                .ToArray();
        }

        /// <inheritdoc/>
        public ITableOptions Load()
        {
            try
            {
                string json = File.ReadAllText(Application.StartupPath + "\\TableOptions.json");
                TableOptions opt = JsonConvert.DeserializeObject<TableOptions>(json);
                Options = opt.Options;
                return this;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return this;
            }
        }

        /// <inheritdoc/>
        public void Save()
        {
            new DataStorage(Application.StartupPath).Save(this);
        }
    }
}
