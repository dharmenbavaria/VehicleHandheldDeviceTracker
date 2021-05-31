using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using DeliveryTracker.Lambda.Settings;
using DeliveryTracker.Model;

namespace DeliveryTracker.Data
{
    public class AlertedVehicleHandheldRepository : IAlertedVehicleHandheldRepository
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly DynamoDBOperationConfig _dynamoDbOperationConfig;

        public AlertedVehicleHandheldRepository(IDynamoDBContext dynamoDbContext, IEnvironmentSettings environmentSettings)
        {
            if (string.IsNullOrWhiteSpace(environmentSettings.DeviceLocationTable))
            {
                throw new ArgumentNullException("Alert vehicle handheld table name not found");
            }

            _dynamoDbContext = dynamoDbContext;
            _dynamoDbOperationConfig = new DynamoDBOperationConfig()
            {

                ConsistentRead = true,
                OverrideTableName = environmentSettings.AlertTable
            };
        }

        public async Task AddAlertVehicleHandheld(AlertedVehicleHandheld alertedVehicleHandheld)
        {
            await _dynamoDbContext.SaveAsync(alertedVehicleHandheld, _dynamoDbOperationConfig);
        }

        public async Task<AlertedVehicleHandheld> GetAlertVehicleHandheld(string vehicleMacId, string deviceMacId)
        {
            return await _dynamoDbContext.LoadAsync<AlertedVehicleHandheld>(vehicleMacId, deviceMacId, _dynamoDbOperationConfig).ConfigureAwait(false);
        }

        public async Task RemoveAlertVehicleHandheld(AlertedVehicleHandheld alertedVehicleHandheld)
        {
            await _dynamoDbContext.DeleteAsync<AlertedVehicleHandheld>(
                alertedVehicleHandheld.MacId,
                alertedVehicleHandheld.LinkedMacId,
                _dynamoDbOperationConfig,
                    CancellationToken.None);
        }
    }
}
