using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Extensions;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Core.Storage;
using GeoChatter.Extensions;
using GeoChatter.Forms.ScoreCalculator;
using GeoChatter.Helpers;
using GeoChatter.Model;
using GeoChatter.Model.Attributes;
using GeoChatter.Model.Enums;
using GeoChatter.Properties;
using GeoChatter.Web;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeoChatter.Forms
{
    public partial class MainForm
    {



        private Dictionary<Tuple<string, Platforms>, Tuple<string, string>> _temporaryGuessesStorage = new();
        /// <summary>
        /// Serializes the guess and sends it to the scoreboard
        /// </summary>
        /// <param name="guess"></param>
        private void SendGuessToJS(Guess guess)
        {
            logger.Debug("SendGuessToJS");
            browser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(JsToCsHelper.GetSendGuessObjToJsScript(guess));
        }
        /// <inheritdoc/>
        public GuessState ProcessViewerGuess(string userId, string userName, Platforms userPlatform, string latString, string lngString, string color, string displayName, Uri profilePicUrl, bool wasRandom, bool isTemporary)
        {
            return ProcessViewerGuess(userId, userName, userPlatform, latString, lngString, color, displayName, profilePicUrl?.OriginalString ?? string.Empty, wasRandom, isTemporary);
        }

#if DEBUG
        [DiscoverableEvent]
        private void RandomBotGuessRecieved(object sender, RandomBotGuessRecievedEventArgs args)
        {
            SendRandomGuess(args.Count, args.Reuse);
        }
#endif

        /// <inheritdoc/>
        public GuessState ProcessViewerGuess(string userId, string userName, Platforms userPlatform, string latString, string lngString, string color = "", string displayName = "", string profilePicUrl = "", bool wasRandom = false, bool isTemporary = false)
        {
            try
            {
                if (ClientDbCache.RunningGame == null || !guessesOpen)
                {
                    logger.Debug("Guess received, but no running game or guesses are closed");
                    return GuessState.NoGame;
                }
                if (isTemporary)
                {
                    logger.Debug($"Temp guess received from {userName} ({userId})");
                    if (ClientDbCache.RunningGame.GetCurrentRound().Guesses.FirstOrDefault(p => p.Player.PlatformId == userId) == null)
                    {
                        if (!_temporaryGuessesStorage.ContainsKey(new(userId, userPlatform)))
                        {
                            logger.Debug("Added temp guess to the list");
                            _temporaryGuessesStorage.Add(new(userId, userPlatform), new Tuple<string, string>(latString, lngString));
                        }
                        else
                        {
                            logger.Debug("updated temp guess in the list");
                            _temporaryGuessesStorage[new(userId, userPlatform)] = new Tuple<string, string>(latString, lngString);
                        }

                    }
                    else
                    {
                        logger.Debug("player has guessed regularly");

                        if (_temporaryGuessesStorage.ContainsKey(new(userId, userPlatform)))
                        {
                            logger.Debug("Removing prev temp guesses from the list");
                            _temporaryGuessesStorage.Remove(new(userId, userPlatform));
                        }
                    }
                    return GuessState.TempSuccess;
                }
                else
                {
                    logger.Debug($"Regular guess received from {userName} ({userId})");

                    if (_temporaryGuessesStorage.ContainsKey(new(userId, userPlatform)))
                    {
                        logger.Debug("Removing prev temp guesses from the list");
                        _temporaryGuessesStorage.Remove(new(userId, userPlatform));
                    }
                }




                logger.Debug("Guess received");
                Game game = ClientDbCache.RunningGame;
                //if(string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userId))
                //    bot?.

                Player player = ClientDbCache.RunningGame.Players.FirstOrDefault(p => p.PlatformId == userId && p.SourcePlatform == userPlatform);

                player = ClientDbCache.Instance.GetPlayerByIDOrName(userId, userName, displayName, profilePicUrl, GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), platform: userPlatform);

                if (player == null)
                {
                    return GuessState.NotFound;
                }

                player.Update(userName, displayName, profilePicUrl, color);
                if (player.IsBanned || (Settings.Default.AutoBanCGTrolls && guessApiClient.Trolls.Contains(player.PlatformId)))
                {
                    player.IsBanned = true;
                    logger.Debug($"Player {player.FullDisplayName} is banned. Ignoring guess.");
                    return GuessState.Banned;
                }

                logger.Debug("Player loaded: " + player.PlatformId);
                bool multi = false;

                Guess existant = game.GetCurrentRound().Guesses.FirstOrDefault(g => g != null && g.Player != null && g.Player.PlatformId == player.PlatformId);
                if (existant != null)
                {
                    if (!game.GetCurrentRound().IsMultiGuess)
                    {
                        logger.Debug("Multiple guess denied");
                        if (Settings.Default.SendDoubleGuessMsg)
                        {
                            CooledDownMessage(5,
                                LanguageStrings.Get("Chat_Msg_doubleGuessReveived", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }),
                                userId,
                                CurrentBot,
                                lastMultiGuessMessage);
                        }
                        return GuessState.GuessedAlready;
                    }
                    else if (DateTime.Now - existant.TimeStamp <= TimeSpan.FromSeconds(10))
                    {
                        logger.Debug("Multiple guess too fast");
                        if (Settings.Default.SendDoubleGuessTooFastMsg)
                        {
                            CooledDownMessage(5,
                                LanguageStrings.Get("Chat_Msg_doubleGuessTooFast", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }),
                                userId,
                                CurrentBot,
                                lastMultiGuessMessage);
                        }
                        return GuessState.TooFast;
                    }
                    logger.Debug("multiple guess received");
                    multi = true;

                }

                // Create the guess
                bool validCoordinates = GCUtils.ValidateAndFixCoordinates(latString, lngString, out double lat, out double lng);

                logger.Debug("coordinates validated");

                if (!validCoordinates)
                {
                    return GuessState.InvalidCoordinates;
                }

                logger.Debug("coordinates valid");

                string guess = lat + " " + lng;
                string lastguesstmp = player.LastGuess;

                if (!Settings.Default.AllowSameLocationGuess && guess == player.LastGuess)
                {
                    if (Settings.Default.SendSameGuessMessage)
                    {
                        CooledDownMessage(5,
                            LanguageStrings.Get("Chat_Msg_sameGuessReveived", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }),
                            userId,
                            CurrentBot,
                            lastSameGuessMessage);
                    }

                    return GuessState.SameCoordinates;
                }
                else
                {
                    lastguesstmp = player.LastGuess;
                    player.LastGuess = guess;
                }

                Country country = CountryHelper.GetCountry(lat.ToStringDefault(),
                                                           lng.ToStringDefault(),
                                                           Settings.Default.UseEnglishCountryNames,
                                                           out Country exactcountry);

                if (existant != null)
                {
                    existant.GuessLocation.Latitude = lat;
                    existant.GuessLocation.Longitude = lng;
                    existant.Country.Code = country.Code;
                    existant.Country.Name = country.Name;
                    existant.CountryExact.Code = exactcountry.Code;
                    existant.CountryExact.Name = exactcountry.Name;
                    existant.GuessedBefore = true;
                    existant.WasRandom = wasRandom;
                    existant.TimeStamp = DateTime.Now;
                    existant.GuessCounter++;
                }
                else
                {
                    existant = new()
                    {
                        Player = player,
                        GuessLocation = new Coordinates(lat, lng),
                        Country = country,
                        CountryExact = exactcountry,
                        TimeStamp = DateTime.Now,
                        GuessedBefore = false,
                        GuessCounter = 1,
                        WasRandom = wasRandom
                    };
                }
                Round round = game.Rounds.First(r => r.RoundNumber == game.CurrentRound);

                if (game.Mode == GameMode.DEFAULT)
                {
                    SetCountryStreak(player, existant, round);
                }

                round
                    .AddGuess(
                        existant,
                        ScoreFormulatorExpression.Create,
                        ScoreFormulatorVariable.GetAsDouble,
                        ScoreFormulator.LastCompiledScript,
                        game.Rounds.Where(r => r.RoundNumber < ClientDbCache.RunningGame.CurrentRound).ToList()
                    );

                TriggerSpecialScoreActions(player, existant.Score);
                TriggerSpecialDistanceActions(player, existant.Distance);
                logger.Debug("Actions executed");

                SendGuessToJS(existant);

                logger.Debug("Sent guess to JS");

                UpdatePlayerAndGuessInCache(player, existant);

                if (Settings.Default.SendConfirmGuessMsg && Settings.Default.EnableTwitchChatMsgs)
                {
                    CurrentBot?.SendMessage(LanguageStrings.Get("Chat_Msg_guessReveived", new Dictionary<string, string>() { { "playerName", player.FullDisplayName } }));
                }

                logger.Debug("Player updated");



                return GuessState.Success;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return GuessState.UndefinedError;
            }
        }

        private static void SetCountryStreak(Player player, Guess guess, Round round)
        {
            if (!player.FirstGuessMade)
            {
                player.FirstGuessMade = true;
                player.StreakBefore = player.CountryStreak;
            }


            Country country = guess.Country;
            //Country exactcountry = guess.CountryExact;

            string correctCountry = ClientDbCache.RunningGame.Source.streakType == Game.USStreaksGame ? round.ExactCountry.Code : round.Country.Code;
            if (country.Code == correctCountry)
            {
                player.CountryStreak = player.StreakBefore + 1;
                guess.Streak = player.CountryStreak;
                player.CorrectCountries++;
                if (player.BestStreak < player.CountryStreak)
                {
                    player.BestStreak = player.CountryStreak;
                }

            }
            else if (country.Code != correctCountry)
            {
                player.CountryStreak = 0;
            }
            player.NumberOfCountries++;
            //playerRep.SavePlayer(player);
        }   
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public void SetStreamerLastStreaksGameGuess(double lat, double lng)
        {
            streakGameStreamerCoords = new(lat, lng);
        }

        private Coordinates streakGameStreamerCoords { get; set; }

        private void GetStreamerGuessData(GeoGuessrGame game, out double lat, out double lng, out double dist)
        {
            lat = 0D;
            lng = 0D;
            dist = 0D;

            try
            {
                int guessIndex = game.player.guesses.Count - 1;

                GGGuess guess = game.player.guesses[guessIndex];

                if (streakGameStreamerCoords == null && ClientDbCache.RunningGame.Mode != GameMode.STREAK)
                {
                    lat = guess.lat;
                    lng = guess.lng;
                }
                else if (streakGameStreamerCoords != null)
                {
                    lat = streakGameStreamerCoords.Latitude;
                    lng = streakGameStreamerCoords.Longitude;
                    streakGameStreamerCoords = null;
                }

                dist = guess.distanceInMeters;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
        }

        /// <inheritdoc/>
        public async void ProcessStreamerGuess(GeoGuessrGame game, bool wasRandom)
        {
            try
            {
                if (ClientDbCache.RunningGame != null && game != null && ClientDbCache.RunningGame.GeoGuessrId == game.token)
                {

                    ProcessTemporaryGuesses();

                    int nextIndex = game.rounds.Count - 1;

                    GetStreamerGuessData(game, out double lat, out double lng, out double dist);

                    Player streamer = ClientDbCache.Instance.Streamer;

                    if (streamer == null)
                    {
                        streamer = ClientDbCache.Instance.GetPlayerByIDOrName(GCResourceRequestHandler.ClientUserID, GCResourceRequestHandler.ClientGeoGuessrName, profilePicUrl: GCResourceRequestHandler.ClientGeoGuessrPic, channelName: GCResourceRequestHandler.ClientUserID.ToLowerInvariant(), isStreamer: true);
                        if (streamer == null)
                        {
                            MessageBox.Show("Failed to create streamer player. Please restart the app.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            logger.Error("Failed to find streamer player");
                            return;
                        }
                    }

                    Guess streamerGuess = new()
                    {
                        Distance = dist / 1000d,
                        GuessLocation = new Coordinates(lat, lng),
                        Player = streamer,
                        Score = 0,//(long)guess["roundScoreInPoints"],
                        IsStreamerGuess = true,
                        GuessCounter = 1,
                        TimeStamp = DateTime.Now,
                        WasRandom = wasRandom
                    };
                    Round finishedRound = ClientDbCache.RunningGame.GetCurrentRound();
                    if (finishedRound == null)
                    {
                        finishedRound = ClientDbCache.RunningGame.Rounds.FirstOrDefault(r => r.RoundNumber == ClientDbCache.RunningGame.Rounds.Count);
                    }

                    Country country = CountryHelper.GetCountry(lat.ToStringDefault(),
                                                               lng.ToStringDefault(),
                                                               Settings.Default.UseEnglishCountryNames,
                                                               out Country exactcountry);

                    streamerGuess.Country = country;
                    streamerGuess.CountryExact = exactcountry;

                    finishedRound.AddGuess(streamerGuess,
                                           ScoreFormulatorExpression.Create,
                                           ScoreFormulatorVariable.GetAsDouble,
                                           ScoreFormulator.LastCompiledScript,
                                           ClientDbCache.RunningGame.Rounds.Where(r => r.RoundNumber < ClientDbCache.RunningGame.CurrentRound).ToList());

                    if (!ClientDbCache.RunningGame.Players.Any(p => p.PlatformId == streamer.PlatformId))
                    {
                        ClientDbCache.RunningGame.Players.Add(streamer);
                    }
                    if (Settings.Default.ResetStreakOnSkippedRound)
                    {
                        ClientDbCache.Instance.ResetCountryStreaks(finishedRound.Guesses.Select(g => g.Player).ToList());
                    }

                    SendGuessToJS(streamerGuess);

                    streamer.IdOfLastGame = ClientDbCache.RunningGame.Id;
                    streamer.RoundNumberOfLastGuess = ClientDbCache.RunningGame.CurrentRound;
                    if (streamer.BestRound < streamerGuess.Score)
                    {
                        streamer.BestRound = streamerGuess.Score;
                    }
                    if (streamerGuess.Score == ScoreFormulatorExpression.PerfectScoreStatic)
                    {
                        streamer.NoOf5kGuesses++;
                    }
                    if (Settings.Default.CheckStreamerGuessForSpecialValues)
                    {
                        TriggerSpecialDistanceActions(streamer, streamerGuess.Distance);
                        TriggerSpecialScoreActions(streamer, streamerGuess.Score);
                    }
                    streamer.NoOfGuesses++;
                    streamer.SumOfGuesses += streamerGuess.Score;
                    streamer.LastGuess = streamerGuess.GuessLocation.Latitude + " " + streamerGuess.GuessLocation.Longitude;

                    bool status = await FinalizeStreamerGuess(game, finishedRound);
                    if (!status)
                    {
                        MessageBox.Show("Something went wrong while finalizing client guess. App restart may be required to prevent any further issues with the game.", "Failed To Finalize Client Guess", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
            }
            _temporaryGuessesStorage?.Clear();
        }

        private void ProcessTemporaryGuesses()
        {
            logger.Debug($"Processing {_temporaryGuessesStorage.Count} temp guesses");
            _temporaryGuessesStorage.ForEach(p =>
            {
                logger.Debug($"Processing temp guess from {p.Key.Item1} ({p.Key.Item2})");
                ProcessViewerGuess(p.Key.Item1, string.Empty, p.Key.Item2, p.Value.Item1, p.Value.Item2);
            });
        }

        private async Task<bool> FinalizeStreamerGuess(GeoGuessrGame game, Round finishedRound)
        {
            try
            {
                Round round = ClientDbCache.RunningGame.GetCurrentRound();
                if (round != null)
                {

                    ClientDbCache.RunningGame.Players.ForEach(p =>
                    {
                        Guess guess = round.Guesses.FirstOrDefault(g => g.Player.PlatformId == p.PlatformId);
                        if (guess != null)
                        {
                            if (p.BestRound < guess.Score)
                            {
                                p.BestRound = guess.Score;
                            }
                            if (guess.Score == ScoreFormulatorExpression.PerfectScoreStatic)
                            {
                                p.NoOf5kGuesses++;
                            }

                            p.NoOfGuesses++;
                            p.SumOfGuesses += guess.Score;
                            p.TotalDistance += guess.Distance;
                            if (ClientDbCache.RunningGame.Mode == GameMode.STREAK || p.PlatformId == GCResourceRequestHandler.ClientUserID)
                            {
                                SetCountryStreak(p, guess, round);
                            }
                        }

                    });
                }

                ClientDbCache.RunningGame.EndCurrentRound(game, LabelPath);

                if (ClientDbCache.RunningGame.CurrentRound > -1)
                {
                    guessesOpen = false;
                    SetRefreshMenuItemsEnabledState(false);
                    SendEndRoundToJS(finishedRound);
                    SendEndRoundToMaps(finishedRound);

                    TriggerRoundEndActions();
                    Round newRound = ClientDbCache.RunningGame.GetCurrentRound();
                    if (newRound != null)
                    {
                        SetCountry(newRound);
                    }
                }
                else if (ClientDbCache.RunningGame.CurrentRound > -2)
                {
                    guessesOpen = false;
                    SendEndGameToJS(ClientDbCache.RunningGame);
                    SendEndGameToMaps(ClientDbCache.RunningGame);
                    SetRefreshMenuItemsEnabledState(false);
                    FinalizeGame();
                }
                else
                {
                    guessesOpen = false;
                    SendEndRoundMessage(round);

                    FinalizeGame();
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return false;
            }
        }
        /// <inheritdoc/>
        public void SetPanoIdForGuessOf(int round, string playerName, string panoid)
        {
            if (ClientDbCache.RunningGame != null && !string.IsNullOrEmpty(playerName) && !string.IsNullOrEmpty(panoid))
            {
                Round r = ClientDbCache.RunningGame.Rounds.FirstOrDefault(r => r.RoundNumber == round);
                if (r == null)
                {
                    return;
                }

                Guess g = r.Guesses.FirstOrDefault(g => g.Player.FullDisplayName == playerName);
                if (g == null)
                {
                    return;
                }
                g.Pano = panoid;
            }
        }

        private GCTaskScheduler RandomBotGuessScheduler { get; set; }

        private List<int> randomUsers = new();
        private RestClient randomBotImageClient = new(new RestClientOptions() { FollowRedirects = false });

        private Task SendRandomBotGuessAsync(CancellationTokenSource tkn, string id, bool requestFlag = false)
        {
            return Task.Factory.StartNew(() =>
            {
                if (requestFlag)
                {
                    FlagRequestReceived(null, new(id, null, CurrentBot, null) { Flag = "random" });
                }

                Coordinates rand = BorderHelper.GetRandomPointCloseOrWithinAPolygon();
                string lat = rand.Latitude.ToStringDefault();
                string lng = rand.Longitude.ToStringDefault();

                //RestRequest req = new("https://picsum.photos/120/120?random=" + random.Next())
                //{
                //    Method = Method.Get,
                //};

                //RestResponse res = randomBotImageClient.Execute(req);
                //if (res.StatusCode == System.Net.HttpStatusCode.Redirect)
                //{
                //res.Headers.FirstOrDefault(h => h.Name == "Location")?.Value.ToStringDefault()
                ProcessViewerGuess(id, null, Platforms.Twitch, lat, lng, string.Empty, string.Empty, string.Empty, wasRandom: true);
                //}
            }, tkn.Token, TaskCreationOptions.PreferFairness, RandomBotGuessScheduler);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="reuse"></param>
        public void SendRandomGuess(int amount = 30, bool reuse = false)
        {
            using CancellationTokenSource tkn = new();
            RandomBotGuessScheduler = new GCTaskScheduler(amount + 1);
            List<Task> lis = new(amount);
            if (!reuse)
            {
                for (int i = 0; i < amount; i++)
                {
                    int idInt = random.Next(10000);
                    randomUsers.Add(idInt);
                    SendRandomBotGuessAsync(tkn, idInt.ToStringDefault(), true);
                }
            }
            else
            {
                randomUsers.ForEach(r => lis.Add(SendRandomBotGuessAsync(tkn, r.ToStringDefault())));
            }

            Task.WaitAll(lis.ToArray());
        }

    }
}
