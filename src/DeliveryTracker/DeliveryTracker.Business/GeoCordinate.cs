using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTracker.Business
{
    public class GeoCordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public GeoCordinate()
        {
        }

        public GeoCordinate(double lat, double lng)
        {
            this.Latitude = lat;
            this.Longitude = lng;
        }
    }
}
