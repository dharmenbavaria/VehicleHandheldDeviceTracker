using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;

namespace DeliveryTracker.Messaging
{
    public class MessagePublishClientBase 
    {
        private readonly IAmazonSimpleNotificationService _snsClient;

        public MessagePublishClientBase(IAmazonSimpleNotificationService snsClient)
        {
            _snsClient = snsClient;
        }

        public async Task<string> Publish<T>(T message, string topicArn, CancellationToken cancellationToken) where T: class
        {
            if (string.IsNullOrWhiteSpace(topicArn))
            {
                throw new ArgumentNullException($"Topic name not configured");
            }

            if (topicArn == null)
            {
                throw new ArgumentNullException(
                    $"Topic {topicArn} for type {typeof(T)} not found");
            }

            var messageToSend = JsonConvert.SerializeObject(message);

            var request = new PublishRequest(topicArn, messageToSend);

            var response = await _snsClient.PublishAsync(request, cancellationToken);

            return response.MessageId;

        }
    }
}
