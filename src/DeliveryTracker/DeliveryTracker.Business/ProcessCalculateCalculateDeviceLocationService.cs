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
    public class ProcessCalculateCalculateDeviceLocationService : IProcessCalculateDeviceLocationService
    {
        private readonly IDeviceLocationRepository _deviceLocationRepository;
        private readonly ICalculateLocationDistanceService _calculateLocationDistanceService;
        private readonly IDeviceLookupRepository _deviceLookupRepository;
        private readonly IDistanceMoreMessagePublishingClient _distanceMoreMessagePublishingClient;
        private readonly int _maxDistanceAllowed;

        public ProcessCalculateCalculateDeviceLocationService(
            IDeviceLocationRepository deviceLocationRepository,
            ICalculateLocationDistanceService calculateLocationDistanceService,
            IDeviceLookupRepository deviceLookupRepository,
            IDistanceMoreMessagePublishingClient distanceMoreMessagePublishingClient,
            IEnvironmentSettings environmentSettings
            )
        {
            _deviceLocationRepository = deviceLocationRepository;
            _calculateLocationDistanceService = calculateLocationDistanceService;
            _deviceLookupRepository = deviceLookupRepository;
            _distanceMoreMessagePublishingClient = distanceMoreMessagePublishingClient;
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
                    foreach (var linkedDeviceLocation in linkedDevicesLocation)
                    {
                        var distance = _calculateLocationDistanceService.HaversineDistance(
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

                        await _distanceMoreMessagePublishingClient.PublishAsync(alertedVehicleHandheld, cancellationToken);
                    }
                }
            }
        }

    }
}
