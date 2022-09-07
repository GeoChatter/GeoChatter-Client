using CefSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomMenuHandler : CefSharp.IContextMenuHandler
    {
        /// <summary>
    /// 
    /// </summary>
    /// <param name="browserControl"></param>
    /// <param name="browser"></param>
    /// <param name="frame"></param>
    /// <param name="parameters"></param>
    /// <param name="model"></param>
        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="browserControl"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        /// <param name="commandId"></param>
        /// <param name="eventFlags"></param>
        /// <returns></returns>
        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="browserControl"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="browserControl"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="parameters"></param>
        /// <param name="model"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
}
