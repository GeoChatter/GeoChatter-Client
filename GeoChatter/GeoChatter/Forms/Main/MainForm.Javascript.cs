using GeoChatter.Core.Helpers;
using GeoChatter.Model.Enums;
using GeoChatter.Model;
using GeoChatter.Properties;
using GeoChatter.Web;
using System;
using System.Collections.Generic;
using System.Text;
using GeoChatter.Core.Common.Extensions;
using System.Linq;
using CefSharp.WinForms;
using System.IO;
using System.Windows.Forms;

namespace GeoChatter.Forms
{
    public partial class MainForm
    {
        /// <summary>
        /// Sign out
        /// </summary>
        public void SendGeoGuessrSignOutFromBrowserAndRedirect(string redirectTo)
        {
            logger.Info($"SendGeoGuessrSignOutFromBrowserAndRedirect {redirectTo}");
            MessageBox.Show("You need to sign out and back into GeoGuessr to verify your account!", "SIGN IN REQUIRED FOR VERIFICATION", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            // TODO: Automate after browser initialized
            // browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetSendSignOutDataToJs(redirectTo));
        }

        /// <summary>
        /// Sends the start round event to JS
        /// </summary>
        /// <param name="round"></param>
        private void SendStartRoundToJS(Round round)
        {
            CurrentState = AppGameState.INROUND;
            logger.Debug("SendStartRoundToJS");
            browser.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetStartRoundJsScript(round));
            if(round.RoundNumber > 1)
                SendStartRoundMessage(round);
        }
              

        /// <summary>
        /// Loading screen display
        /// </summary>
        private void LoadingScreen(bool show = true, string message = "Loading...")
        {
            logger.Debug("LoadingScreen");
            if (show)
            {
                browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.EnableLoadingScreen(message));
            }
            else
            {
                browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.DisableLoadingScreen());
            }
        }

        public void ToggleGuessSlider()
        {
            browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.ToggleGuessSlider());
        }

        /// <summary>
        /// Sends the end round event to JS
        /// </summary>
        /// <param name="round"></param>
        private void SendEndRoundToJS(Round round)
        {
            CurrentState = AppGameState.ROUNDEND;
            logger.Debug("SendEndRoundToJS");
            browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetEndRoundJsScript(round));
            SendEndRoundMessage(round);
        }
        /// <summary>
        /// Sends the start game event to JS
        /// </summary>
        /// <param name="game"></param>
        private void SendStartGameToJS(Game game)
        {
            GameStartFires.Add(game.GeoGuessrId);

            logger.Debug("SendStartGameToJS " + game.GeoGuessrId);

            MapOptions options = GetMapOptions();
            options.GameMode = game.Mode.ToString();
            options.IsUSStreak = game.IsUsStreak;
            guessApiClient.SendMapOptionsToMaps(options);

            browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetStartGameJsScript(game));

            SendStartGameMessage(game);
        }

      
        /// <summary>
        /// Sends the end infinity game event to JS
        /// </summary>
        /// <param name="game"></param>
        private void SendEndInfinityGameToJS(Game game)
        {
            logger.Debug("SendEndInfinityGameToJS");
            browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetEndInfinityGameJsScript(game));
        }
        /// <summary>
        /// Sends the end game event to JS
        /// </summary>
        /// <param name="game"></param>
        private void SendEndGameToJS(Game game)
        {
            logger.Debug("SendEndGameToJS");
            browser.GetBrowser().FocusedFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetEndGameJsScript(game));
            SendRoundEndMessage(game);
        }

      

        /// <summary>
        /// Fire JS events for updating settings
        /// </summary>
        public void SendSettingsUpdateToJS()
        {
            logger.Debug("SendSettingsUpdateToJS");
            if (browser.IsBrowserInitialized)
            {
                browser?.GetBrowser()?.MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetSettingsUpdateJsScript());
            }
        }
        /// <summary>
        /// Sends the exit game event to JS
        /// </summary>
        /// <param name="game"></param>
        private void SendExitGameToJS(Game game)
        {
            CurrentState = AppGameState.NOTINGAME;
            logger.Debug("SendExitGameToJS");
            browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetExitGameJsScript(game));
        }
        /// <summary>
        /// Sends the refresh game event to JS
        /// </summary>
        /// <param name="game"></param>
        private void SendRefreshGameToJS(Game game)
        {
            logger.Debug("SendRefreshGameToJS");
            browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetRefreshGameJsScript(game));
        }
        internal void InitializeWrappers()
        {
            foreach (var sn in ScriptNames)
            {
                var wrapper = new JSWrapper(sn, JSWrapperType.Local);
                Wrappers.Add(wrapper);
            }
        }

        private void SetupBinds()
        {
            Binds.Add(new JSBind("jsHelper", new JsToCsHelper(this)));
        }


        /// <summary>
        /// Registers .NET objects in Javascript
        /// </summary>
        /// <param name="browser"></param>
        private void RegisterJavascriptObjects(ChromiumWebBrowser browser)
        {
            Binds.ForEach(bind =>
            {
                bind.Register(browser.JavascriptObjectRepository);
            });
        }


    }
}
