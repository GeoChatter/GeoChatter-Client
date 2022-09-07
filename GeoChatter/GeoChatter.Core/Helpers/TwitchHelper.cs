using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoChatter.Model;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// Helper methods for Twitch API
    /// </summary>
    public static class TwitchHelper
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TwitchHelper));
        internal static string ClientID = "gcq217zi692u0dszhvjfttroy2ozvo";

        /// <summary>
        /// Secret token for access token requests
        /// </summary>
        public static string ClientSecret { get; set; }

        private static RestClient twitchRestClient { get; set; } = new();
        private static string accessToken;
        /// <summary>
        /// Get Twitch access token with scope user:read:email
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task<string> TwitchAccessToken()
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                return accessToken;
            }

            try
            {   
                if (string.IsNullOrEmpty(ClientSecret))
                {
                    throw new InvalidOperationException($"{nameof(ClientSecret)} is not set!");
                }

                // TODO: Do this on the server
                string url = "https://id.twitch.tv/oauth2/token?" +
                    $"client_id={ClientID}&" +
                    $"client_secret={ClientSecret}&" +
                    "grant_type=client_credentials&" +
                    "scope=user:read:email";


                RestRequest request = new(url, Method.Post);
                request.AddHeader("Accept", "application/json");

                RestResponse resp = await twitchRestClient.ExecuteAsync(request).ConfigureAwait(false);
                if (!resp.IsSuccessful)
                {
                    return null;
                }
                JObject obj = JsonConvert.DeserializeObject<JObject>(resp.Content);
                accessToken = obj.GetValueDefault("access_token").Value<string>();
                if (!string.IsNullOrEmpty(accessToken))
                {
                    return accessToken;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);

            }
            return null;
        }

        /// <summary>
        /// Get Twitch user data from Helix API as a <see cref="JToken"/>
        /// </summary>
        /// <param name="urlAttachment"></param>
        /// <param name="bearer"></param>
        /// <returns></returns>
        public static JToken TwitchUserData(string urlAttachment, string bearer)
        {
            RestRequest request = new("https://api.twitch.tv/helix/users?" + urlAttachment, Method.Get);


            request.AddHeader("Authorization", "Bearer " + bearer);
            request.AddHeader("Client-Id", ClientID);

            RestResponse resp = twitchRestClient.Execute(request);
            return !resp.IsSuccessful ? null : ((JObject)JsonConvert.DeserializeObject(resp.Content)).GetValueDefault("data");
        }

        /// <summary>
        /// Get Twitch user data values in a list
        /// </summary>
        /// <param name="urlAttachment"></param>
        /// <returns></returns>
        public static async Task<List<string>> GetTwitchDataList(string urlAttachment)
        {
            string token = await TwitchAccessToken();
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            JToken playerObj = TwitchUserData(urlAttachment, token);
            if (playerObj == null)
            {
                return null;
            }

            if (!playerObj.Any())
            {
                return null;
            }

            JObject inner = playerObj[0]?.Value<JObject>();
            return inner?.Values().Select(p => p.Value<string>()).ToList();
        }

        /// <summary>
        /// Get Twitch user data as a <see cref="Player"/> instance
        /// </summary>
        /// <param name="userId">Twitch ID</param>
        /// <param name="userName">Backup Twitch user name if <paramref name="userId"/> is not given</param>
        /// <returns></returns>
        public static async Task<Player> GetUserDataFromTwitch(string userId = "", string userName = "")
        {
            Player player = new();
            string urlAttachment = string.Empty;
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(userName))
            {
                return player;
            }

            if (!string.IsNullOrEmpty(userId))
            {
                urlAttachment = "id=" + userId;
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                urlAttachment = "login=" + userName.ToLowerInvariant();
            }
            try
            {
                List<string> values = await GetTwitchDataList(urlAttachment).ConfigureAwait(false);
                if (values == null)
                {
                    logger.Warn($"Twitch data empty for: {urlAttachment}");
                    return null;
                }

                string login = values[1];
                string displayname = values[2];
                string type = values[3];
                string broadcaster_type = values[4];
                string desc = values[5];
                string online_img = values[6];
                string offline_img = values[7];
                player.PlatformId = values[0];
                player.ProfilePictureUrl = online_img;
                player.DisplayName = displayname;
                player.PlayerName = login;
                return player;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return player;
            }
        }



    }
}
