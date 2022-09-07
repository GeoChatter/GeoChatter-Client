using GeoChatter.Helpers;
using GeoChatter.Core.Interfaces;
using GeoChatter.Core.Model;
using GeoChatter.Model.Attributes;
using Google.Apis.YouTube.v3.Data;
using log4net;
using StreamingClient.Base.Model.OAuth;
using StreamingClient.Base.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using YouTube.Base;
using YouTube.Base.Clients;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Helpers;

namespace GeoChatter.Web.YouTube
{
    /// <summary>
    /// Youtube Bot for handling messages and other events
    /// </summary>
    public sealed class YoutubeBot : IBot<YoutubeCommand, LiveChatMessage>
    {
        /// <summary>
        /// Returns the <see cref="YoutubeCommands.Commands"/>
        /// </summary>
        public static List<YoutubeCommand> Commands => YoutubeCommands.Commands;
        private static readonly ILog logger = LogManager.GetLogger(typeof(YoutubeBot));
        internal static string clientID = "203792612305-75jqonist070pvuniqr4j884sgmi7gve.apps.googleusercontent.com";
        const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        const string tokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
        const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";
        /// <summary>
        /// Youtube client secret
        /// </summary>
        public static string ClientSecret { get; set; }

        //private static readonly SemaphoreSlim fileLock = new(1);

        /// <summary>
        /// Scopes for auth
        /// </summary>
        public static readonly List<OAuthClientScopeEnum> scopes = new()
        {
            OAuthClientScopeEnum.ChannelMemberships,
            OAuthClientScopeEnum.ManageAccount,
            OAuthClientScopeEnum.ManageData,
            OAuthClientScopeEnum.ManagePartner,
            OAuthClientScopeEnum.ManagePartnerAudit,
            OAuthClientScopeEnum.ManageVideos,
            OAuthClientScopeEnum.ReadOnlyAccount,
            //OAuthClientScopeEnum.ViewAnalytics,
            //OAuthClientScopeEnum.ViewMonetaryAnalytics
        };
        private ChatClient client;
        private bool disposedValue;

        /// <summary>
        /// Wheter the bot is connected
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Form this bot belongs to
        /// </summary>
        public IMainForm Parent { get; }

        /// <summary>
        /// 
        /// </summary>
        public YoutubeBot() {
            Initialize();
        }

        /// <summary>
        /// Get event arguments object as <see cref="LiveChatMessage"/>
        /// </summary>
        /// <param name="e"></param>
        /// <param name="message"></param>
        /// <param name="underlyingType"></param>
        public bool GetEventArgObject([NotNullWhen(true)] object e, out LiveChatMessage message, out Type underlyingType)
        {
            message = null;
            underlyingType = null;
            if (e is LiveChatMessage l)
            {
                message = l;
                underlyingType = l.GetType();
                return underlyingType != null;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get user info from <see cref="LiveChatMessage"/>
        /// </summary>
        /// <param name="eventargs"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="userlevel"></param>
        public bool GetUserInfo(object eventargs, out string userid, out string username, out int userlevel)
        {
            if (eventargs is LiveChatMessage m)
            {
                userid = m.AuthorDetails.ChannelId;
                username = m.AuthorDetails.DisplayName;
                userlevel = (m.AuthorDetails.IsChatOwner ?? false)
                    ? (int)CommonUserLevel.Broadcaster
                    : (m.AuthorDetails.IsChatModerator ?? false)
                        ? (int)CommonUserLevel.Moderator
                        : (int)CommonUserLevel.Viewer;
            }
            else
            {
                userid = null;
                username = null;
                userlevel = 0;
            }
            return userid != null && username != null;
        }

        #region Custom Events

        /// <inheritdoc/>
        public event EventHandler<GuessReceivedEventArgs> GuessReceived;
        /// <inheritdoc/>
        public event EventHandler<FlagRequestReceivedEventArgs> FlagRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<ColorRequestReceivedEventArgs> ColorRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<TargetBotEventArgs> ColorOfTargetReceived;
        /// <inheritdoc/>
        public event EventHandler<TargetBotEventArgs> FlagOfTargetReceived;
        /// <inheritdoc/>
        public event EventHandler<RandomGuessRecievedEventArgs> RandomGuessRecieved;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> MeRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> BestRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<TargetBotEventArgs> BestOfRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> LinkRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> ColorsRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> FlagsRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> FlagpacksRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> FlagpacksLinkRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> MapRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> GcRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> CommandsRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> ResetStatsRecieved;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> VersionRequestRecieved;
        /// <inheritdoc/>
        public event EventHandler<BannedUserReceivedEventArgs> BannedUserReceived;

#if DEBUG
        /// <inheritdoc/>
        public event EventHandler<RandomBotGuessRecievedEventArgs> RandomBotGuessRecieved;

        /// <inheritdoc/>
        public void FireRandomBotGuessRecieved(RandomBotGuessRecievedEventArgs args)
        {
            RandomBotGuessRecieved?.Invoke(this, args);
        }
#endif

        /// <summary>
        /// Fire <see cref="BannedUserReceived"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireBannedUserReceived(BannedUserReceivedEventArgs args)
        {
            BannedUserReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireColorsRequestReceived(BotEventArgs args)
        {
            ColorsRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireFlagsRequestReceived(BotEventArgs args)
        {
            FlagsRequestReceived?.Invoke(this, args);
        }
        /// <inheritdoc/>
        public void FireFlagpacksRequestReceived(BotEventArgs args)
        {
            FlagpacksRequestReceived?.Invoke(this, args);
        }
        /// <inheritdoc/>
        public void FireFlagpacksLinkRequestReceived(BotEventArgs args)
        {
            FlagpacksLinkRequestReceived?.Invoke(this, args);
        }
        /// <inheritdoc/>
        public void FireCommandsRequestReceived(BotEventArgs args)
        {
            CommandsRequestReceived?.Invoke(this, args);
        }
        /// <inheritdoc/>
        public void FireGuessReceived(GuessReceivedEventArgs args)
        {
            GuessReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireGcRequestReceived(BotEventArgs args)
        {
            GcRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireMapRequestReceived(BotEventArgs args)
        {
            MapRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireRandomGuessRecieved(RandomGuessRecievedEventArgs args)
        {
            RandomGuessRecieved?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireFlagOfTargetReceived(TargetBotEventArgs args)
        {
            FlagOfTargetReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireFlagRequestReceived(FlagRequestReceivedEventArgs args)
        {
            FlagRequestReceived?.Invoke(this, args);
        }
        /// <inheritdoc/>
        public void FireResetStatsRecieved(BotEventArgs args)
        {
            ResetStatsRecieved?.Invoke(this, args);
        }
        /// <inheritdoc/>
        public void FireVersionRequestRecieved(BotEventArgs args)
        {
            VersionRequestRecieved?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireColorRequestReceived(ColorRequestReceivedEventArgs args)
        {
            ColorRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireColorOfTargetReceived(TargetBotEventArgs args)
        {
            ColorOfTargetReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireLinkRequestReceived(BotEventArgs args)
        {
            LinkRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireBestOfRequestReceived(TargetBotEventArgs args)
        {
            BestOfRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireBestRequestReceived(BotEventArgs args)
        {
            BestRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireMeRequestReceived(BotEventArgs args)
        {
            MeRequestReceived?.Invoke(this, args);
        }

        #endregion

        public async Task<string> LoginAndGetRefreshToken()
        {
            return await Task.Run(async () =>
            {
                YouTubeConnection connection = await YouTubeConnection.ConnectViaLocalhostOAuthBrowser(clientID, ClientSecret, scopes);
                OAuthTokenModel token = connection.GetOAuthTokenCopy();
                return token.refreshToken;
            });
        }

        /// <summary>
        /// Initialize the bot
        /// </summary>
        public void Initialize()
        {
            Logger.SetLogLevel(StreamingClient.Base.Util.LogLevel.Debug);

            Logger.LogOccurred += Logger_LogOccurred;
            Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("Initializing connection");

                   // YouTubeConnection connection = await YouTubeConnection.ConnectViaLocalhostOAuthBrowser(clientID, ClientSecret, scopes);
                     string token = $"{{\"$type\":\"StreamingClient.Base.Model.OAuth.OAuthTokenModel, StreamingClient.Base\",\"clientID\":\"{clientID}\",\"clientSecret\":\"{ClientSecret}\"," +
                    $"\"authorizationCode\":\"4/0AX4XfWh4VhrTZ9IX9HJkWDx2GOMqK0hq309gzHmjDFZmf8aRb5azf_Umi54w0w02eF1rGw\"," +
             /// refreshtoken here
                    $"\"refresh_token\":\"1//09_XVoeHf8KTQCgYIARAAGAkSNwF-L9IrhahhK-E877Te6mlC_CysT1pyr2y_4LeKfXIr52us5nog_3UXcJ54RhJdqc2Xe0_mTFE\"," +
                    $"\"access_token\":\"ya29.A0ARrdaM9KCkNGgoio2ndL9msCQsoj31Htn6Re-WmtzC2vr5UzQwnpGtTZkb0TOeEbyLF6mnFWwR6GNREAqIld2-uy-74QvGP3_2XiQcpGEz9ikcCR-6bs8A1FIyOpL8EBUcEHInJTG84V5LBOnz77N9SzU-IR\"," +
                    "\"expires_in\":3599,\"redirectUrl\":null,\"AcquiredDateTime\":\"2022-08-09T12:37:01.7811386+01:00\"}";

                    OAuthTokenModel toke = JSONSerializerHelper.DeserializeFromString<OAuthTokenModel>(token);

                    YouTubeConnection connection = await YouTubeConnection.ConnectViaOAuthToken(toke);
                    
                    if (connection != null)
                    {
                        Channel channel = await connection.Channels.GetMyChannel();
                        
                        //Channel channel = await connection.Channels.GetChannelByID("UC1A8L5H6gBXNGfvWcE2Yuyw");

                        if (channel != null)
                        {
                            Console.WriteLine("Connection successful. Logged in as: " + channel.Snippet.Title);

                            LiveBroadcast broadcast = await connection.LiveBroadcasts.GetChannelActiveBroadcast(channel);

                            Console.WriteLine("Connecting chat client!");
                            
                            client = new ChatClient(connection);
                            AttributeDiscovery.AddEventHandlers(fromMethodSource: this, toTargetInstance: client);
                            
                            //if (await client.Connect())
                            if (await client.Connect(broadcast))
                            {
                                
                                Connected = true;
                                Console.WriteLine("Live chat connection successful!");

                                if (await connection.LiveBroadcasts.GetMyActiveBroadcast() != null)
                                {
                                    Connected = true;
                                    string joinMessage = LanguageStrings.Get("Chat_Msg_joinMessage");
                                    SendMessage(joinMessage);
                                    

                                }

                                while (true) { }
                            }
                            else
                            {
                                Console.WriteLine("Failed to connect to live chat");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            });

            // System.Console.ReadLine();
        }

        /// <inheritdoc/>
        public void SendMessage(string message)
        {
            if (!string.IsNullOrEmpty(message) && Connected && client != null && client.Broadcast != null)
            {
                client.SendMessage(message);
                
            }
        }

        public void pollMessages()
        {
            
        }

        public async void Disconnect()
        {
            if(Connected && client != null && client.Broadcast != null)
                await client.Disconnect();
        }
        [DiscoverableEvent]
        private void Client_OnMessagesReceived(object sender, IEnumerable<LiveChatMessage> messages)
        {
            foreach (LiveChatMessage message in messages)
            {
                try
                {
                    if (message.Snippet.HasDisplayContent.GetValueOrDefault())
                    {
                        TriggerCommands(message);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                }
            }
        }

        private static void Logger_LogOccurred(object sender, Log log)
        {
            if (log.Level >= StreamingClient.Base.Util.LogLevel.Error)
            {
                logger.Error(log.Message);
            }

            //await fileLock.WaitAndRelease(async () =>
            //{
            //    try
            //    {
            //        using StreamWriter writer = new(File.Open("Log.txt", FileMode.Append));

            //        await writer.FlushAsync();
            //    }
            //    catch (Exception) { }
            //});
        }

        /// <summary>
        /// Trigger existing twitch bot commands with given event arguments
        /// </summary>
        /// <param name="eventArgs">Event arguments object to be cast later</param>
        public void TriggerCommands(object eventArgs)
        {
            Commands
                .Where(cmd => cmd.CanBeTriggeredWith(this, eventArgs))
                .ForEach(cmd => cmd.CallCommand(this, eventArgs));
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client?.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
