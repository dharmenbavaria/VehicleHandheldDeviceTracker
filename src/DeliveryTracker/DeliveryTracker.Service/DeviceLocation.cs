using Amazon.DynamoDBv2.DataModel;

namespace DeliveryTracker.Model
{
    public class DeviceLocation
    {
        public const string Mac = "mac";
        private const string Long = "long";
        private const string Lat = "lat";

        [DynamoDBHashKey]
        [DynamoDBProperty(Mac)]
        public string DeviceMacId { get; set; }

        [DynamoDBProperty(Long)]
        public double Longitude { get; set; }

        [DynamoDBProperty(Lat)]
        public double Latitude { get; set; }
    }
}
