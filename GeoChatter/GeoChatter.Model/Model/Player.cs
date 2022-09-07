using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Model
{
    public class Player
    {
        public int Id { get; set; }
        [Required]
        public string? Channel { get; set; } = "DUMMY";
        public string? TwitchId { get; set; }
        public string? PlayerName { get; set; }

        public string? PlayerFlag { get; set; }

        public string? PlayerFlagName { get; set; } = string.Empty;

        public int CountryStreak { get; set; }
        public int BestStreak { get; set; }
        public int CorrectCountries { get; set; }
        public int NumberOfCountries { get; set; }
        public int Wins { get; set; }
        public int Perfects { get; set; }
        public double OverallAverage => SumOfGuesses / (NoOfGuesses == 0 ? 1 : NoOfGuesses);

        public int BestGame { get; set; }

        public int BestRound { get; set; }
        public double SumOfGuesses { get; set; }

        public string? LastGuess { get; set; }
        public int NoOfGuesses { get; set; }
        public int NoOf5kGuesses { get; set; }

        public Game? LastGame { get; set; }
        public int RoundNumberOfLastGuess { get; set; }
        [Required]
        public string? DisplayName { get; set; }
        [Required]
        public string? ProfilePictureUrl { get; set; }
        public string? Color { get; set; }

        public bool IsBanned { get; set; }

        public ICollection<Guess>? Guesses { get; set; } = new List<Guess>();
        public int StreakBefore { get; set; }
        public bool FirstGuessMade { get; set; }
        public int IdOfLastGame { get; set; }

        public override string? ToString()
        {
            return !string.IsNullOrEmpty(DisplayName) ? DisplayName + " (" + PlayerName + ")" : PlayerName;
        }
        public static Player Create(string twitchId, string name, string displayName = "", string profilePicUrl = "")
        {
            Player p = new Player()
            {
                PlayerName = name,
                TwitchId = twitchId,
                CountryStreak = 0,
                BestGame = 0,
                BestRound = 0,
                ProfilePictureUrl = profilePicUrl,
                DisplayName = displayName
            };
            return p;

        }
    }
}
