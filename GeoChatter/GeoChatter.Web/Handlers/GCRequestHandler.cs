using CefSharp;
using CefSharp.Handler;
using GeoChatter.Core.Extensions;
using log4net;
using System;
using System.IO;
using System.Threading.Tasks;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Helpers;
using System.Text;
using GeoChatter.Model;
using Newtonsoft.Json;
using Antlr4.Runtime.Misc;

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
        public bool FiredHijack { get; set; }

        public event EventHandler<EventArgs> OnGeoGuessrCookieHijackedFirst;

        public event EventHandler<EventArgs> OnGeoGuessrSignedIn;

        public event EventHandler<EventArgs> OnGeoGuessrSignedOut;
        public void FireOnGeoGuessrCookieHijackedFirst()
        {
            OnGeoGuessrCookieHijackedFirst?.Invoke(this, null);
        }
        public void FireOnGeoGuessrSignedIn()
        {
            OnGeoGuessrSignedIn?.Invoke(this, null);
        }
        public void FireOnGeoGuessrSignedOut()
        {
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

            if (request.Url.ToLowerInvariant().StartsWithDefault(GeoGuessrClient.GeoGuessrAPI))
            {
                if (request.Url.EndsWithDefault("/accounts/signin"))
                {
                    try
                    {
                        string pdata = Encoding.Default.GetString(request.PostData.Elements[0]?.Bytes ?? Array.Empty<byte>());
                        if (string.IsNullOrEmpty(pdata))
                        {
                            return null;
                        }
                   
                        var p = JsonConvert.DeserializeObject<GG_SignInPost>(pdata);
                        
                        GCResourceRequestHandler.ClientMail = p?.email;
                        
                        return new GCResourceRequestHandler(this, "/accounts/signin");
                    }
                    catch(Exception _e)
                    {

                    }
                }
                else if (request.Url.EndsWithDefault("/accounts/signout"))
                {
                    return new GCResourceRequestHandler(this, "/accounts/signout");
                }
                return new GCResourceRequestHandler(this, GeoGuessrClient.GeoGuessrAPI, true);
            }

            return null;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GCResourceRequestHandler : ResourceRequestHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(GCResourceRequestHandler));
        private MemoryStream memoryStream = new();
        /// <summary>
        /// 
        /// </summary>
        public string InterceptTargetURL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool CatchCookie { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GCRequestHandler Parent { get; set; }

        /// <summary>
        /// GeoGuessr player id of the client
        /// </summary>
        public static string ClientUserID { get; set; }
        /// <summary>
        /// GeoGuessr player email
        /// </summary>
        public static string ClientMail { get; set; }
       /// <summary>
        /// GeoGuessr player name
        /// </summary>
        public static string ClientGeoGuessrName { get; set; }
        public static string ClientGeoGuessrPic { get; set; }
        /// <summary>
        /// GeoGuessr cookie used with GeoGuessr API requests
        /// </summary>
        public static GGSignInResponse SignInData { get; private set; }

        /// <summary>
        /// Wheter the instance is created for handling JS file imports
        /// </summary>
        public bool HandlesFileAccessForImports { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intercept"></param>
        /// <param name="catchCookie"></param>
        /// <param name="fileAccess"></param>
        public GCResourceRequestHandler(GCRequestHandler parent, string intercept, bool catchCookie = false, bool fileAccess = false)
        {
            Parent = parent;
            InterceptTargetURL = intercept;
            CatchCookie = catchCookie;
            HandlesFileAccessForImports = fileAccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            logger.Debug($"Intercepted {request?.Identifier}({request?.Method} {request?.Url})");//{request.Summarize()}");

            if (request == null 
                || HandlesFileAccessForImports 
                || !CatchCookie 
                || request.Headers.Get("Cookie") == null)
            {
                return CefReturnValue.Continue;
            }
            else
            {
                GeoGuessrClient.Cookie = request.Headers.Get("Cookie");
                if (!Parent.FiredHijack && GeoGuessrClient.Cookie != null)
                {
                    Parent.FiredHijack = true;
                    Parent.FireOnGeoGuessrCookieHijackedFirst();
                }

            }

            if (Uri.TryCreate(request.Url, UriKind.Absolute, out _) == false)
            {
                //If we're unable to parse the Uri then cancel the request
                // avoid throwing any exceptions here as we're being called by unmanaged code
                return CefReturnValue.Cancel;
            }

            //Example of how to set Referer
            // Same should work when setting any header

            //Example of setting User-Agent in every request.
            //var headers = request.Headers;

            //var userAgent = headers["User-Agent"];
            //headers["User-Agent"] = userAgent + " CefSharp";

            //request.Headers = headers;

            //NOTE: If you do not wish to implement this method returning false is the default behaviour
            // We also suggest you explicitly Dispose of the callback as it wraps an unmanaged resource.
            //callback.Dispose();
            //return false;

            //NOTE: When executing the callback in an async fashion need to check to see if it's disposed
            //if (!callback.IsDisposed)
            //{
            //    using (callback)
            //    {
            //        if (request.Method == "POST")
            //        {
            //            using (var postData = request.PostData)
            //            {
            //                if (postData != null)
            //                {
            //                    var elements = postData.Elements;

            //                    var charSet = request.GetCharSet();

            //                    foreach (var element in elements)
            //                    {
            //                        if (element.Type == PostDataElementType.Bytes)
            //                        {
            //                            var body = element.GetBody(charSet);
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //        //Note to Redirect simply set the request Url
            //        //if (request.Url.StartsWith("https://www.google.com", StringComparison.OrdinalIgnoreCase))
            //        //{
            //        //    request.Url = "https://github.com/";
            //        //}

            //        //Callback in async fashion
            //        //callback.Continue(true);
            //        //return CefReturnValue.ContinueAsync;
            //    }
            //}

            return CefReturnValue.Continue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="newUrl"></param>
        protected override void OnResourceRedirect(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, ref string newUrl)
        {
            //Example of how to redirect - need to check `newUrl` in the second pass
            //if (request.Url.StartsWith("https://www.google.com", StringComparison.OrdinalIgnoreCase) && !newUrl.Contains("github"))
            //{
            //    newUrl = "https://github.com";
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override bool OnProtocolExecution(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request)
        {
            return request != null && request.Url.StartsWithDefault("mailto");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        protected override bool OnResourceResponse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            //NOTE: You cannot modify the response, only the request
            // You can now access the headers
            //var headers = response.Headers;

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
        {
            if (request != null && response != null && HandlesFileAccessForImports)
            {
                try
                {
                    string path = new Uri(request.Url).LocalPath;
                    path = path.ReplaceDefault("/","\\");
                    string filepath = $"{Directory.GetCurrentDirectory().Trim('\\')}\\{JSWrapper.ScriptsDirectory}\\{path.Trim('\\')}";
                    if (File.Exists(filepath))
                    {
                        string content = File.ReadAllText(filepath);
                        // TODO: Figure this out
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Summarize());
                }
            }
            return new CefSharp.ResponseFilter.StreamResponseFilter(memoryStream);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chromiumWebBrowser"></param>
        /// <param name="browser"></param>
        /// <param name="frame"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="status"></param>
        /// <param name="receivedContentLength"></param>
        protected override void OnResourceLoadComplete(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            if (InterceptTargetURL == "/accounts/signin")
            {
                try
                {
                    var resp = Encoding.ASCII.GetString(memoryStream.ToArray());
                    SignInData = JsonConvert.DeserializeObject<GGSignInResponse>(resp);
                    ClientUserID = SignInData?.id;
                    GeoGuessrClient.Cookie = response.Headers.Get("Set-Cookie");
                    Parent.FireOnGeoGuessrSignedIn();

                }
                catch 
                { 
                    // TODO
                }
            }
            else if (InterceptTargetURL == "/accounts/signout")
            {
                Parent.FireOnGeoGuessrSignedOut();
            }
            logger.Debug($"Resolved request {request?.Identifier}({request?.Method} {request?.Url}): {response?.Summarize()} ");//= {Encoding.GetEncoding(string.IsNullOrWhiteSpace(response.Charset) ? "utf-8" : response.Charset).GetString(memoryStream?.ToArray() ?? Array.Empty<byte>())}");
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void Dispose()
        {
            memoryStream?.Dispose();
            memoryStream = null;

            base.Dispose();
        }
    }
}
