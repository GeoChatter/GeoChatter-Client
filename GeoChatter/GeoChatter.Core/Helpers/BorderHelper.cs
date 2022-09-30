using GeoChatter.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Model;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if !DEBUG
using System.Runtime.CompilerServices;
#endif
using System.Threading;
using System.Threading.Tasks;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Core.Helpers
{
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public sealed class Geometry
    {
        public string type { get; set; }
        public JArray coordinates { get; set; }
    }

    public sealed class MultiPolygon
    {
        internal Polygon[] Polys;
    }

    public sealed class Polygon
    {
        internal double[][,] Coords;
        internal double[] minX;
        internal double[] maxX;
        internal double[] minY;
        internal double[] maxY;
        internal int[] len;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Needs to be same as sent object's")]
    public sealed class Properties
    {
        private string _shapeISO;

        public string shapeName { get; set; }
        public string shapeGroup { get; set; }
        public string shapeISO { get => _shapeISO ?? shapeGroup; set => _shapeISO = value; }
        public string shapeType { get; set; }
        public override string ToString()
        {
            return $"{shapeName} ({(_shapeISO == null ? shapeGroup : (_shapeISO + " / " + shapeGroup))}) <{shapeType}>";
        }
    }

    public sealed class Feature
    {
        public string type { get; set; }
        public Geometry geometry { get; set; }
        public object coords { get; set; }
        public Properties properties { get; set; }

        public override string ToString()
        {
            return $"{properties.shapeName} ({properties.shapeISO})";
        }
    }

    public sealed class GeoJson
    {
        public List<Feature> features { get; set; }
        public string type { get; set; }
        public override string ToString()
        {
            return $"{features[0].properties.shapeGroup} ({features.Count})";
        }
    }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Helper for border related processes
    /// </summary>
    public static class BorderHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(BorderHelper));

        /// <summary>
        /// Random number generator
        /// </summary>
        private static Random random = new();

        /// <summary>
        /// Meta data for border resources
        /// </summary>
        public static ResourceMeta BorderMeta { get; set; } = ResourceHelper.GetResourceMeta(typeof(BorderHelper));

        /// <summary>
        /// Array of all processed border data
        /// </summary>
        public static GeoJson[] BorderData { get; private set; } = Array.Empty<GeoJson>();

        private static bool Initializing { get; set; }
        private static bool Initialized { get; set; }

        private static int ReaderTaskCount { get; } = 5;
        private static int ReaderTaskEnded { get; set; }
        private static int ReaderTaskSuccessCount { get; set; }

        /// <summary>
        /// Weights json file
        /// </summary>
        public static string WeightsFile => "weights.json";

        private static RestClient restClient { get; } = new();

        private static void InitializeSpecialFactors()
        {
            logger.Info("Initializing special weights helper");
            try
            {
                RestRequest req = new(Path.Combine(ResourceHelper.OtherServiceURL, WeightsFile), Method.Get) { RequestFormat = DataFormat.Json };
                RestResponse res = restClient.Execute(req);

                if (res.IsSuccessful)
                {
                    logger.Debug($"GET {WeightsFile} done");
                    SpecialFactorForCountry = new(JsonConvert.DeserializeObject<Dictionary<string, int>>(res.Content), StringComparer.InvariantCultureIgnoreCase);
                    logger.Info($"Initialized special weights with {SpecialFactorForCountry.Count} entries");
                }
                else
                {
                    logger.Error($"GET {WeightsFile} failed({res.ErrorMessage}): {res.ErrorException?.Summarize()}");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        /// <summary>
        /// Initialize borders from file system, process the borders and set up <see cref="BorderData"/>
        /// </summary>
        /// <param name="timeoutMilliseconds">Maximum amount of milliseconds to cancel the initializer and stop processing</param>
        /// <returns></returns>
#if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        public static void Initialize(int timeoutMilliseconds = 60000)
        {
            try
            {
                if (Initializing)
                {
                    return;
                }

                Initializing = true;

                if (!ResourceHelper.HasLatestResource(BorderMeta))
                {
                    ResourceHelper.InstallLatest(BorderMeta);
                }

                DirectoryInfo di = new(BorderMeta.TargetDirectory);
                FileInfo[] fiArr = di.GetFiles().Where(f => f.Extension == ".geojson").ToArray();
                string[] files = fiArr.OrderBy(f => f.Length).Select(f => f.FullName).ToArray();

                BorderData = new GeoJson[files.Length];

                int tasks = ReaderTaskCount;
                int each = files.Length / tasks;
                int left = files.Length - (tasks * each);

                List<Task> taskLis = new(tasks);
                GCTaskScheduler gct = new(tasks);
                using CancellationTokenSource cts = new(timeoutMilliseconds);

                for (int i = 0; i < tasks; i++)
                {
                    int j = i;
                    taskLis.Add(
                        Task.Factory.StartNew(() =>
                        {
                            ProcessGeoJsonFromFilesTask(each * j, files[(each * j)..(each * (j + 1))], j + 1, files.Length);
                        },
                        cts.Token, TaskCreationOptions.PreferFairness, gct)
                    );
                }

                taskLis.Add(
                    Task.Factory.StartNew(() =>
                    {
                        ProcessGeoJsonFromFilesTask(files.Length - left, files[^left..], tasks, files.Length);
                    },
                    cts.Token, TaskCreationOptions.PreferFairness, gct)
                );

                Task.WaitAll(taskLis.ToArray());

                InitializeSpecialFactors();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

#if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        private static void ProcessGeoJsonFromFilesTask(int startId, string[] fs, int id, int fileCount)
        {
            try
            {
                ProcessGeoJsonFromFiles(startId, fs);

                Initializing = !(Initialized = BorderData.Count(g => g != null) == fileCount);

                ReaderTaskSuccessCount++;
                GC.Collect();
            }
            catch (Exception ex)
            {
                logger.Error($"GEOJSON ERROR: {ex.Summarize()}");
            }
            finally
            {
                ReaderTaskEnded++;
            }
        }

#if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        private static void ProcessGeoJsonFromFiles(int startId, string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                string f = files[i];
                try
                {
                    GeoJson o = InnerProcessGeoJsonFromFile(f);
                    if (o == null)
                    {
                        o = InnerProcessGeoJsonFromFile(f); // TODO: For some reason 1st try sometimes fail on debug build, 2nd doesn't...
                    }

                    if (o == null)
                    {
                        throw new IOException("Failed to process geojson file: " + f);
                    }

                    foreach (Feature feat in o.features)
                    {
                        if (feat.geometry.type == "Polygon")
                        {
                            feat.coords = GetPolygon(feat.geometry.coordinates);
                        }
                        else if (feat.geometry.type == "MultiPolygon")
                        {
                            feat.coords = GetMultiPolygon(feat.geometry.coordinates);

                            MultiPolygon mp = feat.coords as MultiPolygon;
                            int total = mp.Polys.Select(p => p.len[0]).Sum();
                            Polygon[] ordered = mp.Polys.OrderByDescending(p => p.len[0]).ToArray();
                            GetRandomPointWithinBoundBoxCache.Add(feat, new(ordered, total));
                        }
                        feat.geometry.coordinates = null;
                    }
                    BorderData[startId + i] = o;
                }
                catch (Exception ex)
                {
                    logger.Error("GEOJSON PROCESSING ERROR for '" + f + "' : " + ex.Summarize());
                }
            }
        }

        private static GeoJson InnerProcessGeoJsonFromFile(string filename)
        {
            try
            {
                string d = File.ReadAllText(filename);
                return JsonConvert.DeserializeObject<GeoJson>(d);
            }
            catch (Exception err)
            {
                logger.Error(err.Summarize());
                return null;
            }
        }
#if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        private static void GetMinMaxFrom(double[,] coordinates, int len, out double _minX, out double _minY, out double _maxX, out double _maxY)
        {
            _minX = coordinates[0, 0];
            _maxX = coordinates[0, 0];
            _minY = coordinates[0, 1];
            _maxY = coordinates[0, 1];
            for (int k = 1; k < len; k++)
            {
                _minX = Math.Min(coordinates[k, 0], _minX);
                _maxX = Math.Max(coordinates[k, 0], _maxX);
                _minY = Math.Min(coordinates[k, 1], _minY);
                _maxY = Math.Max(coordinates[k, 1], _maxY);
            }
        }

#if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        private static MultiPolygon GetMultiPolygon(JArray obj)
        {
            int c = obj.Count;
            Polygon[] polys = new Polygon[c];
            for (int i = 0; i < c; i++)
            {
                polys[i] = GetPolygon((JArray)obj[i]);
            }
            return new() { Polys = polys };
        }

#if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        private static Polygon GetPolygon(JArray obj)
        {
            int count = obj.Count;
            double[][,] coords = new double[count][,];
            double[] minX = new double[count];
            double[] maxX = new double[count];
            double[] minY = new double[count];
            double[] maxY = new double[count];
            int[] lengths = new int[count];

            for (int i = 0; i < count; i++)
            {
                int c = obj[i].Count();
                coords[i] = new double[c, 2];
                for (int j = 0; j < c; j++)
                {
                    double[] a = obj[i][j].Select(t => (double)t).ToArray();
                    coords[i][j, 0] = a[0];
                    coords[i][j, 1] = a[1];
                }

                GetMinMaxFrom(coords[i], c, out double _minX, out double _minY, out double _maxX, out double _maxY);

                lengths[i] = c;
                minX[i] = _minX;
                maxX[i] = _maxX;
                minY[i] = _minY;
                maxY[i] = _maxY;
            }

            return new() { Coords = coords, len = lengths, maxX = maxX, maxY = maxY, minX = minX, minY = minY };
        }

        private static string GetUnknownAlphaForPoint(double[] point)
        {
            return point[1] <= -60D ? ISO3166Helper.AntarcticaAlpha2 : string.Empty;
        }

        private static Coordinates GetRandomPointWithinBoundBox(Polygon poly)
        {
            const double shrink = 0.15;
            const double minDiff = 0.0001;
            double diffY = poly.maxY[0] - poly.minY[0];
            double diffX = poly.maxX[0] - poly.minX[0];
            double minY = poly.minY[0] + (diffY * shrink);
            double maxY = poly.maxY[0] - (diffY * shrink);
            double minX = poly.minX[0] + (diffX * shrink);
            double maxX = poly.maxX[0] - (diffX * shrink);
            int x = random.Next(Convert.ToInt32(Math.Ceiling((maxX - minX) / minDiff)));
            int y = random.Next(Convert.ToInt32(Math.Ceiling((maxY - minY) / minDiff)));
            return new(minY + (minDiff * y), minX + (minDiff * x));
        }

        private static Dictionary<Feature, Tuple<Polygon[], int>> GetRandomPointWithinBoundBoxCache { get; set; } = new();

        /// <summary>
        /// Get a random point within one of given feature's polygon's bounding box
        /// <para>WARNING: This DOES NOT guarantee that the point will be within the polygon</para>
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static Coordinates GetRandomPointWithinBoundBox(Feature feature)
        {
            if (feature == null)
            {
                return new();
            }

            if (feature.coords is Polygon poly)
            {
                return GetRandomPointWithinBoundBox(poly);
            }
            else if (feature.coords is MultiPolygon mp && mp.Polys.Length > 0)
            {
                if (mp.Polys.Length == 1)
                {
                    return GetRandomPointWithinBoundBox(mp.Polys[0]);
                }

                Tuple<Polygon[], int> tuple = GetRandomPointWithinBoundBoxCache[feature];

                int r = random.Next(0, tuple.Item2);
                int i = mp.Polys.Length - 1;

                while (r > 0 && i > 0)
                {
                    r -= tuple.Item1[--i].len[0];
                }
                return GetRandomPointWithinBoundBox(tuple.Item1[i]);
            }

            return new();
        }

        /// <summary>
        /// Get given <paramref name="countryNameOrCode"/>'s <see cref="GeoJson"/>
        /// </summary>
        /// <param name="countryNameOrCode">Country name or code</param>
        /// <returns></returns>
        public static GeoJson GetCountry(string countryNameOrCode)
        {
            return BorderData.FirstOrDefault(g => g.features.Any(f => f.properties.shapeName == countryNameOrCode || f.properties.shapeISO == countryNameOrCode || f.properties.shapeGroup == countryNameOrCode));
        }

        private static int GetFeatureFactor(GeoJson geo)
        {
            return SpecialFactorForCountry.ContainsKey(geo.features[0].properties.shapeGroup)
                ? SpecialFactorForCountry[geo.features[0].properties.shapeGroup]
                : Convert.ToInt32(Math.Max(1, Math.Ceiling(geo.features.Count / FeatureFactor)));
        }

        private const double FeatureFactor = 2D;

        private static Dictionary<string, int> SpecialFactorForCountry { get; set; } = new();

        private static int GetRandomCountryCacheTotal { get; set; } = -1;
        private static GeoJson[] GetRandomCountryCacheBorder { get; set; }

        /// <summary>
        /// Get a random country (including Antarctica)
        /// </summary>
        /// <returns></returns>
        public static GeoJson GetRandomCountry()
        {
            if (GetRandomCountryCacheTotal == -1)
            {
                GetRandomCountryCacheTotal = BorderData.Select(GetFeatureFactor).Sum();
            }

            if (GetRandomCountryCacheBorder == null)
            {
                GetRandomCountryCacheBorder = BorderData.OrderByDescending(GetFeatureFactor).ToArray();
            }

            int i = BorderData.Length - 1;
            int r = random.Next(0, GetRandomCountryCacheTotal);
            while (r > 0 && i > 0)
            {
                GeoJson g = GetRandomCountryCacheBorder[--i];
                r -= GetFeatureFactor(g);
            }
            return GetRandomCountryCacheBorder[i];
        }

        /// <summary>
        /// Get GeoJson of given country code or name
        /// </summary>
        /// <returns></returns>
        public static GeoJson GetGeoJSONOf(string codeOrName)
        {
            codeOrName = codeOrName?.Trim().ToUpperInvariant();
            return BorderData
                .FirstOrDefault(b => b.features
                    .Any(f => f.properties.shapeGroup == codeOrName 
                        || f.properties.shapeISO == codeOrName 
                        || f.properties.shapeName.ToUpperInvariant() == codeOrName));
        }

        /// <summary>
        /// Get a random feature from given country
        /// </summary>
        /// <param name="geo">Country GeoJson object</param>
        /// <returns></returns>
        public static Feature GetRandomFeature(GeoJson geo)
        {
            GCUtils.ThrowIfNull(geo);
            return geo.features[random.Next(geo.features.Count)];
        }

        /// <summary>
        /// See <see cref="GetRandomPointCloseOrWithinAPolygon(out Feature)"/>
        /// </summary>
        /// <returns></returns>
        public static Coordinates GetRandomPointCloseOrWithinAPolygon()
        {
            return GetRandomPointCloseOrWithinAPolygon(out Feature _);
        }

        /// <summary>
        /// Get a random point within ANY polygon's bounding box present in current border set
        /// <para>WARNING: This DOES NOT guarantee that the point will be within a polygon</para>
        /// </summary>
        /// <param name="feature">Randomly picked feature</param>
        /// <returns></returns>
        public static Coordinates GetRandomPointCloseOrWithinAPolygon(out Feature feature)
        {
            feature = GetRandomFeature(GetRandomCountry());
            return GetRandomPointWithinBoundBox(feature);
        }

        /// <summary>
        /// Get a random point within given country polygon's bounding box present in current border set
        /// <para>WARNING: This DOES NOT guarantee that the point will be within a polygon</para>
        /// </summary>
        /// <param name="codeOrName">Randomly picked feature</param>
        /// <param name="feature">Randomly picked feature</param>
        /// <returns></returns>
        public static Coordinates GetRandomPointCloseOrWithin(string codeOrName, out Feature feature)
        {
            feature = GetRandomFeature(GetGeoJSONOf(codeOrName));
            return GetRandomPointWithinBoundBox(feature);
        }

        /// <summary>
        /// Get alpha3 code from alpha2 code or name
        /// </summary>
        /// <param name="codeOrName"></param>
        /// <returns></returns>
        public static string GetAlpha3FromCodeOrName(string codeOrName)
        {
            if (string.IsNullOrWhiteSpace(codeOrName))
            {
                return string.Empty;
            }

            string match = codeOrName.ToUpperInvariant();
            if (match.Length == 2)
                match = ISO3166Helper.FromAlpha2(match)?.Alpha3;
            else
                match = ISO3166Helper.Collection.FirstOrDefault(c => c.Name.ToUpperInvariant() == match)?.Alpha3;

            return match ?? codeOrName;
        }

        /// <summary>
        /// Get random coordinates around given areas
        /// </summary>
        /// <param name="randomGuessQuery"><code>targetCountryNameOrCode</code> OR <code>target1:relativeProbability1 target2:relativeProbability2 ...</code></param>
        /// <returns></returns>
        public static Coordinates GetRandomCoordinateFromRandomGuessQuery(string randomGuessQuery)
        {
            Coordinates rand = null;
            try
            {
                if (string.IsNullOrWhiteSpace(randomGuessQuery))
                {
                    return rand;
                }

                // TODO: Refactor
                string[] countryArgs = randomGuessQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (countryArgs.Length > 0)
                {

                    if (countryArgs.Length == 1)
                    {
                        string match = GetAlpha3FromCodeOrName(countryArgs[0].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0]);
                        rand = GetRandomPointCloseOrWithin(match, out Feature _);
                    }
                    else
                    {
                        List<string> matches = countryArgs
                            .Select(c => c.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0])
                            .ToList();

                        List<double> probs = countryArgs
                            .Select(c =>
                            {
                                string[] splt = c.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                                if (splt.Length == 1)
                                {
                                    return 1;
                                }
                                return splt[1].ParseAsDouble(1);
                            })
                            .ToList();

                        double totalProb = probs.Sum();

                        int i = matches.Count - 1;
                        double r = random.NextDouble() * totalProb;
                        while (r > 0 && i >= 0)
                        {
                            Coordinates old = rand;
                            string match = GetAlpha3FromCodeOrName(matches[i]);
                            rand = GetRandomPointCloseOrWithin(match, out Feature _);

                            if (rand == null)
                            {
                                rand = old;
                            }

                            r -= probs[i--];
                        }

                        if (rand == null)
                        {
                            foreach (string m in matches)
                            {
                                string match = GetAlpha3FromCodeOrName(m);
                                rand = GetRandomPointCloseOrWithin(match, out Feature _);
                                if (rand != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                return rand;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return rand;
            }
        }

        /// <summary>
        /// Get location information of given coordinates <paramref name="point"/>
        /// </summary>
        /// <param name="point">Coordinates of the location as { Longitude, Latitude }</param>
        /// <param name="featureBase">Country parent containing <paramref name="point"/> or <see langword="null"/></param>
        /// <param name="hitFeature">Feature hit in <paramref name="featureBase"/> or <see langword="null"/></param>
        /// <param name="hitPolygon">Multi-part (or single) polygon hit in <paramref name="hitFeature"/> or <see langword="null"/></param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
#if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        public static string GetFeatureHitBy(double[] point, out GeoJson featureBase, out Feature hitFeature, out Polygon hitPolygon)
        {
            featureBase = null;
            hitFeature = null;
            hitPolygon = null;

            if (point == null)
            {
                return string.Empty;
            }

            int tries = 0;
            while (Initializing && tries < 20)
            {
                Thread.Sleep(250);
                tries++;
            }

            if (tries != 0 && ReaderTaskSuccessCount < ReaderTaskCount)
            {
                throw new IOException("Failed to process geojson data... Please restart the application");
            }

            for (int i = 0; i < BorderData.Length; i++)
            {
                GeoJson geo = BorderData[i];
                for (int j = 0; j < geo.features.Count; j++)
                {
                    Feature feat = geo.features[j];
                    if (feat.coords is Polygon p)
                    {
                        if (IsInPolygon(point, p))
                        {
                            featureBase = geo;
                            hitFeature = feat;
                            hitPolygon = p;
                            return ISO3166Helper.FromAlpha3(geo.features[0].properties.shapeGroup)?.Alpha2;
                        }
                    }
                    else if (feat.coords is MultiPolygon mp)
                    {
                        int len = mp.Polys.Length;
                        for (int pm = 0; pm < len; pm++)
                        {
                            if (IsInPolygon(point, mp.Polys[pm]))
                            {
                                featureBase = geo;
                                hitFeature = feat;
                                hitPolygon = mp.Polys[pm];
                                return ISO3166Helper.FromAlpha3(geo.features[0].properties.shapeGroup)?.Alpha2;
                            }
                        }
                    }
                }
            }
            return GetUnknownAlphaForPoint(point);
        }

        /// <summary>
        /// If point <paramref name="p"/> is in <paramref name="poly"/>, returns <see langword="true"/>, otherwise <see langword="false"/>.
        /// </summary>
        /// <param name="p">Point</param>
        /// <param name="poly">Polygon</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Should never be null")]
#if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        public static bool IsInPolygon(double[] p, Polygon poly)
        {
            int len = poly.Coords.Length;
            if (IsInPolygon(p, poly.Coords[0], poly.len[0], poly.minX[0], poly.minY[0], poly.maxX[0], poly.maxY[0]))
            {
                // Check holes
                for (int i = 1; i < len; i++)
                {
                    if (IsInPolygon(p, poly.Coords[i], poly.len[i], poly.minX[i], poly.minY[i], poly.maxX[i], poly.maxY[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// If point <paramref name="p"/> is in <paramref name="polygon"/>, returns <see langword="true"/>, otherwise <see langword="false"/>.
        /// </summary>
        /// <param name="p">Point</param>
        /// <param name="polygon">Polygon</param>
        /// <param name="length">Point count in <paramref name="polygon"/> (Rows, First dimension)</param>
        /// <param name="minX">Minimum x value of <paramref name="polygon"/> edges</param>
        /// <param name="minY">Minimum y value of <paramref name="polygon"/> edges</param>
        /// <param name="maxX">Maximum x value of <paramref name="polygon"/> edges</param>
        /// <param name="maxY">Maximum y value of <paramref name="polygon"/> edges</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Should never be null")]
#if !DEBUG
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        public static bool IsInPolygon(double[] p, double[,] polygon, int length, double minX, double minY, double maxX, double maxY)
        {
            double x = p[0];
            double y = p[1];

            if (x < minX || x > maxX || y < minY || y > maxY)
            {
                return false;
            }

            // https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int i = 0, j = length - 1; i < length; j = i++)
            {
                if ((polygon[i, 1] > y) != (polygon[j, 1] > y) &&
                     x < ((polygon[j, 0] - polygon[i, 0]) * (y - polygon[i, 1]) / (polygon[j, 1] - polygon[i, 1])) + polygon[i, 0])
                {
                    inside = !inside;
                }
            }

            return inside;
        }
    }
}

#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional