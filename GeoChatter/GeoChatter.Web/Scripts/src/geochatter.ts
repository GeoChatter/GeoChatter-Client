import "./core";
import { EventHandler, CustomEvents } from "./events";
import { App } from "./models";
import { Util } from "./utils";
import { Visual } from "./visuals";
import { State } from "./settings";
import { Dependency } from "./dependencies";
import { Control } from "./controls";
import { Enum } from "./enums";
export namespace GeoChatter
{
    export const Main: App = new App({} as GameTableOptions);

    export type ExtendedMap = Nullable<google.maps.Map & { OverrideSetters(): void }>;

    export var Map: ExtendedMap;
    export function SetMap(map: ExtendedMap)
    {
        Map = map;
        console.log("GoogleMaps instance created");
        Map?.OverrideSetters();

        State.Event.AddedStreakMarkerListener = Map ? !0 : !1;
        Map?.addListener("click", EventHandler.streaksMarkerHandler);
    }

    export var Streetview: Nullable<google.maps.StreetViewPanorama>;
    export function SetStreetview(sw: Nullable<google.maps.StreetViewPanorama>)
    {
        console.log("Streetview instance created");
        Streetview = sw;
        Streetview?.addListener("position_changed", EventHandler.OnStreetviewPositionChange);
    }

    export function BeginObserver(): Promise<void>
    {
        Dependency.Finalizer = FinalizeInit;

        return new Promise<void>((resolve, _) =>
        {
            console.log("Observer begun")
            let scriptObserver: Nullable<MutationObserver> = new MutationObserver(mutations =>
            {
                mutations.forEach(m =>
                {
                    m.addedNodes.forEach(n =>
                    {
                        if (n.nodeName === "SCRIPT" &&
                            (n as HTMLScriptElement).src.startsWith("https://maps.googleapis.com/maps/api/js?"))
                        {
                            (n as HTMLScriptElement).onload = () =>
                            {
                                scriptObserver?.disconnect();
                                scriptObserver = undefined;
                                resolve();
                            };
                        }
                    });
                });
            });

            let bodyDone = false;
            let headDone = false;

            new MutationObserver((_, observer) =>
            {
                if (!bodyDone && document.body)
                {
                    if (scriptObserver)
                    {
                        scriptObserver.observe(document.body, {
                            childList: true,
                        });
                        console.log("Observer body completed")
                        bodyDone = true;
                    }
                }
                if (!headDone && document.head)
                {
                    if (scriptObserver)
                    {
                        scriptObserver.observe(document.head, {
                            childList: true,
                        });
                        console.log("Observer head completed")
                        headDone = true;
                    }
                }
                if (headDone && bodyDone)
                {
                    console.log("Observer disconnecting")
                    observer.disconnect();

                    (async () =>
                    {
                        console.log("Observer executing extensions")
                        await CefSharp.BindObjectAsync('jsHelper');
                        jsHelper.executeUserScripts();
                    })();

                    EndObserver();
                }
            }).observe(document, {
                childList: true,
                subtree: true,
            });
            Control.AddCopyMapLinkButton();
            Control.AddCopyResultLinkButton();
        })
            .then(Initialize);
    }

    export function GetGeoGuessrAvatarPath(): string
    {
        return __NEXT_DATA__.props.middlewareResults[0]?.account?.user.pin.url ?? "pin/652e4ee3fb0222d38ca6a151ef409766.png";
    }

    export function GeoGuessrAvatar(size: string | number, name: string): string
    {
        return `https://www.geoguessr.com/images/auto/${size}/${size}/ce/0/plain/${name}`
    }

    var connectionCheckerIntervalID: number = -1;
    const interval = 100;
    async function connectionChecker()
    {
        await CefSharp.BindObjectAsync("jsHelper");
        let res = await jsHelper.connectionState();

        switch (res)
        {
            case Enum.ConnectionState.CONNECTED:
                {
                    Visual.RemoveLoadingScreen();
                    break;
                }
            case Enum.ConnectionState.RECONNECTING:
                {
                    Visual.AddLoadingScreen("Trying to re-establish connection to the GeoChatter servers...");
                    break;
                }
            case Enum.ConnectionState.CONNECTING:
                {
                    Visual.AddLoadingScreen("Establishing connection to the GeoChatter servers...");
                    break;
                }
            case Enum.ConnectionState.DISCONNECTED:
                {
                    Visual.AddLoadingScreen("Lost connection to the GeoChatter servers! Please wait a few moments or try relaunching the application. Check the GeoChatter discord server for planned outages and updates!");
                    break;
                }
            case Enum.ConnectionState.UNKNOWN:
                {
                    Visual.AddLoadingScreen("Failed to connect to the GeoChatter servers! Please try relaunching the application. Check the GeoChatter discord server for planned outages and updates!");
                    break;
                }
        }
    }

    export function StartConnectionChecker()
    {

        if (connectionCheckerIntervalID > -1)
        {
            clearInterval(connectionCheckerIntervalID);
            connectionCheckerIntervalID = -1;
        }

        connectionCheckerIntervalID = window.setInterval(connectionChecker, interval)
    }

    export var IsInitialized: boolean = false;
    export var IsFullyInitialized: boolean = false;

    export async function Initialize()
    {
        try
        {
            if (IsInitialized) return;

            IsInitialized = true;
            console.log("Initialize GeoChatter google maps")

            google.maps.Map = class extends google.maps.Map
            {
                constructor(mapDiv: HTMLElement, opts?: google.maps.MapOptions)
                {
                    super(mapDiv, opts);

                    try
                    {
                        SetMap(this);
                        IsFullyInitialized = true;
                    }
                    catch (error)
                    {
                        console.error("Failed to find GoogleMaps instance", error);
                    }
                }

                old_setStreetView(_options: google.maps.StreetViewPanorama | null) { }
                old_setOptions(_options: google.maps.MapOptions | null) { }

                get _customOptions() {
                    return {
                        mapTypeControl: true,
                        mapTypeControlOptions: {
                            style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR
                        }
                    }
                }

                _setOptions(options: google.maps.MapOptions | null)
                {
                    if (!options) options = {};

                    options =
                    {
                        ...options,
                        ...(this._customOptions)
                    }
                    console.debug("Map options from setter", options);
                    this.old_setOptions(options);
                }
                _setStreetView(sw: google.maps.StreetViewPanorama | null)
                {
                    console.debug("StreetView from setter", sw);
                    SetStreetview(sw);
                    this.old_setStreetView(sw);
                }

                OverrideSetters()
                {
                    this.old_setOptions = this.setOptions;
                    this.setOptions = this._setOptions;

                    this.old_setStreetView = this.setStreetView;
                    this.setStreetView = this._setStreetView;
                }
            };

            google.maps.StreetViewPanorama = class extends google.maps.StreetViewPanorama
            {
                constructor(container: HTMLElement,
                    opts?: google.maps.StreetViewPanoramaOptions | null)
                {
                    super(container, opts)
                    SetStreetview(this);
                }
            }
        }
        catch (e)
        {
            IsInitialized = false;
            console.error("Failed to initialize GeoChatter", e)
        }
    }

    function FinalizeInit()
    {
        State.App.LoadCompleted = true;

        window.setTimeout(async () =>
        {
            await CefSharp.BindObjectAsync('jsHelper');
            jsHelper.reportMainJSCompleted();
        }, 500)

        window.setInterval(Visual.SetDisplayOptions, 100); // TODO: Replace with css properties
        window.setInterval(Visual.CheckMissingScoreboard, 100);

        console.log("Ended observer")
    }

    async function EndObserver(): Promise<void>
    {
        console.log("Finalizing observer")
        CustomEvents.PostGuessButtonTickerBackupInterval = window.setInterval(() =>
        {
            Util.RepeatCallbackAttempt(EventHandler.CheckSelector, 2, 200, true);
            Util.RepeatCallbackAttempt(EventHandler.TryAddCCheck, 2, 200);
        }, 200);

        Dependency.Head = document.head;
        Dependency.Body = document.body;

        await EventHandler.ApplySettings()
        await Dependency.SetupScheme();

        Dependency.InjectAll();
    } 
}

window.GC.GeoChatter = GeoChatter;