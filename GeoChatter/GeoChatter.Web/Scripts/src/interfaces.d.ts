/// <reference path="./types.d.ts" />

declare export global
{
    /** Global GC access */
    export interface IGeoChatterGlobal
    {
        Color: {},
        Constant: {},
        Control: {},
        Dependency: {},
        Div: {},
        EventHandler: {},
        CustomEvents: {},
        GeoChatter: {},
        MapUtil: {},
        Models: {
            [key: string]: { new(...args: any): object }
        },
        Setting: {},
        State: {},
        Util: {},
        Visual: {},
    }

    export interface ICountrySource implements FlagSource
    {
        /** Location source */
        Location: LocationSource;

        /**
         * Get location name
         */
        GetCountryName(): string;

        /**
         * Get location code
         */
        GetCountryCode(): string;

        /**
         * Get flag HTML representation
         */
        GetFlagHTML(): string;

        /**
         * Get title HTML for embed controls
         * @param addPrefix {boolean} Wheter to add prefix to title ("Correct location in...", "Guess of..." etc.)
         */
        GetEmbedTitleHTML(addPrefix: boolean = true): string;

        /**
         * Get google.maps.InfoWindow content to be displayed in HTML
         */
        GetInfoWindowHTML(): string;
    }

    /** Marker with its owner */
    export interface IMarkerWithSource extends google.maps.Marker
    {
        /** Marker owner */
        guess: Nullable<Guess>
    }

    /** App settings */
    export interface IAppSettingTable extends ISettingTable
    {
        /** Preferred export format */
        PreferredExportFormat: ExportFormat,
    }

    /** App states */
    export interface IAppStateChecklist extends IStateChecklist
    {
        /** State of CTRL key being down */
        IsCTRLDown: boolean,
        /** Wheter the main.js has completed loading */
        LoadCompleted: boolean,
        /** Preference of multiguessing for the next round */
        PreferredMultiguess: boolean,
        /** State of current game from Start Game event started (meaning Start Round event was fired) */
        StartedCurrentGame: boolean,
        /** Wheter the round is currently being closed for next round to start */
        ClosingTheRound: boolean,
        /** State of streamer guess listener being added */
        CheckEventAdded: boolean,
        /** Preference of export success alerts */
        AlertExportSuccess: boolean,
        /** Preference of export  */
        AutoExportGameResults: boolean,
        /** Preference of export */
        AutoExportRoundResults: boolean,
        /** Preference of export */
        AutoExportRoundStandings: boolean,
        /** Preference of "Play Again" button showing after infinite game end screen */
        PlayAgainEnabled: boolean,
        /** State of scoreboard being created for the first time */
        Attempting_CreateInitialTable: boolean,
        /** State of missing / delayed scoreboard being created */
        Attempting_RecreateMissingScoreboard: boolean,
        /** Wheter next streamer guess will be marked as random */
        NextStreamerGuessIsRandom: boolean,
        /** State of "all guesses" markers being hidden */
        HideMarkerState: boolean,
        /** State of New Guess handler is currently executing */
        ProcessingGuess: boolean
    }

    /** Event states */
    export interface IEventStateChecklist extends IStateChecklist
    {
        /** State of guess button click listener being added */
        AddedClick: boolean,
        /** State of next round button click listener being added */
        AddedClickEnd: boolean,
        /** State of view summary button click listener being added */
        AddedClickViewSummary: boolean,
        /** State of minimap click listener being added */
        AddedStreakMarkerListener: boolean
    }

    /** Scoreboard states */
    export interface IScoreboardStateChecklist extends IStateChecklist
    {
        /** Wheter to use table options width value or ignore it */
        ShouldUseTableOptionsWidth: boolean,
        /** Wheter to use scoreboard saved width, height, top, left values or default ones */
        ShouldUseLastScoreBoardSettings: boolean,
        /** State of sub-title of scoreboard click listener */
        SubtitleClickReady: boolean,
        /** State of "Current Standings" being displayed */
        DisplayingCurrentStandings: boolean,
        /** State of auto-scrolling currently being active */
        IsScrolling: boolean,
        /** State of auto-scrolling being allowed */
        IsScrollingEnabled: boolean,
    }

    /** Parent-Child model parent interface */
    export interface IParentOf<ChildModel extends IParentedBy<any>>
    {
        /** First child from Children array */
        readonly FirstChild: Nullable<ChildModel>,
        /** Last child from Children array */
        readonly LastChild: Nullable<ChildModel>,
        /** Ordered children array */
        readonly Children: Array<NonNullable<ChildModel>>
    }

    /** Parent-Child model child interface */
    export interface IParentedBy<ParentModel extends IParentOf<any>>
    {
        /** Parent of this child model */
        Parent(): NonNullable<ParentModel>
    }

    /** Identity interface */
    export interface IdentifiedBy<Key, Self>
    {
        /**
         * Check if self is same as other
         * @param other
         */
        Is(other: Self): boolean
        /**
         * Check if self belongs to given owner
         * @param owner
         */
        BelongsTo(owner: Key): boolean
    }

    /** Double linked list/chain model */
    export interface IChained<Model>
    {
        /**
         * Initialize the link
         * @param previous Previous link to connect
         * @param value Current link value
         * @param next Next link to connect
         */
        Initialize(previous: Nullable<IChained<Model>>, value: Nullable<Model>, next: Nullable<IChained<Model>>),
        /** Wheter this link has been initialized via Initialize */
        IsInitialized: boolean,
        /** Value held by link */
        Value: Nullable<Model>,
        /** Previous link in chain */
        PreviousLink: Nullable<IChained<Model>>,
        /** Next link in chain */
        NextLink: Nullable<IChained<Model>>,
        /** First link's value */
        readonly FirstValue: Nullable<Model>,
        /** Previous link's value */
        readonly PreviousValue: Nullable<Model>,
        /** Next link's value */
        readonly NextValue: Nullable<Model>,
        /** Last link's value */
        readonly LastValue: Nullable<Model>,
        /** Position in the chain from start, starts from 1 */
        readonly PositionInChain: number,
        /** Wheter link is the first in chain */
        readonly IsFirstInChain: boolean,
        /** Wheter link is the last in chain */
        readonly IsLastInChain: boolean,
        /** First link of the chain or self */
        readonly FirstInChain: IChained<Model>,
        /** Last link of the chain or self */
        readonly LastInChain: IChained<Model>
    }

    /** Table options interface deserialized from C# */
    export interface IGameTableOptions
    {
        /** Options array */
        Options: IGameOptions[];
    }

    /** Game options with game mode and stages deserialized from C# */
    export interface IGameOptions
    {
        /** Game mode */
        Mode: GAMEMODE;
        /** Game stages for the mode */
        Stages: IStageOptions[];
    }

    /** Scoreboard game stage options deserialized from C# */
    export interface IStageOptions
    {
        /** Game stage */
        Stage: GAMESTAGE;
        /** Columns for the stage */
        Columns: GameTableColumnOptions[];
        /** Offset from top in pixels */
        Top: number;
        /** Offset from left in pixels */
        Left: number;
        /** Width in pixels */
        Width: number;
        /** Height in pixels */
        Height: number;
        /** Wheter to rows are hidden */
        IsMinimized: boolean;
        /** Wheter scoreboard has row numbers */
        ShowRowNumbers: boolean;
        /** Minimap layer preference */
        MinimapLayer: string;
    }

    /** General interface for states interfaces */
    export interface IStateChecklist 
    {
        [statename: string]: boolean
    }

    /** General interface for settings interfaces */
    export interface ISettingTable
    {
        [statename: string]: any
    }
}