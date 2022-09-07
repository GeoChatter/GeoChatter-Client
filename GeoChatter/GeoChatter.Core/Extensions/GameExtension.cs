using GeoChatter.Extensions;
using GeoChatter.Helpers;
using GeoChatter.Model.Enums;
using GeoChatter.Core.Model;
using GeoChatter.Core.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GeoChatter.Model;
using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Core.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Game"/>
    /// </summary>
    public static class GameExtension
    {

        /// <summary>
        /// Ends the current round, or the game if the current round is the last one
        /// </summary>
        /// <param name="game"></param>
        /// <param name="GGgame"></param>
        /// <param name="path"></param>
        public static void EndCurrentRound([NotNull] this Game game, GeoGuessrGame GGgame, string path = "")
        {
            GCUtils.ThrowIfNull(GGgame, nameof(GGgame));

            Round currentGGRound = game.Rounds.First(r => r.RoundNumber == game.CurrentRound);

            if (game.Mode == GameMode.STREAK)
            {
                if (GGgame.roundCount == game.CurrentRound)
                {
                    currentGGRound.CalculateResults();
                    game.CalculateResults();
                    game.WriteGameResultLabels(path);
                    game.CurrentRound = -1;
                }
                else
                {
                    game.CurrentRound++;
                    currentGGRound.CalculateResults();
                    game.WriteRoundResultLabels(currentGGRound, path);
                    Round nextGGRound = RoundExtensions.Create(game, game.CurrentRound);
                    GGRound round = GGgame.rounds[^1];
                    nextGGRound.CorrectLocation = new Coordinates(round.lat, round.lng);
                    nextGGRound.TimeStamp = DateTime.Now;
                    game.Rounds.Add(nextGGRound);

                }
            }
            else
            {
                if (game.CurrentRound == game.Rounds.Count)
                {
                    currentGGRound.CalculateResults();
                    game.CalculateResults();
                    game.WriteGameResultLabels(path);
                    game.CurrentRound = game.IsPartOfInfiniteGame ? -2 : -1;
                }
                else
                {
                    game.CurrentRound++;
                    currentGGRound.CalculateResults();
                    game.WriteRoundResultLabels(currentGGRound, path);
                    Round nextGGRound = game.Rounds.First(r => r.RoundNumber == game.CurrentRound);
                    GGRound round = GGgame.rounds[^1];
                    nextGGRound.CorrectLocation = new Coordinates(round.lat, round.lng);
                    nextGGRound.TimeStamp = DateTime.Now;

                }
            }
            game.Players.ForEach(p => p.StreakBefore = 0);
            game.Players.ForEach(p => p.FirstGuessMade = false);

            //FileName = GameRepository.SaveGame(this);
        }

        /// <summary>
        /// Write game result labels to <paramref name="path"/>
        /// </summary>
        /// <param name="game"></param>
        /// <param name="path"></param>
        public static void WriteGameResultLabels([NotNull] this Game game, string path)
        {
            Dictionary<int, string> scores = new();
            int place = 1;
            foreach (GameResult item in game.Results.BuildOrderBy(game.GetTableOptions().GetDefaultFiltersFor(game.Mode, GameStage.ENDGAME)))
            {
                Player player = item.Player;
                double score = item.Score;
                scores.TryAdd(place, player.PlayerName + "," + score);
                place++;
            }
            if (scores.ContainsKey(1) && game.LabelSettings[LabelType.GameWinner])
            {
                LabelStorage.WriteLabel(LabelType.GameWinner, scores[1].ToString(), path);
            }

            if (scores.ContainsKey(2) && game.LabelSettings[LabelType.GameSecond])
            {
                LabelStorage.WriteLabel(LabelType.GameSecond, scores[2].ToString(), path);
            }

            if (scores.ContainsKey(3) && game.LabelSettings[LabelType.GameThird])
            {
                LabelStorage.WriteLabel(LabelType.GameThird, scores[3].ToString(), path);
            }

            string resultString = string.Empty;
            scores.ForEach(s => resultString += s.Value + ";");
            if (resultString.Length > 0 && game.LabelSettings[LabelType.GameHighScore])
            {
                resultString = resultString.TrimEnd(';');

                LabelStorage.WriteLabel(LabelType.GameHighScore, resultString, path);
            }
        }

        private static void WriteRoundResultLabels([NotNull] this Game game, Round currentGGRound, string path)
        {
            Dictionary<int, string> scores = new();
            int place = 1;
            foreach (RoundResult item in currentGGRound.Results
                .BuildOrderBy(game.GetTableOptions().GetDefaultFiltersFor(game.Mode, GameStage.ENDROUND)))
            {
                Player player = game.Players.FirstOrDefault(p => p.PlatformId == item.Player.PlatformId && p.SourcePlatform == item.Player.SourcePlatform);
                double score = item.Score;
                scores.TryAdd(place, player?.PlayerName + "," + score);
                place++;
            }

            if (scores.ContainsKey(1) && game.LabelSettings[LabelType.RoundWinner])
            {
                LabelStorage.WriteLabel(LabelType.RoundWinner, scores[1].ToString(), path);
            }

            if (scores.ContainsKey(2) && game.LabelSettings[LabelType.RoundSecond])
            {
                LabelStorage.WriteLabel(LabelType.RoundSecond, scores[2].ToString(), path);
            }

            if (scores.ContainsKey(3) && game.LabelSettings[LabelType.RoundThird])
            {
                LabelStorage.WriteLabel(LabelType.RoundThird, scores[3].ToString(), path);
            }

            string resultString = string.Empty;
            scores.OrderBy(s => s.Key).ForEach(s => resultString += s.Value + ";");
            if (resultString.Length > 0 && game.LabelSettings[LabelType.RoundHighScore])
            {
                resultString = resultString.TrimEnd(';');
                LabelStorage.WriteLabel(LabelType.RoundHighScore, resultString, path);
            }
        }
    }
}
