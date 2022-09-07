/// <reference path="../node_modules/devextreme/dist/ts/dx.all.d.ts" />
import { MarkerClusterer } from "@googlemaps/markerclusterer"
import { Enum } from "./enums"

declare export global
{
    /** T may be any object or an empty object */
    export type Emptyable<T> = T | object | {}

    /** T may be undefined or null */
    export type Nullable<T> = T | undefined | null

    /** Scheme handler settings */
    export type Scheme = {
        /** Name of the handler protocol */
        Name: string,
        /** Parameter to trigger a source file reload */
        RefreshParam: string,
        /** Dynamic loading from unknown files from filesystem allowed state */
        Dynamic: boolean
    }

    /** A named cache of non nullable 'Value's*/
    export type CustomCache<Value> = {
        [key: string]: NonNullable<Value>
    }

    /** Storage of markers and polylines intended for interval id caching */
    export type MarkerLineIntervalStore = {
        [key in MarkerLineAccessKey]?: CustomCache<number>
    }

    /** Storage of marker clusters */
    export type MarkerClusterStore = {
        [key in MarkerLineAccessKey]?: MarkerClusterer
    }

    /** Array storage of markers and polylines */
    export type MarkerLineArrayStore<T> = {
        [key in MarkerLineAccessKey]?: Array<T>
    }

    /** Storage of marker and polylines by given types mapped to available access keys */
    export type MarkerLineNamedStore<Marker, Line> = {
        [key in MarkerLineAccessKey]?: MarkerLineStore<Marker, Line>
    }

    /** Storage of marker and polylines by given types */
    export type MarkerLineStore<Marker, Line> = {
        /** Markers array cached by name */
        Markers: CustomCache<Array<NonNullable<Marker>>>,
        /** Polylines array cached by name */
        Lines: CustomCache<Array<NonNullable<Line>>>
    }

    /** Source containing displayable flags */
    export type FlagSource = {
        /** Flag code */
        FlagCode: Nullable<string>,
        /** Flag name */
        FlagName: Nullable<string>,
        /** Flag display span */
        FlagDisplay: Nullable<string>
    }

    /** Coordinates */
    export type Coordinate = {
        /** Latitude */
        Latitude: number,
        /** Longitude */
        Longitude: number,
    }

    /** Source containing panorama data */
    export type PanoramaSource = {
        /** Panorama unique identifier */
        Pano: Nullable<string> = "";
        /** Wheter or not the pano id was overwritten after round start */
        PanoOverwritten: boolean = false;
        /** Heading angle */
        Heading: number = 0;
        /** Pitch angle */
        Pitch: number = 0;
        /** FOV */
        FOV: number = 180;
        /** Zoom level */
        Zoom: number = 0;
    } & Coordinate

    /** Feature containing regional data */
    export type Feature = {
        /** ADM-0 Common Name */
        CountryName: string,
        /** ADM-0 Code */
        CountryCode: string,

        /** ADM-1 Common Name */
        ExactCountryName: string,
        /** ADM-1 Code */
        ExactCountryCode: string,
    }

    /** Player data */
    export type PlayerData = {
        /** User id */
        Id: string,
        /** Platform */
        Platform: PlayerPlatform,
        /** Username */
        Name: string,
        /** Display name */
        Display: string,
        /** Wheter user is the streamer */
        IsStreamer: boolean,
        /** Marker URL */
        MarkerImage: Nullable<string>,
        /** Marker URI */
        MarkerData: Nullable<string>,
        /** Username color */
        Color: Nullable<string>,
        /** Personal bests */
        Bests: Nullable<PlayerStats>
    } & FlagSource

    /** Guess or correct location */
    export type LocationSource = Feature & PanoramaSource

    /** Table options base */
    export type GameTableOptions = {
        [mode in GAMEMODE]: GameModeOptions
    }

    /** Table options by game mode */
    export type GameModeOptions = {
        [stage in GAMESTAGE]: GameStageOptions
    }

    /** Table options by game stage */
    export type GameStageOptions = {
        [field in DataField]: GameTableColumnOptions
    }

    /** Table options by column */
    export type GameTableColumnOptions = {
        /** Column position */
        Position: number,
        /** Column data field */
        readonly DataField: DataField,
        /** Column title */
        Name: string,
        /** Column width */
        Width: number,
        /** Column visibility */
        Visible: boolean,
        /** Wheter column is sortable */
        Sortable: boolean,
        /** Column sort index on the table */
        SortIndex: number,
        /** Column sort order/direction */
        SortOrder: Nullable<DevExpress.common.SortOrder> | "",
        /** Default column sort index on the table */
        readonly DefaultSortIndex: number,
        /** Default column sort order/direction */
        readonly DefaultSortOrder: Nullable<DevExpress.common.SortOrder> | "",
        /** Wheter column is available in multiguess rounds */
        readonly AllowedWithMultiGuess: boolean,
    }

    /** Table options sent to C# serialized */
    export type TableOptionsClassJSON = {
        Mode: GAMEMODE,
        Stages: Array<TableOptionsClassStage>
    }

    /** Scoreboard interactive options by game mode */
    export type TableSettingCache = {
        [mode in GAMEMODE]: TableSettingCacheStage
    }

    /** Scoreboard interactive options by game stage */
    export type TableSettingCacheStage = {
        [stage in GAMESTAGE]: TableSettingCacheOptions
    }

    /** Scoreboard interactive options */
    export type TableSettingCacheOptions = {
        /** Offset from top in pixels */
        Top: number,
        /** Offset from left in pixels */
        Left: number,
        /** Width in pixels */
        Width: number,
        /** Height in pixels */
        Height: number,
        /** Wheter scoreboard rows are hidden */
        IsMinimized: boolean,
        /**  Wheter scoreboard has row numbers */
        ShowRowNumbers: boolean,
        /** Minimap layer preference */
        MinimapLayer: string,
    }

    /** Scoreboard interactive and column options */
    export type TableOptionsClassStage = {
        /** Game stage */
        Stage: GAMESTAGE,
        /** Column options */
        Columns: Array<GameTableColumnOptions>
    } & TableSettingCacheOptions

    /** Table row containing player data */
    export type TableRowPlayerData =
    {
        /** Player that this row belongs to */
        Player: PlayerData
    };

    /** Table row wrapper for each row in scoreboard */
    export type TableRow = {
        [row in DataField]?: TableCell
    } & TableRowPlayerData

    /** Cell of a TableRow */
    export type TableCell = {
        /** HTML to be displayed as the cell value */
        display: string,
        /** Value used while sorting the column of this cell for comparison to other cells */
        sort: number | string
    }

    /** Overlay related settings */
    export type OverlaySettings = {
        /** Distance units to use
         *  @type number*/
        Unit: UNIT,
        /** Display correct location flags on scoreboard summaries
         *  @type boolean*/
        DisplayCorrectLocations: boolean,
        /** Digits to round distances to
         *  @type number*/
        RoundingDigits: number,
        /** Use regional flags when possible
         *  @type boolean*/
        RegionalFlags: boolean,
        /** Use different flag background for incorrect region
         *  @type boolean*/
        UseWrongRegionColors: boolean,
        /**
         * Scoreboard font size
         *  @type string*/
        FontSize: string,
        /**
         * Scoreboard font size unit
         *  @type string*/
        FontSizeUnit: string,
        /**
         * Scoreboard auto scrolling speed
         *  @type number*/
        ScrollSpeed: number,

        /** Scoreboard text color RGB */
        ScoreboardForeground: string,
        /** Scoreboard text color Alpha */
        ScoreboardForegroundA: number,

        /** Scoreboard background color RGB */
        ScoreboardBackground: string,
        /** Scoreboard background color Alpha */
        ScoreboardBackgroundA: number,

        /**
         * Guess marker size in pixels
         */
        MarkerSize: number,
        /**
         * Guess marker border width in pixels
         */
        MarkerBorderSize: number,

        /**
         * Maximum radius in meters to search for streetview for embeds
         */
        StreetViewMaxRadius: number,

        /** Wheter to show coordinates in infowindow popup */
        PopupShowCoordinates: boolean,
        /** Wheter to show streaks in infowindow popup */
        PopupShowStreak: boolean,
        /** Wheter to show distance in infowindow popup */
        PopupShowDistance: boolean,
        /** Wheter to show score in infowindow popup */
        PopupShowScore: boolean,
        /** Wheter to show time taken in infowindow popup */
        PopupShowTime: boolean,

        /**
         * Maximum amount of markers to show on round end
         */
        MaximumMarkerCountForRoundEnd: number,
        /** Top X players to display markers of when "Show All Guesses" is used */
        MaximumRowCountForAllMarkersDisplay: number,
        /** Wheter marker clustering is enabled */
        MarkerClustersEnabled: boolean,

        /** Wheter to draw polylines */
        CreatePolylines: boolean,

        /** Random guess indicator character */
        RandomGuessCharacter: string,

        /** Random guess indicator wrapper HTML */
        GetRandomGuessIndicator(): string,
    }

    /** Custom events from C# */
    export type CustomEventTable = {
        [target in EventTargetBase]: CustomEventDetail
    }

    /** Event handlers for custom events from C# */
    export type CustomEventDetail = {
        [target in EventName]?: Array<EventHandlerData> 
    }

    /** Event handler targets */
    export type EventTargetTable = {
        [key in EventTargetBase]: EventSubTarget
    }

    /** Event handler information */
    export type EventHandlerData = {
        /** Handler function */
        Handler: EventHandler,
        /** Condition to meet before invoking Handler */
        Condition: Nullable<EventHandlerCondition>,
        /** Retrying interval in milliseconds when Condition isnt met, 0 for no retries */
        RetryInterval: number
    }

    /** Filter Text to start with Prefix */
    export type FilterStartsWith<Text, Prefix extends string> = Text extends `${Prefix}${infer _X}`
        ? Text
        : never

    /** Filter Text to end with Suffix */
    export type FilterEndsWith<Text, Suffix extends string> = Text extends `${infer _X}${Suffix}`
        ? Text
        : never

    /** Condition function for allowing Handler invoking */
    export type EventHandlerCondition = {
        (): boolean
    }

    /** Event handler function */
    export type EventHandler = {
        (el: Element, args: EventArgs): void
    }

    /** Possible event arguments */
    export type EventArgs = EventArgsDetail & Event & MouseEvent & PointerEvent & KeyboardEvent & WheelEvent

    /** Custom event argument details sent from C# */
    export type EventArgsDetail = {
        detail?: string | GuessSummary | RoundJson | GameSummary | GameJson | RoundSummary
    }

    /** Game deserialized from JSON */
    export type GameJson = {
        /** GeoGuessr id */
        Id: string,
        /** Streamer name */
        Streamer: string,
        /** Gamem mode */
        Mode: GAMEMODE,
        /** Wheter first round is a multiguess round */
        IsFirstRoundMultiGuess: boolean,
        /** Game settings */
        GameSettings: GameSettings,
        /** Map information */
        GameMapInfo: GameMapInfo,
        /** Previous game */
        Previous: Nullable<GameJson>,
        /** Next game */
        Next: Nullable<GameJson>,
        /** Already played rounds */
        RoundsPlayed: Array<RoundSummary>,
    }

    /** Round deserialized from JSON */
    export type RoundJson = {
        /** Export format preference */
        PreferredExportFormat: ExportFormat,
        /** Export alert preference */
        AlertOnExportSuccess: boolean,
        /** Export game result preference */
        AutoExportGameResults: boolean,
        /** Export round result preference */
        AutoExportRoundResults: boolean,
        /** Export standings preference */
        AutoExportRoundStandings: boolean,
        /** Multiguess round preference */
        MultiGuessEnabled: boolean,
        /** Round number starting from 1 */
        RoundNumber: number,
        /** Round location */
        CorrectLocation: RoundCorrectLocationJson,
        /** Guesses already registered */
        Guesses: Array<NonNullable<GuessSummary>>,
    }

    /** Correct location */
    export type RoundCorrectLocationJson = LocationSource & Feature

    /** Player best stats */
    export type PlayerStats = {
        /** Best streak */
        BestStreak: number,
        /** Amount of correct country guesses */
        CorrectCountries: number,
        /** Amount of guesses */
        NumberOfCountries: number,
        /** Sum of scores */
        SumOfGuesses: number,
        /** Amount of guesses including multiguesses */
        NoOfGuesses: number,
        /** Amoun of perfect rounds */
        NoOf5kGuesses: number,
        /** Amount of wins */
        Wins: number,
        /** Amount of perfect games */
        Perfects: number,
        /** Total distance in meters */
        TotalDistance: number
    }

    /** Game settings */
    export type GameSettings = {
        /** Wheter it is a GAMEMODE.STREAK and US state streaks game */
        IsUSStreak: boolean,
        /** Wheter it is a challenge game */
        IsChallenge: boolean,
        /** Wheter it is a part of an infinite game */
        IsInfinite: boolean,
        /** Wheter it is a part of a tournament game */
        IsTournament: boolean,
        /** Time limit */
        TimeLimit: number,
        /** Wheter moving is enabled */
        MoveEnabled: boolean,
        /** Wheter zooming is enabled */
        ZoomEnabled: boolean,
        /** Wheter panning is enabled */
        PanEnabled: boolean
    }

    /** Map data */
    export type GameMapInfo = {
        /** Map unique id */
        ID: string,
        /** Common name */
        Name: string,
        /** Author name */
        Author?: string
    }

    /** Orderable data fields */
    export type OrderableSource = {
        /** Distance in meters */
        Distance: number,
        /** Country streaks */
        CountryStreak: number,
        /** Score */
        Score: number,
        /** Amount of guesses */
        GuessCount: number,
        /** Time taken in milliseconds */
        TimeTaken: number
    }

    /** Player data wrapper object */
    export type PlayerDataWrapper = {
        PlayerData: PlayerData,
    }

    /** Game result data */
    export type GameResult = PlayerDataWrapper & OrderableSource

    /** Guess data */
    export type GuessSummary = {
        /** Guess location info */
        GuessLocation: LocationSource,
        /** Wheter the owner of the guess has guessed before in current round */
        GuessedBefore: boolean,
        /** Wheter the owner is the streamer */
        IsStreamerGuess: boolean,
        /** Wheter the guess was random */
        WasRandom: boolean,
    } & GameResult

    /** Game results summary */
    export type GameSummary = {
        GameResults: Array<GameResult>
    }

    /** Round results summary */
    export type RoundSummary = {
        /** Round data */
        Round: RoundJson,
        /** Standings after this Round */
        Standings: GameSummary
    }

    /** Callback function with one optional parameter */
    export type Callback<Param1 = void, Returns = void> = {
        (param1: Param1): Returns
    }

    /** Callback function with 2 parameters */
    export type Callback2<Param1, Param2, Returns> = {
        (param1: Param1, param2: Param2): Returns
    }
}