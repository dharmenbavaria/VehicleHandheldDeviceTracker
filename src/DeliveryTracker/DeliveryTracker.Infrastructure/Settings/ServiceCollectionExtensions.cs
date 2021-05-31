using System;
using System.Collections.Generic;
using System.Text;
using DeliveryTracker.Lambda.Settings;
using Microsoft.Extensions.DependencyInjection;
using SystemEnvironment = System.Environment;

namespace DeliveryTracker.Environment.Settings
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupEnvironmentSettings(this IServiceCollection serviceCollection)
        {
            var environmentSettings = new EnvironmentSettings
            {
                LookupTableName = GetEnvironmentSetting(nameof(EnvironmentSettings.LookupTableName)),
                DeviceLocationTable = GetEnvironmentSetting(nameof(EnvironmentSettings.DeviceLocationTable)),
                AlertTable = GetEnvironmentSetting(nameof(EnvironmentSettings.AlertTable))
            };
            serviceCollection.AddSingleton<IEnvironmentSettings>(environmentSettings);
        }
        
        private static string GetEnvironmentSetting(string settingName)
        {
            if (settingName == null)
            {
                throw new ArgumentNullException(nameof(settingName));
            }

            var value = SystemEnvironment.GetEnvironmentVariable(settingName);
            return value;
        }


    }
}
