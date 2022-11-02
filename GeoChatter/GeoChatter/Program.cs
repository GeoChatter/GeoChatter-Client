using AutoUpdaterDotNET;
using CefSharp;
using CefSharp.WinForms;
using GeoChatter.Core.Handlers;
using GeoChatter.Forms;
using GeoChatter.Properties;
using GeoChatter.Web;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;
using System.Windows.Forms;
using GeoChatter.Core.Common.Extensions;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using GeoChatter.Model.Enums;
using GeoChatter.Core.Helpers;
using System.Linq;

namespace GeoChatter
{
    internal static class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        private const string log4netxml = "log4net.xml";
        private static bool uploadLog;

        public static string Version { get; } = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static int Main(params string[] args)
        {
            try
            {
                XmlConfigurator.ConfigureAndWatch(new FileInfo(log4netxml));
                
                logger.Info("Starting the app with arguments: " + string.Join(", ", args));
                logger.Info("Running GC as admin: " + WindowsHelper.IsProcessElevated);
                if (args.Length != 0)
                {
                    //string id = args.FirstOrDefault(a => a.StartsWithDefault("--host-process-id="));
                    //if (!string.IsNullOrWhiteSpace(id))
                    //{
                    //    int i = id[18..].ParseAsInt();
                    //    logger.Info($"[{Environment.ProcessId}]Waiting for exit of " + i);
                    //    await System.Diagnostics.Process.GetProcessById(i).WaitForExitAsync();
                    //    logger.Info($"[{Environment.ProcessId}]Got the exit of " + i);
                    //}
                    return 1;
                }
                if (Settings.Default.EnableDebugLogging)
                {
                    AppDomain.CurrentDomain.FirstChanceException +=
                    (object source, FirstChanceExceptionEventArgs e) =>
                    {
                        
                        string msg = $"FirstChanceException({AppDomain.CurrentDomain.FriendlyName}): {e.Exception.Summarize(1)}";
                        logger.Error(msg);
#if DEBUG
                        Debug.WriteLine(msg);
#endif
                    };
                }
                if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (Settings.Default.UpgradeRequired)
                {
                   
                    Settings.Default.Upgrade();
                    Settings.Default.UpgradeRequired = false;
                    Settings.Default.Save();
                }
                if (string.IsNullOrEmpty(Settings.Default.GCClientId))
                {
                    Settings.Default.GCClientId = Guid.NewGuid().ToStringDefault("N");
                    Settings.Default.Save();
                }

                if (!Settings.Default.EnableDebugLogging)
                {
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.RemoveAppender("DebugAppender");
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).RaiseConfigurationChanged(EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("NOTE:\r\n" +
                        "Debug logging is enabled!\r\n" +
                        "This can cause exponential growth of log files!\r\n" +
                        "To disable it again go to Settings -> Application Settings!", "WARNING - Debug logging is enabled", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    uploadLog = true;
                }
                string tempdir = Path.Combine(Path.GetTempPath(), "GeoChatter");
                if (Directory.Exists(tempdir))
                {
                    Directory.Delete(tempdir, true);
                }
                Directory.CreateDirectory(tempdir);

                logger.Info("Initializing Cef");
                InitializeCef();

                logger.Info("Applying application settings");
                Application.EnableVisualStyles();
                Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
                Application.SetCompatibleTextRenderingDefault(false);

#if WINDOWS7_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
                logger.Info("Initializing auto-updater");
                AutoUpdaterInit();
                CleanUpUploadedLogs();
                logger.Info("Creating main form instance");
                using MainForm form = new(Version);

                logger.Info("Running application with the main form");
                Application.Run(form);

#pragma warning restore CA1416 // Validate platform compatibility
#else
                logger.Error("Unsupported platform! Platform ID: " + Environment.OSVersion.Platform);
#endif
                return 0;
            }
            catch (Exception ex)
            {
                string sum = ex.Summarize();
                logger.ErrorFormat("Main failed: {0}", sum);
                MessageBox.Show("Something went completely wrong! Please check your log files and send them to the developers!\n" + sum, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return -1;
            }
            finally
            {
                if (args.Length == 0)
                {
                    logger.Info("Shutting down Cef");
                    Cef.Shutdown();

                }
            }
        }

        private static void CleanUpUploadedLogs()
        {
            string[] files = Directory.GetFiles(AssembyHelper.AssemblyDirectory, "GeoChatter*.zip", SearchOption.TopDirectoryOnly);
            logger.Debug($"Cleaning up uploaded logs. {files.Length} to check.");
            foreach (string file in files)
            {
                
                FileInfo fileInfo = new FileInfo(file);
                logger.Debug($"Checking {fileInfo.Name}"); 
                logger.Debug($"Creation date: {fileInfo.CreationTime}");
                if (fileInfo.CreationTime < DateTime.Now.AddDays(-7))
                {
                    logger.Debug($"Deleting {fileInfo.Name}");
                    System.IO.File.Delete(file);
                }else
                    logger.Debug($"{fileInfo.Name} is not 7 days old");
            }
        }

        [SupportedOSPlatform("windows7.0")]
        private static void AutoUpdaterInit()
        {
#if DEBUG
            string versionXML = Settings.Default.Link_VersionXML_Debug;
#else
            string versionXML = Settings.Default.Link_VersionXML;
#endif
            // AutoUpdater.InstalledVersion = new Version("0.7.0.0");
            AutoUpdater.Synchronous = true;
            AutoUpdater.RunUpdateAsAdmin = true;
            AutoUpdater.InstalledVersion = Assembly.GetExecutingAssembly().GetName().Version;
            AutoUpdater.Start(versionXML);
        }

        /// <summary>
        /// Initialize Cef with custom settings
        /// </summary>
        private static void InitializeCef()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            using CefSettings settings = new()
            {
                CachePath = path,
                PersistSessionCookies = true,
                LogFile = "browser.log",
               // BrowserSubprocessPath = Application.StartupPath + "CefSharp.BrowserSubprocess.exe",
                //ChromeRuntime = true
            };
            
            CefSharpSettings.ConcurrentTaskExecution = true;

            settings.RegisterScheme(new CefCustomScheme()
            {
                SchemeName = GCSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new GCSchemeHandlerFactory(Version),
            });
            Cef.Initialize(settings);
        }
    }
}
