using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Model
{
    public class Guess
    {
        public int Id { get; set; }

        public double Time { get; set; }
        public Coordinates GuessLocation { get; set; }
        public double Distance { get; set; }
        public bool GuessedBefore { get; set; }
        public bool IsStreamerGuess { get; set; }
        
        public Round Round { get; set; }

        public double Score { get; set; }
        public Player Player { get; set; }
        public DateTime TimeStamp { get; set; }
        public Country Country { get; set; } = new Country("unknown", "__");

        public Country CountryExact { get; set; } = new Country("unknown", "__");
        public int GuessCounter { get; set; }
    }
}
