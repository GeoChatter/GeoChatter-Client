using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Model
{
    public class Coordinates
    {
        public Coordinates()
        {

        }
        public Coordinates(double lat, double log)
        {
            Latitude = lat;
            Longitude = log;
        }
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
