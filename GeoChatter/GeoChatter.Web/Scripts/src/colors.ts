export namespace Color
{
    const WCAG_MINIMUM_RATIOS = [
        ['AA Large', 3],
        ['AA', 4.5],
        ['AAA', 7],
    ] as const

    function luminance(r: number, g: number, b: number): number
    {
        let [lumR, lumG, lumB] = [r, g, b].map(component =>
        {
            let proportion = component / 255;

            return proportion <= 0.03928
                ? proportion / 12.92
                : Math.pow((proportion + 0.055) / 1.055, 2.4);
        });

        if (lumR && lumG && lumB)
            return 0.2126 * lumR + 0.7152 * lumG + 0.0722 * lumB;
        else
            return 0;
    }

    /**
     * Calculate brightness value by RGB or HEX color.
     * @param color (String) The color value in RGB or HEX (for example: #000000 || #000 || rgb(0,0,0) || rgba(0,0,0,0))
     * @returns (Number) The brightness value (dark) 0 ... 255 (light)
     */
    function brightnessByColor(color: string): number
    {
        var color = "" + color,
            isHEX = color.indexOf("#") == 0,
            isRGB = color.indexOf("rgb") == 0,
            r: number = 0,
            g: number = 0,
            b: number = 0,
            hasFullSpec: boolean = false,
            m: Nullable<RegExpMatchArray>;

        if (isHEX)
        {
            hasFullSpec = color.length == 7;
            m = color.substr(1).match(hasFullSpec ? /(\S{2})/g : /(\S{1})/g);
        }
        else if (isRGB)
        {
            m = color.match(/(\d+){3}/g);
        }

        if (m && m.length >= 3)
        {
            if (m[0]) r = parseInt(m[0] + (hasFullSpec ? '' : m[0]), 16);
            if (m[1]) g = parseInt(m[1] + (hasFullSpec ? '' : m[1]), 16);
            if (m[2]) b = parseInt(m[2] + (hasFullSpec ? '' : m[2]), 16);
            return luminance(r, g, b);
        }

        return 0;
    }

    function contrastRatio(luminance1: number, luminance2: number): number
    {
        let lighterLum = Math.max(luminance1, luminance2);
        let darkerLum = Math.min(luminance1, luminance2);

        return (lighterLum + 0.05) / (darkerLum + 0.05);
    }

    //function checkContrast(color1: string, color2: string): number
    //{
    //    let [luminance1, luminance2] = [color1, color2].map(brightnessByColor);

    //    return contrastRatio(luminance1, luminance2);
    //},

    function meetsMinimumRequirements(ratio: number): { didPass: boolean, maxLevel: Nullable<string>, ratio: number }
    {
        let didPass = false
        let maxLevel = null

        for (const [level, minRatio] of WCAG_MINIMUM_RATIOS)
        {
            if (ratio < minRatio) break

            didPass = true
            maxLevel = level
        }

        return { didPass: didPass, maxLevel: maxLevel, ratio: ratio }
    }

    export function ColorUsername(color: Nullable<string>, username: string, paddingX: boolean = false): string
    {
        if (!color) color = RandomColor();

        let clum = brightnessByColor(color);
        let bc = meetsMinimumRequirements(contrastRatio(0, clum))
        let wc = meetsMinimumRequirements(contrastRatio(1, clum))

        let lum;
        if (bc.didPass && wc.didPass)
        {
            lum = bc.ratio >= wc.ratio ? 'dark' : 'light'
        }
        else if (bc.didPass)
        {
            lum = 'dark';
        }
        else
        {
            lum = 'light';
        }
        return `<span class='username-${lum}' style='${(paddingX ? 'padding:0px 0.5rem;' : '')}color:` + color + `'>${username}</span>`
    }

    export function RandomColor(): string
    {
        var letters = '0123456789ABCDEF';
        var color = '#';
        for (var i = 0; i < 6; i++)
        {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }
}

window.GC.Color = Color;