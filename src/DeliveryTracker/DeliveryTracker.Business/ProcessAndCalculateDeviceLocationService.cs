using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracker.Data;
using DeliveryTracker.Lambda.Settings;
using DeliveryTracker.Messaging;
using DeliveryTracker.Model;

namespace DeliveryTracker.Business
{
    public class ProcessAndCalculateDeviceLocationService : IProcessCalculateDeviceLocationService
    {
        private readonly IDeviceLocationRepository _deviceLocationRepository;
        private readonly ICalculateTwoLocationDistanceService _calculateTwoLocationDistanceService;
        private readonly IDeviceLookupRepository _deviceLookupRepository;
        private readonly IDistanceAlertMessagePublishingClient _distanceAlertMessagePublishingClient;
        private readonly int _maxDistanceAllowed;

        public ProcessAndCalculateDeviceLocationService(
            IDeviceLocationRepository deviceLocationRepository,
            ICalculateTwoLocationDistanceService calculateTwoLocationDistanceService,
            IDeviceLookupRepository deviceLookupRepository,
            IDistanceAlertMessagePublishingClient distanceAlertMessagePublishingClient,
            IEnvironmentSettings environmentSettings
            )
        {
            _deviceLocationRepository = deviceLocationRepository;
            _calculateTwoLocationDistanceService = calculateTwoLocationDistanceService;
            _deviceLookupRepository = deviceLookupRepository;
            _distanceAlertMessagePublishingClient = distanceAlertMessagePublishingClient;
            _maxDistanceAllowed = environmentSettings.MaxDistanceAllowed;
        }

        public async Task Handle(DeviceLocation deviceLocation, CancellationToken cancellationToken)
        {
            await _deviceLocationRepository.AddDeviceLocation(deviceLocation);

            var devicesLinked = await _deviceLookupRepository.GetDeviceLinked(deviceLocation.DeviceMacId);

            if (devicesLinked != null && devicesLinked.Any())
            {
                var macIds = devicesLinked.Select(p => p.LinkedDeviceMac).ToArray();

                var linkedDevicesLocation = await _deviceLocationRepository.GetDeviceLocations(macIds);

                if (linkedDevicesLocation != null && linkedDevicesLocation.Any())
                {
                    var tasks = linkedDevicesLocation.Select(p =>
                        ProcessAndPublish(deviceLocation, cancellationToken, p));

                    await Task.WhenAll(tasks);
                }
            }
        }

        private async Task ProcessAndPublish(DeviceLocation deviceLocation, CancellationToken cancellationToken,
            DeviceLocation linkedDeviceLocation)
        {
            var distance = _calculateTwoLocationDistanceService.HaversineDistance(
                new GeoCordinate(linkedDeviceLocation.Latitude, linkedDeviceLocation.Longitude),
                new GeoCordinate(deviceLocation.Latitude, deviceLocation.Longitude));
            var alertedVehicleHandheld = new AlertedVehicleHandheld
            {
                LinkedMacId = linkedDeviceLocation.DeviceMacId,
                MacId = deviceLocation.DeviceMacId,
                IsMoreThanAllowedDistance = distance > _maxDistanceAllowed,
                MacLat = deviceLocation.Latitude,
                MacLong = deviceLocation.Longitude,
                LinkMacLat = linkedDeviceLocation.Latitude,
                LinkMacLong = linkedDeviceLocation.Longitude
            };

            await _distanceAlertMessagePublishingClient.PublishAsync(alertedVehicleHandheld, cancellationToken);
        }
    }
}
