using GeoChatter.Core.Common.Extensions;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GeoChatter.Core.Helpers
{

    /// <summary>
    /// Representation of an ISO3166-1 Country
    /// </summary>
    public sealed class ISO3166Country
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alpha2"></param>
        /// <param name="alpha3"></param>
        /// <param name="numericCode"></param>
        public ISO3166Country(string name, string alpha2, string alpha3, int numericCode)
        {
            Name = name;
            Alpha2 = alpha2;
            Alpha3 = alpha3;
            NumericCode = numericCode;
        }

        /// <summary>
        /// Country or region name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 2 letter ISO code or custom code
        /// </summary>
        public string Alpha2 { get; private set; }

        /// <summary>
        /// 3 letter ISO code or custom code
        /// </summary>
        public string Alpha3 { get; private set; }

        /// <summary>
        /// Numeric ISO code or -1 for unknown
        /// </summary>
        public int NumericCode { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} ({Alpha2}) ({Alpha3})";
        }
    }

    /// <summary>
    /// Helper for ISO codes and names
    /// </summary>
    public static class ISO3166Helper
    {
        /// <summary>
        /// Antarctica ISO3166 2 letter alpha code
        /// </summary>
        public static string AntarcticaAlpha2 => Collection.FirstOrDefault(i => i.Name == "Antarctica")?.Alpha2 ?? "AQ";

        /// <summary>
        /// ISO json file
        /// </summary>
        public static string ISOFile => "iso.json";

        private static readonly ILog logger = LogManager.GetLogger(typeof(ISO3166Helper));
        private static ICollection<ISO3166Country> collection = new List<ISO3166Country>();

        private static RestClient restClient { get; } = new();

        private static bool Initialized { get; set; }

        /// <summary>
        /// Obtain ISO3166-1 Country based on its alpha3 code.
        /// </summary>
        /// <param name="alpha3"></param>
        /// <returns></returns>
        public static ISO3166Country FromAlpha3(string alpha3)
        {
            return Collection.FirstOrDefault(p => p.Alpha3 == alpha3);
        }

        /// <summary>
        /// Obtain ISO3166-1 Country based on its alpha2 code.
        /// </summary>
        /// <param name="alpha2"></param>
        /// <returns></returns>
        public static ISO3166Country FromAlpha2(string alpha2)
        {
            return Collection.FirstOrDefault(p => p.Alpha2 == alpha2);
        }

        /// <summary>
        /// Collection of built in ISO data
        /// </summary>
        public static ICollection<ISO3166Country> Collection
        {
            get
            {
                if (!Initialized)
                {
                    Initialize();
                }
                return collection;
            }
            private set => collection = value;
        }
        /// <summary>
        /// Initializer
        /// </summary>
        public static void Initialize()
        {
            Initialized = true;
            logger.Info("Initializing ISO3166 helper");
            try
            {
                RestRequest req = new(Path.Combine(ResourceHelper.OtherServiceURL, ISOFile), Method.Get) { RequestFormat = DataFormat.Json };
                RestResponse res = restClient.Execute(req);

                if (res.IsSuccessful)
                {
                    logger.Debug($"GET {ISOFile} done");
                    Collection = JsonConvert.DeserializeObject<ICollection<ISO3166Country>>(res.Content);
                    logger.Info($"Initialized ISO collection with {Collection.Count} entries");
                }
                else
                {
                    logger.Error($"GET {ISOFile} failed({res.ErrorMessage}): {res.ErrorException?.Summarize()}");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }
    }
}
