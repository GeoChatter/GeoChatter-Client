import "./core";
import { Constant } from "./constants";

export namespace Dependency
{
    export var Head: HTMLHeadElement;

    export var Body: HTMLElement;

    export var Finalizer: Callback;

    export var Scheme: Nullable<Scheme>;

    function FinalizerCallback(_e: Nullable<Event>): void
    {
        LastIncludedIndex = 0;
        if (Finalizer) Finalizer();
        else console.warn("Finalizer wasn't set for callback on post injection")
    }

    var LastIncludedIndex = 0;

    var Includes: Array<Callback<Nullable<Event>>> = [
        (_e: Nullable<Event>) => IncludeScript("https://cdnjs.cloudflare.com/ajax/libs/babel-polyfill/7.4.0/polyfill.min.js", "sha384-TSD1J+e59Px9jLOsXOCC3tMW3UQIPZaamPyLolootG/N4IIzH/xBmKT+BW/hNrw5", null, Includes[++LastIncludedIndex]),
        (_e: Nullable<Event>) => IncludeStyle("https://cdn3.devexpress.com/jslib/22.1.3/css/dx.dark.css", "sha384-aVrlvGbmhcGCXxAhwuWWd5/UToUqB/h7IrOvPKLD/W/dx4RAiPOZ+B0JEXEr+uwA", null, Includes[++LastIncludedIndex]),
        (_e: Nullable<Event>) => IncludeStyle("https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.min.css", "sha384-J3tLcWkdGTGEaRTYfKrKVaK5EGVBuxR9rg5ZzQFWRuQD+0hZABemSLVXimw8Nrb9", null, Includes[++LastIncludedIndex]),
        (_e: Nullable<Event>) => IncludeStyle("https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/1.1.1/css/bootstrap-multiselect.min.css", "sha384-EzNvbrCmODYdwmi/UWiRDLKmfJAL5loYlm3Bf+zRAmUAc7UBRI2VRdvP/O9A8z9w", null, Includes[++LastIncludedIndex]),
        (_e: Nullable<Event>) => IncludeScript("https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/1.1.1/js/bootstrap-multiselect.min.js", "sha512-fp+kGodOXYBIPyIXInWgdH2vTMiOfbLC9YqwEHslkUxc8JLI7eBL2UQ8/HbB5YehvynU3gA3klc84rAQcTQvXA==", null, Includes[++LastIncludedIndex]),
        (_e: Nullable<Event>) => IncludeSchemeStyle("Styles/flag-icon.min.css", Includes[++LastIncludedIndex]),
        (_e: Nullable<Event>) => IncludeSchemeStyle("Styles/flag-custom.min.css", Includes[++LastIncludedIndex]),
        (_e: Nullable<Event>) => IncludeSchemeStyle(Constant.MainCSS, Includes[++LastIncludedIndex]),
        FinalizerCallback
    ]

    /**
     * Add script element with given source url or filepath to document head
     * @param {string} path Url or relative path for the .js file
     * @param {string|null} hash integrity hash
     * @param {string|null} scheme custom scheme handler to use as source = {scheme}://{path}, or null
     * @param {Function} callback onload callback
     */
    function IncludeScript(path: string, hash: Nullable<string>, scheme: Nullable<string>, callback: Nullable<Callback<Event>>)
    {
        console.log("IncludeScript", path, LastIncludedIndex, callback?.toString())
        const dat = document.createElement("script");
        dat.type = "text/javascript";

        if (callback) dat.onload = callback;

        dat.onerror = console.error;
        dat.onabort = console.error;

        if (hash)
        {
            dat.integrity = hash;
            dat.crossOrigin = "anonymous"
        }

        if (scheme)
        {
            console.log("scheme:script", path)
            dat.src = scheme + "://" + path;
        } else
        {
            console.log("script", path)
            dat.src = path;
        }

        if (!Head) console.error("Head is not set for injection");
        else Head.appendChild(dat);
    }

    function IncludeStyle(path: string, hash: Nullable<string>, scheme: Nullable<string>, callback: Nullable<Callback<Event>>)
    {
        console.log("IncludeStyle", path, LastIncludedIndex, callback?.toString())
        const styles = document.createElement("link");
        styles.rel = "stylesheet";
        styles.type = "text/css";

        if (callback) styles.onload = callback;

        styles.onerror = console.error;
        styles.onabort = console.error;

        if (hash)
        {
            styles.integrity = hash;
            styles.crossOrigin = "anonymous"
        }

        if (scheme)
        {
            console.log("scheme:style", path)
            styles.href = scheme + "://" + path;
        } else
        {
            console.log("style", path)
            styles.href = path;
        }

        if (!Head) console.error("Head is not set for injection");
        else Head.appendChild(styles);
    }

    export function IncludeSchemeStyle(path: string, callback: Nullable<Callback<Event>>)
    {
        IncludeStyle(path, null, Scheme?.Name, callback)
    }

    export function InjectAll()
    {
        LastIncludedIndex = 0;
        let first = Includes[LastIncludedIndex]
        if (first) first(null);
    }

    export async function SetupScheme()
    {
        console.log("Setting up scheme handler");
        await CefSharp.BindObjectAsync('jsHelper');

        let s: string = await jsHelper.getSchemeSettings();

        Scheme = JSON.parse(s) as Scheme;
        console.log("Scheme handler set");
    }
}

window.GC.Dependency = Dependency;