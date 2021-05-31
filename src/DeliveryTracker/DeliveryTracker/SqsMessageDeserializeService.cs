using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json;

namespace DeliveryTracker.Lambda
{
    public class SqsMessageDeserializeService<TModel> : ISqsMessageDeserializerService<TModel>
    {
        static SqsMessageDeserializeService() => Instance = new SqsMessageDeserializeService<TModel>();

        public static ISqsMessageDeserializerService<TModel> Instance { get; }

        public TModel Deserialize(SQSEvent.SQSMessage message)
        {
            dynamic sqsNotification = JsonConvert.DeserializeAnonymousType(message.Body, new { Message = string.Empty });
            var model = JsonConvert.DeserializeObject<TModel>(sqsNotification.Message);
            return model;
        }
    }
}
