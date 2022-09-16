using GeoChatter.Core.Interfaces;
using GeoChatter.Integrations.Classes;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;

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
        private static WebSocket ws;

        /// <summary>
        /// Connect to <paramref name="ip"/>:<paramref name="port"/>
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Connect(string ip, string port)
        {

            if (ws == null)
            {
                if (string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
                {
                    return false;
                }

                ws = new WebSocket($"ws://{ip}:{port}/");
            }
            bool success = true;

            if (!ws.IsAlive)
            {
                ws.Connect();
            }
            success = ws.IsAlive;
            ws.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    ActionsReceivedEventArgs args = new();
                    StreamerbotActionList list = JsonConvert.DeserializeObject<StreamerbotActionList>(e.Data);
                    args.Actions.AddRange(list.actions);
                    OnActionsReceived(args);
                    return;
                }
            };
            return success;
        }

        /// <summary>
        /// Fires <see cref="ActionsReceived"/>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnActionsReceived(ActionsReceivedEventArgs e)
        {
            logger.Debug($"Received actions");
            ActionsReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Send GetActions request through web socket
        /// </summary>
        public static void GetActions()
        {
            logger.Debug($"Getting actions");
            if (ws != null && !ws.IsAlive)
            {
                ws.Connect();
            }
            if (ws != null && ws.IsAlive)
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
        public static void ExecuteAction(string guid, string name, Dictionary<string, string> args)
        {
            if (ws != null && !ws.IsAlive)
            {
                ws.Connect();
            }
            if (ws != null && ws.IsAlive)
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
        public static bool TestConnection(string ip, string port)
        {
            logger.Debug($"Testing Streamerbot connection");
            if (ws == null)
            {
                ws = new WebSocket($"ws://{ip}:{port}/");
            }

            bool success;

            if (!ws.IsAlive)
            {
                ws.Connect();
            }

            success = ws.IsAlive;
            logger.Debug($"SB connection is {success}");
            if (ws.IsAlive)
            {
                ws.Close();
            }

            return success;
        }

        /// <summary>
        /// Close web socket connection
        /// </summary>
        public static void CloseConnection()
        {
            logger.Debug($"Closing Streamer.Bot connection");
            if (ws != null)
            {
                ws.Close();
            }

            return;
        }

        /// <summary>
        /// <see cref="ExecuteAction(string, string, Dictionary{string, string})"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public static void ExecuteAction(string id, string name)
        {
            ExecuteAction(id, name, new Dictionary<string, string>());
        }

        /// <summary>
        /// Wheter connection is still alive
        /// </summary>
        /// <returns></returns>
        public static bool IsAlive()
        {
            return ws != null && ws.IsAlive;
        }
    }
}
