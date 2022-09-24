using log4net;
using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace GeoChatter.Integrations
{
    /// <summary>
    /// OBS client model
    /// </summary>
    public class OBSClient
    {
        /// <summary>
        /// Websocket instance
        /// </summary>
        protected OBSWebsocket obs { get; init; }
        private static readonly ILog logger = LogManager.GetLogger(typeof(OBSClient));
        private List<MyOBSScene> scenes = new List<MyOBSScene>();

        public List<MyOBSScene> Scenes { get { return scenes; } }
        /// <summary>
        /// 
        /// </summary>
        public OBSClient()
        {
            obs = new OBSWebsocket();

            obs.Connected += Obs_Connected;
            obs.Disconnected += Obs_Disconnected;
        }

        /// <summary>
        /// Current scene
        /// </summary>
        public MyOBSScene CurrentScene => (!obs.IsConnected || !scenes.Any()) ? null :  scenes.FirstOrDefault(s => s.Name == obs.GetCurrentProgramScene());
        /// <summary>
        /// Get a list of sources
        /// </summary>
        /// <returns></returns>
        public List<MyOBSScene> GetScenes()
        {
            logger.Debug($"Getting Scenes");
            if (!obs.IsConnected)
            {
                return new List<MyOBSScene>();
            }
            if (scenes.Any())
                return scenes;
            scenes = obs.ListScenes().ConvertAll(s => new MyOBSScene(s));
            foreach (MyOBSScene scene in scenes)
            {
                List<SceneItemDetails> items = GetSources(scene.DisplayName);
                scene.Items = items.ConvertAll(i =>  new MySceneItem(i) );
            }
            logger.Debug($"Received {scenes.Count} sources");
            return scenes;
        }
        /// <summary>
        /// Get a list of sources
        /// </summary>
        /// <returns></returns>
        public List<SceneItemDetails> GetSources(string sceneName)
        {
            logger.Debug($"Getting sources");
            if (!obs.IsConnected)
            {
                return new List<SceneItemDetails>();
            }

            List<SceneItemDetails> sources = obs.GetSceneItemList(sceneName);
            logger.Debug($"Received {sources.Count} sources");
            return sources;
        }

        //public List<SceneItemDetails> GetSceneItemList(string sceneName)
        //{
        //    JObject request = null;
        //    if (!string.IsNullOrEmpty(sceneName))
        //    {
        //        request = new JObject
        //        {
        //            { nameof(sceneName), sceneName }
        //        };
        //    }

        //    var response = obs.SendRequest(nameof(GetSceneItemList), request);
        //    return response["sceneItems"].Select(m => new SceneItemDetails((JObject)m)).ToList();
        //}

        /// <summary>
        /// Hide given item in given scene
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="itemName"></param>
        public void HideItem(string sceneName, int itemId)
        {
            if (!obs.IsConnected)
            {
                return;
            }
            //logger.Debug($"Hiding {itemName}");
            obs.SetSceneItemEnabled(sceneName, itemId, false);
        }
        /// <summary>
        /// Show given item in given scene
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="itemName"></param>
        public void ShowItem(string sceneName, int itemId)
        {
            if (!obs.IsConnected)
            {
                return;
            }
            
            obs.SetSceneItemEnabled(sceneName, itemId, true);
        }

        /// <summary>
        /// Modify a source in a scene to execute <paramref name="action"/>
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="sourceName"></param>
        /// <param name="action"></param>
        public void ModifySource(string sceneName, int itemId, string action)
        {
            if (!obs.IsConnected)
            {
                return;
            }
            logger.Debug($"Modifing source {itemId} in scene {sceneName}. Action: {action}");
                switch (action?.ToLowerInvariant())
                {
                    case "show":
                        obs.SetSceneItemEnabled(sceneName, itemId, true);
                        break;
                    case "hide":
                        obs.SetSceneItemEnabled(sceneName, itemId, false);
                        break;
                    default:
                        bool isActive = obs.GetSceneItemEnabled(sceneName, itemId);
                        obs.SetSceneItemEnabled(sceneName, itemId, !isActive);
                        break;
                }
        }

        /// <summary>
        /// Switch to <paramref name="sceneName"/>
        /// </summary>
        /// <param name="sceneName"></param>
        public void SwitchToScene(string sceneName)
        {
            if (!obs.IsConnected)
            {
                return;
            }
            logger.Debug($"Switching scene to {sceneName}");
            obs.SetCurrentProgramScene(sceneName);
        }



        private void Obs_Connected(object sender, EventArgs e)
        {
        }

        private void Obs_Disconnected(object sender, ObsDisconnectionInfo e)
        {
        }

        /// <summary>
        /// Connect to <paramref name="ip"/> with <paramref name="password"/>
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Connect(string ip, string password)
        {
            logger.Debug($"Connecting OBS");
            if (!obs.IsConnected)
            {
                try
                {
                    int counter = 1;
                    obs.ConnectAsync(ip, password);
                    while (!obs.IsConnected)
                    {
                        if (counter > 100)
                            return false;
                        Thread.Sleep(50);
                        counter++;
                    }
                    logger.Debug($"OBS Connected successfuly");
                }
                catch (AuthFailureException e)
                {
                    logger.Error($"Error authentication with OBS:");
                    logger.Error(e);
                    return false;
                }
                catch (ErrorResponseException e)
                {
                    logger.Error($"Response exception from OBS:");
                    logger.Error(e);
                    return false;
                }
                catch (SocketException e)
                {
                    logger.Error($"Socket exception while connecting to OBS:");
                    logger.Error(e);
                    return false;
                }

            }else
                logger.Debug($"OBS already connected");
            return true;
        }

        /// <summary>
        /// Test the connection to <paramref name="ip"/>
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool TestConnection(string ip, string password)
        {
            logger.Debug($"Testing OBS connection");
            if (!obs.IsConnected)
            {
                try
                {
                    obs.Connect(ip, password);
                    obs.Disconnect();
                    logger.Debug($"OBS connection test successful");
                }
                catch (AuthFailureException e)
                {
                    logger.Error($"Failure authenticating with OBS:");
                    logger.Error(e);
                    return false;
                }
                catch (ErrorResponseException e)
                {
                    logger.Error($"Received response error from OBS:");
                    logger.Error(e);
                    return false;
                }

            }else
                logger.Debug($"OBS is connected, not test neccessary");
            return true;
        }

        /// <summary>
        /// Disconnect from the websocket connection
        /// </summary>
        public void Disconnect()
        {
            logger.Debug($"Disconnecting from OBS");

            if (obs.IsConnected)
            {
                obs.Disconnect();
            }
        }

        /// <summary>
        /// Wheter connection is still alive
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return obs.IsConnected;
        }
    }

}

namespace OBSWebsocketDotNet.Types
{
    /// <summary>
    /// Scene item model
    /// <para>Source information returned by GetSourcesList</para>
    /// </summary>
    public class MySceneItem : SceneItemDetails
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public MySceneItem(SceneItemDetails item)
        {
            Name = item.SourceName;
            Id = item.ItemId;
        }
        /// <summary>
        /// Source name
        /// </summary>
        public string Name { get; set; }

        public int Id { get; set; }

    }

    /// <summary>
    /// OBS Scene model
    /// </summary>
    public class MyOBSScene : SceneBasicInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public MyOBSScene(SceneBasicInfo item)
        {
            DisplayName = item.Name;
         
        }
        public List<MySceneItem> Items = new List<MySceneItem>();

        /// <summary>
        /// Name
        /// </summary>
        public string DisplayName { get; set; }

    }


}
