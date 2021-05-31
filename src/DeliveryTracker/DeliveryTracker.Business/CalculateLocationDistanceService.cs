using System;
using System.Collections.Generic;
using System.Text;
using DeliveryTracker.Business.Extensions;

namespace DeliveryTracker.Business
{
    public class CalculateLocationDistanceService : ICalculateLocationDistanceService
    {
        public double HaversineDistance(GeoCordinate pos1, GeoCordinate pos2)
        {
            double R = 6371;
            var lat = (pos2.Latitude - pos1.Latitude).ToRadians();
            var lng = (pos2.Longitude - pos1.Longitude).ToRadians();
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                     Math.Cos(pos1.Latitude.ToRadians()) * Math.Cos(pos2.Latitude.ToRadians()) *
                     Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return (R * h2) / 1000;
        }

    }
}
