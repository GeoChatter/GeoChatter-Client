using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

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
        public OBSScene CurrentScene => !obs.IsConnected ? null : obs.GetCurrentScene();
        /// <summary>
        /// Get a list of scenes
        /// </summary>
        /// <returns></returns>
        public List<OBSScene> GetScenes()
        {
            if (!obs.IsConnected)
            {
                return new List<OBSScene>();
            }

            List<OBSScene> scenes = obs.ListScenes();
            return scenes;
        }
        /// <summary>
        /// Get a list of sources
        /// </summary>
        /// <returns></returns>
        public List<SourceInfo> GetSources()
        {
            if (!obs.IsConnected)
            {
                return new List<SourceInfo>();
            }

            List<SourceInfo> scenes = obs.GetSourcesList();
            return scenes;
        }
        /// <summary>
        /// Hide given item in given scene
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="itemName"></param>
        public void HideItem(string sceneName, string itemName)
        {
            if (!obs.IsConnected)
            {
                return;
            }
            obs.SetSourceRender(itemName, false);
        }
        /// <summary>
        /// Show given item in given scene
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="itemName"></param>
        public void ShowItem(string sceneName, string itemName)
        {
            if (!obs.IsConnected)
            {
                return;
            }
            obs.SetSourceRender(itemName, true);
        }

        /// <summary>
        /// Modify a source in a scene to execute <paramref name="action"/>
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="sourceName"></param>
        /// <param name="action"></param>
        public void ModifySource(string sceneName, string sourceName, string action)
        {
            if (!obs.IsConnected)
            {
                return;
            }
            OBSScene scene = obs.ListScenes().FirstOrDefault(s => s.Name == sceneName);
            SceneItem item = scene?.Items.FirstOrDefault(i => i.SourceName == sourceName);
            if (item != null)
            {
                switch (action?.ToLowerInvariant())
                {
                    case "show":
                        obs.SetSourceRender(sourceName, true, sceneName);
                        break;
                    case "hide":
                        obs.SetSourceRender(sourceName, false, sceneName);
                        break;
                    default:
                        bool isActive = obs.GetSourceActive(sourceName);
                        obs.SetSourceRender(sourceName, !isActive, sceneName);
                        break;
                }
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
            obs.SetCurrentScene(sceneName);
        }



        private void Obs_Connected(object sender, EventArgs e)
        {
        }

        private void Obs_Disconnected(object sender, EventArgs e)
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
            if (!obs.IsConnected)
            {
                try
                {
                    obs.Connect(ip, password);
                }
                catch (AuthFailureException)
                {
                    return false;
                }
                catch (ErrorResponseException)
                {
                    return false;
                }
                catch (SocketException)
                {
                    return false;
                }

            }
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
            if (!obs.IsConnected)
            {
                try
                {
                    obs.Connect(ip, password);
                    obs.Disconnect();
                }
                catch (AuthFailureException)
                {
                    return false;
                }
                catch (ErrorResponseException)
                {
                    return false;
                }

            }
            return true;
        }

        /// <summary>
        /// Disconnect from the websocket connection
        /// </summary>
        public void Disconnect()
        {

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
    public class MySceneItem : SceneItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public MySceneItem(SceneItem item)
        {
            if (item != null)
            {
                AudioVolume = item.AudioVolume;
                GroupChildren = item.GroupChildren;
                Height = item.Height;
                ID = item.ID;
                InternalType = item.InternalType;
                Locked = item.Locked;
                ParentGroupName = item.ParentGroupName;
                Render = item.Render;
                SourceHeight = item.SourceHeight;
                SourceName = item.SourceName;
                SourceWidth = item.SourceWidth;
                Width = item.Width;
                XPos = item.XPos;
                YPos = item.YPos;
            }
        }
        /// <summary>
        /// Source name
        /// </summary>
        public string Name => SourceName;

    }

    /// <summary>
    /// OBS Scene model
    /// </summary>
    public class MyOBSScene : OBSScene
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public MyOBSScene(OBSScene item)
        {
            if (item != null)
            {
                Name = item.Name;
                Items = item.Items;
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        public string DisplayName => Name;

    }


}
