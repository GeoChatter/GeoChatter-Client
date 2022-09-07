export namespace Enum
{
    /** Game modes */
    export const enum GAMEMODE
    {
        /** Default 5 round game */
        DEFAULT = "DEFAULT",
        /** Streaks mode */
        STREAK = "STREAK"
    }

    /** Game stages */
    export const enum GAMESTAGE
    {
        /** Round starting */
        IDLE = "IDLE",
        /** Currently in a round */
        INROUND = "INROUND",
        /** Round ended */
        ENDROUND = "ENDROUND",
        /** Game ended */
        ENDGAME = "ENDGAME",
        /** Game exited */
        EXITED = "EXITED"
    }

    /** Distance units */
    export const enum UNIT 
    {
        /** Kilometers/meters */
        KM = 0,
        /** Miles/feet */
        MI = 1
    }

    /** Player platform sources */
    export const enum Platform
    {
        /** Unknown platform, shouldn't be used */
        Unknown = 0,
        /** Twitch user */
        Twitch = 1,
        /** Youtube user */
        Youtube = 2
    }

    /** Creation state of a new game in a chain of games
     * @see JsToCsHelper.ChainGameCreateState
     * */
    export const enum ChainGameCreateState
    {
        /** Failed to create a new game */
        CreateFailed = -2,
        /** Failed to create the previous or the new game */
        SaveFailed = -1,
        /** Internal error while creating the game */
        InternalError = 0,
        /** Successfully created and chained a new game */
        Success = 1
    }

    /** Data fields available for scoreboard table */
    export const enum DataField
    {
        /** Player display name. Available all stages and modes */
        PlayerName = "PlayerName",
        /** Country streak of the player. Available all stages in GAMEMODE.DEFAULT and all stages except GAMESTAGE.INROUND for GAMEMODE.STREAK */
        CountryStreak = "CountryStreak",
        /** Guessed country/region name. Available GAMESTAGE.ENDROUND in all gamemodes and GAMESTAGE.INROUND for GAMEMODE.STREAK */
        GuessPoint = "GuessPoint",
        /** All guesses made by a player. Available GAMESTAGE.ENDGAME in all gamemodes */
        Guesses = "Guesses",
        /** Distance in meters. Available all stages and modes */
        Distance = "Distance",
        /** Correct country guesses. Available GAMESTAGE.ENDGAME in GAMEMODE.STREAK */
        CorrectTotal = "CorrectTotal",
        /** Time taken in milliseconds. Available all stages and modes */
        TimeTaken = "TimeTaken",
        /** Score. Available all stages in GAMEMODE.DEFAULT */
        Score = "Score"
    }

    /** Export formats */
    export const enum ExportFormat
    {
        xlsx = "xlsx",
        csv = "csv"
    }

    /** Event target scopes */
    export const enum EventTargetBase
    {
        window = "window",
        document = "document"
    }

    /** Event target scope properties */
    export const enum EventSubTarget
    {
        self = "self",
        body = "body"
    }

    /** Event names */
    export const enum EventName
    {
        /** Game start event */
        StartGameEvent = "StartGameEvent",
        /** Page refresh event fired after round start event when page refreshes */
        RefreshGameEvent = "RefreshGameEvent",
        /** Game exit event */
        ExitGameEvent = "ExitGameEvent",
        /** Round start event */
        StartRoundEvent = "StartRoundEvent",
        /** New guess recieved event */
        NewGuessEvent = "NewGuessEvent",
        /** Guess button listener event */
        RegisterClickEvent = "RegisterClickEvent",
        /** Round end event (streamer guess) */
        EndRoundEvent = "EndRoundEvent",
        /** End game event (view summary) */
        EndGameEvent = "EndGameEvent",
        /** End infinite game event */
        EndInfinityGameEvent = "EndInfinityGameEvent",
        /** Overlay settings updated event */
        SettingsUpdateEvent = "SettingsUpdateEvent",
        /** Main starting a game page landing event */
        AddressMainPlayScreenEvent = "AddressMainPlayScreenEvent",
        /** Click event */
        click = "click",
        /** Mouse wheel event */
        wheel = "wheel",
        /** Key down event */
        keydown = "keydown",
        /** Key up event */
        keyup = "keyup",
        /** Add link copier buttons */
        DrawMapLinkButton = "DrawMapLinkButton",
        /** Loading screen */
        LoadingScreenEvent = "LoadingScreenEvent",
        /** Sign out from GeoGuessr */
        SignOutEvent = "SignOutEvent"
    }

    /** Marker and polyline set names */
    export const enum MarkerLineAccessKey
    {
        /** Belongs to the current game players */
        currentGame = "currentGame",
        /** Correct locations of the current game */
        correctLocations = "correctLocations",
        /** Results of clicking rows on scoreboard */
        rowClickResults = "rowClickResults",
    }

    /** API Connection states */
    export const enum ConnectionState
    {
        /** Unknown state */
        UNKNOWN = -2,
        /** Disconnected */
        DISCONNECTED = -1,
        /** Connecting */
        CONNECTING = 0,
        /** Reconnecting */
        RECONNECTING = 1,
        /** Connected */
        CONNECTED = 2
    }
}

declare global
{
    /** Game modes */
    export type GAMEMODE = Enum.GAMEMODE

    /** Game stages */
    export type GAMESTAGE = Enum.GAMESTAGE

    /** Creation state of a new game in a chain of games
     * @see JsToCsHelper.ChainGameCreateState
     * */
    export type ChainGameCreateState = Enum.ChainGameCreateState

    /** Data fields available for scoreboard table */
    export type DataField = Enum.DataField

    /** Event names */
    export type EventName = Enum.EventName

    /** Event target scope properties */
    export type EventSubTarget = Enum.EventSubTarget

    /** Event target scopes */
    export type EventTargetBase = Enum.EventTargetBase

    /** Export formats */
    export type ExportFormat = Enum.ExportFormat

    /** Distance units */
    export type UNIT = Enum.UNIT

    /** Marker and polyline set names */
    export type MarkerLineAccessKey = Enum.MarkerLineAccessKey

    /** Player platform sources */
    export type PlayerPlatform = Enum.Platform

    /** API Connection states */
    export type ConnectionState = Enum.ConnectionState
}