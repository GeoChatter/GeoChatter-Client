import "./core";
import { Control } from "./controls";
import { Div } from "./divs";
import { GeoChatter } from "./geochatter";
import { MapUtil } from "./maps";
import { Guess } from "./models";
import { Util } from "./utils";
import { Enum } from "./enums"
import { Constant } from "./constants";
import { Setting, State } from "./settings";
import { Dependency } from "./dependencies";

export namespace Visual
{
    export const AnimationOptions = {
        AddNewGuessFadeIn: "slow",
        ChangeGuessFadeIn: "fast",
        InitialScoreboardRows: "slow",
    }

    export const DefaultCSSTargets = [
        ".dx-datagrid",
        "#" + Constant._id_datatable,
        ".dx-button-mode-contained",
        ".dx-texteditor.dx-editor-outlined",
        ".dx-datagrid-headers",
        ".dx-datagrid-filter-row",
        ".dx-datagrid-filter-row input",
        ".dropdown-content button"
    ] as const;

    export function PlatformCSSFromPlatform(platform: PlayerPlatform): string
    {
        switch (platform)
        {
            case Enum.Platform.Twitch:
                {
                    return "platform-twitch";
                }
            case Enum.Platform.Youtube:
                {
                    return "platform-youtube";
                }
            default:
                {
                    return "platform-other";
                }
        }
    }

    export function ApplyCssTo(jqelement: JQuery, fontsize: string, color: string, bgcolor: string)
    {
        jqelement.css("font-size", fontsize)
        jqelement.css("color", color + " !important");
        jqelement.css("background-color", bgcolor + " !important");
    }

    export function ApplyDefaultCssTo(jqelement: JQuery)
    {
        jqelement.css("font-size", Setting.Overlay.FontSize + Setting.Overlay.FontSizeUnit)
        jqelement.css("color", Setting.Overlay.ScoreboardForeground + Util.DecimalToHex(Setting.Overlay.ScoreboardForegroundA, 2) + " !important");
        jqelement.css("background-color", Setting.Overlay.ScoreboardBackground + Util.DecimalToHex(Setting.Overlay.ScoreboardBackgroundA, 2) + " !important");
    }

    export function SetDisplayOptions()
    {
        if (!$) return;

        for (let sc of DefaultCSSTargets)
        {
            let obj = $(sc);
            if (obj[0])
            {
                ApplyDefaultCssTo(obj);
            }
        }

        let tb = $(".dx-toolbar")
        if (tb[0])
        {
            ApplyCssTo(tb,
                Setting.Overlay.FontSize + Setting.Overlay.FontSizeUnit,
                Setting.Overlay.ScoreboardForeground + Util.DecimalToHex(Setting.Overlay.ScoreboardForegroundA, 2),
                "#00000000"
            );
        }

        let info = $(".informerguess")
        if (info[0])
        {
            ApplyCssTo(info,
                Setting.Overlay.FontSize + Setting.Overlay.FontSizeUnit,
                Setting.Overlay.ScoreboardForeground + Util.DecimalToHex(Setting.Overlay.ScoreboardForegroundA, 2),
                "#00000000"
            );
        }
        Util.Zoom();
    }

    export function RemoveLoadingScreen()
    {
        if ($(".gc-loading-screen")[0])
        {
            $(".gc-loading-screen").remove();
        }
    }

    export function AddLoadingScreen(message: string = "Loading...")
    {
        // TODO: Enable this
        return
        if (!$(".gc-loading-screen")[0] && Dependency.Body)
        {
            Dependency.Body.appendChild(
                $("<div>")
                    .addClass("gc-loading-screen")
                    .html(`
            <div class="gc-loading-screen-body">
                <div class="gc-loading-spinner-container">
                    <div class="gc-loading-spinner-square">
                        <div class="gc-loading-spinner-square-inner" style="padding-top: 100%;">
                            <img src="data:image/png;base64,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" class="gc-loading-spinner-circle">
                            <div class="gc-loading-spinner-globe-container">
                                <img src="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 83 42' fill='none'%3E%3Cscript xmlns=''/%3E%3Cpath d='M3.12019 9.51808H1.44327L0.5 8.89179V7.01293H2.38654L1.75769 4.50778L12.5529 5.86473L13.601 4.50778L18.6317 4.9253L18.0029 7.01293L15.9067 8.2655L17.4789 8.99617L18.0029 8.36988L20.7279 7.84798L21.5664 9.51808H22.5096V12.3364L18.0029 11.8145L18.3173 13.4846L14.649 15.781V17.2423H14.0202L13.0769 15.9897L10.876 16.616L10.1423 18.1817L11.5048 18.7036V19.8518L13.0769 20.2694L13.1817 21.4175H17.8981L19.051 22.6701H20.5183V23.8183L22.9289 24.1315L24.8154 27.2629L23.0337 29.0374L23.4529 31.4382L18.9462 35.3003L18.8414 38.8492L20.0991 39.4755L19.4702 41.25L16.9548 39.893L17.2692 38.1186L16.2212 37.3879V30.9163L14.0202 29.6637L14.649 27.8892L12.6577 26.1147V24.3402L13.4962 24.1315L13.9154 22.8789L13.2865 22.2526L12.2385 22.8789L12.0289 21.4175L10.5615 21V20.4781L6.78846 19.5387L7.41731 18.1817L5.42596 16.3029L5.8452 11.8145L6.99808 11.7101L7.2077 9.9356L6.26443 8.47427L3.32981 8.2655L3.12019 9.51808Z' fill='white' fill-opacity='0.1'/%3E%3Cpath d='M21.9856 7.22164L19.8894 6.80412L20.3087 5.44716L19.3654 3.5683V2.4201L25.1298 0.75L28.7981 3.04639L29.3221 5.55154L26.3875 6.69974L24.8154 9.51803L23.5577 9.1005L21.9856 7.22164Z' fill='white' fill-opacity='0.1'/%3E%3Cpath d='M82.25 7.22162L81.5163 5.96904L77.2192 6.59533L75.9615 5.13399L70.6163 4.19456L66.0048 5.13399L63.8038 3.04636H56.1529L54.4759 4.19456V5.13399L52.275 5.76028L51.9606 5.13399H47.9779L44.3096 3.88141L41.6894 5.13399L40.7461 7.63914L41.4798 8.9961H43.0519L43.3663 7.95229L44.5192 7.53476L44.7288 10.1443L41.7942 11.1881L41.4798 9.72677L40.1173 9.51801V11.1881L38.3355 11.71L37.6019 12.8582L38.1259 13.902H36.449L36.8682 16.094H38.5452L38.8596 14.9458L40.4317 15.0502L41.4798 14.4239L42.5279 15.6765L42.1086 16.8247L43.4711 16.1984L42.8423 14.4239L43.4711 14.1108L44.9384 15.9896L46.5106 15.3634L47.8731 16.8247L47.5586 18.1817L44.1 19.0167L42.1086 18.1817L41.4798 16.8247L38.5452 16.7203L34.3529 20.0605L34.0384 22.4613L33.4096 22.8788V25.1752L35.925 27.2628H39.698L41.0606 28.0979L41.1654 30.1855L42.7375 30.9162L42.2134 35.4046L43.3663 37.0747L43.5759 39.7886L46.4057 40.3105L49.2355 37.5966L49.1307 34.3608L51.1221 32.795L51.2269 28.9329L53.2182 26.8453L52.9038 25.2796L50.2836 24.5489L50.1788 22.8788L49.1307 21.7306L49.2355 20.0605H49.9692L51.7509 22.4613L51.6461 23.8183L52.5894 24.1314L55.524 22.3569L53.3231 19.0167L55.2096 18.9123L55.6288 20.1649L57.8298 21.7306L59.4019 22.3569L59.1923 24.5489L62.2317 27.3672L64.1182 22.2525L65.7952 22.5657L66.9481 25.6971L68.1009 26.8453L67.9961 24.7577H68.5202L69.149 25.4883L70.9308 24.4445L70.8259 20.5824L72.3981 20.1649L72.9221 18.1817L71.35 16.4072L72.2933 14.7371H73.1317L75.8567 8.89172L76.9048 9.51801V11.1881L77.9529 11.71L79.2106 8.9961L82.25 7.22162Z' fill='white' fill-opacity='0.1'/%3E%3Cpath d='M53.428 32.5863H52.9039L51.9606 34.0476V36.0309L52.6943 36.8659L53.428 36.4484L54.1616 33.0038L53.428 32.5863Z' fill='white' fill-opacity='0.1'/%3E%3Cpath d='M37.6019 10.8749L38.7548 10.4574V8.47417H37.9163L37.6019 9.51798L36.5538 9.62236V10.6662L37.6019 10.8749Z' fill='white' fill-opacity='0.1'/%3E%3Cpath d='M72.6077 31.6469L72.0837 31.0206L70.302 31.3337L69.3587 32.5863L67.577 33.4213L67.7866 36.4484H70.9308L71.8741 37.1791L74.2847 37.701L75.6471 36.0309V33.9432L74.2847 31.0206L73.551 30.7074V31.8556L72.6077 31.6469Z' fill='white' fill-opacity='0.1'/%3E%3Cpath d='M74.8086 13.9021L75.3326 15.2591L74.1798 18.0773L74.8086 19.2255H75.6471L77.0096 16.8248L76.5903 13.9021L75.6471 13.3802L74.8086 13.9021Z' fill='white' fill-opacity='0.1'/%3E%3Cpath d='M77.4288 35.7177L76.7999 36.6572L76.1711 37.701L74.1798 39.2667L74.599 40.3105H76.2759L77.0096 38.0141L78.3721 36.8659L77.4288 35.7177Z' fill='white' fill-opacity='0.1'/%3E%3C/svg%3E" class="gc-loading-spinner-globe">
                            </div>
                        </div>
                    </div>
                    <div class="fullscreen-spinner_label__oFLtU">${message}</div>
                </div>
            </div>`)[0] as any)
        }
    }

    export function RefreshColumnDisplays(stage?: GAMESTAGE): number
    {
        if (GeoChatter.Main.Table == null || !$ || !GeoChatter.Main.CurrentGame) return window.setTimeout(() => RefreshColumnDisplays(stage), 100);
        if (stage == null)
        {
            stage = GeoChatter.Main.CurrentGame.Stage;
        }

        console.log("Refreshing column display")

        let headercolgroups = $("#datatable .dx-datagrid-headers table colgroup").children();
        let rowcolgroups = $("#datatable .dx-datagrid-rowsview table colgroup").children();
        let idx = 0;
        try
        {
            for (const [_, displayColumn] of Object.entries(GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stage]))
            {
                if (State.App.PreferredMultiguess && !displayColumn.AllowedWithMultiGuess) GeoChatter.Main.Table.columnOption(displayColumn.DataField, "visible", false);
                else GeoChatter.Main.Table.columnOption(displayColumn.DataField, "visible", displayColumn.Visible);

                idx = GeoChatter.Main.Table.getVisibleColumnIndex(displayColumn.DataField);

                if (displayColumn.Visible && idx >= 0)
                {
                    let hcg = headercolgroups[idx];
                    let rcg = rowcolgroups[idx];
                    if (hcg) hcg.style.width = `${Math.max(Constant.MinimumColumnWidth, displayColumn.Width)}px`;
                    if (rcg) rcg.style.width = `${Math.max(Constant.MinimumColumnWidth, displayColumn.Width)}px`;
                }
            }
            return 0;
        }
        catch (e)
        {
            console.error("Column refresh error: " + e, stage, GeoChatter.Main.CurrentGame.Stage)
            return -1;
        }
    }

    export function SetScoreboardTitleCount(count: Nullable<number> = null)
    {
        if (count == null)
        {
            count = GeoChatter.Main.CurrentGame?.CurrentRound?.Guesses.length;
        }
        $("#guessCountHeader").text(count ?? "-")
    }

    export async function PostInfinityGameButtons()
    {
        $('#nextRoundInfinityButton').hide();
        $('#endGameInfinityButton').hide();
        $('#roundSettingsContainer').remove();

        let mapid = GeoChatter.Main.CurrentGame?.Map.ID ?? "world";

        let playagain = $("<button>")
            .attr("id", "playAgainButton")
            .css("background", "#6cb928")
            .css("margin-bottom", "0.5rem")
            .css("margin-left", "0.5rem")
            .attr("type", "button")
            .addClass("button_button__CnARx")
            .append(Control.ButtonLabel("Play Again"))
            .on("click", async () => await Util.GoToMapPage(mapid))

        if (!State.App.PlayAgainEnabled) playagain.css("display", "none")

        let mainmenu = $("<button>")
            .attr("id", "goToMainmenuButton")
            .css("background", "transparent")
            .css("border", ".0625rem solid hsla(0,0%,100%,.8)")
            .css("margin-bottom", "0.5rem")
            .css("margin-left", "0.5rem")
            .attr("type", "button")
            .addClass("button_button__CnARx")
            .append(Control.ButtonLabel("Main Menu"))
            .on("click", Util.GoToMainMenu)

        let target = $('[data-qa="play-same-map"]')

        if (!target[0])
        {
            target = $('[data-qa="close-round-result"]')
                .parent()
        }

        $(target[0] ?? target)
            .parent()
            .append(playagain, mainmenu, await Control.RoundSettingsContainer(false))
    }

    export async function SetEndScreenButtons()
    {
        if (GeoChatter.Main.CurrentGame?.Settings.IsInfinite)
        {
            let nextRoundBtn = $("<button>")
                .attr("id", "nextRoundInfinityButton")
                .css("background", "#6cb928")
                .css("margin-bottom", "0.5rem")
                .css("margin-left", "0.5rem")
                .attr("type", "button")
                .addClass("button_button__CnARx")
                .append(Control.ButtonLabel("Next Round"))
                .on("click", InfinityGameNextRoundClick)

            let endGameBtn = $("<button>")
                .attr("id", "endGameInfinityButton")
                .css("background", "purple")
                .css("margin-bottom", "0.5rem")
                .css("margin-left", "0.5rem")
                .attr("type", "button")
                .addClass("button_button__CnARx")
                .append(Control.ButtonLabel("End Infinite Game!"))
                .on("click", InfinityGameEndGameClick)

            let target = $('[data-qa="play-same-map"]')

            if (!target[0])
            {
                target = $('[data-qa="close-round-result"]')
                    .parent()
            }

            $(target[0] ?? target)
                .parent()
                .append(nextRoundBtn, endGameBtn, await Control.RoundSettingsContainer(false))
        }
        else
        {
            $(".button_link__xHa3x")
                .css("display", "inline-block");

            if (GeoChatter.Main.CurrentGame?.Mode == Enum.GAMEMODE.STREAK && GeoChatter.Main.CurrentGame?.Stage != Enum.GAMESTAGE.ENDGAME)
            {
                $(".buttons_buttons__0B3SB")
                    .children("a")
                    .hide()
            }

            let target = $('[data-qa="play-same-map"]')
                .css("display", "inline-block")

            if (!target[0])
            {
                target = $('[data-qa="close-round-result"]')
                    .css("display", "inline-block")
                    .parent()
            }

            $(target[0] ?? target)
                .parent()
                .append(await Control.RoundSettingsContainer(false));
        }
    }

    export function ReEnableNavigationButtons(delay = 500)
    {
        return window.setTimeout(SetEndScreenButtons, delay)
    }

    async function InfinityGameNextRoundClick(this: JQueryEventObject)
    {
        if ($(this).prop("disabled")) return;

        $(this).prop("disabled", true);
        $('#endGameInfinityButton').hide();

        if (GeoChatter.Main.CurrentGame?.CurrentRound?.ID == 5)
        {
            await CefSharp.BindObjectAsync('jsHelper');

            let res = await jsHelper.createNextGameInChain();
            if (res == -2)
            {
                console.error("Failed to create next game in chain");
                alert("Failed to process infinite game process. App restart and starting a new game is required.")
            }
            else if (res == -1)
            {
                console.error("Failed to save games in chain");
                alert("Failed to process infinite game process, rounds played from now on may not be saved to database properly. App restart and starting a new game is advised.")
            }
            else if (res == 0)
            {
                console.error("Failed to create game in chain due to internal error");
                alert("Failed to process infinite game process. App restart and starting a new game is required.")
            }
            else
            {
                console.log("Loading next game in chain")
            }
        }
        else
        {
            Util.ClickElement('[data-qa="close-round-result"]');
        }
    }

    async function InfinityGameEndGameClick(this: JQueryEventObject)
    {
        if ($(this).prop("disabled") || !confirm(`Are you sure you want to end the infinite game at round ${GeoChatter.Main.CurrentGame?.TotalRoundCount}?`)) return;

        $(this).prop("disabled", true);
        $('#nextRoundInfinityButton').hide();

        $('#endGameInfinityButton')
            .hide()
            .append(
                $("<a>")
                    .data("qa", "main-menu-infinity-button")
                    .addClass("button_link__xHa3x button_variantSecondary__lSxsR")
                    .append(Control.ButtonLabel("Main Menu"))
            );

        await CefSharp.BindObjectAsync('jsHelper');

        let result = await jsHelper.endInfiniteGame();

        if (!result)
        {
            alert("Failed to save infinite game progress, rounds from now on may not be saved correctly to the database.");
        }
    }

    export function ToggleMinimized()
    {
        if (!GeoChatter.Main.CurrentGame) return

        SetScoreboardMinimized(!Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][GeoChatter.Main.CurrentGame.Stage].IsMinimized)
    }

    export function SetScoreboardMinimized(s: boolean)
    {
        if (!GeoChatter.Main.CurrentGame) return
        Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][GeoChatter.Main.CurrentGame.Stage].IsMinimized = s;
        if (s)
        {
            $("#minimalizedState").removeClass("toggle-state-off").addClass("toggle-state-on").text("Show");
            $("#resizeable_container").height($("#draghandle").height() ?? 60);
            $("#collapseState").text("HIDDEN").attr('data-tooltip', 'Use "Actions" menu to show/hide rows')
            console.log("Minimizing scoreboard");
        }
        else
        {
            $("#minimalizedState").removeClass("toggle-state-on").addClass("toggle-state-off").text("Hide");
            $("#resizeable_container").height(Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][GeoChatter.Main.CurrentGame.Stage].Height);
            $("#collapseState").text("").attr('data-tooltip', '')
            console.log("Maximizing scoreboard");
        }
    }

    /**
     * Remove scoreboard div
     * @param {GAMESTAGE} stage game stage
     * @param {boolean} removeMarkers wheter to remove markers and polylines too
     */
    export function RemoveScoreboard(stage: Nullable<GAMESTAGE>, removeMarkers: boolean = true)
    {
        try
        {
            console.log("Removing the scoreboard", stage)

            if (GeoChatter.Main.Table != null) GeoChatter.Main.Table.element().dxDataGrid("dispose");
            GeoChatter.Main.Table = null;

            if (GeoChatter.Main.ScoreboardParentContainer != null) GeoChatter.Main.ScoreboardParentContainer.remove();
            GeoChatter.Main.ScoreboardParentContainer = null;

            $("#scoreboardContainer").remove()

            if (removeMarkers) MapUtil.ResetMarkersAndScores();

            while (GeoChatter.Main.SingleOnContentReadyWeakCallbacks[0]) GeoChatter.Main.SingleOnContentReadyWeakCallbacks.pop();
            //playersToFadeIn = {};
            State.Scoreboard.ShouldUseTableOptionsWidth = false;
            State.Scoreboard.ShouldUseLastScoreBoardSettings = false;
            State.Scoreboard.SubtitleClickReady = true;
            State.Scoreboard.DisplayingCurrentStandings = false;
            State.App.HideMarkerState = true;
            State.App.NextStreamerGuessIsRandom = false;

            Util.Stopscroller();
        }
        catch (e)
        {
            console.error(e)
        }
    }
    /**
     * Re-initialize the overlays
     * @param {GAMESTAGE} stage game stage
     * @param {string|null} title new scoreboard title, null for new round default title
     * @param {boolean} removeMarkers wheter to remove markers and polylines
     */
    export function ResetOverlays(stage: Nullable<GAMESTAGE>, title: Nullable<string> = null, removeMarkers: boolean = true)
    {
        console.log("ResetOverlays", stage, title, removeMarkers)
        if (!State.App.Attempting_CreateInitialTable) RemoveScoreboard(stage, removeMarkers);

        let prev = document.getElementById("embedstatsContainer_" + Constant.PlayerStatsEmbedID);
        if (prev) prev.remove();

        Div.InitializeScoreboardViewport(stage);
        console.log("Reseted overlays")
    }
    /**
    * Clear the scoreboard and display given rows in it with given title for game summaries
    * @param {GAMESTAGE} stage game stage
    * @param {Array.<object>|null} rows Data rows to be displayed
    * @param {string} title Scoreboard title
    * @param {boolean} removeMarkers Wheter to remove markers from the map
    * @param {boolean} enableNav Wheter to add navigation buttons for next round and end game (infinity)
    */
    export async function SetNewScoreboard(mode: GAMEMODE, stage: GAMESTAGE, rows: Array<TableRow>, title: Nullable<string>, removeMarkers: boolean = true, enableNav: boolean = true): Promise<Nullable<DevExpress.ui.dxDataGrid>>
    {
        try
        {
            let m = mode, s = stage;
            if (GeoChatter.Map)
                google.maps.event.addListenerOnce(GeoChatter.Map, "tilesloaded", () => { if (GeoChatter.Main.CurrentGame) GeoChatter.Map?.setMapTypeId(Setting.ScoreboardDisplay[m][s].MinimapLayer) })

            console.log("Setting new scoreboard(initial_attempt=" + State.App.Attempting_CreateInitialTable + ")", stage, title);
            ResetOverlays(stage, title, removeMarkers);

            GeoChatter.Main.SingleOnContentReadyPersistentCallbacks.map(cb => GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(cb));
            while (GeoChatter.Main.SingleOnContentReadyPersistentCallbacks[0]) GeoChatter.Main.SingleOnContentReadyPersistentCallbacks.pop()

            GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(Util.Zoom);
            GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() =>
            {
                if (!GeoChatter.Main.CurrentGame) return
                SetScoreboardMinimized(Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][GeoChatter.Main.CurrentGame.Stage].IsMinimized)
            });
            //onContentReadyCallbackOnce.push(refreshColumnDisplays);
            if (enableNav) GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(ReEnableNavigationButtons);

            console.log("Creating new table", mode, stage)
            GeoChatter.Main.Table = await Control.ScoreboardConstructor(mode, stage);
            console.log("Created new table", GeoChatter.Main.Table)

            if (title != null) $("#scoreboardTitle").html(title);
            $("#scoreboardTitle").attr("data-title-gamestage", stage);

            if (rows != null && rows.length > 0)
            {
                console.log("Scoreboard set: ", rows.length);
                AddRows(rows);
            }
            else
            {
                console.log("Scoreboard set empty");
                SetScoreboardTitleCount(0);
            }

            RefreshTable();
        }
        catch (e)
        {
            console.error(e)
        }
        return GeoChatter.Main.Table;
    }

    export function RefreshTable(changeOnly?: boolean): void
    {
        GeoChatter.Main.Table?.refresh(changeOnly ?? false);
    }

    /**
     * Add row to current table
     * @param {object} row
     */
    export function AddRow(row: TableRow, _idx: number = 0): void
    {
        //playersToFadeIn[row.PlayerName.sort] = idx;

        var dataSource = GeoChatter.Main.Table?.getDataSource();
        dataSource?.store().push([
            { type: "insert", data: row }
        ])
    }

    export function AddRows(rows: Array<TableRow>): void
    {
        if (!rows) return;

        rows.forEach(AddRow);
        SetScoreboardTitleCount();
    }

    /**
     * Change row and given guess
     */
    export function ChangeGuess(row: TableRow, guess: Guess)
    {
        let guessr = guess.PlayerData;
        let rows = GeoChatter.Main.TableRows;
        let len = rows.length;
        for (let i = 0; i < len; i++)
        {
            let r = rows[i];
            if (r && Util.AreTheSamePlayers(r.Player, guessr))
            {
                rows[i] = row;
                break;
            }
        }

        let gs = guess.Round.Guesses;
        for (let i = 0; i < gs.length; i++)
        {
            let g = gs[i]
            if (g && g.BelongsTo(guessr))
            {
                guess.PreviousLink = g;
                g.NextLink = guess;
                gs[i] = guess;
                break;
            }
        }

        RefreshTable(true);
    }
    
    export async function CreateInitialTable(rows: TableRow[])
    {
        if (!GeoChatter.Main.CurrentGame) return;

        console.log("Creating the initial table...", rows);

        await SetNewScoreboard(GeoChatter.Main.CurrentGame.Mode,
            GeoChatter.Main.CurrentGame.Stage,
            rows, null);
    }

    export function AttemptToCreateInitialTable(rows: TableRow[])
    {
        State.App.Attempting_CreateInitialTable = true;
        CreateInitialTable(rows)
            .then((e) => { console.log("Initial table created", e); State.App.Attempting_CreateInitialTable = false; State.App.Attempting_RecreateMissingScoreboard = false; })
            .catch((e) => { console.error("Initial table FAILED", e); State.App.Attempting_CreateInitialTable = false; State.App.Attempting_RecreateMissingScoreboard = false; });
    }

    export function ScoreboardFirstContentReadyCallback(stage: GAMESTAGE): Callback
    {
        let s = stage;
        return async () =>
        {
            console.log("Content ready callback in constructor")
            if (!GeoChatter.Main.CurrentGame) return;

            $(Constant.DataGridRowView).stop();
            State.Scoreboard.IsScrolling = true;

            $(Constant.DataGridRowView).hover(
                Util.Stopscroller,
                () => { State.Scoreboard.IsScrolling = true; window.setTimeout(() => { if (State.Scoreboard.IsScrolling) Util.Scroller(Constant.DataGridRowView); }, 2500) }
            );

            window.setTimeout(() => { if (State.Scoreboard.IsScrolling) Util.Scroller(Constant.DataGridRowView) }, 5000);

            RefreshColumnDisplays(s);

            while (!$('#markerSelect').multiselect)
            {
                await new Promise((res, _rej) => window.setTimeout(res, 100));
            }

            $('#markerSelect').multiselect({
                includeSelectAllOption: true
            });

            $("#dropdownmenu .multiselect-container").css("display", "none");
            $(".multiselect-selected-text").text("All selected (" + GeoChatter.Main.CurrentGame.AllRounds.length + ")")

            $("#dropdownmenu .btn-group").children("[data-toggle='dropdown']").on("click",
                (e) =>
                {
                    e.stopPropagation();
                    let isDisplaying = $("#dropdownmenu .multiselect-container").css("display") != "none";
                    if (isDisplaying)
                    {
                        $("#dropdownmenu .multiselect-container").css("display", "none")
                    }
                    else
                    {
                        $("#dropdownmenu .multiselect-container").css("display", "block")
                    }
                }
            )

            // MakeToolbarElementHideable(".dropdown");

            SetDisplayOptions();
        }
    }

    async function Invoke(f: any)
    {
        try
        {
            if (f && f.constructor)
            {
                console.log("Invoke", f.name);
                switch (f.constructor.name)
                {
                    case "AsyncFunction":
                        {
                            await f();
                            break;
                        }
                    default:
                        {
                            f();
                            break;
                        }
                }
            }
        }
        catch (e)
        {
            console.error("Failed to call the callback function: ", (f ? f.toString() : "<null>"), e);
        }
    }

    /**
    * Get datagrid config
    * @param {GAMEMODE} mode
    * @param {GAMESTAGE} stage
    */
    export function GetDataGridConfig(mode: GAMEMODE, stage: GAMESTAGE): DevExpress.ui.dxDataGrid.Properties
    {
        if (!GeoChatter.Main.CurrentGame) return {};
        if (mode == null) mode = GeoChatter.Main.CurrentGame.Mode;
        if (stage == null) stage = GeoChatter.Main.CurrentGame.Stage;
        let props: DevExpress.ui.dxDataGrid.Properties = {
            dataSource: new DevExpress.data.DataSource({
                paginate: false,
                reshapeOnPush: true,
                store: {
                    type: "array",
                    key: Enum.DataField.PlayerName,
                    data: GeoChatter.Main.TableRows,
                },
            } as unknown as DevExpress.data.utils.Store<string, any>),
            columnWidth: "auto",
            columnAutoWidth: true,
            allowColumnReordering: true,
            allowColumnResizing: true,
            columnResizingMode: Constant.ColumnResizingMode,
            showBorders: true,
            showRowLines: false,
            showColumnLines: true,
            rowAlternationEnabled: true,
            sorting: {
                mode: "multiple"
            },
            columnChooser: {
                enabled: true,
                mode: "select",
                title: "Hide/Show Columns"
            },
            filterRow: {
                visible: true
            },
            export: {
                enabled: stage != Enum.GAMESTAGE.INROUND,
                formats: [Enum.ExportFormat.xlsx, Enum.ExportFormat.csv]
            },
            searchPanel: {
                visible: false
            },
            paging: {
                enabled: false
            },
            toolbar: GetDataGridToolbarConfig(mode, stage),
            onContentReady: OnContentReady,
            onRowUpdated: OnRowUpdated,
            onRowPrepared: OnRowPrepared,
            onRowClick: OnRowClick,
            onCellPrepared: OnCellPrepared,
            onCellHoverChanged: OnCellHoverChanged,
            onOptionChanged: OnOptionChanged,
            columnMinWidth: Constant.MinimumColumnWidth,
            columns: GetColumnConfiguration(stage)
        }
        return props;
    }

    async function OnContentReady(_e: DevExpress.ui.dxDataGrid.ContentReadyEvent)
    {
        console.log("Content ready");
        let cbs = GeoChatter.Main.SingleOnContentReadyWeakCallbacks.map(cb => cb);
        let len = cbs.length;

        while (GeoChatter.Main.SingleOnContentReadyWeakCallbacks[0]) GeoChatter.Main.SingleOnContentReadyWeakCallbacks.pop();

        for (let i = 0; i < len; i++)
            await Invoke(cbs[i])
    }
    
    async function OnCellHoverChanged(e: DevExpress.ui.dxDataGrid.CellHoverChangedEvent)
    {
        if (GeoChatter.Main.CurrentGame
            && GeoChatter.Main.CurrentGame.Stage != Enum.GAMESTAGE.INROUND
            && e.column
            && e.column.dataField == Enum.DataField.PlayerName
            && e.data)
        {
            if (e.eventType == "mouseover")
            {
                let r = GeoChatter.Main.CurrentGame.CurrentRound;
                let pd = (e.data as TableRow).Player;
                let guess = r?.GetGuessOf(pd);
                let order = State.Scoreboard.DisplayingCurrentStandings || GeoChatter.Main.CurrentGame.Stage == Enum.GAMESTAGE.ENDGAME
                    ? e.rowIndex + 1
                    : guess?.ResultFinalOrder;

                order ??= 0;
                let i = 1;
                while (!guess && r)
                {
                    r = r.Game.Rounds[r.ID - (++i)]
                    guess = r?.GetGuessOf(pd);
                }
                Control.EmbedPlayerStats(guess, order);
            }
            else
            {
                let prev = document.getElementById("embedstatsContainer_" + Constant.PlayerStatsEmbedID);
                if (prev) prev.remove();
            }
        }
    }

    function OnCellPrepared(e: DevExpress.ui.dxDataGrid.CellPreparedEvent)
    {
        if (e.rowType == "header")
        {
            e.cellElement.css("text-align", Constant.HeaderAlignment);
        }
        else if (e.rowType == "data")
            e.cellElement.css("text-align", Constant.DataAlignment);
    }

    /** Scoreboard row click event handler */
    function OnRowClick(e: DevExpress.ui.dxDataGrid.RowClickEvent)
    {
        console.log("Row click", e)
        var pd = (e.data as TableRow).Player;

        MapUtil.ShowMarkersOf(pd);
    }

    function OnRowPrepared(e: DevExpress.ui.dxDataGrid.RowPreparedEvent)
    {
        if (e.rowType == "data")
        {
            SetScoreboardTitleCount();
        }
    }

    async function OnOptionChanged(e: DevExpress.ui.dxDataGrid.OptionChangedEvent)
    {
        if (e && e.name == "columns" && GeoChatter.Main.CurrentGame && GeoChatter.Main.Table)
        {
            let start = e.fullName.indexOf("columns[")
            if (start < 0) return;

            let stg = State.Scoreboard.DisplayingCurrentStandings ? Enum.GAMESTAGE.ENDGAME : GeoChatter.Main.CurrentGame.Stage;
            let id = parseInt(e.fullName.slice(start + 8, start + 9));
            let name = (GeoChatter.Main.Table as any)._controllers.columns._columns.find((c: any) => c.index == id).name as DataField;

            if (e.fullName.endsWith(".width"))
            {
                if (e.value == "auto" || e.value < Constant.MinimumColumnWidth) return;

                GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg][name].Width = e.value;
                State.Scoreboard.ShouldUseTableOptionsWidth = true;
                // Not awaited because this is fired too fast
            }
            else if (e.fullName.endsWith(".visible"))
            {
                GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg][name].Visible = e.value;
                await Util.SaveAllScoreboardSettingsForStage(stg);
            }
            else if (e.fullName.endsWith(".visibleIndex"))
            {
                GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg][name].Position = e.value;
                await Util.SaveAllScoreboardSettingsForStage(stg);
            }
            else if (e.fullName.endsWith(".sortOrder")
                && (!State.App.PreferredMultiguess || GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg][name].AllowedWithMultiGuess))
            {
                let cols = (GeoChatter.Main.Table as any)._controllers.columns._columns as Array<DevExpress.ui.dxDataGrid.Column<any, any>>;
                for (let col of cols)
                {
                    GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg][col.name as DataField].SortOrder = col.sortOrder;
                    GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg][col.name as DataField].SortIndex = col.sortIndex ?? -1;
                }
                GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg][name].SortOrder = e.value;
                await Util.SaveAllScoreboardSettingsForStage(stg);
            }
        }
    }

    function OnRowUpdated(e: DevExpress.ui.dxDataGrid.RowUpdatedEvent)
    {
        console.log("Row updated", e);
        $(e.element).fadeIn(AnimationOptions.ChangeGuessFadeIn);
    }

    /**
    * Get datagrid toolbar config
    * @param {GAMEMODE} mode
    * @param {GAMESTAGE} stage
    */
    function GetDataGridToolbarConfig(mode: GAMEMODE, stage: GAMESTAGE): DevExpress.ui.dxDataGrid.Toolbar
    {
        return {
            items: [
                {
                    location: 'before',
                    template()
                    {
                        return $('<div>')
                            .addClass('informerguess')
                            .append(
                                $('<h2>')
                                    .addClass('count')
                                    .attr('id', 'guessCountHeader')
                                    .attr('title', 'Amount of unique players')
                                    .text(0),
                                $('<span>')
                                    .addClass('name')
                                    .text(stage == Enum.GAMESTAGE.INROUND ? 'Guesses' : 'Results'),
                                $('<span>')
                                    .attr('id', 'collapseState')
                                    .attr('data-tooltip', 'Use "Actions" menu to show/hide rows')
                                    .text(Setting.ScoreboardDisplay[mode][stage].IsMinimized ? 'HIDDEN' : ''),
                            );
                    },
                },
                {
                    location: 'center',
                    template()
                    {
                        let r = GeoChatter.Main.CurrentGame?.CurrentRound;
                        return $('<div>')
                            .append(
                                $('<h3>')
                                    .addClass(`scoreboard-title`)
                                    .css("display", "flex")
                                    .css("align-items", "center")
                                    .css("flex-direction", stage == Enum.GAMESTAGE.ENDGAME ? 'column' : 'row')
                                    .attr('id', 'scoreboardTitle')
                                    .attr('title', 'Scoreboard title')
                                    .html(stage == Enum.GAMESTAGE.INROUND
                                        ? `<span id='roundTitle'>${(State.App.PreferredMultiguess ? "MULTIGUESS " : "")}ROUND</span> <span id='roundNumberSpan'>${(r ? r.Game.TotalRoundCount : "...")}</span>`
                                        : ``),
                                $('<span>')
                                    .addClass('scoreboard-subtitle')
                                    .attr('id', 'scoreboardSubTitle')
                                    .css('cursor', 'pointer')
                                    .css('display', stage == Enum.GAMESTAGE.INROUND || (stage == Enum.GAMESTAGE.ENDGAME && GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.ENDGAME)
                                        ? 'none'
                                        : 'unset')
                                    .on('click', ScoreboardSubTitleClick)
                                    .text('(Current Standings)'),
                            );
                    },
                },
                {
                    location: 'after',
                    template()
                    {
                        let st = State.Scoreboard.IsScrollingEnabled ? 'off' : 'on';
                        return $('<div>')
                            .css('display', "flex")
                            .append(
                                // Actions
                                $('<div>')
                                    .addClass('dropdown dropdowntemp')
                                    .css("display", "inline-flex")
                                    .css("align-items", "center")
                                    .attr('title', 'Other action buttons')
                                    .append(
                                        $('<button>')
                                            .addClass('dropbtn dropdowntemp')
                                            .text('Actions')
                                            .attr('title', 'Other action buttons')
                                            .on("click", DropdownClick),
                                        $('<div>')
                                            .attr('id', "dropdownmenu")
                                            .addClass('dropdown-content dropdowntemp')
                                            .append(
                                                $('<button>')
                                                    .attr('id', "columnChooserBtn")
                                                    .attr('title', 'Change column visibilities')
                                                    .addClass('dropdowntemp')
                                                    .html(`Show/Hide columns`)
                                                    .on("click", ColumnChooserBtnClick),

                                                $('<button>')
                                                    .attr('id', "indexHideShowBtn")
                                                    .attr('title', `${(Setting.ScoreboardDisplay[mode][stage].ShowRowNumbers ? "Hide" : "Show")}`)
                                                    .addClass('dropdowntemp')
                                                    .html(`<span id='indexHideShowSpan' class='username-dark toggle-state-${(Setting.ScoreboardDisplay[mode][stage].ShowRowNumbers ? 'off' : 'on')}'>${(Setting.ScoreboardDisplay[mode][stage].ShowRowNumbers ? "Hide" : "Show")}</span> row numbers (#1, #2, ...)`)
                                                    .on("click", IndexHideShowBtnClick),

                                                $('<button>')
                                                    .attr('id', "showAllGuessesBtn")
                                                    .attr('title', 'Click here to show guess markers from rounds selected below')
                                                    .prop('disabled', stage != Enum.GAMESTAGE.ENDGAME)
                                                    .addClass('dropdowntemp')
                                                    .html(`Click to show guesses from: ${GetMultipleRoundSelectBox()}`)
                                                    .on("click", MapUtil.ToggleMarkers),

                                                $('<button>')
                                                    .attr('id', "toggleScroll")
                                                    .attr('title', 'Enable/Disable auto-scrolling when table has hidden rows')
                                                    .addClass('dropdowntemp')
                                                    .html(`Turn <span id='toggleState' class='username-dark toggle-state-${st}'>${st}</span> auto-scrolling`)
                                                    .on("click", AutoScrollButtonClick),
                                                $('<button>')
                                                    .addClass('dropdowntemp')
                                                    .attr('title', 'Reset sorting back to default')
                                                    .text("Reset scoreboard sorting")
                                                    .on("click", ResetSorting),

                                                $('<button>')
                                                    .attr('id', "exportBtnxlsx")
                                                    .attr('title', 'Export current scoreboard to /exports as .xlsx')
                                                    .addClass('dropdowntemp')
                                                    .html(`Export as .xlsx`)
                                                    .on("click", () => { Util.ExportCurrentScoreboardAs(Enum.ExportFormat.xlsx) }),
                                                $('<button>')
                                                    .attr('id', "exportBtncsv")
                                                    .attr('title', 'Export current scoreboard to /exports as .csv')
                                                    .addClass('dropdowntemp')
                                                    .html(`Export as .csv`)
                                                    .on("click", () => { Util.ExportCurrentScoreboardAs(Enum.ExportFormat.csv) }),

                                                $('<button>')
                                                    .addClass('dropdowntemp')
                                                    .attr('title', 'Refresh scoreboard to fix possible visual glitches')
                                                    .text("Refresh scoreboard")
                                                    .on("click", () => RefreshTable(false)),

                                                $('<button>')
                                                    .addClass('dropdowntemp')
                                                    .attr('title', "Hide/Show rows below the header of the scoreboard")
                                                    .html(`<span id='minimalizedState' class='username-dark toggle-state-${(!Setting.ScoreboardDisplay[mode][stage].IsMinimized ? 'off' : 'on')}'>${(!Setting.ScoreboardDisplay[mode][stage].IsMinimized ? 'Hide' : 'Show')}</span> the guess rows`)
                                                    .on("click", ToggleMinimized)
                                            )
                                ),
                                // Guess toggle
                                (stage == Enum.GAMESTAGE.INROUND
                                    ? $('<label>')
                                        .addClass('switch')
                                        .css("display", "inline-flex")
                                        .css("align-items", "center")
                                        .attr('id', "toggleSliderLabel")
                                        .attr('title', 'Open/Close guesses')
                                        .append(
                                            $('<input>')
                                                .attr('id', Constant._id_guess_toggle_slider)
                                                .attr('type', 'checkbox')
                                                .prop('checked', true),
                                            $('<span>')
                                                .addClass('slider round')
                                        )
                                        .change(Util.ToggleGuesses)
                                    : "")
                            );
                    },
                }
            ]
        };
    }

    /**
    * Get datagrid column config
    * @param {GAMESTAGE} stage
    */
    function GetColumnConfiguration(stage: GAMESTAGE): Array<DevExpress.ui.dxDataGrid.Column<any, any>>
    {
        if (!GeoChatter.Main.CurrentGame) return [];
        var columns: Array<DevExpress.ui.dxDataGrid.Column<any, string>> = [];
        for (const [_colname, col] of Object.entries(GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stage]))
        {
            let vis = col.Visible;
            let available = !State.App.PreferredMultiguess || col.AllowedWithMultiGuess;
            if (!available) vis = false;

            let funcs = GetCalculateFunctionList(col.DataField)
            let colcfg: DevExpress.ui.dxDataGrid.Column<any, any> = {
                allowSearch: false,
                dataField: col.DataField,
                caption: col.Name,
                visible: vis,
                visibleIndex: col.Position,
                showInColumnChooser: available,
                //width: col.Width,
                allowSorting: col.Sortable,
                sortIndex: available && col.SortIndex >= 0 ? col.SortIndex : -1,
                headerCellTemplate: headerCellTemplate,
                cellTemplate: CellTemplate,
                calculateSortValue: funcs.calculateSortValue,
                calculateCellValue: funcs.calculateCellValue,
                calculateDisplayValue: funcs.calculateDisplayValue,
            }

            if (available && col.SortOrder) colcfg.sortOrder = col.SortOrder;

            columns.push(colcfg);
        }

        return columns
    }

    function GetMultipleRoundSelectBox()
    {
        let markerSelectOptions = "<div class='dropdowntemp' id='markerSelectWrapper'><select class='dropdowntemp' id='markerSelect' multiple='multiple'>"
        let i = 1;
        if (GeoChatter.Main.CurrentGame)
        {
            for (let round of GeoChatter.Main.CurrentGame.AllRounds)
            {
                let j = i++;
                markerSelectOptions += `<option class='dropdowntemp' val='${j}'>${round.GetFlagHTML()} ${(round.MultiGuessEnabled ? "Multiguess-" : "")}Round ${j}</option >`;
            }
        }
        return markerSelectOptions + "</select></div>";
    }

    function GetCalculateFunctionList(property: DataField)
    {
        return {
            calculateSortValue(rowData: TableRow)
            {
                return rowData[property]?.sort;
            },
            calculateCellValue(rowData: TableRow)
            {
                return rowData[property]?.sort;
            },
            calculateDisplayValue(rowData: TableRow)
            {
                return rowData[property]?.display;
            }
        }
    }

    function ColumnChooserBtnClick()
    {
        if ($(".dx-popup-draggable").data("displaying") == "1")
        {
            $(".dx-popup-draggable").data("displaying", "0");
            GeoChatter.Main.Table?.hideColumnChooser();
        }
        else
        {
            $(".dx-popup-draggable").data("displaying", "1");
            GeoChatter.Main.Table?.showColumnChooser();
        }
    }

    function IndexHideShowBtnClick()
    {
        if (!GeoChatter.Main.CurrentGame) return;

        if (Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][GeoChatter.Main.CurrentGame.Stage].ShowRowNumbers)
        {
            Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][GeoChatter.Main.CurrentGame.Stage].ShowRowNumbers = false;
            $(".row-index").hide();
            $("#indexHideShowSpan").text("Show").addClass("toggle-state-on").removeClass("toggle-state-off")
        }
        else
        {
            Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][GeoChatter.Main.CurrentGame.Stage].ShowRowNumbers = true;
            $(".row-index").show();
            $("#indexHideShowSpan").text("Hide").removeClass("toggle-state-on").addClass("toggle-state-off")
        }
    }

    async function ScoreboardSubTitleClick()
    {
        try
        {
            if (!State.Scoreboard.SubtitleClickReady
                || !$
                || !GeoChatter.Main.CurrentGame
            ) return

            let r = GeoChatter.Main.CurrentGame.CurrentRound;
            if (!r) return;

            State.Scoreboard.SubtitleClickReady = false;

            console.log("Subtitle click")
            let oldtitle = $("#scoreboardTitle").html();
            let oldsubtitle = $("#scoreboardSubTitle").html();
            if (!State.Scoreboard.DisplayingCurrentStandings) // Currently on round results
            {
                let len = r.Standings.length;
                await SetNewScoreboard(GeoChatter.Main.CurrentGame.Mode,
                    Enum.GAMESTAGE.ENDGAME,
                    r.Standings,
                    null,
                    false,
                    false);
                GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() =>
                {
                    $("#scoreboardTitle").text('Current Standings');
                    $("#scoreboardSubTitle").html(oldtitle);
                    State.Scoreboard.DisplayingCurrentStandings = true;
                    State.Scoreboard.SubtitleClickReady = true;
                    SetScoreboardTitleCount(len);
                });
            }
            else
            {
                let len = r.ResultTableRows.length;
                await SetNewScoreboard(GeoChatter.Main.CurrentGame.Mode,
                    Enum.GAMESTAGE.ENDROUND,
                    r.ResultTableRows,
                    null,
                    false,
                    false);
                GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() =>
                {
                    $("#scoreboardTitle").html(oldsubtitle);
                    $("#scoreboardSubTitle").text('(Current Standings)');
                    State.Scoreboard.DisplayingCurrentStandings = false;
                    State.Scoreboard.SubtitleClickReady = true;
                    SetScoreboardTitleCount(len);
                });
            }

        }
        catch (e)
        {
            console.error(e);
            State.Scoreboard.SubtitleClickReady = true;
        }
    }

    function DropdownClick()
    {
        document.getElementById("dropdownmenu")?.classList.toggle("show");
    }

    export function DropDownMenuFocusOut(_el: Element, event: PointerEvent): void
    {
        let el = event.target as Element;
        if (el && !el.matches('.dropdowntemp'))
        {
            var dropdowns = document.getElementsByClassName("dropdown-content");
            var i;
            for (i = 0; i < dropdowns.length; i++)
            {
                var openDropdown = dropdowns[i];
                if (openDropdown && openDropdown.classList.contains('show'))
                {
                    openDropdown.classList.remove('show');
                }
            }
        }
    }
    function AutoScrollButtonClick()
    {
        if (State.Scoreboard.IsScrollingEnabled)
        {
            $("#toggleState").text("on").removeClass("toggle-state-off").addClass("toggle-state-on")
            Util.Stopscroller()
            State.Scoreboard.IsScrollingEnabled = false;
        }
        else
        {
            $("#toggleState").text("off").removeClass("toggle-state-on").addClass("toggle-state-off")
            State.Scoreboard.IsScrollingEnabled = true;
            Util.Scroller()
        }
    }

    export function ResetSorting()
    {
        if (!GeoChatter.Main.CurrentGame || !GeoChatter.Main.Table) return;

        let stg = State.Scoreboard.DisplayingCurrentStandings ? Enum.GAMESTAGE.ENDGAME : GeoChatter.Main.CurrentGame.Stage;
        for (const [colname, col] of Object.entries(GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg]).sort((curr, prev) => curr[1].DefaultSortIndex < prev[1].DefaultSortIndex ? -1 : (curr[1].DefaultSortIndex > prev[1].DefaultSortIndex ? 1 : 0)))
        {
            GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg][colname as DataField].SortIndex = col.DefaultSortIndex;
            GeoChatter.Main.GameTableOptions[GeoChatter.Main.CurrentGame.Mode][stg][colname as DataField].SortOrder = col.DefaultSortOrder;
            GeoChatter.Main.Table.columnOption(col.DataField, "sortIndex", col.DefaultSortIndex)
            GeoChatter.Main.Table.columnOption(col.DataField, "sortOrder", col.DefaultSortOrder)
        }
        RefreshTable();
    }

    function headerCellTemplate(header: DevExpress.core.DxElement, info: DevExpress.ui.dxDataGrid.ColumnHeaderCellTemplateData)
    {
        $('<span>')
            .html(info.column.caption ?? "Unknown")
            .css('font-size', '1.3em')
            .css('padding', '2px')
            .appendTo(header);
    }

    function CellTemplate(element: DevExpress.core.DxElement, info: DevExpress.ui.dxDataGrid.ColumnCellTemplateData)
    {
        if (info.rowType == "data")
        {
            switch (info.column.dataField)
            {
                case Enum.DataField.PlayerName:
                    {
                        element.append(`<div data-colfield='${info.column.dataField}'><span class='row-index' ${(GeoChatter.Main.CurrentGame && Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][GeoChatter.Main.CurrentGame.Stage].ShowRowNumbers ? "" : "style='display:none'")}>${(info.rowIndex + 1)}</span>${info.text}</div>`)
                        break;
                    }
                case Enum.DataField.GuessPoint:
                case Enum.DataField.Guesses:
                    {
                        element.append(`<div data-colfield='${info.column.dataField}' class='flag-cell-wrap'>${info.text}</div>`)
                        break;
                    }
                default:
                    {
                        element.append(`<div data-colfield='${info.column.dataField}'>${info.text}</div>`)
                        break;
                    }
            }
        }
        else element.append(`<div data-colfield='${info.column.dataField}'>${info.text}</div>`)
    }

    export function CheckMissingScoreboard()
    {
        if (State.App.LoadCompleted
            && !GeoChatter.Main.Table
            && !State.App.Attempting_CreateInitialTable
            && !State.App.Attempting_RecreateMissingScoreboard
            && !GeoChatter.Main.CurrentGame
            && Util.IsCurrentlyInGame())
        {
            State.App.Attempting_RecreateMissingScoreboard = true;
            console.log("Executing checkMissingScoreboard");
            (async () =>
            {
                await CefSharp.BindObjectAsync('jsHelper');
                jsHelper.reTriggerStartGameEvent()
                    .then((r: boolean) => { if (r) console.log("Re-triggered game start event"); else State.App.Attempting_RecreateMissingScoreboard = false; })
                    .catch(() => { console.error("FAILED to re-trigger game start event"); });
            })();
        }
    }
    export function SetRoundSpecificStatusBar()
    {
        if (!GeoChatter.Main.CurrentGame) return
        let r = GeoChatter.Main.CurrentGame.CurrentRound;
        $('[data-qa="round-number"]')
            .css("display", "block");

        $('[data-qa="score"]')
            .css("display", "block");

        if (GeoChatter.Main.CurrentGame.Settings.IsInfinite && r)
        {
            $('[data-qa="round-number"]')
                .empty()
                .append(...Control.StatusLabel("Round", `${GeoChatter.Main.CurrentGame.TotalRoundCount}/${GeoChatter.Main.CurrentGame.MaxRoundCountString}`));

            $('[data-qa="score"]')
                .empty()
                .append(...Control.StatusLabel("Score", GeoChatter.Main.CurrentGame.CurrentScoreOfStreamer.toString()));
        }
    }

    
}

window.GC.Visual = Visual;