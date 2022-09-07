using GeoChatter.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Core.Storage;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using GeoChatter.Model;
using GeoChatter.Core.Common.Extensions;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace GeoChatter.Core.Helpers
{

    /// <summary>
    /// Static helper class to map "unofficial" country codes to countires, e.g. IM (Isle of Man) to UK (United Kingdom)
    /// </summary>
    public static class CountryHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CountryHelper));

        /// <summary>
        /// Directory of built in flag files
        /// </summary>
        public static ResourceMeta FlagMeta { get; set; } = ResourceHelper.GetResourceMeta(typeof(CountryHelper));

        private static Dictionary<string, NameMapping> mappedDefaultNames = new();

        private static Dictionary<string, string> countryDictionary { get; } = ISO3166.Country.List.OrderBy(x => x.Name).ToDictionary(x => x.TwoLetterCode, x => x.Name);

        /// <summary>
        /// Available culture instances
        /// </summary>
        public static CultureInfo[] Cultures { get; } = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

        private static List<RegionInfo> regionInfos { get; } = GetRegionInfos();

        private static Random random { get; } = new();

        /// <summary>
        /// Get a dictionary of currently available flag mappings
        /// </summary>
        public static Dictionary<string, string> MappedNames
        {
            get
            {
                Dictionary<string, string> result = new(StringComparer.InvariantCultureIgnoreCase);
                FlagPackHelper.MappedFlagPackNames.ForEach(x => result[x.Key] = x.Value);
                MappedDefaultNames.ForEach(x => result[x.Key] = x.Value.Code);
                return result;
            }

        }


        /// <summary>
        /// Mapping of common names to their flag codes (file names)
        /// </summary>
        public static Dictionary<string, NameMapping> MappedDefaultNames
        {
            get
            {
                if (!Initialized)
                {
                    Initialize();
                }
                return mappedDefaultNames;
            }
            private set => mappedDefaultNames = value;
        }

        /// <summary>
        /// Name mapping json file
        /// </summary>
        public static string NameMappingFile => "namemapping.json";

        private static RestClient restClient { get; } = new();

        private static bool Initialized { get; set; }

        /// <summary>
        /// All flag codes from files names
        /// </summary>
        public static List<string> FlagCodes { get; } = new();

        /// <summary>
        /// Add given flag codes to <see cref="FlagCodes"/>
        /// </summary>
        /// <param name="codes"></param>
        /// <exception cref="FileLoadException"></exception>
        public static void AddFlagCodes(string[] codes)
        {
            if (codes != null)
            {
                foreach (string code in codes)
                {
                    if (FlagCodes.Contains(code))
                    {
                        throw new FileLoadException($"Flag with code '{code}' already exists!");
                    }
                    FlagCodes.Add(code);
                }
            }
        }

        /// <summary>
        /// Load flags
        /// </summary>
        /// <returns></returns>
        public static void LoadFlags()
        {
            FlagCodes.Clear();

            if (!ResourceHelper.HasLatestResource(FlagMeta))
            {
                ResourceHelper.InstallLatest(FlagMeta);
            }

            string[] codes = Directory.GetFiles(FlagMeta.TargetDirectory, "*.svg", SearchOption.TopDirectoryOnly)
                .Select(path => Path.GetFileNameWithoutExtension(path).ToLowerInvariant())
                .ToArray();

            AddFlagCodes(codes);
        }

        /// <summary>
        /// Initializer
        /// </summary>
        public static void Initialize()
        {
            Initialized = true;
            logger.Info("Initializing name mappings");

            try
            {
                RestRequest req = new(Path.Combine(ResourceHelper.OtherServiceURL, NameMappingFile), Method.Get) { RequestFormat = DataFormat.Json };
                RestResponse res = restClient.Execute(req);

                if (res.IsSuccessful)
                {
                    logger.Debug($"GET {NameMappingFile} done");
                    MappedDefaultNames = new(JsonConvert.DeserializeObject<Dictionary<string, NameMapping>>(res.Content), StringComparer.InvariantCultureIgnoreCase);
                    Country.MappedNames = MappedDefaultNames;
                    logger.Info($"Initialized name mappings with {MappedDefaultNames.Count} entries");
                }
                else
                {
                    logger.Error($"GET {NameMappingFile} failed({res.ErrorMessage}): {res.ErrorException?.Summarize()}");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        /// <summary>
        /// Get country and region given <paramref name="lat"/> and <paramref name="lng"/> coordinates are in
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lng">Longitude</param>
        /// <param name="englishNames">Wheter to use english names</param>
        /// <param name="exactcountry">Exact region within returned country. If nothing specific found, same values as returned value.</param>
        /// <returns></returns>
        public static Country GetCountry(string lat, string lng, bool englishNames, out Country exactcountry)
        {
            bool valid = GCUtils.ValidateAndFixCoordinates(lat, lng, out double latitude, out double longitude);

            if (!valid)
            {
                exactcountry = Country.Unknown;
                return Country.Unknown;
            }

            string code = BorderHelper.GetFeatureHitBy(new double[] { longitude, latitude }, out GeoJson geo, out Feature hitFeature, out Polygon _);

            Country country = GetCountryInformation(code, geo, hitFeature, englishNames, out exactcountry);

            if (ClientDbCache.RunningGame.Source.streakType == Game.USStreaksGame)
            {
                country = new(exactcountry.Code, exactcountry.Name);
            }

            return country;
        }

        /// <summary>
        /// Alpha-2, Alpha-3 code or name to ISO3166 name
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string NameFromCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return string.Empty;
            }

            code = code.Trim();
            if (code.Length == 2)
            {
                return ISO3166Helper.FromAlpha2(code)?.Name;
            }
            else if (code.Length == 3)
            {
                return ISO3166Helper.FromAlpha3(code)?.Name;
            }
            return ISO3166Helper.Collection.FirstOrDefault(c => c.Name.ToLowerInvariant() == code.ToLowerInvariant())?.Name;
        }

        /// <summary>
        /// Process flag name or code and return process success
        /// </summary>
        /// <param name="flag">Flag name or code to be fixed</param>
        /// <param name="cname">Real flag name if process succeeds</param>
        /// <returns><see langword="true"/> if <paramref name="flag"/> was valid, otherwise <see langword="false"/></returns>
        public static bool CheckFlagCode(ref string flag, out string cname, Dictionary<string, string> cache = null)
        {
            flag ??= string.Empty;
            cname = string.Empty;
            Dictionary<string, string> mapped = cache ?? MappedNames;

            string org = flag;

            flag = flag.ToLowerInvariant();

            if (flag == "random")
            {
                List<string> uniqmapped = mapped.Values.Distinct().ToList();
                int idx = random.Next(FlagCodes.Count + uniqmapped.Count);
                if (idx >= FlagCodes.Count)
                {
                    flag = uniqmapped.ElementAt(idx - FlagCodes.Count);
                    string _flag = flag;
                    string name = mapped.Where(kv => kv.Value == _flag).RandomPick().Key ?? cname;
                    cname = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name);
                }
                else
                {
                    flag = FlagCodes[idx].ToUpperInvariant();
                    foreach (KeyValuePair<string, string> spair in mapped)
                    {
                        if (spair.Value == flag || spair.Value == org)
                        {
                            cname = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(spair.Key);
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(cname))
                {
                    cname = NameFromCode(flag);
                }
                cname = Country.GetMappedCountryName(cname);
                return true;
            }

            int i = FlagCodes.IndexOf(flag);
            if (i == -1)
            {
                if (mapped.ContainsKey(flag))
                {
                    cname = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(flag);
                    cname = Country.GetMappedCountryName(cname);
                    flag = mapped[flag];
                    return true;
                }

                cname = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(flag);
                if (string.IsNullOrEmpty(cname))
                {
                    cname = NameFromCode(flag);
                }
                cname = Country.GetMappedCountryName(cname);
                flag = GetCountryCodeFromName(flag);
                flag = flag.ToLowerInvariant();
                i = FlagCodes.IndexOf(flag);
            }
            else
            {
                string alt = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(flag);
                flag = flag.ToUpperInvariant();

                foreach (KeyValuePair<string, string> spair in mapped)
                {
                    if (spair.Value == flag || spair.Value == org || spair.Value == alt)
                    {
                        cname = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(spair.Key);
                        break;
                    }
                }
                if (string.IsNullOrEmpty(cname))
                {
                    cname = NameFromCode(flag);
                }
                cname = Country.GetMappedCountryName(cname);
                return true;
            }
            flag = flag.ToUpperInvariant();
            return i != -1;
        }

        /// <summary>
        /// Get 2 letter ISO or custom code from given name if it exists, othewise <see cref="string.Empty"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetCountryCodeFromName(string name)
        {
            RegionInfo rinfo = regionInfos.FirstOrDefault(r => r.EnglishName.ToLowerInvariant() == name);
            if (rinfo == null)
            {
                foreach (KeyValuePair<string, string> pair in countryDictionary)
                {
                    string low = pair.Value.ToLowerInvariant();
                    if (low == name || low.Split(',')[0] == name)
                    {
                        return pair.Key.ToUpperInvariant();
                    }
                }
            }
            else
            {
                return rinfo.TwoLetterISORegionName.ToUpperInvariant();
            }
            return string.Empty;
        }

        /// <summary>
        /// Get all available region information
        /// </summary>
        /// <returns></returns>
        public static List<RegionInfo> GetRegionInfos()
        {
            List<RegionInfo> reginfos = new();

            foreach (CultureInfo info in Cultures)
            {
                RegionInfo regInfo = new(info.LCID);
                if (!reginfos.Contains(regInfo))
                {
                    reginfos.Add(regInfo);
                }
            }
            return reginfos;
        }

        /// <summary>
        /// Get exact US Minor Outlying Island by <paramref name="feat"/>'s ISO
        /// </summary>
        /// <param name="geo"></param>
        /// <param name="feat"></param>
        /// <param name="englishName"></param>
        /// <param name="exactcountry"></param>
        /// <returns></returns>
        public static Country GetUSMinorOutlyingIslandData(GeoJson geo, Feature feat, bool englishName, out Country exactcountry)
        {
            Country country = GetCountryInformation("US", null, null, englishName, out Country _);
            string iso = feat?.properties.shapeISO;
            switch (iso)
            {
                case "UM-67":
                    {
                        exactcountry = new("UM-JOHNSTON-ATOLL", "Johnston Atoll");
                        break;
                    }
                case "UM-71":
                    {
                        exactcountry = new("UM-MIDWAY-ATOLL", "Midway Atoll");
                        break;
                    }
                case "UM-76":
                    {
                        exactcountry = new("UM-NAVASSA-ISLAND", "Navassa Island");
                        break;
                    }
                case "UM-79":
                    {
                        exactcountry = new("UM-WAKE-ISLAND", "Wake Island");
                        break;
                    }
                case "UM-81":
                    {
                        exactcountry = new("UM-BAKER-ISLAND", "Baker Island");
                        break;
                    }
                case "UM-84":
                    {
                        exactcountry = new("UM-HOWLAND-ISLAND", "Howland Island");
                        break;
                    }
                case "UM-86":
                    {
                        exactcountry = new("UM-JARVIS-ISLAND", "Jarvis Island");
                        break;
                    }
                case "UM-89":
                    {
                        exactcountry = new("UM-KINGMAN-REEF", "Kingman Reef");
                        break;
                    }
                case "UM-95":
                    {
                        exactcountry = new("UM-PALMYRA-ATOLL", "Palmyra Atoll");
                        break;
                    }
                case "UM-BAJO-NUEVO":
                    {
                        exactcountry = new("UM-BAJO-NUEVO-BANK", "Bajo Nuevo Bank");
                        break;
                    }
                case "UM-SERRANILLA":
                    {
                        exactcountry = new("UM-SERRANILLA-BANK", "Serranilla Bank");
                        break;
                    }
                default:
                    {
                        exactcountry = new("UM", country.Name);
                        break;
                    }
            }
            return country;
        }

        /// <summary>
        /// Get region 2 letter iso or custom code from <paramref name="feat"/> inside <paramref name="countryCode"/>
        /// <para>Regions of ADM1 (US, CA, GB etc.) type will have the region code returned, otherwise <paramref name="countryCode"/> is returned.</para>
        /// </summary>
        /// <param name="countryCode">Country code <paramref name="feat"/> is based on</param>
        /// <param name="feat">Feature to get region code from</param>
        /// <returns></returns>
        private static string GetCodeOfFeature(string countryCode, Feature feat)
        {
            switch (countryCode)
            {
                case "US":
                    return ISO3166Helper.FromAlpha3(feat.properties.shapeISO)?.Alpha2 ?? feat.properties.shapeISO.ReplaceDefault("-", string.Empty);
                case "CA":
                    return feat.properties.shapeISO;
                case "GB":
                    return ISO3166Helper.FromAlpha3(feat.properties.shapeISO)?.Alpha2 ?? feat.properties.shapeISO;
                default:
                    return ISO3166Helper.FromAlpha3(feat.properties.shapeISO)?.Alpha2 ?? countryCode;
            }
        }

        /// <summary>
        /// Get country information from country <paramref name="code"/> and processed border data
        /// </summary>
        /// <param name="code">2 letter ISO or custom code</param>
        /// <param name="geo">Processed geojson data</param>
        /// <param name="feat">Feature hit in <paramref name="geo"/> if any</param>
        /// <param name="englishName">Wheter to use English names</param>
        /// <param name="exactcountry">Exact region data from <paramref name="feat"/> or same as return value</param>
        /// <returns>Country data from given arguments</returns>
        public static Country GetCountryInformation(string code, GeoJson geo, Feature feat, bool englishName, out Country exactcountry)
        {
            if (!string.IsNullOrEmpty(code))
            {
                if (feat != null && code == "US" && feat.properties.shapeISO.StartsWithDefault("UM"))
                {
                    return GetUSMinorOutlyingIslandData(geo, feat, englishName, out exactcountry);
                }

                string exactCountryName = null, exactCode = null;
                code = code.ToUpperInvariant();
                exactCode = code;

                string countryName = string.Empty;
                RegionInfo codeInfo = null;

                if (countryDictionary.ContainsKey(exactCode))
                {
                    exactCountryName = countryDictionary[exactCode];
                }

                codeInfo = regionInfos.FirstOrDefault(r => r.Name == code);
                if (codeInfo != null)
                {
                    countryName = englishName ? codeInfo.EnglishName : codeInfo.DisplayName;
                }
                else if (countryDictionary.ContainsKey(code))
                {
                    countryName = countryDictionary[code];
                }

                if (feat != null)
                {
                    exactCode = GetCodeOfFeature(code, feat);
                    exactCountryName = exactCode == code && !string.IsNullOrEmpty(countryName)
                        ? countryName
                        : feat.properties.shapeName;
                }

                if (exactCountryName == null)
                {
                    exactCountryName = countryName;
                }

                if (string.IsNullOrEmpty(countryName) && !string.IsNullOrEmpty(exactCountryName))
                {
                    countryName = exactCountryName;
                }

                if (!string.IsNullOrEmpty(countryName) && !string.IsNullOrEmpty(code))
                {
                    exactcountry = new Country(exactCode, exactCountryName);
                    return new Country(code, countryName);
                }
            }
            exactcountry = Country.Unknown;
            return Country.Unknown;
        }

    }
}
