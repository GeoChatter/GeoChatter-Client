using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoChatter.Forms
{
    public partial class MainForm
    {
        #region UserScripts

        private bool ExecutedManaged { get; set; }

        internal void AddDocumentStartScripts()
        {
            Wrappers.AddRange(
                UserScriptManager.UserScripts
                    .Where(e => e.IsEnabled && e.RunAtDocumentStart)
                    .Select(e => new JSWrapper(e.Wrapper.FullScript, JSWrapperType.Raw, JSWrapperTarget.Launch))
            );
        }


        /// <summary>
        /// Initialize scripts provided in <see cref="Wrappers"/>
        /// </summary>
        internal void InitializeExtensions()
        {
            logger.Info("InitializeExtensions");
            ExtensionsInitialized = true;
            if (waitingForApi)
            {
                // Call after execution
                // LoadingScreen(true, "Logging into GeoChatter servers...");
            }

            ExecuteScripts(Wrappers.Where(wrap => wrap.Target == JSWrapperTarget.Launch));
        }
        /// <inheritdoc/>
        public void ExecuteUserScripts()
        {
            logger.Debug("ExecuteManagedExtensions");
            ExecuteScripts(UserScriptManager.UserScripts.Where(e => e.IsEnabled && !e.RunAtDocumentStart).Select(e => e.Wrapper));
            ExecutedManaged = true;
        }

        private bool SetupExtensionsAfterReload { get; set; }
        /// <summary>
        /// Execute the installed JS scripts
        /// </summary>
        /// <param name="scripts"></param>
        private void ExecuteScripts(IEnumerable<JSWrapper> scripts)
        {
            logger.Debug("ExecuteExtensions");
            try
            {
                scripts
                .ForEach(wrapper =>
                {
                    int max = wrapper.Source.Length < 125 ? wrapper.Source.Length : 120;
                    logger.Info("Executing JSWrapper: " + wrapper.Source[..max]);

                    wrapper.OnSourceInvalid += (object sender, EventArgs e) =>
                    {
                        logger.Warn($"JSWrapper({sender.GetHashCode().ToStringDefault()}) source invalid");
                    };

                    if (wrapper.ExecutionFrame == null)
                    {
                        wrapper.Execute(browser.GetBrowser().MainFrame);
                    }
                    else
                    {
                        wrapper.Execute();
                    }
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        #endregion


    }
}
