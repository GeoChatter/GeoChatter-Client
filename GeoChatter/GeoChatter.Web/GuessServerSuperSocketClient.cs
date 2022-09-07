using GeoChatter.Core.Interfaces;
using GeoChatter.Core.Model;
using GeoChatter.Core.Storage;
using log4net;
using Newtonsoft.Json;
using SuperSocket.ClientEngine;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading;
using System.Timers;
using TwitchLib.PubSub.Models.Responses.Messages;

namespace GeoChatter.Web
{
    public class GuessServerSuperSocketClient
    {
        public GuessServerSuperSocketClient(string botname, string channelName, IMainForm form)
        {
            this.botName = botname;
            this.channelName = channelName;
            this.mainForm = form;
            bw.DoWork += Bw_DoWork;
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Send($"PING {channelName} ({botName})");
        }

        private static readonly ILog logger = LogManager.GetLogger(typeof(GuessServerSuperSocketClient));
        private const int PORT_NO = 2012;
#if DEBUG
       private const string SERVER_IP = "127.0.0.1";
#else
        private const string SERVER_IP = "185.56.150.187";
#endif
        private string botName;
        private string channelName;
        private string version;
        private IMainForm mainForm;
        private Socket socketClient;
        public void ConnectToServer(string version)
        {
            EasyClient client = new EasyClient();
            client.
            this.version = version;
            try
            {
                socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(SERVER_IP);
                IPEndPoint point = new IPEndPoint(ip, PORT_NO);
                //Make connection
                socketClient.Connect(point);

                
                //Receive messages from the server continuously
                Thread thread = new Thread(Receive)
                {
                    IsBackground = true
                };
                thread.Start();


                Send($"LOGIN {channelName} ({version})");
                StartEcho();

            }
            catch (Exception e)
            {
                logger.Error("Error during connection to GuessServer");
                logger.Error(e);
                FireOnServerConnectionLost(new ServerConnectionLostArgs("Server unavailable", "GuessServer seems to be unavailable.\n\rPlease press Yes to try reconnecting or No to cancel.\n\rIf you cancel, you need to switch to whispers instead!"));
            }
        }

        public bool isConnected { get { return socketClient.Connected; } }

        System.Timers.Timer timer;
        private void StartEcho()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 30000;
            timer.Enabled = true;
            timer.Elapsed += timer_Elapsed; // not button1_Click
            timer.Start();

        }
        BackgroundWorker bw = new BackgroundWorker();
        
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bw.RunWorkerAsync();
        }

        private void Send(string message)
        {
            try
            {

                byte[] buffter = Encoding.UTF8.GetBytes(message + "\r\n");
                if(!hasBeenFired)
                    socketClient.Send(buffter);
            }
            catch (Exception e)
            {
                logger.Error("Error during send");
                logger.Error(e);
                FireOnServerConnectionLost(new ServerConnectionLostArgs("Error sending message", "GuessServer seems to be unavailable.\n\rPlease press Yes to try reconnecting or No to cancel.\n\rIf you cancel, you need to switch to whispers instead!"));
            }
        }

        private void Receive()
        {
            try
            { //Why is it possible to use a telnet client, but not this one.
                while (true)
                {
                    //Get the message sent
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    int effective = socketClient.Receive(buffer);
                    if (effective == 0)
                    {
                        break;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, effective);
                    string command = str.Split(' ')[0];
                    string commandBody = str.Replace(command + " ", "").Trim(new[] { '\r', '\n' });
                    switch (command)
                    {
                        case "LOGIN":
                            if (commandBody == "OK")
                            {
                                logger.Info("Logged in\r\n");
                            }

                            break;
                        case "!g":
                            string[] parts = commandBody.Split(' ');
                            if (mainForm.ProcessViewerGuess(parts[0], parts[4], parts[1], parts[2], "", parts[3], parts[5]))
                                Send($"OK {parts[6]}");
                            else
                                Send($"FAILURE {parts[6]}");
                            break;
                        default:
                            logger.Info(str);
                            break;
                    }
                    //Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error during receive");
                logger.Error(ex);
                logger.Error("Reconnecting....");
                FireOnServerConnectionLost(new ServerConnectionLostArgs("Lost connection", "GuessServer seems to be unavailable.\n\rPlease press Yes to try reconnecting or No to cancel.\n\rIf you cancel, you need to switch to whispers instead!"));
            }
        }

        public class ServerConnectionLostArgs
{
            public ServerConnectionLostArgs(string title, string msg)
            {
                Title = title;
                Message = msg;
            }

            public string Title { get; }
            public string Message { get; }
        }

        public event EventHandler<ServerConnectionLostArgs> onServerConnectionLost;
        bool hasBeenFired = false;
        public void FireOnServerConnectionLost(ServerConnectionLostArgs args)
        {
            if (!hasBeenFired)
            {
                onServerConnectionLost?.Invoke(this, args);
                hasBeenFired = true;
            }
        }

        public void Disconnect(bool allowReconnect = false)
        {
            if(socketClient != null && socketClient.Connected)
                if(allowReconnect)
                    socketClient.Disconnect(allowReconnect);
                else
                    socketClient.Close();
        }

        public void SendGameToServer(Game runningGame)
        {
            try
            {
                byte[] buffter = Encoding.UTF8.GetBytes("GAME "+ JsonConvert.SerializeObject(runningGame));
                socketClient.Send(buffter);
            }
            catch (Exception e)
            {
                logger.Error("Error during send");
                logger.Error(e);
                FireOnServerConnectionLost(new ServerConnectionLostArgs("Error sending game", "GuessServer seems to be unavailable.\n\rPlease press Yes to try reconnecting or No to cancel.\n\rIf you cancel, you need to switch to whispers instead!"));
            }
        }
    }
}
