import "./core";
import { Control } from "./controls";
import { Div } from "./divs";
import { GeoChatter } from "./geochatter";
import { MapUtil } from "./maps";
import { GameTableColumn, Guess, Game, Round } from "./models";
import { Util } from "./utils";
import { Visual } from "./visuals";
import { Enum } from "./enums"
import { Constant } from "./constants";
import { State, Setting } from "./settings";

export namespace EventHandler
{
    //////////////////////
    /// Exports
    //////////////////////
    export async function ApplySettings(_el?: Element): Promise<void>
    {
        await CefSharp.BindObjectAsync('jsHelper');

        await jsHelper.getTableOptions()
            .then(s =>
            {
                let o: IGameTableOptions = JSON.parse(s);

                for (let [_, modeObj] of Object.entries(o.Options))
                {
                    let tobj = {} as GameModeOptions;
                    for (let [_, stageObj] of Object.entries(modeObj.Stages))
                    {
                        let tsobj = {} as GameStageOptions;

                        for (let [_, col] of Object.entries(stageObj.Columns))
                        {
                            let wdth = col.Width;
                            if (State.Scoreboard.ShouldUseTableOptionsWidth)
                            {
                                wdth = GeoChatter.Main.GameTableOptions[modeObj.Mode][stageObj.Stage][col.DataField].Width;
                            }
                            tsobj[col.DataField] = new GameTableColumn(col.Position, col.DataField, col.Name, wdth, col.SortOrder, col.SortIndex, col.DefaultSortOrder, col.DefaultSortIndex, col.Visible, col.Sortable, col.AllowedWithMultiGuess);
                        }

                        tobj[stageObj.Stage] = tsobj;

                        if (!State.Scoreboard.ShouldUseLastScoreBoardSettings)
                        {
                            Setting.ScoreboardDisplay[modeObj.Mode][stageObj.Stage] = {
                                Top: stageObj.Top,
                                Left: stageObj.Left,
                                Width: stageObj.Width,
                                Height: stageObj.Height,
                                IsMinimized: stageObj.IsMinimized,
                                ShowRowNumbers: stageObj.ShowRowNumbers,
                                MinimapLayer: stageObj.MinimapLayer
                            }
                        }
                    }
                    GeoChatter.Main.GameTableOptions[modeObj.Mode] = tobj;
                }

                State.Scoreboard.ShouldUseTableOptionsWidth = false;
                State.Scoreboard.ShouldUseLastScoreBoardSettings = false;

                console.log("Set TableOptions and LastScoreBoardSettings");
                Visual.RefreshColumnDisplays();
            })
            .catch(console.error);

        await jsHelper.getOverlaySettings()
            .then(s =>
            {
                let o: OverlaySettings = JSON.parse(s);
                for (let [setting, value] of Object.entries(o))
                {
                    Object.assign(Setting.Overlay, { [setting]: value });
                }

                Visual.SetDisplayOptions();

                console.log("Set Setting.Overlay");

                Visual.RefreshColumnDisplays();
            })
            .catch(console.error);

        await Util.Zoom();
    }

    export function OnKeyDownCTRL(_el: Element, e: KeyboardEvent): void
    {
        if (e.key == "Control")
        {
            State.App.IsCTRLDown = true;
        }
    }

    export function OnKeyUpCTRL(_el: Element, e: KeyboardEvent): void
    {
        if (e.key == "Control")
        {
            State.App.IsCTRLDown = false;
        }
    }

    export function OnWheelScrollZoom(_el: Element, e: WheelEvent): void
    {
        if (State.App.IsCTRLDown)
        {
            let d = e.deltaY;
            Util.Zoom(d < 0 ? Constant.ZoomSensitivity : -Constant.ZoomSensitivity);
        }
    }

    export function OnDrawMapLinkButton(_el: Element, _e: Event): void
    {
        Control.AddCopyMapLinkButton();
        Control.AddCopyResultLinkButton();
    }

    export function OnExitGameEvent(_el: Element, e: WheelEvent): void
    {
        if (!GeoChatter.Main.CurrentGame) return;

        console.log("Firing OnExitGameEvent", e);

        let stage = GeoChatter.Main.CurrentGame.Stage;
        Util.SaveAllScoreboardSettingsForStage(stage).then(() =>
        {
            if (!GeoChatter.Main.CurrentGame) return;

            GeoChatter.Main.CurrentGame.Stage = Enum.GAMESTAGE.EXITED;

            Visual.RemoveScoreboard(stage, true);

            ResetCheckStates();

            State.App.StartedCurrentGame = false;
        });
        Control.AddCopyResultLinkButton();
        Control.AddCopyMapLinkButton();
    }

    export function OnRefreshGameEvent(_el: Element, e: WheelEvent): void
    {
        console.log("Firing OnRefreshGameEvent", e);

        window.setInterval(() =>
        {
            let e = $("#scoreboardContainer .dx-viewport")[0];
            if (e) e.classList.remove("dx-theme-generic-typography")
        }, 100);
        Control.AddCopyResultLinkButton();
        Control.AddCopyMapLinkButton();
    }

    export function PostGuessButtonClickHandlers(_el: Element): void
    {
        Util.RepeatCallbackAttempt(CheckSelector, 5, 400, true);
        Util.RepeatCallbackAttempt(TryAddCCheck, 7, 300);
    }

    export async function SignOutFromBrowser(_el: Element, e: EventArgs)
    {
        let red = e.detail as unknown as string;
        await fetch("https://www.geoguessr.com/api/v3/accounts/signout", { method: "post", headers: { cookie: document.cookie } })
        Util.GoToPage(red)
    }

    export function LoadingScreenRequest(_el: Element, e: EventArgs)
    {
        let details = e.detail as unknown as Nullable<string>
        
        if (!details)
        {
            Visual.RemoveLoadingScreen()
        }
        else
        {
            Visual.AddLoadingScreen(details);
        }
    }

    export async function OnAddressMainPlayScreen(_el: Element)
    {
        console.log("AddressMainPlayScreen");
        if (!$("#startInfinityButton")[0])
        {
            let btn = $("[data-qa='start-game-button']")[0] || $("[data-qa='start-streak-game-button']")[0]
            if (btn)
            {
                $(btn)
                    .parent()
                    .append(Control.InfiniteGameButton().css("display", $(btn).data("qa") == "start-streak-game-button" ? "none" : "inline-block"))
                    .parent()
                    .append(await Control.RoundSettingsContainer(true));
            }
            else
            {
                window.setTimeout(OnAddressMainPlayScreen, 250);
            }
        }
        Control.AddCopyResultLinkButton();
        Control.AddCopyMapLinkButton();
    }

    export async function OnNewGuessEvent(_el: Element, e: EventArgs)
    {
        if (!GeoChatter.Main.CurrentGame) return;

        let round = GeoChatter.Main.CurrentGame.CurrentRound;

        if (!round) return;

        console.log("Firing OnNewGuessEvent");
        State.App.ProcessingGuess = true;

        let details = e.detail as GuessSummary

        var guess = new Guess(round, details);

        var row = guess.AsTableRow();
        console.log("Created row instance for guess", row);

        if (guess.IsFirstGuess)
        {
            round.Guesses.push(guess);
            Visual.AddRow(row)
        }
        else
        {
            Visual.ChangeGuess(row, guess);
        }

        console.log("Added row to scoreboard");

        State.App.ProcessingGuess = false;

        (async () =>
        {
            try
            {
                await MapUtil.GetStreetviewPanorama(
                    {
                        lat: guess.Location.Latitude,
                        lng: guess.Location.Longitude
                    },
                    Setting.Overlay.StreetViewMaxRadius)
                    .then(async (loc) =>
                    {
                        await CefSharp.BindObjectAsync('jsHelper');
                        guess.Location.Pano = loc.pano;
                        guess.Location.PanoOverwritten = true;

                        await jsHelper.setGuessPanoId(guess.Round.ID, guess.PlayerData.Name, loc.pano);
                    });
            }
            catch (e)
            {
                console.warn("Couldn't find streetview for guess at " + guess.Location.Latitude + ", " + guess.Location.Longitude, e)
            }
        })();
    }

    export async function Handle_GuessButton_Clicked()
    {
        State.Event.AddedClick = false;
        console.log("Clicked guess button");
        await Util.SaveAllScoreboardSettingsForStage(Enum.GAMESTAGE.INROUND);
    }

    export async function OnGuessButtonClicked_Listener(_el: Element, _e: EventArgs)
    {
        var button = Div.Get_GuessButton();

        if (button && !State.Event.AddedClick)
        {
            State.Event.AddedClick = true;
            console.log("Listening guess button");

            button.addEventListener("click", Handle_GuessButton_Clicked, false);

            while (!GeoChatter.Main.CurrentGame?.CurrentRound
                || GeoChatter.Main.CurrentGame.CurrentRound.Location.PanoOverwritten)
            {
                await new Promise((r) => setTimeout(r, 100));
            }

            await HandlePostRoundStartData(GeoChatter.Main.CurrentGame.CurrentRound);
        }
    }

    /**
    * Fire ending game (viewing summary) event in C#
    * @param {object} e extra information gotten from last round ending event fired from C#
    */
    export async function EndGame(e: GameSummary): Promise<void>
    {
        if (!GeoChatter.Main.CurrentGame) return;

        await CefSharp.BindObjectAsync('jsHelper');

        jsHelper.endGame(GeoChatter.Main.CurrentGame.ID, GeoChatter.Main.CurrentGame.Settings.IsChallenge).then(async function()
        {
            if (!GeoChatter.Main.CurrentGame) return;

            console.log("Ending the game");

            GeoChatter.Main.CurrentGame.Stage = Enum.GAMESTAGE.ENDGAME;

            let flags =
                Setting.Overlay.DisplayCorrectLocations ?
                    `<div id="flagstitle">${GeoChatter.Main.CurrentGame.Rounds.map(round => round.GetFlagHTML()).join(" ")}</div>` :
                    "";

            let { streamer, score } = GetStreamerAndScoreFromGameResult(e.GameResults)

            let rows = GeoChatter.Main.CurrentGame.AsTableRows(e);
            await Visual.SetNewScoreboard(GeoChatter.Main.CurrentGame.Mode,
                Enum.GAMESTAGE.ENDGAME,
                rows,
                `GAME SUMMARY</br>${flags}`);


            GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() =>
            {
                $$("[data-qa=guess-marker]").forEach((e) => $(e).css("display", "none"))
                $$("[data-qa=correct-location-marker]").forEach((e) => $(e).css("display", "none"))

                MapUtil.AddCorrectLocationMarkers();

                //MakeCluster(window, "correctLocationMarkers");

                if (streamer) MapUtil.ShowMarkersOf(streamer.PlayerData);

                window.setTimeout(() =>
                {
                    if (!GeoChatter.Main.CurrentGame) return;

                    let sc = document.querySelector("." + Constant._class_gameResult_StreamerPoints);
                    if (sc && score != -1)
                    {
                        let percentage = Math.min(score / (5000 * (GeoChatter.Main.CurrentGame.PlaceInChain() - 1) + 5000 * GeoChatter.Main.CurrentGame.Rounds.length), 1);
                        $(".progress-bar_progress__Fmo83")
                            .css("width", `${(percentage * 100).toFixed(2)}%`)

                        let p = sc.textContent?.split(" ")

                        if (p && !Util.IsSameScore(p[0], score.toString())) sc.textContent = score + " (GeoGuessr: " + p[0] + ") points";
                    }
                }, 1000)

                Visual.SetScoreboardTitleCount(rows.length);
            });

            GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() => window.setTimeout(Util.AutoexportFuncFor(Enum.GAMESTAGE.ENDGAME), 250))
        });
        Control.AddCopyResultLinkButton();
        Control.AddCopyMapLinkButton();
    }

    export async function Handle_ViewSummaryButton_Clicked(e: GameSummary)
    {
        State.Event.AddedClickViewSummary = false;
        console.log("Clicked view summary button");
        EndGame(e);
        await Util.SaveGame();
    }

    export function OnViewSummaryClicked_Listener(_el: Element, e: EventArgs)
    {
        var details = e.detail as GameSummary;
        let button = Div.Get_ViewSummaryButton();

        if (button)
        {
            State.Event.AddedClickViewSummary = true;
            console.log("Listening view summary button");
            button.addEventListener("click", () => Handle_ViewSummaryButton_Clicked(details), false);
        }
    }

    export async function CloseRound()
    {
        if (!GeoChatter.Main.CurrentGame) return

        State.App.ClosingTheRound = true;
        await CefSharp.BindObjectAsync('jsHelper');

        await jsHelper.closeRound();

        console.log("Closing the round")

        GeoChatter.Main.CurrentGame.Stage = Enum.GAMESTAGE.INROUND;
        Visual.ResetOverlays(null);

        if (!State.App.Attempting_CreateInitialTable && !GeoChatter.Main.Table)
        {
            console.log("Attempt from closeRound")
            Visual.AttemptToCreateInitialTable([]);
        }
        State.App.ClosingTheRound = false;
        
        Control.AddCopyMapLinkButton();
    }

    export async function Handle_EndRoundButton_Clicked()
    {
        State.Event.AddedClickEnd = false;
        console.log("Clicked next round button");
        await Util.SaveAllScoreboardSettingsForStage(Enum.GAMESTAGE.ENDROUND)
        await CloseRound();
    }

    export function OnNextRoundClicked_Listener(_el: Element, e: EventArgs)
    {
        console.log("OnNextRoundClicked_Listener", e)

        var button = Div.Get_EndRoundButton();
        if (GeoChatter.Main.CurrentGame && button)
        {
            State.Event.AddedClickEnd = true;

            MapUtil.StreamerStreaksMarker?.setMap(null);

            while (MapUtil.Markers.correctLocations && MapUtil.Markers.correctLocations[0])
            {
                MapUtil.Markers.correctLocations.pop()?.setMap(null);
            }

            console.log("Listening next round button");
            button.addEventListener("click", Handle_EndRoundButton_Clicked, false);

            GeoChatter.Main.CurrentGame.Stage = Enum.GAMESTAGE.ENDROUND;
        }
        Control.AddCopyMapLinkButton();
    }

    export function streaksMarkerHandler(e: google.maps.MapMouseEvent)
    {
        if (!GeoChatter.Main.CurrentGame
            || (GeoChatter.Main.CurrentGame.Mode == Enum.GAMEMODE.DEFAULT && State.Event.AddedStreakMarkerListener))
        {
            State.Event.AddedStreakMarkerListener = false;
            return
        }

        if (GeoChatter.Main.CurrentGame.Mode == Enum.GAMEMODE.STREAK &&
            GeoChatter.Main.CurrentGame.Stage == Enum.GAMESTAGE.INROUND
            && e.latLng)
        {
            if (MapUtil.StreamerStreaksMarker)
                MapUtil.StreamerStreaksMarker.setPosition(e.latLng);
            else
                MapUtil.StreamerStreaksMarker = MapUtil.CreateMarker(e.latLng, null, "")
        }
    }

    export function OnStartGameEvent(_el: Element, e: EventArgs)
    {
        let details = e.detail as GameJson;
        State.App.PreferredMultiguess = details.IsFirstRoundMultiGuess;

        if (GeoChatter.Map && !State.Event.AddedStreakMarkerListener)
        {
            State.Event.AddedStreakMarkerListener = true;
            GeoChatter.Map.addListener("click", streaksMarkerHandler);
        }

        console.log("Firing OnStartGameEvent", e);
        GeoChatter.Main.CurrentGame = Game.CreateGameFromJson(GeoChatter.Main, details);

        Div.InitializeScoreboardViewport();

        State.App.StartedCurrentGame = true;
        Control.AddCopyMapLinkButton();
    }

    export function OnStartRoundEvent(_el: Element, e: EventArgs)
    {
        if (!GeoChatter.Main.CurrentGame) return

        console.log("Firing OnStartRoundEvent", e);
        let detail = e.detail as RoundJson;
        let round = Round.CreateRoundFromJson(GeoChatter.Main.CurrentGame, detail, null)

        console.log(round);

        GeoChatter.Main.CurrentGame.Rounds.push(round);
        GeoChatter.Main.CurrentGame.Stage = Enum.GAMESTAGE.INROUND;

        State.App.PreferredMultiguess = round.MultiGuessEnabled;

        Setting.App.PreferredExportFormat = detail.PreferredExportFormat;
        State.App.AutoExportGameResults = detail.AutoExportGameResults;
        State.App.AutoExportRoundResults = detail.AutoExportRoundResults;
        State.App.AutoExportRoundStandings = detail.AutoExportRoundStandings;
        State.App.AlertExportSuccess = detail.AlertOnExportSuccess;

        ResetCheckStates();

        let cb = async () =>
        {
            console.log(`Setting rows(${round.Guesses.length}) for round ${round.ID}`);
            Visual.AddRows(round.Guesses.map(g => g.AsTableRow()))
            Visual.SetRoundSpecificStatusBar();

            await CefSharp.BindObjectAsync('jsHelper');
            await jsHelper.markNextRoundAsMultiGuess(State.App.PreferredMultiguess);
            await jsHelper.verifyGameStarted(round.Game.ID);
        }

        if (GeoChatter.Main.Table) Visual.RefreshTable();

        if (!State.App.Attempting_CreateInitialTable && GeoChatter.Main.Table == null)
        {
            console.log("Attempt from OnStartRoundEvent")
            GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(cb);
            Visual.AttemptToCreateInitialTable([]);
        }
        else if (State.App.Attempting_CreateInitialTable)
        {
            console.log("Adding round cb to oncontentready")
            GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(cb);
        }
        else
        {
            console.log("Invoking round cb directly")
            cb();
        } 
        Control.AddCopyMapLinkButton();
    }

    function GetStreamerAndScoreFromGameResult(res: Array<GameResult>): { streamer: Nullable<GameResult>, score: number }
    {
        let streamer: Nullable<GameResult> = null;
        for (let s of res)
        {
            if (s.PlayerData.IsStreamer)
            {
                streamer = s;
                break;
            }
        }

        if (!streamer) console.error("Failed to find streamer in game results");

        let score = streamer ? streamer.Score : 0;
        return { streamer, score };
    }

    export async function OnEndInfinityGame(_el: Element, e: EventArgs)
    {
        if (!GeoChatter.Main.CurrentGame) return;

        console.log("Ending the infinity game", e);

        let details = e.detail as GameSummary;

        GeoChatter.Main.CurrentGame.Stage = Enum.GAMESTAGE.ENDGAME;

        let flags =
            Setting.Overlay.DisplayCorrectLocations ?
                `<div id="flagstitle">${GeoChatter.Main.CurrentGame.AllRounds.map(round => round.GetFlagHTML()).join(" ")}</div>` :
                "";

        let { streamer, score } = GetStreamerAndScoreFromGameResult(details.GameResults)

        let rows = GeoChatter.Main.CurrentGame.AsTableRows(details);
        await Visual.SetNewScoreboard(GeoChatter.Main.CurrentGame.Mode,
            Enum.GAMESTAGE.ENDGAME,
            rows,
            `GAME SUMMARY</br>${flags}`,
            true,
            false
        );

        $('#nextRoundInfinityButton').hide();
        $('#endGameInfinityButton').hide();
        $('#roundSettingsContainer').remove();

        GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() =>
        {
            $$("[data-qa=guess-marker]").forEach((el: JQuery<HTMLElement>) => $(el).css("display", "none"))
            $$("[data-qa=correct-location-marker]").forEach((el: JQuery<HTMLElement>) => $(el).css("display", "none"))

            MapUtil.AddCorrectLocationMarkers();

            //MakeCluster(window, "correctLocationMarkers");

            if (streamer) MapUtil.ShowMarkersOf(streamer.PlayerData);

            window.setTimeout(() =>
            {
                if (!GeoChatter.Main.CurrentGame) return;

                let percentage = Math.min(score / (25000 * (GeoChatter.Main.CurrentGame.PlaceInChain() - 1) + 5000 * GeoChatter.Main.CurrentGame.Rounds.length), 1);
                $(".progress-bar_progress__Fmo83")
                    .css("width", `${(percentage * 100).toFixed(2)}%`)

                let sc = document.querySelector("." + Constant._class_gameResult_StreamerPoints);
                if (sc && score != -1)
                {
                    sc.textContent = score + " points";
                }
                else
                {
                    sc = document.querySelector("." + Constant._class_roundResult_StreamerPoints);
                    if (sc && score != -1)
                    {
                        sc.textContent = score + " points";
                    }
                }

            }, 1000)

            Visual.SetScoreboardTitleCount(rows.length);

            window.setTimeout(async () => await Visual.PostInfinityGameButtons(), 1500);
        });
        GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(Util.SaveGame);
        Control.AddCopyMapLinkButton();
        Control.AddCopyResultLinkButton();
    }
    //////////////////////
    /// Util vars
    //////////////////////

    var checked: number = 0

    ////////////////////////////////////////////
    /// Util funcs
    ////////////////////////////////////////////
    export function ResetCheckStates()
    {
        State.App.HideMarkerState = true;
        State.App.CheckEventAdded = false;
        State.Event.AddedClick = false;
        State.Event.AddedClickEnd = false;
        State.Event.AddedClickViewSummary = false;
    }

    /** Last panoroma location after panoid change */
    export var lastPanoData: Nullable<google.maps.StreetViewLocation>;
    export var lastHandledPanoChange: Nullable<google.maps.StreetViewLocation>;

    export function OnStreetviewPositionChange()
    {
        console.log("Streetview position change", lastPanoData?.pano, GeoChatter.Streetview?.getLocation()?.pano)
        if (GeoChatter.Streetview
            && GeoChatter.Main.CurrentGame
            && GeoChatter.Main.CurrentGame.CurrentRound
            && (!lastHandledPanoChange || (GeoChatter.Streetview?.getPano() != lastHandledPanoChange?.pano)))
        {
            lastPanoData = GeoChatter.Streetview?.getLocation();
            console.log("Streetview position change saved", lastPanoData)
        }
    }

    async function HandlePostRoundStartData(round: Round)
    {
        if (round.Location.PanoOverwritten) return;

        console.log("HandlePostRoundStartData");
        await CefSharp.BindObjectAsync('jsHelper');

        while (!GeoChatter.Streetview
            || !GeoChatter.Streetview.getLocation() // First round streetview instance not ready
            || !lastPanoData
        )
        {
            await new Promise((res, _rej) => window.setTimeout(res, 10));
        }

        if (round.Location.PanoOverwritten) return;

        round.Location.PanoOverwritten = true;
        lastHandledPanoChange = lastPanoData;
        lastPanoData = null;
        await OverwriteRoundLocationData(round, lastHandledPanoChange)
    }

    async function OverwriteRoundLocationData(round: Round, data: google.maps.StreetViewLocation)
    {
        //let l = data.latLng;
        // TODO: Commented out as a temporary fix
        //  It sometimes end up using the previous round location
        //let lat = l?.lat() ?? 0;
        //let lng = l?.lng() ?? 0;
        //round.Location.Latitude = lat;
        //round.Location.Longitude = lng;
        round.Location.Pano = data.pano ?? "";

        console.log(`Overwriting round ${round.ID} latlng: ${round.Location.Latitude}, ${round.Location.Longitude} | data: ${round.Location.Pano}`, data)
        await jsHelper.overwriteRoundData(round.ID, round.Location.Latitude, round.Location.Longitude, round.Location.Pano);
    }

    export function TryAddCCheck(): void
    {
        if (document.getElementById(Constant._id_empty_div_check) || !Util.IsCurrentlyInGame()) return

        if (Div.Get5RoundGameSummaryDiv()) Div.Set5RoundGameSummaryDivPlaceHolder()
        else if (Div.Get5RoundGameSummaryEndDiv()) Div.Set5RoundGameSummaryDivPlaceHolderEnd()
        else if (Div.GetStreakGameSummaryDiv()) Div.SetStreakGameSummaryDivPlaceHolder()

        let c = document.getElementById(Constant._id_empty_div_check);
        if (c && !State.App.CheckEventAdded)
        {
            State.App.CheckEventAdded = true
        }
    }

    function IsStorageChecked(): boolean
    {
        return sessionStorage.getItem(Constant._storage_checked) != "0"
    }

    export function CheckSelector(): void
    {
        if (!!document.querySelector("." + Constant._class_roundResult_root_layout) && Util.IsCurrentlyInGame() && !IsStorageChecked())
        {
            console.log("Triggering streamer guess");
            checked = checked + 1;
            sessionStorage.setItem(Constant._storage_checked, checked.toString());

            StreamerGuess();
        } else if (!document.querySelector("." + Constant._class_roundResult_root_layout) && Util.IsCurrentlyInGame() && IsStorageChecked())
        {
            checked = 0;
            sessionStorage.setItem(Constant._storage_checked, checked.toString())
            State.App.CheckEventAdded = false;
        };
    }

    async function StreamerGuess()
    {
        if (!GeoChatter.Main.CurrentGame) return;

        await CefSharp.BindObjectAsync('jsHelper');

        if (GeoChatter.Main.CurrentGame.Mode == Enum.GAMEMODE.STREAK)
        {
            await jsHelper.setStreamerLastStreaksGameGuess(MapUtil.StreamerStreaksMarker?.getPosition()?.lat() ?? 0, MapUtil.StreamerStreaksMarker?.getPosition()?.lng() ?? 0);
        }

        let loc = GeoChatter.Streetview?.getLocation();
        if (loc
            && GeoChatter.Main.CurrentGame.CurrentRound
            && !GeoChatter.Main.CurrentGame.CurrentRound?.Location.PanoOverwritten)
        {
            await OverwriteRoundLocationData(GeoChatter.Main.CurrentGame.CurrentRound, loc)
        }

        jsHelper.endRound(GeoChatter.Main.CurrentGame.ID, GeoChatter.Main.CurrentGame.Settings.IsChallenge, State.App.NextStreamerGuessIsRandom)
            .then(async function(result)
            {
                if (!GeoChatter.Main.CurrentGame) return;

                let round = GeoChatter.Main.CurrentGame.CurrentRound;

                if (!round) return;

                console.log("Ending round", round)

                GeoChatter.Main.CurrentGame.Stage = Enum.GAMESTAGE.ENDROUND;
                let flg = Setting.Overlay.DisplayCorrectLocations ?
                    `<div id="flagstitle">${round.GetFlagHTML()}</div>` :
                    "";

                let streamerGuess = round.Guesses[round.Guesses.length - 1];

                await Util.SaveAllScoreboardSettingsForStage(Enum.GAMESTAGE.INROUND);
                await Visual.SetNewScoreboard(GeoChatter.Main.CurrentGame.Mode,
                    Enum.GAMESTAGE.ENDROUND,
                    round.AsTableRows(result),
                    `${flg}ROUND ${round.Game.TotalRoundCount} RESULTS`,
                    false);

                window.setTimeout(() =>
                {
                    let sc = document.querySelector("." + Constant._class_roundResult_StreamerPoints);
                    if (sc && streamerGuess)
                    {
                        let p = sc.textContent?.split(" ");
                        let scs = streamerGuess.Score;
                        // TODO: Get current perfect score instead of 5000 here
                        let percentage = Math.min(scs / 5000, 1);
                        if (p && !Util.IsSameScore(p[0], scs.toString())) sc.textContent = scs + " (GeoGuessr: " + p[0] + ") points"
                        $(".progress-bar_progress__Fmo83")
                            .css("width", `${(percentage * 100).toFixed(2)}%`)
                    }
                }, 1500)

                MapUtil.AddMarkerRemover();
                GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() => window.setTimeout(() => round?.CalculateGuessOrders(), 20))
                GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() => window.setTimeout(MapUtil.PopulateMapWithCurrentRound, 100));
                GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() => window.setTimeout(Util.AutoexportFuncFor(Enum.GAMESTAGE.ENDROUND), 250));
            });
    }
}

export namespace CustomEvents
{
    export var PostGuessButtonTickerBackupInterval: number;

    /** All event listeners */
    export const CustomEvents: CustomEventTable =
    {
        [Enum.EventTargetBase.window]: {
            /** 
                * EVENTS FIRED FROM C#
                **/
            // - GAME START
            [Enum.EventName.StartGameEvent]: [
                { Handler: EventHandler.OnStartGameEvent, Condition: () => State.App.LoadCompleted, RetryInterval: 500 }
            ],

            // - REFRESH GAME
            [Enum.EventName.RefreshGameEvent]: [
                { Handler: EventHandler.OnRefreshGameEvent, Condition: () => State.App.LoadCompleted && State.App.StartedCurrentGame, RetryInterval: 200 }
            ],

            // - EXIT GAME
            [Enum.EventName.ExitGameEvent]: [
                { Handler: EventHandler.OnExitGameEvent, Condition: () => State.App.LoadCompleted && State.App.StartedCurrentGame, RetryInterval: 0 }
            ],

            // - ROUND START
            [Enum.EventName.StartRoundEvent]: [
                { Handler: EventHandler.OnStartRoundEvent, Condition: () => State.App.LoadCompleted && State.App.StartedCurrentGame && !State.App.ClosingTheRound, RetryInterval: 100 }
            ],

            // - NEW GUESS
            [Enum.EventName.NewGuessEvent]: [
                { Handler: EventHandler.OnNewGuessEvent, Condition: () => State.App.LoadCompleted && State.App.StartedCurrentGame, RetryInterval: 333 }
            ],

            // - REGISTER GUESS BUTTON EVENTS
            [Enum.EventName.RegisterClickEvent]: [
                { Handler: EventHandler.OnGuessButtonClicked_Listener, Condition: () => !State.Event.AddedClick, RetryInterval: 0 }
            ],

            // - END ROUND LISTENER (NEXT ROUND)
            [Enum.EventName.EndRoundEvent]: [
                { Handler: EventHandler.OnNextRoundClicked_Listener, Condition: () => !State.Event.AddedClickEnd, RetryInterval: 0 }
            ],
             // - END ROUND LISTENER (NEXT ROUND)
            [Enum.EventName.DrawMapLinkButton]: [
                { Handler: EventHandler.OnDrawMapLinkButton, Condition: null, RetryInterval: 0 }
            ],

            // - END GAME (FINAL RESULTS)
            [Enum.EventName.EndGameEvent]: [
                { Handler: EventHandler.OnViewSummaryClicked_Listener, Condition: () => !State.Event.AddedClickViewSummary, RetryInterval: 0 }
            ],

            // - END INFINITY GAME (FINAL RESULTS)
            [Enum.EventName.EndInfinityGameEvent]: [
                { Handler: EventHandler.OnEndInfinityGame, Condition: null, RetryInterval: 0 }
            ],

            // - SETTINGS CHANGE
            [Enum.EventName.SettingsUpdateEvent]: [
                { Handler: EventHandler.ApplySettings, Condition: null, RetryInterval: 0 }
            ],

            // - ADDRESS CHANGED TO MAIN PLAY SCREEN
            [Enum.EventName.AddressMainPlayScreenEvent]: [
                { Handler: EventHandler.OnAddressMainPlayScreen, Condition: () => State.App.LoadCompleted, RetryInterval: 500 }
            ],
            // - GUESS BUTTON CLICK
            [Enum.EventName.LoadingScreenEvent]: [
                { Handler: EventHandler.LoadingScreenRequest, Condition: null, RetryInterval: 0 }
            ],
            // - GUESS BUTTON CLICK
            [Enum.EventName.SignOutEvent]: [
                { Handler: EventHandler.SignOutFromBrowser, Condition: null, RetryInterval: 0 }
            ],
            /**
                * EVENTS FIRED FROM BROWSER
                **/
            // - GUESS BUTTON CLICK
            [Enum.EventName.click]: [
                { Handler: EventHandler.PostGuessButtonClickHandlers, Condition: null, RetryInterval: 0 },
                { Handler: Visual.DropDownMenuFocusOut, Condition: null, RetryInterval: 0 }
            ],
        },

        [Enum.EventTargetBase.document]:
        {
            // Wheel scroll 
            [Enum.EventName.wheel]: [
                { Handler: EventHandler.OnWheelScrollZoom, Condition: null, RetryInterval: 0 }
            ],

            // Key down
            [Enum.EventName.keydown]: [
                { Handler: EventHandler.OnKeyDownCTRL, Condition: null, RetryInterval: 0 }
            ],

            // Key up
            [Enum.EventName.keyup]: [
                { Handler: EventHandler.OnKeyUpCTRL, Condition: null, RetryInterval: 0 }
            ],
        },
    }
    
    export const EventTargets: EventTargetTable =
    {
        [Enum.EventTargetBase.window]: Enum.EventSubTarget.self,
        [Enum.EventTargetBase.document]: Enum.EventSubTarget.body,
    };

    export function InvokeEventHandler(el: Element, listener: EventHandlerData, args: EventArgs): void
    {
        let handler = listener.Handler;
        if (!listener.Condition
            || listener.Condition()
            || handler)
        {
            //console.debug("Invoking handler", handler.name, el, args);
            handler(el, args)
        }
        else if (listener.RetryInterval > 0)
        {
            //console.debug("Delaying invoke", listener);
            window.setTimeout(() => InvokeEventHandler(el, listener, args), listener.RetryInterval)
        }
    }

    export function AddInitialEventHandlers(): void
    {
        for (const [eventBase, eventTarget] of Object.entries(EventTargets))
        {
            let target = (window as any)[eventBase][eventTarget] as Element;
            let evdata = CustomEvents[eventBase as EventTargetBase] as CustomEventDetail;
            for (const [eventName, listenerArray] of Object.entries(evdata))
            {
                listenerArray.forEach((listener, idx) =>
                {
                    console.log(`Adding event handler to ${eventBase}.${eventTarget}: ${eventName}: index ${idx}`);
                    target.addEventListener(eventName, function(this: Element, args: Event)
                    {
                        if (!listener.Handler)
                        {
                            // MisfireQueue.push({ e: eventName, l: listener, a: args })
                            console.warn(`Tried firing too early: ${eventBase} -> ${eventName}`)
                            return
                        }
                        InvokeEventHandler(this, listener, args as EventArgs);
                    }, false);
                })
            }
        }
    }
}

window.GC.CustomEvents = CustomEvents;
window.GC.EventHandler = EventHandler;