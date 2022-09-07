using GeoChatter.Core.Common.Extensions;
using System.Drawing;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// Provides helper methods for color conversions
    /// </summary>
    public static class ColorHelper
    {
        private static System.Text.RegularExpressions.Regex CSSColor { get; } = new(@"rgba\((\d+),(\d+),(\d+),(\d+)\)");

        /// <summary>
        /// From CSS <c>rgba(r,g,b,a)</c> to <c>#rgb</c>
        /// </summary>
        /// <param name="rgba"></param>
        /// <returns></returns>
        public static string GetHexColorFromCSS(string rgba)
        {
            if (CSSColor.Match(rgba) is System.Text.RegularExpressions.Match match
                && match.Groups.Count >= 4
                && match.Groups[1].Value.TryParseIntDefault(out int r)
                && match.Groups[2].Value.TryParseIntDefault(out int g)
                && match.Groups[3].Value.TryParseIntDefault(out int b))
            {
                return $"#{r.ToHex()}{g.ToHex()}{b.ToHex()}"; // TODO: add alpha in the future
            }
            return rgba;
        }

        private static string ColorToCSS(Color color)
        {
            return $"rgba({color.R},{color.G},{color.B},{color.A})";
        }

        private static Color FromHtmlToColor(string html, out string hexcode)
        {
            hexcode = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(html))
                {
                    return Color.Empty;
                }

                hexcode = "#" + html.TrimStart('#');
                return ColorTranslator.FromHtml(hexcode);
            }
            catch
            {
                return Color.Empty;
            }
        }

        /// <summary>
        /// Parse given color name to <c>rgba(r,g,b,a)</c> format
        /// </summary>
        /// <param name="colorName">Color name or hexadecimal #RRGGBB value</param>
        /// <param name="colorRealName">Real color of <paramref name="colorName"/> if any</param
        /// <param name="color">Result as Color struct</param>
        /// <returns></returns>
        public static string ParseColor(string colorName, out string colorRealName, out Color color)
        {
            color = Color.FromName(colorName);
            colorRealName = color.Name;

            if (!color.IsKnownColor)
            {
                color = FromHtmlToColor(colorName, out string hexcode);

                if (color.IsEmpty)
                {
                    return string.Empty;
                }
                else
                {
                    colorRealName = color.IsNamedColor ? color.Name : hexcode;
                }
            }

            string rgba = ColorToCSS(color);
            return rgba;
        }
    }
}
