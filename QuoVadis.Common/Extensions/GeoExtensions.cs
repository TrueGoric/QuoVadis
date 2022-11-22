using Geolocation;
using QuoVadis.Common.ValueObjects;

namespace QuoVadis.Common.Extensions
{
    public static class GeoExtensions
    {
        public static double CalculateDistanceToInMeters(this Location from, Location to)
            => GeoCalculator.GetDistance(from.Latitude, from.Longitude, to.Latitude, to.Longitude, 1, DistanceUnit.Meters);
    }
}
