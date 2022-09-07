/// <reference path="./interfaces.d.ts" />
/// <reference path="./geoguessr.d.ts" />
/// <reference path="./cef.d.ts" />
/// <reference path="../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../node_modules/@types/jqueryui/index.d.ts" />
/// <reference path="../node_modules/@types/bootstrap-multiselect/index.d.ts" />

// DEPENDENCIES FOR OUTPUT
import "../lib/jqueryImport"
import "../node_modules/devextreme/dist/js/dx.all"
import "../node_modules/jquery-ui/dist/jquery-ui.min.js"

import "file-saver"
import * as Excel from "exceljs"
import * as MarkerClustering from "@googlemaps/markerclusterer";
//

declare global
{
    export function $$(selector: string, context?: Document): Array<JQuery<HTMLElement>>;

    interface Window
    {
        GC: IGeoChatterGlobal
    }
}

window.GC = {} as IGeoChatterGlobal

window.$$ = function(selector: string, context?: Document): Array<JQuery<HTMLElement>>
{
    context = context || document;
    var elements = context.querySelectorAll(selector);
    return Array.prototype.slice.call(elements);
}

export
{
    Excel,
    MarkerClustering
}
