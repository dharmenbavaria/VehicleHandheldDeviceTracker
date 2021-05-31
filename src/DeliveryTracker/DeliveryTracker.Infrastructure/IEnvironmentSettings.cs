namespace DeliveryTracker.Lambda.Settings
{
    public interface IEnvironmentSettings
    {
        string LookupTableName { get; set; }
        string DeviceLocationTable { get; set; }
        string AlertTable { get; set; }
        string VehicleHandheldDistanceAlert { get; set; }
        string PostNlAlertTopicArn { get; set; }
        int MaxDistanceAllowed { get; set; }
    }
}