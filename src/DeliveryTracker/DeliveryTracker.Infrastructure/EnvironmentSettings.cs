using DeliveryTracker.Lambda.Settings;

namespace DeliveryTracker.Environment
{
    public class EnvironmentSettings : IEnvironmentSettings
    {
        public string LookupTableName { get; set; }

        public string DeviceLocationTable { get; set; }

        public string AlertTable { get; set; }

        public string VehicleHandheldDistanceAlert { get; set; }

        public string PostNlAlertTopicArn { get; set; }
        public int MaxDistanceAllowed { get; set; }
    }
}
