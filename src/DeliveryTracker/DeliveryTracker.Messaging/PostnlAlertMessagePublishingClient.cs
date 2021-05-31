using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using DeliveryTracker.Lambda.Settings;

namespace DeliveryTracker.Messaging
{
    public class PostnlAlertMessagePublishingClient : MessagePublishClientBase, IPostnlAlertMessagePublishingClient
    {
        private readonly string _postNlAlertTopicArn;

        public PostnlAlertMessagePublishingClient(IAmazonSimpleNotificationService snsClient, IEnvironmentSettings environmentSettings) : base(snsClient)
        {
            _postNlAlertTopicArn = environmentSettings.PostNlAlertTopicArn;
        }

        public async Task<string> PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
        {
            return await Publish(message, _postNlAlertTopicArn, cancellationToken);
        }
    }
}
