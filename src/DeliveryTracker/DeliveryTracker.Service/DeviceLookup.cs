using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace DeliveryTracker.Model
{
    public class DeviceLookup
    {
        private const string LinkedDevice = "ln-dv-mac";

        [DynamoDBHashKey]
        public string Mac { get; set; }

        [DynamoDBRangeKey]
        [DynamoDBProperty(LinkedDevice)]
        public string LinkedDeviceMac { get; set; }
    }
}
