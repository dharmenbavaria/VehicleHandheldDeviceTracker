using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using DeliveryTracker.Lambda.Settings;

namespace DeliveryTracker.Messaging
{
    public class DistanceMoreMessagePublishingClient : MessagePublishClientBase, IDistanceMoreMessagePublishingClient
    {
        private readonly string _distanceMoreTopicArn;

        public DistanceMoreMessagePublishingClient(IAmazonSimpleNotificationService amazonSimpleNotificationService, IEnvironmentSettings environmentSettings)
        : base(amazonSimpleNotificationService)
        {
            _distanceMoreTopicArn = environmentSettings.VehicleHandheldDistanceAlert;
        }

        public async Task<string> PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
        {
            return await Publish(message, _distanceMoreTopicArn, cancellationToken);
        }
    }
}
