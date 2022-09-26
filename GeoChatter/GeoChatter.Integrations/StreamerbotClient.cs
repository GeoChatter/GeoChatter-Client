using GeoChatter.Core.Interfaces;
using GeoChatter.Integrations.Classes;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace GeoChatter.Integrations
{
    /// <summary>
    /// StreamerBot client model
    /// </summary>
    public class StreamerbotClient
    {
        /// <summary>
        /// Action recieve event
        /// </summary>
        public event EventHandler<ActionsReceivedEventArgs> ActionsReceived;
        private static readonly ILog logger = LogManager.GetLogger(typeof(StreamerbotClient));
        private WebsocketClient ws;
        private List<StreamerbotAction> actions = new List<StreamerbotAction>();
        /// <summary>
        /// Returns a list of StreamerbotActions
        /// </summary>
        public List<StreamerbotAction> Actions { get { return actions; } }
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);
        private Uri adress;

        /// <summary>
        /// Connect to <paramref name="ip"/>:<paramref name="port"/>
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public async Task<bool> Connect(string ip, string port)
        {

            if (ws == null)
            {
                if (string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
                {
                    return false;
                }

                adress = new Uri($"ws://{ip}:{port}/");
                ws = new WebsocketClient(adress);
            }
            bool success = true;
            
            if (!ws.IsRunning)
            {
               ws.Start();

                //ExitEvent.WaitOne();
            }
            success = ws.IsStarted;
            ws.MessageReceived.Subscribe(msg =>
            {
                
                if (msg.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
                {
                    ActionsReceivedEventArgs args = new();
                    StreamerbotActionList list = JsonConvert.DeserializeObject<StreamerbotActionList>(msg.Text);
                    args.Actions.AddRange(list.actions);
                    OnActionsReceived(args);
                    return;
                }
            });
            return success;
        }

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



        /// <summary>
        /// Send GetActions request through web socket
        /// </summary>
        public void GetActions()
        {
            logger.Debug($"Getting actions");
            if (ws != null && !ws.IsRunning)
            {
                ws.Start();
            }
            if (ws != null && ws.IsRunning)
            {
                ws.Send("{\"request\":\"GetActions\",\"id\":\"123\"}");
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
            if (ws != null && !ws.IsRunning)
            {
                ws.Start();
            }
            if (ws != null && ws.IsRunning)
            {
                string argsString = JsonConvert.SerializeObject(args);
                string reqString = @"{""request"": ""DoAction"",""action"": { ""id"": """ + guid + @""", ""name"": """ + name + @""" }, ""args"":  " + argsString + @" , ""id"": ""1402""}";
                logger.Debug($"Executing action: {reqString}");
                ws.Send(reqString);
            }
        }

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
                ws = new WebsocketClient(new Uri($"ws://{ip}:{port}/"));
            }

            bool success;

            if (!ws.IsRunning)
            {
                ws.Start();
            }

            success = ws.IsRunning;
            logger.Debug($"SB connection is {success}");
            if (ws.IsRunning)
            {
                ws.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "");
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
                var success = await ws.Stop(System.Net.WebSockets.WebSocketCloseStatus.Empty,"");
                if (!success && adress != null)
                    ws = new WebsocketClient(adress);
                return ws.IsRunning;
            }

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
        public bool IsRunning()
        {
            return ws != null && ws.IsRunning;
        }
    }
}
