using CefSharp;
using CefSharp.Handler;
using GeoChatter.Core.Extensions;
using log4net;
using System;
using System.IO;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Helpers;
using System.Text;
using GeoChatter.Model;
using Newtonsoft.Json;

namespace GeoChatter.Web
{
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

        // Thanks a lot GeoGuessr and Apple
        private static bool HandlingAppleSignIn { get; set; }

        private static DateTime LastAppleSignInHandling { get; set; }

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
            switch (InterceptTargetURL)
            {
                case GCRequestHandler.GeoGuessrSignInPath:
                case GCRequestHandler.FacebookSignInPath:
                case GCRequestHandler.GoogleSignInPath:
                    {
                        try
                        {
                            var resp = Encoding.ASCII.GetString(memoryStream.ToArray());
                            if (response?.StatusCode == 200)
                            {
                                SignInData = JsonConvert.DeserializeObject<GGSignInResponse>(resp);
                                ClientUserID = SignInData?.id;
                                GeoGuessrClient.Cookie = response.Headers.Get("Set-Cookie");
                                Parent.FireOnGeoGuessrSignedIn();
                            }
                            else
                            {
                                logger.Error($"Sign in failed. {response?.ErrorCode}: \r\n\t{resp}");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Warn(ex.Summarize());
                        }
                        break;
                    }
                case GCRequestHandler.AppleSignInPath:
                    {
                        if (HandlingAppleSignIn || DateTime.Now - LastAppleSignInHandling < TimeSpan.FromSeconds(10))
                        {
                            logger.Warn("Ignoring excessive Apple sign in requests");
                            break;
                        }
                        LastAppleSignInHandling = DateTime.Now;
                        HandlingAppleSignIn = true;
                        try
                        {
                            var resp = Encoding.ASCII.GetString(memoryStream.ToArray());
                            if (response?.StatusCode == 200)
                            {
                                SignInData = JsonConvert.DeserializeObject<GGSignInResponse>(resp);
                                ClientUserID = SignInData?.id;
                                GeoGuessrClient.Cookie = response.Headers.Get("Set-Cookie");
                                Parent.FireOnGeoGuessrSignedIn();
                            }
                            else
                            {
                                logger.Error($"Sign in failed. {response?.ErrorCode}: \r\n\t{resp}");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Warn(ex.Summarize());
                        }
                        finally
                        {
                            HandlingAppleSignIn = false;
                        }
                        break;
                    }
                case GCRequestHandler.SignOutPath:
                    {
                        try
                        {
                            if (response?.StatusCode == 200)
                            {
                                Parent.FireOnGeoGuessrSignedOut();
                            }
                            else
                            {
                                var resp = Encoding.ASCII.GetString(memoryStream.ToArray());
                                logger.Error($"Sign out failed. {response?.ErrorCode}: \r\n\t{resp}");
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Warn(ex.Summarize());
                        }
                        break;
                    }
                default:
                    break;
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
