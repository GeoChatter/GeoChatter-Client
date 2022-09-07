using CefSharp;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Interfaces;
using GeoChatter.Forms;
using log4net;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace GeoChatter.Handlers
{
    /// <summary>
    /// Context handler
    /// </summary>
    public class RenderProcessMessageHandler : IRenderProcessMessageHandler
    {
        /// <summary>
        /// Parent browser form
        /// </summary>
        public MainForm Parent { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public RenderProcessMessageHandler(MainForm parent)
        {
            Parent = parent;
            MainJS = File.ReadAllText(MainJSPath);
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(RenderProcessMessageHandler));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        public void OnContextReleased(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {
#if DEBUG
            logger.Debug($"OnContextReleased: {frame?.Name}");
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="node"></param>
        public void OnFocusedNodeChanged(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IDomNode node)
        {
#if DEBUG
            logger.Debug($"OnFocusedNodeChanged: {node?.TagName}");
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="exception"></param>
        public void OnUncaughtException(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, JavascriptException exception)
        {
            logger.Error($"Render-OnUncaughtException: {exception?.Message}");
        }

        /// <summary>
        /// Main script file path
        /// </summary>
        public const string MainJSPath = "./Scripts/dist/main.js";

        /// <summary>
        /// Wheter main script file has been executed
        /// </summary>
        public string MainJS { get; private set; }

        /// <summary>
        /// Wheter main script file has been executed
        /// </summary>
        public bool MainJSExecuted { get; private set; }

        // Wait for the underlying JavaScript Context to be created. This is only called for the main frame.
        // If the page has no JavaScript, no context will be created.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="browserControl"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        public void OnContextCreated(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {
            logger.Info($"Context created: {frame?.Name}, {frame?.Url}, {Parent.ExtensionsInitialized}");
            string script = $"console.log('Context created: {frame?.Name}, {frame?.Url}', performance.now()); document.addEventListener('DOMContentLoaded', () => console.log('DOM content loaded', performance.now()));";

            frame.ExecuteJavaScriptAsync(script);

#pragma warning disable CA1416 // Validate platform compatibility

            if (!Parent.ExtensionsInitialized && frame.IsMain && frame.Url != "about:blank")
            {
                if (!MainJSExecuted)
                {
                    try
                    {
                        MainJSExecuted = true;
                        frame.ExecuteJavaScriptAsync(MainJS);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Summarize());
                        MainJSExecuted = false;
                    }
                }

                Parent.Wrappers.Clear();

                Parent.InitializeWrappers();

                Parent.AddDocumentStartScripts();

                Parent.Wrappers
                    .Where(wrap => wrap.Target == JSWrapperTarget.Launch)
                    .ForEach(wrap => wrap.ExecutionFrame = frame);

                Parent.InitializeExtensions();
            }
#pragma warning restore CA1416 // Validate platform compatibility
        }
    }
}
