using GeoChatter.Model.Enums;

namespace GeoChatter.Core.Model
{
    /// <summary>
    /// Settings in Overlay tab
    /// </summary>
    public class OverlaySettings
    {
        /// <summary>
        /// Unit of distance
        /// </summary>
        public Units Unit { get; set; }
        /// <summary>
        /// Digits to round distance to
        /// </summary>
        public int RoundingDigits { get; set; }

        /// <summary>
        /// Display correct location flag on top of scoreboard
        /// </summary>
        public bool DisplayCorrectLocations { get; set; }
        /// <summary>
        /// Display incorrect region but correct country guesses with purple background
        /// </summary>
        public bool UseWrongRegionColors { get; set; }
        /// <summary>
        /// Group markers in clusters
        /// </summary>
        public bool MarkerClustersEnabled { get; set; }
        /// <summary>
        /// Display regional flags when available
        /// </summary>
        public bool RegionalFlags { get; set; }
        /// <summary>
        /// Use US state flags for US round and guesses
        /// </summary>
        public bool UsStateFlags { get; set; }
        /// <summary>
        /// Auto scroll speed pixel per second for scoreboard
        /// </summary>
        public int ScrollSpeed { get; set; } = 40;
        /// <summary>
        /// Scoreboard font size value
        /// </summary>
        public string FontSize { get; set; }
        /// <summary>
        /// Scoreboard font size unit
        /// </summary>
        public string FontSizeUnit { get; set; }

        /// <summary>
        /// Scoreboard background #RRGGBB
        /// </summary>
        public string ScoreboardBackground { get; set; }

        /// <summary>
        /// Scoreboard foreground #RRGGBB
        /// </summary>
        public string ScoreboardForeground { get; set; }

        /// <summary>
        /// Scoreboard background alpha value 0-255
        /// </summary>
        public byte ScoreboardBackgroundA { get; set; }
        /// <summary>
        /// Scoreboard foreground alpha value 0-255
        /// </summary>
        public byte ScoreboardForegroundA { get; set; }
        /// <summary>
        /// Google maps info window popup: show coordinates
        /// </summary>
        public bool PopupShowCoordinates { get; set; }
        /// <summary>
        /// Google maps info window popup: show distance
        /// </summary>
        public bool PopupShowDistance { get; set; }
        /// <summary>
        /// Google maps info window popup: show score
        /// </summary>
        public bool PopupShowScore { get; set; }
        /// <summary>
        /// Google maps info window popup: show current streak
        /// </summary>
        public bool PopupShowStreak { get; set; }
        /// <summary>
        /// Google maps info window popup: show time taken
        /// </summary>
        public bool PopupShowTime { get; set; }
        /// <summary>
        /// Number of markers to display on round end
        /// </summary>
        public int MaximumMarkerCountForRoundEnd { get; set; }
        /// <summary>
        /// Display top X players guesses when showing guesses from selected rounds:
        /// </summary>
        public int MaximumRowCountForAllMarkersDisplay { get; set; }
    }
}
