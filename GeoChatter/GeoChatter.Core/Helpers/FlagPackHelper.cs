using GeoChatter.Helpers;
using GeoChatter.Core.Handlers;
using GeoChatter.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static GeoChatter.Core.Handlers.GCSchemeHandler;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// Helper methods for flag packs
    /// </summary>
    public static class FlagPackHelper
    {
        /// <summary>
        /// Directory of flag pack folders
        /// </summary>
        public const string FlagsDirectory = "./Styles/flags/";

        /// <summary>
        /// .json file name for flag packs
        /// </summary>
        public static string FlagPackJSON { get; } = "flagpack.json";

        /// <summary>
        /// Available flag packs
        /// </summary>
        public static List<FlagPack> FlagPacks { get; set; } = new();

        /// <summary>
        /// Names from flag packs mapped to codes
        /// </summary>
        public static Dictionary<string, string> MappedFlagPackNames { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Load all custom flag packs
        /// </summary>
        public static void LoadCustomFlagPacks()
        {
            if (!Directory.Exists(FlagsDirectory))
            {
                Directory.CreateDirectory(FlagsDirectory);
            }

            foreach (KeyValuePair<string, string> pair in MappedFlagPackNames)
            {
                if (CountryHelper.FlagCodes.Contains(pair.Value))
                {
                    CountryHelper.FlagCodes.Remove(pair.Value);
                }
            }

            MappedFlagPackNames.Clear();

            string[] dirs = Directory.GetDirectories(FlagsDirectory);

            foreach (string dir in dirs)
            {
                LoadCustomFlagPack(dir);
            }
        }

        /// <summary>
        /// Load custom flag pack at given directory <paramref name="dir"/> to <see cref="MappedFlagPackNames"/>
        /// </summary>
        /// <param name="dir"></param>
        /// <exception cref="FileLoadException"></exception>
        public static void LoadCustomFlagPack(string dir)
        {
            FlagPack flagPack = JsonConvert.DeserializeObject<FlagPack>(File.ReadAllText(Path.Combine(dir, FlagPackJSON)));
            if (flagPack == null)
            {
                throw new FileLoadException($"Failed to parse flag pack at directory: '{dir}'");
            }

            if (FlagPacks.FirstOrDefault(fp => fp.Name == flagPack.Name) is FlagPack fpr)
            {
                FlagPacks.Remove(fpr);
            }

            foreach (KeyValuePair<string, string> item in flagPack.Flags)
            {
                if (MappedFlagPackNames.ContainsKey(item.Key))
                {
                    throw new FileLoadException($"Flag with code '{item.Key}' already exists!");
                }
                MappedFlagPackNames.Add(item.Key, item.Value);
            }

            string[] codes = Directory.GetFiles(dir, "*.svg", SearchOption.AllDirectories)
                .Select(path => Path.GetFileNameWithoutExtension(path).ToLowerInvariant())
                .ToArray();

            WriteCSS(flagPack);

            CountryHelper.AddFlagCodes(codes);

            FlagPacks.Add(flagPack);
        }

        /// <summary>
        /// Create and append required CSS classes from <paramref name="flagPack"/> to flag-custom.min.css
        /// </summary>
        /// <param name="flagPack"></param>
        public static void WriteCSS(FlagPack flagPack)
        {
            GCUtils.ThrowIfNull(flagPack, nameof(flagPack), "Flag pack was null.");

            // Delete previous if any
            DeleteCSS(flagPack);

            StringBuilder writer = new(ResourceDictionary[$"{SchemePrefix}styles/flag-custom.min.css"].Value);
            writer.AppendLine("\r\n");
            writer.AppendLine(CultureInfo.InvariantCulture, $"/* START {flagPack.Name} */");
            foreach (KeyValuePair<string, string> item in flagPack.Flags)
            {

                writer.AppendLine(".flag-icon-" + item.Value.ToLowerInvariant() + " {");

                writer.AppendLine(CultureInfo.InvariantCulture, $"background-image: url(flags/{flagPack.Name}/{item.Value}.svg)");
                writer.AppendLine("}");
            }
            writer.AppendLine(CultureInfo.InvariantCulture, $"/* END {flagPack.Name} */");
            ResourceDictionary[$"{SchemePrefix}styles/flag-custom.min.css"] = new SchemeResource() { Path = $"{SchemePrefix}styles/flag-custom.min.css", Value = writer.ToString() };
        }

        /// <summary>
        /// Remove CSS written by <see cref="WriteCSS(FlagPack)"/> for <paramref name="flagPack"/>
        /// </summary>
        /// <param name="flagPack"></param>
        public static void DeleteCSS(FlagPack flagPack)
        {
            GCUtils.ThrowIfNull(flagPack, nameof(flagPack), "Flag pack was null.");

            SchemeResource resource = ResourceDictionary[$"{SchemePrefix}styles/flag-custom.min.css"];
            int start = resource.Value.IndexOf($"/* START {flagPack.Name} */", StringComparison.InvariantCulture);
            int end = resource.Value.IndexOf($"/* END {flagPack.Name} */", StringComparison.InvariantCulture) + 10 + flagPack.Name.Length - start;
            if (start != -1 && end != -1)
            {
                resource.Value = resource.Value.Remove(start, end);
                ResourceDictionary[$"{SchemePrefix}styles/flag-custom.min.css"] = resource;
            }
        }

    }
}
