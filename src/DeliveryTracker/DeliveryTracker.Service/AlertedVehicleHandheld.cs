using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace DeliveryTracker.Model
{
    public class AlertedVehicleHandheld
    {
        public const string mac = "mac";
        public const string Lmac = "lmac";
        public const string IsMore = "more";
        public const string mlong = "mlong";
        public const string mlat = "mlat";
        public const string llong = "llong";
        public const string llat = "llat";

        [DynamoDBHashKey]
        [DynamoDBProperty(mac)]
        public string MacId { get; set; }

        [DynamoDBRangeKey]
        [DynamoDBProperty(Lmac)]
        public string LinkedMacId { get; set; }

        [DynamoDBProperty(IsMore)]
        public bool IsMoreThanAllowedDistance { get; set; }

        [DynamoDBProperty(mlong)]
        public double MacLong { get; set; }

        [DynamoDBProperty(mlat)]
        public double MacLat { get; set; }

        [DynamoDBProperty(llong)]
        public double LinkMacLong { get; set; }

        [DynamoDBProperty(llat)]
        public double LinkMacLat { get; set; }

    }
}
