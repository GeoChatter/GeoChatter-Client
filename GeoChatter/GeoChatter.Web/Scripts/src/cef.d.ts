import { Enum } from "./enums"

declare export global
{
    /**
     * Object with C# methods bound
     * */
    export interface IJSHelper
    {
        /** Get available layer names */
        getAvailableLayers(): Promise<string[]>

        /**
         * Change MapRoundSetting for next round
         * @param settingName setting name
         * @param value new value
         * @param forClient wheter only for the client
         */
        changeRoundSetting(settingName: string, value: boolean | string | number, forClient: boolean): Promise<void>

        /** Execute managed user scripts */
        executeUserScripts(): Promise<void>

        /** DEBUG: Show devtools window */
        showDevTools?(): Promise<void>

        /**
         *  DEBUG: Send random guesses
         * @param amount guess amount
         */
        sendRandomGuess?(amount: number): Promise<void>

        /**
         * Trigger browser zoom
         * @param amount delta value
         */
        zoom(amount: number): Promise<void>

        /** Mark form about the next address change being a page refresh */
        goingToRefresh(path: string): Promise<void>

        /**
         * Mark game with given Geoguessr id as "started"
         * @param id
         */
        verifyGameStarted(id: string): Promise<void>

        /** Get the current connection state of the hub connection */
        connectionState(): Promise<ConnectionState>

        /**
         * Mark game with given Geoguessr id as "started"
         * @param id
         */
        verifyGameStarted(id: string): Promise<void>

        /**
         * Save given buffer array of scoreboard data as given file path
         * @param name file path
         * @param arr JSON stringified buffer
         * @returns {string} Error message if any
         */
        exportScoreboard(name: string, arr: string): Promise<string>

        /**
         * Set streamer guess from minimap click for streak games
         * @param lat latitude
         * @param lng longitude
         */
        setStreamerLastStreaksGameGuess(lat: number, lng: number): Promise<void>

        /**
         * End a non-chained game with given game id
         * @param gameId GeoGuessr game id
         * @param challenge wheter game is a challenge game
         */
        endGame(gameId: string, challenge?: boolean): Promise<void>

        /** End an infinite game 
         * @returns {boolean} Success of infinite game being saved to database
         */
        endInfiniteGame(): Promise<boolean>

        /**
         * Set panoid for the latest guess of a player in a round
         * @param round Round number
         * @param player Player name
         * @param pano Panoroma id
         */
        setGuessPanoId(round: number, player: string, pano: string): Promise<void>

        /**
         * Overwrite round correct location information
         * @param round Round number
         * @param lat Latitude
         * @param lng Longitude
         * @param pano Panoroma id
         */
        overwriteRoundData(round: number, lat: number, lng: number, pano: string): Promise<void>

        /** Save current game to client db
         * @returns {boolean} Success state of saving
         * */
        saveGameToClient(): Promise<boolean>

        /** Save current game to server db */
        saveGameToServer(): Promise<void>

        /** Mark main javascript file as completed initializing */
        reportMainJSCompleted(): Promise<void>

        /** Mark next game as an infinite game start */
        markNextStartAsInfinite(): Promise<void>

        /**
         * Mark next round to be a multiguessing round or not
         * @param state Multiguess state
         */
        markNextRoundAsMultiGuess(state: boolean): Promise<void>

        /**
         * Create a game from serialized game data
         * @param serialized serialized game data 
         */
        createGame(serialized: string): Promise<string>

        /** Create the next game in a chain of games */
        createNextGameInChain(): Promise<ChainGameCreateState>

        /** Re-trigger OnStartGame from C# 
         * @returns Wheter event was successfully re-triggered
         */
        reTriggerStartGameEvent(): Promise<boolean>

        /** Get TableOptions serialized */
        getTableOptions(): Promise<string>

        /**
         *  Set new TableOptions
         * @param settingsJson serialized json data
         */
        setTableOptions(settingsJson: string): Promise<void>

        /** Get Scheme handler settings */
        getSchemeSettings(): Promise<string>

        /** Get random coordinates formatted "latitude, longitude" */
        getRandomCoordinates(): Promise<string>

        /** Get random coordinates formatted "latitude, longitude" */
        copyMapLink(): Promise<void>
          /** Get random coordinates formatted "latitude, longitude" */
        playRandomMap(): Promise<void>

        /** Get random coordinates formatted "latitude, longitude" */
        copyResultLink(): Promise<void>

        /** Get OverlaySettings */
        getOverlaySettings(): Promise<string>

        /**
         * Set OverlaySettings
         * @param settingsJson serialized data
         */
        setOverlaySettings(settingsJson: string): Promise<void>

        /**
         * Toggle guess accepting state
         * @param open
         */
        toggleGuesses(open: boolean): Promise<void>

        /**
         * End round after streamer's guess
         * @param gameId GeoGuessr game id
         * @param challenge wheter game is a challenge
         * @param randomStreamerGuess wheter streamer guess was random
         */
        endRound(gameId: string, challenge: boolean, randomStreamerGuess: boolean): Promise<string>

        /** Close the last ended round */
        closeRound(): Promise<void>

        /**
         * Set scoreboard exporting preferences
         * @param format
         * @param alert
         * @param autorounds
         * @param autostandings
         * @param autogames
         */
        setExportPreferences(format: string, alert: boolean, autorounds: boolean, autostandings: boolean, autogames: boolean): Promise<void>
    }

    export interface ICefSharp
    {
        /**
         * Bind an object named 'jsHelper' to current scope for C# method access
         * @param name expects 'jsHelper'
         */
        BindObjectAsync(name: "jsHelper"): Promise<IJSHelper>
    }

    /** CefSharp namespace */
    export const CefSharp: ICefSharp;
    /** C# bound object for method access */
    export const jsHelper: IJSHelper;
}