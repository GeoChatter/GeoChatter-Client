using GeoChatter.Core.Interfaces;
using GeoChatter.Integrations.Classes;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using GeoChatter.Integrations.StreamerBot;
using GeoChatter.Core.Model;
using System.Diagnostics.CodeAnalysis;
using GeoChatter.Core.Common.Extensions;
using CefSharp.DevTools.IO;
using GeoChatter.Helpers;
using System.Windows;

using GuessServerApiInterfaces;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using GeoChatter.Model.Enums;
using GeoChatter.Core.Helpers;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxOptions = System.Windows.Forms.MessageBoxOptions;
using System.Windows.Interop;


namespace GeoChatter.Integrations
{
    /// <summary>
    /// StreamerBot client model
    /// </summary>
    public class StreamerbotClient : IBot<StreamerBotCommand, StreamerBotCommandMessagePart>, IDisposable
    {
        /// </summary>
        public IMainForm Parent { get { return parent; }  }
        private IMainForm parent;
        private bool sendJoin;
        /// <summary>
        /// Returns the <see cref="TwitchCommands.Commands"/>
        /// </summary>
        public static List<StreamerBotCommand> Commands => StreamerBotCommands.Commands;

        #region basics
        /// <summary>
        /// Action recieve event
        /// </summary>
        public event EventHandler<ActionsReceivedEventArgs> ActionsReceived;
        public event EventHandler<GuessReceivedEventArgs> GuessReceived;
        public event EventHandler<FlagRequestReceivedEventArgs> FlagRequestReceived;
        public event EventHandler<TargetBotEventArgs> FlagOfTargetReceived;
        public event EventHandler<RandomGuessRecievedEventArgs> RandomGuessRecieved;
        public event EventHandler<ColorRequestReceivedEventArgs> ColorRequestReceived;
        public event EventHandler<TargetBotEventArgs> ColorOfTargetReceived;
        public event EventHandler<BotEventArgs> MeRequestReceived;
        public event EventHandler<BotEventArgs> BestRequestReceived;
        public event EventHandler<TargetBotEventArgs> BestOfRequestReceived;
        public event EventHandler<BotEventArgs> MapRequestReceived;
        public event EventHandler<BotEventArgs> LinkRequestReceived;
        public event EventHandler<BotEventArgs> ColorsRequestReceived;
        public event EventHandler<BotEventArgs> FlagsRequestReceived;
        public event EventHandler<BotEventArgs> CommandsRequestReceived;
        public event EventHandler<BotEventArgs> ResetStatsRecieved;
        public event EventHandler<BotEventArgs> VersionRequestRecieved;
        public event EventHandler<BannedUserReceivedEventArgs> BannedUserReceived;
#if DEBUG
       public event EventHandler<RandomBotGuessRecievedEventArgs> RandomBotGuessRecieved;
#endif
        public event EventHandler<BotEventArgs> FlagpacksLinkRequestReceived;
        /// <inheritdoc/>
        public event EventHandler<BotEventArgs> FlagpacksRequestReceived;
        private static readonly ILog logger = LogManager.GetLogger(typeof(StreamerbotClient));
        private WebSocketSharp.WebSocket ws;
        private List<StreamerbotAction> actions = new List<StreamerbotAction>();
        /// <summary>
        /// Returns a list of StreamerbotActions
        /// </summary>
        public List<StreamerbotAction> Actions { get { return actions; } }


        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);
        private Uri adress;
        private string sendMessageActionGuid;
        private string sendMessageActionName;
        private bool sendChatMessages;
        private bool isInitialStart = false;

        /// <summary>
        /// Connect to <paramref name="ip"/>:<paramref name="port"/>
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public async Task<bool> Connect(string ip, string port, string sendMessageActionGuid, string sendMessageActionName, bool sendChatMessages, IMainForm mainForm, bool sendJoinMessage = true)
        {
            isInitialStart = true;
            parent = mainForm;
            sendJoin = sendJoinMessage;
            if (ws == null)
            {
                if (string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
                {
                    return false;
                }

                adress = new Uri($"ws://{ip}:{port}/");

                ws = new WebSocketSharp.WebSocket(adress.ToStringDefault());
            }
            bool success = false;
            this.sendMessageActionGuid = sendMessageActionGuid;
            this.sendMessageActionName = sendMessageActionName;
            this.sendChatMessages = sendChatMessages;
            AttributeDiscovery.AddEventHandlers(fromMethodSource: this, toTargetInstance: this);

            SubscribeToWS();
            
            if (ws.ReadyState != WebSocketState.Open)
            {
                try
                {
                    ws.Connect();
                    while(ws != null && ws.ReadyState != WebSocketState.Open)    
                        Thread.Sleep(500);
                    if (ws != null && sendJoin)
                    {
                        SendMessage(LanguageStrings.Get("Chat_Msg_joinMessage"));
                    }
                }
                catch (Exception e)
                {
                    string t = e.Message;
                }
                //ExitEvent.WaitOne();
            }
            InitiateHeartBeat();

            SubscribeToEvents();
            isInitialStart = false;
            ConnectionSucceeded = true;
            return ws != null && ws.ReadyState == WebSocketState.Open;

        }
        private void SubscribeToWS()
        {
            //ws.ReconnectionHappened.Subscribe(s =>
            //{
            //    if (reconnectedAlready == false)
            //    {
            //        if ((s.Type == ReconnectionType.Error))
            //        {
            //            logger.Debug("Streamerbot disconnected, reconnected");
            //            // SubscribeToEvents();
            //            reconnectedAlready = true;
            //            MessageBox.Show("Reconnected to Streamer.Bot");
            //        }
            //        if (sendJoin)
            //        {
            //            SendMessage(LanguageStrings.Get("Chat_Msg_joinMessage"));
            //        }
            //    }
            //    else
            //    {
            //        reconnectedAlready = !reconnectedAlready;
            //    }
            //    ConnectionSucceeded = true;
            //});

            ws.OnMessage +=(sender, e) =>
            {
                //success = ws.IsStarted;
                if (!e.Data.ToLowerInvariant().Contains("\"id\":\"0\",\"events\":", StringComparison.InvariantCulture)
                   && e.Data.ToLowerInvariant().Contains("\"source\":\"command\"", StringComparison.InvariantCulture))
                {
                    StreamerBotCommandMessage command = JsonConvert.DeserializeObject<StreamerBotCommandMessage>(e.Data);

                    TriggerCommands(command);
                }
                if (!e.Data.ToLowerInvariant().Contains("\"id\":\"0\",\"events\":", StringComparison.InvariantCulture)
                && e.Data.ToLowerInvariant().Contains("\"source\":\"raw\"", StringComparison.InvariantCulture))
                {
                    StreamerBotActionMessage action = JsonConvert.DeserializeObject<StreamerBotActionMessage>(e.Data);
                    if (action.@event.type.ToLowerInvariant() == "subaction")
                        logger.Debug("Subaction execution resceived: " + action.data.name);
                    else
                        logger.Debug("Action execution resceived: " + action.data.name);
                }
                if (!e.Data.ToLowerInvariant().Contains("\"id\":\"0\",\"events\":", StringComparison.InvariantCulture)
                    && e.Data.ToLowerInvariant().Contains("count", StringComparison.InvariantCulture) && e.Data.ToLowerInvariant().Contains("actions", StringComparison.InvariantCulture))
                {
                    StreamerbotActionList list = JsonConvert.DeserializeObject<StreamerbotActionList>(e.Data);
                    ActionsReceivedEventArgs args = new();
                    args.Actions.AddRange(list.actions);
                    OnActionsReceived(args);
                };
                if (!e.Data.ToLowerInvariant().Contains("\"id\":\"0\",\"events\":", StringComparison.InvariantCulture)
                    && e.Data.ToLowerInvariant().Contains("custom", StringComparison.InvariantCulture))
                {
                    StreamerBotCustomMessage @event = JsonConvert.DeserializeObject<StreamerBotCustomMessage>(e.Data);
                    logger.Debug("Custom Event received: " + @event.Data.Data);
                    if (@event.Data.Data == "TimerEnd")
                    {
                        parent.ToggleGuesses();
                    }
                };
                if (e.IsPing || e.Data.ToLowerInvariant().Contains("\"id\":\"0\",\"events\":", StringComparison.InvariantCulture))
                {
                    logger.Debug("Heartbeat successful");
                    ConnectionSucceeded = true;

                };
            };
            ws.EmitOnPing = true;
            
            ws.OnError += (sender, e) => {
                string t = e.Message;
            
            };
            ws.OnClose += (sender, e) => {
                // sb close: 1005, clean
                //sb closed on start: 1006, false, text: An exception has occurred while connecting.
                //sb crashes: 1006, false, text: An exception has occurred while receiving.
                if(e.WasClean)
                    reconnectWanted = MessageBox.Show("Connection to Streamer.Bot lost! Do you want to try to reconnect?", "Connection lost", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                if (!e.WasClean && e.Reason.ToLowerInvariant() == "an exception has occurred while connecting." && !reconnectWanted)
                {
                    MessageBox.Show("Could not connect to Streamer.Bot\r\nPlease make sure that IP and port are correct,\rthat Streamer.Bot and its websocket server are running!\rThen restart GeoChatter!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    ws = null;
                }

                if (!reconnectWanted && !e.WasClean && e.Reason.ToLowerInvariant() == "an exception has occurred while receiving.")
                { 
                    reconnectWanted = MessageBox.Show("Connection to Streamer.Bot unexpectedly lost! Do you want to try to reconnect?", "Connection lost", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                }
                
                
            };
            //ws.DisconnectionHappened.Subscribe(async e =>
            //{
            //    logger.Debug("Streamerbot disconnected, reconnecting....");

            //    if ((e.Type == DisconnectionType.ByServer && e.CloseStatus == WebSocketCloseStatus.NormalClosure))
            //    {
            //        MessageBox.Show("Connection closed by Streamer.Bot.\r\nTrying to reconnect...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            //    }
            //    if (e.Type == DisconnectionType.Lost && e.CloseStatus != WebSocketCloseStatus.NormalClosure)
            //    {
            //        MessageBox.Show("Connection to Streamer.Bot lost.\r\nTrying to reconnect...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            //    }
            //    if (e.Type == DisconnectionType.Error && e.Exception.Message == "Unable to connect to the remote server")
            //    {
            //        MessageBox.Show("Could not connect to Streamer.Bot\r\nPlease make sure that IP and port are correct,\rthat Streamer.Bot and its websocket server are running!\rThen restart GeoChatter!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            //        e.CancelReconnection = true;
            //    }
            //    ConnectionSucceeded = false;
            //    //if(ws.IsRunning || ws.IsStarted)
            //    //ws.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "");
            //    //ws.Reconnect();
            //    //Thread.Sleep(5000);
            //});
            
        }

        private bool reconnectWanted = false;
        public bool ConnectionSucceeded { get; set; } = false;
        /// <summary>
        /// Fires <see cref="ActionsReceived"/>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnActionsReceived(ActionsReceivedEventArgs e)
        {
            logger.Debug($"Received actions");
            if (actions.Any())
                actions.Clear();
            actions.AddRange(e.Actions);    
            ActionsReceived?.Invoke(this, e);
        }
        System.Threading.Timer timer;

        private void InitiateHeartBeat()
        {
            timer = new System.Threading.Timer(async (state) =>
            {
                if (ws != null && ws.ReadyState != WebSocketState.Open && reconnectWanted)
                {
                    ws.Connect();
                   
                    while(ws.ReadyState != WebSocketState.Open)
                        Thread.Sleep(500);
                    SubscribeToEvents();
                    if (sendJoin)
                    {
                        SendMessage(LanguageStrings.Get("Chat_Msg_joinMessage"));
                        reconnectWanted = false;
                    }
                    
                }
                if (ws != null && ws.ReadyState == WebSocketState.Open)
                {
                    ws.Ping();
                }


            }, null, 1000, 10000);
            //{ "request": "GetEvents", "id": "<message id>"}
            
        }


        /// <summary>
        /// Send GetActions request through web socket
        /// </summary>
        public void GetActions()
        {
            logger.Debug($"Getting actions");
            if (ws != null && ws.ReadyState != WebSocketState.Open)
            {
                ws.Connect();
            }
            if (ws != null && ws.ReadyState == WebSocketState.Open)
            {
                ws.Send("{\"request\":\"GetActions\",\"id\":\"123\"}");
            }
        }


        /// <summary>
        /// Send GetActions request through web socket
        /// </summary>
        public void SubscribeToEvents()
        {
            logger.Debug($"Subscribing to Streamer.Bot events");
            if (ws != null && ws.ReadyState != WebSocketState.Open)
            {
                ws.Connect();
            }
            if (ws != null && ws.ReadyState == WebSocketState.Open)
            {
                string requestString = "{\"request\":\"Subscribe\",\"events\":{\"Twitch\":[\"RewardRedemption\"],  \"command\": [\"Message\", \"Whisper\", \"BotWhisper\"],  \"general\": [\"Custom\"],  \"raw\": [\"Action\", \"SubAction\"]},\"id\":\"123\"}";
                ws.Send(requestString);
            }
        }

        /// <summary>
        /// Send DoAction request with given arguments through web socket
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public void ExecuteAction(string guid, string name, Dictionary<string, string> args)
        {
            if (ws != null && ws.ReadyState != WebSocketState.Open)
            {
                ws.Connect();
            }
            if (ws != null && ws.ReadyState == WebSocketState.Open)
            {
                string argsString = JsonConvert.SerializeObject(args);
                string reqString = @"{""request"": ""DoAction"",""action"": { ""id"": """ + guid + @""", ""name"": """ + name + @""" }, ""args"":  " + argsString +@", ""id"":""123""}";
                logger.Debug($"Executing action: {reqString}");
                ws.Send(reqString);
            }
        }
        /// <summary>
        /// Sends a chat command to the specified action
        /// </summary>
        /// <param name="chatActionGuid"></param>
        /// <param name="chatActionName"></param>
        /// <param name="message"></param>
        //public void SendChatMessage(string chatActionGuid, string chatActionName, string message)
        //{
        //    ExecuteAction(chatActionGuid, chatActionName, new Dictionary<string, string>() { { "message", message } });    
        //}


        /// <summary>
        /// Test connection
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool TestConnection(string ip, string port)
        {
            logger.Debug($"Testing Streamerbot connection");
            if (ws == null)
            {
                ws = new WebSocketSharp.WebSocket($"ws://{ip}:{port}/");
            }

            bool success;

            if (ws.ReadyState != WebSocketState.Open)
            {
                ws.Connect();
            }

            success = ws.ReadyState == WebSocketState.Open;
            logger.Debug($"SB connection is {success}");
            if (ws.ReadyState == WebSocketState.Open)
            {
                ws.CloseAsync(CloseStatusCode.Normal, "");
            }

            return success;
        }

        /// <summary>
        /// Close web socket connection
        /// </summary>
        public async Task<bool> CloseConnection()
        {
            logger.Debug($"Closing Streamer.Bot connection");
            if (ws != null)
            {
                ws.CloseAsync(CloseStatusCode.Normal,"");
                if (adress != null)
                    ws = new WebSocketSharp.WebSocket(adress.ToStringDefault());
                return ws.ReadyState == WebSocketState.Open;
            }
            ConnectionSucceeded = false;
            return true;
        }

        /// <summary>
        /// <see cref="ExecuteAction(string, string, Dictionary{string, string})"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void ExecuteAction(string id, string name)
        {
            ExecuteAction(id, name, new Dictionary<string, string>());
        }

        /// <summary>
        /// Wheter connection is still alive
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsRunning()
        {
            try
            {
                if(ws != null)
                   ws.Send("ping");
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
                
        }
#endregion
#region botInterface

        public bool GetEventArgObject([NotNullWhen(true)] object e, [NotNullWhen(true)] out StreamerBotCommandMessagePart message, [NotNullWhen(true)] out Type underlyingType)
        {
              underlyingType = null;
            message = null;
            if (e is StreamerBotCommandMessage m)
            {
                underlyingType = m.GetType();
                message = m.data;
            }
            //else if (e is OnWhisperReceivedArgs w)
            //{
            //    underlyingType = w.GetType();
            //    command = w.WhisperMessage;
            //}
            else
            {
                return false;
            }
            return message != null && underlyingType != null;
        }

        public bool GetUserInfo(object eventargs, [NotNullWhen(true)] out string userid, [NotNullWhen(true)] out string username, out int userlevel, out Platforms userPlatform)
        {
            if (eventargs == null)
            {
                userid = username = string.Empty;
                userlevel = 0;
                userPlatform = Platforms.Unknown;
                return false;
            }
            username = ((StreamerBotCommandMessage)eventargs).data.user.name;
            userid = ((StreamerBotCommandMessage)eventargs).data.user.id.ToStringDefault();
            userlevel = ((StreamerBotCommandMessage)eventargs).data.user.role;
            switch (((StreamerBotCommandMessage)eventargs).data.user.type.ToLowerInvariant())
            {
                case "twitch":
                default:
                    userPlatform = Platforms.Twitch;
                    break;
                case "youtube":
                    userPlatform = Platforms.YouTube;
                    break;

            }

            return true;
        }

        public void SendMessage(string message)
        {
            if(sendChatMessages && !string.IsNullOrEmpty(sendMessageActionGuid) && !string.IsNullOrEmpty(sendMessageActionName))
                ExecuteAction(sendMessageActionGuid, sendMessageActionName, new Dictionary<string, string>() { { "message", message } });
            
        }

        public void SetChatAction(string actionGuid, string actionName)
        {
            if (sendMessageActionGuid != actionGuid && sendMessageActionName != actionName)
            {
                sendMessageActionGuid = actionGuid;
                sendMessageActionName = actionName;
                if (sendJoin)
                {
                    SendMessage(LanguageStrings.Get("Chat_Msg_joinMessage"));
                }
            }
        }

        public void TriggerCommands(object eventArgs)
        {
            Commands
               .Where(cmd => cmd.CanBeTriggeredWith(this, eventArgs))
               .ForEach(cmd => cmd.CallCommand(this, eventArgs));
        }
#if DEBUG
        public void FireRandomBotGuessRecieved(RandomBotGuessRecievedEventArgs args)
        {
            
            RandomBotGuessRecieved?.Invoke(this, args);
        }
#endif
        public void FireFlagsRequestReceived(BotEventArgs args)
        {
            
            FlagsRequestReceived?.Invoke(this, args);
        }

        public void FireFlagpacksRequestReceived(BotEventArgs args)
        {
            
            FlagpacksRequestReceived?.Invoke(this, args);
        }

        public void FireFlagpacksLinkRequestReceived(BotEventArgs args)
        {
            
            FlagpacksLinkRequestReceived?.Invoke(this, args);
        }

        public void FireCommandsRequestReceived(BotEventArgs args)
        {
            
            CommandsRequestReceived?.Invoke(this, args);
        }

        public void FireVersionRequestRecieved(BotEventArgs args)
        {
            
            VersionRequestRecieved?.Invoke(this, args);
        }

        public void FireResetStatsRecieved(BotEventArgs args)
        {
            
            ResetStatsRecieved?.Invoke(this, args);
        }

        public void FireColorsRequestReceived(BotEventArgs args)
        {
            
            ColorsRequestReceived?.Invoke(this, args);
        }

        public void FireMapRequestReceived(BotEventArgs args)
        {
            
            MapRequestReceived?.Invoke(this, args);
        }

        public void FireLinkRequestReceived(BotEventArgs args)
        {
            
            LinkRequestReceived?.Invoke(this, args);
        }

        public void FireBestRequestReceived(BotEventArgs args)
        {
            
            BestRequestReceived?.Invoke(this, args);
        }

        public void FireBestOfRequestReceived(TargetBotEventArgs args)
        {
            
            BestOfRequestReceived?.Invoke(this, args);
        }

        public void FireMeRequestReceived(BotEventArgs args)
        {
            
            MeRequestReceived?.Invoke(this, args);
        }

        public void FireGuessReceived(GuessReceivedEventArgs args)
        {
            
            GuessReceived?.Invoke(this, args);
        }

        public void FireRandomGuessRecieved(RandomGuessRecievedEventArgs args)
        {
            
            RandomGuessRecieved?.Invoke(this, args);
        }

        public void FireFlagOfTargetReceived(TargetBotEventArgs args)
        {
            
            FlagOfTargetReceived?.Invoke(this, args);
        }

        public void FireFlagRequestReceived(FlagRequestReceivedEventArgs args)
        {
            
            FlagRequestReceived?.Invoke(this, args);
        }

        public void FireColorRequestReceived(ColorRequestReceivedEventArgs args)
        {
            
            ColorRequestReceived?.Invoke(this, args);
        }

        public void FireColorOfTargetReceived(TargetBotEventArgs args)
        {
            
            ColorOfTargetReceived?.Invoke(this, args);
        }

        public void FireBannedUserReceived(BannedUserReceivedEventArgs args)
        {
            BannedUserReceived?.Invoke(this, args);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
#endregion

    }
}
