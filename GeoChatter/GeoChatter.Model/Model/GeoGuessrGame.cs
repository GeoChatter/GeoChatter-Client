
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoChatter.Model
{
    public class GeoGuessrGame
    {
        public int Id { get; set; }
        public string token { get; set; }
        public string type { get; set; }
        public string mode { get; set; }
        public string state { get; set; }
        public int roundCount { get; set; }
        public int timeLimit { get; set; }
        public bool forbidMoving { get; set; }
        public bool forbidZooming { get; set; }
        public bool forbidRotating { get; set; }
        public string streakType { get; set; }
        public string map { get; set; }
        public string mapName { get; set; }
        public int panoramaProvider { get; set; }
        public GGBounds bounds { get; set; }
        public int round { get; set; }
        public List<GGRound> rounds { get; set; }
        public GGPlayer player { get; set; }
        [NotMapped]
        public object progressChange { get; set; }
    }

    public class GGMin
    {
        public int Id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class GGMax
    {
        public int Id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class GGBounds
    {
        public GGBounds()
        {

        }
        public GGBounds(double minLat, double minLng, double maxLat, double maxLng)
        {
            min = new GGMin() { lat = minLat, lng = minLng };
            max = new GGMax() { lat = maxLat, lng = maxLng };
        }
        public int Id { get; set; }
        public GGMin min { get; set; }
        public GGMax max { get; set; }
        public double Scale => calculateScale(this);
        public static double calculateScale(GGBounds bounds)
        {
            return haversineDistance(bounds.min, bounds.max) / 7.458421;
        }

        /**
		 * Returns distance in km between two coordinates
		 * @param {Object} mk1 {lat, lng}
		 * @param {Object} mk2 {lat, lng}
		 * @return {number} km
		 */
        public static double haversineDistance(GGMin mk1, GGMax mk2)
        {
            double R = 6371.071;
            double rlat1 = mk1.lat * (Math.PI / 180);
            double rlat2 = mk2.lat * (Math.PI / 180);
            double difflat = rlat2 - rlat1;
            double difflon = (mk2.lng - mk1.lng) * (Math.PI / 180);
            double km = 2 * R * Math.Asin(Math.Sqrt(Math.Sin(difflat / 2) * Math.Sin(difflat / 2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Sin(difflon / 2) * Math.Sin(difflon / 2)));
            return km;
        }
    }

    public class GGRound
    {
        public int Id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        [NotMapped]
        public object panoId { get; set; }
        public double heading { get; set; }
        public double pitch { get; set; }
        public int zoom { get; set; }
        [NotMapped]
        public object streakLocationCode { get; set; }
    }

    public class GGTotalScore
    {
        public int Id { get; set; }
        public string amount { get; set; }
        public string unit { get; set; }
        public double percentage { get; set; }
    }

    public class GGMeters
    {
        public int Id { get; set; }
        public string amount { get; set; }
        public string unit { get; set; }
    }

    public class GGMiles
    {
        public int Id { get; set; }
        public string amount { get; set; }
        public string unit { get; set; }
    }

    public class GGTotalDistance
    {
        public int Id { get; set; }
        public GGMeters meters { get; set; }
        public GGMiles miles { get; set; }
    }

    public class GGPin
    {
        public int Id { get; set; }
        public string url { get; set; }
        public string anchor { get; set; }
        public bool isDefault { get; set; }
    }

    public class GGPlayer
    {
        public int GcId { get; set; }
        public GGTotalScore totalScore { get; set; }
        public GGTotalDistance totalDistance { get; set; }
        public double totalDistanceInMeters { get; set; }
        public int totalTime { get; set; }
        public int totalStreak { get; set; }
        [NotMapped]
        public List<object> guesses { get; set; }
        public bool isLeader { get; set; }
        public int currentPosition { get; set; }
        public GGPin pin { get; set; }
        [NotMapped]
        public List<object> newBadges { get; set; }
        [NotMapped]
        public object explorer { get; set; }
        public string id { get; set; }
        public string nick { get; set; }
        public bool isVerified { get; set; }
    }
}