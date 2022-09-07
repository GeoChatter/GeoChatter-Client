using CefSharp;

using CefSharp.WinForms;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Extensions;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Storage;
using GeoChatter.Forms.ScoreCalculator;
using GeoChatter.Handlers;
using GeoChatter.Helpers;
using GeoChatter.Model.Attributes;
using GeoChatter.Properties;
using GeoChatter.Web;
using Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoChatter.Forms
{
    public partial class MainForm
    {

        /// <summary>
        /// Set default settings for <paramref name="browser"/>
        /// </summary>
        /// <param name="browser">Browser to use</param>
        private static void SetBrowserSettings(ChromiumWebBrowser browser)
        {
            browser.Dock = DockStyle.Fill;
        }

        /// <inheritdoc/>
        public void RefreshBrowser(string url = null)
        {
            GoingToRefresh();
            logger.Debug("RefreshBrowser");
            SetupExtensionsAfterReload = false;
            ExtensionsInitialized = false;
            ExecutedManaged = false;
            if (ClientDbCache.RunningGame != null)
            {
                SaveAndExit()
                    .ContinueWith((_) =>
                    {
                        logger.Info("SaveAndExit.ContinueWith");
                        InitializeBrowser(url ?? browser.Address, true);
                    });
            }
            else
            {
                InitializeBrowser(url ?? browser.Address, true);
            }
        }

        /// <inheritdoc/>
        public void LoadURL(string url)
        {
            if (!string.IsNullOrEmpty(url) && browser != null && browser.IsBrowserInitialized)
            {
                RefreshBrowser(url);
            }
        }

        /// <inheritdoc/>
        public void InitializeBrowser(string url = "", bool refresh = false)
        {
            logger.Debug($"InitializeBrowser(refresh: {refresh}, LastAddress: {LastAddress})");
            if (!IsHandleCreated)
            {
                CreateHandle();
            }

            Invoke(
                delegate ()
                {
                    logger.Debug("InitializeBrowser Invoke");
                    // TODO: Sometimes Access violation crashes somewhere after this
                    try
                    {
                        if (browser != null)
                        {
                            AttributeDiscovery.RemoveEventHandlers(this, browser);
                            browser.LoadUrl("about:blank");
                            logger.Debug("Removed event handlers");
                            SetPanelControls(browser, true);
                            logger.Debug("Removed panel controls");
                            logger.Debug("Destroyed window");
                            Refresh();
                            logger.Debug("Control refreshed");
                            LastBrowserID = browser.GetBrowser().Identifier;
                        }

                        // TODO: On refresh, window doesnt belong to new browser, though scripts load for it
                        browser = new ChromiumWebBrowser(string.IsNullOrWhiteSpace(url) ? startPageUrl : url)
                        {
                            KeyboardHandler = new KeyboardHandler(this)
                        };
                        logger.Debug("Created browser instance");
#if !DEBUG
                        browser.MenuHandler = new CustomMenuHandler();
#endif
                        if (refresh)
                        {
                            LastAddress = string.Empty;
                        }

                        SetPanelControls(browser);
                        logger.Debug("Set panel controls");
                        SetBrowserSettings(browser);
                        logger.Debug("Set browser settings");

                        AttributeDiscovery.AddEventHandlers(this, browser);
                        logger.Debug("Added event handlers");

                        RegisterJavascriptObjects(browser);
                        logger.Debug("Added JS objects");
                        browser.RequestHandler = new GCRequestHandler();

                        ((GCRequestHandler)browser.RequestHandler).OnGeoGuessrSignedIn += OnGeoGuessrSignedIn;
                        ((GCRequestHandler)browser.RequestHandler).OnGeoGuessrSignedOut += OnGeoGuessrSignedOut; ;
                        ((GCRequestHandler)browser.RequestHandler).OnGeoGuessrCookieHijackedFirst += OnGeoGuessrCookieHijackedFirst;
                        logger.Debug("Added request intercepter");
                        browser.RenderProcessMessageHandler = new RenderProcessMessageHandler(this);


                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Summarize());
                    }
                }
            );
        }

        /// <summary>
        /// Main browser
        /// </summary>
        private ChromiumWebBrowser browser { get; set; }
        /// <summary>
        /// Objects to bind
        /// </summary>
        public IList<JSBind> Binds { get; } = new List<JSBind>();
        private delegate void SetPanelControlsCallback(ChromiumWebBrowser browser, bool remove = false);
        /// <summary>
        /// Make sure click events are handled in JS
        /// </summary>
        private void RegisterClickEventInJS()
        {
            browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetRegisterClickToJsScript());
        }

        private void SetPanelControls(ChromiumWebBrowser browser, bool remove = false)
        {
            if (browser == null)
            {
                return;
            }
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (panel1.InvokeRequired)
            {
                SetPanelControlsCallback d = new(SetPanelControls);
                Invoke(d, new object[] { browser, remove });
            }
            else
            {
                if (remove)
                {
                    panel1.Controls.Remove(browser);
                }
                else
                {
                    panel1.Controls.Add(browser);
                }
            }
        } 
        
        /// <inheritdoc/>
        public void ReportMainJSCompleted()
        {
            Task.Run(() =>
            {
                // Initialize
                using ScoreFormulator mgr = new(this.Version);
            });
        }
        [DiscoverableEvent]
        private void Browser_LoadError(object sender, LoadErrorEventArgs e)
        {
            logger.Error(e.DebugSummary());
        }

        [DiscoverableEvent]
        private void Browser_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            if (e.Message != null)
            {
                logger.Info($"JS Message: {e.Message.ToStringDefault()}");
            }
        }
        /// <summary>
        /// Last address before <see cref="Browser_AddressChanged(object, AddressChangedEventArgs)"/> was fired
        /// </summary>
        [DiscoverableEvent]
        private async void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            logger.Debug(e.DebugSummary() + " from " + LastAddress);
            if (!browser.IsBrowserInitialized
                || !ExtensionsInitialized
                || !SetupExtensionsAfterReload)
            {
                logger.Debug($"initialized: {browser.IsBrowserInitialized}, extInitialized: {ExtensionsInitialized}, setupAfterReload: {SetupExtensionsAfterReload}, execmanaged: {ExecutedManaged}, LastAddress. {LastAddress}, NextChangeIsRefresh: {NextChangeIsRefresh}");
                if (!string.IsNullOrWhiteSpace(LastAddress))
                {
                    logger.Info($"Address changed but browser not initialized: {LastAddress} -> {e.Address}");
                }
                return;
            }

            if (NextChangeIsRefresh)
            {
                SaveAndExitPreviousGameIfDifferent(e.Address);

                RefreshHandled = true;
                LastAddress = e.Address;
                RefreshBrowser();
                return;
            }

            Zoom(0);

            if (e.Address != LastAddress)
            {
                logger.Info("Executing target='All' JSWrappers");
                Wrappers
                        .Where(wrap => wrap.Target == JSWrapperTarget.All)
                        .ForEach(wrap => wrap.Execute(wrap.ExecutionFrame ?? browser.GetBrowser().MainFrame));
            }

            SaveAndExitPreviousGameIfDifferent(e.Address);

            LastAddress = e.Address;
            RefreshHandled = false;

            if (GeoGuessrClient.IsGameStartAddress(e.Address))
            {
                logger.Info("Address is game starting page, setting overlay");
                browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetAddressMainPlayScreenJsScript());
            }
            else if (GeoGuessrClient.IsGameAddress(e.Address)
                && !GameSetupInTitleChange)
            {
                logger.Info("Initializing game");
                await InitializeGamefromAddress(e.Address);
            }

            GameSetupInTitleChange = false;
            browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.DrawMapLinkButton());
        }
        [DiscoverableEvent]
        private void Browser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            this.InvokeDefault(f => f.Text = "GeoChatter - " + e.Title + " - Connected Map: " + (string.IsNullOrEmpty(guessApiClient?.MapIdentifier) ? "Disconnected" : guessApiClient.MapIdentifier));
        }
        [DiscoverableEvent]
        private void Browser_StatusMessage(object sender, StatusMessageEventArgs e)
        {
        }
        [DiscoverableEvent]
        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs frameLoadEndEventArgs)
        {
            logger.Debug("FrameLoadEnd " + frameLoadEndEventArgs.Url);
            ChromiumWebBrowser browser = (ChromiumWebBrowser)sender;

            //Wait for the MainFrame to finish loading
            if (frameLoadEndEventArgs.Frame.IsMain)
            {
                browser.SetZoomLevel(0);
            }
        }

        [DiscoverableEvent]
        private void Browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
#if DEBUG
            logger.Debug(e.DebugSummary());
#endif
        }

        /// <summary>
        /// CONFIRM: Fired everytime a new page loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DiscoverableEvent]
        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            logger.Debug(e.DebugSummary());
            // only inject Javascript AFTER page load is finished, once
            if (!e.IsLoading)
            {
                if (ClientDbCache.RunningGame != null)
                {
                    RegisterClickEventInJS();
                }

                Zoom(0);
            }
        }

        [DiscoverableEvent]
        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            logger.Debug($"Browser[{browser.GetBrowser().Identifier}, {NextChangeIsRefresh.ToStringDefault()}] initialize event...");

            if (SetupExtensionsAfterReload)
            {
                return;
            }

            if (browser.IsBrowserInitialized)
            {
                SetupExtensionsAfterReload = true;
                if (NextChangeIsRefresh)
                {
                    logger.Info("Initializing after a refresh...");
                }
                NextChangeIsRefresh = false;

                Zoom(0);
            }
        }

    }
}
