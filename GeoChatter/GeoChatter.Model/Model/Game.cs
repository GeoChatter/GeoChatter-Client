using GeoChatter.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Model
{
    public enum GameMode
    {
        /// <summary>
        /// Default 5 round game
        /// </summary>
        DEFAULT,
        /// <summary>
        /// Streak mode
        /// </summary>
        STREAK
    }
  
    public class Game
    {
        public Game()
        {
            Players = new List<Player>();
            Rounds = new List<Round>();
            Results = new List<GameResult>();
        }
        public int Id { get; set; }
        public string Channel { get; set; } = "DUMMY";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? GeoGuessrId { get; set; }
        public bool IsUsStreak { get; set; } = false;
        [NotMapped]
        public Dictionary<LabelType, bool> LabelSettings = new Dictionary<LabelType, bool>();
        public Bounds? Bounds { get; set; }
        public GameMode Mode { get; set; } = GameMode.DEFAULT; 
        [NotMapped]
        public int CurrentRound { get; set; }
        public ICollection<Player> Players { get; set; }
        public ICollection<Round> Rounds { get; set; }
        public ICollection<GameResult> Results { get; set; }
        [NotMapped]
        public GeoGuessrGame Source { get; set; }

        
    }
}
