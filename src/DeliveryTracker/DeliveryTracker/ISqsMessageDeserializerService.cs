using Amazon.Lambda.SQSEvents;

namespace DeliveryTracker.Lambda
{
    public interface ISqsMessageDeserializerService<out TModel>
    {
        TModel Deserialize(SQSEvent.SQSMessage message);
    }
}