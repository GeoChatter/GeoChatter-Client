using CefSharp;
using CefSharp.Handler;
using GeoChatter.Core.Extensions;
using log4net;
using System;
using System.Threading.Tasks;
using GeoChatter.Core.Common.Extensions;
using System.Text;

namespace GeoChatter.Web
{

    /// <summary>
    /// <see cref="RequestHandler"/> provides a base class for you to inherit from 
    /// you only need to implement the methods that are relevant to you. 
    /// If you implement the IRequestHandler interface you will need to
    /// implement every method
    /// </summary>
    public class GCRequestHandler : RequestHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(GCRequestHandler));
        public bool FiredHijack { get; set; }

        public event EventHandler<EventArgs> OnGeoGuessrCookieHijackedFirst;

        public event EventHandler<EventArgs> OnGeoGuessrSignedIn;

        public event EventHandler<EventArgs> OnGeoGuessrSignedOut;
        public void FireOnGeoGuessrCookieHijackedFirst()
        {
            logger.Debug("Firing GeoGuessr API cookie hijack");
            OnGeoGuessrCookieHijackedFirst?.Invoke(this, null);
        }
        public void FireOnGeoGuessrSignedIn()
        {
            logger.Debug("Firing GeoGuessr sign in");
            OnGeoGuessrSignedIn?.Invoke(this, null);
        }
        public void FireOnGeoGuessrSignedOut()
        {
            logger.Debug("Firing GeoGuessr sign out");
            OnGeoGuessrSignedOut?.Invoke(this, null);
        }
        /// <summary>
        /// 
        /// </summary>
        public static readonly string VersionNumberString = "Chromium: {0}, CEF: {1}, CefSharp: {2}".FormatDefault(Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="userGesture"></param>
        /// <param name="isRedirect"></param>
        /// <returns></returns>
        protected override bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="targetUrl"></param>
        /// <param name="targetDisposition"></param>
        /// <param name="userGesture"></param>
        /// <returns></returns>
        protected override bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
        {
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="errorCode"></param>
        /// <param name="requestUrl"></param>
        /// <param name="sslInfo"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected override bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            //NOTE: We also suggest you wrap callback in a using statement or explicitly execute callback.Dispose as callback wraps an unmanaged resource.

            //Example #1
            //Return true and call IRequestCallback.Continue() at a later time to continue or cancel the request.
            //In this instance we'll use a Task, typically you'd invoke a call to the UI Thread and display a Dialog to the user
            //You can cast the IWebBrowser param to ChromiumWebBrowser to easily access
            //control, from there you can invoke onto the UI thread, should be in an async fashion
            Task.Run(() =>
            {
                //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
                if (!callback.IsDisposed)
                {
                    using (callback)
                    {
                        callback.Continue(true);
                    }
                }
            });

            return true;

            //Example #2
            //Execute the callback and return true to immediately allow the invalid certificate
            //callback.Continue(true); //Callback will Dispose it's self once exeucted
            //return true;

            //Example #3
            //Return false for the default behaviour (cancel request immediately)
            //callback.Dispose(); //Dispose of callback
            //return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="originUrl"></param>
        /// <param name="isProxy"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="realm"></param>
        /// <param name="scheme"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected override bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
        {
            //NOTE: We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.

            //Example #1
            //Spawn a Task to execute our callback and return true;
            //Typical usage would see you invoke onto the UI thread to open a username/password dialog
            //Then execute the callback with the response username/password
            //You can cast the IWebBrowser param to ChromiumWebBrowser to easily access
            //control, from there you can invoke onto the UI thread, should be in an async fashion
            //Load https://httpbin.org/basic-auth/cefsharp/passwd in the browser to test
            //Task.Run(() =>
            //{
            //    using (callback)
            //    {
            //        //if (originUrl.Contains("https://httpbin.org/basic-auth/"))
            //        //{
            //        //    string[] parts = originUrl.Split('/');
            //        //    string username = parts[^2];
            //        //    string password = parts[^1];
            //        //    callback.Continue(username, password);
            //        //}
            //    }
            //});

            return true;

            //Example #2
            //Return false to cancel the request
            //callback.Dispose();
            //return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="status"></param>
        protected override void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefTerminationStatus status)
        {
            // Add your own code here for handling scenarios where the Render Process terminated for one reason or another.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="originUrl"></param>
        /// <param name="newSize"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected override bool OnQuotaRequest(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
        {
            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            //if (callback != null && !callback.IsDisposed)
            //{
            //    using (callback)
            //    {
            //        //Accept Request to raise Quota
            //        //callback.Continue(true);
            //        //return true;
            //    }
            //}

            return false;
        }

        public const string GoogleSignInPath = "/googleplus/signin";
        public const string AppleSignInPath = "/apple/signin";
        public const string FacebookSignInPath = "/facebook/signin";
        public const string GeoGuessrSignInPath = "/accounts/signin";
        public const string SignOutPath = "/accounts/signout";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="isNavigation"></param>
        /// <param name="isDownload"></param>
        /// <param name="requestInitiator"></param>
        /// <param name="disableDefaultHandling"></param>
        /// <returns></returns>
        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            if (request == null)
            {
                return null;
            }

            // TODO: Figure a way around Google login blocks
            if (request.Url.ToLowerInvariant().StartsWithDefault(GeoGuessrClient.GeoGuessrAPI))
            {
                bool isGGAccount = request.Url.EndsWithDefault(GeoGuessrSignInPath);
                bool isFacebookAccount = request.Url.EndsWithDefault(FacebookSignInPath);
                bool isGoogleAccount = request.Url.EndsWithDefault(GoogleSignInPath);
                bool isAppleAccount = request.Url.EndsWithDefault(AppleSignInPath);
                if (isGGAccount
                    || isFacebookAccount
                    || isGoogleAccount
                    || isAppleAccount)
                {
                    try
                    {
                        string pdata = Encoding.Default.GetString(request.PostData.Elements[0]?.Bytes ?? Array.Empty<byte>());
                        if (string.IsNullOrEmpty(pdata))
                        {
                            return null;
                        }
                        
                        if (isGGAccount)
                        {
                            return new GCResourceRequestHandler(this, GeoGuessrSignInPath);
                        }
                        else if (isFacebookAccount)
                        {
                            return new GCResourceRequestHandler(this, FacebookSignInPath);
                        }
                        else if (isGoogleAccount)
                        {
                            return new GCResourceRequestHandler(this, GoogleSignInPath);
                        }
                        else
                        {
                            return new GCResourceRequestHandler(this, AppleSignInPath);
                        }
                    }
                    catch(Exception _e)
                    {
                        logger.Error(_e.Summarize());
                    }
                }
                else if (request.Url.EndsWithDefault(SignOutPath))
                {
                    return new GCResourceRequestHandler(this, SignOutPath);
                }
                return new GCResourceRequestHandler(this, GeoGuessrClient.GeoGuessrAPI, true);
            }

            return null;
        }
    }
}
