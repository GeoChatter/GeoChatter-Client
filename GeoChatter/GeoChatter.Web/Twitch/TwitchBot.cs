using GeoChatter.Helpers;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Interfaces;
using GeoChatter.Core.Model;
using GeoChatter.Model.Attributes;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Web.Twitch
{
    /// <summary>
    /// Twitch Bot for handling messages and other events
    /// </summary>
    public sealed class TwitchBot : IBot<TwitchCommand, TwitchLibMessage>, IDisposable
    {
#if TWITCHBOTAPI
        public static TwitchLib.Api.Interfaces.ITwitchAPI API { get; set; }
#endif

        private static readonly ILog logger = LogManager.GetLogger(typeof(TwitchBot));

        /// <summary>
        /// Form this bot belongs to
        /// </summary>
        public IMainForm Parent { get; }

        /// <summary>
        /// Returns the <see cref="TwitchCommands.Commands"/>
        /// </summary>
        public static List<TwitchCommand> Commands => TwitchCommands.Commands;

        /// <summary>
        /// Twitch user name of the bot account
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Channel user name
        /// </summary>
        public string ChannelName { get; }

        /// <summary>
        /// Last reconnection attempt, set by MainForm
        /// </summary>
        public DateTime LastReconnectAttempt { get; set; } = DateTime.Now;

        /// <summary>
        /// Reconnection message
        /// </summary>
        public DateTime LastReconnectAttemptMessage { get; set; } = DateTime.Now;

        /// <summary>
        /// OAuth token for channel
        /// </summary>
        private string oAuthToken { get; }
        private bool sendJoin { get; }
        /// <summary>
        /// Main client instance
        /// </summary>
        private TwitchClient client { get; }

        private bool disposedValue;


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool GetEventArgObject(object e, out TwitchLibMessage message, out Type underlyingType)
        {
            underlyingType = null;
            message = null;
            if (e is OnMessageReceivedArgs m)
            {
                underlyingType = m.GetType();
                message = m.ChatMessage;
            }
            else if (e is OnWhisperReceivedArgs w)
            {
                underlyingType = w.GetType();
                message = w.WhisperMessage;
            }
            else
            {
                return false;
            }
            return message != null && underlyingType != null;
        }

        /// <summary>
        /// Get user info from <see cref="OnWhisperReceivedArgs"/> or <see cref="OnMessageReceivedArgs"/>
        /// </summary>
        /// <param name="eventargs"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="userlevel"></param>
        public bool GetUserInfo(object eventargs, out string userid, out string username, out int userlevel)
        {
            if (eventargs is OnMessageReceivedArgs m)
            {
                userid = m.ChatMessage.UserId;
                username = m.ChatMessage.Username;
                userlevel = (int)m.ChatMessage.UserType;
            }
            else if (eventargs is OnWhisperReceivedArgs w)
            {
                userid = w.WhisperMessage.UserId;
                username = w.WhisperMessage.Username;
                userlevel = (int)w.WhisperMessage.UserType;
            }
            else
            {
                userid = null;
                username = null;
                userlevel = 0;
            }
            return userid != null && username != null;
        }

        #region Events
        /// <inheritdoc/>
        public event EventHandler<GuessReceivedEventArgs> GuessReceived;
        /// <inheritdoc/>
        public event EventHandler<FlagRequestReceivedEventArgs> FlagRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<ColorRequestReceivedEventArgs> ColorRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<TargetBotEventArgs> ColorOfTargetReceived;
        /// <inheritdoc/>
        public event EventHandler<BannedUserReceivedEventArgs> BannedUserReceived;
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
        public event EventHandler<BotEventArgs> MapRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> ColorsRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> FlagsRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> FlagpacksRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> FlagpacksLinkRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> CommandsRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> ResetStatsRecieved;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> VersionRequestRecieved;
#if DEBUG
        /// <inheritdoc/>
        public event EventHandler<RandomBotGuessRecievedEventArgs> RandomBotGuessRecieved;
#endif
        /// <summary>
        /// Joined Twitch channel chat event
        /// </summary>
        public event EventHandler<ChannelJoinedEventArgs> ChannelJoined;
        /// <summary>
        /// Failed to join channel chat event
        /// </summary>
        public event EventHandler<ChannelNotJoinedEventArgs> ChannelNotJoined;
        /// <summary>
        /// Connection error event
        /// </summary>
        public event EventHandler<TwitchClient.BadStateExceptionThrownArgs> BadStateExceptionThrown;

        /// <summary>
        /// Fire <see cref="BadStateExceptionThrown"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireBadStateExceptionThrown(TwitchClient.BadStateExceptionThrownArgs args)
        {
            BadStateExceptionThrown?.Invoke(this, args);
        }
        /// <summary>
        /// Fire <see cref="ChannelNotJoined"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireChannelNotJoined(ChannelNotJoinedEventArgs args)
        {
            ChannelNotJoined?.Invoke(this, args);
        }
        /// <summary>
        /// Fire <see cref="ChannelJoined"/>
        /// </summary>
        /// <param name="args"></param>
        public void FireChannelJoined(ChannelJoinedEventArgs args)
        {
            ChannelJoined?.Invoke(this, args);
        }
#if DEBUG
        /// <inheritdoc/>
        public void FireRandomBotGuessRecieved(RandomBotGuessRecievedEventArgs args)
        {
            RandomBotGuessRecieved?.Invoke(this, args);
        }
#endif
        /// <inheritdoc/>
        public void FireGuessReceived(GuessReceivedEventArgs args)
        {
            GuessReceived?.Invoke(this, args);
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
        public void FireFlagOfTargetReceived(TargetBotEventArgs args)
        {
            FlagOfTargetReceived?.Invoke(this, args);
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
        public void FireColorOfTargetReceived(TargetBotEventArgs args)
        {
            ColorOfTargetReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireFlagRequestReceived(FlagRequestReceivedEventArgs args)
        {
            FlagRequestReceived?.Invoke(this, args);
        }
        /// <inheritdoc/>
        public void FireColorRequestReceived(ColorRequestReceivedEventArgs args)
        {
            ColorRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireLinkRequestReceived(BotEventArgs args)
        {
            LinkRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireBestRequestReceived(BotEventArgs args)
        {
            BestRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireBestOfRequestReceived(TargetBotEventArgs args)
        {
            BestOfRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireMeRequestReceived(BotEventArgs args)
        {
            MeRequestReceived?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void FireBannedUserReceived(BannedUserReceivedEventArgs args)
        {
            BannedUserReceived?.Invoke(this, args);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public TwitchBot()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="botUserName"></param>
        /// <param name="channelName"></param>
        /// <param name="twitchOAuth"></param>
        /// <param name="sendJoinMessage"></param>
        public TwitchBot(IMainForm parent, string botUserName, string channelName, string twitchOAuth, bool sendJoinMessage = true) : this()
        {
            Parent = parent;
            oAuthToken = twitchOAuth;
            Name = botUserName?.ToLowerInvariant();
            ChannelName = channelName?.ToLowerInvariant();
            sendJoin = sendJoinMessage;
            client = SetupClient();

            InitializeAndConnect();

            AttributeDiscovery.AddEventHandlers(fromMethodSource: this, toTargetInstance: client);
        }

        private static TwitchClient SetupClient()
        {
            ClientOptions clientOptions = new()
            {
                MessagesAllowedInPeriod = 1250,
                ThrottlingPeriod = TimeSpan.FromSeconds(15),
            };

            try
            {
                WebSocketClient customClient = new(clientOptions);
                return new TwitchClient(customClient);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed to connect to Twitch, please restart the app. " + e.Message);
            }
        }

        /// <summary>
        /// Initialize the client and connect
        /// </summary>
        private void InitializeAndConnect()
        {
            ConnectionCredentials credentials = new(Name, oAuthToken);
            client.Initialize(credentials, ChannelName, autoReListenOnExceptions: true);

            if (!client.Connect())
            {
                throw new InvalidOperationException("Failed to connect to Twitch! Please re-launch the game!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel"></param>
        public void JoinChannel(string channel)
        {
            client.JoinChannel(channel);
        }

        public bool? IsConnected
        {
            get
            {
                return client?.IsConnected;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect()
        {
            client?.Disconnect();
            Dispose();
        }

        /// <inheritdoc/>
        public void SendMessage(string message)
        {
            try
            {
                string msg = message;
                Task.Run(() =>
                {
                    try
                    {
                        client.SendMessage(client.GetJoinedChannel(ChannelName), msg);
                    }
                    catch (TwitchLib.Client.Exceptions.BadStateException e)
                    {
                        logger.Error(e.Message);
                        FireBadStateExceptionThrown(new(msg, e));
                    }
                });
            }
            catch (TwitchLib.Client.Exceptions.BadStateException e)
            {
                logger.Error(e.Message);
                FireBadStateExceptionThrown(new(message, e));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="msg"></param>
        public void SendWhisper(string recipient, string msg)
        {
            client.SendWhisper(recipient, msg);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="eventArgs"></param>
        public void TriggerCommands(object eventArgs)
        {
            Commands
                .Where(cmd => cmd.CanBeTriggeredWith(this, eventArgs))
                .ForEach(cmd => cmd.CallCommand(this, eventArgs));
        }

        /* 
         * Important: Definition order of the methods are taken as event handler calling order
         * Important: Event handler methods must be private and non-static
         * 
         * Options to declare a method an event handler:
         *  1- Name method same as the event name and add DiscoverableEvent attribute
         *  2- Name method matching {CustomName}_{EventName} pattern and add DiscoverableEvent attribute
         *  3- Name method whatever and add DiscoverableEvent attribute constructed with the event's name
         */

        #region Event Handlers

        [DiscoverableEvent]
        private void Client_BadStateExceptionThrown(object sender, TwitchClient.BadStateExceptionThrownArgs e)
        {
            FireBadStateExceptionThrown(e);
        }

        /// <summary>
        /// A log was sent by twitchlib
        /// </summary>
        [DiscoverableEvent]
        private void OnLog(object sender, OnLogArgs e)
        {
            // Do stuff with log args
        }

        /// <summary>
        /// Another event handler for OnLog
        /// </summary>
        [DiscoverableEvent]
        private void CustomHandlerName_OnLog(object sender, OnLogArgs e)
        {
            // Do stuff with log args
        }

        [DiscoverableEvent]
        private void Custom_OnFailureToReceiveJoinConfirmation(object sender, OnFailureToReceiveJoinConfirmationArgs e)
        {
            FireChannelNotJoined(new(e.Exception.Channel, e.Exception.Details));
        }

        /// <summary>
        /// Another event handler for OnLog
        /// </summary>
        [DiscoverableEvent(nameof(TwitchClient.OnLog))] // Use 'nameof' for easy documentation access to event
        private void CustomHandlerName(object sender, OnLogArgs e)
        {
            // Do stuff with log args
        }

        /// <summary>
        /// Client connected to twitch
        /// </summary>
        [DiscoverableEvent]
        private void OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {ChannelName}");

        }

        [DiscoverableEvent]
        private void OnUserBanned(object sender, OnUserBannedArgs e)
        {
            FireBannedUserReceived(new BannedUserReceivedEventArgs(e.UserBan.TargetUserId));
        }



        /// <summary>
        /// Bot joined the channel(connected)
        /// </summary>
        [DiscoverableEvent]
        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            string joinMessage = LanguageStrings.Get("Chat_Msg_joinMessage");
            if (sendJoin)
            {
                client.SendMessage(e.Channel, joinMessage);
            }

            FireChannelJoined(new ChannelJoinedEventArgs(e.Channel, e.BotUsername));

        }

        /// <summary>
        /// Chat message handling
        /// </summary>
        [DiscoverableEvent]
        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            TriggerCommands(e);
        }

        /// <summary>
        /// Whisper handling
        /// </summary>
        //[DiscoverableEvent]
        //private void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex) { logger.Error(ex.Summarize()); }


        //}

        #endregion

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (client != null && client.IsConnected)
                    {
                        client.Disconnect();
                    }
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
