using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using DeliveryTracker.Business;
using DeliveryTracker.Lambda.Settings;
using DeliveryTracker.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DeliveryTracker.Lambda
{
    public class DeviceLocationFunctionHandler
    {
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();

        protected static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public DeviceLocationFunctionHandler()
        {

        }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            if (ServiceProvider == null)
            {
                ServiceProvider = SetupConfiguration(_serviceCollection);
            }

            foreach (var message in evnt.Records)
            {
                await ProcessMessageAsync(message);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message)
        {
            var service = ServiceProvider.GetService<IProcessCalculateDeviceLocationService>();
            using var scope = ServiceProvider.CreateScope();

            var logger = scope.ServiceProvider.GetService<ILogger<DeviceLocationFunctionHandler>>();
            logger.LogDebug("Handling Sqs record {@ProcessingSnsRecord}", message);
            try
            {
                var deviceLocation = SqsMessageDeserializeService<DeviceLocation>.Instance.Deserialize(message);
                await service.Handle(deviceLocation, CancellationToken.None);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred while handling SNS record {@FailedSnsRecord}", message);
                throw;
            }
        }

        public IServiceProvider SetupConfiguration(IServiceCollection serviceCollection)
        {
            return serviceCollection.SetupDependencies();
        }
    }
}
