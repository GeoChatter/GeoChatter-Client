import { GeoChatter } from "./geochatter";
import { CustomEvents } from "./events";

// Save the date
console.info("GeoChatter Main", performance.now());
GeoChatter.Main.StartedRun = new Date();

// Connection checks
GeoChatter.StartConnectionChecker();

// Add event handlers
CustomEvents.AddInitialEventHandlers();

if (typeof google !== "undefined" && typeof google.maps !== "undefined" && typeof google.maps.Map !== "undefined")
{
    // This happens when refreshed on a game page
    GeoChatter.Initialize();
}

// Begin mutation observer
GeoChatter.BeginObserver()
    .then(() => console.log("Main observed successfully."));
