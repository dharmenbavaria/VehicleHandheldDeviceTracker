using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using DeliveryTracker.Lambda.Settings;
using DeliveryTracker.Model;

namespace DeliveryTracker.Data
{
    public class DeviceLookupRepository : IDeviceLookupRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly DynamoDBOperationConfig _dynamoDbOperationConfig;

        public DeviceLookupRepository(IDynamoDBContext dynamoDbContext, IEnvironmentSettings environmentSettings)
        {
            if (string.IsNullOrWhiteSpace(environmentSettings.DeviceLocationTable))
            {
                throw new ArgumentNullException("Device lookup table name not found");
            }

            _dynamoDbContext = dynamoDbContext;
            _dynamoDbOperationConfig = new DynamoDBOperationConfig()
            {

                ConsistentRead = true,
                OverrideTableName = environmentSettings.LookupTableName
            };
        }
        public async Task<IReadOnlyCollection<DeviceLookup>> GetDeviceLinked(string macId)
        {
            var asyncOp = _dynamoDbContext.QueryAsync<DeviceLookup>(macId, _dynamoDbOperationConfig);
            return await asyncOp.GetRemainingAsync();
        }
    }
}
