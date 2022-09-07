import "./core";
import { Constant } from "./constants";
import { Control } from "./controls";
import { GeoChatter } from "./geochatter";
import { MapUtil } from "./maps";

export namespace Div
{
    // Streetview panaroma canvas div or null
    export function GetStreetViewDivTarget() : Nullable<Element>
    {
        let div = document.getElementsByClassName(Constant._class_streetview_div);
        if (div.length == 0) return null
        else return div[0]
    }

    // 5 round game round summary div or null
    export function Get5RoundGameSummaryDiv(): Nullable<Element>
    {
        let div = document.getElementsByClassName(Constant._class_roundResult_5roundGame);
        if (div.length == 0) return null
        else return div[0]
    }

    export function Set5RoundGameSummaryDivPlaceHolder()
    {
        let cCheck = document.createElement("div")
        document.body.appendChild(cCheck);
        cCheck.id = Constant._id_empty_div_check;
    }

    // Game summary div or null
    export function Get5RoundGameSummaryEndDiv(): Nullable<Element>
    {
        let div = document.getElementsByClassName(Constant._class_endResult_5roundGame);
        if (div.length == 0) return null
        else return div[0]
    }

    export function Set5RoundGameSummaryDivPlaceHolderEnd()
    {
        let cCheck = document.createElement("div")
        document.body.appendChild(cCheck);
        cCheck.id = Constant._id_empty_div_check;
    }

    // Streak game round summary div or null
    export function GetStreakGameSummaryDiv(): Nullable<Element>
    {
        let div = document.getElementsByClassName(Constant._class_roundResult_streakGame);
        if (div.length == 0) return null
        else return div[0]
    }

    export function SetStreakGameSummaryDivPlaceHolder()
    {
        let cCheck = document.createElement("div")
        document.body.appendChild(cCheck);
        cCheck.id = Constant._id_empty_div_check;
    }

    /////// 

    export function Get_GuessButton(): Nullable<HTMLInputElement>
    {
        return document.querySelectorAll("[data-qa='perform-guess']")[0] as HTMLInputElement;
    }

    export function Get_ViewSummaryButton(): Nullable<HTMLInputElement>
    {
        return document.querySelectorAll("[data-qa='close-round-result']")[0] as HTMLInputElement;
    }

    export function Get_EndRoundButton(): Nullable<HTMLInputElement>
    {
        return document.querySelectorAll("[data-qa='close-round-result']")[0] as HTMLInputElement;
    }

    export function GetTableRow(i: number): Nullable<JQuery<HTMLElement>>
    {
        let el = $("#" + Constant._id_datatable + " .dx-datagrid-content table tr").filter("[aria-rowindex]")[i];
        return el ? $(el) : null;
    }

    /**
     * Initialize scoreboard container
     * @param {GAMESTAGE} stage game stage
     */
    export function InitializeScoreboardViewport(stage: Nullable<GAMESTAGE> = null): void
    {
        try
        {
            if (MapUtil.StreamerStreaksMarker)
            {
                MapUtil.StreamerStreaksMarker.setMap(null);
                MapUtil.StreamerStreaksMarker = null;
            }

            while (MapUtil.Markers.correctLocations && MapUtil.Markers.correctLocations[0]) MapUtil.Markers.correctLocations.pop()?.setMap(null);

            console.log("initAll", stage);
            $("#scoreboardContainer").remove();

            GeoChatter.Main.ScoreboardParentContainer = document.createElement("div");
            GeoChatter.Main.ScoreboardParentContainer.setAttribute("id", "scoreboardContainer");

            GeoChatter.Main.TableRows = [];

            GeoChatter.Main.ScoreboardParentContainer.innerHTML = `
<div class="dx-viewport" >
    <div id='resizeable_container'>
        <div id='${Constant._id_datatable}' >
        </div>
    </div>
</div>`;

            document.body.appendChild(GeoChatter.Main.ScoreboardParentContainer);
            console.log("Appended scoreboard div")
            Control.AddRandomGuessButton();
        }
        catch (e)
        {
            console.error(e)
        }
    }
}

window.GC.Div = Div;