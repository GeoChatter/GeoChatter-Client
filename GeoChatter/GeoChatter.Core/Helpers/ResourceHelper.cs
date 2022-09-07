using GeoChatter.Helpers;
using GeoChatter.Core.Model;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using GeoChatter.Core.Common.Extensions;
using System.Windows.Forms;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// Helper methods and constants for resources
    /// </summary>
    public static class ResourceHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ResourceHelper));

        private static RestClient restClient { get; } = new();

        /// <summary>
        /// Resource service URL
        /// </summary>
        public const string ResourceServiceURL = "https://service.geochatter.tv/resources/";

        /// <summary>
        /// Script service URL
        /// </summary>
        public const string ScriptServiceURL = ResourceServiceURL + "scripts";

        /// <summary>
        /// Script service URL
        /// </summary>
        public const string StyleServiceURL = ResourceServiceURL + "styles";

        /// <summary>
        /// Versioned script service URL
        /// </summary>
        public static string VersionedScriptServiceURL(string version) => ScriptServiceURL + "/" + version;

        /// <summary>
        /// Versioned style service URL
        /// </summary>
        public static string VersionedStyleServiceURL(string version) => StyleServiceURL + "/" + version;

        /// <summary>
        /// Other resource files service URL
        /// </summary>
        public const string OtherServiceURL = ResourceServiceURL + "other";

        /// <summary>
        /// Resource meta file name
        /// </summary>
        public const string MetaFile = "meta.json";

        /// <summary>
        /// Resource content file name
        /// </summary>
        public const string ContentArchive = "content.zip";

        /// <summary>
        /// Resource sub directory names of helper classes
        /// <para>Values to append to <see cref="ResourceServiceURL"/> url for getting resource root</para>
        /// </summary>
        public static Dictionary<Type, string> HelperTypeResourceDirectories { get; } = new()
        {
            { typeof(CountryHelper), "flags" },
            { typeof(BorderHelper), "borders" },
        };

        /// <summary>
        /// Make a GET request to <paramref name="url"/> and invoke <paramref name="callbackSuccess"/> on success
        /// </summary>
        /// <param name="url"></param>
        /// <param name="callbackSuccess"></param>
        /// <param name="callbackFail"></param>
        /// <returns></returns>
        public static bool GetWebResource(string url, Action<string> callbackSuccess, Action<string, Exception> callbackFail)
        {
            GCUtils.ThrowIfNullEmptyOrWhiteSpace(url);
            GCUtils.ThrowIfNull(callbackSuccess);
            GCUtils.ThrowIfNull(callbackFail);
            try
            {
                RestRequest req = new(url, Method.Get);
                req.AddHeader("Accept", "*/*");
                RestResponse res = restClient.Execute(req);
                if (res.IsSuccessful)
                {
                    callbackSuccess(res.Content);
                    return true;
                }
                else
                {
                    callbackFail(res.ErrorMessage, res.ErrorException);
                    return false;
                }
            }
            catch (Exception ex)
            {
                callbackFail(string.Empty, ex);
                return false;
            }
        }

        /// <summary>
        /// Get meta data for given helper's target resource defined in <see cref="HelperTypeResourceDirectories"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ResourceMeta GetResourceMeta(Type type)
        {
            try
            {
                GCUtils.ThrowIfNull(type);

                RestRequest req = new(Path.Combine(ResourceServiceURL, HelperTypeResourceDirectories[type], MetaFile), Method.Get) { RequestFormat = DataFormat.Json };
                RestResponse res = restClient.Execute(req);

                return JsonConvert.DeserializeObject<ResourceMeta>(res.Content);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return null;
            }
        }

        /// <summary>
        /// Check if client has the latest resource defined by <paramref name="meta"/>
        /// </summary>
        /// <param name="meta">Most recent meta data</param>
        /// <returns></returns>
        public static bool HasLatestResource(ResourceMeta meta)
        {
            try
            {
                GCUtils.ThrowIfNull(meta);

                if (!Directory.Exists(meta.target)
                    || !File.Exists(Path.Combine(meta.target, MetaFile)))
                {
                    return false;
                }

                ResourceMeta installedMeta = JsonConvert.DeserializeObject<ResourceMeta>(File.ReadAllText(Path.Combine(meta.target, MetaFile)));
                if (!Directory.Exists(meta.target+"\\"+meta.content))
                    ResourceHelper.InstallLatest(meta);
                return installedMeta != null && installedMeta.version == meta.version;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return false;
            }
        }

        /// <summary>
        /// Install latest resource content
        /// </summary>
        /// <param name="meta"></param>
        /// <returns></returns>
        public static void InstallLatest(ResourceMeta meta)
        {
            try
            {
                GCUtils.ThrowIfNull(meta);

                RestRequest req = new(Path.Combine(ResourceServiceURL, meta.name, ContentArchive), Method.Get);
                req.AddOrUpdateHeader("Accept", "*/*");
                byte[] res = restClient.DownloadData(req);

                GCUtils.ThrowIfNull(res);

                string targetDir = Path.Combine(Path.GetTempPath(), "GeoChatter", meta.name);
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                string targetpath = Path.Combine(targetDir, Guid.NewGuid() + ContentArchive);
                File.WriteAllBytes(targetpath, res);

                FastZip fastZip = new();

                fastZip.ExtractZip(targetpath, targetDir, null);

                if (!Directory.Exists(meta.target))
                {
                    Directory.CreateDirectory(meta.target);
                }
                else
                {
                    Directory.Delete(meta.target, true);
                    Directory.CreateDirectory(meta.target);
                }

                foreach (string newPath in Directory.GetFiles(targetDir, "*.*", SearchOption.AllDirectories))
                {
                    if (newPath != targetpath)
                    {
                        FileInfo file = new(newPath.ReplaceDefault(targetDir, meta.target));
                        file.Directory.Create();
                        File.Copy(newPath, file.FullName, true);
                    }
                }

                File.WriteAllText(Path.Combine(meta.target, MetaFile), JsonConvert.SerializeObject(meta));
                File.Delete(targetpath);
                Directory.Delete(targetDir, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

    }
}
