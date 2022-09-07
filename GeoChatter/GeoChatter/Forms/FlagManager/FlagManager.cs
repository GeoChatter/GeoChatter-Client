using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.FormUtils;
using GeoChatter.Properties;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using Newtonsoft.Json;
using Svg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GeoChatter.Model;
using GeoChatter.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Drawing;
using System.Diagnostics.Metrics;

namespace GeoChatter.Forms.FlagManager
{
    /// <summary>
    /// Flag manager
    /// </summary>
    public partial class FlagManagerDialog : Form
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(FlagManagerDialog));

        /// <summary>
        /// Wheter any installation or uninstallation was made
        /// </summary>
        public bool HasChanged { get; set; }

        /// <summary>
        /// Wheter to load default flagsB
        /// </summary>
        public bool Loaddefault { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FlagManagerDialog(bool loaddefault)
        {
            Loaddefault = loaddefault;
        }

        /// <summary>
        /// 
        /// </summary>
        public FlagManagerDialog()
        {
            InitializeComponent();
        }

        private readonly MainForm parent;

        /// <summary>
        /// 
        /// </summary>
        public FlagManagerDialog(MainForm mainform) : this()
        {
            parent = mainform;
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
            parent?.ResetJSCTRLCheck();
#pragma warning restore CA1416 // Validate platform compatibility
#endif
        }

        private BackgroundWorker loadPacksWorker = new();
        private ProgressWindow progrsswin = new();

        internal bool loadPacksWorkerDone { get; private set; }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (progrsswin != null && progrsswin.Visible)
            {
                loadPacksWorkerDone = true;
                progrsswin.Dispose();
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progrsswin != null)
            {
                progrsswin.SetProgress(e.UserState?.ToString(), e.ProgressPercentage);
            }
        }

        private void FlagManager_Load(object sender, EventArgs e)
        {
            //  CreateColumns(treeList1);
           // InitializeWorkers();
            PopulateTreelist();
        }

        private void InitializeWorkers()
        {
            loadPacksWorkerDone = false;

            loadPacksWorker?.Dispose();
            loadPacksWorker = new();

            loadPacksWorker.DoWork += PopulateSubNodes;
            loadPacksWorker.ProgressChanged += Worker_ProgressChanged;
            loadPacksWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            loadPacksWorker.WorkerReportsProgress = true;

            progrsswin?.Dispose();
            progrsswin = new ProgressWindow("Loading flags", "Preparing...");
            progrsswin.TopMost = true;

            
        }

        private TreeNode rootnode, customNode;
        private delegate TreeNode AddNodeCallBack(TreeNodeCollection parent, string title);

        private TreeNode AddNode(TreeNodeCollection parent, string title)
        {
            if (treeView1.InvokeRequired)
            {
                AddNodeCallBack d = new(AddNode);
                return Invoke(d, parent, title) as TreeNode;
            }
            else
            {
                rootnode = parent.Add(title);
                return rootnode;
            }
        }

        private delegate void AddImagesCallBack();

        private void AddImages()
        {
            if (treeView1.InvokeRequired)
            {
                AddImagesCallBack d = new(AddImages);
                Invoke(d);
            }
            else
            {
                treeView1.ImageList = coll1;

            }
        }


        private delegate void ClearTreeviewCallBack();

        private void ClearTreeview()
        {
            if (treeView1.InvokeRequired)
            {
                ClearTreeviewCallBack d = new(ClearTreeview);
                Invoke(d);
            }
            else
            {
                treeView1.Nodes.Clear();

            }
        }

        private delegate void ClearMapCtrlCallBack();

        private void ClearMapCtrl()
        {
            if (treeView1.InvokeRequired)
            {
                ClearMapCtrlCallBack d = new(ClearMapCtrl);
                Invoke(d);
            }
            else
            {
                grpMapPack.Controls.Clear();

            }
        }
        private delegate void SetImageIndexCallBack(TreeNode flagnode, int index);

        private void SetImageIndex(TreeNode flagnode, int index)
        {
            if (treeView1.InvokeRequired)
            {
                SetImageIndexCallBack d = new(SetImageIndex);
                Invoke(d, flagnode, index);
            }
            else
            {
                flagnode.ImageIndex = index;
                flagnode.SelectedImageIndex = index;

            }
        }

        private static void FixInvalidSvgAttributes(string name)
        {
            try
            {
                File.WriteAllText(name, File.ReadAllText(name).ReplaceDefault(" height=\"px\"", ""));
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        private void PopulateTreelist()
        {
            string iconsmaller = Path.ChangeExtension(nameof(Resources.icon), ".ico");

            ComponentResourceManager resources = new(typeof(FlagManagerDialog));
            coll1 = new ImageList
            {
                ColorDepth = ColorDepth.Depth8Bit,
                ImageStream = (ImageListStreamer)resources.GetObject("coll1.ImageStream", CultureInfo.InvariantCulture),
                TransparentColor = System.Drawing.Color.Transparent
            };
            coll1.Images.SetKeyName(0, iconsmaller);
            ClearMapCtrl();
            ClearTreeview();
            coll1 = new ImageList
            {
                ColorDepth = ColorDepth.Depth8Bit,
                ImageStream = (ImageListStreamer)resources.GetObject("coll1.ImageStream", CultureInfo.InvariantCulture),
                TransparentColor = System.Drawing.Color.Transparent
            };
            coll1.Images.SetKeyName(0, iconsmaller);

            int index = 1;
            foreach (FlagPack pack in FlagPackHelper.FlagPacks)
            {
                //foreach (KeyValuePair<string, string> pair in pack.Flags)
                //{
                //    SvgDocument img = new();

                //    FileInfo info = new(Path.Combine(FlagPackHelper.FlagsDirectory, pack.Name, Path.ChangeExtension(pair.Value, ".svg")));
                //    FixInvalidSvgAttributes(info.FullName);

                //    if (info.Exists)
                //    {

                //        img = SvgDocument.Open(info.FullName);
                //        System.Drawing.Bitmap bit = img.Draw();
                //        coll1.Images.Add(pair.Key, bit);
                //        index++;
                //    }
                //    counter++;
                //    double percentage = counter / (Convert.ToDouble(flagCount) / 100);
                //    loadPacksWorker.ReportProgress(Convert.ToInt32(percentage), $"{counter} of {flagCount}");
                //}
            }

            TreeNode localRootNode = AddNode(treeView1.Nodes, "Flags");

            if (Loaddefault)
            {
                foreach (KeyValuePair<string, NameMapping> keyValuePair in CountryHelper.MappedDefaultNames)
                {
                    try
                    {
                        SvgDocument img = new();

                        FileInfo info = new(Path.Combine(CountryHelper.FlagMeta.TargetDirectory, Path.ChangeExtension(keyValuePair.Value.Code, ".svg")));
                        if (info.Exists)
                        {
                            img = SvgDocument.Open(info.FullName);
                            System.Drawing.Bitmap bit = img.Draw();
                            coll1.Images.Add(keyValuePair.Key, bit);
                            index++;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Summarize());
                    }
                }

                TreeNode defaultNode = AddNode(localRootNode.Nodes, "Default");

                foreach (KeyValuePair<string, NameMapping> keyValuePair in CountryHelper.MappedDefaultNames)
                {
                    TreeNode flagnode = defaultNode.Nodes.Add(Path.Combine(keyValuePair.Key, keyValuePair.Value.Code));
                    SetImageIndex(flagnode, coll1.Images.IndexOfKey(keyValuePair.Key));
                    flagnode.Tag = keyValuePair;
                }
            }
            customNode = AddNode(rootnode.Nodes, "Flag packs");
            foreach (FlagPack pack in FlagPackHelper.FlagPacks)
            {
                TreeNode packnode = AddNode(customNode.Nodes, pack.Name);

                packnode.Tag = pack;
                foreach (KeyValuePair<string, string> keyValuePair in pack.Flags)


                {
                    TreeNode flagnode = AddNode(packnode.Nodes, Path.Combine(keyValuePair.Key, keyValuePair.Value));
                    flagnode.Name = flagnode.Text;
                  //  SetImageIndex(flagnode, coll1.Images.IndexOfKey(keyValuePair.Key));
                    flagnode.Tag = pack;

                }
            }
            AddImages();

        }

        private void installPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InstallFlagPack();
        }

        internal static void InstallFlagPack(FlagManagerDialog mgr, string zipfilename, bool validate, ref FlagPack result)
        {
            string finalDir = string.Empty;
            string targetDir = string.Empty;
            try
            {
                FastZip fastZip = new();
                string fileFilter = null;
                targetDir = Path.Combine(Path.GetTempPath(), "GeoChatter");
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                // Will always overwrite if target filenames already exist
                fastZip.ExtractZip(zipfilename, targetDir, fileFilter);

                if (JsonConvert.DeserializeObject<FlagPack>(File.ReadAllText(Path.Combine(targetDir, FlagPackHelper.FlagPackJSON))) is FlagPack flagPack)
                {
                    DialogResult dr = validate
                        ? MessageBox.Show($"Are you sure you want to install the flag pack '{flagPack.Name}'?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                        : DialogResult.Yes;

                    if (dr == DialogResult.Yes)
                    {

                        string existingFlag = string.Empty;
                        foreach (KeyValuePair<string, string> flag in flagPack.Flags)
                        {
                            if (CountryHelper.MappedDefaultNames.ContainsKey(flag.Value))
                            {
                                existingFlag += flag.Value + ",";
                            }

                            if (CountryHelper.MappedDefaultNames.ContainsKey(flag.Key))
                            {
                                existingFlag += flag.Key + ",";
                            }

                            if (CountryHelper.MappedDefaultNames.Values.Any(n => n.Code == flag.Value))
                            {
                                existingFlag += flag.Value + ",";
                            }

                            if (CountryHelper.MappedDefaultNames.Values.Any(n => n.Code == flag.Key))
                            {
                                existingFlag += flag.Key + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(existingFlag))
                        {
                            existingFlag = existingFlag.TrimEnd(',');
                            if (MessageBox.Show($"The flag pack will overwrite at least one existing flag: {existingFlag}. Are you sure you want to install this flag pack?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                            {
                                return;
                            }
                        }
                        result = flagPack;

                        finalDir = Path.Combine(FlagPackHelper.FlagsDirectory, flagPack.Name);
                        if (!Directory.Exists(finalDir))
                        {
                            Directory.CreateDirectory(finalDir);
                        }

                        foreach (string newPath in Directory.GetFiles(targetDir, "*.*", SearchOption.AllDirectories))
                        {
                            File.Copy(newPath, newPath.ReplaceDefault(targetDir, finalDir), true);
                        }

                        FlagPackHelper.LoadCustomFlagPack(finalDir);

                        mgr.InitializeWorkers();
                        mgr.HasChanged = true;
                        mgr.PopulateTreelist();
                    }
                }

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(finalDir) && Directory.Exists(finalDir))
                {
                    Directory.Delete(finalDir, true);
                }
                logger.Error(ex);
            }
            finally
            {
                Directory.Delete(targetDir, true);
            }
        }

        private void InstallFlagPack()
        {
            FlagPack pack = null;
            try
            {
                using OpenFileDialog fileDialog = new();
                fileDialog.Filter = "Zipped flag pack | *.zip";
                fileDialog.Title = "Select flag pack to install";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    InstallFlagPack(this, fileDialog.FileName, true, ref pack);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                MessageBox.Show("There was an error installing the flag pack: " + ex.Message);
                if (pack != null)
                {
                    DeleteFlagPack(pack);
                }
            }
        }

        private void deletePackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode?.Tag is FlagPack flagPack)
            {
                DeleteFlagPack(flagPack);

            }
        }

        private void DeleteFlagPack(FlagPack flagPack)
        {
            if (flagPack == null)
            {
                return;
            }

            DialogResult dr = MessageBox.Show($"Are you sure you want to uninstall the flag pack '{flagPack.Name}'?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly); ;
            if (dr == DialogResult.Yes)
            {
                string finalDir = Path.Combine(FlagPackHelper.FlagsDirectory, flagPack.Name);
                Directory.Delete(finalDir, true);
                FlagPackHelper.DeleteCSS(flagPack);
                FlagPackHelper.FlagPacks.Remove(flagPack);

                CountryHelper.LoadFlags();
                FlagPackHelper.LoadCustomFlagPacks();
            }
            InitializeWorkers();
            HasChanged = true;
           PopulateTreelist();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Parent == null)
            {
                e.Cancel = true;
            }
            else if (treeView1.SelectedNode.Parent == customNode)
            {
                deletePackToolStripMenuItem.Visible = true;
                installPackToolStripMenuItem.Visible = false;
            }
            else if (treeView1.SelectedNode.Parent == rootnode)
            {

                deletePackToolStripMenuItem.Visible = false;
                installPackToolStripMenuItem.Visible = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUnInstall_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Tag is not null and FlagPack)
            {
                DeleteFlagPack(((Button)sender).Tag as FlagPack);
                btnUnInstall.Text = "Install from .zip";
                btnUnInstall.Tag = null;

            }
            else
            {
                InstallFlagPack();
            }
        }

        private void linkLblDownloadPacks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GeneralPurposeUtils.OpenURL(Settings.Default.FlagpacksPageURL);
        }

        private void FlagManagerDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (loadPacksWorker != null)
            {
                loadPacksWorker.DoWork -= PopulateSubNodes;
                loadPacksWorker.ProgressChanged -= Worker_ProgressChanged;
                loadPacksWorker.RunWorkerCompleted -= Worker_RunWorkerCompleted;
                loadPacksWorker.Dispose();
                loadPacksWorker = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using FlagManagerOfficialPacksDialog diag = new(this);
            diag.ShowDialog();
            
        }

        private void FlagManagerDialog_Enter(object sender, EventArgs e)
        {
#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
            parent?.ResetJSCTRLCheck();
#pragma warning restore CA1416 // Validate platform compatibility
#endif
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            InitializeWorkers();


            loadPacksWorker.RunWorkerAsync(e.Node);

            progrsswin.ShowDialog();
        }


        private void PopulateSubNodes(object sender, DoWorkEventArgs e)
        {
            TreeNode parent = e.Argument as TreeNode;
            if (parent != null && parent.Tag is FlagPack pack)
            {   int counter = 0;
                int flagCount = pack.Flags.Count;
                double percentage = counter / (Convert.ToDouble(flagCount) / 100);
                loadPacksWorker.ReportProgress(Convert.ToInt32(percentage), $"{counter} of {flagCount}");
                foreach (KeyValuePair<string, string> pair in pack.Flags)
                {
                    SvgDocument img = new();

                    FileInfo info = new(Path.Combine(FlagPackHelper.FlagsDirectory, pack.Name, Path.ChangeExtension(pair.Value, ".svg")));
                    FixInvalidSvgAttributes(info.FullName);

                    if (info.Exists)
                    {

                        img = SvgDocument.Open(info.FullName);
                        System.Drawing.Bitmap bit = img.Draw();

                        AddImageToCollection(pair.Key, bit);

                    }
                    string text = Path.Combine(pair.Key, pair.Value);
                    TreeNode[] flagNode = parent.Nodes.Find(text, true);
                    if (flagNode != null)
                        SetImageIndex(flagNode[0], coll1.Images.IndexOfKey(pair.Key));
                    counter++;
                    percentage = counter / (Convert.ToDouble(flagCount) / 100);
                    loadPacksWorker.ReportProgress(Convert.ToInt32(percentage), $"{counter} of {flagCount}");
                }
            }
        }
        private delegate void AddImageToCollectionCallBack(string key, Bitmap bit);

        private void AddImageToCollection(string key, Bitmap bit)
        {
            if (treeView1.InvokeRequired)
            {
                AddImageToCollectionCallBack d = new(AddImageToCollection);
                Invoke(d, key, bit);
            }
            else
            {
                coll1.Images.Add(key, bit);

            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {


            if (e.Node.Tag is FlagPack pack)
            {
                grpMapPack.Controls.Clear();
                ctrlMapPack ctrl = new(pack)
                {
                    Dock = DockStyle.Fill
                };
                grpMapPack.Controls.Add(ctrl);
                btnUnInstall.Text = "Uninstall flag pack";
                btnUnInstall.Tag = pack;
            }
            else
            {
                btnUnInstall.Text = "Install from .zip";
                btnUnInstall.Tag = null;
            }

        }
    }
}
