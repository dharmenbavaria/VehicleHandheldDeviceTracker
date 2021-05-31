using System;
using System.Collections.Generic;
using System.Text;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryTracker.Messaging.Settings
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupMessaging(this IServiceCollection collectionExtensions)
        {
            collectionExtensions.AddTransient<IAmazonSimpleNotificationService>(provider =>
                new AmazonSimpleNotificationServiceClient(new AmazonSimpleNotificationServiceConfig()
                {
                    MaxErrorRetry = 3
                }));
            collectionExtensions
                .AddTransient<IDistanceAlertMessagePublishingClient, DistanceAlertMessagePublishingClient>();
            collectionExtensions
                .AddTransient<IDistanceAlertMessagePublishingClient, DistanceAlertMessagePublishingClient>();
        }
    }
}
