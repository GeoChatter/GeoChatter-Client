using GeoChatter.Helpers;
using GeoChatter.Core.Model;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// Twitch OAuth helper class
    /// </summary>
    public static class TwitchOAuthHelper
    {
        private static HttpListener twitchListener;
        private const string ReturnUrl = "http://localhost:56207";

        /// <summary>
        /// Listener instance
        /// </summary>
        public static HttpListener TwitchListener
        {
            get
            {
                if (twitchListener == null)
                {
                    twitchListener = new HttpListener();
                    twitchListener.Prefixes.Add(ReturnUrl + "/");
                }
                return twitchListener;
            }
        }

        /// <summary>
        /// Get Twitch Authentication Async
        /// </summary>
        /// <returns></returns>
        public static Task<TwitchAuthenticationModel> GetAuthenticationValuesAsync()
        {
            return Task.Run(GetAuthenticationValues);
        }

        /// <summary>
        /// Start the listener
        /// </summary>
        /// <returns>returns IsListening Value</returns>
        private static bool StartListener()
        {
            try
            {
                TwitchListener.Start();
            }
            catch (HttpListenerException ex)
            {
                throw new InvalidOperationException("Cant start listener for TwitchAuthentication" + Environment.NewLine + ex);
            }

            return TwitchListener.IsListening;
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        private static void StopListener()
        {
            TwitchListener.Stop();
        }

        /// <summary>
        /// Get Twitch Auths
        /// </summary>
        /// <returns></returns>
        public static TwitchAuthenticationModel GetAuthenticationValues()
        {
            StartListener();

            TwitchAuthenticationModel Values = null;
            while (TwitchListener.IsListening)
            {
                HttpListenerContext context = TwitchListener.GetContext();

                if (context.Request.QueryString.HasKeys())
                {
                    if (context.Request.RawUrl.ContainsDefault("access_token"))
                    {
                        Uri myUri = new(context.Request.Url.OriginalString);
                        string scope = System.Web.HttpUtility.ParseQueryString(myUri.Query).Get("scope");
                        string access_token = System.Web.HttpUtility.ParseQueryString(myUri.Query).Get(0).ReplaceDefault("access_token=", "");

                        if (!string.IsNullOrEmpty(scope) && !string.IsNullOrEmpty(access_token))
                        {
                            Values = GetModel(access_token, scope);
                        }
                    }
                }

                byte[] b = Encoding.UTF8.GetBytes(GetResponse());
                context.Response.StatusCode = 200;
                context.Response.KeepAlive = false;
                context.Response.ContentLength64 = b.Length;

                System.IO.Stream output = context.Response.OutputStream;
                output.Write(b, 0, b.Length);
                context.Response.Close();

                if (Values != null)
                {
                    StopListener();
                    return Values;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates the Response for TwitchOAuth
        /// </summary>
        /// <returns>Response</returns>
        private static string GetResponse()
        {
            StringBuilder builder = new();

            builder.Append("<html>");
            builder.Append(Environment.NewLine);
            builder.Append("<head>");
            builder.Append(Environment.NewLine);
            builder.Append("<title>GeoChatter Twitch Oauth</title>");
            builder.Append(Environment.NewLine);
            builder.Append("<script language=\"JavaScript\">");
            builder.Append(Environment.NewLine);
            builder.Append("if(window.location.hash) {");
            builder.Append(Environment.NewLine);
            builder.Append("window.location.href = window.location.href.replace(\"/#\",\"?=\");");
            builder.Append(Environment.NewLine);
            builder.Append('}');
            builder.Append(Environment.NewLine);
            builder.Append("</script>");
            builder.Append(Environment.NewLine);
            builder.Append("</head>");
            builder.Append(Environment.NewLine);
            builder.Append("<body>You can close this tab</body>");
            builder.Append(Environment.NewLine);
            builder.Append("</html>");

            return builder.ToString();
        }

        /// <summary>
        /// Creates the Model to return
        /// </summary>
        /// <param name="token">Twitch Token</param>
        /// <param name="scopes">Twitch Scopes</param>
        /// <returns></returns>
        private static TwitchAuthenticationModel GetModel(string token, string scopes)
        {
            return new TwitchAuthenticationModel
            {
                Token = token,
                Scopes = scopes
            };
        }

        /// <summary>
        /// Starts the Request for the User to authorize for twitch
        /// </summary>
        /// <param name="open"></param>
        public static string SendRequestToBrowser(bool open = true)
        {
            Thread.Sleep(500);

            string urlS = GetUrl(TwitchHelper.ClientID);
            if (open)
            {
                GCUtils.OpenURL(urlS);
            }
            return urlS;
        }

        /// <summary>
        /// Returns the URL which we call to create a oauth token
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        private static string GetUrl(string ClientID)
        {
            StringBuilder sb = new("https://api.twitch.tv/kraken/oauth2/authorize");
            sb.Append("?response_type=token");
            sb.Append("&client_id=bmb7wvyrs88jdza7zo2sva60rc661t");// ClientID);
            sb.Append("&redirect_uri=").Append(ReturnUrl);
            sb.Append("&scope=chat:read+chat:edit+whispers:read+whispers:edit");
            return sb.ToString();
        }
    }
}
