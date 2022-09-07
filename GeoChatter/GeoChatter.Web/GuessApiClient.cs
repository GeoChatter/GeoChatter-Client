using GeoChatter.Core.Helpers;
using GeoChatter.Core.Interfaces;
using GeoChatter.Core.Logging;
using GeoChatter.Model.Enums;
using GeoChatter.Core.Model;
using GeoChatter.Security.Client;
using GuessServerApiInterfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GeoChatter.Model;
using GeoChatter.Core.Common.Extensions;
using System.ServiceModel.Channels;
using System.Drawing;
using System.Windows.Forms;
using GeoChatter.Core.Model.Map;
using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using GeoChatter.Core.Model.Enums;
using System.Linq;
using System.ComponentModel;
using System.Timers;

namespace GeoChatter.Web
{
    /// <summary>
    /// API log event arguments model
    /// </summary>
    public sealed class LogEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public LogEventArgs(string message, LogLevel level)
        {
            Message = message;
            LogLevel = level;
        }

        public object ExceptionObject { get; set; }
        /// <summary>
        /// Message to log
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Level
        /// </summary>
        public LogLevel LogLevel { get; }

        public string MessageboxMessage { get; set; }
    }
    public sealed class ConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="connectionTime"></param>
        public ConnectedEventArgs(string message, string connectionTime)
        {
            Message = message;
            ConnectionTime = connectionTime;
        }
        /// <summary>
        /// Message to log
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Level
        /// </summary>
        public string ConnectionTime { get; }
    }

    /// <summary>
    /// Log levels
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Debug
        /// </summary>
        Debug,
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// Warning
        /// </summary>
        Warning,
        /// <summary>
        /// Information
        /// </summary>
        Information

    }
    /// <summary>
    /// API client model
    /// </summary>
    public class GuessApiClient
    {
       

        private static readonly object _mutex = new();
        private static volatile GuessApiClient _instance;
        private static LoginResponse loginResponse;
        private string apiUrl;
        private ApiClient client;
        private bool uploadLog = false;

        /// <summary>
        /// API instance
        /// </summary>
        public static GuessApiClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        _instance = new GuessApiClient();
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// List of dummies
        /// </summary>
        public List<string> Trolls { get; private set; }
        /// <summary>
        /// Keys provided by API connection
        /// </summary>
        public Dictionary<string, string> Keys { get; private set; }

        public string MapIdentifier { get; set; }
        public bool SummaryEnabled { get; set; }
        /// <summary>
        /// Get a provided key named <paramref name="key"/> or <see langword="null"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetKey(string key)
        {
            return Keys != null && Keys.TryGetValue(key, out string val) ? val : string.Empty;
        }

        #region Events

        /// <summary>
        /// Logging event
        /// </summary>
        public event EventHandler<LogEventArgs> OnLog;

        /// <summary>
        /// Fires <see cref="OnLog"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void FireLog(string message, LogLevel level, object exceptionObject = null)
        {
            OnLog?.Invoke(this, new(message, level) { ExceptionObject = exceptionObject });
        }

        /// <summary>
        /// Disconnect event
        /// </summary>
        public event EventHandler<LogEventArgs> OnDisconnect;
        /// <summary>
        /// Fires <see cref="OnDisconnect"/>
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="level"></param>
        public void FireDisconnected(string msg, string msgBoxmsg, LogLevel level, object exceptionObject = null)
        {
            OnDisconnect?.Invoke(this, new(msg, level) { ExceptionObject = exceptionObject, MessageboxMessage = msgBoxmsg});
        }
        /// <summary>
        /// Reconnect event
        /// </summary>
        public event EventHandler<EventArgs> OnReconnected;
        /// <summary>
        /// Fires <see cref="OnReconnected"/>
        /// </summary>
        public void FireReconnected()
        {
            OnReconnected?.Invoke(this, new());
        }
        /// <summary>
        /// Connect event
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;
        /// <summary>
        /// Fires <see cref="Connected"/>
        /// </summary>
        public void FireConnected(string message, string time)
        {

            ConnectedEventArgs eventArgs = new ConnectedEventArgs(message, time);
            Connected?.Invoke(this, eventArgs);
        }
        #endregion

        /// <summary>
        /// Current connection instance
        /// </summary>
        public HubConnection CurrentConnection => connection;

        private HubConnection connection;
        private Task Connection_Reconnecting(Exception arg)
        {
            FireLog($"Connection started reconnecting due to an error: {arg}", LogLevel.Error, arg);
            return Task.CompletedTask;
        }

        bool forcedReconnect;
        private async Task Connection_Closed(Exception arg)
        {
            FireLog($"Connection closed due to an error: {arg}", LogLevel.Error, arg);
            timer.DisposeAsync();
            if (forcedReconnect || arg != null)
            {
                FireDisconnected($"Connection to guess server closed due to an error: {arg?.Message}", "Trying to reconnect!", LogLevel.Error);
                await Initialize(apiUrl, mainForm, gcClientId, uploadLog, false);
                Connect(client, null, true, true);
            }
            return;
        }

        private async Task<Task> Connection_Reconnected(string arg)
        {
            try
            {
                FireLog($"Connection reconnected: {arg}", LogLevel.Information);
                loginResponse = await connection.InvokeAsync<LoginResponse>(nameof(IGeoChatterHub.Login), client);
                if (loginResponse == null)
                {
                    throw new InvalidDataException("Login failed, please restart the client!");
                }

                MapIdentifier = client.MapIdentifier = loginResponse.MapIdentifier;
                LanguageStrings.SetMapIdentifier(MapIdentifier);
                FireLog($"Received MapId: {MapIdentifier} for {GCResourceRequestHandler.ClientGeoGuessrName} from reconnect attempt", LogLevel.Debug);
                FireReconnected();
            }catch(Exception e)
            {
                FireLog("Error during reconnection", LogLevel.Error, e);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Wheter API connection is established
        /// </summary>
        public bool IsConnected => connection != null && connection.State == HubConnectionState.Connected;
        private IMainForm mainForm;
        string gcClientId="";
        string token = "";
        /// <summary>
        /// Initialize connection to API
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        public async Task<bool> Initialize(string url, IMainForm form, string clientId, bool debugEnabled, bool isGGLogon = false)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId))
                {
                    FireLog($"Connection could not be initialized due to an error: ClientId is empty", LogLevel.Error);
                    FireDisconnected($"Connection to guess server could not be initialized due to an error: ClientId is empty","GeoGuessr user could not be identified", LogLevel.Error);
                    return false;
                }
                gcClientId = clientId;
                watch = new Stopwatch();
                watch.Start();
                apiUrl = url;
                uploadLog = debugEnabled;
                mainForm = form;
                FireLog("isGGlogon: ", LogLevel.Debug);
                forcedReconnect = isGGLogon;
                FireLog("Starting token validation", LogLevel.Debug);
                string validationResult = await ValidateToken(isGGLogon);
                if (!string.IsNullOrEmpty(token))
                {
                    //"https://localhost/api/guessServerHub"
                    connection = new HubConnectionBuilder()
                        .WithUrl(url, options =>
                        {

                            options.AccessTokenProvider = () => { return Task.FromResult(token); };
                            options.UseDefaultCredentials = true;
                            options.HttpMessageHandlerFactory = (msg) =>
                            {
                                if (msg is HttpClientHandler clientHandler)
                                {
                                    // bypass SSL certificate
                                    clientHandler.ServerCertificateCustomValidationCallback +=
                                        (sender, certificate, chain, sslPolicyErrors) => { return true; };
                                }

                                return msg;
                            };
                        })
                           .ConfigureLogging(logging =>
                           {
                               // Log to the Console
                               //logging.AddConsole();
                               logging.AddDebug();
                               logging.AddLog4Net();
                               // This will set ALL logging to Debug level
                               logging.SetMinimumLevel((Microsoft.Extensions.Logging.LogLevel)LogLevel.Information);
                           }).WithAutomaticReconnect().Build();
                    connection.Reconnecting += Connection_Reconnecting;
                    connection.Reconnected += Connection_Reconnected;
                    connection.Closed += Connection_Closed;


                    //connection.On<string, string>(nameof(IGeoChatterClient.ReceiveMessage), async (user, message) =>
                    //{
                    //    await Task.Run(() => FireLog(user + " says " + message + "\r\n", LogLevel.Information));
                    //});


                    connection.On<Guess>(nameof(IGeoChatterClient.ReceiveGuess), GuessReceived);
                    connection.On(nameof(IGeoChatterClient.InitiateDisconnect), OnInitiateDisconnect);
                    connection.On<Player, string>(nameof(IGeoChatterClient.ReceiveFlag), FlagRequestReceived);
                    connection.On<Player, string>(nameof(IGeoChatterClient.ReceiveColor), ColorRequestReceived);
                    forcedReconnect = false;
                    return true;
                }
                else if (connection != null)
                {
                    await connection.DisposeAsync();
                    return false;
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Summarize();
                FireLog($"Connection could not be initialized due to an error: {msg}", LogLevel.Error);
                FireDisconnected($"Connection to guess server could not be initialized due to an error: {msg}","There was an error initializing the connection", LogLevel.Error, ex);
            
            }
            return false;
        }

        private async Task OnInitiateDisconnect()
        {
            await Disconnect(true, "This user connected on another client!");
        }

        private async Task<string> ValidateToken(bool isGGLogon)
        {
            if (isGGLogon)
            {
                FireLog("resetting token", LogLevel.Debug);
                token = String.Empty;
            }
            if (!string.IsNullOrEmpty(token))
            {
                FireLog("token exists", LogLevel.Debug);
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

                if (jwtSecurityToken.ValidTo > DateTime.UtcNow.AddSeconds(10))
                {
                    FireLog("valid token", LogLevel.Debug);
                    return "token not expired"; 
                }
                else
                    FireLog("expired token, needs refresh", LogLevel.Debug);
            }
            return await GetToken(isGGLogon);
        }
        /// <summary>
        /// Requests Bearer token from API
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetToken(bool isLogin=false)
        {
           // isLogin = true;
            TokenResponse tokenResponse = null;

            // discover endpoints from metadata
            using (var client = new HttpClient())
            {
                FireLog("getting discovery endpoint: "+ apiUrl.ReplaceDefault("/geoChatterHub"), LogLevel.Debug);
                var disco = await client.GetDiscoveryDocumentAsync(apiUrl.ReplaceDefault("/geoChatterHub"));
                if (disco.IsError)
                {
                    FireLog(disco.Error, LogLevel.Error);
                    return "disco.Error";
                }
                FireLog("Creating token request", LogLevel.Debug);
                using (var req = new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "client",
                    ClientSecret = "secret",

                    Scope = "GeoChatterApi",
                    Parameters =
                    {
                        { "GGUserId", GCResourceRequestHandler.ClientUserID ?? "0" },
                        { "GGUserName", GCResourceRequestHandler.ClientGeoGuessrName ?? "Unknown" },
                        { "Action", isLogin?ServerActions.StartVerification.ToStringDefault(): ServerActions.Verify.ToStringDefault() },
                        {"GCClientId", gcClientId }
                    }
                })
                {
                    FireLog("Sending request", LogLevel.Debug);
                    tokenResponse = await client.RequestClientCredentialsTokenAsync(req);
                    FireLog("token request response received: " + tokenResponse.Json, LogLevel.Debug);
                    if (tokenResponse != null)
                    {
                        GCTokenResponse resp = JsonConvert.DeserializeObject<GCTokenResponse>(tokenResponse.Json.ToStringDefault());
                        FireLog("Parsed response: " + resp, LogLevel.Debug);

                        if (tokenResponse.IsError)
                        {
                            watch.Stop();
                            TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
                            string answer = string.Format(CultureInfo.InvariantCulture, "{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                                    t.Hours,
                                                    t.Minutes,
                                                    t.Seconds,
                                                    t.Milliseconds);
                            if (tokenResponse.Error == "Unauthorized")
                            {
                                FireLog("Established GuessServerConnection in " + answer, LogLevel.Debug);
                                FireLog($"No valid Verification found (ggname: {GCResourceRequestHandler.ClientGeoGuessrName}, ggid: {GCResourceRequestHandler.ClientUserID}, clientid: {gcClientId})", LogLevel.Error);
                                throw new UnauthorizedAccessException("Could not verify your identity!\n\r\n\rPlease login into GeoGuessr again!");
                            }
                            else if (tokenResponse.Error == "GGApiBroken")
                            {
                                FireLog("Established GuessServerConnection in " + answer, LogLevel.Debug);
                                FireLog($"Server has issues connecting to GeoGuessr: {tokenResponse.ErrorDescription}", LogLevel.Error);
                                throw new UnauthorizedAccessException("Unable to contact GeoGuessr API!\n\r\n\rPlease try again later!");

                            }
                            else
                                FireLog("ERROR: " + tokenResponse.Error + ", Desc: " + tokenResponse.ErrorDescription, LogLevel.Error);
                        }
                        if(!string.IsNullOrEmpty(resp.DuelId))
                        {
                            FireLog("Party ID received: " + resp.PartyId, LogLevel.Debug);
                            FireLog("Duel ID received: " + resp.DuelId, LogLevel.Debug);
                            GeoGuessrClient.JoinParty(resp.PartyId);
                            FireLog("Joined party", LogLevel.Debug);
                            GeoGuessrClient.JoinPartyLobby(resp.DuelId);
                            FireLog("Joined lobby, generating verification request", LogLevel.Debug);
                            GGPartyLobby party = null;
                            while (party?.party?.openGames[0]?.players?.Count(p => p.id == GCResourceRequestHandler.ClientUserID) != 1)
                            {
                                party = GeoGuessrClient.GetPartyData(resp.PartyId);
                                FireLog($"Lobby found", LogLevel.Debug);
                            }

                            using (var req2 = new ClientCredentialsTokenRequest
                            {
                                Address = disco.TokenEndpoint,
                                ClientId = "client",
                                ClientSecret = "secret",

                                Scope = "GeoChatterApi",
                                Parameters =
                    {
                        { "GGUserId", GCResourceRequestHandler.ClientUserID },
                        { "GGUserName", GCResourceRequestHandler.ClientGeoGuessrName },
                        { "Action", ServerActions.Verify.ToStringDefault() },
                        {"GCClientId", gcClientId },
                        {"DuelId", resp.DuelId }
                    }
                            })
                            {
                                FireLog("Sending join verification request", LogLevel.Debug);
                                tokenResponse = await client.RequestClientCredentialsTokenAsync(req2);
                                //FireLog("", LogLevel.Debug);
                                GCTokenResponse resp2 = JsonConvert.DeserializeObject<GCTokenResponse>(tokenResponse.Json.ToStringDefault());
                                FireLog("Parsed response: " + resp2, LogLevel.Debug);

                            }
                        }
                       
                       
                        FireLog("token response: " + tokenResponse.Json.ToStringDefault(), LogLevel.Debug);
                        token = tokenResponse.AccessToken;
                     
                    }
                    else
                    {
                        FireLog("No token response received", LogLevel.Error);
                        return "";

                    }
                }
            }
            return "";
            //// call api
            //var apiClient = new HttpClient();
            //apiClient.SetBearerToken(tokenResponse.AccessToken);

            //var response = await apiClient.GetAsync("https://localhost:6001/identity");
            //if (!response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine(response.StatusCode);
            //}
            //else
            //{
            //    var content = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine(JArray.Parse(content));
            //}
        }

        public int State
        {
            get
            {
                if (CurrentConnection == null)
                {
                    return (int)ConnectionState.UNKNOWN;
                }

                switch (CurrentConnection.State)
                {
                    case Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Disconnected:
                        {
                            return (int)ConnectionState.DISCONNECTED;
                        }
                    case Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected:
                        {
                            return (int)ConnectionState.CONNECTED;
                        }
                    case Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connecting:
                        {
                            return (int)ConnectionState.CONNECTING;
                        }
                    case Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Reconnecting:
                        {
                            return (int)ConnectionState.RECONNECTING;
                        }
                    default:
                        {
                            return (int)ConnectionState.UNKNOWN;
                        }
                }
            }
        }
        private async Task GuessReceived(Guess guess)
        {
            FireLog($"Viewer guess received: {(string.IsNullOrEmpty(guess.Player.DisplayName) ? guess.Player.PlatformId : guess.Player.DisplayName)}", LogLevel.Debug);
            GuessState result = GuessState.Submitted;
            await Task.Run((Action)(() =>
            {
                try
                {
                    if (guess.WasRandom)
                    {
                        guess.GuessLocation = BorderHelper.GetRandomPointCloseOrWithinAPolygon();
                    }

                    GuessState? state = mainForm?.ProcessViewerGuess(guess.Player.PlatformId.ToStringDefault(), guess.Player.PlayerName, guess.Player.SourcePlatform, guess.GuessLocation.Latitude.ToStringDefault(), guess.GuessLocation.Longitude.ToStringDefault(), "", guess.Player.DisplayName, guess.Player.ProfilePictureUrl, guess.WasRandom, guess.IsTemporary);
                    result = state.HasValue ? state.Value : GuessState.UndefinedError;

                    connection.SendAsync(nameof(IGeoChatterHub.ReportGuessState), guess.Id, result);
                }
                catch (Exception ex)
                {
                    FireLog(ex.Summarize(), LogLevel.Error);
                }
                result = GuessState.UndefinedError;
            }));
        }
        private async void FlagRequestReceived(Player player, string flag)
        {
            FireLog($"Viewer flag received: {player.PlatformId}, {flag}", LogLevel.Debug);
            await Task.Run(() =>
            {
                mainForm?.ProcessViewerFlag(player.PlatformId, flag, player.PlayerName, player.SourcePlatform, player.DisplayName, player.ProfilePictureUrl);

            });
        }
        private async void ColorRequestReceived(Player player, string color)
        {
            FireLog($"Viewer color received: {player.PlatformId}, {color}", LogLevel.Debug);
            await Task.Run(() =>
            {
                mainForm?.ProcessViewerColor(player.PlatformId, color, player.PlayerName, player.SourcePlatform, player.DisplayName, player.ProfilePictureUrl);
            });
        }

        Stopwatch watch;
        DateTime lastKeepalive;
        System.Threading.Timer timer;
        /// <summary>
        /// Connect given instance to server
        /// </summary>
        /// <param name="apiClient"></param>
        /// <param name="options"></param>
        /// <param name="login"></param>
        /// <param name="reconnect"></param>
        public async Task Connect(ApiClient apiClient, MapOptions options, bool login = true, bool reconnect = false)
        {
            
            if (connection == null )
            {
                FireLog($"Connect called before initialized due to an error", LogLevel.Error);
                FireDisconnected($"Connect to guess server called before initialized due to an error", "Connect to guess server called too early!", LogLevel.Error);

            }
            client = apiClient;
            try
            {
                if (!reconnect || (reconnect && connection.State != HubConnectionState.Connected))
                {
                    lastKeepalive = DateTime.Now;
                    timer = new System.Threading.Timer(async (state) =>
                    {
                        if (connection.State == HubConnectionState.Connected && lastKeepalive.AddMinutes(30) < DateTime.Now)
                            await connection.SendAsync(nameof(IGeoChatterHub.KeepAlive), client);
                        
                    },null, 0, 600000);
                    
                    FireLog("Starting connection...", LogLevel.Debug);
                    await connection.StartAsync();

                }
                if (login)
                {
                 
                    loginResponse = await connection.InvokeAsync<LoginResponse>(nameof(IGeoChatterHub.Login), client);
                    if (loginResponse == null || loginResponse.Result == LoginResult.Failure)
                    {
                        throw new InvalidDataException("Login failed, please restart the client!");
                    }
                    if(loginResponse.Result == LoginResult.OtherClient)
                    {
                        DialogResult res = MessageBox.Show("Another client is already connected using the same GeoGuessr account\n\rPlease make sure to only have one instance of GeoChatter running!\n\rTo close the other clients connection and continue to login click OK!", "User already connected", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2);
                        if (res == DialogResult.OK)
                        {
                            await connection.InvokeAsync<Task>(nameof(IGeoChatterHub.DisconnectClients), GCResourceRequestHandler.ClientUserID);
                            Thread.Sleep(2000);
                            await Connect(apiClient, options, true, true);
                        }
                        else
                        {
                            await connection.DisposeAsync();
                        }
                        return;


                    }
                    MapIdentifier = client.MapIdentifier = loginResponse.MapIdentifier;
                    mainForm?.UpdateMapInTitle();
                    LanguageStrings.SetMapIdentifier(MapIdentifier);
                    FireLog($"Received MapId : {MapIdentifier} for {GCResourceRequestHandler.ClientGeoGuessrName} from login in apiclient.connect", LogLevel.Debug);
                }

                

                    FireLog($"Retrieving troll list", LogLevel.Debug);
                    Trolls = await connection.InvokeAsync<IEnumerable<string>>(nameof(IGeoChatterHub.GetCgTrollIds)) as List<string>;
                    FireLog($"No of Trolls received: {Trolls.Count}", LogLevel.Debug);
                    FireLog($"Retrieving ApiKeys", LogLevel.Debug);
                    Keys = await connection.InvokeAsync<Dictionary<string, string>>(nameof(IGeoChatterHub.GetApiKeys));
                    if (Keys != null)
                    {
                        FireLog($"No of ApiKeysReceived: {Keys.Count}", LogLevel.Debug);
                        foreach (KeyValuePair<string, string> apikey in Keys)
                        {
                            FireLog($"key: '{apikey.Key}', value: '{apikey.Value[..8]}'", LogLevel.Debug);
                        }
                        mainForm?.InitializeGlobalSecrets();
                    }
                    else
                    {
                        FireLog($"could not retrieve apikeys", LogLevel.Debug);
                    }
                    FireLog($"Retrieval successful", LogLevel.Debug);
                    FireLog($"Checking availability of Summary", LogLevel.Debug);

                    SummaryEnabled = await connection.InvokeAsync<bool>(nameof(IGeoChatterHub.IsSummaryEnabled));
                    if (options != null)
                    {
                        FireLog($"Sending MapOptions to server", LogLevel.Debug);
                        options.MapIdentifier = client.MapIdentifier;

                        await connection.SendAsync(nameof(IGeoChatterHub.UpdateMapOptions), options);
                    }
                watch.Stop();
                TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
                string answer = string.Format(CultureInfo.InvariantCulture, "{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds,
                                        t.Milliseconds);
                FireLog("Established GuessServerConnection in " + answer, LogLevel.Debug);
                FireConnected(MapIdentifier,answer);
            }
            catch (Exception ex)
            {
                string msg = ex.Summarize();
                FireLog($"Login failed due to an error: {msg}", LogLevel.Error);
                FireDisconnected($"Login at guess server failed due to an error: {msg}", "Login at guess server failed due to an error", LogLevel.Error);

                return;
            }


        }

      

        /// <summary>
        /// Send <paramref name="guess"/> to server
        /// </summary>
        /// <param name="guess"></param>
        /// <returns></returns>
        public async Task SendGuessToApi(Guess guess)
        {
            try
            {
                    await connection.SendAsync(nameof(IGeoChatterHub.SaveGuess), guess);
                
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }

        /// <summary>
        /// Send <paramref name="game"/> to server to be saved
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public async Task SaveGameToApi(Game game)
        {
            try
            {
                FireLog($"Saving game (ggID: {game?.GeoGuessrId}) to api", LogLevel.Debug);
                    await connection.SendAsync(nameof(IGeoChatterHub.SaveGame), game);
                    FireLog($"Game saved to api successfully", LogLevel.Debug);
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }

        /// <summary>
        /// Report the state of guess with <paramref name="id"/> back to server
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task ReportGuessState(int id, GuessState state)
        {
            try
            {
              
                    await connection.SendAsync(nameof(IGeoChatterHub.ReportGuessState), id, state);
                
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }

        /// <summary>
        /// Send <paramref name="player"/> to server to be saved
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public async Task SavePlayer(Player player)
        {
            try
            {
                    await connection.SendAsync(nameof(IGeoChatterHub.SavePlayer), player);
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }

        /// <summary>
        /// Send <paramref name="player"/> to server to be saved
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public async Task SendMapOptionsToMaps(MapOptions options)
        {
            try
            {

                    if (options != null)
                    {
                        if(string.IsNullOrEmpty(options.MapIdentifier))
                            options.MapIdentifier = MapIdentifier;
                        await connection.SendAsync(nameof(IGeoChatterHub.UpdateMapOptions), options);
                    }
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public async Task Disconnect(bool notifyUser=false, string disconnectMsg="")
        {
            FireLog("Stopping connection...", LogLevel.Debug);
            try
            {
                if (uploadLog)
                {
                    if (!UploadLogViaSignal())
                    {
                        FireLog("Failed to auto upload debug log", LogLevel.Error);
                    }
                    else
                    {
                        FireLog("Uploaded debug log successfully", LogLevel.Information);
                    }
                }
                await connection.InvokeAsync<string>(nameof(IGeoChatterHub.Logoff), client);

                await connection.StopAsync();
                MapIdentifier = "";
                mainForm?.UpdateMapInTitle();
                if(notifyUser)
                    FireDisconnected("User logged in on another client!", disconnectMsg, LogLevel.Information);
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
            
            FireLog("Connection terminated.", LogLevel.Debug);

        }


        /// <summary>
        /// Upload logs via ws
        /// </summary>
        /// <returns></returns>
        public bool UploadLogViaSignal()
        {
            try
            {
                if (client == null
                    || string.IsNullOrEmpty(client.ChannelId) || connection == null
                    )
                {
                    FireLog($"Couldn't update logs via signal. client?.ChannelId: {client?.ChannelId ?? ""}", LogLevel.Error);
                    return false;
                }
                string outputName = $"geochatter-{client.ChannelId}-{DateTime.Now.ToStringDefault("dd-MM-yyyy_HH-mm-ss")}.zip";

                byte[] logContent = ZipHelper.ZipLogFiles(outputName);
                LogFile logFile = new()
                {
                    Data = logContent,
                    Channel = client.ChannelId,
                    FileName = outputName,
                    UploadDate = DateTime.Now
                };
                connection.InvokeAsync<bool>(nameof(IGeoChatterHub.UploadLog), logFile);

                return true;
            }
            catch (Exception ex)
            {
                FireLog($"Failed to update logs via signal. {ex.Summarize()}", LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// <see cref="Initialize(string, IMainForm)"/>
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        public async void Initialize(Uri url, IMainForm form, string clientId, bool debugEnabled)
        {
            await Initialize(url?.OriginalString ?? string.Empty, form, clientId, debugEnabled);
        }

        public async void SendStartGameToMaps(MapGameSettings gameSettings)
        {
            try
            {

                    if (gameSettings != null)
                    {
                        await connection.SendAsync(nameof(IGeoChatterHub.StartGame), gameSettings);
                    }
                
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }
        public async void SendStartRoundToMaps(MapRoundSettings roundSettings)
        {
            try
            {

                    if (roundSettings != null)
                    {
                        await connection.SendAsync(nameof(IGeoChatterHub.StartRound), roundSettings);
                    }
                
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }
        
        public async void SendEndRoundToMaps(List<MapResult> results)
        {
            try
            {

                        await connection.SendAsync(nameof(IGeoChatterHub.EndRound), results);
                
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }
        public async void SendEndGameToMaps(List<MapResult> results)
        {
            try
            {

                        await connection.SendAsync(nameof(IGeoChatterHub.EndGame), results);
                
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }
        public async void SendExitGameToMaps()
        {
            try
            {

                
                
                        await connection.SendAsync(nameof(IGeoChatterHub.ExitGame));
                   
                
            }
            catch (Exception ex)
            {
                FireLog(ex.Summarize(), LogLevel.Error);
            }
        }

        public async Task<string> GetServerLog()
        {
            return await connection.InvokeAsync<string>(nameof(IGeoChatterHub.GetServerLog));
        }
        public async Task<LogFile> GetLogFile(int id)
        {
            return await connection.InvokeAsync<LogFile>(nameof(IGeoChatterHub.GetLogFile), id);
        }
        public async Task<List<LogFile>> GetLogFiles()
        {
            return await connection.InvokeAsync<List<LogFile>>(nameof(IGeoChatterHub.GetLogFiles));
        }
    }

    public class GCTokenResponse
    {
        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string Access_Token;

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public string Scope;

        
        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        public string Token_Type;

        /// <summary>
        /// Gets the expires in.
        /// </summary>
        /// <value>
        /// The expires in.
        /// </value>
        public int Expires_In;
        public string DuelId;
        public string PartyId;
        public ClientVerificationState VerificationResult;

        public override string ToString()
        {
            return $"Access_Token: {Access_Token}, Scope: {Scope}, Duel: {DuelId}, Result: {VerificationResult}";
        }

    }
}
