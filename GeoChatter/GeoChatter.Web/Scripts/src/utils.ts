import { Excel } from "./core";
import { Div } from "./divs";
import { GeoChatter } from "./geochatter";
import { Color } from "./colors";
import { Visual } from "./visuals";
import { Enum } from "./enums"
import { Constant } from "./constants";
import { Setting, State } from "./settings";

export namespace Util
{
    /**
     * Set cookie by name and value for given life time
     * @param name
     * @param value
     * @param days
     */
    export function setCookie(name: string, value: string, days: number)
    {
        var expires = "";
        if (days >= 0)
        {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + (value || "") + expires + "; path=/";
    }

    /**
     * Get cookie by name
     * @param name
     */
    export function getCookie(name: string)
    {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++)
        {
            var c = ca[i];
            if (!c) continue;

            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    export function TooltipTitleWrap(title: number | string, data: string, classes: Nullable<string> = null): string
    {
        return '<span data-toggle="tooltip" title="' + title + '" class="' + (classes ? classes : "") + '">' + data + '</span>';
    }

    export function FormatTimeToString(ms: number, decimals: number = 2): TableCell
    {
        return AsDataTableRowCell((ms / 1000).toFixed(decimals) + "s", ms);
    }

    export function AsDataTableRowCell(display: string, sort: number | string, classes: Nullable<string> = null): TableCell
    {
        return {
            display: TooltipTitleWrap(sort, display, classes),
            sort: sort
        };
    }

    export function CoordinatesToURLArgument(lat: number, lng: number): string
    {
        return `${lat.toString().replace(",", ".")},${lng.toString().replace(",", ".")}`
    }

    export function GetPlayerDataKey(player: PlayerData): string
    {
        return `${player.Platform}_${player.Id}`;
    }

    export function AreTheSamePlayers(player1: PlayerData, player2: PlayerData): boolean
    {
        return player1 && player2 && GetPlayerDataKey(player1) == GetPlayerDataKey(player2)
    }

    export function AreTwoCoordinatesClose(lat1: number, lng1: number, lat2: number, lng2: number, maxdiff: number): boolean
    {
        return Math.abs(lat1 - lat2) <= maxdiff && Math.abs(lng1 - lng2) <= maxdiff;
    }

    /**
     * Fix flag code to be a icon in a span 
     * @param {string} flag flag code 
     * @param {string} name country name
     * @returns {string}
     */
    export function FixFlagHTML(flag: Nullable<string>, name?: Nullable<string>): string
    {
        let fs = (typeof flag === "string" && flag != "");
        return `<div class="flagWrapper"${(fs && typeof name === "string"
            ? (` title="${name} (${flag?.toUpperCase()})" data-tooltip="${name} (${flag?.toUpperCase()})"`)
            : "")}><span class="flag-icon flag-icon-${(fs ? flag : Constant.TransparentFlagCSSSuffix)}"></span></div>`;
    }

    /**
     * Check if given GG score is same as calculated one
     * @param {string} ggScore GG score
     * @param {any} calculated Calculated score
     * @returns {boolean}
     */
    export function IsSameScore(ggScore: Nullable<string>, calculated: string): boolean
    {
        return typeof ggScore === "string"
            && ggScore.replace(",", "") == calculated.toString().replace(",", "")
    }


    /**
     * Convert given distance to displayable string with current settings
     * @param {number} dist distance in kilometers
     */
    export function GetConvertedDistance(dist: number): TableCell
    {
        if (Setting.Overlay.Unit == Enum.UNIT.KM)
        {
            if (dist < 1)
                return AsDataTableRowCell((dist * 1000).toFixed(0) + "m", dist);

            else return AsDataTableRowCell(dist.toFixed(Setting.Overlay.RoundingDigits) + "km", dist);
        }
        else
        {
            dist *= 0.621371;

            if (dist < 1)
                return AsDataTableRowCell((dist * 5280).toFixed(0) + "ft", dist);

            else return AsDataTableRowCell(dist.toFixed(Setting.Overlay.RoundingDigits) + "mi", dist);
        }
    }

    /**
     * Data uri image to svg data uri
     */
    export function DataUrlToMarkerSVGDataUrl(img: string, size: number, color: Nullable<string>, borderSize: number): string
    {
        if (!color) color = Color.RandomColor();
        var half = size / 2;
        return `data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' xmlns:ev='http://www.w3.org/2001/xml-events' version='1.1' baseProfile='full'%3E%3Cdefs%3E%3Cpattern id='img1' patternUnits='userSpaceOnUse' width='${size}' height='${size}'
            %3E%3Cimage xlink:href='${img}' x='${borderSize}' y='${borderSize}' width='${size - borderSize}' height='${size - borderSize}'
            /%3E%3C/pattern%3E%3C/defs%3E
            %3Ccircle style='stroke: ${color.replace("#", "%23")};stroke-width: ${borderSize}px;' cx='${half + 1}' cy='${half + 1}' r='${half}' fill='url(%23img1)'/%3E%3C/svg%3E`;
    }

    /**
    * Image uri to data uri for performance
    * @param {any} url image url
    * @param {any} callback callback function to call with resulting data
    */
    export async function ImageUrlToDataUrl(url: string, callback: Callback<string | ArrayBuffer | null, void>)
    {
        if (!url.endsWith(".png"))
        {
            let blob = await fetch(url).then(r => r.blob());
            let dataUrl: string | ArrayBuffer | null = await new Promise(resolve =>
            {
                let reader = new FileReader();
                reader.onload = () => resolve(reader.result);
                reader.readAsDataURL(blob);
            });
            callback(dataUrl);
        }
        else
        {
            var xmlHTTP = new XMLHttpRequest();
            xmlHTTP.open('GET', url, true);
            xmlHTTP.responseType = 'arraybuffer';
            xmlHTTP.onload = function()
            {
                var arr = new Uint8Array(this.response);
                var binary = '';
                var len = arr.byteLength;
                for (var i = 0; i < len; i++)
                {
                    binary += String.fromCharCode(arr[i] ?? 0);
                }
                var b64 = btoa(binary);
                callback("data:image/png;base64," + b64);
            };
            xmlHTTP.send();
        }
    }

    export function DecimalToHex(d: any, padding: number): string
    {
        var hex = Number(d).toString(16);
        padding = typeof (padding) === "undefined" || padding === null ? padding = 2 : padding;

        while (hex.length < padding)
        {
            hex = "0" + hex;
        }

        return hex;
    }

    export function ClickElement(selector: string): void
    {
        let el = $(selector)[0];
        if (el)
        {
            el.dispatchEvent(new PointerEvent("click", {
                pointerId: -1,
                bubbles: true,
                view: window
            }))
        }
    }
    
    export async function SetExportSettings()
    {
        await CefSharp.BindObjectAsync('jsHelper');
        await jsHelper.setExportPreferences(Setting.App.PreferredExportFormat, State.App.AlertExportSuccess, State.App.AutoExportRoundResults, State.App.AutoExportRoundStandings, State.App.AutoExportGameResults);
    }

    export async function Zoom(a?: number) : Promise<void>
    {
        await CefSharp.BindObjectAsync('jsHelper');

        await jsHelper.zoom(a ?? 0);
    }


    export async function MarkNextRoundAsMultiGuess(this: JQueryEventObject): Promise<void>
    {
        let v = $(this).is(":checked");
        console.log("Marking next round multiguess: " + v);
        State.App.PreferredMultiguess = v;

        await CefSharp.BindObjectAsync('jsHelper');

        await jsHelper.markNextRoundAsMultiGuess(v);
    }

    export async function MarkNextStartAsInfinite(): Promise<void>
    {
        await CefSharp.BindObjectAsync('jsHelper');

        await jsHelper.markNextStartAsInfinite();

        ClickElement("[data-qa='start-game-button']");
    }

    /** Check if current path is a 5 round game path
     *  @returns {boolean} */
    export function IsInA5RoundGame(): boolean
    {
        return location.pathname.startsWith("/game/");
    }

    /** Check if current path is a challenge game path
     *  @returns {boolean} */
    export function IsInAChallengeGame(): boolean
    {
        return location.pathname.startsWith("/challenge/");
    }

    /** Check if current path is in game
     *  @returns {boolean} */
    export function IsCurrentlyInGame() : boolean
    {
        return location.pathname.startsWith("/results/") || IsInA5RoundGame() || IsInAChallengeGame();
    }

    export async function GoToPage(path: string)
    {
        await CefSharp.BindObjectAsync('jsHelper');

        await jsHelper.goingToRefresh(path.startsWith("https://") ? path : (location.origin + "/" + path));
    }

    export async function GoToMainMenu()
    {
        await GoToPage("/classic");
    }

    export async function GoToMapPage(mapname: string)
    {
        await GoToPage(`/maps/${mapname}/play`);
    }

    /**
    * @returns {string}
    * @param {Round} round
    */
    export function GetInfoWindowHtmlForCluster(names: Array<string>): string
    {
        return `<div class="gm-iw-custom" style='border: 3px solid #ff0000 !important;'>
                    <div style="font-size:${Setting.Overlay.FontSize}${Setting.Overlay.FontSizeUnit}">
                        GUESSES AROUND AREA (${names.length})
                    </div>
                    ${(names ? names.join("<br/>") : "")}
                </div>`;
    }

    export function GetTableRowOf(player: PlayerData): Nullable<JQuery<HTMLElement>>
    {
        if (!GeoChatter.Main.Table) return;

        let rows = GeoChatter.Main.Table.getVisibleRows();
        let len = rows.length;
        for (let i = 0; i < len; i++)
        {
            let r = rows[i];
            if (r && r.data && AreTheSamePlayers((r.data as TableRow).Player, player))
                return Div.GetTableRow(i);
        }
        return null;
    }

    export async function SaveGame(): Promise<void>
    {
        await CefSharp.BindObjectAsync('jsHelper');
        await jsHelper.saveGameToServer();
        let status = await jsHelper.saveGameToClient();
        if (status)
        {
            console.log("Saved game to client DB");
        }
        else
        {
            console.error("Failed to save game to client DB");
            alert("Something went wrong while saving the game and results page won't be available. Please use 'Export' action if you want to save the game.")
        }
    }

    export function TriggerSendGuess()
    {
        var b = Div.Get_GuessButton();

        if (b && !b.disabled)
        {
            State.App.NextStreamerGuessIsRandom = true;
            b.dispatchEvent(new PointerEvent("click", {
                pointerId: -1,
                bubbles: true,
                view: window
            }))
        }
    }

    export function RepeatCallbackAttempt(callback: Callback<void, void>, times: number, intervals: number, callOnceFirst: boolean = false): void
    {
        if (callOnceFirst) callback()

        for (let i = 0; i < times; i++)
        {
            window.window.setTimeout(callback, intervals * (i + 1));
        }
    }

    export function Stopscroller(): void
    {
        State.Scoreboard.IsScrolling = false;
        $(Constant.DataGridRowView).stop();
    }

    export function Scroller(elem?: string): void
    {
        const div = $(elem ?? "");

        const loop = () =>
        {
            if (!div[0]) return;

            div.stop();

            if (!State.Scoreboard.IsScrollingEnabled || !State.Scoreboard.IsScrolling) return;
            //console.log("Outer", div[0].scrollHeight, div.scrollTop())
            div.animate(
                {
                    scrollTop: div[0].scrollHeight
                },
                ((div[0].scrollHeight - (div.scrollTop() ?? 0)) * 1000 * 2) / Setting.Overlay.ScrollSpeed, "linear",
                () =>
                {
                    //console.log("1-Inner")
                    window.setTimeout(() =>
                    {
                        if (!div[0]) return;
                        div.stop()
                        //console.log("Timeout-1-Inner", div[0].scrollHeight, div.scrollTop())

                        if (State.Scoreboard.IsScrollingEnabled && State.Scoreboard.IsScrolling)
                        {
                            div.animate(
                                {
                                    scrollTop: 0
                                },
                                1000, "swing",
                                () =>
                                {
                                    //console.log("2-Inner")
                                    window.setTimeout(() =>
                                    {
                                        //console.log("Timeout-2-Inner")
                                        loop();
                                    }, 4000)
                                }
                            );
                        }
                    }, 1000);
                }
            );
        };
        loop();
    }

    function ToggleGuessButtonState(disable?: boolean)
    {
        var cb = document.getElementById(Constant._id_guess_toggle_slider) as HTMLInputElement;
        if (!cb) return;

        cb.disabled = disable == null ? !cb.disabled : disable;
    }

    export async function ToggleGuesses()
    {
        var cb = document.getElementById(Constant._id_guess_toggle_slider) as HTMLInputElement;
        if (!cb) return;

        ToggleGuessButtonState(true);

        await CefSharp.BindObjectAsync('jsHelper');

        await jsHelper.toggleGuesses(cb.checked).then(() => ToggleGuessButtonState(false)).catch(() => ToggleGuessButtonState(false));
    }

    export function AutoexportFuncFor(stage: GAMESTAGE): Callback<ExportFormat>
    {
        switch (stage)
        {
            case Enum.GAMESTAGE.ENDROUND:
                {
                    return () => { if (State.App.AutoExportRoundResults) { ExportCurrentScoreboardAs(Setting.App.PreferredExportFormat); } };
                }
            case Enum.GAMESTAGE.ENDGAME:
                {
                    return () => { if (State.App.AutoExportGameResults) { ExportCurrentScoreboardAs(Setting.App.PreferredExportFormat); } };
                }
            default:
                {
                    return () => { console.error("Attempted to export for stage: " + stage) };
                }
        }
    }

    export function GetExportScoreboardFileName(): {redacted: string, name: string}
    {
        if (!GeoChatter.Main.CurrentGame) return { redacted: "", name: "" };

        let stage = State.Scoreboard.DisplayingCurrentStandings ? Enum.GAMESTAGE.ENDGAME : GeoChatter.Main.CurrentGame.Stage;
        switch (stage)
        {
            case Enum.GAMESTAGE.ENDGAME:
                {
                    if (State.Scoreboard.DisplayingCurrentStandings)
                    {
                        return {
                            redacted: `GeoChatter-${GeoChatter.Main.CurrentGame.Mode}-${GeoChatter.Main.CurrentGame.ID.slice(0, 4)}***-STANDINGS-${GeoChatter.Main.CurrentGame.TotalRoundCount}`,
                            name: `GeoChatter-${GeoChatter.Main.CurrentGame.Mode}-${GeoChatter.Main.CurrentGame.ID}-STANDINGS-${GeoChatter.Main.CurrentGame.TotalRoundCount}`
                        };
                    }
                    else
                    {
                        return {
                            redacted: `GeoChatter-${GeoChatter.Main.CurrentGame.Mode}-${GeoChatter.Main.CurrentGame.ID.slice(0, 4)}***`,
                            name: `GeoChatter-${GeoChatter.Main.CurrentGame.Mode}-${GeoChatter.Main.CurrentGame.ID}`
                        };
                    }
                }
            default:
                {
                    return {
                        redacted: `GeoChatter-${GeoChatter.Main.CurrentGame.Mode}-${GeoChatter.Main.CurrentGame.ID.slice(0, 4)}***-${GeoChatter.Main.CurrentGame.Stage}-${GeoChatter.Main.CurrentGame.TotalRoundCount}`,
                        name: `GeoChatter-${GeoChatter.Main.CurrentGame.Mode}-${GeoChatter.Main.CurrentGame.ID}-${GeoChatter.Main.CurrentGame.Stage}-${GeoChatter.Main.CurrentGame.TotalRoundCount}`
                    };
                }
        }
    }

    function ExportCustumizeCell(options: { gridCell?: DevExpress.excelExporter.DataGridCell, excelCell?: Excel.CellModel })
    {
        if (!GeoChatter.Main.CurrentGame) return

        var { gridCell, excelCell } = options;

        if (excelCell && gridCell?.rowType === 'data')
        {
            let val;
            if (gridCell.column?.dataField == "Guess")
            {
                switch (GeoChatter.Main.CurrentGame.Stage)
                {
                    case Enum.GAMESTAGE.ENDROUND:
                        {
                            let round = GeoChatter.Main.CurrentGame.CurrentRound;
                            if (round?.Results)
                            {
                                let guess = round.GetGuessOf((gridCell.data as TableRow).Player);

                                if (guess) val = guess.Location.ExactCountryCode.toLowerCase()
                                else val = "X";
                            }
                            else val = "X";
                            break;
                        }
                    default:
                        {
                            val = "X";
                            break;
                        }
                }
            }
            else if (gridCell.column?.dataField == "Guesses")
            {
                let stg = State.Scoreboard.DisplayingCurrentStandings ? Enum.GAMESTAGE.ENDGAME : GeoChatter.Main.CurrentGame.Stage;
                switch (stg)
                {
                    case Enum.GAMESTAGE.ENDGAME:
                        {
                            let fl = "";
                            let rs = GeoChatter.Main.CurrentGame.AllRounds;
                            let len = rs.length;
                            for (let i = 0; i < len; i++)
                            {
                                let round = rs[i];
                                if (round?.Results)
                                {
                                    let guess = round.GetGuessOf((gridCell.data as TableRow).Player);

                                    let tag;

                                    if (guess) tag = guess.Location.ExactCountryCode.toLowerCase()
                                    else tag = "X";

                                    if (i != len - 1) tag += ", "
                                    fl += tag;
                                }
                            }
                            val = fl;
                            break;
                        }
                    default:
                        {
                            val = "X";
                            break;
                        }
                }
            }
            else if (gridCell.column?.dataField == "TimeTaken")
            {
                val = (gridCell.value / 1000).toFixed(Setting.Overlay.RoundingDigits);
            }
            else val = gridCell.value;

            excelCell.value = val
        }
    }

    type WorkBook = Excel.Workbook;

    async function ExportWriter(fullname: string, buffer: Excel.Buffer)
    {
        await CefSharp.BindObjectAsync('jsHelper');

        var msg = await jsHelper.exportScoreboard(fullname, JSON.stringify(buffer))
        if (!msg)
        {
            return true;
        }
        else
        {
            alert("Something went wrong while exporting: " + msg)
            return null;
        }
    }

    function ExportWriterWrapper(workbook: WorkBook, fullname: string, ext: ExportFormat)
    {
        return (workbook[ext]).writeBuffer()
            .then((buffer: Excel.Buffer) => ExportWriter(fullname, buffer))
    }

    function ExportCurrentGameDataTo(_workbook: WorkBook, _fullname: string, redacted: string, ext: ExportFormat)
    {
        // TODO: Add game data sheet
        if (State.App.AlertExportSuccess)
        {
            alert(`Exported the scoreboard to '/GeoChatter/exports/${redacted}.${ext}' successfully! (File name partially hidden)`)
        }
    }

    export function ExportCurrentScoreboardAs(ext: ExportFormat)
    {
        let { redacted, name } = GetExportScoreboardFileName();
        let fullname = `${name}.${ext}`

        console.log("Exporting scoreboard as " + fullname)

        switch (ext)
        {
            case Enum.ExportFormat.csv:
            case Enum.ExportFormat.xlsx:
                {
                    if (!GeoChatter.Main.Table) return;

                    const workbook = new Excel.Workbook();
                    const worksheet = workbook.addWorksheet('Scoreboard');

                    DevExpress.excelExporter.exportDataGrid({
                        component: GeoChatter.Main.Table,
                        worksheet,
                        autoFilterEnabled: true,
                        loadPanel: {
                            text: "EXPORTING AS " + ext + "..."
                        },
                        customizeCell: ExportCustumizeCell
                    })
                        .then(() => ExportWriterWrapper(workbook, fullname, ext))
                        .then((res) => res
                            ? ExportCurrentGameDataTo(workbook, fullname, redacted, ext)
                            : null);
                    break;
                }
            default:
                {
                    alert("Unknown export format: " + ext);
                    break;
                }
        }
    }

    export async function SaveAllScoreboardSettingsForStage(stage: Nullable<GAMESTAGE>): Promise<void>
    {
        if (stage == null) stage = Enum.GAMESTAGE.INROUND;

        if (!GeoChatter.Main.CurrentGame
            || !GeoChatter.Main.Table
            || !GeoChatter.Main.Table.element
            || !GeoChatter.Main.Table.element()) return;

        let sc = GeoChatter.Main.Table.element();
        Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Width = sc.width() ?? Constant.MinimumTableWidth;
        if (!Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].IsMinimized) Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Height = sc.height() ?? 60;
        Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Top = sc.parent().position().top;
        Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Left = sc.parent().position().left;

        Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].MinimapLayer = GeoChatter.Map?.getMapTypeId() ?? "roadmap";
        await SaveTableOptions();
    }
    
    export async function SaveTableOptions(): Promise<void>
    {
        await CefSharp.BindObjectAsync('jsHelper');

        let opts = [];

        for (let [mode, modesettings] of Object.entries(GeoChatter.Main.GameTableOptions))
        {
            let mod = mode as GAMEMODE
            const modeobj: TableOptionsClassJSON = {
                Mode: mod,
                Stages: []
            };

            for (let [stage, stagesettings] of Object.entries(modesettings))
            {
                let stg = stage as GAMESTAGE
                const stobj: TableOptionsClassStage = {
                    Stage: stg,
                    Top: Setting.ScoreboardDisplay[mod][stg].Top,
                    Left: Setting.ScoreboardDisplay[mod][stg].Left,
                    Width: Setting.ScoreboardDisplay[mod][stg].Width,
                    Height: Setting.ScoreboardDisplay[mod][stg].Height,
                    IsMinimized: Setting.ScoreboardDisplay[mod][stg].IsMinimized,
                    ShowRowNumbers: Setting.ScoreboardDisplay[mod][stg].ShowRowNumbers,
                    MinimapLayer: Setting.ScoreboardDisplay[mod][stg].MinimapLayer,
                    Columns: []
                }
                for (let [id, colobj] of Object.entries(stagesettings))
                {
                    let o: GameTableColumnOptions = {
                        Position: colobj.Position,
                        DataField: id as DataField,
                        Width: typeof colobj.Width === "number" ? colobj.Width : 0,
                        Name: colobj.Name,
                        Visible: colobj.Visible,
                        Sortable: colobj.Sortable,
                        SortOrder: !colobj.SortOrder ? "" : colobj.SortOrder,
                        SortIndex: colobj.SortIndex == undefined ? -1 : colobj.SortIndex,
                        DefaultSortIndex: colobj.DefaultSortIndex,
                        DefaultSortOrder: !colobj.DefaultSortOrder ? "" : colobj.DefaultSortOrder,
                        AllowedWithMultiGuess: colobj.AllowedWithMultiGuess,
                    };
                    stobj.Columns.push(o);
                }
                modeobj.Stages.push(stobj);
            }
            opts.push(modeobj);
        }
        let st = JSON.stringify(opts);

        jsHelper.setTableOptions(st)
            .then(() =>
            {
                console.log("SAVED TABLE OPTIONS");
                Visual.RefreshColumnDisplays();
            })
            .catch((e) =>
            {
                console.error("FAILED TO SAVE TABLE OPTIONS", e);
            });
    }
}

window.GC.Util = Util;