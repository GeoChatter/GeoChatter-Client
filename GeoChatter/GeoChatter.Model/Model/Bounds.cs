using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoChatter.Model
{
    public class Bounds
    {
        public int Id { get; set; }
        public Coordinates Min { get; set; }
        public Coordinates Max { get; set; }
        [NotMapped]
        public double Scale => calculateScale(this);
        public static double calculateScale(Bounds bounds)
        {
            return haversineDistance(bounds.Min, bounds.Max) / 7.458421;
        }

        /**
		 * Returns distance in km between two coordinates
		 * @param {Object} mk1 {lat, lng}
		 * @param {Object} mk2 {lat, lng}
		 * @return {number} km
		 */
        public static double haversineDistance(Coordinates mk1, Coordinates mk2)
        {
            double R = 6371.071;
            double rlat1 = mk1.Latitude * (Math.PI / 180);
            double rlat2 = mk2.Latitude * (Math.PI / 180);
            double difflat = rlat2 - rlat1;
            double difflon = (mk2.Longitude - mk1.Longitude) * (Math.PI / 180);
            double km = 2 * R * Math.Asin(Math.Sqrt(Math.Sin(difflat / 2) * Math.Sin(difflat / 2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Sin(difflon / 2) * Math.Sin(difflon / 2)));
            return km;
        }
    }
}
