using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryTracker.Data.Settings
{
    public static class ServiceCollectionExtension
    {
        public static void SetupRepository(this IServiceCollection serviceCollection)
        {
            var amazonDynamoDbClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig()
            {
                MaxErrorRetry = 3
            });
            var dynamoDbContext = new DynamoDBContext(amazonDynamoDbClient);
            serviceCollection.AddSingleton<IAmazonDynamoDB>(amazonDynamoDbClient);
            serviceCollection.AddSingleton<IDynamoDBContext>(dynamoDbContext);
            serviceCollection.AddTransient<IDeviceLocationRepository, DeviceLocationRepository>();
            serviceCollection.AddTransient<IAlertedVehicleHandheldRepository, AlertedVehicleHandheldRepository>();
            serviceCollection.AddTransient<IDeviceLookupRepository, DeviceLookupRepository>();
        }
    }
}
