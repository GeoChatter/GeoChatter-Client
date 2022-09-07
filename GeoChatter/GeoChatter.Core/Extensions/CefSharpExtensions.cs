using CefSharp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GeoChatter.Core.Extensions
{
    /// <summary>
    /// Extension methods for CefSharp classes
    /// </summary>
    public static class CefSharpExtensions
    {
        #region Debug Extensions
        private static string LogPrefix(IBrowser browser)
        {
            return $"[BROWSER={browser.Identifier}] ";
        }
        /// <summary>
        /// Summarize event arguments instance for debugging messages
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string DebugSummary([NotNull] EventArgs e)
        {
            return e.ToString();
        }

        /// <summary>
        /// Summarize event arguments instance for debugging messages
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string DebugSummary([NotNull] this LoadErrorEventArgs e)
        {
            return LogPrefix(e.Browser) + $"{e.ErrorCode} at ({e.FailedUrl}): {e.ErrorText}";
        }

        /// <summary>
        /// Summarize event arguments instance for debugging messages
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string DebugSummary([NotNull] this AddressChangedEventArgs e)
        {
            return LogPrefix(e.Browser) + $"Adress: {e.Address}";
        }

        /// <summary>
        /// Summarize event arguments instance for debugging messages
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string DebugSummary([NotNull] this TitleChangedEventArgs e)
        {
            return LogPrefix(e.Browser) + $"Title: {e.Title}";
        }

        /// <summary>
        /// Summarize event arguments instance for debugging messages
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string DebugSummary([NotNull] this StatusMessageEventArgs e)
        {
            return LogPrefix(e.Browser) + (string.IsNullOrWhiteSpace(e.Value) ? "No status message upon change..." : $"Status: {e.Value}");
        }

        /// <summary>
        /// Summarize event arguments instance for debugging messages
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string DebugSummary([NotNull] this ConsoleMessageEventArgs e)
        {
            return LogPrefix(e.Browser) + $"Console(source={e.Source}, level={e.Level}, line={e.Line}): {e.Message}";
        }

        /// <summary>
        /// Summarize event arguments instance for debugging messages
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string DebugSummary([NotNull] this LoadingStateChangedEventArgs e)
        {
            return LogPrefix(e.Browser) + $"Loading state: IsLoading={e.IsLoading}, CanReload={e.CanReload}, CanGoBack={e.CanGoBack}, CanGoForward={e.CanGoForward}";
        }


        /// <summary>
        /// Summarize request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string Summarize([NotNull] this IRequest request)
        {
            return $"REQUEST[{request.Identifier}] = {request.Method} : {request.Url}\r\n\t{string.Join("\r\n\t", request.Headers.AllKeys.Select(k => $"'{k}' = '{request.Headers[k]}'"))}\r\n\t--------\r\n\t{string.Join("\r\n\t", request.PostData?.Elements.Select(e => $"'{e.GetBody()}'") ?? Array.Empty<string>())}";
        }

        /// <summary>
        /// Summarize response
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string Summarize([NotNull] this IResponse response)
        {
            return $"RESPONSE Status:{response.StatusCode}({response.StatusText}), Mime:'{response.MimeType}'";
        }
        #endregion
    }
}
