import { Color } from "./colors";
import { Enum } from "./enums"

export namespace Setting
{
    export const ScoreboardDisplay: TableSettingCache =
    {
        [Enum.GAMEMODE.DEFAULT]: {
            [Enum.GAMESTAGE.INROUND]: {
                Width: 550,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            },
            [Enum.GAMESTAGE.ENDROUND]: {
                Width: 650,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            },
            [Enum.GAMESTAGE.ENDGAME]: {
                Width: 770,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            },
            [Enum.GAMESTAGE.IDLE]: {
                Width: 550,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            },
            [Enum.GAMESTAGE.EXITED]: {
                Width: 550,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            }
        },
        [Enum.GAMEMODE.STREAK]: {
            [Enum.GAMESTAGE.INROUND]: {
                Width: 550,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            },
            [Enum.GAMESTAGE.ENDROUND]: {
                Width: 650,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            },
            [Enum.GAMESTAGE.ENDGAME]: {
                Width: 770,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            },
            [Enum.GAMESTAGE.IDLE]: {
                Width: 550,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            },
            [Enum.GAMESTAGE.EXITED]: {
                Width: 550,
                Height: 400,
                Top: 10,
                Left: 5,
                IsMinimized: false,
                ShowRowNumbers: false,
                MinimapLayer: "roadmap"
            }
        }
    }

    export const App: IAppSettingTable =
    {
        PreferredExportFormat: Enum.ExportFormat.xlsx
    }

    export const Overlay: OverlaySettings = {
        /** Distance units to use
         *  @type number*/
        Unit: Enum.UNIT.KM,
        /** Display correct location flags on scoreboard summaries
         *  @type boolean*/
        DisplayCorrectLocations: true,
        /** Digits to round distances to
         *  @type number*/
        RoundingDigits: 3,
        /** Use regional flags when possible
         *  @type boolean*/
        RegionalFlags: false,
        /** Use different flag background for incorrect region
         *  @type boolean*/
        UseWrongRegionColors: true,
        /**
         * Scoreboard font size
         *  @type string*/
        FontSize: "1",
        /**
         * Scoreboard font size unit
         *  @type string*/
        FontSizeUnit: "em",
        /**
         * Scoreboard auto scrolling speed
         *  @type number*/
        ScrollSpeed: 40,

        ScoreboardForeground: "#FFFFFF",
        ScoreboardForegroundA: 255,

        ScoreboardBackground: "#000000",
        ScoreboardBackgroundA: 85,

        /*
         * Guess marker size in pixels
         */
        MarkerSize: 32,
        /*
         * Guess marker border width in pixels
         */
        MarkerBorderSize: 2,

        /*
         * Maximum radius in meters to search for streetview for embeds
         */
        StreetViewMaxRadius: 2000,

        PopupShowCoordinates: true,
        PopupShowStreak: true,
        PopupShowDistance: true,
        PopupShowScore: true,
        PopupShowTime: true,

        /*
         * Markers related 
         */
        MaximumMarkerCountForRoundEnd: 50,
        MaximumRowCountForAllMarkersDisplay: 40,
        MarkerClustersEnabled: true,

        CreatePolylines: true,

        RandomGuessCharacter: "*",

        GetRandomGuessIndicator()
        {
            return `<span title='Used random guess'>${Color.ColorUsername(this.ScoreboardForeground, this.RandomGuessCharacter)}</span>`
        }
    };

}

export namespace State
{

    export const Scoreboard: IScoreboardStateChecklist =
    {
        ShouldUseTableOptionsWidth: false,
        ShouldUseLastScoreBoardSettings: false,
        SubtitleClickReady: true,
        DisplayingCurrentStandings: false,
        IsScrolling: true,
        IsScrollingEnabled: true,
    }

    export const Event: IEventStateChecklist =
    {
        AddedClick: false,
        AddedClickEnd: false,
        AddedClickViewSummary: false,
        AddedStreakMarkerListener: false
    }

    export const App: IAppStateChecklist =
    {
        IsCTRLDown: false,
        LoadCompleted: false,
        PreferredMultiguess: false,
        StartedCurrentGame: false,
        ClosingTheRound: false,
        CheckEventAdded: false,
        AlertExportSuccess: true,
        AutoExportGameResults: false,
        AutoExportRoundResults: false,
        AutoExportRoundStandings: false,
        PlayAgainEnabled: true,
        Attempting_CreateInitialTable: false,
        Attempting_RecreateMissingScoreboard: false,
        NextStreamerGuessIsRandom: false,
        HideMarkerState: true,
        ProcessingGuess: false
    }

}

window.GC.Setting = Setting;
window.GC.State = State;