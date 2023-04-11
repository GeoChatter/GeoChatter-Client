import "./core"
import { GeoChatter } from "./geochatter";
import { MapUtil } from "./maps";
import { Guess } from "./models";
import { Util } from "./utils";
import { Visual } from "./visuals";
import { Enum } from "./enums"
import { Constant } from "./constants";
import { Color } from "./colors";
import { State, Setting } from "./settings";

export namespace Control
{
    var RandomGuessBtnAdderInterval: number;
    var MapLinkInterval: number;
    var MapRandomInterval: number;
    var ResultLinkInterval: number;

    export const LastSWCorrectLocEmbed = {
        w: 600,
        h: 600,
        t: 25,
        l: 25
    }

    export const LastSWEmbed = {
        w: 600,
        h: 600,
        t: 25,
        l: 25
    }
    export async function OnChangeautoExportNotify(this: JQueryEventObject)
    {
        State.App.AlertExportSuccess = $(this).is(":checked")
        await Util.SetExportSettings();
    }

    export async function OnChangepreferredExportFormat(this: JQueryEventObject)
    {
        Setting.App.PreferredExportFormat = $(this).val() as ExportFormat;
        await Util.SetExportSettings();
    }

    export async function OnChangeautoExportRoundResults(this: JQueryEventObject)
    {
        State.App.AutoExportRoundResults = $(this).is(":checked")
        SetExportFormatDropdownDisplay();
        await Util.SetExportSettings();
    }

    export async function OnChangeautoExportRoundStandings(this: JQueryEventObject)
    {
        State.App.AutoExportRoundStandings = $(this).is(":checked")
        SetExportFormatDropdownDisplay();
        await Util.SetExportSettings();
    }

    export async function OnChangeautoExportGameResults(this: JQueryEventObject)
    {
        State.App.AutoExportGameResults = $(this).is(":checked")
        SetExportFormatDropdownDisplay();
        await Util.SetExportSettings();
    }

    /**
    * Create a new scoreboard
    * @param {GAMEMODE} mode game mode
    * @param {GAMESTAGE} stage game stage
    */
    export async function ScoreboardConstructor(mode: GAMEMODE, stage: GAMESTAGE)
    {
        while (!State.App.LoadCompleted
            || !$
            || !$('#' + Constant._id_datatable).dxDataGrid
            || !window.DevExpress) await new Promise((rs, _rj) => window.setTimeout(rs, 100))

        let dt = $('#' + Constant._id_datatable);
        console.log("Scoreboard constructor: " + stage, dt);

        GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(Visual.ScoreboardFirstContentReadyCallback(stage));

        let m = mode, s = stage;
        GeoChatter.Main.SingleOnContentReadyWeakCallbacks.push(() => GeoChatter.Map?.setMapTypeId(Setting.ScoreboardDisplay[m][s].MinimapLayer));

        let t = dt.dxDataGrid(Visual.GetDataGridConfig(mode, stage))
            .dxDataGrid("instance");

        console.log("Making scoreboard interactable", t)
        await MakeInteractable(t, stage);
        console.log("Scoreboard is now interactable", t)

        return t;
    }
    async function MakeInteractable(table: DevExpress.ui.dxDataGrid, stage: GAMESTAGE)
    {
        try
        {
            if (!GeoChatter.Main.CurrentGame) return;
            console.log("MakeInteractable", $, table, stage, $("#resizeable_container")[0])

            while (!$("#resizeable_container")[0]) await new Promise((rs, _rj) => window.setTimeout(rs, 100))

            let scoreboard = $("#resizeable_container");

            if (stage == null) stage = GeoChatter.Main.CurrentGame.Stage;

            $(".dx-datagrid-header-panel")[0]?.setAttribute("id", "draghandle");
            $("#draghandle")
                .css("cursor", "move")
                .on("dblclick", Visual.ToggleMinimized);


            while (!scoreboard.dxResizable
                || !scoreboard.draggable)
            {
                await new Promise((res, _rej) => window.setTimeout(res, 100));
            }

            scoreboard
                .dxResizable({
                    width: Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Width,
                    height: Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Height,
                    minWidth: Constant.MinimumTableWidth,
                    async onResizeEnd(e: DevExpress.ui.dxResizable.ResizeEndEvent)
                    {
                        console.log("Resize end", e);
                        if (!GeoChatter.Main.CurrentGame || !GeoChatter.Main.Table) return;
                        State.Scoreboard.ShouldUseLastScoreBoardSettings = true;
                        Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Width = GeoChatter.Main.Table.element().width() ?? Constant.MinimumTableWidth;
                        if (!Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].IsMinimized) Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Height = GeoChatter.Main.Table.element().height() ?? 60;
                        await Util.SaveAllScoreboardSettingsForStage(stage);
                    }
                }).draggable({
                    containment: "#scoreboardContainer",
                    handle: "#draghandle"
                }).on(
                    "dragstop",
                    async function(_event, ui: JQueryUI.DraggableEventUIParams)
                    {
                        if (!GeoChatter.Main.CurrentGame) return;
                        State.Scoreboard.ShouldUseLastScoreBoardSettings = true;
                        Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Top = ui.position.top;
                        Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Left = ui.position.left;
                        await Util.SaveAllScoreboardSettingsForStage(stage);
                    }
                );

            console.log("Scoreboard is now resizable and draggable");

            table.element().parent().offset({ top: Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Top, left: Setting.ScoreboardDisplay[GeoChatter.Main.CurrentGame.Mode][stage].Left });
        }
        catch (e)
        {
            console.error(e)
        }
    }

    export function ButtonLabel(lbl: string)
    {
        return $("<div>")
            .addClass("button_wrapper__NkcHZ")
            .append(
                $("<span>")
                    .addClass("button_label__kpJrA")
                    .text(lbl));
    }

    export function StatusLabel(lbl: string, val: string)
    {
        return [
            $("<div>")
                .addClass("status_label__SNHKT")
                .text(lbl),
            $("<div>")
                .addClass("status_value__xZMNY")
                .text(val)
        ];
    }

    export function HeaderBars(text: string): JQuery<HTMLElement>
    {
        return $("<div>")
            .addClass("bars_root___G89E bars_center__vAqnw")
            .append(
                $("<div>")
                    .addClass("bars_before__xAA7R bars_lengthLong__XyWLx"),
                $("<span>")
                    .addClass("bars_content__UVGlL")
                    .append(
                        $("<h2>")
                            .text(text)
                    ),
                $("<div>")
                    .addClass("bars_after__Z1Rxt bars_lengthLong__XyWLx")
            );
    }

    export function InfiniteGameButton(): JQuery<HTMLElement>
    {
        return $("<button>")
            .attr("id", "startInfinityButton")
            .css("margin-left", "2em")
            .css("background", "purple")
            .attr("type", "button")
            .addClass("button_button__CnARx")
            .append(Control.ButtonLabel("Start An Infinite Game!"))
            .on("click", Util.MarkNextStartAsInfinite)
    }

    export async function RoundSettingsContainer(mainpage: boolean = false): Promise<JQuery<HTMLElement>>
    {
        let dropdownExportFormat = DropdownRoundSettingsElement("Preferred export format",
            "exportFormatDropdown",
            OnChangepreferredExportFormat,
            {
                [Enum.ExportFormat.xlsx]: Enum.ExportFormat.xlsx.toUpperCase(),
                [Enum.ExportFormat.csv]: Enum.ExportFormat.csv.toUpperCase()
            },
            Setting.App.PreferredExportFormat
        ).hide();

        SetExportFormatDropdownDisplay(dropdownExportFormat);

        let layers = GeoChatter.Main.LastChild?.LastChild?.Settings?.Layers ?? GeoChatter.Main.InitiallyAvailableLayers;

        return $("<div>")
            .attr("id", "roundSettingsContainer")
            .addClass("section_sectionHeader__WQ7Xz section_sizeMedium__yPqLK")
            .append(
                HeaderBars("Round Settings"),

                CheckboxRoundSettingElement(mainpage ? "Enable multiguessing for the first round" : "Enable multiguessing for next round",
                    "start-firstround-multiguess-button",
                    Util.MarkNextRoundAsMultiGuess,
                    State.App.PreferredMultiguess),

                HeaderBars("Map Settings"),

                CheckboxDropdownRoundSettingElement("Available Layers",
                    "round-layers-map",
                    (layer: any) => Util.ChangeRoundSetting("Layers", layer, false),
                    GeoChatter.Main.InitiallyAvailableLayers,
                    (layer: any) => layers.indexOf(layer) != -1),

                NumericInputRoundSettingElement("Maximum zoom level (1-23)",
                    "round-maxzoom-level",
                    (e: JQuery<HTMLElement>) => Util.ChangeRoundSetting("MaxZoomLevel", parseFloat(e.val()?.toString() ?? "23") as number, false),
                    GeoChatter.Main.LastChild?.LastChild?.Settings?.MaxZoomLevel),

                CheckboxRoundSettingElement("Allow 3D Viewing",
                    "round-is3d-map",
                    (_: Event) => Util.ChangeRoundSetting("Is3dEnabled", $("[data-qa=round-is3d-map]").is(":checked"), false),
                    GeoChatter.Main.LastChild?.LastChild?.Settings?.Is3dEnabled ?? false),

                CheckboxRoundSettingElement("Blur Effect",
                    "round-blurry-map",
                    (_: Event) => Util.ChangeRoundSetting("Blurry", $("[data-qa=round-blurry-map]").is(":checked"), false),
                    GeoChatter.Main.LastChild?.LastChild?.Settings?.Blurry ?? false),

                // TODO: Figure out why B&W is shown as enabled in rounds 2-5, even though it is in fact disabled
                //CheckboxRoundSettingElement("Black & White Effect",
                //    "round-blackandwhite-map",
                //    (_: Event) => Util.ChangeRoundSetting("BlackAndWhite", $("[data-qa=round-blackandwhite-map]").is(":checked"), false),
                //    GeoChatter.Main.LastChild?.LastChild?.Settings?.BlackAndWhite ?? false),

                CheckboxRoundSettingElement("Mirrored Map",
                    "round-mirrored-map",
                    (_: Event) => Util.ChangeRoundSetting("Mirrored", $("[data-qa=round-mirrored-map]").is(":checked"), false),
                    GeoChatter.Main.LastChild?.LastChild?.Settings?.Mirrored ?? false),

                CheckboxRoundSettingElement("Upside Down Map",
                    "round-upsidedown-map",
                    (_: Event) => Util.ChangeRoundSetting("UpsideDown", $("[data-qa=round-upsidedown-map]").is(":checked"), false),
                    GeoChatter.Main.LastChild?.LastChild?.Settings?.UpsideDown ?? false),

                CheckboxRoundSettingElement("Sepia Effect",
                    "round-sepia-map",
                    (_: Event) => Util.ChangeRoundSetting("Sepia", $("[data-qa=round-sepia-map]").is(":checked"), false),
                    GeoChatter.Main.LastChild?.LastChild?.Settings?.Sepia ?? false),

                HeaderBars("Export Settings"),

                CheckboxRoundSettingElement("Export round results automatically",
                    "autoexport-round-button",
                    OnChangeautoExportRoundResults,
                    State.App.AutoExportRoundResults
                ),
                //checkboxRoundSettingElement("Export current standings automatically",
                //    "autoexport-standings-button",
                //    onChangeautoExportRoundStandings,
                //    autoExportRoundStandings
                //),
                CheckboxRoundSettingElement("Export game result automatically",
                    "autoexport-game-button",
                    OnChangeautoExportGameResults,
                    State.App.AutoExportGameResults),

                CheckboxRoundSettingElement("Notify when exporting succeeds",
                    "autoexport-notify-button",
                    OnChangeautoExportNotify,
                    State.App.AlertExportSuccess
                ),

                dropdownExportFormat
            );
    }

    export function SetExportFormatDropdownDisplay(el?: Nullable<JQuery<HTMLElement>>): void
    {
        if (!el || !el[0]) el = $("#exportFormatDropdown");
        if (State.App.AutoExportGameResults || State.App.AutoExportRoundResults || State.App.AutoExportRoundStandings)
        {
            el.show();
        }
        else
        {
            el.hide();
        }
    }

    export function CheckboxDropdownRoundSettingElement(text: string, inputqa: string,
        inputcallback: Callback<any, void>, allItems: any[],
        startChecked: Callback<any, boolean>): JQuery<HTMLElement>
    {

        let container = $("<div>")
            .addClass("checkboxlist")
            .attr("data-qa", inputqa)
            .data("qa", inputqa);

        let root = $("<div>")
            .addClass("checkboxlistroot")
            .append($("<span>").text(text))
            .append(container);

        let inputqaelement = inputqa + "-element";

        for (let c of allItems)
        {
            let val = c;
            let inp = $("<input>")
                .attr("type", "checkbox")
                .attr("data-qa", inputqaelement)
                .data("qa", inputqaelement)
                .addClass("toggle_toggle__hwnyw")
                .on("change", () => inputcallback(val))
                .prop("checked", startChecked(c));

            let ele = RoundSettingElementContainer(
                $("<div>")
                    .addClass("game-settings_toggleLabel__nipwm")
                    .append(
                        $("<div>")
                            .addClass("label_sizeXSmall__mFnrR")
                            .text(c)
                    ),
                inp
            );

            container.append(ele);
        }

        return root;
    }

    export function NumericInputRoundSettingElement(text: string,
        inputqa: string,
        inputcallback: Callback<JQuery<HTMLElement>, void>,
        start: number = 23,
        min: number = 1,
        max: number = 23): JQuery<HTMLElement>
    {
        if (!start || start <= 0 || start > max || start < min)
        {
            start = max;
        }

        let inp = $("<input>")
            .attr("data-qa", inputqa)
            .data("qa", inputqa)
            .attr("type", "number")
            .attr("min", min.toString())
            .attr("max", max.toString())
            .addClass("numericinputSetting")
            .change(function()
            {
                let v = $(this).val() ?? 0;
                if (v > max)
                {
                    $(this).val(max);
                }
                else if (v < min)
                {
                    $(this).val(min);
                }
                inputcallback($(this))
            })
            .val(start);

        let ele = RoundSettingElementContainer(
            $("<div>")
                .addClass("game-settings_toggleLabel__nipwm")
                .css("margin", "auto 0")
                .append(
                    $("<div>")
                        .addClass("label_sizeXSmall__mFnrR")
                        .text(text)
                ),
            $("<div>")
                .css("margin", "auto 0")
                .append(inp)
        );

        return ele;
    }

    export function CheckboxRoundSettingElement(text: string, inputqa: string, inputcallback: Callback<Event, void>, startchecked: boolean = false): JQuery<HTMLElement>
    {
        let inp = $("<input>")
            .attr("data-qa", inputqa)
            .data("qa", inputqa)
            .attr("type", "checkbox")
            .addClass("toggle_toggle__hwnyw")
            .on("change", inputcallback)
            .prop("checked", startchecked);

        let ele = RoundSettingElementContainer(
            $("<div>")
                .addClass("game-settings_toggleLabel__nipwm")
                .append(
                    $("<div>")
                        .addClass("label_sizeXSmall__mFnrR")
                        .text(text)
                ),
            inp
        );

        return ele;
    }

    export function DropdownRoundSettingsElement(text: string, id: string, inputcallback: Callback<Event, void>, items: {[k: string]: string}, def: string | number | string[]): JQuery<HTMLElement>
    {
        let slc = $('<select />')
            .val(def)
            .addClass("dropdownroundSetting")
            .on("change", inputcallback)

        for (var val in items)
        {
            $('<option />', { value: val, text: items[val] }).appendTo(slc);
        }

        let ele = RoundSettingElementContainer(
            $("<div>")
                .addClass("game-settings_toggleLabel__nipwm")
                .append(
                    $("<div>")
                        .addClass("label_sizeXSmall__mFnrR")
                        .text(text)
                ),
            $("<div>")
                .append(slc)
        )
            .attr("id", id);

        return ele;
    }

    export function RoundSettingElementContainer(...cols: JQuery<HTMLElement>[]): JQuery<HTMLElement>
    {
        return $("<div>")
            .addClass("section_sectionMedium__yXgE6")
            .append(
                $("<div>")
                    .addClass("start-standard-game_settings__x94PU")
                    .append(
                        $("<div>")
                            .addClass("game-settings_default__DIBgs")
                            .addClass("settingRow")
                            .append(...cols)
                    )
            );
    }

    export function CreateIFrame(elementId: string, src: string): HTMLIFrameElement
    {
        let f = document.createElement("iframe");

        f.id = elementId;
        f.src = src;
        f.width = "95%";
        f.height = "81%";

        return f;
    }

    function CreateGoogleMapsAnchor(coords: string)
    {
        let anc = document.createElement("a")
        anc.href = `https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=${coords}`
        anc.text = "Open in Google Maps in a new tab...";
        anc.target = "_blank";
        anc.style.display = "block";
        anc.style.textAlign = "center";
        anc.style.fontSize = "1.3em";
        anc.style.fontWeight = "800";
        anc.style.color = "white";
        anc.style.width = "100%";
        anc.classList.add("username-dark");
        return anc;
    }

    export async function EmbedLocation(iframeID: string, titlehtml: string, loc: google.maps.LatLngLiteral & { pano?: string }, heading: number = 0, pitch: number = 0, fov: number = 180, pano: Nullable<string>, src?: ICountrySource)
    {
        let prev = document.getElementById("embedggContainer_" + iframeID);
        if (prev) prev.remove();

        if (!pano && src && src.Location.Pano)
        {
            pano = src.Location.Pano;
        }

        if (!pano)
        {
            let resloc = await MapUtil.GetStreetviewPanorama({
                lat: loc.lat,
                lng: loc.lng
            }, Setting.Overlay.StreetViewMaxRadius);
            if (resloc)
            {
                loc = resloc;
                pano = loc.pano;
                console.log("Found pano id from coordinates")
            }
        }

        try
        {
            let url = "";

            let zoom = Math.log(180 / fov) / (Math.log(2)) - 1;
            pitch *= -1; // For some reason used reversed
            let coordstr = Util.CoordinatesToURLArgument(loc.lat, loc.lng);

            if (!pano)
            {
                if (src)
                {
                    let flg = src.GetFlagHTML();
                    let location = flg + src.GetCountryName();

                    if (src instanceof Guess)
                        EmbedStreetviewNotFound(src.PlayerData.FlagDisplay + Color.ColorUsername(src.PlayerData.Color, src.PlayerData.Display) + " in " + location, coordstr);
                    else
                        EmbedStreetviewNotFound("Correct location in " + location, coordstr);

                    return;
                }
            }
            else
            {
                console.log("Embedding: ", iframeID, titlehtml, coordstr, heading, pitch, fov, pano)

                url = `https://maps.google.com/maps?layer=c&panoid=${pano}&source=embed&output=svembed&ie=UTF8&cbp=,${heading + 0.0001},,${zoom},${pitch + 0.0001}`;
            }

            let f = CreateIFrame(iframeID, url);

            let maindiv = document.createElement("div");
            let subdiv = document.createElement("div")
            let title = document.createElement("h3");
            let closebtn = document.createElement("span");
            let titleinfo = document.createElement("div");

            closebtn.classList.add("embedCloseBtn");
            closebtn.textContent = "X"

            closebtn.onclick = () =>
            {
                $("#embedggContainer_" + iframeID).remove()
            }

            title.classList.add("username-dark");
            title.classList.add("embedtitle");

            titleinfo.classList.add("embedtitleinfo");
            titleinfo.innerHTML = titlehtml;

            title.appendChild(titleinfo)
            title.appendChild(closebtn)

            subdiv.appendChild(title)
            subdiv.appendChild(f)

            let anc = CreateGoogleMapsAnchor(coordstr);
            subdiv.appendChild(anc)


            subdiv.id = "embedgg_" + iframeID
            subdiv.classList.add("embedgg");
            subdiv.style.width = LastSWEmbed.w + "px";
            subdiv.style.height = LastSWEmbed.h + "px";
            subdiv.style.border = "2px solid white";

            maindiv.id = "embedggContainer_" + iframeID
            maindiv.classList.add("embedggContainer");

            maindiv.appendChild(subdiv);

            document.body.appendChild(maindiv);

            $(subdiv)
                .resizable({
                    handles: "n, e, s, w, ne, se, sw, nw",
                    containment: "#embedggContainer_" + iframeID,
                })
                .draggable({
                    containment: "#embedggContainer_" + iframeID,
                })
                .on(
                    "dragstop",
                    async function(_event: JQuery.Event, ui: JQueryUI.DraggableEventUIParams)
                    {
                        LastSWEmbed.t = ui.position.top;
                        LastSWEmbed.l = ui.position.left;
                    })
                .on(
                    "resizestop",
                    async function(_event: JQuery.Event, _ui: JQueryUI.DraggableEventUIParams)
                    {
                        LastSWEmbed.w = $("#embedgg_" + iframeID).width() ?? 25;
                        LastSWEmbed.h = $("#embedgg_" + iframeID).height() ?? 25;
                    })
                .offset({ top: LastSWEmbed.t + 5, left: LastSWEmbed.l + 5 });
        }
        catch (e)
        {
            console.error("Embedding error: " + e)
        }
    }

    export async function EmbedPlayerStats(guess: Nullable<Guess>, placement: number)
    {
        let id = Constant.PlayerStatsEmbedID

        let prev = document.getElementById("embedstatsContainer_" + id);
        if (prev) prev.remove();

        if (!guess || !guess.PlayerData.Bests) return;

        let img = Util.DataUrlToMarkerSVGDataUrl(guess.PlayerData.MarkerData ?? "", Setting.Overlay.MarkerSize, guess.PlayerData.Color, Setting.Overlay.MarkerBorderSize);
        let stats = document.createElement("div")
        stats.id = id;
        stats.innerHTML = `
<table style='width: 100%; margin-left: auto; margin-right: auto'>
    <colgroup>
        <col style="background-color:var(--gc-gg-main-color-transparent-high)">
        <col style="background-color: var(--gc-gg-main-color-lightest-transparent-high);">
    </colgroup>
    <tbody>
        <tr>
            <td>Best Streak</td>
            <td>${guess.PlayerData.Bests.BestStreak}</td>
        </tr>
        <tr>
            <td>Correct Countries</td>
            <td>${(guess.PlayerData.Bests.NumberOfCountries != 0 ? `${guess.PlayerData.Bests.CorrectCountries}/${guess.PlayerData.Bests.NumberOfCountries} (${(guess.PlayerData.Bests.CorrectCountries * 100 / guess.PlayerData.Bests.NumberOfCountries).toFixed(Setting.Overlay.RoundingDigits)}%)` : "-")}</td>
        </tr>
        <tr>
            <td>Average Distance</td>
            <td>${(guess.PlayerData.Bests.TotalDistance != 0 ? `${Util.GetConvertedDistance(guess.PlayerData.Bests.TotalDistance / guess.PlayerData.Bests.NoOfGuesses).display}` : "-")}</td>
        </tr>
        <tr>
            <td>Average Score</td>
            <td>${(guess.PlayerData.Bests.NoOfGuesses != 0 ? `${(guess.PlayerData.Bests.SumOfGuesses / guess.PlayerData.Bests.NoOfGuesses).toFixed(Setting.Overlay.RoundingDigits)}` : "-")}</td>
        </tr>
        <tr>
            <td>Perfect Rounds</td>
            <td>${guess.PlayerData.Bests.NoOf5kGuesses}</td>
        </tr>
        <tr>
            <td>Wins</td>
            <td>${guess.PlayerData.Bests.Wins}</td>
        </tr>
        <tr>
            <td>Perfect Games</td>
            <td>${guess.PlayerData.Bests.Perfects}</td>
        </tr>
    </tbody>
</table>
`

        let maindiv = document.createElement("div");
        document.body.appendChild(maindiv);

        let subdiv = document.createElement("div")
        let header = document.createElement("div")
        let order = document.createElement("sup")
        order.style.position = "absolute";
        order.style.right = "2%";
        order.style.fontSize = "1.3em";
        order.style.fontWeight = "bolder";
        order.classList.add("username-dark");
        order.textContent = `#${placement}`

        let title = document.createElement("h2")
        let imgel = document.createElement("img")

        title.classList.add("username-dark");
        title.innerHTML = "Best of " + guess.GetPlayerNameDisplayHTML() + "";

        imgel.src = img;
        imgel.style.width = (Setting.Overlay.MarkerSize + Setting.Overlay.MarkerBorderSize) + "px"
        imgel.style.height = (Setting.Overlay.MarkerSize + Setting.Overlay.MarkerBorderSize) + "px"
        imgel.style.padding = "2%"
        imgel.style.float = "left"

        header.classList.add("embedstatstitle")
        header.appendChild(imgel)
        header.appendChild(order)
        header.appendChild(title)

        subdiv.appendChild(header)
        subdiv.appendChild(stats)

        subdiv.id = "embedstats_" + id
        subdiv.classList.add("embedgg");
        subdiv.style.width = "min-content";
        subdiv.style.boxShadow = "unset";
        subdiv.style.minHeight = "unset";
        subdiv.style.marginLeft = (GeoChatter.Main.Table as any)._controllers.columns._columns.find((c: DevExpress.ui.dxDataGridColumn) => c.dataField == Enum.DataField.PlayerName).visibleWidth + "px";

        Visual.ApplyDefaultCssTo($(subdiv));

        subdiv.style.border = "2px solid " + guess.PlayerData.Color;

        maindiv.id = "embedstatsContainer_" + id
        maindiv.classList.add("embedggContainer");

        let row = Util.GetTableRowOf(guess.PlayerData)
        if (row)
        {
            let offset = row.offset()
            if (offset)
            {
                offset.top = offset.top + (row.height() ?? 30) + 4
                $(maindiv).offset(offset)
            }
        }

        maindiv.appendChild(subdiv);
    }

    export function EmbedStreetviewNotFound(titleExtra: string = "", coords: string = "")
    {

        let id = "GCerror-frame"
        let prev = document.getElementById("embedggContainer_" + id);
        if (prev) prev.remove();

        let err = document.createElement("h2")
        err.id = id;
        err.textContent = "Couldn't find any streetview in " + (Setting.Overlay.StreetViewMaxRadius / 1000).toFixed(2) + "km radius of the guess!";

        let maindiv = document.createElement("div");
        document.body.appendChild(maindiv);

        let subdiv = document.createElement("div")
        let title = document.createElement("h2")
        let closebtn = document.createElement("span")
        let titleinfo = document.createElement("div");

        closebtn.classList.add("embedCloseBtn");
        closebtn.textContent = "X"

        closebtn.onclick = () =>
        {
            $("#embedgg_" + id).remove()
        }

        titleinfo.classList.add("embedtitleinfo");
        titleinfo.innerHTML = "No Streetview" + (titleExtra ? (` for ${titleExtra}`) : "");

        title.classList.add("username-dark");
        title.classList.add("embedtitle");

        title.appendChild(titleinfo)
        title.appendChild(closebtn);

        let anc = CreateGoogleMapsAnchor(coords);

        subdiv.appendChild(title)
        subdiv.appendChild(err)
        subdiv.appendChild(anc)

        subdiv.id = "embedgg_" + id
        subdiv.classList.add("embedgg");
        subdiv.style.width = LastSWEmbed.w + "px";
        subdiv.style.height = LastSWEmbed.h + "px";
        subdiv.style.border = "2px solid white";


        maindiv.id = "embedggContainer_" + id
        maindiv.classList.add("embedggContainer");

        maindiv.appendChild(subdiv);

        $(subdiv)
            .resizable({
                handles: "n, e, s, w, ne, se, sw, nw",
                containment: "#embedggContainer_" + id,
            })
            .draggable({
                containment: "#embedggContainer_" + id,
            })
            .on(
                "dragstop",
                async function(_event: JQuery.Event, ui: JQueryUI.DraggableEventUIParams)
                {
                    LastSWCorrectLocEmbed.t = ui.position.top;
                    LastSWCorrectLocEmbed.l = ui.position.left;
                })
            .on(
                "resizestop",
                async function(_event: JQuery.Event, _ui: JQueryUI.DraggableEventUIParams)
                {
                    LastSWCorrectLocEmbed.w = $("#embedgg_" + id).width() ?? 0;
                    LastSWCorrectLocEmbed.h = $("#embedgg_" + id).height() ?? 0;
                })
            .offset({ top: LastSWCorrectLocEmbed.t, left: LastSWCorrectLocEmbed.l });
    }

    export async function StreamerRandomGuess(this: HTMLButtonElement, _ev: MouseEvent)
    {
        if (!GeoChatter.Map || !confirm(`Are you sure you want to make a random guess?`)) return;

        console.log("StreamerRandomGuess")

        await CefSharp.BindObjectAsync('jsHelper');

        jsHelper.getRandomCoordinates().then(res =>
        {
            if (!GeoChatter.Map) return;

            console.log("StreamerRandomGuess got random coordinates", res)

            var o = JSON.parse(res) as Coordinate;
            google.maps.event.trigger(GeoChatter.Map, 'click', {
                latLng: new google.maps.LatLng(o.Latitude, o.Longitude)
            });
            Util.RepeatCallbackAttempt(Util.TriggerSendGuess, 10, 100);
        });
    }

    export async function CopyMapLink(this: HTMLButtonElement, _ev: MouseEvent)
    {
        await CefSharp.BindObjectAsync('jsHelper');
        jsHelper.copyMapLink();
    }

    export async function PlayRandomMap(this: HTMLButtonElement, _ev: MouseEvent)
    {
        await CefSharp.BindObjectAsync('jsHelper');
        jsHelper.playRandomMap();
    }
     export async function CopyResultLink(this: HTMLButtonElement, _ev: MouseEvent)
    {
        await CefSharp.BindObjectAsync('jsHelper');
        jsHelper.copyResultLink();
    }

    export function AddRandomGuessButton()
    {
        if (!RandomGuessBtnAdderInterval)
        {
            console.log("Adding random guess button")
            RandomGuessBtnAdderInterval = window.setInterval(() =>
            {
                var container = $(".guess-map__guess-button")[0]
                if (!container) return;
                if (!$("#randomGuessButton")[0])
                {
                    var randbtn = document.createElement("button");
                    randbtn.id = "randomGuessButton";
                    container.insertBefore(randbtn, container.firstChild);
                    
                    randbtn.classList.add("randomGuessBtn");
                    randbtn.title = "Random Guess";

                    randbtn.addEventListener("click", StreamerRandomGuess);
                }
                //clearInterval(window.RandomGuessBtnAdder);
                //window.RandomGuessBtnAdder = null;
            }, 50)
        }
    }

    export function AddCopyMapLinkButton()
    {
        if (!MapLinkInterval)
        {
            
            MapLinkInterval = window.setInterval(() => {
                var isDifferentStage = $("#mapLinkButton").data("stage") != GeoChatter.Main.CurrentGame?.Stage.toString();
                let btn = $("#mapLinkButton")[0];
                if (!btn || isDifferentStage) {

                    if (isDifferentStage && btn) {
                        btn.remove();
                        console.log("old map link button removed")
                    }

                    if (GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.ENDGAME
                        || GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.ENDROUND) {

                        var container = $(".result-layout_content__jAHfP")[0]
                        var cssClass = "mapLinkBtnIngame";

                    }
                    else if (GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.INROUND) {
                        var container = $(".status_inner__1eytg")[0]
                        var cssClass = "mapLinkBtnIngame";
                    } else {
                        var container = $(".header_context__UqsBa")[0]
                        var cssClass = "mapLinkBtnPage";
                    }
                    if (!container) {
                        console.log("Couldn't find container to add map link button to in stage " + GeoChatter.Main.CurrentGame?.Stage)
                        return;
                    }
                    var mapBtn = document.createElement("button");
                    mapBtn.id = "mapLinkButton";
                    //if (GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.ENDGAME
                    //    || GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.ENDROUND) {
                        container.insertBefore(mapBtn, container.firstChild);
                    //} else {
                    //    container.appendChild(mapBtn);
                    //}
                    if (GeoChatter.Main.CurrentGame)
                        $(mapBtn).data("stage", GeoChatter.Main.CurrentGame.Stage.toString());
                    mapBtn.classList.add(cssClass);
                    $(mapBtn).data("tooltip", "Copy map link");
                    mapBtn.innerText = "Copy link";
                    mapBtn.addEventListener("click", CopyMapLink);
                } 
                //clearInterval(window.RandomGuessBtnAdder);
                //window.RandomGuessBtnAdder = null;
            }, 50)
        }
    }

    export function AddPlayRandomMapLinkButton() {
        if (!MapRandomInterval) {

            MapRandomInterval = window.setInterval(() => {
                var isDifferentStage = $("#mapRandomButton").data("stage") != GeoChatter.Main.CurrentGame?.Stage.toString();
                let btn = $("#mapRandomButton")[0];
                if (!btn || isDifferentStage) {

                    if (isDifferentStage && btn) {
                        btn.remove();
                        console.log("old random map button removed")
                    }

             
                    var container = $(".header_context__UqsBa")[0]
                        var cssClass = "mapRandomBtnPage";
                    
                    if (!container) {
                        console.log("Couldn't find container to add random map link button to")
                        return;
                    }
                    var mapBtn = document.createElement("button");
                    mapBtn.id = "mapRandomButton";
                    //if (GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.ENDGAME
                    //    || GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.ENDROUND) {
                        container.insertBefore(mapBtn, container.firstChild);
                    //} else {
                    //    container.appendChild(mapBtn);
                    //}
                    if (GeoChatter.Main.CurrentGame)
                        $(mapBtn).data("stage", GeoChatter.Main.CurrentGame.Stage.toString());
                    mapBtn.classList.add(cssClass);
                    $(mapBtn).data("tooltip", "Play a random liked map");
                    mapBtn.innerText = "Random liked map";
                    mapBtn.addEventListener("click", PlayRandomMap);
                }
                //clearInterval(window.RandomGuessBtnAdder);
                //window.RandomGuessBtnAdder = null;
            }, 50)
        }
    }

    export function AddCopyResultLinkButton() {
        if (!ResultLinkInterval) {

            ResultLinkInterval = window.setInterval(() => {
                var isDifferentStage = $("#resultLinkButton").data("stage") != GeoChatter.Main.CurrentGame?.Stage.toString();
                let btn = $("#resultLinkButton")[0];
                if (!btn || isDifferentStage) {

                    if (isDifferentStage && btn) {
                        btn.remove();
                        console.log("old result link button removed")
                    }
                    var cssClass = "resultLinkBtn";
                    if (GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.ENDGAME) {

                        var container = $(".result-layout_content__jAHfP")[0]
                        if (!container) {
                            console.log("Couldn't find container to add result link button to")
                            return;
                        }
                        var resultBtn = document.createElement("button");
                        resultBtn.id = "resultLinkButton";
                        if (GeoChatter.Main.CurrentGame?.Stage == Enum.GAMESTAGE.ENDGAME) {
                            container.insertBefore(resultBtn, container.firstChild);
                        } 
                        if (GeoChatter.Main.CurrentGame)
                            $(resultBtn).data("stage", GeoChatter.Main.CurrentGame.Stage.toString());
                        resultBtn.classList.add(cssClass);
                        $(resultBtn).data("tooltip", "Copy result link");
                        resultBtn.innerText = "Results";
                        resultBtn.addEventListener("click", CopyResultLink);
                    }
                }
                //clearInterval(window.RandomGuessBtnAdder);
                //window.RandomGuessBtnAdder = null;
            }, 50)
        }
    }
}

window.GC.Control = Control;