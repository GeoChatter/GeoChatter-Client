using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GeoChatter.Model
{
    public  class Round
    {
        public int Id { get; set; }
        public Coordinates CorrectLocation { get; set; }
        
        private Game Game { get; set; }
        public int RoundNumber { get; set; }
        public ICollection<Guess> Guesses { get; set; } = new List<Guess>();
        [NotMapped]
        private ICollection<RoundResult> results = new List<RoundResult>();
        public ICollection<RoundResult> Results
        {
            get
            {
                if (results == null || !results.Any())
                {
                    //CalculateResults();
                }


                return results;
            }
            set => results = value;
        }
        public Country Country { get; set; }
               
        public Country ExactCountry { get; set; }
        public DateTime TimeStamp { get; set; }

      

      
    }


   
}
