using System;
using System.IO;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using Snappet.StructuredLogging;
using DeliveryTracker.Messaging.Settings;
using DeliveryTracker.Environment.Settings;
using DeliveryTracker.Data.Settings;
using SystemEnvironment = System.Environment;

namespace DeliveryTracker.Lambda.Settings
{
    public static class ServiceCollectionExtension
    {
        private static string? _runningEnvironment;

        public static IServiceProvider SetupDependencies(this IServiceCollection serviceCollection)
        {
            SetupLogging(serviceCollection);
            serviceCollection.SetupRepository();
            serviceCollection.SetupMessaging();
            serviceCollection.SetupEnvironmentSettings();
            serviceCollection.SetupMessaging();
            return serviceCollection.BuildServiceProvider();
        }

        
        public static string RunningEnvironment
        {
            get
            {
                if (_runningEnvironment == null)
                {
                    var env = SystemEnvironment.GetEnvironmentVariable("LAMBDA_ENVIRONMENT");
                    if (string.IsNullOrWhiteSpace(env) || env.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    {
                        env = "Development";
                    }
                    env = env.Trim();
                    _runningEnvironment = env;
                }

                return _runningEnvironment;
            }
        }
        public static bool IsRunningOnDevelopment => "Development".Equals(RunningEnvironment);

        private static void SetupLogging(IServiceCollection serviceCollection)
        {
            serviceCollection.AddStructuredConsoleLogging(
                typeof(ServiceCollectionExtension).Assembly.GetName().Name,
                SystemEnvironment.GetEnvironmentVariable("LAMBDA_ENVIRONMENT"),
                null,
                !IsRunningOnDevelopment
            );
        }
       
    }
}
