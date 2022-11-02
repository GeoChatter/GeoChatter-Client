using GeoChatter.Helpers;
using GeoChatter.Core.Extensions;
using GeoChatter.Core.Helpers;
using GeoChatter.Core.Model;
using GeoChatter.Model;
using GeoChatter.Model.Attributes;
using GeoChatter.Model.Enums;
using GeoChatter.Model.Interfaces;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoChatter.Core.Common.Extensions;
using GeoChatter.Core.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.ServiceModel.Channels;

namespace GeoChatter.Core.Storage
{
    /// <summary>
    /// Client cache for DB
    /// </summary>
    public class ClientDbCache : IClientDbCache
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ClientDbCache));
        private static Game runningGame;
        private static ClientDbContext _gameContext = new();
        private static readonly object _mutex = new();
        private static volatile ClientDbCache _instance;

        /// <summary>
        /// Table options to use
        /// </summary>
        public ITableOptions TableOptions { get; set; }

        /// <inheritdoc/>
        public GameOption NextGameGameOptions { get; set; } = GameOption.DEFAULT;

        /// <inheritdoc/>
        public RoundOption NextRoundRoundOptions { get; set; } = RoundOption.DEFAULT;
        /// <inheritdoc/>
        public string PreferredExportFormat { get; set; } = "xlsx";
        /// <inheritdoc/>
        public bool AlertOnExportSuccess { get; set; } = true;
        /// <inheritdoc/>
        public bool AutoExportGames { get; set; }
        /// <inheritdoc/>
        public bool AutoExportRounds { get; set; }
        /// <inheritdoc/>
        public bool AutoExportStandings { get; set; }
        /// <inheritdoc/>
        public static Game RunningGame
        {
            get => runningGame;
            set
            {
                if (runningGame != null)
                {
                    using ClientDbContext dbContext = new();
                    dbContext.Entry(runningGame).State = EntityState.Detached;
                }

                runningGame = value;

                if (runningGame != null)
                {
                    runningGame.Flags |= Instance.NextGameGameOptions;
                    Instance.NextGameGameOptions = GameOption.DEFAULT;
                }
            }
        }

        /// <inheritdoc/>
        public List<Player> Players { get; set; } = new();

        /// <inheritdoc/>
        public static ClientDbCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_mutex)
                    {
                        _instance = new ClientDbCache();
                        _instance.Initialize();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initialize caches
        /// </summary>
        public void Initialize()
        {
            Players = _gameContext.Players.ToList();
            TableOptions = new TableOptions();
            TableOptions.Load();
            AttributeDiscovery.AddEventHandlers(fromMethodSource: this, toTargetInstance: _gameContext);
        }

        /// <summary>
        /// Validate the game and fix it if neccessary
        /// </summary>
        /// <param name="game"></param>
        public static void ValidateGame(Game game)
        {
            if (game == null)
            {
                return;
            }

            int maxr = 1;
            foreach (Round r in game.Rounds)
            {
                if (r.TimeStamp.Date != DateTime.MinValue.Date)
                {
                    maxr = r.RoundNumber;
                }

                r.Game = game;
                foreach (Guess g in r.Guesses)
                {
                    g.Round = r;
                }
                foreach (RoundResult re in r.Results)
                {
                    re.Round = r;
                }
            }

            if (maxr == game.Rounds.Count)
            {
                maxr = game.IsPartOfInfiniteGame ? -2 : -1;
            }
            game.CurrentRound = maxr;
        }

        /// <inheritdoc/>
        public Game FindGame(string gameId, out GameFoundStatus status)
        {
            status = GameFoundStatus.NOTFOUND;

            if (string.IsNullOrWhiteSpace(gameId))
            {
                return null;
            }

            try
            {
                List<string> paths = _gameContext.GetIncludePaths(typeof(Game)).ToList();

                Game ret = _gameContext.Set<Game>()
                    .Include(paths)
                    .FirstOrDefault(g => g.GeoGuessrId == gameId);

                if (ret != null)
                {
                    if (ret.CurrentRound < -1 && ret.Next == null)
                    {
                        status = GameFoundStatus.FINISHED;
                        return null;
                    }

                    ValidateGame(ret);
                    status = GameFoundStatus.FOUND;
                    logger.Info("Found " + gameId);
                }
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return null;
            }
        }

        /// <summary>
        /// Get player with best average score
        /// </summary>
        /// <returns></returns>
        public Player GetBestAverage()
        {
            Player best = null;
            best = Players.Where(p => !p.IsBanned && p.NoOfGuesses > 1).ToList().MaxBy(p => p.SumOfGuesses / p.NoOfGuesses);
            return best;
        }
        /// <summary>
        /// Get player with best streak
        /// </summary>
        /// <returns></returns>
        public Player GetBestStreak()
        {
            Player best = null;
            best = Players.Where(p => !p.IsBanned).MaxBy(p => p.BestStreak);
            return best;
        }
        /// <summary>
        /// Get player with best round
        /// </summary>
        /// <returns></returns>
        public Player GetBestRound()
        {
            Player best = null;
            best = Players.Where(p => !p.IsBanned).MaxBy(p => p.BestRound);
            return best;
        }
        /// <summary>
        /// Get player with most wins
        /// </summary>
        /// <returns></returns>
        public Player GetBestWins()
        {
            Player best = null;
            best = Players.Where(p => !p.IsBanned).MaxBy(p => p.Wins);
            return best;
        }

        /// <summary>
        /// Get player with most perfect games
        /// </summary>
        /// <returns></returns>
        public Player GetBestPerfects()
        {
            Player best = null;
            best = Players.Where(p => !p.IsBanned).MaxBy(p => p.Perfects);
            return best;
        }
        /// <summary>
        /// Get player with most perfect scores
        /// </summary>
        /// <returns></returns>
        public Player GetBestNoOf5ks()
        {
            Player best = null;
            best = Players.Where(p => !p.IsBanned).MaxBy(p => p.NoOf5kGuesses);
            return best;
        }

        /// <inheritdoc/>
        public Player GetPlayerByTwitchIDOrName(string id, string name = "", string channelName = "")
        {
            Player player = null;
            player = Players.FirstOrDefault(p => (p.PlatformId == id || p.PlayerName == name || p.DisplayName == name) && p.SourcePlatform == Platforms.Twitch);
            if (player == null)
            {
                logger.Info($"Getting ('{name}' [{id}]) from Twitch");
                Player twitchPlayer = TwitchHelper.GetUserDataFromTwitch(id, name).Result;
                if (twitchPlayer != null && !string.IsNullOrEmpty(twitchPlayer.DisplayName))
                {
                    logger.Info($"Found ('{name}' [{id}]) -> ('{twitchPlayer.PlayerName}' [{twitchPlayer.PlatformId}])");
                    player = new Player() { PlatformId = twitchPlayer.PlatformId, Channel = channelName, PlayerName = twitchPlayer.PlayerName, DisplayName = twitchPlayer.DisplayName, ProfilePictureUrl = twitchPlayer.ProfilePictureUrl, SourcePlatform = Platforms.Twitch };
                    Players.Add(player);
                }
                else
                {
                    logger.Warn($"Could not find Twitch player ('{name}' [{id}])");
                }
            }

            return player;
        }

        public Player GetPlayerByIDOrName(string id, string name = "", string displayName = "", string profilePicUrl = "", string channelName = "", bool isStreamer=false, Platforms platform = Platforms.Unknown)
        {
            
            Player player = null;
            if (id == channelName || isStreamer)
                player = GetStreamerById(id, name, profilePicUrl, channelName);
            if(player == null)
                switch (platform)
                {
                    case Platforms.Twitch:
                        player = GetPlayerByTwitchIDOrName(id, name, channelName);
                        break;
                    case Platforms.YouTube:
                        player = GetPlayerByYtIDOrName(id, name, displayName, profilePicUrl, channelName);
                        break;
                    default:
                        player = GetPlayerByTwitchIDOrName(id, name, channelName);
                        if (player == null)
                            player = GetPlayerByYtIDOrName(id, name, displayName, profilePicUrl, channelName);

                        break;
                }

          return player;
        }

        public Player Streamer
        {
            get
            {
                return Players.FirstOrDefault(p => p.PlatformId == p.Channel && p.SourcePlatform == Platforms.GeoGuessr);
            }
        }

        private Player GetStreamerById(string id, string name = "", string profilePicUrl = "", string channelName = "")
        {
            Player player = null;
            player = Players.FirstOrDefault(p => p.PlatformId == id && p.SourcePlatform == Platforms.GeoGuessr);
            if(player == null)
                player = new Player()
                {
                    PlatformId = id,
                    Channel = channelName,
                    PlayerName = name,
                    DisplayName = name,
                    ProfilePictureUrl = profilePicUrl,
                    SourcePlatform = Platforms.GeoGuessr
                };
            Players.Add(player);
            return player;
        }

        public Player GetPlayerByYtIDOrName(string id, string name = "", string displayName = "", string profilePicUrl = "", string channelName = "")
        {
            Player player = null;
            player = Players.FirstOrDefault(p => (p.PlatformId == id || p.PlayerName == name || p.DisplayName == name) && p.SourcePlatform == Platforms.YouTube);
            if (player == null)
            {
                if (!string.IsNullOrEmpty(id) && (!string.IsNullOrEmpty(displayName) || !string.IsNullOrEmpty(name)))
                {
                    logger.Info($"Creating new YT player ('{name}' [{id}])");
                    player = new Player()
                    {
                        PlatformId = id,
                        Channel = channelName,
                        PlayerName = name,
                        DisplayName = displayName ?? name,
                        ProfilePictureUrl = profilePicUrl,
                        SourcePlatform = Platforms.YouTube
                    };
                    Players.Add(player);
                }
            }
            else
            {
                logger.Info($"Found YT player ('{name}' [{id}]) -> ('{player.PlayerName}' [{player.PlatformId}])");


            }

            return player;
        }

        /// <summary>
        /// Save game to DB
        /// </summary>
        /// <param name="game"></param>
        public static async Task<bool> SaveGame(Game game)
        {
            try
            {
                // TODO: Infinite games ending in midgame will have an unplayed round gotten from GG API in their list
                //  remove the extra round or handle otherwise
                if (game == null)
                {
                    logger.Error("Game argument was null, failed to save.");
                    return false;
                }

                if (game.Source.Id != 0)
                {
                    logger.Info($"Updating GGgame in DB: {game.Source.Id} {game.GeoGuessrId}");
                    _gameContext.Update(game.Source);
                }
                else
                {
                    logger.Info($"Adding GGgame to DB: {game.GeoGuessrId}");
                    await _gameContext.AddAsync(game.Source);
                }

                if (game.Id != 0)
                {
                    logger.Info($"Updating game in DB: {game.Id} {game.GeoGuessrId}");
                    _gameContext.Update(game);
                }
                else
                {
                    logger.Info($"Adding game to DB: {game.GeoGuessrId}");
                    await _gameContext.AddAsync(game);
                }

                await _gameContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Summarize());
                return false;
            }
        }

        [DiscoverableEvent]
        private void SavingChanges(object sender, SavingChangesEventArgs e)
        {
            logger.Info($"Saving game context changes.");
        }

        [DiscoverableEvent]
        private void SavedChanges(object sender, SavedChangesEventArgs e)
        {
            logger.Info($"Successfully saved game context {e.EntitiesSavedCount} changes.");
        }

        [DiscoverableEvent]
        private void SaveChangesFailed(object sender, SaveChangesFailedEventArgs e)
        {
            logger.Info($"Failed to save game context changes: {e.Exception?.Summarize()}");
        }

        /// <summary>
        /// Get player by user name
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Player GetByName(string target)
        {
            target = target?.ToLowerInvariant();
            return Players.FirstOrDefault(p => p.PlayerName?.ToLowerInvariant() == target || p.DisplayName?.ToLowerInvariant() == target);
        }

        /// <summary>
        /// Reset country streaks of all players except given list <paramref name="playerList"/>
        /// </summary>
        /// <param name="playerList"></param>
        public void ResetCountryStreaks(List<Player> playerList)
        {
            try
            {
                List<Player> playersToReset = new();
                playersToReset = Players.Where(p => !playerList.Contains(p)).ToList();
                foreach (Player player in playersToReset)
                {
                    player.StreakBefore = 0;
                    player.CountryStreak = 0;
                }

            }
            catch (Exception e)
            {
                logger.Error(e);
            }

        }
    }
}
