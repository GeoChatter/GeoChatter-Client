import { Control } from "./controls";
import { Div } from "./divs";
import { GeoChatter } from "./geochatter";
import { MarkerClustererExtended, Guess } from "./models";
import { Util } from "./utils";
import { Enum } from "./enums"
import { Color } from "./colors";
import { Constant } from "./constants";
import { MarkerClustering } from "./core";
import { Setting, State } from "./settings";
import { Dependency } from "./dependencies";

export namespace MapUtil
{
    export var CurrentClusterer: Nullable<MarkerClustererExtended>;

    export var StreamerStreaksMarker: Nullable<IMarkerWithSource>;

    export const NamedMarkers: MarkerLineNamedStore<IMarkerWithSource, google.maps.Polyline> =
    {
        rowClickResults: {
            Markers: {},
            Lines: {}
        }
    }

    export const Markers: MarkerLineArrayStore<IMarkerWithSource | google.maps.Marker> =
    {
        correctLocations: [],
        currentGame: [],
    };

    export const CurrentMarkerClusters: MarkerClusterStore = { };

    export const Lines: MarkerLineArrayStore<google.maps.Polyline> =
    {
        correctLocations: [],
        currentGame: [],
    };

    /**
    * Clear a cluster of given markers property of the scope
    * @param {any} scope Object to get properties from
    * @param {any} markersProp Property name of markers array in scope
    */
    export function ClearCluster(key: MarkerLineAccessKey): void
    {
        if (CurrentMarkerClusters[key])
        {
            CurrentMarkerClusters[key]?.clearMarkers();
            delete CurrentMarkerClusters[key];
        }
    }

    /**
     * Create a cluster of given markers property of the scope
     */
    export function MakeCluster(key: MarkerLineAccessKey, ignoreFirst: boolean = false): void
    {
        ClearCluster(key);

        if (!Markers[key]) return;

        const markerIcon = {
            path: google.maps.SymbolPath.CIRCLE,
            fillColor: "#ff0000",
            fillOpacity: 0.82,
            scale: 19,
            strokeColor: "#FFFFFF",
            strokeWeight: 1.4,
        };

        const markers = ignoreFirst
            ? Markers[key]?.slice(1)
            : Markers[key];

        const map = GeoChatter.Map;

        if (!map || !markers) return;

        const renderer: MarkerClustering.Renderer = {
            render: ({
                count,
                position
            }) =>
            {
                let m = new google.maps.Marker({
                    label: {
                        text: String(count),
                        color: "white",
                        fontSize: "1.5em",
                        fontWeight: "bolder"
                    },
                    position,
                    zIndex: Number(google.maps.Marker.MAX_ZINDEX) + count,
                    icon: markerIcon
                });
                m.setVisible(count > 0);

                let infowindow = new google.maps.InfoWindow({
                    disableAutoPan: true
                });

                google.maps.event.addListener(m, "mouseover", () =>
                {
                    if (!CurrentMarkerClusters[key]) return;

                    let names = [];
                    let markers = GetSortedGuessesFromMarkerCluster(CurrentMarkerClusters[key] as MarkerClustererExtended, position);
                    if (!markers) return;

                    for (var i = 0; i < markers.length; i++)
                    {
                        let mg = markers[i]?.guess as Guess;
                        if (mg) names.push(`<div class="clusterInfoWindowRow">#${mg.ResultFinalOrder} ${mg.GetPlayerNameDisplayHTML()}</div>`);
                    }
                    let html = Util.GetInfoWindowHtmlForCluster(names);
                    infowindow.setContent(html);
                    infowindow.open(GeoChatter.Map, m);
                    $(infowindow).prev('div').remove();
                });
                google.maps.event.addListener(m, "mouseout", () => infowindow.close());

                return m;
            }
        };
        CurrentMarkerClusters[key] = new MarkerClustering.MarkerClusterer({
            map,
            markers,
            renderer
        });
    }

    export function GetSortedGuessesFromMarkerCluster(cluster: MarkerClustererExtended, position: google.maps.LatLng)
    {
        let pos = position;
        return cluster?.clusters.find(x => x.position.equals(pos))?.markers?.sort((a, b) => (a.guess as Guess).ResultFinalOrder < (b.guess as Guess).ResultFinalOrder ? -1 : 1)
    }

    /**
     * Create a cluster of given markers
     * @param {Array} lis markers
     */
    export function MakeClusterFromList(lis: Array<IMarkerWithSource>)
    {

        const markerIcon = {
            path: google.maps.SymbolPath.CIRCLE,
            fillColor: "#ff0000",
            fillOpacity: 0.82,
            scale: 19,
            strokeColor: "#FFFFFF",
            strokeWeight: 1.4,
        };

        const markers = lis;
        const map = GeoChatter.Map;

        if (!map) return;

        const renderer: MarkerClustering.Renderer = {
            render: ({
                count,
                position
            }) =>
            {
                let m = new google.maps.Marker({
                    label: {
                        text: String(count),
                        color: "white",
                        fontSize: "1.5em",
                        fontWeight: "bolder"
                    },
                    position,
                    zIndex: Number(google.maps.Marker.MAX_ZINDEX) + count,
                    icon: markerIcon
                });

                m.setVisible(count > 0);

                let infowindow = new google.maps.InfoWindow({
                    disableAutoPan: true
                });

                google.maps.event.addListener(m, "mouseover", () =>
                {
                    if (!CurrentClusterer) return;

                    let names = [];
                    let markers = GetSortedGuessesFromMarkerCluster(CurrentClusterer as MarkerClustererExtended, position);
                    if (!markers) return;

                    for (var i = 0; i < markers.length; i++)
                    {
                        let mg = markers[i]?.guess;
                        if (mg) names.push(mg.GetPlayerNameDisplayHTML());
                    }
                    let html = Util.GetInfoWindowHtmlForCluster(names);
                    infowindow.setContent(html);
                    infowindow.open(GeoChatter.Map, m);
                    $(infowindow).prev('div').remove();
                });
                google.maps.event.addListener(m, "mouseout", () => infowindow.close());

                return m;
            }
        };

        CurrentClusterer = new MarkerClustererExtended({
            map,
            markers,
            renderer
        });

        return CurrentClusterer
    }

    export function AddCorrectLocationMarkers()
    {
        if (!GeoChatter.Main.CurrentGame) return;

        let rounds = GeoChatter.Main.CurrentGame.AllRounds

        const infowindow = new google.maps.InfoWindow({
            disableAutoPan: true
        });

        for (let i = 0; i < rounds.length; i++)
        {
            let round = rounds[i];
            if (!round) continue

            let correct = {
                lat: round?.Location.Latitude ?? 0,
                lng: round?.Location.Longitude ?? 0
            };
            let locationMarker = CreateMarker(correct, null, (i + 1).toString(), true);
            if (!locationMarker) return;

            let heading = round.Location.Heading;
            let pitch = round.Location.Pitch;
            let fov = round.Location.FOV;
            let pano = round.Location.Pano;

            google.maps.event.addListener(locationMarker, "click", () =>
            {
                if (!round) return;
                Control.EmbedLocation(`round-${round.PositionInChain}-correctLocation`, round.GetEmbedTitleHTML(), correct, heading, pitch, fov, pano, round)
            });

            AddMarkerInfoWindowEvents(infowindow, locationMarker, round);

            Markers.correctLocations?.push(locationMarker)
        }
    }

    /**
     * Increases everytime a new marker instance is created 
     */
    export var MarkerCounter = 1

    export const CorrectLocationMarker = {
        strokeColor: "#000000",
        strokeWeight: 2,
        scale: 0.125,
        fillOpacity: 0.8,
        path: `M 193.2 53.469 C 192.21 53.54 188.25 53.868 184.4 54.199 C 180.55 54.53 175.15 54.975 172.4 55.187 C 169.375 55.422 166.531 55.884 165.2 56.358 C 163.99 56.789 161.999 57.505 160.775 57.95 C 159.552 58.395 156.942 58.958 154.975 59.201 C 152.147 59.55 150.856 59.941 148.8 61.07 C 147.327 61.879 144.472 62.925 142.215 63.483 C 139.344 64.192 137.795 64.812 136.676 65.698 C 135.812 66.382 133.949 67.254 132.482 67.661 C 131.03 68.064 129.202 68.907 128.421 69.535 C 126.647 70.96 124.981 71.822 123.299 72.184 C 122.583 72.338 121.235 73.154 120.303 73.998 C 119.371 74.842 117.616 75.929 116.404 76.414 C 115.192 76.899 113.84 77.707 113.4 78.209 C 112.96 78.712 111.626 79.693 110.436 80.389 C 109.246 81.085 107.928 82.092 107.507 82.628 C 107.086 83.163 105.9 84.079 104.871 84.664 C 103.842 85.248 102.28 86.384 101.4 87.188 C 100.52 87.992 99.17 89.116 98.4 89.685 C 97.165 90.598 90.26 97.028 88.256 99.132 C 86.065 101.432 84.378 103.358 83.38 104.698 C 82.766 105.524 81.662 106.816 80.929 107.569 C 80.195 108.322 79.123 109.762 78.546 110.769 C 77.969 111.776 76.913 113.14 76.201 113.8 C 75.488 114.46 74.538 115.72 74.091 116.6 C 73.644 117.48 72.793 118.65 72.201 119.2 C 71.609 119.75 70.592 121.37 69.942 122.8 C 69.293 124.23 68.237 125.941 67.597 126.601 C 66.958 127.262 66.157 128.589 65.818 129.55 C 65.479 130.511 64.493 132.358 63.627 133.654 C 62.762 134.95 61.776 136.953 61.436 138.105 C 61.096 139.257 60.094 141.28 59.21 142.6 C 58.326 143.92 57.602 145.277 57.601 145.616 C 57.598 147.048 56.292 151.105 55.418 152.4 C 54.255 154.121 53.412 156.772 52.644 161.119 C 52.322 162.944 51.507 165.614 50.834 167.053 C 49.606 169.676 49.525 170.129 48.388 180.6 C 48.15 182.8 47.56 186.94 47.077 189.8 C 45.865 196.988 45.902 210.519 47.152 216.677 C 47.607 218.92 48.253 223.42 48.587 226.677 C 49.4 234.614 49.565 235.542 50.487 237.361 C 51.779 239.909 52.356 241.858 52.997 245.841 C 53.454 248.686 53.997 250.369 55.211 252.705 C 56.234 254.672 57.035 256.946 57.412 258.945 C 57.856 261.302 58.317 262.485 59.256 263.671 C 59.999 264.609 60.877 266.52 61.415 268.369 C 62.42 271.827 63.232 273.597 64.172 274.377 C 64.516 274.662 65.163 276.089 65.61 277.548 C 66.057 279.007 67.07 281.146 67.862 282.303 C 68.654 283.459 69.737 285.605 70.269 287.071 C 70.801 288.536 71.748 290.318 72.374 291.031 C 72.999 291.744 73.807 293.218 74.169 294.307 C 74.531 295.396 75.471 297.04 76.258 297.959 C 77.045 298.878 78.042 300.569 78.474 301.715 C 78.906 302.862 79.78 304.342 80.415 305.004 C 81.051 305.666 82.225 307.551 83.024 309.193 C 83.823 310.835 84.797 312.403 85.188 312.677 C 85.58 312.951 86.304 314.036 86.797 315.088 C 87.29 316.14 88.296 317.669 89.033 318.486 C 89.769 319.303 90.847 320.923 91.428 322.086 C 92.009 323.249 92.976 324.74 93.577 325.4 C 94.179 326.06 95.153 327.447 95.741 328.482 C 96.33 329.516 97.144 330.596 97.549 330.882 C 97.955 331.167 98.892 332.39 99.631 333.6 C 100.37 334.81 101.745 336.752 102.687 337.916 C 103.629 339.079 104.4 340.202 104.4 340.411 C 104.4 340.62 105.3 341.785 106.4 343 C 107.5 344.215 108.4 345.378 108.4 345.586 C 108.4 345.793 109.165 346.758 110.1 347.731 C 111.035 348.703 112.61 350.617 113.6 351.984 C 117.868 357.878 118.46 358.641 119.355 359.4 C 119.874 359.84 120.771 360.96 121.348 361.89 C 121.925 362.819 122.857 363.881 123.419 364.249 C 123.981 364.618 124.701 365.421 125.018 366.035 C 125.335 366.649 126.451 368.044 127.498 369.136 C 129.622 371.351 131.702 373.693 136.264 379 C 142.206 385.915 154.193 398.904 157.304 401.8 C 157.777 402.24 158.532 403.034 158.982 403.565 C 159.432 404.096 160.318 404.996 160.952 405.565 C 161.586 406.134 162.576 407.111 163.152 407.736 C 164.228 408.902 164.389 409.058 167.795 412.228 C 168.892 413.249 170.836 414.612 172.115 415.258 C 173.394 415.904 175.003 416.969 175.69 417.626 C 176.377 418.283 177.403 418.922 177.97 419.047 C 181.047 419.725 184.598 420.838 185.433 421.385 C 189.589 424.108 204.007 424.095 209.827 421.363 C 210.941 420.84 213.228 420.068 214.907 419.648 C 217.062 419.11 218.468 418.474 219.681 417.492 C 220.626 416.726 222.415 415.607 223.655 415.006 C 224.896 414.405 226.122 413.519 226.381 413.036 C 226.639 412.553 227.574 411.717 228.459 411.179 C 229.343 410.64 230.321 409.713 230.634 409.119 C 230.946 408.524 231.999 407.489 232.974 406.818 C 233.949 406.147 234.858 405.246 234.995 404.816 C 235.132 404.385 235.999 403.442 236.922 402.719 C 237.845 401.997 238.673 401.225 238.762 401.003 C 238.918 400.615 243.07 396.246 244.847 394.6 C 245.322 394.16 246.091 393.383 246.555 392.874 C 247.02 392.365 248.313 391.015 249.43 389.874 C 250.546 388.733 251.757 387.318 252.12 386.728 C 252.484 386.138 253.326 385.334 253.991 384.941 C 254.657 384.548 255.299 383.917 255.419 383.539 C 255.612 382.933 257.02 381.339 261.655 376.482 C 262.347 375.758 263.453 374.408 264.114 373.482 C 264.775 372.557 266.145 370.871 267.158 369.736 C 268.171 368.601 269.99 366.474 271.2 365.01 C 272.41 363.546 274.229 361.501 275.242 360.466 C 276.255 359.431 277.363 358.047 277.703 357.392 C 278.044 356.736 278.878 355.791 279.558 355.29 C 280.237 354.79 281.157 353.668 281.602 352.796 C 282.047 351.925 282.868 350.912 283.428 350.545 C 283.988 350.178 284.781 349.221 285.191 348.418 C 285.6 347.615 286.734 346.068 287.71 344.979 C 288.685 343.891 289.784 342.415 290.152 341.701 C 290.519 340.986 291.42 339.855 292.154 339.187 C 292.888 338.52 293.9 337.173 294.403 336.194 C 294.906 335.215 295.74 334.138 296.256 333.8 C 296.772 333.462 297.634 332.316 298.173 331.254 C 298.711 330.191 299.973 328.36 300.976 327.185 C 301.979 326.009 302.8 324.804 302.8 324.507 C 302.8 324.209 303.59 323.118 304.556 322.083 C 305.522 321.047 306.603 319.501 306.958 318.646 C 307.313 317.792 308.253 316.374 309.047 315.495 C 309.841 314.617 310.847 313.091 311.283 312.106 C 311.719 311.12 312.619 309.748 313.283 309.057 C 313.947 308.366 314.913 306.849 315.429 305.687 C 315.945 304.525 316.989 302.815 317.748 301.887 C 318.508 300.959 319.447 299.39 319.836 298.4 C 320.225 297.41 321.14 295.894 321.869 295.032 C 322.597 294.169 323.561 292.483 324.009 291.284 C 324.457 290.086 325.421 288.361 326.151 287.452 C 326.881 286.544 327.792 284.81 328.174 283.6 C 328.557 282.39 329.575 280.462 330.436 279.316 C 331.397 278.038 332.169 276.451 332.435 275.21 C 332.674 274.094 333.59 272.108 334.479 270.777 C 335.463 269.303 336.371 267.268 336.816 265.536 C 337.217 263.98 338.185 261.668 338.968 260.4 C 339.829 259.005 340.62 257.004 340.971 255.337 C 341.643 252.143 342.016 251.089 343.645 247.774 C 344.419 246.198 345.038 244.027 345.411 241.574 C 346.057 237.319 346.296 236.443 347.783 232.862 C 348.568 230.972 348.937 229.135 349.229 225.662 C 349.445 223.098 349.806 219.47 350.031 217.6 C 350.705 211.99 350.91 200.555 350.435 195 C 350.191 192.14 349.812 187.19 349.594 184 C 349.184 178.01 348.749 175.789 347.046 171 C 346.499 169.46 345.762 166.4 345.409 164.2 C 344.959 161.396 344.301 159.184 343.208 156.8 C 342.351 154.93 341.363 152.14 341.013 150.6 C 340.658 149.041 339.768 146.81 339.004 145.566 C 338.249 144.337 337.197 141.961 336.665 140.287 C 336.066 138.4 335.182 136.639 334.342 135.657 C 333.596 134.786 332.753 133.204 332.469 132.143 C 332.185 131.081 331.265 129.31 330.425 128.207 C 329.584 127.103 328.539 125.39 328.103 124.4 C 327.666 123.41 326.815 122.15 326.211 121.6 C 325.608 121.05 324.674 119.61 324.137 118.4 C 323.6 117.19 322.469 115.48 321.624 114.6 C 320.779 113.72 319.794 112.417 319.435 111.704 C 319.077 110.991 318.328 110.109 317.771 109.744 C 317.215 109.38 316.515 108.61 316.217 108.033 C 315.03 105.737 305.771 96.008 300.387 91.4 C 298.588 89.86 296.438 87.942 295.61 87.138 C 294.782 86.334 293.332 85.188 292.388 84.592 C 291.444 83.997 290.229 82.983 289.688 82.34 C 289.147 81.697 288.075 80.908 287.306 80.587 C 286.538 80.265 285.267 79.439 284.482 78.75 C 283.698 78.061 282.01 77.003 280.731 76.399 C 279.453 75.794 277.685 74.647 276.802 73.85 C 275.92 73.052 274.883 72.396 274.499 72.391 C 273.443 72.378 270.751 71.035 269.4 69.848 C 268.738 69.267 266.764 68.352 265 67.81 C 263.167 67.247 261.19 66.322 260.373 65.644 C 259.333 64.783 257.833 64.187 254.849 63.45 C 252.357 62.835 249.949 61.945 248.7 61.177 C 247.071 60.174 245.709 59.763 242.088 59.182 C 239.246 58.726 236.223 57.926 234.064 57.058 C 231.963 56.214 229.577 55.576 228 55.436 C 225.622 55.225 219.035 54.69 205.492 53.608 C 200.636 53.22 197.21 53.181 193.2 53.469 M 210.585 143.591 C 213.502 143.898 214.783 144.269 217.461 145.584 C 219.271 146.473 221.091 147.2 221.504 147.2 C 223.015 147.2 226.289 148.415 227.034 149.252 C 227.455 149.726 229.24 150.784 231 151.604 C 232.76 152.424 234.65 153.586 235.2 154.186 C 235.75 154.787 236.845 155.608 237.632 156.013 C 238.42 156.417 239.548 157.38 240.138 158.154 C 240.728 158.928 241.724 159.865 242.351 160.235 C 242.979 160.605 243.817 161.469 244.214 162.154 C 244.611 162.839 245.632 164.004 246.482 164.743 C 247.332 165.482 248.362 166.742 248.771 167.543 C 249.181 168.344 250.136 169.658 250.895 170.462 C 251.681 171.295 252.503 172.74 252.806 173.821 C 253.098 174.865 253.918 176.479 254.628 177.409 C 256.441 179.784 256.657 180.268 257.218 183.2 C 257.492 184.63 258.305 187.06 259.024 188.6 C 262.067 195.118 262.176 210.158 259.223 216.2 C 258.577 217.52 257.68 220.295 257.228 222.367 C 256.623 225.14 255.998 226.777 254.86 228.567 C 253.989 229.938 253.107 231.981 252.84 233.248 C 252.512 234.802 251.926 236.01 250.94 237.162 C 250.078 238.168 249.216 239.821 248.761 241.337 C 248.314 242.829 247.445 244.505 246.618 245.47 C 245.854 246.363 244.844 248.123 244.373 249.38 C 243.903 250.637 243.02 252.236 242.412 252.933 C 241.803 253.63 240.813 255.19 240.211 256.4 C 239.61 257.61 238.527 259.23 237.805 260 C 237.084 260.77 236.273 262.09 236.004 262.934 C 235.735 263.778 234.717 265.367 233.742 266.465 C 232.767 267.563 231.691 269.157 231.35 270.007 C 231.01 270.857 230.298 271.837 229.768 272.184 C 229.237 272.532 228.28 273.848 227.639 275.108 C 226.999 276.369 225.902 277.94 225.2 278.6 C 224.499 279.26 223.503 280.627 222.988 281.638 C 222.472 282.648 221.609 283.764 221.07 284.117 C 220.531 284.47 219.678 285.443 219.174 286.28 C 218.671 287.116 217.525 288.621 216.629 289.623 C 215.733 290.626 214.689 291.976 214.309 292.623 C 213.368 294.224 208.279 300.468 203.9 305.393 C 202.635 306.816 201.6 308.157 201.6 308.374 C 201.6 311.182 195.821 310.827 193.459 307.874 C 192.767 307.008 191.555 305.557 190.767 304.65 C 189.979 303.742 188.657 301.965 187.828 300.7 C 187 299.435 186.157 298.4 185.956 298.4 C 185.755 298.4 184.982 297.455 184.239 296.3 C 183.496 295.145 182.398 293.75 181.8 293.2 C 181.202 292.65 180.128 291.3 179.415 290.2 C 178.701 289.1 177.711 287.84 177.216 287.4 C 176.72 286.96 175.966 285.917 175.54 285.082 C 175.114 284.247 174.176 282.897 173.455 282.082 C 172.735 281.267 171.724 279.88 171.21 279 C 170.696 278.12 169.702 276.77 169.002 276 C 168.301 275.23 167.237 273.738 166.638 272.684 C 166.038 271.63 165.045 270.19 164.43 269.484 C 163.815 268.778 162.934 267.338 162.472 266.285 C 162.01 265.232 161.097 263.836 160.444 263.182 C 159.791 262.529 158.796 260.99 158.234 259.762 C 157.672 258.535 156.732 256.961 156.146 256.265 C 155.559 255.569 154.589 253.92 153.99 252.6 C 153.391 251.28 152.35 249.514 151.675 248.676 C 151.001 247.838 150.076 246.038 149.621 244.676 C 149.165 243.314 148.174 241.422 147.419 240.472 C 146.558 239.389 145.859 237.912 145.546 236.516 C 145.272 235.291 144.357 233.22 143.513 231.914 C 142.456 230.278 141.73 228.471 141.177 226.1 C 140.736 224.208 139.74 221.377 138.965 219.808 C 135.44 212.675 135.707 191.253 139.403 184.632 C 139.928 183.691 140.574 182.039 140.837 180.961 C 141.416 178.593 142.047 177.293 143.803 174.849 C 144.536 173.828 145.266 172.474 145.426 171.84 C 145.585 171.206 146.382 170.038 147.198 169.244 C 148.014 168.45 148.974 167.232 149.331 166.539 C 150.226 164.8 158.415 156.674 160.06 155.892 C 160.797 155.542 161.94 154.68 162.6 153.976 C 163.26 153.272 164.79 152.254 166 151.714 C 167.21 151.173 168.74 150.323 169.4 149.825 C 171.255 148.424 172.836 147.72 174.8 147.419 C 175.79 147.267 178.04 146.44 179.8 145.581 C 184.336 143.366 199.27 142.401 210.585 143.591`,
    }

    export function CreateMarker(pos: google.maps.LatLng | google.maps.LatLngLiteral, guess: Nullable<Guess>, label?: string, useCorrectLocationImg?: boolean, zIndex?: number): Nullable<IMarkerWithSource>
    {
        if (!GeoChatter.Map || !GeoChatter.Main.CurrentGame) return null;

        try
        {
            var size = Setting.Overlay.MarkerSize;

            var color = guess ?
                guess.PlayerData.Color :
                useCorrectLocationImg ?
                    "#CCCCCC" :
                    "#333333";

            var url = !guess || guess.IsStreamerGuess ?
                !useCorrectLocationImg ?
                    GeoChatter.GeoGuessrAvatar(size, GeoChatter.GetGeoGuessrAvatarPath()) :
                    $("[data-qa=correct-location-marker] img").attr("src") :
                guess.PlayerData.MarkerImage;

            var icon;
            var lblopts: google.maps.MarkerLabel | string = "";

            if (!url)
            {
                icon = {
                    ...CorrectLocationMarker,
                    anchor: new google.maps.Point(195, 440),
                    labelOrigin: new google.maps.Point(330, 40),
                    fillColor: color
                };

                if (label)
                {
                    lblopts = {
                        fontWeight: "bolder",
                        fontSize: "20px",
                        className: "ggMarkerLabel",
                        text: `${label}`
                    };
                }
            }
            else
            {
                if (guess)
                {
                    if (url in GeoChatter.Main.ImageCache && !GeoChatter.Main.ImageCache[url]?.startsWith("data:text/html"))
                    {
                        let u = GeoChatter.Main.ImageCache[url];

                        if (u) url = Util.DataUrlToMarkerSVGDataUrl(u, size, guess.PlayerData.Color, Setting.Overlay.MarkerBorderSize);
                    }
                    else if (guess.PlayerData.MarkerData != null && !guess.PlayerData.MarkerData.startsWith("data:text/html"))
                    {
                        GeoChatter.Main.ImageCache[url] = guess.PlayerData.MarkerData;
                        url = Util.DataUrlToMarkerSVGDataUrl(guess.PlayerData.MarkerData, size, guess.PlayerData.Color, Setting.Overlay.MarkerBorderSize);
                    }
                    else
                    {
                        Util.ImageUrlToDataUrl(url, (data) =>
                        {
                            if (url && typeof data === "string") GeoChatter.Main.ImageCache[url] = data;
                        })
                    }
                }

                let isize = size + Setting.Overlay.MarkerBorderSize;
                icon = {
                    url: url,
                    size: new google.maps.Size(isize, isize),
                    scaledSize: new google.maps.Size(isize, isize),
                    origin: new google.maps.Point(0, 0),
                    anchor: new google.maps.Point(isize / 2, isize / 2),
                    labelOrigin: new google.maps.Point(isize - 1, 0)
                };

                if (label)
                {
                    lblopts = {
                        text: `${label}`,
                        className: "ggMarkerLabel"
                    };
                }
            }

            var opts = {
                position: pos,
                icon: icon,
                map: GeoChatter.Map,
                label: lblopts,
                optimized: true,
                clickable: label ? true : false
            } as google.maps.MarkerOptions & { guess?: Guess };
            if (guess) opts.guess = guess;

            if (zIndex != null) opts["zIndex"] = zIndex;

            if (!url) opts["zIndex"] = Number(google.maps.Marker.MAX_ZINDEX) + MarkerCounter++;

            var marker = new google.maps.Marker(opts);

            return marker as IMarkerWithSource;
        }
        catch (e)
        {
            console.error("Failed to create guess marker, creating backup marker", e)
            var opts: google.maps.MarkerOptions & { guess?: Guess } = {
                position: pos,
                icon:
                {
                    ...CorrectLocationMarker,
                    fillColor: Color.RandomColor(),
                    anchor: new google.maps.Point(195, 440),
                    labelOrigin: new google.maps.Point(330, 40),
                },
                map: GeoChatter.Map,
                label: {
                    color: "#000000",
                    fontWeight: "bold",
                    fontSize: "17px",
                    text: `${label}`
                },
                optimized: true,
                clickable: true
            };
            if (guess) opts.guess = guess;

            var marker = new google.maps.Marker(opts);
            return marker as IMarkerWithSource;
        }
    }

    /**
     * 
     * @param {{lat: number,lng: number,pano: string|null}} loc location data
     * @param {number} maxrad maximum radius in meters to search for
     * @param {google.maps.StreetViewPreference|null} pref streetview quality preference
     * @param {boolean} retry wheter to retry on fail
     */
    export function GetStreetviewPanorama(loc: google.maps.LatLngLiteral & { pano?: string }, maxrad: number, pref: google.maps.StreetViewPreference = google.maps.StreetViewPreference.NEAREST, retry = true): Promise<any>
    {
        const webService: google.maps.StreetViewService
            | { getPanoramaById(pano: string, callback: Callback2<google.maps.StreetViewPanoramaData, google.maps.StreetViewStatus, void>): Promise<google.maps.StreetViewPanoramaData> }
            = new google.maps.StreetViewService()

        return new Promise(async (resolve: any, reject) =>
        {
            try
            {
                let callback = (res: google.maps.StreetViewPanoramaData | null, status: google.maps.StreetViewStatus) =>
                {
                    if (status != google.maps.StreetViewStatus.OK)
                        return reject({
                            ...loc,
                            reason: "Streetview not found"
                        });

                    loc.lat = res?.location?.latLng?.lat() ?? 0;
                    loc.lng = res?.location?.latLng?.lng() ?? 0;
                    loc.pano = res?.location?.pano ?? "";
                    resolve(loc);
                }

                if (!loc.pano)
                {
                    let opts = {
                        location: new google.maps.LatLng(loc.lat, loc.lng),
                        preference: pref,
                        radius: maxrad,
                        source: google.maps.StreetViewSource.DEFAULT
                    };

                    await webService.getPanorama(opts, callback)
                        .catch((e: any) => retry
                            ? GetStreetviewPanorama(loc, maxrad, google.maps.StreetViewPreference.BEST, false)
                            : reject({
                                loc,
                                reason: e.message
                            }));
                }
                else
                {
                    await (webService as any).getPanoramaById(loc.pano, callback).catch((e: any) => reject({
                        loc,
                        reason: e.message
                    }));
                }
            }
            catch (e)
            {
                console.warn("Couldn't find streetview. ", e);
                reject({
                    loc,
                    reason: "Streetview not found"
                })
            }

        });
    }

    export function ToggleMarkers()
    {
        if ($("#showAllGuessesBtn").prop("disabled")
            || !GeoChatter.Main.Table
            || !GeoChatter.Main.CurrentGame) return;

        console.log("Showing/Hiding markers")

        let rounds = $("#markerSelect").val() as Nullable<string>[];
        rounds = rounds.map(rname =>
        {
            let spl = rname?.split(" ");
            return spl ? spl[spl.length - 1] : "";
        })

        let rows = GeoChatter.Main.Table.getVisibleRows();
        let len = Math.min(rows.length, Setting.Overlay.MaximumRowCountForAllMarkersDisplay);
        let addedStreamers = false;
        let streamer = GeoChatter.Main.CurrentGame.CurrentRound?.Guesses.filter(g => g.IsStreamerGuess)[0]?.PlayerData;
        let key = streamer ? Util.GetPlayerDataKey(streamer) : "";

        for (let i = 0; i < len || (!addedStreamers && i < rows.length); i++)
        {
            let p = (rows[i]?.data as TableRow).Player;
            let pkey = Util.GetPlayerDataKey(p)
            if (!addedStreamers)
            {
                addedStreamers = pkey == key;
            }

            if (State.App.HideMarkerState)
            {
                if (!NamedMarkers.rowClickResults?.Markers[pkey] && !NamedMarkers.rowClickResults?.Lines[pkey])
                {
                    PopulateMapWithGuessesOf(Div.GetTableRow(i), p, rounds)
                }
            } else
            {
                if (NamedMarkers.rowClickResults?.Markers[pkey] || NamedMarkers.rowClickResults?.Lines[pkey])
                {
                    PopulateMapWithGuessesOf(Div.GetTableRow(i), p, rounds)
                }
            }
        }

        if (State.App.HideMarkerState)
        {
            if (streamer && !addedStreamers && !NamedMarkers.rowClickResults?.Markers[key] && !NamedMarkers.rowClickResults?.Lines[key])
            {
                PopulateMapWithGuessesOf(Util.GetTableRowOf(streamer), streamer, rounds);
            }

            if (Setting.Overlay.MarkerClustersEnabled)
            {
                var lis: Array<IMarkerWithSource> = [];
                Object.values(NamedMarkers.rowClickResults?.Markers ?? []).forEach(x => x.forEach(y => lis.push(y)))
                MakeClusterFromList(lis);
            }
            AddCorrectLocationMarkers();
        } else
        {
            if (streamer && !addedStreamers && (NamedMarkers.rowClickResults?.Markers[key] || NamedMarkers.rowClickResults?.Lines[key]))
            {
                PopulateMapWithGuessesOf(Util.GetTableRowOf(streamer), streamer, rounds);
            }

            if (CurrentClusterer != null)
            {
                CurrentClusterer.clearMarkers();
                CurrentClusterer = null;
            }
        }

        State.App.HideMarkerState = !State.App.HideMarkerState;
    }

    export function PopulateMapWithGuessesOf(element: Nullable<JQuery<HTMLElement>>, player: PlayerData, rounds: Array<Nullable<string>>)
    {
        let col = GeoChatter.Main.Table?.getVisibleColumns().find(c => c.dataField == Enum.DataField.PlayerName);
        if (!col) return alert("Player names column must be visible to enable scoreboard click actions!");

        let cell = element
            ? $(element).children("[aria-describedby='" + (col as any).headerId + "']")
            : null;

        let key = Util.GetPlayerDataKey(player);

        if (NamedMarkers.rowClickResults?.Markers[key] || NamedMarkers.rowClickResults?.Lines[key])
        {
            let ms = NamedMarkers.rowClickResults?.Markers[key];
            while (ms && ms[0])
            {
                ms.pop()?.setMap(null);
            }

            delete NamedMarkers.rowClickResults?.Markers[key]

            if (NamedMarkers.rowClickResults?.Lines[key])
            {
                let ls = NamedMarkers.rowClickResults?.Lines[key];
                while (ls && ls[0])
                {
                    ls.pop()?.setMap(null);
                }

                delete NamedMarkers.rowClickResults?.Lines[key]
            }
            cell?.css("background", "unset")

            let cs = Markers.correctLocations;
            while (State.Scoreboard.DisplayingCurrentStandings && cs && cs[0]) cs.pop()?.setMap(null);

        }
        else if (GeoChatter.Main.CurrentGame)
        {
            if (State.Scoreboard.DisplayingCurrentStandings || GeoChatter.Main.CurrentGame.Stage == Enum.GAMESTAGE.ENDGAME)
            {
                var c = "white";
                cell?.css("background", `linear-gradient(360deg, ${c}, #ffffff00, ${c}, #ffffff00, ${c}, #ffffff00, ${c}, #ffffff00, ${c}, #ffffff00)`)

                const infowindow = new google.maps.InfoWindow({
                    disableAutoPan: true
                });

                let color = Color.RandomColor();
                let i = 0;

                if (NamedMarkers.rowClickResults)
                {
                    NamedMarkers.rowClickResults.Markers[key] = [];
                    NamedMarkers.rowClickResults.Lines[key] = [];
                }

                let allrounds = GeoChatter.Main.CurrentGame.AllRounds;
                for (let round of allrounds)
                {
                    i++;
                    let guess = round.GetGuessOf(player);

                    if (!guess) continue;

                    if (rounds && rounds.length > 0 && rounds.indexOf(round.ID.toString()) < 0) continue;

                    if (guess.PlayerData.Color) color = guess.PlayerData.Color;

                    let correct = {
                        lng: round.Location.Longitude,
                        lat: round.Location.Latitude
                    };

                    let pos = {
                        lat: guess.Location.Latitude,
                        lng: guess.Location.Longitude
                    };

                    const guessMarker = CreateMarker(pos, guess, `R${i} #${guess.ResultFinalOrder}`);
                    if (!guessMarker) continue;

                    google.maps.event.addListener(guessMarker, "click", () =>
                    {
                        guess?.EmbedLocation();
                    });

                    AddMarkerInfoWindowEvents(infowindow, guessMarker, guess);

                    if (!NamedMarkers.rowClickResults)
                        NamedMarkers.rowClickResults = {
                            Markers: {},
                            Lines: {}
                        };

                    if (!NamedMarkers.rowClickResults.Markers[key]) NamedMarkers.rowClickResults.Markers[key] = [];

                    NamedMarkers.rowClickResults?.Markers[key]?.push(guessMarker);

                    if (Setting.Overlay.CreatePolylines && NamedMarkers.rowClickResults)
                    {
                        let pl = CreatePolyLine(color, 2, pos, correct);
                        if (pl)
                        {
                            if (!NamedMarkers.rowClickResults.Lines[key]) NamedMarkers.rowClickResults.Lines[key] = [];

                            NamedMarkers.rowClickResults.Lines[key]?.push(pl);
                        }
                    }
                }
            }
            else if (GeoChatter.Main.CurrentGame.Stage == Enum.GAMESTAGE.ENDROUND)
            {
                var guess = GeoChatter.Main.CurrentGame.CurrentRound?.GetGuessOf(player);
                if (guess)
                {
                    if (guess.MapsMarker) guess.MapsMarker.setVisible(true);
                    if (guess.MapsLine) guess.MapsLine.setVisible(true);

                    GeoChatter.Map?.setCenter(guess.MapsMarker?.getPosition() ?? new google.maps.LatLng(0, 0))
                    if ((GeoChatter.Map?.getZoom() ?? Infinity) < 5)
                    {
                        GeoChatter.Map?.setZoom(5)
                    }
                }
            }
        }
    }

    /**
     * Create a googlemaps polyline
     * @param {string} color color
     * @param {number} weight stroke weight
     * @param {Object<string,number>} guessed guessed location
     * @param {Object<string,number>} correct correct location
     */
    export function CreatePolyLine(color: Nullable<string>, weight: number, guessed: google.maps.LatLngLiteral, correct: google.maps.LatLngLiteral): Nullable<google.maps.Polyline>
    {
        if (!color) color = Color.RandomColor();

        return GeoChatter.Map
            ? new google.maps.Polyline({
                map: GeoChatter.Map,

                strokeColor: color,
                strokeWeight: weight,
                strokeOpacity: 0.74,

                geodesic: true,
                path: [guessed, correct],

                clickable: false
            })
            : null;
    }

    export function ShowMarkersOf(player: PlayerData)
    {
        let row = Util.GetTableRowOf(player)
        return PopulateMapWithGuessesOf(row, player, [])
    }

    /** Remove geoguessr marker remover from google maps */
    export function DeleteMarkerRemover()
    {
        let o = document.getElementById(Constant._id_MarkerRemover);
        if (o) o.remove();
    }

    /** Add geoguessr marker remover for google maps */
    export function AddMarkerRemover()
    {
        let mark = document.createElement("style");
        mark.id = Constant._id_MarkerRemover
        mark.innerHTML = ".map-pin{display:none}";
        Dependency.Head.appendChild(mark);
        return mark;
    }

    //////////////////
    /** Remove custom markers and polylines */
    export function ResetMarkersAndScores()
    {
        window.setTimeout(ClearMarkers, 100);
        window.setTimeout(DeleteMarkerRemover, 100);
    }

    /** Remove markers from the map canvas */
    export function ClearMarkers()
    {
        if (NamedMarkers.rowClickResults)
        {
            for (let [name, _] of Object.entries(NamedMarkers.rowClickResults.Markers))
            {
                let ms = NamedMarkers.rowClickResults.Markers[name];
                while (ms && ms[0])
                {
                    ms.pop()?.setMap(null);
                }

                delete NamedMarkers.rowClickResults.Markers[name]

            }
            for (let [lname, _] of Object.entries(NamedMarkers.rowClickResults.Lines))
            {
                let ls = NamedMarkers.rowClickResults.Lines[lname];
                while (ls && ls[0])
                {
                    ls.pop()?.setMap(null);
                }

                delete NamedMarkers.rowClickResults.Lines[lname]
            }

            let cs = Markers.correctLocations;
            let cls = Lines.correctLocations;
            while (State.Scoreboard.DisplayingCurrentStandings && cs && cs[0]) cs.pop()?.setMap(null);
            while (State.Scoreboard.DisplayingCurrentStandings && cls && cls[0]) cls.pop()?.setMap(null);
        }

        for (let k of [Enum.MarkerLineAccessKey.currentGame, Enum.MarkerLineAccessKey.rowClickResults])
        {
            let mk = Markers[k];
            if (mk)
            {
                while (mk[0])
                {
                    mk.pop()?.setMap(null);
                }
            }

            let lk = Lines[k];
            if (lk)
            {
                while (lk[0])
                {
                    lk.pop()?.setMap(null);
                }
            }
        }

        ClearCluster(Enum.MarkerLineAccessKey.currentGame);
        ClearCluster(Enum.MarkerLineAccessKey.rowClickResults);
        //ClearCluster(window, "correctLocationMarkers");
        if (CurrentClusterer != null)
        {
            CurrentClusterer.clearMarkers();
            CurrentClusterer = null;
        }
    }

    function AddMarkerInfoWindowEvents(infowindow: Nullable<google.maps.InfoWindow>, marker: google.maps.Marker, source: ICountrySource)
    {
        if (!infowindow)
            infowindow = new google.maps.InfoWindow({
                disableAutoPan: true
            });

        google.maps.event.addListener(marker, "mouseover", () =>
        {
            if (!source || !infowindow) return;
            infowindow.setContent(source.GetInfoWindowHTML());
            infowindow.open(GeoChatter.Map, marker);
            $(infowindow).prev('div').remove();
        });
        google.maps.event.addListener(marker, "mouseout", () => infowindow?.close());

        return infowindow;
    }

    export function PopulateMapWithCurrentRound()
    {
        if (!GeoChatter.Map
            || !GeoChatter.Main.CurrentGame) return;

        const icon = {
            ...CorrectLocationMarker,
            fillColor: "#ff0000",
            anchor: new google.maps.Point(195, 440),
            labelOrigin: new google.maps.Point(330, 40),
        };

        let round = GeoChatter.Main.CurrentGame.CurrentRound;
        if (!round) return;

        let correct = {
            lng: round.Location.Longitude,
            lat: round.Location.Latitude
        };

        const locationMarker = new google.maps.Marker({
            position: correct,
            icon: icon,
            map: GeoChatter.Map,
        });

        let heading = round.Location.Heading;
        let pitch = round.Location.Pitch;
        let fov = round.Location.FOV;
        let pano = round.Location.Pano;

        google.maps.event.addListener(locationMarker, "click", () =>
        {
            if (!round) return;
            Control.EmbedLocation(`round-${round.PositionInChain}-correctLocation`, round.GetEmbedTitleHTML(), correct, heading, pitch, fov, pano, round)
        });

        const infowindow = AddMarkerInfoWindowEvents(null, locationMarker, round);

        if (!Markers.currentGame) Markers.currentGame = [];

        Markers.currentGame?.push(locationMarker);

        let addedStreamers = false;

        round.Results.forEach((guess, index) => 
        {
            if (!GeoChatter.Main.CurrentGame) return;

            let visible = true;
            if (index >= Setting.Overlay.MaximumMarkerCountForRoundEnd)
            {
                if (addedStreamers || !guess.IsStreamerGuess) visible = false;
            }

            if (!addedStreamers)
            {
                addedStreamers = guess.IsStreamerGuess;
            }

            let pos = {
                lat: guess.Location.Latitude,
                lng: guess.Location.Longitude
            };

            const guessMarker = CreateMarker(pos, guess, `#${guess.ResultFinalOrder}`);
            if (!guessMarker) return;

            guess.MapsMarker = guessMarker;

            guessMarker.setVisible(visible);

            google.maps.event.addListener(guessMarker, "click", () =>
            {
                guess?.EmbedLocation();
            });

            AddMarkerInfoWindowEvents(infowindow, guessMarker, guess);

            if (!Markers.currentGame) Markers.currentGame = [];

            Markers.currentGame?.push(guessMarker);

            if (Setting.Overlay.CreatePolylines)
            {
                let poly: Nullable<google.maps.Polyline> = null;
                if (!guess.IsStreamerGuess)
                {
                    poly = CreatePolyLine(guess.PlayerData.Color, 2, pos, correct);
                }
                else
                {
                    poly = CreatePolyLine(guess.PlayerData.Color, 3, pos, correct);
                }
                if (!poly) return;

                guess.MapsLine = poly;
                guess.MapsLine.setVisible(visible);
                if (!Lines.currentGame) Lines.currentGame = [];

                Lines.currentGame?.push(poly);
            }
        });

        if (Setting.Overlay.MarkerClustersEnabled) MakeCluster(Enum.MarkerLineAccessKey.currentGame, true);
    }
}

window.GC.MapUtil = MapUtil;