using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Model
{
    public class GameResult
    {
        public GameResult()
        {

        }
       public int Id { get; set; }
        public int GuessCount { get; set; }
        public Player Player { get; set; }
        public Game Game { get; set; }
        public double Distance { get; set; }
        public double Score { get; set; }
        public double Time { get; set; }
    }
}
