using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Forms.FlagManager
{
    /// <summary>
    /// Dialog to install flag packs automatically
    /// </summary>
    public partial class FlagManagerOfficialPacksDialog : Form
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(FlagManagerOfficialPacksDialog));
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Disposed on form closing")]
        private readonly RestClient restClient = new();

        private const string FlagPackService = "https://service.geochatter.tv/flagpacks/";
        private const string FlagPackNamesFile = "names.json";
        private class FlagPackNameContainer
        {
            public Dictionary<string, string> packs { get; set; } = new();
        }

        private FlagManagerDialog parent { get; }

        /// <summary>
        /// 
        /// </summary>
        public FlagManagerOfficialPacksDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public FlagManagerOfficialPacksDialog(FlagManagerDialog mgr) : this()
        {
            SetLists();
            parent = mgr;
        }

        private FlagPackNameContainer container { get; set; }

        private void SetLists()
        {
            flagPackNamesCB.Items.Clear();
            listView1.Items.Clear();

            container = GetNames();
            if (container != null && container.packs.Count > 0)
            {
                foreach (KeyValuePair<string, string> name in container.packs)
                {
                    if (FlagPackHelper.FlagPacks.FirstOrDefault(p => p.Name.ToLowerInvariant() == name.Value.ToLowerInvariant()) == null)
                    {
                        flagPackNamesCB.Items.Add(name.Value);
                    }
                    else
                    {
                        listView1.Items.Add(name.Value);
                    }
                }
            }
        }

        private FlagPackNameContainer GetNames()
        {
            try
            {
                RestRequest req = new(FlagPackService + FlagPackNamesFile, Method.Get);
                RestResponse res = restClient.Execute(req);

                if (res.IsSuccessful && res.Content.Length > 0)
                {
                    return JsonConvert.DeserializeObject<FlagPackNameContainer>(res.Content);
                }

                logger.Warn($"Flag pack names getter failed with ({res.StatusCode}): {res.ErrorMessage}");
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return null;
            }
        }

        private void InstallPack(string name)
        {
            if (parent == null)
            {
                return;
            }

            try
            {
                RestRequest req = new(GetAsFlagPackURL(name), Method.Get);
                req.AddOrUpdateHeader("Accept", "*/*");
                byte[] res = restClient.DownloadData(req);
                if (res.Length > 0)
                {
                    logger.Info($"Installing flag pack '{name}'...");

                    string fname = Path.ChangeExtension(name, ".zip");
                    string targetDir = Path.Combine(Path.GetTempPath(), "GeoChatter");
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    string targetpath = Path.Combine(targetDir, fname);
                    File.WriteAllBytes(targetpath, res);

                    FlagPack pack = null;
                    FlagManagerDialog.InstallFlagPack(parent, targetpath, false, ref pack);

                    if (pack == null)
                    {
                        logger.Error($"Failed to install flag pack '{name}'");
                    }
                    else
                    {
                        //while (!parent.loadPacksWorkerDone)
                        //{
                        //    System.Threading.Thread.Sleep(200);
                        //}
                        logger.Info($"Installed flag pack '{name}'");
                        SetLists();
                    }
                }
                else
                {
                    logger.Warn($"Flag pack download failed for '{name}'");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        private string GetAsFlagPackURL(string name)
        {
            return $"{FlagPackService}{container.packs.FirstOrDefault(p => p.Value.ToLowerInvariant() == name.ToLowerInvariant()).Key}.zip";
        }

        private void InstallPacks(params string[] names)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            try
            {
                foreach (string item in names)
                {
                    InstallPack(item);
                }
            }
            finally
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InstallPacks(flagPackNamesCB.CheckedItems.Cast<string>().ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InstallPacks(flagPackNamesCB.Items.Cast<string>().ToArray());
        }

        private void FlagManagerOfficialPacksDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            restClient?.Dispose();
        }
    }
}
