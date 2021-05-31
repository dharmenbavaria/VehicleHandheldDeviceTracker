using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using DeliveryTracker.Lambda.Settings;
using DeliveryTracker.Model;

namespace DeliveryTracker.Data
{
    public class DeviceLocationRepository : IDeviceLocationRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly DynamoDBOperationConfig _dynamoDbOperationConfig;

        public DeviceLocationRepository(IDynamoDBContext dynamoDbContext, IEnvironmentSettings environmentSettings)
        {
            if (string.IsNullOrWhiteSpace(environmentSettings.DeviceLocationTable))
            {
                throw new ArgumentNullException("Device location table name not found");
            }

            _dynamoDbContext = dynamoDbContext;
            _dynamoDbOperationConfig = new DynamoDBOperationConfig()
            {

                ConsistentRead = true,
                OverrideTableName = environmentSettings.DeviceLocationTable
            };
        }

        public async Task AddDeviceLocation(DeviceLocation deviceLocation)
        {
            await _dynamoDbContext.SaveAsync(deviceLocation, _dynamoDbOperationConfig);
        }

        public async Task<DeviceLocation> GetDeviceLocation(string macId)
        {
            return await _dynamoDbContext.LoadAsync<DeviceLocation>(macId, _dynamoDbOperationConfig);
        }

        public async Task<IReadOnlyCollection<DeviceLocation>> GetDeviceLocations(string[] macIds)
        {
            var deviceLocations = new List<DeviceLocation>();
            var batchGet = _dynamoDbContext.CreateBatchGet<DeviceLocation>(_dynamoDbOperationConfig);

            foreach (var macId in macIds)
            {
                batchGet.AddKey(macId);
            }

            await batchGet.ExecuteAsync();
            deviceLocations.AddRange(batchGet.Results);

            return deviceLocations;
        }
    }
}
