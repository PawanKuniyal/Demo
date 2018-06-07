using System.Globalization;
using System.Data.Entity.Spatial;

namespace Helpa.Api.Utilities
{
    public static class LocationPoint
    {
        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var point = string.Format(CultureInfo.InvariantCulture.NumberFormat, "POINT({0} {1})", longitude, latitude);
            return DbGeography.PointFromText(point, 4326);
        }
    }
}