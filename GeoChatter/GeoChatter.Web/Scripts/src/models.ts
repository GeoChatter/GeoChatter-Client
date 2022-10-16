import { MarkerClustering } from "./core";
import { Control } from "./controls";
import { MapUtil } from "./maps";
import { Util } from "./utils";
import { Enum } from "./enums"
import { Color } from "./colors";
import { Setting } from "./settings";
import { Visual } from "./visuals";

export class Chain<Model extends Chain<Model>>
    implements IChained<Model>
{
    Initialize(this: Chain<Model>, previous: Nullable<Chain<Model>>, value: Nullable<Model>, next: Nullable<Chain<Model>>)
    {
        if (this.IsInitialized) return;

        if (previous)
        {
            this.PreviousLink = previous;
            previous.NextLink = this;
        }

        this.Value = value;

        if (next)
        {
            this.NextLink = next;
            next.PreviousLink = this;
        }

        this.IsInitialized = true;
    }

    IsInitialized: boolean = false;

    Value: Nullable<Model>;

    PreviousLink: Nullable<Chain<Model>>;

    NextLink: Nullable<Chain<Model>>;

    get FirstValue(): Nullable<Model>
    {
        return this.FirstInChain.Value;
    }

    get PreviousValue(): Nullable<Model>
    {
        return this.PreviousLink?.Value;
    }

    get NextValue(): Nullable<Model>
    {
        return this.NextLink?.Value;
    }

    get LastValue(): Nullable<Model>
    {
        return this.LastInChain.Value;
    }

    get PositionInChain(): number
    {
        let g = this as Chain<Model>;
        while (g.PreviousLink)
        {
            g = g.PreviousLink;
        }

        let i = 1;
        while (g.NextLink && g != this)
        {
            g = g.NextLink;
            i++;
        }

        return i;
    }

    get FirstInChain(): Chain<Model>
    {
        let g = this as Chain<Model>;
        while (g.PreviousLink)
        {
            g = g.PreviousLink;
        }
        return g;
    }

    get LastInChain(): Chain<Model>
    {
        let g = this as Chain<Model>;
        while (g.NextLink)
        {
            g = g.NextLink;
        }
        return g;
    }

    get IsFirstInChain(): boolean
    {
        return this.FirstInChain == this;
    }

    get IsLastInChain(): boolean
    {
        return this.LastInChain == this;
    }

}

/** Main app */
export class App
    implements IParentOf<Game>
{
    constructor(gametable: GameTableOptions)
    {
        this.GameTableOptions = gametable;
        this.Constructed = new Date();
    }

    Constructed: Date;

    StartedRun: Nullable<Date>;

    InitiallyAvailableLayers: string[] = [];

    get FirstChild(): Nullable<Game>
    {
        return this.Games[0]
    }
    get LastChild(): Nullable<Game>
    {
        return this.Games[this.Games.length - 1]
    }
    get Children(): Game[]
    {
        return this.Games;
    }

    get CurrentGame(): Nullable<Game> 
    {
        return this.LastChild;
    }

    set CurrentGame(game: Nullable<Game>)
    {
        if (this.CurrentGame) this.CurrentGame.Stage = Enum.GAMESTAGE.EXITED;

        if (game) this.Games.push(game);
    }
    
    Table: Nullable<DevExpress.ui.dxDataGrid>;

    TableRows: Array<TableRow> = [];

    ScoreboardParentContainer: Nullable<HTMLDivElement>;

    Games: Array<Game> = [];

    readonly SingleOnContentReadyWeakCallbacks: Array<Callback<any>> = []

    readonly SingleOnContentReadyPersistentCallbacks: Array<Callback<any>> = []

    readonly GameTableOptions = {} as GameTableOptions;

    readonly ImageCache: CustomCache<string> = {};

    VerifyInImageCache(guess: Guess): void
    {
        const marker = guess.PlayerData.MarkerImage;
        if (typeof marker === "string")
        {
            if (marker in this.ImageCache && !this.ImageCache[marker]?.startsWith("data:text/html"))
            {
                guess.PlayerData.MarkerData = this.ImageCache[marker];
            }
            else
            {
                Util.ImageUrlToDataUrl(marker, (data) =>
                {
                    guess.PlayerData.MarkerData = data?.toString();
                    if (guess.PlayerData.MarkerData) this.ImageCache[marker] = guess.PlayerData.MarkerData;
                })
            }
        }
    }
}

/** Game object */
export class Game
    extends Chain<Game>
    implements IParentedBy<App>, IParentOf<Round>
{
    /**
     * Create a game instance
     */
    constructor(app: App, id: string, streamer: string, mode: GAMEMODE)
    {
        super();
        this.App = app;
        this.ID = id;
        this.StreamerName = streamer;
        this.Mode = mode;
    }

    get FirstChild(): Nullable<Round>
    {
        return this.Rounds[0]
    }
    get LastChild(): Nullable<Round> 
    {
        return this.Rounds[this.Rounds.length - 1]
    }
    get Children(): Round[]
    {
        return this.Rounds;
    }

    Parent(this: Game): App
    {
        return this.App;
    }

    App: App;

    readonly ID: string = "";

    readonly StreamerName: string = "";

    readonly Mode: GAMEMODE = Enum.GAMEMODE.DEFAULT;

    Stage: GAMESTAGE = Enum.GAMESTAGE.IDLE;

    StartedWithMultiGuessRound: boolean = false;

    Rounds: Array<Round> = [];

    Settings: GameSettings = {
        IsUSStreak: false,
        IsChallenge: false,
        IsInfinite: false,
        IsTournament: false,
        TimeLimit: 0,
        MoveEnabled: false,
        ZoomEnabled: false,
        PanEnabled: false
    };

    Map: GameMapInfo = {
        ID: "world",
        Name: "World"
    };

    PlaceInChain = function(this: Game) : number
    {
        let i: number = 1;
        let g: Game = this;
        while (g.PreviousValue)
        {
            i++;
            g = g.PreviousValue;
        }
        return i;
    }

    FirstPlayedRound = function(this: Game): Nullable<Round>
    {
        return this.Rounds[0];
    }

    PreviousRound = function(this: Game): Nullable<Round>
    {
        if (this.Rounds.length == 1)
        {
            return this.Rounds[0];
        }
        return this.Rounds[this.Rounds.length - 2];
    }

    get CurrentRound(): Nullable<Round>
    {
        return this.Rounds[this.Rounds.length - 1];
    }

    get FirstGameInChain(): Game
    {
        let g: Game = this;
        while (g.PreviousValue)
        {
            g = g.PreviousValue;
        }
        return g;
    }

    get AllRounds(): Array<Round>
    {
        if (!this.Settings.IsInfinite) return this.Rounds;

        let r: Array<Round> = [];
        let g: Nullable<Game> = this.FirstGameInChain;
        while (g)
        {
            r.push(...g.Rounds)
            g = g.NextValue;
        }
        return r;
    }

    get TotalRoundCount(): number
    {
        if (!this.Settings.IsInfinite) return this.Rounds.length;

        let n: number = 0;
        let g: Nullable<Game> = this;
        while (g)
        {
            n += g.Rounds.length;
            g = g.PreviousValue;
        }
        return n;
    }

    /**
     * @returns {string}
     */
    get MaxRoundCountString(): string
    {
        if (this.Settings.IsInfinite)
        {
            return "∞";
        }
        return "5"
    }

    /**
     * @returns {number}
     */
    get CurrentScoreOfStreamer(): number
    {
        let score: number = this.AllRounds
            .map(r =>
            {
                let g: Nullable<Guess> = r.Guesses.find(g => g.IsStreamerGuess)
                if (!g) return 0;
                return g.Score;
            })
            .reduce((prev, curr) => prev + curr, 0)

        return score;
    }

    static CreateGameFromJson(app: App, data: GameJson, useasprev: Nullable<Game> = null, useasnext: Nullable<Game> = null): Nullable<Game>
    {
        if (!data) return;
        console.log("CreateGameFromJson: ", data, useasprev, useasnext)

        let previous: Nullable<Game>;
        let next: Nullable<Game>;

        let game = new Game(app, data.Id, data.Streamer, data.Mode);
        game.Stage = Enum.GAMESTAGE.INROUND;
        game.StartedWithMultiGuessRound = data.IsFirstRoundMultiGuess;
        game.Settings = data.GameSettings;
        game.Map = data.GameMapInfo;
        game.Rounds = data.RoundsPlayed.map((r)=>
        {
            let round = Round.CreateRoundFromJson(game, r.Round, r.Standings);
            return round;
        });

        console.log("CreateGameFromJson: Before links", game);
        if (useasprev)
        {
            previous = useasprev;
        }
        else if (data.Previous) 
        {
            previous = Game.CreateGameFromJson(app, data.Previous, null, game);
        }

        if (useasnext)
        {
            next = useasnext;
        }
        else if (data.Next) 
        {
            next = Game.CreateGameFromJson(app, data.Next, game, null);
        }

        game.Initialize(previous, game, next);

        console.log("CreateGameFromJson: After links", game)
        return game;
    }

    /**
     * Flag span html from player's guesses
     * @param {string} playerName player name
     * @param {Array.<Round>} rounds rounds
     * @param {boolean} summary is for summary table
     * @returns {string}
     */
    static GetFlagIconsFromRoundsFor = function(player: PlayerData, rounds: Array<Round>, summary?: boolean) : string
    {
        let tags = ""
        for (let i = 0; i < rounds.length; i++)
        {
            let round: Nullable<Round> = rounds[i];
            if (round && round.Guesses)
            {
                let guess: Nullable<Guess> = round.GetGuessOf(player);

                let tag: string = "";

                if (guess)
                {
                    tag = guess.WrapFlagWithBorder(summary);
                }
                else tag = `<div class='flagBack flagNotGuessed ${(summary ? "flagBackSummary" : "")}'>${Util.FixFlagHTML("x", "No Guess Made")}</div>`;

                tags += tag;
            }
        }
        return tags;
    }

    /**
     * Get game summary as datatable rows
     * @param {object} summaries json sent from C# 
     * @returns {Array.<object>}
     */
    AsTableRows = function(this: Game, summaries: GameSummary) : Array<TableRow>
    {
        console.log("Game AsTableRows", this, summaries);
        let rows = [];

        if (this.Mode == Enum.GAMEMODE.DEFAULT)
        {

            for (let summary of summaries.GameResults)
            {
                if (!summary.PlayerData.FlagDisplay)
                {
                    summary.PlayerData.FlagDisplay = Util.FixFlagHTML(summary.PlayerData.FlagCode, summary.PlayerData.FlagName);
                }

                let row: TableRow = { Player: summary.PlayerData }
                let fl: string = "";
                let color: string = Color.RandomColor();
                let gn: number = 0;
                let rs: Array<Round> = this.AllRounds;
                let fsort = 0;
                for (let i = 0; i < rs.length; i++)
                {
                    let round: Nullable<Round> = rs[i];
                    if (round && round.Results)
                    {
                        let guess: Nullable<Guess> = round.GetGuessOf(row.Player);

                        let tag = "";

                        if (guess)
                        {
                            tag = guess.WrapFlagWithBorder(true);

                            if (guess.PlayerData.Color) color = guess.PlayerData.Color;

                            gn += guess.GuessNumber;
                            fsort += guess.GetGuessPoints();

                        }
                        else tag = `<div class='flagBack flagNotGuessed "flagBackSummary"}'>${Util.FixFlagHTML("x", "No Guess Made")}</div>`;

                        fl += tag;
                    }
                }

                row.PlayerName = Util.AsDataTableRowCell(`${summary.PlayerData.FlagDisplay}${Color.ColorUsername(color, summary.PlayerData.Display)}`, summary.PlayerData.Name.toLowerCase(), Visual.PlatformCSSFromPlatform(summary.PlayerData.Platform));
                row.CountryStreak = Util.AsDataTableRowCell(summary.CountryStreak.toString(), summary.CountryStreak);
                row.Guesses = Util.AsDataTableRowCell(fl, fsort);
                row.Distance = Util.GetConvertedDistance(summary.Distance);
                row.Score = Util.AsDataTableRowCell(summary.Score + ` (${gn})`, summary.Score);
                row.TimeTaken = Util.FormatTimeToString(summary.TimeTaken);

                rows.push(row);
            }
        }
        else if (this.Mode == Enum.GAMEMODE.STREAK)
        {
            for (let summary of summaries.GameResults)
            {
                if (!summary.PlayerData.FlagDisplay)
                {
                    summary.PlayerData.FlagDisplay = Util.FixFlagHTML(summary.PlayerData.FlagCode, summary.PlayerData.FlagName);
                }

                let row: TableRow = { Player: summary.PlayerData }
                let fl = "";
                let color: string = Color.RandomColor();
                let total = 0;
                let correct = 0;
                let rs = this.AllRounds;
                let fsort = 0;

                for (let i = 0; i < rs.length; i++)
                {
                    let round = rs[i];
                    if (round && round.Results)
                    {
                        let guess = round.GetGuessOf(row.Player);

                        let tag = "";

                        if (guess)
                        {
                            total++;
                            tag = guess.WrapFlagWithBorder(true);

                            if (guess.PlayerData.Color) color = guess.PlayerData.Color;

                            fsort += guess.GetGuessPoints();

                            if (guess.Location.CountryCode == round.Location.CountryCode &&
                                guess.Location.CountryName == round.Location.CountryName &&
                                (!this.Settings.IsUSStreak || guess.Location.ExactCountryCode == round.Location.ExactCountryCode &&
                                guess.Location.ExactCountryName == round.Location.ExactCountryName)
                            )
                            {
                                correct++;
                            }
                        } else tag = `<div class='flagBack flagNotGuessed flagBackSummary'>${Util.FixFlagHTML("x", "No Guess Made")}</div>`;

                        fl += tag;
                    }
                }

                row.PlayerName = Util.AsDataTableRowCell(`${summary.PlayerData.FlagDisplay}${Color.ColorUsername(color, summary.PlayerData.Display)}`, summary.PlayerData.Name.toLowerCase(), Visual.PlatformCSSFromPlatform(summary.PlayerData.Platform));
                row.CountryStreak = Util.AsDataTableRowCell(summary.CountryStreak.toString(), summary.CountryStreak);
                row.Guesses = Util.AsDataTableRowCell("<div style='overflow-x:auto;overflow-y:hidden'>" + fl + "</div>", fsort);
                row.CorrectTotal = Util.AsDataTableRowCell(`${correct}/${total}`, correct);
                row.Distance = Util.GetConvertedDistance(summary.Distance);
                row.TimeTaken = Util.FormatTimeToString(summary.TimeTaken);

                rows.push(row);
            }
        }

        return rows;
    }
}

/** Game round object */
export class Round
    extends Chain<Round>
    implements IParentedBy<Game>, IParentOf<Guess>, ICountrySource, IdentifiedBy<Game, Round>
{
    /**
     * Round constructor
     */
    constructor(game: Game, id: number, multi: boolean, location: LocationSource, settings: MapRoundSettings)
    {
        super();
        this.Game = game;
        this.ID = id;
        this.MultiGuessEnabled = multi;
        this.Location = location;
        this.Location.PanoOverwritten = false;
        this.Guesses = [];
        this.Settings = settings;

        if (this.Game.PreviousValue)
        {
            this.Initialize(this.Game.PreviousValue.CurrentRound, this, null);
        }
        else
        {
            this.Initialize(this.Game.CurrentRound, this, null);
        }
    }

    Location: LocationSource;

    get FirstChild(): Nullable<Guess>
    {
        return this.Guesses[0]
    }
    get LastChild(): Nullable<Guess>
    {
        return this.Guesses[this.Guesses.length - 1]
    }
    get Children(): Guess[]
    {
        return this.Guesses
    }

    Parent(this: Round) : Game
    {
        return this.Game;
    }

    Is(this: Round, other: Round): boolean
    {
        return this == other;
    }

    BelongsTo(this: Round, owner: Game): boolean
    {
        return owner.Rounds.indexOf(this) >= 0;
    }

    Game: Game;

    ID: number;

    MultiGuessEnabled: boolean;

    get FlagDisplay(): Nullable<string> 
    {
        return this.GetFlagHTML();
    }

    FlagCode: Nullable<string>;

    FlagName: Nullable<string>;

    Guesses: Array<Guess> = [];

    Results: Array<Guess> = [];

    ResultTableRows: Array<TableRow> = [];

    Standings: Array<TableRow> = [];

    IsStandingsSet: boolean = false;

    Settings: MapRoundSettings;

    _OriginalAsTableRowsParse = {} as RoundSummary;

    CoordinateDecimals: number = 4;

    CalculateGuessOrders(this: Round)
    {
        let rows = this.Game.App.Table?.getVisibleRows();
        this.Results.forEach((guess) =>
        {
            let order = guess.GetOrderFromName(rows);
            guess.ResultFinalOrder = order;
        });

        this.Results = this.Results.sort((a, b) => a.ResultFinalOrder - b.ResultFinalOrder)
    }

    /**
     * Get guess of given player in the round
     */
    GetGuessOf = function(this: Round, player: PlayerData) : Nullable<Guess>
    {
        let source = [];
        if (this.Results.length > 0)
        {
            source = this.Results;
        }
        else
        {
            source = this.Guesses;
        }
        for (let guess of source)
        {
            if (guess.BelongsTo(player)) return guess;
        }
        return;
    }

    GetCountryName(this: Round): string
    {
        return (Setting.Overlay.RegionalFlags ?
            this.Location.ExactCountryName :
            this.Location.CountryName
        );
    }

    GetCountryCode(this: Round): string
    {
        return (Setting.Overlay.RegionalFlags ?
            this.Location.ExactCountryCode :
            this.Location.CountryCode
        );
    }

    GetFlagHTML(this: Round): string
    {
        return (Setting.Overlay.RegionalFlags ?
            Util.FixFlagHTML(this.Location.ExactCountryCode?.toLowerCase(), this.Location.ExactCountryName) :
            Util.FixFlagHTML(this.Location.CountryCode?.toLowerCase(), this.Location.CountryName)
        );
    }

    GetEmbedTitleHTML(this: Round, addPrefix: boolean = true): string
    {
        return `${(addPrefix ? "Correct location " : "")}${this.GetFlagHTML()} ${('<span style="color: skyblue;">' + this.GetCountryName() + '</span>') }`;
    }

    GetInfoWindowHTML(this: Round): string
    {
        return `<div class="gm-iw-custom" style='border: 3px solid red !important'>
                    <div style="font-size:${Setting.Overlay.FontSize}${Setting.Overlay.FontSizeUnit}">
                        ROUND ${this.Game.TotalRoundCount}
                    </div>
                    (${this.GetFlagHTML()}: ${this.Location.Latitude.toFixed(this.CoordinateDecimals)}, ${this.Location.Longitude.toFixed(this.CoordinateDecimals)})
                </div>`;
    }

    /**
     * Get round summary as datatable rows
     */
    AsTableRows = function(this: Round, details: RoundSummary | string, guessesAlreadyOrdered?: boolean): Array<TableRow>
    {
        if (typeof details !== "string")
        {
            console.log("Round AsTableRows", this, details, guessesAlreadyOrdered);
            this._OriginalAsTableRowsParse = details;
        }
        else
        {
            console.log("Round AsTableRows", this, btoa("details:b64:" + unescape(encodeURIComponent(details))), guessesAlreadyOrdered);
            this._OriginalAsTableRowsParse = (JSON.parse(details) as EventArgsDetail).detail as RoundSummary;
        }

        let glis: Array<GuessSummary> = this._OriginalAsTableRowsParse.Round.Guesses;
        let rows: Array<TableRow> = [];
        this.Results = [];

        if (this.Game.Mode == Enum.GAMEMODE.DEFAULT)
        {
            for (let i = 0; i < glis.length; i++)
            {
                let g = glis[i];
                if (!g) continue;

                let guess = new Guess(this, g);
                if (guessesAlreadyOrdered)
                {
                    guess.ResultFinalOrder = i + 1;
                }

                this.Results.push(guess);
                let row: TableRow = { Player: guess.PlayerData }
                let fl = guess.WrapFlagWithBorder();
                let fsort = guess.GetGuessPoints();

                row.PlayerName = Util.AsDataTableRowCell(guess.GetPlayerNameDisplayHTML(), guess.PlayerData.Name.toLowerCase(), Visual.PlatformCSSFromPlatform(guess.PlayerData.Platform));
                row.CountryStreak = Util.AsDataTableRowCell(guess.CountryStreak.toString(), guess.CountryStreak);
                row.GuessPoint = Util.AsDataTableRowCell(fl, fsort);
                row.Distance = Util.GetConvertedDistance(guess.Distance);
                row.Score = Util.AsDataTableRowCell(guess.Score + (this.MultiGuessEnabled && guess.GuessNumber > 1 ? ` (${guess.GuessNumber})` : ""), guess.Score);
                row.TimeTaken = Util.FormatTimeToString(guess.TimeTaken);

                rows.push(row);
            }
        }
        else if (this.Game.Mode == Enum.GAMEMODE.STREAK)
        {
            for (let i = 0; i < glis.length; i++)
            {
                let g = glis[i];
                if (!g) continue;

                let guess = new Guess(this, g);
                if (guessesAlreadyOrdered)
                {
                    guess.ResultFinalOrder = i + 1;
                }

                this.Results.push(guess);
                let row: TableRow = { Player: guess.PlayerData }
                let fl = guess.WrapFlagWithBorder();
                let fsort = guess.GetGuessPoints();

                row.PlayerName = Util.AsDataTableRowCell(guess.GetPlayerNameDisplayHTML(), guess.PlayerData.Name.toLowerCase(), Visual.PlatformCSSFromPlatform(guess.PlayerData.Platform));
                row.CountryStreak = Util.AsDataTableRowCell(guess.CountryStreak.toString(), guess.CountryStreak);
                row.GuessPoint = Util.AsDataTableRowCell(fl, fsort);
                row.Distance = Util.GetConvertedDistance(guess.Distance);
                row.TimeTaken = Util.FormatTimeToString(guess.TimeTaken);

                rows.push(row);
            }
        }

        this.ResultTableRows = rows;
        if (!this.IsStandingsSet)
        {
            this.Standings = this.Game.AsTableRows(this._OriginalAsTableRowsParse.Standings);
        }

        return rows;
    }

    static CreateRoundFromJson(game: Game, data: RoundJson, standings: Nullable<GameSummary> = null)
    {
        let round = new Round(game,
            data.RoundNumber,
            data.MultiGuessEnabled,
            data.CorrectLocation,
            data.MapRoundSettings)

        let guesses = data.Guesses;
        for (let i = 0; i < guesses.length; i++)
        {
            let g = guesses[i];
            if (!g) continue;

            let guess = new Guess(round, g);

            if (standings) guess.ResultFinalOrder = i + 1;
            round.Guesses.push(guess);
        }

        if (standings)
        {
            round.AsTableRows({ Round: data, Standings: standings }, true);
            round.IsStandingsSet = true;
        }

        return round;
    }
}

/** Guess object */
export class Guess
    extends Chain<Guess>
    implements IParentedBy<Round>, ICountrySource, IdentifiedBy<PlayerData, Guess>
{
    /**
     * Full Guess instance
     */
    constructor(round: Round, data: GuessSummary)
    {
        super();
        this.Round = round;
        this.Location = data.GuessLocation;
        this.Location.PanoOverwritten = false;
        this.PlayerData = data.PlayerData;

        this.Distance = data.Distance;
        this.Score = data.Score;
        this.CountryStreak = data.CountryStreak;
        this.TimeTaken = data.TimeTaken;

        this.IsStreamerGuess = data.IsStreamerGuess;
        this.IsFirstGuess = !data.GuessedBefore;
        this.GuessNumber = data.GuessCount;
        this.IsRandom = data.WasRandom;
        this.RandomGuessArgs = data.RandomGuessArgs;
        this.Source = data.Source;
        this.Layer = data.Layer;

        this.PlayerData.FlagDisplay = Util.FixFlagHTML(this.PlayerData.FlagCode, this.PlayerData.FlagName);
        if (!this.PlayerData.Color)
        {
            this.PlayerData.Color = Color.RandomColor();
        }

        round.Game.App.VerifyInImageCache(this);

        let prevguess = this.Round.GetGuessOf(this.PlayerData);
        if (prevguess)
        {
            this.Initialize(prevguess, this, null);
        }
        else
        {
            this.Initialize(null, this, null);
        }
    }

    Location: LocationSource;

    Round: Round;

    PlayerData: PlayerData;

    Parent(this: Guess): Round
    {
        return this.Round;
    }

    Is(this: Guess, model: Guess): boolean
    {
        return this == model;
    }

    BelongsTo(this: Guess, player: PlayerData): boolean
    {
        return this.PlayerData.Id == player.Id
            && this.PlayerData.Platform == player.Platform
    }

    MapsMarker: Nullable<google.maps.Marker>;

    MapsLine: Nullable<google.maps.Polyline>;

    CountryStreak: number;

    Distance: number;

    Score: number;

    TimeTaken: number;

    IsStreamerGuess: boolean;

    IsFirstGuess: boolean;

    GuessNumber: number;

    IsRandom: boolean;

    RandomGuessArgs: string;

    Source: GuessSource;

    Layer: string;

    ResultFinalOrder: number = 0;

    static EmptyGuessCode: string = "x";

    static EmptyGuessName: string = "No Guess Made";

    GetCountryName(this: Guess): string
    {
        return (Setting.Overlay.RegionalFlags ?
            this.Location.ExactCountryName :
            this.Location.CountryName
        );
    }

    GetCountryCode(this: Guess): string
    {
        return (Setting.Overlay.RegionalFlags ?
            this.Location.ExactCountryCode :
            this.Location.CountryCode
        );
    }

    GetFlagHTML(this: Guess): string
    {
        return (Setting.Overlay.RegionalFlags ?
            Util.FixFlagHTML(this.Location.ExactCountryCode?.toLowerCase(), this.Location.ExactCountryName) :
            Util.FixFlagHTML(this.Location.CountryCode?.toLowerCase(), this.Location.CountryName)
        );
    }

    GetEmbedTitleHTML(this: Guess, addPrefix: boolean = true): string
    {
        return `${(addPrefix ? "Guess of " : "")}${this.PlayerData.FlagDisplay} ${Color.ColorUsername(this.PlayerData.Color, this.PlayerData.Name, true)} in ${this.GetFlagHTML() + ('<span style="color: skyblue;">' + this.GetCountryName() + '</span>')}`;
    }

    GetOrderFromName(this: Guess, rows: Nullable<Array<DevExpress.ui.dxDataGrid.Row>>): number
    {
        if (!rows) rows = this.Round.Game.App.Table?.getVisibleRows();
        if (!rows) return -1

        let len = rows.length;
        let j = 0;
        for (let i = 0; i < len; i++)
        {
            if (Util.AreTheSamePlayers(this.PlayerData, (rows[i]?.data as TableRow).Player))
            {
                j = i + 1;
                break;
            }
        }
        return j > 0 ? j: -1;
    }

    GetInfoWindowHTML(this: Guess): string
    {
        let dist = Util.GetConvertedDistance(this.Distance);

        let name = `<div style="font-size:${Setting.Overlay.FontSize}${Setting.Overlay.FontSizeUnit}">
                ${this.PlayerData.FlagDisplay}
                ${Color.ColorUsername(this.PlayerData.Color, this.PlayerData.Display)}
                </div>`;

        let streak = Setting.Overlay.PopupShowStreak ?
            `${this.CountryStreak} streak` :
            "";
        let first = !Setting.Overlay.PopupShowStreak;

        let coor = Setting.Overlay.PopupShowCoordinates ?
            `${(first ? "" : "<br>")}(${this.GetFlagHTML()}: ${this.Location.Latitude.toFixed(4)}, ${this.Location.Longitude.toFixed(4)})` :
            "";
        first &&= !Setting.Overlay.PopupShowCoordinates;

        let dis = Setting.Overlay.PopupShowDistance ?
            `${(first ? "" : "<br>")}${dist.display}` :
            "";
        first &&= !Setting.Overlay.PopupShowDistance;

        let score = Setting.Overlay.PopupShowScore && this.Round.Game.Mode != Enum.GAMEMODE.STREAK ?
            `${(first ? "" : "<br>")}${this.Score} points` :
            "";
        first &&= !Setting.Overlay.PopupShowScore;

        let t = Setting.Overlay.PopupShowTime ?
            `${(first ? "" : "<br>")}${Util.FormatTimeToString(this.TimeTaken).display}` :
            "";

        return `<div class="gm-iw-custom" style='border: 3px solid ${this.PlayerData.Color} !important'>
                    ${name}
                    ${streak}
                    ${coor}
                    ${dis}
                    ${score}
                    ${t}
                </div>`;
    }

    GetPlayerNameDisplayHTML = function(this: Guess)
    {
        return `${this.PlayerData.FlagDisplay}${(this.IsRandom ? Setting.Overlay.GetRandomGuessIndicator(this.RandomGuessArgs) : "")}${Color.ColorUsername(this.PlayerData.Color, this.PlayerData.Display)}`
    }

    GetGuessPoints = function(this: Guess)
    {
        return this.Round.Location.ExactCountryCode != this.Round.Location.CountryCode
            ? (this.Location.ExactCountryCode == this.Round.Location.ExactCountryCode
                ? 2
                : this.Location.CountryCode == this.Round.Location.CountryCode ? 1 : 0)
            : this.Location.CountryCode == this.Round.Location.CountryCode ? 2 : 0
    }

    /**
     * Wrap flag span with border div
     * @param {boolean} summary is for summary table
     * @returns {string}
     */
    WrapFlagWithBorder = function(this: Guess, summary: boolean = false): string
    {
        let flag: string = this.GetFlagHTML();
        let extras: string = summary ? 'flagBackSummary' : '';

        if (this.Round.Game.Mode == Enum.GAMEMODE.STREAK && this.Round.Game.Stage == Enum.GAMESTAGE.INROUND)
        {
            flag = `<div class='flagBack flagNotGuessed ${extras}'>${flag}</div>`;
        } else
        {
            if (this.Round.Location.CountryCode.toLowerCase() == this.Location.CountryCode.toLowerCase() &&
                (!Setting.Overlay.UseWrongRegionColors || (this.Location.ExactCountryCode.toLowerCase() == this.Round.Location.ExactCountryCode.toLowerCase())))
                flag = `<div title='Correct country and region' class='flagBack flagBackCorrectRegion ${extras}'>${flag}</div>`;

            else if (this.Round.Location.CountryCode.toLowerCase() == this.Location.CountryCode.toLowerCase())
            {
                if (!Setting.Overlay.UseWrongRegionColors)
                    flag = `<div title='Correct country and region' class='flagBack flagBackCorrectRegion ${extras}'>${flag}</div>`
                else
                    flag = `<div title='Correct country / Incorrect region: ${this.Location.CountryName} / ${this.Location.ExactCountryName} ' class='flagBack flagBackCorrectCountry ${extras}'>${flag}</div>`
            } else if (!flag || flag == "")
                flag = `<div class='flagBack flagNotGuessed ${extras}'>${Guess.EmptyGuessFlagHTML()}</div>`;
            else
                flag = `<div title='Incorrect country and region' class='flagBack flagBackIncorrect ${extras}'>${flag}</div>`
        }

        return flag;
    }

    async EmbedLocation(this: Guess)
    {
        try
        {
            await MapUtil.GetStreetviewPanorama({
                lat: this.Location.Latitude,
                lng: this.Location.Longitude
            }, Setting.Overlay.StreetViewMaxRadius)
                .then(loc => Control.EmbedLocation(`${this.PlayerData.Platform}-${this.PlayerData.Id}`,
                    this.GetEmbedTitleHTML(),
                    loc,
                    0, 0, 180, loc.pano, this
                ))
                .catch(() =>
                {
                    Control.EmbedStreetviewNotFound(this.GetEmbedTitleHTML(false), Util.CoordinatesToURLArgument(this.Location.Latitude, this.Location.Longitude));
                });
        } catch {
            Control.EmbedStreetviewNotFound(this.GetEmbedTitleHTML(false), Util.CoordinatesToURLArgument(this.Location.Latitude, this.Location.Longitude));
        }
    }

    static EmptyGuessFlagHTML()
    {
        return Util.FixFlagHTML(Guess.EmptyGuessCode, Guess.EmptyGuessName);
    }

    /**
     * Get guess as a row object to be added to table during the round
     */
    AsTableRow = function(this: Guess) : TableRow
    {
        console.log("Guess AsTableRow", this);
        let row: TableRow = { Player: this.PlayerData }

        if (this.Round.Game.Mode == Enum.GAMEMODE.DEFAULT)
        {
            row.PlayerName = Util.AsDataTableRowCell(this.GetPlayerNameDisplayHTML(), this.PlayerData.Name.toLowerCase(), Visual.PlatformCSSFromPlatform(this.PlayerData.Platform));
            row.CountryStreak = Util.AsDataTableRowCell(this.CountryStreak.toString(), this.CountryStreak);
            row.Distance = Util.GetConvertedDistance(this.Distance);
            row.Score = Util.AsDataTableRowCell(this.Score.toString(), this.Score);
            row.TimeTaken = Util.FormatTimeToString(this.TimeTaken);
        }
        else if (this.Round.Game.Mode == Enum.GAMEMODE.STREAK)
        {
            row.GuessPoint = Util.AsDataTableRowCell(this.WrapFlagWithBorder(), this.Location.ExactCountryCode);
            row.PlayerName = Util.AsDataTableRowCell(this.GetPlayerNameDisplayHTML(), this.PlayerData.Name.toLowerCase(), Visual.PlatformCSSFromPlatform(this.PlayerData.Platform));
            row.TimeTaken = Util.FormatTimeToString(this.TimeTaken);
        }

        return row;
    }
}

/** Table column object */
export class GameTableColumn
    implements GameTableColumnOptions
{
    /**
     * Table column
     * @param {number} pos column visible index
     * @param {string} id field name
     * @param {string} name display name
     * @param {number} width width
     * @param {string} sortOrder asc or desc
     * @param {number} sortIndex sorting index 
     * @param {string} defsortOrder default asc or desc
     * @param {number} defsortIndex default sorting index 
     * @param {boolean} visible visibility
     * @param {boolean} sortable sortability
     * @param {boolean} multi visible in multiguess mode
     */
    constructor(pos: number, id: DataField, name: string, width: number, sortOrder: Nullable<DevExpress.common.SortOrder> | "" = null, sortIndex: number = -1, defsortOrder: Nullable<DevExpress.common.SortOrder> | "" = null, defsortIndex: number = -1, visible: boolean = true, sortable: boolean = true, multi: boolean = true)
    {
        this.Position = pos;
        this.DataField = id;
        this.Name = name
        this.Visible = visible;
        this.Width = width;
        this.Sortable = sortable;
        this.SortOrder = sortOrder;
        this.SortIndex = sortIndex;
        this.DefaultSortOrder = defsortOrder;
        this.DefaultSortIndex = defsortIndex;
        this.AllowedWithMultiGuess = multi;
    }

    Position: number;

    readonly DataField: DataField;

    Name: string;

    Width: number;

    Visible: boolean;

    Sortable: boolean;

    SortIndex: number;

    SortOrder: Nullable<DevExpress.common.SortOrder> | "" = null;

    readonly DefaultSortIndex: number;

    readonly DefaultSortOrder: Nullable<DevExpress.common.SortOrder> | "" = null;

    readonly AllowedWithMultiGuess: boolean;
}

export class MarkerClustererExtended
    extends MarkerClustering.MarkerClusterer
{
    override clusters: ClusterExtended[] = []
}

export class ClusterExtended
    extends MarkerClustering.Cluster
{
    override readonly markers?: Array<IMarkerWithSource>
}

window.GC.Models = {
    Chain,
    App,
    Game,
    Round,
    Guess,
    GameTableColumn,
    MarkerClustererExtended,
    ClusterExtended
}
