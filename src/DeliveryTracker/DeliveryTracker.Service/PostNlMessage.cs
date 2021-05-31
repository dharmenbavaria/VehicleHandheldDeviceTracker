using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTracker.Model
{
    public class PostNlMessage
    {
        public string MacId { get; set; }

        public string LinkedDeviceMacId { get; set; }

        public double MacLong { get; set; }

        public double MacLat { get; set; }

        public double LinkedLong { get; set; }

        public double LinkedLat { get; set; }
    }
}
